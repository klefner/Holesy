# QA Review: Master 16.58 Government Building Touch-Crash Fix

Date: 2026-06-07

## Scope

Repair the crash observed when the player hole first touches a government building.

## Root Cause

`activateGovernmentBuildingFromPiece()` called `playVoxelCubeImpact()`, but no helper with that name exists in the modular runtime. The existing budgeted voxel impact gateway is `playVoxelCubeImpactSound()`.

## Change

- Replaced the undefined helper call with `playVoxelCubeImpactSound(obj, 0.85)` in source and release `js/main.js`.
- Promoted build metadata to `Master 16.58`.
- Updated current basis and build changelog documentation.

## Validation

- `node --check` on source `js/main.js`
- `node --check` on source `js/build-info.js`
- `node --check` on release `js/main.js`
- `node --check` on release `js/build-info.js`
- Static grep confirms no remaining `playVoxelCubeImpact(` calls.
- HTTP smoke confirms the release package serves `index.html` with status 200.
- HTTP smoke confirms release `js/build-info.js` reports `BUILD_SUB = 58` and `Master 16.58`.

## Notes

This is a surgical crash fix. It does not change government-building physics behavior, visuals, scoring, save/load, or the separate government-building module.

Playwright browser automation was unavailable in the local Node runtime because the bundled `playwright-core` package could not be resolved, so validation used syntax/static checks and direct HTTP smoke checks.
