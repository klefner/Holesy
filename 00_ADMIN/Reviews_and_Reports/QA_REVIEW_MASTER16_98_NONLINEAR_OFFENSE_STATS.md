# QA Review - Master 16.98 Nonlinear Offense And Stats Panel

Date: 2026-06-24

Build under review: `Master 16.98`

## Scope

- Replaced the straight offensive-unit wave ramp with a stronger nonlinear wave-pressure curve.
- Preserved slower soldier movement while allowing vehicle and boss-derived units to reach higher speed and damage caps in later waves.
- Moved the Game Stats view from a separate popup window into an in-page modal.
- Refreshed source, release package, and governed source-of-truth labels to `Master 16.98`.

## Verification

- Passed: `node --check "10_SOURCE/Masters/Master 16/js/main.js"`.
- Passed: `node --check "10_SOURCE/Masters/Master 16/js/build-info.js"`.
- Passed: `node --check "40_RELEASE/Website_Publish_Package/holesy/js/main.js"`.
- Passed: `node --check "40_RELEASE/Website_Publish_Package/holesy/js/build-info.js"`.
- Passed: source/release SHA-256 parity for `index.html`, `how-to-play.html`, `PACKAGE_MANIFEST.md`, `css/styles.css`, `js/main.js`, and `js/build-info.js`.
- Passed: HTTP smoke for `http://127.0.0.1:4174/?v=16.98` returned status `200`.
- Passed: Browser smoke on `http://127.0.0.1:4174/?v=16.98` loaded `Master 16.98`, found Begin enabled, and opened the in-page `Game Stats` modal from the Stats button.

## Notes

- The live GoDaddy site was not checked or updated in this pass.
- The convenience GoDaddy upload folders in Downloads still lag the governed source and release package.
