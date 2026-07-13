# QA Review - Master 16.99 Devour Mandate Targets

Date: 2026-06-25

Build under review: `Master 16.99`

## Scope

- Added the Devour Mandate target system for the current populated district.
- Selected one person, one prop, one car, one mid building, and one skyscraper after each district/world population.
- Added the centered Mandate HUD panel with five dots, remaining-count text, collected state, and late-round warning pulse.
- Awarded `+2,500` score and completion feedback when the player consumes all Mandate targets.
- Added incomplete-Mandate failure feedback on round end.
- Preserved later-PBI boundaries: no Locator Pulse, city transition modal, failure modal, or settings defaults were implemented.

## Verification

- Passed: `node --check "10_SOURCE/Masters/Master 16/js/main.js"`.
- Passed: `node --check "10_SOURCE/Masters/Master 16/js/build-info.js"`.
- Passed: `node --check "40_RELEASE/Website_Publish_Package/holesy/js/main.js"`.
- Passed: `node --check "40_RELEASE/Website_Publish_Package/holesy/js/build-info.js"`.
- Passed: source/release SHA-256 parity for `index.html`, `how-to-play.html`, `PACKAGE_MANIFEST.md`, `css/styles.css`, `js/main.js`, and `js/build-info.js`.
- Passed: HTTP smoke for `http://127.0.0.1:4174/?v=16.99` returned status `200`.
- Passed: Browser smoke on `http://127.0.0.1:4174/?v=16.99` loaded `Master 16.99`, started a round, showed the Mandate HUD, rendered five unfilled Mandate dots, displayed `5 left`, and produced no console errors or missing-resource responses.

## Notes

- The attached Claude briefing referred to an older single-file architecture. Implementation followed the governed modular `Master 16` package instead.
- Wave and Endless modes rebuild their worlds as before; each newly populated district selects a fresh Mandate target set for that wave.
- The live GoDaddy site was not checked or updated in this pass.
