# QA Review - Master 16.94 Boss Roster Drops

Date: 2026-06-24

Change:

- Removed the temporary tank-every-level testing flag.
- Added three offensive boss archetypes:
  - Siege Tank: green tank body, slow cannon cadence, cannon-shot audio, high single-hit damage, and existing push/building-pressure behavior.
  - Twin-Gun Mech: blue/steel walker body with two heavy machine-gun hands and paired tracers.
  - Shield Commander: red/black heavy infantry body with shield, slower heavy bursts, and higher reward than soldiers.
- Every fifth Endless wave now selects one boss archetype and drops that boss alone.
- Each revealed boss archetype becomes a later-wave random plane-drop option at reduced damage; soldiers remain the baseline drop option.
- Wave 1 remains soldier-free; Waves 2 through 4 remain soldier-led before the first boss reveal.
- Updated the How to Play build label from stale `Master 16.55` to `Master 16.94`.

Implementation Note:

- Plane drops now use an explicit unit manifest instead of hard-coded soldier count plus one tank flag. Saved Endless planes/paratroopers/soldiers include unit type so in-progress runs can restore the correct offensive unit model and damage profile.
- Boss selection favors unrevealed boss archetypes first, so the first three fifth-wave boss reveals introduce the full roster before repeats.

Verification:

- Passed: JavaScript syntax check for source and release `js/main.js` and `js/build-info.js`.
- Passed: source/release parity check for `index.html`, `how-to-play.html`, `js/main.js`, `js/build-info.js`, and `PACKAGE_MANIFEST.md`.
- Passed: static search confirmed the temporary tank-testing flag and helper path are removed.
- Passed: fresh local test server at `http://127.0.0.1:4174/` served `Master 16.94`.
- Passed: browser smoke test loaded `Downtown Devour`, confirmed `Master 16.94` on the first screen, clicked Begin, reached the gameplay HUD, and reported no console warnings/errors.

Defects Detected:

- None from mechanical and smoke verification. Player QA should specifically validate live fifth-wave boss selection, cannon/mech/heavy weapon feel, and later-wave random unlocked-unit drops.
