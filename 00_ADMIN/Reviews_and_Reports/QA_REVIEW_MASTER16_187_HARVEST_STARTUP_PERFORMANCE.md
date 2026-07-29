# QA Review - Master 16.187 Harvest Startup Performance

Date: 2026-07-29

## Finding

Harvest County loaded its prefab and destructible resources sequentially. More than two dozen network and decode waits therefore accumulated behind the disabled `Building City...` button.

## Change

- start all required Harvest prefab and destructible loads through the existing GLTFLoader caches when Harvest County is selected
- await the shared preload promise before town population
- preserve the existing frame-yielded scene construction and every shipped asset
- publish preload state through `data-holesy-harvest-preload`

## Verification

- Live Master 16.185 baseline in automated Chrome: 10.8 seconds from Begin to visible gameplay.
- Master 16.187 with 350 ms artificial latency on every Harvest asset request: 1.17 seconds from Begin to visible gameplay.
- Completed town telemetry: preload `ready`, 8 buildings, 792 edibles.
- No page or console errors.
- Visual screenshot review confirmed the gameplay scene and HUD rendered.
- JavaScript syntax passed for source/release `main.js` and `build-info.js`.
- SHA-256 source/release parity passed for `index.html`, `js/main.js`, and `js/build-info.js`.

## Acceptance

Harvest County should normally enter gameplay within a few seconds. Connection speed, cold browser cache, and device decode performance can vary, but network latency no longer multiplies across every asset.
