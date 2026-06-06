# ============================================================
#  Downtown Devour - First-Time Setup
#
#  RIGHT-CLICK this file and choose "Run with PowerShell"
#  Do NOT double-click (that opens it as a text file).
#
#  Run this ONCE to create the Unity project and install
#  all game files. Use update-project.ps1 for future updates.
# ============================================================

$REPO_OWNER   = "klefner"
$REPO_NAME    = "holesy"
$BRANCH       = "claude/happy-clarke-ORWAI"
$PROJECT_SUB  = "DowntownDevour"
$PROJECT_NAME = "DowntownDevour"
$INSTALL_ROOT = "C:\Users\KentLefner"
$UNITY_VER    = "6000.0.40f1"

# Suppress the slow progress bar on Invoke-WebRequest (speeds up download 10x)
$ProgressPreference = "SilentlyContinue"

# Force TLS 1.2 for HTTPS
[Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Tls12

# -----------------------------------------------------------------------------

Clear-Host
Write-Host ""
Write-Host "  Downtown Devour - First-Time Setup" -ForegroundColor Cyan
Write-Host "  ====================================" -ForegroundColor Cyan
Write-Host ""

# -- Step 1: Find Unity Hub ---------------------------------------------------

Write-Host "  [1/4] Locating Unity Hub..." -ForegroundColor Yellow

$HUB_PATHS = @(
    "C:\Program Files\Unity Hub\Unity Hub.exe",
    "$env:LOCALAPPDATA\Programs\Unity Hub\Unity Hub.exe",
    "$env:ProgramFiles\Unity Hub\Unity Hub.exe"
)

$HUB_EXE = $null
foreach ($p in $HUB_PATHS) {
    if (Test-Path $p) { $HUB_EXE = $p; break }
}

if (-not $HUB_EXE) {
    Write-Host ""
    Write-Host "  ERROR: Unity Hub not found in standard locations." -ForegroundColor Red
    Write-Host "  Please enter the full path to 'Unity Hub.exe':" -ForegroundColor Yellow
    $HUB_EXE = Read-Host "  Path"
    if (-not (Test-Path $HUB_EXE)) {
        Write-Host "  That path does not exist. Please reinstall Unity Hub." -ForegroundColor Red
        Read-Host "  Press Enter to close"
        exit 1
    }
}

Write-Host "  Unity Hub found: $HUB_EXE" -ForegroundColor Green

# -- Step 2: Download game files from GitHub ----------------------------------
# (Done before project creation so the ZIP is available for the project scaffold)

Write-Host ""
Write-Host "  [2/4] Downloading game files from GitHub..." -ForegroundColor Yellow

$ZIP_URL  = "https://github.com/$REPO_OWNER/$REPO_NAME/archive/refs/heads/$BRANCH.zip"
$TEMP_ZIP = "$env:TEMP\downtown-devour-setup.zip"
$TEMP_DIR = "$env:TEMP\downtown-devour-setup-extract"

try {
    Invoke-WebRequest -Uri $ZIP_URL -OutFile $TEMP_ZIP -UseBasicParsing
} catch {
    Write-Host ""
    Write-Host "  ERROR: Download failed. Check your internet connection." -ForegroundColor Red
    Write-Host "  $($_.Exception.Message)" -ForegroundColor Red
    Read-Host "  Press Enter to close"
    exit 1
}

Write-Host "  Download complete. Extracting..." -ForegroundColor Gray

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
    Write-Host "  Top-level folder found: $($TOP.FullName)" -ForegroundColor Gray
    Read-Host "  Press Enter to close"
    exit 1
}

Write-Host "  Game files ready." -ForegroundColor Green

# -- Step 3: Create the Unity project -----------------------------------------

Write-Host ""
Write-Host "  [3/4] Creating Unity project '$PROJECT_NAME'..." -ForegroundColor Yellow

$PROJECT_DIR = Join-Path $INSTALL_ROOT $PROJECT_NAME

