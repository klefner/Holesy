# QA Review - Master 16.31 Voxel Support And Jams

Date: 2026-05-29

## Scope

- Refine medium-office voxel collapse so upper cubes do not fall immediately when the bottom of a column is triggered.
- Add 3D cube separation for active medium-office pieces so falling cubes push apart instead of visually occupying the same space.
- Add generic oversized-object jam behavior for future large cubes that exceed the visible hole diameter.

## Implementation Notes

- `Master 16.31` keeps the modular source package under `10_SOURCE/Masters/Master 16/`.
- Medium-office bottom cubes release first; upper cubes wait until the support below is falling, consumed, missing, or has dropped far enough to count as failed support.
- Released cubes continue using gravity acceleration, damping, bounce, and settle behavior from the existing stack physics system.
- Oversized objects that are inside a hole but too large to fit can become jammed, drag with the hole, and eject once impacts from other objects accumulate enough mass.

## Validation

- Static syntax check required for `10_SOURCE/Masters/Master 16/js/main.js`.
- Release-package source parity required for changed modular files.
- Browser smoke required to confirm `Master 16.31` loads and begins gameplay.

## Remaining User Validation Focus

- Medium-office upper cubes should feel like support failed before they fall.
- Falling cubes should collide and separate without obvious overlap.
- Future or debug oversized cubes should visibly jam and then eject after enough smaller-object impacts.
- Watch performance on mobile because procedural offices can still generate high cube counts.
