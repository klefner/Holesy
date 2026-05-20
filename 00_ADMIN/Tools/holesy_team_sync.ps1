param(
  [switch]$WriteSnapshot
)

$ErrorActionPreference = 'Stop'

$scriptPath = Split-Path -Parent $MyInvocation.MyCommand.Path
$repoRoot = Resolve-Path (Join-Path $scriptPath '..\..')
Set-Location $repoRoot

function Section($title) {
  ""
  "## $title"
  ""
}

function Read-IfExists($path, $maxLines = 80) {
  if (Test-Path -LiteralPath $path) {
    Get-Content -LiteralPath $path -TotalCount $maxLines
  } else {
    "MISSING: $path"
  }
}

function Get-CurrentRecommendation {
  $path = '00_ADMIN/Requirements/PRODUCT_BACKLOG.md'
  if (-not (Test-Path -LiteralPath $path)) { return @('MISSING: PRODUCT_BACKLOG.md') }
  $lines = Get-Content -LiteralPath $path
  $start = [Array]::FindIndex($lines, [Predicate[string]]{ param($line) $line -eq '## Current Recommendation' })
  if ($start -lt 0) { return @('Current Recommendation heading not found') }
  return $lines[$start..([Math]::Min($lines.Count - 1, $start + 35))]
}

function Get-LatestHandoff {
  $handoffs = Get-ChildItem -LiteralPath '00_ADMIN/Policies_and_Procedures' -Filter 'NEXT_CODEX_CHAT_HANDOFF_MASTER*.md' |
    Sort-Object {
      if ($_.BaseName -match 'MASTER(\d+)') { [int]$Matches[1] } else { 0 }
    } -Descending
  if ($handoffs.Count -eq 0) { return $null }
  return $handoffs[0]
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

Add-Lines (Section 'Git State')
Add-Lines '```text'
Add-Lines (git status --short --branch)
Add-Lines (git log --oneline --decorate -8)
Add-Lines (git branch -vv)
Add-Lines '```'

Add-Lines (Section 'Release Source Of Truth Manifest')
Add-Lines (Read-IfExists '00_ADMIN/Policies_and_Procedures/RELEASE_SOURCE_OF_TRUTH_MANIFEST.md' 140)

Add-Lines (Section 'Product Intent Gate')
Add-Lines (Read-IfExists '00_ADMIN/Policies_and_Procedures/PRODUCT_INTENT_GATE.md' 140)

Add-Lines (Section 'Current Basis')
Add-Lines (Read-IfExists '10_SOURCE/Current/CURRENT_BASIS.md' 80)

Add-Lines (Section 'Architecture Decision')
Add-Lines (Read-IfExists '00_ADMIN/Requirements/ARCHITECTURE_DECISION_MODULAR_CLIENT_SPLIT.md' 140)

Add-Lines (Section 'Current Backlog Recommendation')
Add-Lines (Get-CurrentRecommendation)

Add-Lines (Section 'Issue Log Open And Monitor Items')
Add-Lines '```text'
$issueMatches = Select-String -Path '00_ADMIN/Reviews_and_Reports/ISSUE_LOG.md' -Pattern '\| QA-.*\| (open|monitor) \|'
if ($issueMatches) {
  Add-Lines ($issueMatches | ForEach-Object { $_.Line })
} else {
  Add-Lines 'No open or monitor QA items found.'
}
Add-Lines '```'

$handoff = Get-LatestHandoff
Add-Lines (Section 'Latest Handoff')
if ($handoff) {
  Add-Lines "File: $($handoff.FullName)"
  Add-Lines (Get-Content -LiteralPath $handoff.FullName -TotalCount 140)
} else {
  Add-Lines 'No handoff file found.'
}

Add-Lines (Section 'Required Next-Chat Declaration')
Add-Lines 'A new assistant must explicitly state whether the user request conflicts with the manifest, product intent gate, architecture decision, issue log, or backlog recommendation before acting.'

$text = $snapshot -join [Environment]::NewLine
Write-Output $text

if ($WriteSnapshot) {
  $outPath = Join-Path $repoRoot '00_ADMIN/Reviews_and_Reports/NEW_CHAT_CONTEXT_SNAPSHOT_CURRENT.md'
  Set-Content -LiteralPath $outPath -Value $text -Encoding UTF8
  Write-Host ""
  Write-Host "Snapshot written to $outPath"
}
