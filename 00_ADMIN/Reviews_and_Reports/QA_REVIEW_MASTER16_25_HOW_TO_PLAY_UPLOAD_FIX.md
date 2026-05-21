# QA Review - Master 16.25 How To Play Upload Fix

Date: 2026-05-21

## Scope

- Fix live test finding where the `How to Play` menu control appeared as a plain shaded text link instead of matching the adjacent menu buttons.
- Fix live test finding where the How to Play summary image rendered as a broken image.
- Preserve the changed-files-only GoDaddy package rule while allowing missing live dependencies to be included in the delta.

## Runtime Changes

- Updated menu action styling to target `#menu-action-row button` so all action controls share the same button styling path.
- Promoted the build label to `Master 16.25`.
- Added `Master 16.25` build notes.

## GoDaddy Delta Package

Package:

- `C:\Users\KentLefner\Downloads\holesy-godaddy-delta-master-16.25-from-16.24\holesy\`

Files to upload:

- `index.html`
- `how-to-play.html`
- `css/styles.css`
- `js/main.js`
- `assets/images/how-to-play-game-summary.svg`

Additional cleanup note:

- delete `holesy/assets/images/archive-achievement-spider.svg` from GoDaddy if it exists

## Validation

- Passed: source and release files match by SHA-256 for the changed runtime files.
- Passed: `js/main.js` passed a module-aware syntax check through a temporary `.mjs` copy.
- Passed: local browser smoke confirmed the menu has four action buttons with equal computed height and the How to Play summary image loads with non-zero natural dimensions.
- Passed: local How to Play page reports `Master 16.25`.

## Residual Risk

The live site must receive `css/styles.css` and `assets/images/how-to-play-game-summary.svg` for this fix to appear in production. This is a dependency-repair exception to the normal changed-files-only delta rule because production testing proved the live site was missing an unchanged image asset.
