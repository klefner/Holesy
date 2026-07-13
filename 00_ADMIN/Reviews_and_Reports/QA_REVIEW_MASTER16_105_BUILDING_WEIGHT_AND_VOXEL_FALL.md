# Quality Inspection Report: Master 16.105 Building Weight and Voxel Fall

Date: 2026-06-30

## Scope

Repair building collapse feel after player feedback that building objects still felt too light and that medium-building cubes could hang in the air, then appear on the ground.

## Changes Reviewed

- Increased shared stack gravity, voxel gravity, voxel terminal velocity, and voxel consume gravity.
- Reduced artificial medium-building voxel hop, upward release impulse, teeter duration, release delay, bounce, and ground roll retention.
- Reduced non-voxel skyscraper chunk upward lift so collapse debris reads heavier.
- Reduced legacy government physics lift, hop, restitution, and ground damping while increasing gravity and max fall speed.
- Gated the medium-building missed-hole settle path so it only snaps a cube to its floor after the cube is already at ground height.
- Bumped the governed source and release package label to `Master 16.105`.

## Verification

- Passed: JavaScript syntax checks for source and release `js/main.js`, `js/government-physics.js`, and `js/build-info.js`.
- Passed: source/release SHA-256 parity for `index.html`, `PACKAGE_MANIFEST.md`, `css/styles.css`, `how-to-play.html`, `js/main.js`, `js/build-info.js`, and `js/government-physics.js`.
- Passed: local HTTP smoke at `http://127.0.0.1:4175/index.html?fresh=16.105` returned status 200 and served `Master 16.105` plus `js/main.js?v=16.105`.
- Passed: Chrome-channel browser smoke loaded the title screen, displayed `Master 16.105`, found visible mode-selection buttons, and reported no console or page errors.

## Notes

This is a narrow physics-feel repair. It does not change scoring, Run Goals, Mandates, mode selection rules, save format, boss behavior, or release architecture.
