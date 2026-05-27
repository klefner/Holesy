# ============================================================
#  Downtown Devour - Project Updater
#  Double-click this file any time Claude pushes new code.
#  It downloads the latest version and overwrites your files.
# ============================================================

$REPO_OWNER  = "klefner"
$REPO_NAME   = "holesy"
$BRANCH      = "claude/happy-clarke-ORWAI"
$PROJECT_SUB = "DowntownDevour"   # subfolder inside the repo

# ── Where is your DowntownDevour project on this machine? ────────────────────
# The script searches common locations first; if it can't find it, it asks you.

$SEARCH_ROOTS = @(
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

# ─────────────────────────────────────────────────────────────────────────────

Clear-Host
Write-Host ""
Write-Host "  Downtown Devour - Project Updater" -ForegroundColor Cyan
Write-Host "  ===================================" -ForegroundColor Cyan
Write-Host ""

# Force TLS 1.2 (required on older Windows for HTTPS downloads)
[Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Tls12

# Find the project
$INSTALL_DIR = Find-Project

if (-not $INSTALL_DIR) {
    Write-Host "  Could not find DowntownDevour automatically." -ForegroundColor Yellow
    Write-Host "  Enter the full path to your DowntownDevour folder" -ForegroundColor Yellow
    Write-Host "  (example: C:\Users\KentLefner\DowntownDevour)" -ForegroundColor Gray
    Write-Host ""
    $INSTALL_DIR = Read-Host "  Path"
    if (-not (Test-Path $INSTALL_DIR)) {
        Write-Host ""
        Write-Host "  Creating new folder at: $INSTALL_DIR" -ForegroundColor Green
        New-Item -ItemType Directory -Path $INSTALL_DIR -Force | Out-Null
    }
}

Write-Host "  Project location: $INSTALL_DIR" -ForegroundColor Green
Write-Host ""

# ── Download the latest ZIP from GitHub ──────────────────────────────────────

$ZIP_URL  = "https://github.com/$REPO_OWNER/$REPO_NAME/archive/refs/heads/$BRANCH.zip"
$TEMP_ZIP = "$env:TEMP\downtown-devour-update.zip"
$TEMP_DIR = "$env:TEMP\downtown-devour-extract"

Write-Host "  Downloading latest code from GitHub..." -ForegroundColor Yellow

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

# Clean up old temp extract if present
if (Test-Path $TEMP_DIR) { Remove-Item $TEMP_DIR -Recurse -Force }

Expand-Archive -Path $TEMP_ZIP -DestinationPath $TEMP_DIR -Force

# ── Find the DowntownDevour subfolder inside the extracted archive ─────────────
# GitHub names the top folder like: holesy-claude-happy-clarke-ORWAI
# We just grab whatever folder is there and look inside it.

$TOP = Get-ChildItem $TEMP_DIR -Directory | Select-Object -First 1
if (-not $TOP) {
    Write-Host "  ERROR: Archive was empty or corrupted." -ForegroundColor Red
    Read-Host "  Press Enter to close"
    exit 1
}

$SOURCE = Join-Path $TOP.FullName $PROJECT_SUB

if (-not (Test-Path $SOURCE)) {
    Write-Host "  ERROR: Could not find '$PROJECT_SUB' inside the downloaded archive." -ForegroundColor Red
    Write-Host "  Top-level folder found: $($TOP.FullName)" -ForegroundColor Gray
    Read-Host "  Press Enter to close"
    exit 1
}

# ── Copy files into the project folder ───────────────────────────────────────

Write-Host "  Copying updated files to project..." -ForegroundColor Yellow

# Copy Assets and ProjectSettings (skip Library - Unity regenerates it)
$FOLDERS_TO_UPDATE = @("Assets", "ProjectSettings", "Packages")
foreach ($folder in $FOLDERS_TO_UPDATE) {
    $srcFolder = Join-Path $SOURCE $folder
    $dstFolder = Join-Path $INSTALL_DIR $folder
    if (Test-Path $srcFolder) {
        Copy-Item -Path "$srcFolder\*" -Destination $dstFolder -Recurse -Force
        Write-Host "    Updated: $folder" -ForegroundColor Gray
    }
}

# ── Clean up temp files ───────────────────────────────────────────────────────

Remove-Item $TEMP_ZIP  -Force -ErrorAction SilentlyContinue
Remove-Item $TEMP_DIR  -Recurse -Force -ErrorAction SilentlyContinue

# ── Done ──────────────────────────────────────────────────────────────────────

Write-Host ""
Write-Host "  All done!" -ForegroundColor Green
Write-Host ""
Write-Host "  If Unity is already open, it will recompile automatically." -ForegroundColor Cyan
Write-Host "  Just look at the bottom of the Unity window for the progress bar." -ForegroundColor Cyan
Write-Host ""
Write-Host "  If Unity is not open:" -ForegroundColor Cyan
Write-Host "  Open Unity Hub -> click DowntownDevour -> press Play" -ForegroundColor Cyan
Write-Host ""
Read-Host "  Press Enter to close"
