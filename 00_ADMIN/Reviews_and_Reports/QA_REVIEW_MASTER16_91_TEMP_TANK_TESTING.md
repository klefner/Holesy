# QA Review - Master 16.91 Temporary Tank Testing

Date: 2026-06-23

Change:

- Temporarily forced the green tank army boss into every playable mode and wave so player testing can see it immediately.
- Shortened the first testing deployment delay through a named temporary configuration flag.
- Preserved the `Master 16.90` tank mesh, push behavior, and building-pressure collapse behavior.

Rollback Note:

- Remove or disable `HOLESY_CONFIG.military.armyBossTestingEveryLevel` after tank validation to restore normal boss cadence.

Verification:

- Passed: JavaScript syntax check for source and release `js/main.js` and `js/build-info.js`.
- Passed: source/release parity check for `index.html`, `js/main.js`, `js/build-info.js`, and `PACKAGE_MANIFEST.md`.
- Passed: browser smoke test loaded `Master 16.91`, started Endless mode, displayed the HUD, and reported no page exceptions.
- Passed: test-only browser instrumentation counted the real temporary spawn path creating one green tank army boss shortly after Begin, with the `ARMY BOSS INBOUND! GREEN TANK IS 2X CAR SIZE AND 3X DAMAGE.` banner visible.

Defects Detected:

- None after implementation.
