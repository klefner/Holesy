# QA Review: Master 16.60 Time-Of-Day Lighting

Date: 2026-06-09

## Scope

Add distinct morning, mid day, evening, and night visual looks to the game without changing gameplay rules or the modular package structure.

## Change

- Added a shared `TIME_OF_DAY_LOOKS` palette in `js/main.js`.
- Wave-based modes now rotate sky color, fog color, ambient light, sun color, sun angle, and ground tint by wave.
- Timed and Last Man Standing modes keep the readable mid day look as their stable baseline.
- Source and release entry labels now report `Master 16.60`.

## Architecture Notes

The change stays inside the existing Three.js scene setup and wave-start flow. It does not add new assets, alter physics, or change the modular browser-client structure.

Source and release package files were kept mirrored for:

- `index.html`
- `js/build-info.js`
- `js/main.js`

## Validation

- Ran governed Team Sync before implementation.
- `node --check` on source `js/main.js`
- `node --check` on release `js/main.js`
- `node --check` on source `js/build-info.js`
- `node --check` on release `js/build-info.js`
- Source/release hashes match for `index.html`, `js/build-info.js`, and `js/main.js`.
- HTTP smoke confirms the local release package serves `index.html`, `js/build-info.js`, and `js/main.js` with status 200.
- HTTP smoke confirms release `js/build-info.js` reports `BUILD_SUB = 60` and `Master 16.60`.
- HTTP smoke confirms release `js/main.js` contains `TIME_OF_DAY_LOOKS`.

Browser screenshot automation was attempted through the bundled Playwright package, but this runtime is missing `playwright-core`; visual screenshot approval remains a follow-up.

## Follow-Up

Run browser playtest screenshots across multiple Endless waves to visually approve the four time-of-day looks before treating the lighting pass as user-validated.
