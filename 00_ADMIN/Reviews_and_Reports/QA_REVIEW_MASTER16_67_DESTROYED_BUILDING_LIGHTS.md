# QA Review - Master 16.67 Destroyed Building Lights

Date: 2026-06-09

## Scope

- Source: `10_SOURCE/Masters/Master 16/`
- Release package: `40_RELEASE/Website_Publish_Package/holesy/`
- Build label: `Master 16.67`

## Change Reviewed

- Building-window night lighting now records an extinguished state per registered window mesh.
- Normal falling consumption, object removal, skyscraper stack activation, medium-office voxel-column activation, and government-building activation extinguish affected building windows.
- Time-of-day cycling respects the extinguished state so destroyed building windows do not relight in evening or night.

## Verification

- `node --check` passed for source and release `js/main.js` and `js/build-info.js`.
- SHA256 parity passed for source/release `index.html`, `js/main.js`, and `js/build-info.js`.
- Local HTTP smoke at `http://127.0.0.1:4173/index.html` returned 200, served `Master 16.67`, kept the Time button present, and kept the removed Weather button absent.
- Local HTTP module checks confirmed served `js/build-info.js` contains `BUILD_SUB = 67` and served `js/main.js` contains the destroyed-building light extinguish helpers and activation hooks.
- Playwright browser smoke was not available in this runtime because the local Playwright install is missing `playwright-core`; HTTP/static served-package checks were used as the fallback.
