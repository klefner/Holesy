# ============================================================
#  Downtown Devour - Project Updater
#  Right-click this file and choose "Run with PowerShell"
#  any time Claude pushes new code.
#  Injects Scripts, Scenes, and Shaders into your URP project.
#  Does NOT touch ProjectSettings or Packages (preserves URP).
# ============================================================

$REPO_OWNER  = "klefner"
$REPO_NAME   = "holesy"
$BRANCH      = "claude/happy-clarke-ORWAI"
$PROJECT_SUB = "DowntownDevour"

# -- Locate the Unity project -------------------------------------------------
# Searches common locations for a folder named DowntownDevour with an Assets
# subfolder.

$SEARCH_ROOTS = @(
    "C:\holesy",
    "$env:USERPROFILE",
    "$env:USERPROFILE\Documents",
    "$env:USERPROFILE\Desktop",
    "C:\Users\KentLefner"
)

function Find-Project {
    foreach ($root in $SEARCH_ROOTS) {
        $candidate = Join-Path $root $PROJECT_SUB
        if (Test-Path (Join-Path $candidate "Assets")) { return $candidate }
    }
    return $null
}

# -----------------------------------------------------------------------------

Clear-Host
Write-Host ""
Write-Host "  Downtown Devour - Project Updater" -ForegroundColor Cyan
Write-Host "  ===================================" -ForegroundColor Cyan
Write-Host ""

[Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Tls12

# Suppress the PS 5.1 progress bar that makes downloads 10x slower
$ProgressPreference = "SilentlyContinue"

$INSTALL_DIR = Find-Project

if (-not $INSTALL_DIR) {
    Write-Host "  Could not find DowntownDevour automatically." -ForegroundColor Yellow
    Write-Host "  Enter the full path to your DowntownDevour folder" -ForegroundColor Yellow
    Write-Host "  (example: C:\Users\KentLefner\DowntownDevour)" -ForegroundColor Gray
    Write-Host ""
    $INSTALL_DIR = Read-Host "  Path"
    if (-not (Test-Path $INSTALL_DIR)) {
        Write-Host ""
        Write-Host "  Folder not found at: $INSTALL_DIR" -ForegroundColor Red
        Read-Host "  Press Enter to close"
        exit 1
    }
}

Write-Host "  Project location: $INSTALL_DIR" -ForegroundColor Green
Write-Host ""

# -- Download latest ZIP ------------------------------------------------------

$ZIP_URL  = "https://github.com/$REPO_OWNER/$REPO_NAME/archive/refs/heads/$BRANCH.zip"
$TEMP_ZIP = "$env:TEMP\downtown-devour-update.zip"
$TEMP_DIR = "$env:TEMP\downtown-devour-extract"

Write-Host "  Downloading latest code from GitHub..." -ForegroundColor Yellow
Write-Host "  (This may take a minute depending on your connection...)" -ForegroundColor Gray

try {
    Invoke-WebRequest -Uri $ZIP_URL -OutFile $TEMP_ZIP -UseBasicParsing
} catch {
    Write-Host ""
    Write-Host "  ERROR: Download failed. Check your internet connection." -ForegroundColor Red
    Write-Host "  $($_.Exception.Message)" -ForegroundColor Red
    Write-Host ""
    Read-Host "  Press Enter to close"
    exit 1
}

Write-Host "  Download complete." -ForegroundColor Green
Write-Host "  Extracting..." -ForegroundColor Yellow

if (Test-Path $TEMP_DIR) { Remove-Item $TEMP_DIR -Recurse -Force }
Expand-Archive -Path $TEMP_ZIP -DestinationPath $TEMP_DIR -Force

$TOP = Get-ChildItem $TEMP_DIR -Directory | Select-Object -First 1
if (-not $TOP) {
    Write-Host "  ERROR: Archive was empty or corrupted." -ForegroundColor Red
    Read-Host "  Press Enter to close"
    exit 1
}

$SOURCE = Join-Path $TOP.FullName $PROJECT_SUB

if (-not (Test-Path $SOURCE)) {
    Write-Host "  ERROR: Could not find '$PROJECT_SUB' inside the downloaded archive." -ForegroundColor Red
    Read-Host "  Press Enter to close"
    exit 1
}

# -- Inject code and shaders only ---------------------------------------------
# Never touch ProjectSettings or Packages - preserve the URP pipeline assets
# Unity created when the project was first opened.

Write-Host "  Copying scripts and shaders into project..." -ForegroundColor Yellow

$INJECT_SUBFOLDERS = @(
    "Assets\Scripts",
    "Assets\Scenes",
    "Assets\Shaders",
    "Assets\Resources"
)

foreach ($sub in $INJECT_SUBFOLDERS) {
    $srcFolder = Join-Path $SOURCE $sub
    $dstFolder = Join-Path $INSTALL_DIR $sub

    if (Test-Path $srcFolder) {
        if (-not (Test-Path $dstFolder)) {
            New-Item -ItemType Directory -Path $dstFolder -Force | Out-Null
        }
        Copy-Item -Path "$srcFolder\*" -Destination $dstFolder -Recurse -Force
        Write-Host "    Updated: $sub" -ForegroundColor Gray
    }
}

# -- Clean up -----------------------------------------------------------------

Remove-Item $TEMP_ZIP -Force -ErrorAction SilentlyContinue
Remove-Item $TEMP_DIR -Recurse -Force -ErrorAction SilentlyContinue

# -- Done ---------------------------------------------------------------------

Write-Host ""
Write-Host "  All done!" -ForegroundColor Green
Write-Host ""
Write-Host "  Open Unity Hub -> click DowntownDevour." -ForegroundColor Cyan
Write-Host "  Unity will recompile (watch the progress bar at the bottom)." -ForegroundColor Cyan
Write-Host "  When the bar clears: File -> Open Scene -> Assets/Scenes/Game" -ForegroundColor Cyan
Write-Host "  Then press the Play button." -ForegroundColor Cyan
Write-Host ""
Read-Host "  Press Enter to close"
