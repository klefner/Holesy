# QA Review - Master 16.38 PERF-012 Phase 2

Date: 2026-06-01
Branch: codex/publish-master4-structure
Source: 10_SOURCE/Masters/Master 16/
Build label: Master 16.38

## Scope

- Complete the next low-risk `PERF-012` modular extraction slice without gameplay behavior changes.
- Move stable build metadata, patch notes, difficulty profiles, and Archive lore data out of `js/main.js`.
- Refresh the modular release package and changed-files-only GoDaddy delta package.

## Changes Reviewed

- Added `js/build-info.js` for `BUILD_LABEL` and `BUILD_CHANGELOG`.
- Added `js/difficulty-profiles.js` for difficulty profile configuration.
- Added `data/lore-documents.js` for Archive lore documents and starter Field Pattern unlocks.
- Updated `js/main.js` to import those modules instead of carrying the data inline.
- Bumped source, manual, and release package cache/version labels to `Master 16.38`.

## Validation

- `node --check 10_SOURCE/Masters/Master 16/js/main.js` passed.
- `node --check 10_SOURCE/Masters/Master 16/js/build-info.js` passed.
- `node --check 10_SOURCE/Masters/Master 16/js/difficulty-profiles.js` passed.
- `node --check 10_SOURCE/Masters/Master 16/data/lore-documents.js` passed.
- Release package files refreshed from the modular source package.
- Source and release file hashes compared for changed files.
- Static modular smoke passed from local release server:
  - `http://127.0.0.1:8798/index.html?v=16.38-perf012-phase2`
  - verified `index.html`, `js/main.js`, `js/build-info.js`, `js/difficulty-profiles.js`, `data/lore-documents.js`, `how-to-play.html`, and the How to Play summary image all return HTTP 200.
  - verified `Master 16.38` appears in the entry point and build notes.

## Human Regression Focus

- Confirm the title screen loads and displays `Master 16.38`.
- Confirm Begin starts the selected mode.
- Confirm Archive opens and starter Field Patterns plus recovered documents still render.
- Confirm How to Play opens and the summary image still renders.
- Confirm build notes open from the bottom-left version button and include `Master 16.38`.
