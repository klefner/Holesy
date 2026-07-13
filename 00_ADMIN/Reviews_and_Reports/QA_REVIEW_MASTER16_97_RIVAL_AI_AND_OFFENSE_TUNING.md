# QA Review - Master 16.97 Rival AI And Offense Tuning

Date: 2026-06-24

Build under review: `Master 16.97`

Scope:

- Defect repair for Endless rival scores decreasing after the player devours a rival hole.
- AI behavior repair for rivals leaving their own building-collapse rubble instead of collecting it.
- Combat tuning for boss-derived/offensive units that felt too weak and too same-speed.

Changes reviewed:

- Endless rival respawn no longer reduces `score`; respawn size is still reset through `sizeResetScoreFloor` so rivals return smaller without losing leaderboard points.
- Rival AI difficulty profiles roll once per game and are saved/restored with Endless saves, instead of being overwritten by Endless wave pressure each wave.
- Rival holes remember building/voxel collapse sites they caused and prioritize collectible rubble around that site before returning to normal wandering.
- Offensive units now have distinct speed tiers: soldiers are slowest, vehicles/boss-derived units are faster, and drones/shock/flame/tank archetypes are visibly quicker.
- Offensive unit speed and damage now scale upward by wave number, with true fifth-wave boss forms receiving an additional boss-form boost.

Verification:

- `node --check` passed for source and release `js/main.js` and `js/build-info.js`.
- SHA-256 source/release parity matched for `index.html`, `how-to-play.html`, `PACKAGE_MANIFEST.md`, `js/main.js`, and `js/build-info.js`.
- Browser menu smoke passed on `http://127.0.0.1:4174/?v=16.97`: loaded `Master 16.97`, reached `menu-listeners-ready`, found the Begin button enabled, and produced no console errors.
- Browser automation click/screenshot dispatch timed out in the in-app browser bridge during this run, so gameplay entry remains player-test validation rather than automated proof.

Release note:

- This updates the governed local source package and the local release package for player testing. It does not verify or update the live GoDaddy site.
