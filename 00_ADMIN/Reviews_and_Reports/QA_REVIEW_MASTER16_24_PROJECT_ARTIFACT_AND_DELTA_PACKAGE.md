# QA Review - Master 16.24 Project Artifact And Delta Package

Date: 2026-05-21

## Scope

- Remove the archive/achievement spider map from the player-facing How to Play popup.
- Preserve the spider map as a project artifact at `00_ADMIN/Requirements/ARCHIVE_ACHIEVEMENT_SPIDER_MAP.md`.
- Establish changed-files-only GoDaddy delta packages as the default manual upload artifact.

## Changed Runtime Files

- `10_SOURCE/Masters/Master 16/index.html`
- `10_SOURCE/Masters/Master 16/how-to-play.html`
- `10_SOURCE/Masters/Master 16/js/main.js`
- `40_RELEASE/Website_Publish_Package/holesy/index.html`
- `40_RELEASE/Website_Publish_Package/holesy/how-to-play.html`
- `40_RELEASE/Website_Publish_Package/holesy/js/main.js`

## Removed Runtime Asset

- `assets/images/archive-achievement-spider.svg`

This file was intentionally removed from the source and release game packages because the spider diagram is project documentation, not player-facing game UI.

## GoDaddy Delta Package

Default manual upload package:

- `C:\Users\KentLefner\Downloads\holesy-godaddy-delta-master-16.24-from-16.23\holesy\`

Delta package files:

- `index.html`
- `how-to-play.html`
- `js/main.js`

Live-site cleanup note:

- delete `holesy/assets/images/archive-achievement-spider.svg` from GoDaddy if it exists

Full package remains available for clean resync or rollback:

- `C:\Users\KentLefner\Downloads\holesy-godaddy-upload-master-16.24\holesy\`

## Validation

- Passed: source and release `index.html` share the `Master 16.24` build label.
- Passed: `js/main.js` uses `BUILD_SUB = 24`, so the in-game build badge resolves to `Master 16.24` at runtime.
- Passed: source and release How to Play files no longer include the `Archive Map` navigation link, section, or spider SVG reference.
- Passed: the spider map exists as a governed project artifact instead of a game asset.
- Passed: the delta package contains only the runtime files changed from `Master 16.23` to `Master 16.24`, preserving relative paths.
- Passed: the release package remains modular; `index.html` is still only the entry point.
- Passed: module-aware syntax check via a temporary `.mjs` copy of `js/main.js`.
- Passed: local browser smoke at `http://127.0.0.1:8791/index.html?v=16.24-project-artifact-delta-3` showed build `Master 16.24`, four mode options, and no `Archive Map` text.
- Passed: local browser smoke at `http://127.0.0.1:8791/how-to-play.html?v=16.24-project-artifact-delta` showed build `Master 16.24`, `Game Summary` present, and `Archive Map` absent.

## Residual Risk

The changed-files-only delta package assumes the live GoDaddy site already has all unchanged files from the previous approved master. If the live site is missing older modular files, use the full package once to resync before returning to deltas.
