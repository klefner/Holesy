# QA Review - Master 16.37 Save, Audio, And Voxel Tuning

Date: 2026-05-30
Branch: codex/publish-master4-structure
Source: 10_SOURCE/Masters/Master 16/
Build label: Master 16.37

## Scope

- Repair Endless Save Game failure reported after medium-office voxel expansion.
- Apply the same short sparse audio treatment from medium-office cube impacts to skyscraper collapse and chunk sounds.
- Tune medium-office voxel fall timing so lower cubes enter the hole promptly and upper cubes descend faster with more sideways debris spread.

## Changes Reviewed

- Endless object serialization now saves compact type-specific fields instead of every possible runtime field for every object.
- Skyscraper collapse and chunk audio now use short, rate-limited impact voices keyed by stack instead of layered long building samples.
- Medium-office voxel gravity, terminal velocity, support delay, teeter duration, consume gravity, and lateral kick were increased/tightened to reduce floating and footprint-only debris piles.

## Validation

- `node --check 10_SOURCE/Masters/Master 16/js/main.js` passed.
- Release package files were refreshed from the modular source package.
- Source and release file hashes were compared for changed runtime files.
- Browser smoke test target: http://127.0.0.1:8798/index.html?v=16.37-save-audio-voxel

## Remaining Human Regression Focus

- Confirm Pause > Save Endless shows the pause-menu save confirmation and no browser storage error.
- Confirm skyscraper collapse sound no longer turns into long layered static.
- Confirm medium-office cubes feel faster than Master 16.36 without returning to the unplayable performance state.
- Confirm medium-office debris spreads beyond a perfect original footprint while still reading as falling cubes, not noisy particles.

## User Validation

2026-06-01: User reported "TEST PASSED" and separately confirmed game loading passed.

Closed validation points:

- Save Game: passed.
- Load Game: passed.
- Medium building sound: passed.
- Skyscraper sound treatment: accepted unless future play reveals otherwise.
- Medium cube fall tuning: accepted for this slice.
