# ============================================================
#  Downtown Devour - First-Time Setup
#  Double-click this file ONCE to create the Unity project
#  and install all game files automatically.
#
#  After this runs, use update-project.ps1 for future updates.
# ============================================================

$REPO_OWNER   = "klefner"
$REPO_NAME    = "holesy"
$BRANCH       = "claude/happy-clarke-ORWAI"
$PROJECT_SUB  = "DowntownDevour"
$PROJECT_NAME = "DowntownDevour"
$INSTALL_ROOT = "C:\Users\KentLefner"
$UNITY_VER    = "6000.1.14f1"
$TEMPLATE     = "com.unity.template.universal-3d"

# ─────────────────────────────────────────────────────────────────────────────

Clear-Host
Write-Host ""
Write-Host "  Downtown Devour - First-Time Setup" -ForegroundColor Cyan
Write-Host "  ====================================" -ForegroundColor Cyan
Write-Host ""

# Force TLS 1.2 for HTTPS downloads
[Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Tls12

# ── Step 1: Find Unity Hub ────────────────────────────────────────────────────

Write-Host "  [1/5] Locating Unity Hub..." -ForegroundColor Yellow

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
        Write-Host "  That path does not exist. Please reinstall Unity Hub from unityhub.com" -ForegroundColor Red
        Read-Host "  Press Enter to close"
        exit 1
    }
}

Write-Host "  Unity Hub found: $HUB_EXE" -ForegroundColor Green

# ── Step 2: Find Unity 6 editor ──────────────────────────────────────────────

Write-Host ""
Write-Host "  [2/5] Locating Unity $UNITY_VER editor..." -ForegroundColor Yellow

$EDITOR_ROOTS = @(
    "C:\Program Files\Unity\Hub\Editor",
    "$env:ProgramFiles\Unity\Hub\Editor"
)

$UNITY_EXE = $null
foreach ($root in $EDITOR_ROOTS) {
    $candidate = Join-Path $root "$UNITY_VER\Editor\Unity.exe"
    if (Test-Path $candidate) { $UNITY_EXE = $candidate; break }
}

if (-not $UNITY_EXE) {
    Write-Host ""
    Write-Host "  Unity $UNITY_VER was not found." -ForegroundColor Yellow
    Write-Host "  Please make sure Unity $UNITY_VER (LTS) is installed via Unity Hub." -ForegroundColor Yellow
    Write-Host "  Open Unity Hub -> Installs -> Install Editor -> Official Releases -> $UNITY_VER" -ForegroundColor Gray
    Write-Host ""
    Write-Host "  If Unity IS installed but in a different folder, enter the path to Unity.exe:" -ForegroundColor Yellow
    Write-Host "  (Or press Enter to skip — you can still open the project manually)" -ForegroundColor Gray
    $manual = Read-Host "  Path to Unity.exe"
    if ($manual -and (Test-Path $manual)) { $UNITY_EXE = $manual }
}

if ($UNITY_EXE) {
    Write-Host "  Unity editor found: $UNITY_EXE" -ForegroundColor Green
} else {
    Write-Host "  Continuing without Unity path (you will open the project manually)." -ForegroundColor Yellow
}

# ── Step 3: Create the Unity project ─────────────────────────────────────────

Write-Host ""
Write-Host "  [3/5] Creating Unity project '$PROJECT_NAME'..." -ForegroundColor Yellow

$PROJECT_DIR = Join-Path $INSTALL_ROOT $PROJECT_NAME

