# QA Review: Master 16.61 Time And Weather Controls

Date: 2026-06-09

## Scope

Add an in-game preview control for cycling time-of-day looks and add weather effects without changing gameplay rules or the modular package structure.

## Change

- Added compact in-game `Time` and `Weather` buttons beside Pause.
- The `Time` button manually cycles Morning, Mid Day, Evening, and Night.
- The `Weather` button cycles Clear, Rain, Snow, and Ash.
- Weather uses a lightweight Three.js particle field around the active camera/player.
- Weather also adjusts fog distance/color, sky tint, ambient light, sun intensity, and ground tint.
- Updated cache-busted source/release entry points and build metadata to `Master 16.61`.

## Architecture Notes

The implementation stays inside the modular browser-client package:

- `index.html` for the two game-screen controls
- `css/styles.css` for compact responsive button layout
- `js/main.js` for time/weather scene state and particles
- `js/build-info.js` for build notes

No new asset files were added. The weather visuals are generated at runtime and do not alter scoring, physics, AI, save data, or wave configuration.

## Validation

- Ran governed Team Sync before implementation.
- `node --check` on source `js/main.js`
- `node --check` on release `js/main.js`
- `node --check` on source `js/build-info.js`
- `node --check` on release `js/build-info.js`
- Source/release hashes match for `index.html`, `css/styles.css`, `js/main.js`, and `js/build-info.js`.
- HTTP smoke confirms the local release package serves `index.html`, `css/styles.css`, `js/build-info.js`, and `js/main.js` with status 200.
- HTTP smoke confirms release `js/build-info.js` reports `BUILD_SUB = 61` and `Master 16.61`.
- HTTP smoke confirms release `index.html` contains the `time-cycle-btn` and `weather-cycle-btn` controls.
- HTTP smoke confirms release `js/main.js` contains `WEATHER_LOOKS` and the time-cycle handler.
- HTTP smoke confirms release `css/styles.css` contains weather-control styling.

## Follow-Up

Run browser playtest screenshots for the game screen controls and all weather states before treating the visual pass as user-validated.
