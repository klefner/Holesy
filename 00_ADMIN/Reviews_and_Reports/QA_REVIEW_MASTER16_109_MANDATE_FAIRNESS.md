# QA Review - Master 16.109 Mandate Fairness

Date: 2026-07-06
Scope: `10_SOURCE/Masters/Master 16/` and mirrored release package.

## Summary

`Master 16.109` repairs Mandate target pressure after player testing showed wave 1 people and car counts were too hard even with focused play.

## Changes Reviewed

- Replaced high first-wave category-inventory percentages with explicit per-category count curves.
- Lowered first-wave people and car pressure from near-total category sweeps to achievable focused-play targets.
- Added actual-supply caps so scarcer categories on harder difficulties cannot demand too much of what spawned.
- Preserved per-wave scaling so Mandates still become harder over later waves and Endless progression.
- Kept Mandates mandatory; this is a fairness repair, not a removal of the survival contract.

## Verification

- `node --check` passed for source `js/main.js` and `js/build-info.js`.
- Static Mandate math sample using normal first-wave supply showed approximate targets of 33 people and 7 cars on wave 1, scaling to 48 people and 11 cars by wave 4, and 72 people and 16 cars by wave 10.
- Source-to-release SHA-256 parity passed for `index.html`, `how-to-play.html`, `PACKAGE_MANIFEST.md`, `css/styles.css`, `js/main.js`, `js/build-info.js`, `js/difficulty-profiles.js`, `js/government-physics.js`, and `data/lore-documents.js`.
- Release-to-upload SHA-256 parity passed for the same checked file set.
- Full upload copy created at `C:\Users\KentLefner\Downloads\holesy-godaddy-upload-master-16.109\holesy\` with 422 files and 22 directories.
- Browser smoke passed at `http://127.0.0.1:4186/index.html?fresh=16.109`: `Master 16.109` loaded, no console warnings/errors were reported, `Begin` entered Endless, and the wave 1 Mandate HUD showed `Eat People 0/35` and `Eat Cars 0/8` in the observed run.
- Live site was not verified in this review.
