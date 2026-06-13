# ============================================================
#  Downtown Devour - Build & Play Both
#  Pulls the latest code, injects it into the local Unity
#  project, builds the Windows PC player AND the Web player,
#  verifies the build contains the new code, deploys the Web
#  build to GitHub Pages, then launches the PC game.
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

# -- Locate the git repo and pull the latest code -------------------------------

$REPO_ROOTS = @(
    "C:\holesy",
    "C:\holesy-repo",
    "$env:USERPROFILE\holesy",
    "$env:USERPROFILE\Holesy",
    "$env:USERPROFILE\Documents\holesy",
    "$env:USERPROFILE\Documents\Holesy",
    "$env:USERPROFILE\Desktop\holesy",
    "$env:USERPROFILE\Desktop\Holesy"
)
$REPO_DIR = $null
foreach ($root in $REPO_ROOTS) {
    if (Test-Path (Join-Path $root ".git")) { $REPO_DIR = $root; break }
}

if (-not $REPO_DIR) {
    Write-Host "  ERROR: git repo not found. Looked in:" -ForegroundColor Red
    $REPO_ROOTS | ForEach-Object { Write-Host "    $_" -ForegroundColor Red }
    Read-Host "  Press Enter to close"
    exit 1
}
Write-Host "  Repo:    $REPO_DIR" -ForegroundColor Green

Write-Host "  Pulling latest code..." -ForegroundColor Yellow
Push-Location $REPO_DIR
git fetch origin $BRANCH 2>&1 | Out-Null
git checkout $BRANCH 2>&1 | Out-Null
git pull origin $BRANCH 2>&1
$pullOk = ($LASTEXITCODE -eq 0)
Pop-Location
if (-not $pullOk) {
    # A failed pull means the build would silently compile OLD code -
    # exactly the bug where new versions never show up in the game.
    Write-Host ""
    Write-Host "  ERROR: git pull failed. Refusing to build stale code." -ForegroundColor Red
    Write-Host "  Open PowerShell in $REPO_DIR, run 'git status', and send" -ForegroundColor Yellow
    Write-Host "  Claude the output so the repo can be repaired." -ForegroundColor Yellow
    Read-Host "  Press Enter to close"
    exit 1
}
Write-Host "  Code up to date." -ForegroundColor Green
Write-Host ""

# -- Locate the Unity project ---------------------------------------------------
# The standalone project (created by setup-project.ps1 and opened in Unity)
# carries the URP pipeline assets the repo does not track, so it is the one
# that must be built.  Fresh code is injected into it below.

$PROJECT_DIRS = @(
    "C:\holesy\DowntownDevour",
    "$env:USERPROFILE\holesy\DowntownDevour",
    "$env:USERPROFILE\Holesy\DowntownDevour",
    "$env:USERPROFILE\DowntownDevour",
    (Join-Path $REPO_DIR "DowntownDevour")
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

# -- Inject the pulled code into the project ------------------------------------
# This replaces the old update-project.ps1 step.  Without it, the project
# keeps compiling whatever code it last received - the exact cause of builds
# being stuck on an old version while the repo was current.

$REPO_PROJ = Join-Path $REPO_DIR "DowntownDevour"
if ((Resolve-Path $PROJ).Path -ne (Resolve-Path $REPO_PROJ -ErrorAction SilentlyContinue).Path) {
    Write-Host "  Updating project code from repo..." -ForegroundColor Yellow
    $INJECT = @("Assets\Scripts", "Assets\Scenes", "Assets\Shaders", "Assets\Resources", "Scripts")
    foreach ($sub in $INJECT) {
        $src = Join-Path $REPO_PROJ $sub
        $dst = Join-Path $PROJ $sub
        if (Test-Path $src) {
            if (-not (Test-Path $dst)) { New-Item -ItemType Directory -Path $dst -Force | Out-Null }
            Copy-Item -Path "$src\*" -Destination $dst -Recurse -Force
            Write-Host "    Updated: $sub" -ForegroundColor Gray
        }
    }
}

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
# cleanly.  If a "save changes?" dialog pops up, answer it - the script waits.

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
Write-Host "  Building Windows PC + Web (WebGL can take 15-30 min)..." -ForegroundColor Yellow
Write-Host "  Log: $LOG" -ForegroundColor Gray
Write-Host "  Progress dots appear every 15 s while Unity is working." -ForegroundColor Gray
Write-Host ""

$proc = Start-Process -FilePath $UNITY -ArgumentList @(
    "-batchmode", "-quit",
    "-projectPath", "`"$PROJ`"",
    "-executeMethod", "AutoBuild.BuildAll",
    "-logFile", "`"$LOG`""
) -PassThru -NoNewWindow

# Poll every 15 s so the window shows visible progress instead of looking frozen.
# We also print the last meaningful log line so you can see what Unity is doing.
$buildStart  = Get-Date
$lastLogSize = 0
$dots        = 0

while (-not $proc.HasExited) {
    Start-Sleep -Seconds 15
    $elapsed = [int](((Get-Date) - $buildStart).TotalSeconds)
    $min     = [int]($elapsed / 60)
    $sec     = $elapsed % 60

    $logLine = ""
    if (Test-Path $LOG) {
        $sz = (Get-Item $LOG).Length
        if ($sz -ne $lastLogSize) {
            $lastLogSize = $sz
            # Show the last non-blank line from the log for context
            $logLine = (Get-Content $LOG -ErrorAction SilentlyContinue |
                        Where-Object { $_.Trim() -ne "" } |
                        Select-Object -Last 1)
        }
    }

    $dots++
    $dot = "." * (($dots % 4) + 1)
    $timeStr = if ($min -gt 0) { "${min}m ${sec}s" } else { "${sec}s" }
    Write-Host ("  [{0,-4}] {1}" -f $timeStr, ($logLine -replace "^\[.*?\]\s*", "").Trim()) -ForegroundColor Gray
}
$proc.WaitForExit()

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
# of the project - deploying it would be pointless.

if ($EXPECTED_VERSION) {
    $dataFile = Join-Path $WEB_DIR "Build\Web.data"
    if (Test-Path $dataFile) {
        $found = Select-String -Path $dataFile -Pattern ([regex]::Escape($EXPECTED_VERSION)) -Quiet
        if ($found) {
            Write-Host "  Verified: build contains $EXPECTED_VERSION." -ForegroundColor Green
        } else {
            Write-Host ""
            Write-Host "  ERROR: build does NOT contain $EXPECTED_VERSION - it compiled stale code." -ForegroundColor Red
            Write-Host "  Project built: $PROJ" -ForegroundColor Red
            Write-Host "  Tell Claude this happened and include the two lines above." -ForegroundColor Yellow
            Read-Host "  Press Enter to close"
            exit 1
        }
    }
}

# -- Deploy Web build to GitHub Pages -------------------------------------------

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
