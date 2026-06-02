# QA Review - Master 16.44 Voxel Hole-Miss Cleanup

Date: 2026-06-02

## Scope

Regression repair for medium-office voxel cubes that began falling near a hole and then continued falling visibly outside the hole after the hole moved away.

## User Evidence

- `Master 16.43` still showed medium-office cubes below/outside the live hole continuing to descend after the hole left the area.
- Screenshot evidence showed cubes behaving like an independent falling column rather than either falling visibly inside the hole or settling as ground debris.

## Root Cause

`Master 16.43` masked only objects already in the formal `falling` / swallowed-object descent path. Medium-office cubes can also be active `stackActive` voxel physics pieces before they become formally swallowed, so that path could keep falling outside the live mouth.

## Fix

- Added a shared live-mouth boundary helper for hole visibility checks.
- Added active voxel miss detection before per-frame voxel gravity/motion.
- When an active medium-office cube has started falling but its source hole no longer covers its footprint, it now settles as ordinary ground debris instead of continuing an orphaned descent.

## Validation

- Static syntax checks required for changed modules.
- Browser smoke required at `http://127.0.0.1:8798/index.html?v=16.44-voxel-hole-miss`.
- User regression should specifically confirm that medium-office cubes no longer keep falling visibly outside the hole after the hole moves away.

## Risk

- Medium-office cube behavior is physics-sensitive. Watch for cubes settling too abruptly when the player barely clips a column, but prefer that over the visible outside-hole falling defect.
