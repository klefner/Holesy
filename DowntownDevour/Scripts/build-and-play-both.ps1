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

# -- Locate Unity project ------------------------------------------------------

$PROJECT_DIRS = @(
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

# -- Refuse to build while the editor is open ----------------------------------

$unityRunning = Get-Process Unity -ErrorAction SilentlyContinue
if ($unityRunning) {
    Write-Host ""
    Write-Host "  ERROR: Unity is running. Close the Unity editor first," -ForegroundColor Red
    Write-Host "  then double-click this file again." -ForegroundColor Red
    Read-Host "  Press Enter to close"
    exit 1
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

# -- Deploy Web build to GitHub Pages -------------------------------------------

$REPO_ROOTS = @(
    "C:\holesy",
    "C:\holesy-repo",
    "$env:USERPROFILE\holesy",
    "$env:USERPROFILE\Holesy"
)
$REPO_DIR = $null
foreach ($root in $REPO_ROOTS) {
    if (Test-Path (Join-Path $root ".git")) { $REPO_DIR = $root; break }
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
