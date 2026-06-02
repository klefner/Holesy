# QA Review - Master 16.40 Hole Descent Paths

Date: 2026-06-02
Build label: Master 16.40

## Scope

Implemented same-side hole descent paths so objects falling into a hole preserve their actual mouth contact point and drift downward toward a lower same-side point instead of snapping to a center drain.

## Changes Reviewed

- Added centralized `HOLE_DESCENT_CONFIG`.
- Added `configureHoleDescentPath()` for per-object hole-entry and lower-mouth targets.
- Updated `beginConsume()` to assign an entry path at consume start.
- Updated the falling-object loop to use depth-based interpolation from entry point to same-side lower-mouth point.
- Preserved voxel missed-hole checks against the original entry point.
- Extended Endless save/load object state with active fall-entry and fall-bottom coordinates.
- Removed residual DOM assignment from `js/build-info.js` so the build metadata module remains data-only.

## Validation

- `node --check` passed for source and release `js/main.js`.
- `node --check` passed for source and release `js/build-info.js`.
- Source, release, and GoDaddy delta hashes match for changed runtime files.
- Chrome CDP smoke passed at `http://127.0.0.1:8798/index.html?v=16.40-hole-descent`:
  - build label displays `Master 16.40`
  - mode selection changes to Waves
  - Begin hides the menu overlay
  - HUD, mini scoreboard, and game controls appear
  - no runtime errors were reported

## User Validation Needed

- Confirm objects visually appear to fall down into the hole from their contact point rather than being pulled into the center.
- Confirm medium-office cubes still feel acceptable after the descent-path change.
- Confirm no new save/load regression appears during an Endless run with active falling objects.