if (Test-Path (Join-Path $PROJECT_DIR "Assets")) {
    Write-Host "  Project already exists at: $PROJECT_DIR" -ForegroundColor Green
    Write-Host "  Skipping project creation, will only update game files." -ForegroundColor Gray
} else {
    Write-Host "  Creating project folder structure..." -ForegroundColor Gray
    New-Item -ItemType Directory -Path "$PROJECT_DIR\Assets\Scenes"   -Force | Out-Null
    New-Item -ItemType Directory -Path "$PROJECT_DIR\Assets\Scripts"  -Force | Out-Null
    New-Item -ItemType Directory -Path "$PROJECT_DIR\Assets\Shaders"  -Force | Out-Null
    New-Item -ItemType Directory -Path "$PROJECT_DIR\Packages"        -Force | Out-Null
    New-Item -ItemType Directory -Path "$PROJECT_DIR\ProjectSettings" -Force | Out-Null

    # Copy Packages and ProjectSettings from the repo ZIP
    # (avoids hardcoding versions; uses .NET WriteAllText to avoid UTF-8 BOM from PS 5.1)
    $srcPackages = Join-Path $SOURCE "Packages"
    $srcSettings = Join-Path $SOURCE "ProjectSettings"

    if (Test-Path $srcPackages) {
        Copy-Item -Path "$srcPackages\*" -Destination "$PROJECT_DIR\Packages" -Recurse -Force
        Write-Host "    Copied: Packages" -ForegroundColor Gray
    } else {
        $manifest = "{`n  ""dependencies"": {`n    ""com.unity.ugui"": ""2.0.0"",`n    ""com.unity.textmeshpro"": ""3.0.9"",`n    ""com.unity.modules.audio"": ""1.0.0"",`n    ""com.unity.modules.physics"": ""1.0.0"",`n    ""com.unity.modules.ui"": ""1.0.0"",`n    ""com.unity.modules.imgui"": ""1.0.0"",`n    ""com.unity.modules.jsonserialize"": ""1.0.0""`n  }`n}"
        [System.IO.File]::WriteAllText("$PROJECT_DIR\Packages\manifest.json", $manifest, [System.Text.Encoding]::UTF8)
    }

    if (Test-Path $srcSettings) {
        Copy-Item -Path "$srcSettings\*" -Destination "$PROJECT_DIR\ProjectSettings" -Recurse -Force
        Write-Host "    Copied: ProjectSettings" -ForegroundColor Gray
    } else {
        $version = "m_EditorVersion: $UNITY_VER`r`nm_EditorVersionWithRevision: $UNITY_VER (placeholder)`r`n"
        [System.IO.File]::WriteAllText("$PROJECT_DIR\ProjectSettings\ProjectVersion.txt", $version, [System.Text.Encoding]::UTF8)
    }

    Write-Host "  Project structure created." -ForegroundColor Green
    Write-Host ""
    Write-Host "  NOTE: When you first open this project in Unity Hub, Unity will" -ForegroundColor Cyan
    Write-Host "  download packages automatically. This takes 3-10 minutes (one time only)." -ForegroundColor Cyan
}

# -- Step 4: Inject game files ------------------------------------------------

Write-Host ""
Write-Host "  [4/4] Installing game files..." -ForegroundColor Yellow

$INJECT_SUBFOLDERS = @("Assets\Scripts", "Assets\Scenes", "Assets\Shaders")

foreach ($sub in $INJECT_SUBFOLDERS) {
    $srcFolder = Join-Path $SOURCE $sub
    $dstFolder = Join-Path $PROJECT_DIR $sub
    if (Test-Path $srcFolder) {
        if (-not (Test-Path $dstFolder)) {
            New-Item -ItemType Directory -Path $dstFolder -Force | Out-Null
        }
        Copy-Item -Path "$srcFolder\*" -Destination $dstFolder -Recurse -Force
        Write-Host "    Installed: $sub" -ForegroundColor Gray
    }
}

Remove-Item $TEMP_ZIP -Force -ErrorAction SilentlyContinue
Remove-Item $TEMP_DIR -Recurse -Force -ErrorAction SilentlyContinue

Write-Host "  Game files installed." -ForegroundColor Green

# -- Open Unity Hub -----------------------------------------------------------

Write-Host ""
Write-Host "  Opening Unity Hub..." -ForegroundColor Yellow
Start-Process -FilePath $HUB_EXE
Write-Host "  Unity Hub launched." -ForegroundColor Green

# -- Done ---------------------------------------------------------------------

Write-Host ""
Write-Host "  ============================================================" -ForegroundColor Cyan
Write-Host "  Setup complete!  Project folder:" -ForegroundColor Green
Write-Host "  $PROJECT_DIR" -ForegroundColor White
Write-Host "  ============================================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "  Next steps:" -ForegroundColor Cyan
Write-Host "  1. In Unity Hub, click 'DowntownDevour' to open it." -ForegroundColor White
Write-Host "     If not listed: click Add -> browse to the folder above." -ForegroundColor Gray
Write-Host "  2. Wait for Unity to import everything (progress bar at bottom)." -ForegroundColor White
Write-Host "     First open takes 3-10 minutes. Later opens are fast." -ForegroundColor Gray
Write-Host "  3. In Unity: File -> Open Scene -> Assets/Scenes/Game" -ForegroundColor White
Write-Host "  4. Press the Play button (triangle at top)." -ForegroundColor White
Write-Host "  5. Move your hole with the mouse or WASD." -ForegroundColor White
Write-Host ""
Write-Host "  For future code updates, run update-project.ps1 instead." -ForegroundColor Gray
Write-Host ""
Read-Host "  Press Enter to close"
