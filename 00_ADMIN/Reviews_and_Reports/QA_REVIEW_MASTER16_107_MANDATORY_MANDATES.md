# QA Review - Master 16.107 Mandatory Mandates

Date: 2026-07-06
Scope: `10_SOURCE/Masters/Master 16/` and mirrored release package.

## Summary

`Master 16.107` changes Mandates from bonus-only wave objectives into the required wave contract for wave-based play.

## Changes Reviewed

- In Waves and Endless, wave expiry now checks Mandate completion before advancing the wave.
- If the Mandate is incomplete at wave expiry, the run ends with `Mandate Failed`.
- `Mandate Failed` is classified as a loss for stats/lore reward purposes.
- Mandate completion still awards score, but now also grants a `Mandate Surge`.
- `Mandate Surge` applies speed and bullet-protection effects until the end of the current wave.
- The active-effects HUD can display the Mandate Surge state.
- The Mandate HUD instruction now tells the player that every count must be finished before wave end.
- How to Play now distinguishes mandatory Mandates from optional Run Goals.

## QA Notes

- Expected gameplay effect: Run Goals remain optional side rewards.
- Expected gameplay effect: Mandates now carry real failure pressure in Waves and Endless.
- Expected gameplay effect: completing the Mandate early creates a meaningful end-of-wave power window.
- Timed and LMS behavior are not changed into Mandate-failure modes by this slice.

## Verification

- `node --check` passed for source `js/main.js` and `js/build-info.js`.
- `node --check` passed for release `js/main.js` and `js/build-info.js`.
- Source/release SHA-256 parity passed for entry HTML, How to Play, package manifest, stylesheet, runtime modules, difficulty profiles, government physics, and lore data.
- Local release package served successfully at `http://127.0.0.1:4186/index.html?fresh=16.107`.
- Browser smoke confirmed visible `Master 16.107`, Endless selected by default, four enabled mode buttons, updated Mandate instruction text, and no console errors/warnings.
- Full upload folder created at `C:\Users\KentLefner\Downloads\holesy-godaddy-upload-master-16.107\holesy\` with 422 files.
- Upload-folder spot-check hashes matched the release package for entry HTML, How to Play, package manifest, stylesheet, runtime module, build-info module, government physics, and lore data.
- Live site was not verified in this review.
