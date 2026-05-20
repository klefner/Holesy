# Architecture Alignment Review: Master 16.16 Modular Client Split

Date: 2026-05-20

Reviewer: Codex

## Executive Finding

Holesy is committed to the accepted modular browser-client architecture. The current `Master 16.16` production-test source and GoDaddy upload package are not aligned with that architecture yet because they remain bundled into one large HTML file.

The single-file `index.html` package is a temporary release artifact only. It must not be described as the project direction or as the normal production architecture.

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

## Current State Inspected

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

## Not Aligned

- `Master 16.16` source is still a giant HTML file.
- The website publish package is still a giant HTML file.
- The GoDaddy upload convenience folder is still a giant HTML file.
- Modular candidate assets are not yet promoted into the production package structure.
- Build/release guidance can still be misread as "upload only index.html" unless the answer explicitly distinguishes temporary artifact from committed architecture.
- Team Sync warns about modular architecture, but it does not yet make production modular migration the default next technical action loudly enough.

## Required Alignment Plan

### Phase 1: Production Package Skeleton

- Create a production package directory with the committed target structure:
  - `index.html`
  - `css/styles.css`
  - `js/`
  - `assets/images/`
  - `assets/audio/`
  - `data/`
- Move the full inline stylesheet from `Master 16.16` into `css/styles.css`.
- Keep JavaScript bundled inside `index.html` for this first production-scope slice.
- Validate that CSS loads locally and on mobile.

### Phase 2: Low-Risk JS/Data Extraction

- Extract build metadata to `js/build-info.js`.
- Extract difficulty profiles to `js/difficulty-profiles.js`.
- Extract lore documents to `data/lore-documents.js` or `data/lore-documents.json`, depending on the lowest-risk import path.
- Keep game loop, rendering, input, collision, audio, AI, and save/load inside `index.html` until smaller seams are proven.

### Phase 3: Gameplay System Modules

- Extract UI and archive code to `js/ui.js` / `js/archive.js`.
- Extract save/load to `js/saveSystem.js`.
- Extract wave config and level/theme definitions to `js/levels.js` and `data/levels.json`.
- Extract player/rival logic only after save/load and UI extraction are stable.

### Phase 4: Asset Discipline

- Move generated or embedded audio into `assets/audio/` when practical.
- Move future theme art, object sprites/textures, and world packs into `assets/images/` or dedicated theme folders.
- Add a package manifest that enumerates every required upload file before GoDaddy release.

## Future Chat Control

Every future chat must treat this as a committed architecture constraint:

- Current bundled package is a temporary exception.
- The default next architecture task is `PERF-012`.
- Any release/package answer must identify the full package shape and whether it aligns with the modular target.
- A single-file production answer is incomplete unless the user explicitly approves a temporary exception for that specific release.

## Recommended Next Engineering Action

Start `PERF-012` with Phase 1:

- externalize `Master 16.16` CSS into `40_RELEASE/Website_Publish_Package/holesy/css/styles.css`
- update `40_RELEASE/Website_Publish_Package/holesy/index.html` to reference it
- create matching source-side modular candidate artifacts
- validate browser/mobile startup and game mode selection
- update release manifest and QA evidence