if (Test-Path (Join-Path $PROJECT_DIR "Assets")) {
    Write-Host "  Project already exists at: $PROJECT_DIR" -ForegroundColor Green
    Write-Host "  Skipping project creation, will only update game files." -ForegroundColor Gray
    $SKIP_CREATE = $true
} else {
    $SKIP_CREATE = $false

    # Try Unity Hub headless project creation
    Write-Host "  Attempting Unity Hub headless create (this can take 2-5 minutes)..." -ForegroundColor Gray
    Write-Host "  A Unity splash screen may briefly appear — that is normal." -ForegroundColor Gray
    Write-Host ""

    $hubArgs = @(
        "--",
        "--headless", "create-project",
        "--name",     $PROJECT_NAME,
        "--path",     $INSTALL_ROOT,
        "--template", $TEMPLATE,
        "--version",  $UNITY_VER
    )

    try {
        $proc = Start-Process -FilePath $HUB_EXE -ArgumentList $hubArgs `
                              -Wait -PassThru -WindowStyle Normal
        $exitCode = $proc.ExitCode
    } catch {
        $exitCode = -1
    }

    # Verify the project was actually created
    if (-not (Test-Path (Join-Path $PROJECT_DIR "Assets"))) {
        Write-Host ""
        Write-Host "  Unity Hub headless create did not produce a project folder." -ForegroundColor Yellow
        Write-Host "  Falling back: creating minimal project structure manually..." -ForegroundColor Yellow

        # Minimal URP project scaffold
        New-Item -ItemType Directory -Path "$PROJECT_DIR\Assets"           -Force | Out-Null
        New-Item -ItemType Directory -Path "$PROJECT_DIR\Assets\Scenes"    -Force | Out-Null
        New-Item -ItemType Directory -Path "$PROJECT_DIR\Assets\Scripts"   -Force | Out-Null
        New-Item -ItemType Directory -Path "$PROJECT_DIR\Assets\Shaders"   -Force | Out-Null
        New-Item -ItemType Directory -Path "$PROJECT_DIR\Packages"         -Force | Out-Null
        New-Item -ItemType Directory -Path "$PROJECT_DIR\ProjectSettings"  -Force | Out-Null

        # Packages/manifest.json — requests URP and TextMeshPro
        $manifest = @"
{
  "dependencies": {
    "com.unity.render-pipelines.universal": "17.0.3",
    "com.unity.textmeshpro": "3.0.9",
    "com.unity.modules.physics": "1.0.0",
    "com.unity.modules.ui": "1.0.0",
    "com.unity.modules.audio": "1.0.0",
    "com.unity.modules.imageconversion": "1.0.0"
  }
}
"@
        Set-Content -Path "$PROJECT_DIR\Packages\manifest.json" -Value $manifest -Encoding UTF8

        # ProjectSettings/ProjectVersion.txt
        $version = "m_EditorVersion: $UNITY_VER`nm_EditorVersionWithRevision: $UNITY_VER (default)`n"
        Set-Content -Path "$PROJECT_DIR\ProjectSettings\ProjectVersion.txt" -Value $version -Encoding UTF8

        Write-Host "  Minimal project structure created." -ForegroundColor Green
        Write-Host ""
        Write-Host "  IMPORTANT: When you open this project in Unity Hub for the first time," -ForegroundColor Cyan
        Write-Host "  Unity will download URP packages automatically (requires internet)." -ForegroundColor Cyan
        Write-Host "  This takes 3-10 minutes and only happens once." -ForegroundColor Cyan
    } else {
        Write-Host "  Project created via Unity Hub." -ForegroundColor Green
    }
}

# ── Step 4: Inject game files from GitHub ─────────────────────────────────────

Write-Host ""
Write-Host "  [4/5] Downloading latest game files from GitHub..." -ForegroundColor Yellow

$ZIP_URL  = "https://github.com/$REPO_OWNER/$REPO_NAME/archive/refs/heads/$BRANCH.zip"
$TEMP_ZIP = "$env:TEMP\downtown-devour-setup.zip"
$TEMP_DIR = "$env:TEMP\downtown-devour-setup-extract"

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

Write-Host "  Download complete. Extracting..." -ForegroundColor Green

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
    Write-Host "  Top-level folder: $($TOP.FullName)" -ForegroundColor Gray
    Read-Host "  Press Enter to close"
    exit 1
}

# Only inject Scripts, Scenes, Shaders — never overwrite URP pipeline assets
$INJECT_SUBFOLDERS = @("Assets\Scripts", "Assets\Scenes", "Assets\Shaders")

foreach ($sub in $INJECT_SUBFOLDERS) {
    $srcFolder = Join-Path $SOURCE $sub
    $dstFolder = Join-Path $PROJECT_DIR $sub
    if (Test-Path $srcFolder) {
        if (-not (Test-Path $dstFolder)) {
            New-Item -ItemType Directory -Path $dstFolder -Force | Out-Null
        }
        Copy-Item -Path "$srcFolder\*" -Destination $dstFolder -Recurse -Force
        Write-Host "    Injected: $sub" -ForegroundColor Gray
    }
}

# Clean up temp files
Remove-Item $TEMP_ZIP -Force -ErrorAction SilentlyContinue
Remove-Item $TEMP_DIR -Recurse -Force -ErrorAction SilentlyContinue

Write-Host "  Game files installed." -ForegroundColor Green

# ── Step 5: Add project to Unity Hub and open it ─────────────────────────────

Write-Host ""
Write-Host "  [5/5] Opening Unity Hub..." -ForegroundColor Yellow

# Register the project with Unity Hub (Hub automatically detects projects added this way)
$addArgs = @("--", "--headless", "add-project", "--path", $PROJECT_DIR)
try {
    Start-Process -FilePath $HUB_EXE -ArgumentList $addArgs -Wait -WindowStyle Hidden
} catch {
    # Non-fatal — user can add manually
}

# Open Unity Hub so the user can click the project
Start-Process -FilePath $HUB_EXE
Write-Host "  Unity Hub launched." -ForegroundColor Green

# ── Done ──────────────────────────────────────────────────────────────────────

Write-Host ""
Write-Host "  ============================================================" -ForegroundColor Cyan
Write-Host "  Setup complete!  Project is at:" -ForegroundColor Green
Write-Host "  $PROJECT_DIR" -ForegroundColor White
Write-Host "  ============================================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "  Next steps:" -ForegroundColor Cyan
Write-Host "  1. In Unity Hub, click 'DowntownDevour' to open it." -ForegroundColor White
Write-Host "     (If it's not listed, click Add -> browse to the folder above)" -ForegroundColor Gray
Write-Host "  2. Wait for Unity to import everything (progress bar at bottom)." -ForegroundColor White
Write-Host "     First open takes 3-10 minutes. Subsequent opens are fast." -ForegroundColor Gray
Write-Host "  3. In Unity: File -> Open Scene -> Assets/Scenes/Game" -ForegroundColor White
Write-Host "  4. Press the Play button (triangle at the top)." -ForegroundColor White
Write-Host "  5. Move your hole with the mouse or WASD." -ForegroundColor White
Write-Host ""
Write-Host "  For future code updates, run update-project.ps1 instead." -ForegroundColor Gray
Write-Host ""
Read-Host "  Press Enter to close"
