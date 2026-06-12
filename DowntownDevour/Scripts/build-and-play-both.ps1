# ============================================================
#  Downtown Devour - Build & Play Both
#  Builds the Windows PC player AND the Web player, deploys the
#  Web build to GitHub Pages, then launches the PC game.
#
#  IMPORTANT: Close the Unity editor before running this -
#  batch builds cannot run while the project is open.
# ============================================================

$REPO_OWNER = "klefner"
$REPO_NAME  = "Holesy"
$BRANCH     = "claude/happy-clarke-ORWAI"
$PAGES_URL  = "https://$REPO_OWNER.github.io/$REPO_NAME/"

Clear-Host
Write-Host ""
Write-Host "  Downtown Devour - Build & Play Both" -ForegroundColor Cyan
Write-Host "  =====================================" -ForegroundColor Cyan
Write-Host ""

# -- Pull latest code from GitHub first ----------------------------------------
# This replaces the separate update-project step.  The Unity project lives
# inside the repo, so a git pull is all that's needed to sync everything.

$REPO_ROOTS = @(
    "C:\holesy",
    "C:\holesy-repo",
    "$env:USERPROFILE\holesy",
    "$env:USERPROFILE\Holesy"
)
$PULL_DIR = $null
foreach ($root in $REPO_ROOTS) {
    if (Test-Path (Join-Path $root ".git")) { $PULL_DIR = $root; break }
}

if ($PULL_DIR) {
    Write-Host "  Pulling latest code..." -ForegroundColor Yellow
    Push-Location $PULL_DIR
    git fetch origin $BRANCH 2>&1 | Out-Null
    git checkout $BRANCH 2>&1 | Out-Null
    git pull origin $BRANCH 2>&1
    $pullOk = ($LASTEXITCODE -eq 0)
    Pop-Location
    if ($pullOk) {
        Write-Host "  Code up to date." -ForegroundColor Green
    } else {
        # A failed pull means the build silently compiles OLD code — this is
        # exactly how new versions stopped showing up in the game.  Stop hard.
        Write-Host ""
        Write-Host "  ERROR: git pull failed. Refusing to build stale code." -ForegroundColor Red
        Write-Host "  Open PowerShell in $PULL_DIR, run 'git status', and send" -ForegroundColor Yellow
        Write-Host "  Claude the output so the repo can be repaired." -ForegroundColor Yellow
        Read-Host "  Press Enter to close"
        exit 1
    }
} else {
    Write-Host "  WARNING: repo not found — building with current local files." -ForegroundColor Yellow
}

Write-Host ""

# -- Locate Unity project ------------------------------------------------------
# The project inside the pulled repo ALWAYS wins.  Searching fixed paths first
# can silently pick up a stale copy of the project left behind by the old
# setup/update scripts, which then builds outdated code forever.

$PROJECT_DIRS = @()
if ($PULL_DIR) { $PROJECT_DIRS += (Join-Path $PULL_DIR "DowntownDevour") }
$PROJECT_DIRS += @(
    "C:\holesy\DowntownDevour",
    "$env:USERPROFILE\holesy\DowntownDevour",
    "$env:USERPROFILE\Holesy\DowntownDevour"
)

$PROJ = $null
foreach ($p in $PROJECT_DIRS) {
    if (Test-Path (Join-Path $p "Assets")) { $PROJ = $p; break }
}
if (-not $PROJ) {
    Write-Host "  ERROR: Unity project not found (looked for an Assets folder in:" -ForegroundColor Red
    $PROJECT_DIRS | ForEach-Object { Write-Host "    $_" -ForegroundColor Red }
    Read-Host "  Press Enter to close"
    exit 1
}
Write-Host "  Project: $PROJ" -ForegroundColor Green

# Read the version stamp from the code about to be built, so the finished
# build can be verified against it after compiling.
$EXPECTED_VERSION = $null
$uiFile = Join-Path $PROJ "Assets\Scripts\UIManager.cs"
if (Test-Path $uiFile) {
    $m = Select-String -Path $uiFile -Pattern 'VERSION\s*=\s*"(v[0-9.]+)"' | Select-Object -First 1
    if ($m) {
        $EXPECTED_VERSION = $m.Matches[0].Groups[1].Value
        Write-Host "  Code version: $EXPECTED_VERSION" -ForegroundColor Green
    }
}

