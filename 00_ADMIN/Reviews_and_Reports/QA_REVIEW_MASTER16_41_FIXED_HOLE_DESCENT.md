# QA Review - Master 16.41 Fixed Hole Descent

Date: 2026-06-02
Build label: Master 16.41

## Scope

Refined the `Master 16.40` hole-descent behavior so devoured objects no longer read as moving into a larger center point. Objects now keep falling at the exact world-space mouth contact point and remain visible longer before being cleaned up deeper below the hole.

## Changes Reviewed

- Changed `HOLE_DESCENT_CONFIG` to disable inward and tangent drift.
- Falling objects now hold their fixed entry `x/z` while descending.
- Non-voxel objects now shrink only after they are already deep below the mouth.
- Falling objects are removed at a deeper threshold instead of disappearing shortly after crossing the surface.
- Save/load now preserves the fixed entry point without storing obsolete lower-mouth targets.

## Validation

- `node --check` passed for source and release `js/main.js`.
- `node --check` passed for source and release `js/build-info.js`.
- Source, release, and GoDaddy delta hashes match for changed runtime files.

## User Validation Needed

- Confirm objects appear to fall down the same spot where they entered the hole.
- Confirm objects stay visible long enough to feel like they are falling down a deep shaft.
- Confirm moving the hole away does not visually pull already-entered objects along with it.
