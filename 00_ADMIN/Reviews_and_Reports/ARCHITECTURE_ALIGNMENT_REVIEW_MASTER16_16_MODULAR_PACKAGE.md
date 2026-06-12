# Architecture Alignment Review: Master 16.16 To Master 16.17 Modular Client Split

Date: 2026-05-20

Reviewer: Codex

## Executive Finding

Holesy is committed to the accepted modular browser-client architecture. This review originally found that the `Master 16.16` production-test source and GoDaddy upload package were not aligned with that architecture yet because they remained bundled into one large HTML file.

`Master 16.17` completed the Phase 1 correction: the governed source, release package, and upload convenience folder now use the modular package shape with `index.html`, `css/styles.css`, `js/main.js`, `assets/`, and `data/`.

`Master 16.18` preserves that package shape and fixes the first startup regression found during regression testing: the menu rendered, but module execution halted before mode-selection and Begin listeners were wired.

`Master 16.38` completed the Phase 2 low-risk JS/data extraction by moving build metadata, difficulty profiles, and Archive lore/starter unlock data into ES modules.

`Master 16.74` closes `PERF-012` as a production package migration item by adding a governed package manifest to the source and release `holesy/` folders, refreshing release package guidance, and confirming the modular package baseline remains the release source of truth.

`index.html` is now the package entry point only. It must not be described as the entire game package or as a return to single-file architecture.

## Master 16.17 Update

Phase 1 is aligned:

- Governed source package: `10_SOURCE/Masters/Master 16/`
- Source entry point: `10_SOURCE/Masters/Master 16/index.html`
- Source stylesheet: `10_SOURCE/Masters/Master 16/css/styles.css`
- Source game module: `10_SOURCE/Masters/Master 16/js/main.js`
- Release package: `40_RELEASE/Website_Publish_Package/holesy/`
- Upload convenience package: `C:\Users\KentLefner\Downloads\holesy-godaddy-upload-master-16.18\holesy\`

Master 16.18 hotfix evidence:

- corrected DOM initialization order for the pause-version label
- removed custom global/window state writes from module startup
- verified Endless mode selection and Begin via the modular package URL

Remaining architecture work after PERF-012:

- `js/main.js` is still intentionally large, but that no longer blocks the production package migration.
- Future extraction of UI/archive, save/load, levels/theme, rewards/quests, and gameplay systems should be tracked as separate feature-support architecture items rather than reopening `PERF-012`.

## Governing Decision

Primary decision record:

- `00_ADMIN/Requirements/ARCHITECTURE_DECISION_MODULAR_CLIENT_SPLIT.md`

Accepted target:

```text
index.html
css/styles.css
js/main.js
js/gameLoop.js
js/player.js
js/enemies.js
js/levels.js
js/ui.js
js/saveSystem.js
assets/images/
assets/audio/
data/levels.json
```

Key constraints:

- keep the game client-side
- split the giant HTML file over time into browser-native assets
- do not replace the browser game loop with Python/server code for FPS/performance
- preserve governed candidate/master/release workflow during migration

## Original State Inspected

Current source basis:

- `10_SOURCE/Masters/Master 16.html`
- in-game label: `Master 16.16`

Current release package:

- `40_RELEASE/Website_Publish_Package/holesy/index.html`

Current GoDaddy convenience copy:

- `C:\Users\KentLefner\Downloads\holesy-godaddy-upload-master-16.16\holesy\index.html`

Current package shape:

- one bundled `index.html`
- no production `css/`, `js/`, `assets/`, or `data/` package directories yet
- imports Three.js from CDN through import map

## Already Aligned

- Architecture decision is accepted and documented.
- Product Intent Gate requires architecture decision review before release/package answers.
- Team Sync reads the architecture decision and backlog recommendation.
- Backlog contains `PERF-012 Incremental Modular Production Package Migration`.
- Candidate proof files already demonstrated low-risk modular extraction:
  - `Master 15.28 - modular-css-proof.css`
  - `Master 15.29 - build-info.js`
  - `Master 15.29 - difficulty-profiles.js`
  - `Master 15.30 - game-stats.js`
  - `Master 15.40` through `Master 15.45` external lore/stats/build CSS/JS proof artifacts
- Release package README already labels the current bundled package as temporary.

## Original Gaps Found

- `Master 16.16` source is still a giant HTML file.
- The website publish package is still a giant HTML file.
- The GoDaddy upload convenience folder is still a giant HTML file.
- Modular candidate assets are not yet promoted into the production package structure.
- Build/release guidance can still be misread as "upload only index.html" unless the answer explicitly distinguishes temporary artifact from committed architecture.
- Team Sync warns about modular architecture, but it does not yet make production modular migration the default next technical action loudly enough.

## Required Alignment Plan

### Phase 1: Production Package Skeleton - Completed In Master 16.17

- Create a production package directory with the committed target structure:
  - `index.html`
  - `css/styles.css`
  - `js/`
  - `assets/images/`
  - `assets/audio/`
  - `data/`
- Move the full inline stylesheet from `Master 16.16` into `css/styles.css`.
- Move the primary game module into `js/main.js`.
- Validate that CSS loads locally and on mobile.

### Phase 2: Low-Risk JS/Data Extraction - Completed In Master 16.38

- Extract build metadata to `js/build-info.js`.
- Extract difficulty profiles to `js/difficulty-profiles.js`.
- Extract lore documents to `data/lore-documents.js` or `data/lore-documents.json`, depending on the lowest-risk import path.
- Keep game loop, rendering, input, collision, audio, AI, and save/load inside `js/main.js` until smaller seams are proven.

### PERF-012 Closure - Completed In Master 16.74

- Source and release package folders preserve the modular browser-client structure.
- `PACKAGE_MANIFEST.md` now enumerates every required governed upload file and directory.
- The release package README distinguishes the full modular baseline from changed-files-only GoDaddy delta upload convenience.
- `index.html` remains the package entry point only.
- Future extraction work is still desirable, but it should be tracked as new architecture/product work, not as an open PERF-012 blocker.

### Future Phase: Gameplay System Modules

- Extract UI and archive code to `js/ui.js` / `js/archive.js`.
- Extract save/load to `js/saveSystem.js`.
- Extract wave config and level/theme definitions to `js/levels.js` and `data/levels.json`.
- Extract player/rival logic only after save/load and UI extraction are stable.

### Future Phase: Asset Discipline

- Move generated or embedded audio into `assets/audio/` when practical.
- Move future theme art, object sprites/textures, and world packs into `assets/images/` or dedicated theme folders.
- Keep `PACKAGE_MANIFEST.md` current whenever package files are added or removed.

## Future Chat Control

Every future chat must treat this as a committed architecture constraint:

- `index.html` is the package entry point only.
- `PERF-012` is complete as of `Master 16.74`; do not reopen it for general future modularization.
- Any release/package answer must identify the full package shape and whether it aligns with the modular target.
- A single-file production answer is incomplete unless the user explicitly approves a temporary exception for that specific release.

## Recommended Next Engineering Action

Return to product/gameplay intake or open a new named architecture item for the next extraction family:

- UI/archive extraction
- save/load extraction
- level/theme data extraction
- reward/quest data extraction
