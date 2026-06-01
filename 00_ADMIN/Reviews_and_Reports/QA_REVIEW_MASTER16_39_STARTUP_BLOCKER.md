# QA Review - Master 16.39 Startup Blocker

Date: 2026-06-01
Branch: codex/publish-master4-structure
Source: 10_SOURCE/Masters/Master 16/
Build label: Master 16.39

## Defect

`Master 16.38` loaded the title/menu screen, but game-mode options did not change selected mode and the Begin button did not start a game.

## Root Cause

The `PERF-012` Phase 2 extraction boundary for `BUILD_CHANGELOG` was too broad. It moved renderer/game setup code and the build-version DOM bindings into `js/build-info.js`. Because `js/build-info.js` executed before `main.js` defined runtime helpers, module startup failed before menu listeners were usable.

## Fix

- Rebuilt the extraction from the known-good `Master 16.37` runtime with corrected boundaries.
- Kept renderer, scene, build-version DOM binding, and gameplay setup in `js/main.js`.
- Kept only build metadata and build changelog data in `js/build-info.js`.
- Promoted the blocker repair as `Master 16.39`.

## Validation

- `node --check 10_SOURCE/Masters/Master 16/js/main.js` passed.
- `node --check 10_SOURCE/Masters/Master 16/js/build-info.js` passed.
- `node --check 10_SOURCE/Masters/Master 16/js/difficulty-profiles.js` passed.
- `node --check 10_SOURCE/Masters/Master 16/data/lore-documents.js` passed.
- Headless Chrome interaction test against local release URL confirmed:
  - build label displays `Master 16.39`.
  - boot marker reaches `menu-listeners-ready`.
  - selecting `Endless Waves` changes `.mode-option.selected` to `endless`.
  - clicking Begin hides the overlay and shows the HUD.
  - no relevant runtime exceptions after the fix.

## Local Test URL

`http://127.0.0.1:8798/index.html?v=16.39-startup-fix`
