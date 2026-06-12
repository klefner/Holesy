# QA Review: Master 16.66 Evening and Night City Lights

Date: 2026-06-09

Scope:

- Added evening/night visual toggles for street lamp glow, sparse lit building windows, and car headlights/taillights.
- Registered windows from small buildings, medium-office cubes, skyscraper chunks, government-building pieces, and their save/load reconstruction paths.
- Kept fewer than half of registered windows eligible to light up by using per-pane random lit chances below 50%.
- Used mesh visibility and material swaps instead of dynamic point lights to preserve performance.
- Kept the Weather button and weather particle system removed.

Verification completed:

- Ran JavaScript syntax checks for source and release `js/main.js` and `js/build-info.js`.
- Compared SHA-256 hashes for changed source/release `index.html`, `js/main.js`, and `js/build-info.js`; all checked pairs matched.
- Confirmed no live Weather button/runtime references returned for `weatherCycleBtn`, `cycleWeatherLook`, `updateWeather`, `WEATHER_LOOKS`, or `weather-cycle-btn`.
- Loaded `http://127.0.0.1:4173/index.html` in the in-app browser; the page reported `Master 16.66`, showed `Time: Mid Day Pause`, omitted the Weather button, and had no console errors.
- Browser click automation for cycling Time was unreliable in this run, so the browser check was limited to boot/control smoke rather than visual screenshot validation.
