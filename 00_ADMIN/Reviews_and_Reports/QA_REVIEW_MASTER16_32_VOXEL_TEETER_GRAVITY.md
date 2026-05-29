# QA Review - Master 16.32 Voxel Teeter Gravity

Date: 2026-05-29

Scope:

- Promote `Master 16.32` as the next modular `Master 16` patch.
- Refine medium-office voxel behavior so falling cubes feel slower, more physical, and less like instant vertical drops.
- Preserve the modular release package shape and changed-files-only GoDaddy delta convention.

Changes Reviewed:

- Added dedicated configurable voxel gravity, terminal velocity, ground-roll retention, and impact impulse settings.
- Added column teeter/lean behavior before voxel release; columns can visibly wobble without always collapsing.
- Upper cubes inherit sideways velocity from the leaning column and accelerate according to fall distance.
- Faster falling cubes transfer more momentum to nearby objects, rolls, bounces, and spin.
- Updated build labels and governed source/release/package documentation from `Master 16.31` to `Master 16.32`.

Validation Required:

- Confirm medium-office columns no longer drop too fast in normal play.
- Confirm some touched columns teeter or lean before falling.
- Confirm cubes that miss the hole remain visible as settled debris rather than disappearing.
- Confirm faster/higher cube falls create stronger movement, spin, and object impact than lower cube falls.
- Confirm skyscraper and house behavior remain distinct from medium-office voxel behavior.

Validation Completed In This Slice:

- Static module syntax check for source and release `js/main.js`.
- Source-to-release hash parity checks for changed modular files.
- Local browser smoke against the release package.

Residual Risk:

- This remains a lightweight custom physics approximation, not a full rigid-body physics engine. User feel-validation remains required for whether the teeter, gravity, and impact tuning is satisfying enough before deeper physics-library work is considered.
