# QA Review - Master 16.101 Count-Based Mandates

Date: 2026-06-26

Build under review: `Master 16.101`

## Scope

- Replaced hidden exact-object Mandate targets with explicit category-count objectives.
- Mandate rows now show the action, category, and live progress count, for example `Eat People 0/8`.
- Any matching object in the listed category advances that row; Mandate order does not matter.
- Moved the Mandate card lower in the right HUD stack so the title and instructions remain readable below the timer/control cluster.

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
- Passed browser smoke on `http://127.0.0.1:4174/?v=16.101` using installed Chrome:
  - Build label rendered as `Master 16.101`.
  - Endless Waves round started successfully.
  - Mandate panel rendered readable title and instruction text.
  - Mandate rows rendered as explicit count objectives, including examples such as `EAT PEOPLE 0/12`, `EAT PROPS 0/9`, `EAT CARS 0/3`, `EAT OFFICES 0/3`, and `EAT TOWERS 0/2`.
  - Mandate panel did not overlap the timer or Time/Pause controls.
  - No browser console errors or page errors were detected.

## Notes

- This patch implements count-based eat objectives only. Future avoid-style Mandates should use the same explicit verb/count row model.
- The live GoDaddy site was not checked or updated in this pass.
