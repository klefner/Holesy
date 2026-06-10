# ============================================================
#  Downtown Devour - WebGL Deploy to GitHub Pages
#  Run AFTER doing a WebGL build in Unity.
#  Requirements: Git for Windows (https://git-scm.com/download/win)
# ============================================================

param([string]$BuildPath = "")

$REPO_URL    = "https://github.com/klefner/holesy.git"
$BRANCH      = "claude/happy-clarke-ORWAI"
$REPO_ROOTS  = @("C:\holesy-repo", "C:\holesy", "$env:USERPROFILE\holesy")

Clear-Host
Write-Host ""
Write-Host "  Downtown Devour - WebGL Deploy" -ForegroundColor Cyan
Write-Host "  ================================" -ForegroundColor Cyan
Write-Host ""

[Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Tls12
$ProgressPreference = "SilentlyContinue"

# ── 1. Check git ─────────────────────────────────────────────────────────────
if (-not (Get-Command git -ErrorAction SilentlyContinue)) {
    Write-Host "  Git is not installed." -ForegroundColor Yellow
    Write-Host ""
    Write-Host "  Install it now:" -ForegroundColor White
    Write-Host "  1. Go to: https://git-scm.com/download/win" -ForegroundColor Gray
    Write-Host "  2. Download and run the installer (click through all defaults)" -ForegroundColor Gray
    Write-Host "  3. Close and reopen PowerShell, then run this script again" -ForegroundColor Gray
    Write-Host ""
    Read-Host "  Press Enter to close"
    exit 1
}

# ── 2. Find WebGL build folder ────────────────────────────────────────────────
if (-not $BuildPath) {
    Write-Host "  Enter the path to your Unity WebGL build output folder." -ForegroundColor Yellow
    Write-Host "  (The folder containing index.html and a Build subfolder.)" -ForegroundColor Gray
    Write-Host "  Example: C:\Users\KentLefner\Desktop\DowntownDevour-WebGL" -ForegroundColor Gray
    Write-Host ""
    $BuildPath = Read-Host "  WebGL build path"
}

if (-not (Test-Path (Join-Path $BuildPath "index.html"))) {
    Write-Host ""
    Write-Host "  ERROR: index.html not found at: $BuildPath" -ForegroundColor Red
    Write-Host "  Make sure you point to the root of the WebGL build output." -ForegroundColor Gray
    Read-Host "  Press Enter to close"
    exit 1
}

Write-Host "  Build folder: $BuildPath" -ForegroundColor Green

# ── 3. Find or clone the repo ─────────────────────────────────────────────────
$REPO_DIR = $null
foreach ($r in $REPO_ROOTS) {
    if (Test-Path (Join-Path $r ".git")) { $REPO_DIR = $r; break }
}

if (-not $REPO_DIR) {
    Write-Host ""
    Write-Host "  Cloning the holesy repo to C:\holesy-repo ..." -ForegroundColor Yellow
    git clone $REPO_URL "C:\holesy-repo" 2>&1
    if (Test-Path "C:\holesy-repo\.git") {
        $REPO_DIR = "C:\holesy-repo"
        Write-Host "  Cloned." -ForegroundColor Green
    } else {
        Write-Host ""
        Write-Host "  Clone failed. You may need to authenticate with GitHub." -ForegroundColor Red
        Write-Host "  Try running this in PowerShell, then re-run this script:" -ForegroundColor Gray
        Write-Host "    git clone $REPO_URL C:\holesy-repo" -ForegroundColor White
        Read-Host "  Press Enter to close"
        exit 1
    }
}

Write-Host "  Repo folder:  $REPO_DIR" -ForegroundColor Green

# ── 4. Pull latest ────────────────────────────────────────────────────────────
Write-Host ""
Write-Host "  Pulling latest code..." -ForegroundColor Yellow
Set-Location $REPO_DIR
git fetch origin $BRANCH 2>&1 | Out-Null
git checkout $BRANCH 2>&1 | Out-Null
git pull origin $BRANCH 2>&1 | Out-Null
Write-Host "  Up to date." -ForegroundColor Green

# ── 5. Copy WebGL build into docs/ ───────────────────────────────────────────
$DOCS_DIR = Join-Path $REPO_DIR "docs"
Write-Host ""
Write-Host "  Copying WebGL build to docs/ ..." -ForegroundColor Yellow

if (-not (Test-Path $DOCS_DIR)) { New-Item -ItemType Directory -Path $DOCS_DIR -Force | Out-Null }

# Remove old build files (keep .nojekyll)
Get-ChildItem $DOCS_DIR | Where-Object { $_.Name -ne ".nojekyll" } | Remove-Item -Recurse -Force

# Copy new build
Copy-Item -Path "$BuildPath\*" -Destination $DOCS_DIR -Recurse -Force

# Ensure .nojekyll is present (prevents GitHub Pages from running Jekyll)
New-Item -ItemType File -Path (Join-Path $DOCS_DIR ".nojekyll") -Force | Out-Null

Write-Host "  Copied." -ForegroundColor Green

# ── 6. Commit and push ────────────────────────────────────────────────────────
Write-Host ""
Write-Host "  Pushing to GitHub..." -ForegroundColor Yellow

git add docs/
$stamp = Get-Date -Format "yyyy-MM-dd HH:mm"
git commit -m "Deploy WebGL build to GitHub Pages ($stamp)"
git push origin $BRANCH

# ── 7. Done ───────────────────────────────────────────────────────────────────
Write-Host ""
Write-Host "  Deployed!" -ForegroundColor Green
Write-Host ""
Write-Host "  Your game will be live in about 60 seconds at:" -ForegroundColor Cyan
Write-Host "  https://klefner.github.io/holesy/" -ForegroundColor White
Write-Host ""
Write-Host "  First time? You still need to enable GitHub Pages:" -ForegroundColor Yellow
Write-Host "  1. Go to: https://github.com/klefner/holesy/settings/pages" -ForegroundColor Gray
Write-Host "  2. Under Branch, select: claude/happy-clarke-ORWAI" -ForegroundColor Gray
Write-Host "  3. Set folder to: /docs" -ForegroundColor Gray
Write-Host "  4. Click Save" -ForegroundColor Gray
Write-Host ""
Read-Host "  Press Enter to close"