# -- Close the Unity editor if it is open ---------------------------------------
# Graceful close only (same as clicking the X) so Unity saves and shuts down
# cleanly.  If a "save changes?" dialog pops up, answer it — the script waits.

$unityRunning = Get-Process Unity -ErrorAction SilentlyContinue
if ($unityRunning) {
    Write-Host "  Unity is open - closing it..." -ForegroundColor Yellow
    $unityRunning | ForEach-Object { $_.CloseMainWindow() | Out-Null }

    $waited = 0
    while ((Get-Process Unity -ErrorAction SilentlyContinue) -and $waited -lt 90) {
        Start-Sleep -Seconds 2
        $waited += 2
        if ($waited -eq 10) {
            Write-Host "  Still closing... if Unity is asking to save, answer the dialog." -ForegroundColor Gray
        }
    }

    if (Get-Process Unity -ErrorAction SilentlyContinue) {
        Write-Host ""
        Write-Host "  ERROR: Unity did not close (waited 90 s)." -ForegroundColor Red
        Write-Host "  Close it manually, then double-click this file again." -ForegroundColor Red
        Read-Host "  Press Enter to close"
        exit 1
    }
    Write-Host "  Unity closed." -ForegroundColor Green
    Write-Host ""
}

# -- Locate the matching Unity editor ------------------------------------------

$verFile = Join-Path $PROJ "ProjectSettings\ProjectVersion.txt"
$UNITY   = $null
if (Test-Path $verFile) {
    $ver = (Get-Content $verFile | Select-String "m_EditorVersion:").ToString().Split(":")[1].Trim()
    $candidate = "C:\Program Files\Unity\Hub\Editor\$ver\Editor\Unity.exe"
    if (Test-Path $candidate) { $UNITY = $candidate }
}
if (-not $UNITY) {
    # Fall back to the newest installed editor
    $editors = Get-ChildItem "C:\Program Files\Unity\Hub\Editor" -Directory -ErrorAction SilentlyContinue |
               Sort-Object Name -Descending
    foreach ($e in $editors) {
        $candidate = Join-Path $e.FullName "Editor\Unity.exe"
        if (Test-Path $candidate) { $UNITY = $candidate; break }
    }
}
if (-not $UNITY) {
    Write-Host "  ERROR: Unity.exe not found under C:\Program Files\Unity\Hub\Editor" -ForegroundColor Red
    Read-Host "  Press Enter to close"
    exit 1
}
Write-Host "  Unity:   $UNITY" -ForegroundColor Green

# -- Build both platforms -------------------------------------------------------

$BUILDS = Join-Path $PROJ "Builds"
$LOG    = Join-Path $BUILDS "build.log"
New-Item -ItemType Directory -Path $BUILDS -Force | Out-Null

# Wipe old outputs first.  Leftovers from a previous build can make a failed
# or skipped build look successful (the file-exists checks below would pass).
Remove-Item (Join-Path $BUILDS "Windows") -Recurse -Force -ErrorAction SilentlyContinue
Remove-Item (Join-Path $BUILDS "Web")     -Recurse -Force -ErrorAction SilentlyContinue

Write-Host ""
Write-Host "  Building Windows PC + Web (this takes several minutes)..." -ForegroundColor Yellow
Write-Host "  Progress log: $LOG" -ForegroundColor Gray

$proc = Start-Process -FilePath $UNITY -ArgumentList @(
    "-batchmode", "-quit",
    "-projectPath", "`"$PROJ`"",
    "-executeMethod", "AutoBuild.BuildAll",
    "-logFile", "`"$LOG`""
) -PassThru -Wait -NoNewWindow

$PC_EXE  = Join-Path $BUILDS "Windows\DowntownDevour.exe"
$WEB_DIR = Join-Path $BUILDS "Web"

