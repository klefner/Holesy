# QA Review - Master 16.92 Randomized Wave Corners

Date: 2026-06-23

Change:

- Randomized the corner assignment for each alive hole at the start of every wave.
- Added a previous-corner guard so a hole avoids returning to its prior wave-start corner when another corner is available.
- Preserved score, radius, AI reset state, and the temporary `Master 16.91` tank-testing behavior.

Implementation Note:

- This is wave spawn-position logic, not object physics. It shuffles corner coordinate assignments before the wave world resumes.

Verification:

- Passed: JavaScript syntax check for source and release `js/main.js` and `js/build-info.js`.
- Passed: source/release parity check for `index.html`, `js/main.js`, `js/build-info.js`, and `PACKAGE_MANIFEST.md`.
- Passed: browser smoke test loaded `Master 16.92`, started Endless mode, displayed the HUD, and reported no page exceptions.
- Passed: test-only browser instrumentation repeatedly invoked the real wave reposition function and confirmed unique corner assignment with no alive hole repeating its previous corner when another corner was available.

Defects Detected:

- None after implementation.
