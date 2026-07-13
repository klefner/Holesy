# QA Review - Master 16.89 Wave Transition Lock Fix

Date: 2026-06-23

Defect:

- Player reported the game locked up during the transition from Endless Wave 1 to Wave 2.
- Browser automation reproduced a runtime stop on the current army-boss build: `timedMode is not defined`.

Root Cause:

- `Master 16.88` added late-mode army boss triggers.
- The trigger checked `timedMode`, but this runtime tracks timed play through `selectedMode === 'timed'`; no `timedMode` variable exists.
- When Wave 2 enabled soldier deployment, `updateWaves()` called the late-mode trigger and threw, stopping the game loop.

Fix:

- Replaced the invalid `timedMode` reference with `selectedMode === 'timed'`.
- Preserved the army boss behavior for Timed, LMS, regular Waves, and Endless fifth-wave spawns.
- Bumped the testable build label and cache-busters to `Master 16.89`.

Verification:

- Passed: JavaScript syntax check for source and release `js/main.js` and `js/build-info.js`.
- Passed: controlled browser transition test from Endless Wave 1 to Wave 2; Wave 2 reached `state: playing`, troop deployment countdown resumed, and no `timedMode` runtime error occurred.
- Passed: source/release hash parity check for `index.html`, `js/main.js`, `js/build-info.js`, and `PACKAGE_MANIFEST.md`.

Defects Detected:

- None after implementation pending verification.
