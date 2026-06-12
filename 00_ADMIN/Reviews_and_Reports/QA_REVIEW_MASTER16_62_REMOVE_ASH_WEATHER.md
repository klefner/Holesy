# QA Review: Master 16.62 Remove Ash Weather

Date: 2026-06-09

## Scope

Remove Ash from the in-game Weather cycle while preserving the richer Rain and Snow visual behavior from `Master 16.61`.

## Change

- Removed the Ash weather preset from `WEATHER_LOOKS`.
- Restored the `Master 16.61` weather particle count and per-frame Rain/Snow motion behavior.
- Kept the in-game Weather cycle to Clear, Rain, and Snow.
- Updated source/release entry labels and build metadata to `Master 16.62`.

## Validation

- `node --check` on source `js/main.js`
- `node --check` on release `js/main.js`
- `node --check` on source `js/build-info.js`
- `node --check` on release `js/build-info.js`
- Source/release hashes match for `index.html`, `js/main.js`, and `js/build-info.js`.

## Follow-Up

Use one active game tab during visual review so browser resource contention does not masquerade as a single-build performance regression.
