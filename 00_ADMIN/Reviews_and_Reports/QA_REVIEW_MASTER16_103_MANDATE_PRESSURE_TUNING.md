# QA Review - Master 16.103 Mandate Pressure Tuning

Date: 2026-06-26

Build under review: `Master 16.103`

## Scope

- Retuned Mandate required counts from small fixed numbers to pressure targets based on a larger share of the live district inventory.
- Added wave-based Mandate pressure growth so later waves ask for more of each listed category.
- Preserved the `Master 16.101` count-based Mandate HUD and the `Master 16.102` readable font sizing.

## Verification

- Pending final verification in this implementation pass:
  - Source and release JavaScript syntax checks.
  - Source/release hash parity for changed files.
  - Browser smoke on the local test URL confirming `Master 16.103`, higher generated Mandate counts, readable HUD rows, and no console errors.

## Notes

- This tuning is intentionally more demanding so the player feels rushed for most of the wave instead of completing the Mandate in the first 15-20 seconds.
- The live GoDaddy site was not checked or updated in this pass.
