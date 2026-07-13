# QA Review - Master 16.93 Skyscraper Outward Debris

Date: 2026-06-23

Change:

- Corrected the skyscraper collapse-plan direction so chunks launch from the building center toward the hit/source side.
- Removed the reversed inward shove that made large skyscraper debris appear to arc out and then return toward the footprint.
- Preserved the existing chaotic lateral scatter, medium-office voxel containment, and temporary `Master 16.91` tank-testing visibility.

Implementation Note:

- This is a narrow `js/main.js` collapse-vector fix. The previous vector used the source/hit position back toward the building center; the new vector starts at the stack center and points outward toward the consumed piece or collapse source, with a random fallback only for exact center hits.

Verification:

- Passed: JavaScript syntax check for source and release `js/main.js` and `js/build-info.js`.
- Passed: source/release parity check for `index.html`, `js/main.js`, `js/build-info.js`, and `PACKAGE_MANIFEST.md`.
- Passed: local test server at `http://127.0.0.1:4173/` served `Master 16.93`.
- Passed: browser smoke test loaded `Downtown Devour`, confirmed `Master 16.93` on the first screen, reported no console warnings/errors, selected a mode, clicked Begin, and reached the gameplay HUD.

Defects Detected:

- None from mechanical and smoke verification. Player visual QA should confirm the skyscraper debris now falls naturally during real collapse play.
