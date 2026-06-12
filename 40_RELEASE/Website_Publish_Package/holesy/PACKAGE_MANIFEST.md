# Holesy Modular Package Manifest

Build label: `Master 16.74`

Purpose: enumerate the complete governed modular package so release and upload work no longer treats `index.html` as the whole game.

## Required Package Files

- `index.html`
- `how-to-play.html`
- `PACKAGE_MANIFEST.md`
- `css/styles.css`
- `js/main.js`
- `js/build-info.js`
- `js/difficulty-profiles.js`
- `js/government-physics.js`
- `data/lore-documents.js`
- `assets/images/how-to-play-game-summary.svg`

## Required Package Directories

- `assets/audio/`
- `assets/images/`
- `css/`
- `data/`
- `js/`

## Upload Rule

- Clean install or live drift recovery: upload the full governed `holesy/` folder with this structure intact.
- Routine update when live is already on the prior approved master: upload a changed-files-only delta that preserves these same relative paths under `/holesy/`.
- Removed live files must be deleted manually; a delta upload cannot remove old live files by itself.

## PERF-012 Completion Evidence

- Phase 1 package shape is complete: entry HTML, stylesheet, JS modules, assets, and data are separate files in source and release.
- Phase 2 low-risk extraction is complete: build metadata, difficulty profiles, lore documents, starter unlock data, and government-building physics are external modules.
- Source and release package paths mirror this manifest for the current governed baseline.
