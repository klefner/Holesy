param(
  [string]$RequestText = '',
  [switch]$VerifyLive,
  [switch]$WriteSnapshot,
  [switch]$SkipFetch
)

$ErrorActionPreference = 'Stop'

$scriptPath = Split-Path -Parent $MyInvocation.MyCommand.Path
$repoRoot = Resolve-Path (Join-Path $scriptPath '..\..')
Set-Location $repoRoot

$sourceMasterPath = '10_SOURCE/Masters/Master 16/index.html'
$releasePackagePath = '40_RELEASE/Website_Publish_Package/holesy/index.html'
$godaddyUploadRoot = 'C:\Users\KentLefner\Downloads\holesy-godaddy-upload-master-16.52\holesy'
$godaddyUploadPath = Join-Path $godaddyUploadRoot 'index.html'
$godaddyDeltaUploadRoot = 'C:\Users\KentLefner\Downloads\holesy-godaddy-delta-master-16.52-from-16.51\holesy'
$automationPath = 'C:\Users\KentLefner\.codex\automations\daily-qa-audit\automation.toml'
$auditorPromptPath = '00_ADMIN/Policies_and_Procedures/AUDITOR_AUTOMATION_PROMPT.md'
$issueLogPath = '00_ADMIN/Reviews_and_Reports/ISSUE_LOG.md'
$backlogPath = '00_ADMIN/Requirements/PRODUCT_BACKLOG.md'

$script:ConfidenceWarnings = New-Object System.Collections.Generic.List[string]
$script:ConfidenceFailures = New-Object System.Collections.Generic.List[string]

function Section($title) {
  ''
  "## $title"
  ''
}

function Add-Warning($message) {
  $script:ConfidenceWarnings.Add($message)
}

function Add-Failure($message) {
  $script:ConfidenceFailures.Add($message)
}

function Add-Collection($target, $items) {
  foreach ($item in @($items)) {
    $target.Add([string]$item)
  }
}

function Invoke-Git {
  param(
    [Parameter(ValueFromRemainingArguments = $true)]
    [string[]]$Args
  )

  & git -c "safe.directory=$($repoRoot.Path)" @Args
}

function Read-IfExists($path, $maxLines = 80) {
  if (Test-Path -LiteralPath $path) {
    Get-Content -LiteralPath $path -TotalCount $maxLines
  } else {
    Add-Failure "Missing required file: $path"
    "MISSING: $path"
  }
}