if ($proc.ExitCode -ne 0 -or
    -not (Test-Path $PC_EXE) -or
    -not (Test-Path (Join-Path $WEB_DIR "index.html"))) {
    Write-Host ""
    Write-Host "  ERROR: Build failed (exit $($proc.ExitCode))." -ForegroundColor Red
    Write-Host "  Check the log: $LOG" -ForegroundColor Red
    Write-Host "  (If the PC build failed: install 'Windows Build Support' for this" -ForegroundColor Gray
    Write-Host "   Unity version via Unity Hub > Installs > gear icon > Add modules.)" -ForegroundColor Gray
    Read-Host "  Press Enter to close"
    exit 1
}

Write-Host "  Both builds succeeded." -ForegroundColor Green

# -- Verify the build actually contains the current code -------------------------
# String constants (like the UIManager version stamp) end up in Web.data.
# If the stamp is missing, the build compiled from a different (stale) copy
# of the project — deploying it would be pointless.

if ($EXPECTED_VERSION) {
    $dataFile = Join-Path $WEB_DIR "Build\Web.data"
    if (Test-Path $dataFile) {
        $found = Select-String -Path $dataFile -Pattern ([regex]::Escape($EXPECTED_VERSION)) -Quiet
        if ($found) {
            Write-Host "  Verified: build contains $EXPECTED_VERSION." -ForegroundColor Green
        } else {
            Write-Host ""
            Write-Host "  ERROR: build does NOT contain $EXPECTED_VERSION — it compiled stale code." -ForegroundColor Red
            Write-Host "  Project built: $PROJ" -ForegroundColor Red
            Write-Host "  Tell Claude this happened and include the two lines above." -ForegroundColor Yellow
            Read-Host "  Press Enter to close"
            exit 1
        }
    }
}

# -- Deploy Web build to GitHub Pages -------------------------------------------

$REPO_DIR = $PULL_DIR
if (-not $REPO_DIR) {
    foreach ($root in $REPO_ROOTS) {
        if (Test-Path (Join-Path $root ".git")) { $REPO_DIR = $root; break }
    }
}

if ($REPO_DIR) {
    Write-Host ""
    Write-Host "  Deploying Web build to GitHub Pages..." -ForegroundColor Yellow
    Push-Location $REPO_DIR

    git fetch origin $BRANCH 2>&1 | Out-Null
    git checkout $BRANCH 2>&1 | Out-Null
    git pull origin $BRANCH 2>&1 | Out-Null

    $DOCS_DIR = Join-Path $REPO_DIR "docs"
    if (Test-Path $DOCS_DIR) { Remove-Item $DOCS_DIR -Recurse -Force }
    New-Item -ItemType Directory -Path $DOCS_DIR -Force | Out-Null
    Copy-Item -Path "$WEB_DIR\*" -Destination $DOCS_DIR -Recurse -Force
    New-Item -ItemType File -Path (Join-Path $DOCS_DIR ".nojekyll") -Force | Out-Null

    git add docs/ 2>&1 | Out-Null
    $timestamp = Get-Date -Format "yyyy-MM-dd HH:mm"
    git commit -m "Deploy Web build $timestamp" 2>&1 | Out-Null
    git push -u origin $BRANCH 2>&1

    if ($LASTEXITCODE -eq 0) {
        Write-Host "  Web build deployed." -ForegroundColor Green
    } else {
        Write-Host "  WARNING: push failed - web deploy skipped. PC build still works." -ForegroundColor Yellow
    }
    Pop-Location
} else {
    Write-Host "  WARNING: local repo not found - web deploy skipped." -ForegroundColor Yellow
}

# -- Launch the PC game ----------------------------------------------------------

Write-Host ""
Write-Host "  Launching PC game..." -ForegroundColor Yellow
Start-Process -FilePath $PC_EXE

Write-Host ""
Write-Host "  Done!" -ForegroundColor Green
Write-Host ""
Write-Host "  PC:  running now ($PC_EXE)" -ForegroundColor Cyan
Write-Host "  Web: $PAGES_URL" -ForegroundColor Cyan
Write-Host "       (GitHub Pages takes 1-2 minutes to update after a push)" -ForegroundColor Gray
Write-Host ""
Read-Host "  Press Enter to close"
