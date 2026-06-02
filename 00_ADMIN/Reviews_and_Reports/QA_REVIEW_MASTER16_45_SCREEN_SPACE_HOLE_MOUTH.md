# QA Review - Master 16.45 Screen-Space Hole Mouth

Date: 2026-06-02

## Scope

Follow-up repair after the `Master 16.44` active-voxel miss cleanup did not visibly resolve medium-office cubes continuing to fall outside the black hole.

## User Evidence

- User tested `Master 16.44` and reported no noticeable change.
- Screenshot showed objects below/outside the black disk continuing to fall after the hole left the area.

## Root Cause Update

The `Master 16.44` repair still used a ground-space mouth/radius test. With the angled camera, a cube can remain inside the world-space radius while its rendered position appears outside the visible black disk.

## Fix

- Added projected screen-space mouth clipping for falling/hole-descent visibility.
- Formal swallowed-object descent now renders only while the mesh projects inside the visible hole mouth.
- Active medium-office voxel miss detection now uses the same projected visible-mouth test before allowing continued descent behavior.

## Validation

- Static syntax checks required for changed modules.
- Browser load check required at `http://127.0.0.1:8798/index.html?v=16.45-screen-mouth`.
- User regression should confirm objects no longer visibly continue descending outside the black disk after the hole moves away.

## Risk

- Projection-based clipping may hide a cube slightly earlier than a pure physics model would. That is acceptable for this slice because the player-facing defect is visual: objects must not appear to keep falling down a hole when they are no longer visibly inside that hole.
