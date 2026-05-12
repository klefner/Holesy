# QA Review - Master 15.36 Car Crash Trigger Fix

Date:

- 2026-05-12

Candidate under review:

- `C:\Users\KentLefner\Desktop\game-repo\Holesy-clean\20_TESTS\Candidate_Builds\Master 15.36 - car-crash-trigger-fix.html`
- `C:\Users\KentLefner\Desktop\game-repo\Holesy-clean\20_TESTS\Candidate_Builds\Master 15.36 - car-crash-trigger-fix.css`
- `C:\Users\KentLefner\Desktop\game-repo\Holesy-clean\20_TESTS\Candidate_Builds\Master 15.36 - build-info.js`

Baseline:

- `C:\Users\KentLefner\Desktop\game-repo\Holesy-clean\20_TESTS\Candidate_Builds\Master 15.35 - car-crash-visibility-tuning.html`

Backlog / issue basis:

- User validation confirmed cars speed up and wobble.
- User still did not observe any car crashing.
- Root cause hypothesis: crash probability was tied to short per-frame random chances during brief panic windows.

Implementation review:

- created `Master 15.36 - car-crash-trigger-fix.html`
- advanced build marker to `Master 15.36`
- added `panicTimer` tracking on moving cars
- lowered crash speed gate from `11.25` to `9.75`
- added `panicCrashWarmupSeconds = 0.65`
- added `panicCrashForcedSeconds = 1.45`
- added `panicCrashForcedChancePerSecond = 1.25`
- crash chance now increases substantially after a sustained panic chase

Risk controls:

- no source master was changed
- no website release package was changed
- normal non-panicked traffic behavior remains unchanged
- crashes still require panic state, speed gate, and warm-up time

Validation required:

1. Launch candidate and confirm build marker shows `Master 15.36`.
2. Confirm cars still speed up and wobble near the player.
3. Maintain a chase behind/near a panicked car for a few seconds.
4. Confirm at least some cars lose control and crash.
5. Confirm crashes still stop cars and produce smoke/flame/explosion variation.
6. Confirm crashes are not happening for calm traffic away from holes.
7. Confirm no browser console errors during traffic panic and crash.

Validation performed on 2026-05-12:

- Candidate file exists.
- CSS file exists.
- Build-info module exists.
- Candidate imports `Master 15.36` module files.
- Candidate content confirms panic timer and forced crash-rate tuning exists.
- Local preview returned HTTP 200 for the candidate HTML, CSS, build-info, difficulty-profiles, and game-stats files.
- Extracted module script passed JavaScript syntax check.

Validation still open:

- Player-facing gameplay validation.
- User validation.

Status:

- ready for gameplay validation