function Get-RelativePath($path) {
  $resolved = Resolve-Path -LiteralPath $path -ErrorAction SilentlyContinue
  if (-not $resolved) { return $path }
  $root = $repoRoot.Path.TrimEnd('\')
  if ($resolved.Path.StartsWith($root, [System.StringComparison]::OrdinalIgnoreCase)) {
    return $resolved.Path.Substring($root.Length).TrimStart('\')
  }
  return $resolved.Path
}

function Get-FileFact($path) {
  $exists = Test-Path -LiteralPath $path
  if (-not $exists) {
    return [pscustomobject]@{
      Path = $path
      Exists = $false
      Sha256 = 'missing'
      BuildLabel = 'missing'
      Length = 0
    }
  }

  $hash = (Get-FileHash -Algorithm SHA256 -LiteralPath $path).Hash.ToLowerInvariant()
  $item = Get-Item -LiteralPath $path
  $buildLabel = 'not found'
  $match = Select-String -LiteralPath $path -Pattern 'Master\s+\d+(?:\.\d+)?' -AllMatches |
    Select-Object -First 1
  if ($match -and $match.Matches.Count -gt 0) {
    $buildLabel = $match.Matches[0].Value
  } else {
    Add-Warning "No build label found in $path"
  }

  [pscustomobject]@{
    Path = $path
    Exists = $true
    Sha256 = $hash
    BuildLabel = $buildLabel
    Length = $item.Length
  }
}

function Format-FileFact($fact) {
  if (-not $fact.Exists) {
    return '- `' + $fact.Path + '`: MISSING'
  }
  return '- `' + $fact.Path + '`: exists, bytes=' + $fact.Length + ', build=`' + $fact.BuildLabel + '`, sha256=' + $fact.Sha256
}

function Get-CurrentRecommendation {
  if (-not (Test-Path -LiteralPath $backlogPath)) { return @('MISSING: PRODUCT_BACKLOG.md') }
  $lines = Get-Content -LiteralPath $backlogPath
  $start = [Array]::FindIndex($lines, [Predicate[string]]{ param($line) $line -eq '## Current Recommendation' })
  if ($start -lt 0) {
    Add-Warning 'Current Recommendation heading not found in product backlog.'
    return @('Current Recommendation heading not found')
  }
  return $lines[$start..([Math]::Min($lines.Count - 1, $start + 35))]
}

function Get-StartedBacklogItems {
  if (-not (Test-Path -LiteralPath $backlogPath)) { return @('MISSING: PRODUCT_BACKLOG.md') }
  $lines = Get-Content -LiteralPath $backlogPath
  $items = New-Object System.Collections.Generic.List[string]
  for ($i = 0; $i -lt $lines.Count; $i++) {
    if ($lines[$i] -match '^(#{3,4})\s+(.+)$') {
      $heading = $Matches[2].Trim()
      $windowEnd = [Math]::Min($lines.Count - 1, $i + 12)
      $window = $lines[$i..$windowEnd] -join "`n"
      if ($window -match '(?im)^\s*-\s*in progress|^\s*-\s*paused|^\s*-\s*monitor|^\s*-\s*deferred') {
        $status = ($Matches[0] -replace '^\s*-\s*','').Trim()
        $items.Add("- $heading | status signal: $status")
      }
    }
  }
  if ($items.Count -eq 0) { return @('No started/not-complete backlog headings detected by status scan.') }
  return $items
}

function Get-PriorityOneItems {
  if (-not (Test-Path -LiteralPath $backlogPath)) { return @('MISSING: PRODUCT_BACKLOG.md') }
  $lines = Get-Content -LiteralPath $backlogPath
  $start = [Array]::FindIndex($lines, [Predicate[string]]{ param($line) $line -like '## Priority 1*' })
  if ($start -lt 0) { return @('Priority 1 heading not found.') }
  $end = $lines.Count - 1
  for ($i = $start + 1; $i -lt $lines.Count; $i++) {
    if ($lines[$i] -match '^## Priority [2-9]') { $end = $i - 1; break }
  }
  $section = $lines[$start..$end]
  $section | Where-Object { $_ -match '^###\s+' } | ForEach-Object { "- " + ($_ -replace '^###\s+', '') }
}

function Get-LatestHandoff {
  $handoffs = Get-ChildItem -LiteralPath '00_ADMIN/Policies_and_Procedures' -Filter 'NEXT_CODEX_CHAT_HANDOFF_MASTER*.md' |
    Sort-Object {
      if ($_.BaseName -match 'MASTER(\d+)') { [int]$Matches[1] } else { 0 }
    } -Descending
  if ($handoffs.Count -eq 0) { return $null }
  return $handoffs[0]
}

function Get-GitFactLines {
  $lines = New-Object System.Collections.Generic.List[string]

  if (-not $SkipFetch) {
    try {
      $fetchOutput = Invoke-Git fetch --prune 2>&1
      if ($LASTEXITCODE -ne 0) {
        Add-Warning "git fetch --prune returned exit code $LASTEXITCODE."
        $lines.Add("Fetch: WARNING exit=$LASTEXITCODE")
        Add-Collection $lines $fetchOutput
      } else {
        $lines.Add('Fetch: ok (`git fetch --prune`)')
      }
    } catch {
      Add-Warning "git fetch --prune failed: $($_.Exception.Message)"
      $lines.Add("Fetch: FAILED - $($_.Exception.Message)")
    }
  } else {
    Add-Warning 'Remote freshness not checked because -SkipFetch was used.'
    $lines.Add('Fetch: skipped by -SkipFetch')
  }

  $status = Invoke-Git status --short --branch
  $branch = (Invoke-Git branch --show-current).Trim()
  $upstream = (Invoke-Git rev-parse --abbrev-ref --symbolic-full-name '@{u}' 2>$null)
  $head = (Invoke-Git rev-parse HEAD).Trim()
  $main = (Invoke-Git rev-parse main).Trim()

  $lines.Add('')
  $lines.Add('Status:')
  Add-Collection $lines $status
  $lines.Add('')
  $lines.Add("Active branch: $branch")
  $lines.Add("HEAD: $head")

  if ($upstream) {
    $upstreamSha = (Invoke-Git rev-parse $upstream).Trim()
    $aheadBehind = (Invoke-Git rev-list --left-right --count "$upstream...HEAD").Trim()
    $parts = $aheadBehind -split '\s+'
    $behind = [int]$parts[0]
    $ahead = [int]$parts[1]
    $lines.Add("Upstream: $upstream ($upstreamSha)")
    $lines.Add("Ahead/behind active upstream: ahead=$ahead behind=$behind")
    if ($ahead -ne 0 -or $behind -ne 0) {
      Add-Warning "Active branch is not aligned with upstream: ahead=$ahead behind=$behind."
    }
  } else {
    Add-Warning 'Active branch has no upstream configured.'
    $lines.Add('Upstream: none')
  }

  $mainCompare = (Invoke-Git rev-list --left-right --count "main...HEAD").Trim()
  $mainParts = $mainCompare -split '\s+'
  $headBehindMain = [int]$mainParts[0]
  $headAheadMain = [int]$mainParts[1]
  $lines.Add("main: $main")
  $lines.Add("HEAD versus main: ahead=$headAheadMain behind=$headBehindMain")
  if ($headAheadMain -gt 0 -or $headBehindMain -gt 0) {
    $lines.Add('main currency: stale or divergent relative to active branch')
  } else {
    $lines.Add('main currency: aligned with active branch')
  }

  $lines.Add('')
  $lines.Add('Latest commits:')
  Add-Collection $lines (Invoke-Git log --oneline --decorate -8)
  $lines.Add('')
  $lines.Add('Branches:')
  Add-Collection $lines (Invoke-Git branch -vv)

  return $lines
}

function Get-IssueFacts {
  if (-not (Test-Path -LiteralPath $issueLogPath)) {
    Add-Failure "Missing issue log: $issueLogPath"
    return @('MISSING: ISSUE_LOG.md')
  }

  $content = Get-Content -LiteralPath $issueLogPath
  $qaRows = $content | Where-Object { $_ -match '^\| QA-' }
  $open = @($qaRows | Where-Object { $_ -match '\| open \|' })
  $monitor = @($qaRows | Where-Object { $_ -match '\| monitor \|' })
  $resolved = @($qaRows | Where-Object { $_ -match '\| resolved \|' })

  $lines = New-Object System.Collections.Generic.List[string]
  $lines.Add("Counts: open=$($open.Count), monitor=$($monitor.Count), resolved=$($resolved.Count)")
  if ($open.Count -gt 0) {
    Add-Warning "Issue log has $($open.Count) open QA item(s)."
  }
  if (($open.Count + $monitor.Count) -gt 0) {
    $lines.Add('')
    $lines.Add('Open/monitor rows:')
    Add-Collection $lines ($open + $monitor)
  } else {
    $lines.Add('No open or monitor QA items found.')
  }
  return $lines
}

function Get-GovernanceInventory {
  $expected = @(
    'AGENTS.md',
    '00_ADMIN/Policies_and_Procedures/QA_REVIEW_STANDARD.md',
    '00_ADMIN/Policies_and_Procedures/QA_CHAT_RISK_AND_CONTROL_MATRIX.md',
    '00_ADMIN/Policies_and_Procedures/RISK_AND_CONTROLS_POLICY.md',
    '00_ADMIN/Reviews_and_Reports/AUDIT_WORKPLAN_UNIFIED_QA_AND_RELEASE_CONTROLS.md',
    '00_ADMIN/Policies_and_Procedures/GITHUB_OPERATING_MODEL.md',
    '00_ADMIN/Policies_and_Procedures/STANDALONE_WEBSITE_PUBLISH_WORKFLOW.md',
    '00_ADMIN/Policies_and_Procedures/RELEASE_SOURCE_OF_TRUTH_MANIFEST.md',
    '00_ADMIN/Policies_and_Procedures/PRODUCT_INTENT_GATE.md',
    '00_ADMIN/Policies_and_Procedures/NEW_CHAT_TEAM_SYNC_PROTOCOL.md',
    '00_ADMIN/Policies_and_Procedures/AUDITOR_AUTOMATION_PROMPT.md',
    '00_ADMIN/Requirements/ARCHITECTURE_DECISION_MODULAR_CLIENT_SPLIT.md',
    '00_ADMIN/Reviews_and_Reports/ARCHITECTURE_ALIGNMENT_REVIEW_MASTER16_16_MODULAR_PACKAGE.md',
    '00_ADMIN/Requirements/PRODUCT_BACKLOG.md',
    '00_ADMIN/Reviews_and_Reports/ISSUE_LOG.md',
    '10_SOURCE/Current/CURRENT_BASIS.md'
  )

  $handoff = Get-LatestHandoff
  if ($handoff) { $expected += (Get-RelativePath $handoff.FullName) }

  $lines = New-Object System.Collections.Generic.List[string]
  foreach ($path in $expected) {
    if (Test-Path -LiteralPath $path) {
      $item = Get-Item -LiteralPath $path
      $modified = $item.LastWriteTime.ToString('yyyy-MM-dd HH:mm:ss')
      $lines.Add("- OK: ``$path`` (modified $modified)")
    } else {
      Add-Failure "Missing expected governance document: $path"
      $lines.Add("- MISSING: ``$path``")
    }
  }

  $recent = Get-ChildItem -LiteralPath '00_ADMIN' -Recurse -File -Include '*.md' |
    Sort-Object LastWriteTime -Descending |
    Select-Object -First 10
  $lines.Add('')
  $lines.Add('Recently changed governance/admin docs:')
  foreach ($item in $recent) {
    $relative = Get-RelativePath $item.FullName
    $modified = $item.LastWriteTime.ToString('yyyy-MM-dd HH:mm:ss')
    $lines.Add("- $relative ($modified)")
  }
  return $lines
}

function Get-AutomationDriftLines {
  $lines = New-Object System.Collections.Generic.List[string]
  if (-not (Test-Path -LiteralPath $automationPath)) {
    Add-Warning "Automation file not found: $automationPath"
    return @("Automation TOML missing: $automationPath")
  }
  if (-not (Test-Path -LiteralPath $auditorPromptPath)) {
    Add-Failure "Auditor prompt governance file missing: $auditorPromptPath"
    return @("Auditor prompt governance file missing: $auditorPromptPath")
  }

  $toml = Get-Content -Raw -LiteralPath $automationPath
  $requiredPhrases = @(
    'study the current process and procedure governance corpus every time',
    'QA review standard',
    'release source-of-truth manifest',
    'Product Intent Gate',
    'new-chat team sync protocol',
    'issue log',
    'newest dated daily-audit report',
    'missing-run or skipped-run note'
  )
  $missing = @()
  foreach ($phrase in $requiredPhrases) {
    if ($toml -notmatch [regex]::Escape($phrase)) { $missing += $phrase }
  }

  $lines.Add("Automation file: $automationPath")
  $lines.Add("Automation prompt intent file: $auditorPromptPath")
  $status = Select-String -LiteralPath $automationPath -Pattern 'status\s*=\s*"([^"]+)"' | Select-Object -First 1
  if ($status) { $lines.Add("Automation status line: $($status.Line.Trim())") }

  if ($missing.Count -eq 0) {
    $lines.Add('Automation drift check: OK - actual automation prompt contains required governed intent markers.')
  } else {
    Add-Warning "Automation drift check missing required prompt marker(s): $($missing -join ', ')"
    $lines.Add("Automation drift check: WARNING - missing markers: $($missing -join ', ')")
  }
  return $lines
}

function Get-LiveVerificationLines {
  if (-not $VerifyLive) {
    Add-Warning 'Live site not checked because -VerifyLive was not used.'
    return @('Live verification: not checked. Use -VerifyLive when production/live state matters.')
  }

  $url = "https://ptbooksinc.com/holesy/?v=$([DateTimeOffset]::UtcNow.ToUnixTimeSeconds())"
  $webClient = New-Object System.Net.WebClient
  try {
    $webClient.Headers.Add('Cache-Control', 'no-cache')
    $bytes = $webClient.DownloadData($url)
    $content = [System.Text.Encoding]::UTF8.GetString($bytes)
    $sha = [System.Security.Cryptography.SHA256]::Create()
    try {
      $hashBytes = $sha.ComputeHash($bytes)
      $hash = ([System.BitConverter]::ToString($hashBytes)).Replace('-', '').ToLowerInvariant()
    } finally {
      $sha.Dispose()
    }
    $build = 'not found'
    $match = [regex]::Match($content, 'Master\s+\d+(?:\.\d+)?')
    if ($match.Success) { $build = $match.Value } else { Add-Warning 'No build label found in live-site HTML.' }

    $lines = New-Object System.Collections.Generic.List[string]
    $lines.Add("Live verification URL: $url")
    $lines.Add('HTTP status: 200')
    $lines.Add("Live build label: $build")
    $lines.Add("Live content sha256: $hash")
    $lines.Add("Live bytes: $($bytes.Length)")

    if (Test-Path -LiteralPath $releasePackagePath) {
      $releaseHash = (Get-FileHash -Algorithm SHA256 -LiteralPath $releasePackagePath).Hash.ToLowerInvariant()
      $releaseBuild = (Get-FileFact $releasePackagePath).BuildLabel
      if ($hash -eq $releaseHash) {
        $lines.Add('Live/release hash comparison: OK')
      } else {
        Add-Warning 'Live site hash differs from the local release package hash.'
        $lines.Add("Live/release hash comparison: WARNING - live=$hash release=$releaseHash")
      }
      if ($build -eq $releaseBuild) {
        $lines.Add('Live/release build label comparison: OK')
      } else {
        Add-Warning 'Live site build label differs from the local release package build label.'
        $lines.Add("Live/release build label comparison: WARNING - live=$build release=$releaseBuild")
      }
    } else {
      Add-Warning 'Release package missing; live site cannot be compared against local release artifact.'
      $lines.Add('Live/release comparison: skipped because local release package is missing.')
    }

    return $lines
  } catch {
    Add-Warning "Live verification failed: $($_.Exception.Message)"
    return @("Live verification failed: $($_.Exception.Message)")
  } finally {
    $webClient.Dispose()
  }
}

function Get-ProductIntentAssessment {
  $lines = New-Object System.Collections.Generic.List[string]
  if ([string]::IsNullOrWhiteSpace($RequestText)) {
    Add-Warning 'No -RequestText supplied; Product Intent Gate can only provide baseline conflict rules.'
    $lines.Add('Request text: not supplied.')
    $lines.Add('Gate result: baseline only. New assistant must still assess the user request before acting.')
    return $lines
  }

  $lines.Add("Request text: $RequestText")
  $lower = $RequestText.ToLowerInvariant()
  $conflicts = New-Object System.Collections.Generic.List[string]

  if ($lower -match 'publish|package|upload|godaddy|production|prod') {
    $conflicts.Add('Release/package request detected: modular browser-client split is a committed architecture constraint; index.html is only the package entry point, the full modular /holesy/ package must be preserved as baseline, and routine GoDaddy upload packages should be changed-files-only deltas when live is already on the previous master.')
  }
  if ($lower -match 'modular|architecture|split|js|css|asset') {
    $conflicts.Add('Architecture request detected: must preserve accepted modular browser-client split, read the Master 16.16 to Master 16.17 alignment review and Master 16.18 startup hotfix, and route follow-on package work through PERF-012.')
  }
  if ($lower -match 'defect|bug|fix|gameplay|feature|build|implement') {
    $conflicts.Add('Implementation request detected: must check issue-log monitors and current backlog recommendation before coding.')
  }

  if ($conflicts.Count -eq 0) {
    $lines.Add('Gate result: no obvious conflict keywords detected; still preserve manifest, backlog, issue log, and architecture decision.')
  } else {
    $lines.Add('Gate result: action allowed only if these controls are addressed before work:')
    foreach ($conflict in $conflicts) { $lines.Add("- $conflict") }
  }
  return $lines
}

$snapshot = New-Object System.Collections.Generic.List[string]

function Add-Lines($lines) {
  foreach ($line in @($lines)) {
    $snapshot.Add([string]$line)
  }
}

Add-Lines '# Holesy New Chat Context Snapshot'
Add-Lines ''
Add-Lines "Generated: $(Get-Date -Format 'yyyy-MM-dd HH:mm:ss zzz')"
Add-Lines "Repo root: $($repoRoot.Path)"
Add-Lines "Request text supplied: $([bool](-not [string]::IsNullOrWhiteSpace($RequestText)))"
Add-Lines "Live verification requested: $([bool]$VerifyLive)"

Add-Lines (Section 'Git State And Remote Freshness')
Add-Lines '```text'
Add-Lines (Get-GitFactLines)
Add-Lines '```'

Add-Lines (Section 'Source Package Integrity')
$sourceFact = Get-FileFact $sourceMasterPath
$releaseFact = Get-FileFact $releasePackagePath
$uploadFact = Get-FileFact $godaddyUploadPath
Add-Lines (Format-FileFact $sourceFact)
Add-Lines (Format-FileFact $releaseFact)
Add-Lines (Format-FileFact $uploadFact)
if ($sourceFact.Exists -and $releaseFact.Exists) {
  if ($sourceFact.Sha256 -eq $releaseFact.Sha256) {
    Add-Lines '- Source/release hash comparison: OK'
  } else {
    Add-Warning 'Source master and release package hashes differ.'
    Add-Lines '- Source/release hash comparison: WARNING - hashes differ'
  }
  if ($sourceFact.BuildLabel -eq $releaseFact.BuildLabel) {
    Add-Lines '- Source/release build label comparison: OK'
  } else {
    Add-Warning 'Source master and release package build labels differ.'
    Add-Lines '- Source/release build label comparison: WARNING - labels differ'
  }
}
if ($releaseFact.Exists -and $uploadFact.Exists) {
  if ($releaseFact.Sha256 -eq $uploadFact.Sha256) {
    Add-Lines '- Release/upload hash comparison: OK'
  } else {
    Add-Warning 'Release package and GoDaddy upload copy hashes differ.'
    Add-Lines '- Release/upload hash comparison: WARNING - hashes differ'
  }
}
$modularAssets = @(
  @{ Name = 'how-to-play.html'; Source = '10_SOURCE/Masters/Master 16/how-to-play.html'; Release = '40_RELEASE/Website_Publish_Package/holesy/how-to-play.html'; Upload = (Join-Path $godaddyUploadRoot 'how-to-play.html') },
  @{ Name = 'css/styles.css'; Source = '10_SOURCE/Masters/Master 16/css/styles.css'; Release = '40_RELEASE/Website_Publish_Package/holesy/css/styles.css'; Upload = (Join-Path $godaddyUploadRoot 'css\styles.css') },
  @{ Name = 'js/main.js'; Source = '10_SOURCE/Masters/Master 16/js/main.js'; Release = '40_RELEASE/Website_Publish_Package/holesy/js/main.js'; Upload = (Join-Path $godaddyUploadRoot 'js\main.js') },
  @{ Name = 'js/build-info.js'; Source = '10_SOURCE/Masters/Master 16/js/build-info.js'; Release = '40_RELEASE/Website_Publish_Package/holesy/js/build-info.js'; Upload = (Join-Path $godaddyUploadRoot 'js\build-info.js') },
  @{ Name = 'js/difficulty-profiles.js'; Source = '10_SOURCE/Masters/Master 16/js/difficulty-profiles.js'; Release = '40_RELEASE/Website_Publish_Package/holesy/js/difficulty-profiles.js'; Upload = (Join-Path $godaddyUploadRoot 'js\difficulty-profiles.js') },
  @{ Name = 'data/lore-documents.js'; Source = '10_SOURCE/Masters/Master 16/data/lore-documents.js'; Release = '40_RELEASE/Website_Publish_Package/holesy/data/lore-documents.js'; Upload = (Join-Path $godaddyUploadRoot 'data\lore-documents.js') },
  @{ Name = 'assets/images/how-to-play-game-summary.svg'; Source = '10_SOURCE/Masters/Master 16/assets/images/how-to-play-game-summary.svg'; Release = '40_RELEASE/Website_Publish_Package/holesy/assets/images/how-to-play-game-summary.svg'; Upload = (Join-Path $godaddyUploadRoot 'assets\images\how-to-play-game-summary.svg') }
)
foreach ($asset in $modularAssets) {
  $sourceAssetFact = Get-FileFact $asset.Source
  $releaseAssetFact = Get-FileFact $asset.Release
  $uploadAssetFact = Get-FileFact $asset.Upload
  if (-not ($sourceAssetFact.Exists -and $releaseAssetFact.Exists -and $uploadAssetFact.Exists)) {
    Add-Warning ("Modular asset missing from source/release/upload package: " + $asset.Name)
    Add-Lines ('- Modular asset `' + $asset.Name + '`: WARNING - missing from at least one package layer')
  } elseif ($sourceAssetFact.Sha256 -eq $releaseAssetFact.Sha256 -and $releaseAssetFact.Sha256 -eq $uploadAssetFact.Sha256) {
    Add-Lines ('- Modular asset `' + $asset.Name + '` hash comparison: OK')
  } else {
    Add-Warning ("Modular asset hash mismatch across source/release/upload package: " + $asset.Name)
    Add-Lines ('- Modular asset `' + $asset.Name + '` hash comparison: WARNING - hashes differ')
  }
}
Add-Lines '- GoDaddy manual upload default: changed-files-only delta package when live is already on the previous approved master.'
if (Test-Path -LiteralPath $godaddyDeltaUploadRoot) {
  Add-Lines "- Current GoDaddy delta package root: $godaddyDeltaUploadRoot"
  $deltaFiles = Get-ChildItem -LiteralPath $godaddyDeltaUploadRoot -Recurse -File | ForEach-Object {
    $_.FullName.Substring($godaddyDeltaUploadRoot.Length).TrimStart('\')
  }
  foreach ($deltaFile in $deltaFiles) {
    Add-Lines ("  - " + $deltaFile)
  }
} else {
  Add-Warning 'Current GoDaddy delta package folder is missing.'
  Add-Lines "- Current GoDaddy delta package root missing: $godaddyDeltaUploadRoot"
}

Add-Lines (Section 'Live Site Verification')
Add-Lines (Get-LiveVerificationLines)

Add-Lines (Section 'Automation Drift Check')
Add-Lines (Get-AutomationDriftLines)

Add-Lines (Section 'Governance Corpus Inventory')
Add-Lines (Get-GovernanceInventory)

Add-Lines (Section 'Release Source Of Truth Manifest')
Add-Lines (Read-IfExists '00_ADMIN/Policies_and_Procedures/RELEASE_SOURCE_OF_TRUTH_MANIFEST.md' 140)

Add-Lines (Section 'Product Intent Gate')
Add-Lines (Read-IfExists '00_ADMIN/Policies_and_Procedures/PRODUCT_INTENT_GATE.md' 140)

Add-Lines (Section 'Request-Aware Product Intent Assessment')
Add-Lines (Get-ProductIntentAssessment)

Add-Lines (Section 'Current Basis')
Add-Lines (Read-IfExists '10_SOURCE/Current/CURRENT_BASIS.md' 80)

Add-Lines (Section 'Architecture Decision')
Add-Lines (Read-IfExists '00_ADMIN/Requirements/ARCHITECTURE_DECISION_MODULAR_CLIENT_SPLIT.md' 140)

Add-Lines (Section 'Architecture Alignment Review')
Add-Lines (Read-IfExists '00_ADMIN/Reviews_and_Reports/ARCHITECTURE_ALIGNMENT_REVIEW_MASTER16_16_MODULAR_PACKAGE.md' 180)

Add-Lines (Section 'Current Backlog Recommendation')
Add-Lines (Get-CurrentRecommendation)

Add-Lines (Section 'Backlog Readiness')
Add-Lines 'Priority 1 items:'
Add-Lines (Get-PriorityOneItems)
Add-Lines ''
Add-Lines 'Started/not-complete status signals:'
Add-Lines (Get-StartedBacklogItems)

Add-Lines (Section 'Issue Log Open And Monitor Items')
Add-Lines '```text'
Add-Lines (Get-IssueFacts)
Add-Lines '```'

$handoff = Get-LatestHandoff
Add-Lines (Section 'Latest Handoff')
if ($handoff) {
  Add-Lines "File: $($handoff.FullName)"
  Add-Lines (Get-Content -LiteralPath $handoff.FullName -TotalCount 140)
} else {
  Add-Failure 'No handoff file found.'
  Add-Lines 'No handoff file found.'
}

Add-Lines (Section 'Required Next-Chat Declaration')
Add-Lines 'A new assistant must explicitly state whether the user request conflicts with the manifest, product intent gate, architecture decision, architecture alignment review, issue log, or backlog recommendation before acting.'
Add-Lines 'The modular browser-client split is a committed architecture constraint. Treat index.html as the package entry point only, keep the full modular /holesy/ package as baseline, provide changed-files-only GoDaddy delta packages by default, and route follow-on architecture/package alignment through PERF-012.'

Add-Lines (Section 'Confidence Footer')
if ($script:ConfidenceFailures.Count -gt 0) {
  Add-Lines 'Status: Blocked/missing evidence'
  Add-Lines 'Failures:'
  Add-Lines ($script:ConfidenceFailures | ForEach-Object { "- $_" })
} elseif ($script:ConfidenceWarnings.Count -gt 0) {
  Add-Lines 'Status: Verified current with stated limitations'
} else {
  Add-Lines 'Status: Verified current'
}
if ($script:ConfidenceWarnings.Count -gt 0) {
  Add-Lines 'Limitations/warnings:'
  Add-Lines ($script:ConfidenceWarnings | ForEach-Object { "- $_" })
}

$text = $snapshot -join [Environment]::NewLine
Write-Output $text

if ($WriteSnapshot) {
  $outPath = Join-Path $repoRoot '00_ADMIN/Reviews_and_Reports/NEW_CHAT_CONTEXT_SNAPSHOT_CURRENT.md'
  Set-Content -LiteralPath $outPath -Value $text -Encoding UTF8
  Write-Host ''
  Write-Host "Snapshot written to $outPath"
}


