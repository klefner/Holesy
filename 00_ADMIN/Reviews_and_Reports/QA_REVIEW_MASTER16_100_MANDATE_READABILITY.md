# QA Review - Master 16.100 Mandate Readability

Date: 2026-06-26

Build under review: `Master 16.100`

## Scope

- Moved the Mandate panel from the top-center lane to the right HUD stack under the timer.
- Added target labels for Person, Prop, Car, Office, and Tower so the Mandate is actionable without guessing.
- Preserved the five-dot collection and late-warning behavior from `Master 16.99`.

## Verification

- Passed source and release JavaScript syntax checks:
  - `10_SOURCE/Masters/Master 16/js/main.js`
  - `10_SOURCE/Masters/Master 16/js/build-info.js`
  - `40_RELEASE/Website_Publish_Package/holesy/js/main.js`
  - `40_RELEASE/Website_Publish_Package/holesy/js/build-info.js`
- Passed source/release hash parity for changed package files:
  - `index.html`
  - `how-to-play.html`
  - `PACKAGE_MANIFEST.md`
  - `css/styles.css`
  - `js/main.js`
  - `js/build-info.js`
- Passed browser smoke on `http://127.0.0.1:4174/?v=16.100` using installed Chrome:
  - Build label rendered as `Master 16.100`.
  - Endless Waves round started successfully.
  - Mandate panel rendered under the right-side timer stack instead of the top-center banner lane.
  - Mandate panel text rendered as `PERSON`, `PROP`, `CAR`, `OFFICE`, and `TOWER`.
  - No browser console errors or page errors were detected.

## Notes

- The live GoDaddy site was not checked or updated in this pass.
