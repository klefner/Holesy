# ============================================================
#  Downtown Devour - Web Deployer
#  Copies a Unity Web build to docs/ and pushes to GitHub Pages.
#  Run AFTER building in Unity: Build Profiles -> Build
# ============================================================

$REPO_OWNER = "klefner"
$REPO_NAME  = "Holesy"
$BRANCH     = "claude/happy-clarke-ORWAI"
$PAGES_URL  = "https://$REPO_OWNER.github.io/$REPO_NAME/"

Clear-Host
Write-Host ""
Write-Host "  Downtown Devour - Web Deployer" -ForegroundColor Cyan
Write-Host "  ================================" -ForegroundColor Cyan
Write-Host ""

[Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Tls12

# ------ Locate Unity Web build ------------------------------------------------------------------------------------------------------------------------------------------------------------

Write-Host "  Enter the path to your Unity Web build folder." -ForegroundColor Yellow
Write-Host "  (The folder containing index.html and a Build subfolder)" -ForegroundColor Gray
Write-Host "  Example: C:\holesy\WebBuild" -ForegroundColor Gray
Write-Host ""
$BUILD_DIR = Read-Host "  Build folder path"
$BUILD_DIR = $BUILD_DIR.Trim('"').Trim("'")

if (-not (Test-Path (Join-Path $BUILD_DIR "index.html"))) {
    Write-Host ""
    Write-Host "  ERROR: index.html not found in: $BUILD_DIR" -ForegroundColor Red
    Write-Host "  Make sure you built the Web platform in Unity first." -ForegroundColor Red
    Read-Host "  Press Enter to close"
    exit 1
}

Write-Host "  Build found." -ForegroundColor Green

# ------ Locate local git repo ---------------------------------------------------------------------------------------------------------------------------------------------------------------

$REPO_ROOTS = @(
    "C:\holesy",
    "C:\holesy-repo",
    "$env:USERPROFILE\holesy",
    "$env:USERPROFILE\Holesy"
)

$REPO_DIR = $null
foreach ($root in $REPO_ROOTS) {
    if (Test-Path (Join-Path $root ".git")) {
        $REPO_DIR = $root
        break
    }
}

if (-not $REPO_DIR) {
    Write-Host ""
    Write-Host "  Local repo not found in common locations." -ForegroundColor Yellow
    Write-Host "  Enter the path to your local Holesy repo (must contain .git):" -ForegroundColor Yellow
    Write-Host "  (Or press Enter to clone it now)" -ForegroundColor Gray
    $input = Read-Host "  Repo path"
    $input = $input.Trim()

    if ($input -eq "") {
        Write-Host "  Cloning repo..." -ForegroundColor Yellow
        $REPO_DIR = "C:\holesy-repo"
        git clone "https://github.com/$REPO_OWNER/$REPO_NAME.git" $REPO_DIR
        if ($LASTEXITCODE -ne 0) {
            Write-Host "  ERROR: Clone failed. Check your internet and GitHub credentials." -ForegroundColor Red
            Read-Host "  Press Enter to close"
            exit 1
        }
    } else {
        $REPO_DIR = $input
        if (-not (Test-Path (Join-Path $REPO_DIR ".git"))) {
            Write-Host "  ERROR: No .git folder found at: $REPO_DIR" -ForegroundColor Red
            Read-Host "  Press Enter to close"
            exit 1
        }
    }
}

Write-Host "  Repo: $REPO_DIR" -ForegroundColor Green

# ------ Check git ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

try { git --version 2>&1 | Out-Null }
catch {
    Write-Host ""
    Write-Host "  ERROR: git not found. Install from https://git-scm.com" -ForegroundColor Red
    Read-Host "  Press Enter to close"
    exit 1
}

# ------ Sync and prepare branch ---------------------------------------------------------------------------------------------------------------------------------------------------------

Push-Location $REPO_DIR

Write-Host "  Checking out branch $BRANCH ..." -ForegroundColor Yellow
git fetch origin $BRANCH 2>&1 | Out-Null
git checkout $BRANCH 2>&1 | Out-Null
git pull origin $BRANCH 2>&1 | Out-Null

# ------ Copy build to docs/ ---------------------------------------------------------------------------------------------------------------------------------------------------------------------

$DOCS_DIR = Join-Path $REPO_DIR "docs"

Write-Host "  Copying build to docs/ ..." -ForegroundColor Yellow

if (Test-Path $DOCS_DIR) { Remove-Item $DOCS_DIR -Recurse -Force }
New-Item -ItemType Directory -Path $DOCS_DIR -Force | Out-Null
Copy-Item -Path "$BUILD_DIR\*" -Destination $DOCS_DIR -Recurse -Force

# Required: tells GitHub Pages not to run Jekyll on the files
New-Item -ItemType File -Path (Join-Path $DOCS_DIR ".nojekyll") -Force | Out-Null

Write-Host "  Build copied." -ForegroundColor Green

# ------ Commit and push ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

Write-Host "  Pushing to GitHub..." -ForegroundColor Yellow

git add docs/ 2>&1 | Out-Null
$timestamp = Get-Date -Format "yyyy-MM-dd HH:mm"
git commit -m "Deploy Web build $timestamp" 2>&1 | Out-Null

if ($LASTEXITCODE -ne 0 -and $LASTEXITCODE -ne 1) {
    Write-Host "  WARNING: commit may have failed (exit $LASTEXITCODE)" -ForegroundColor Yellow
}

git push -u origin $BRANCH 2>&1
if ($LASTEXITCODE -ne 0) {
    Write-Host ""
    Write-Host "  ERROR: Push failed. Make sure you have push access and are logged in." -ForegroundColor Red
    Write-Host "  You may need to run: git config --global credential.helper manager" -ForegroundColor Gray
    Pop-Location
    Read-Host "  Press Enter to close"
    exit 1
}

Pop-Location

# ------ Done ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

Write-Host ""
Write-Host "  Deployed!" -ForegroundColor Green
Write-Host ""
Write-Host "  Live at: $PAGES_URL" -ForegroundColor Cyan
Write-Host ""
Write-Host "  GitHub Pages takes 1-2 minutes to update after a push." -ForegroundColor Gray
Write-Host ""
Read-Host "  Press Enter to close"
