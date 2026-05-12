# QA Review - Master 15.35 Car Crash Visibility Tuning

Date:

- 2026-05-12

Candidate under review:

- `C:\Users\KentLefner\Desktop\game-repo\Holesy-clean\20_TESTS\Candidate_Builds\Master 15.35 - car-crash-visibility-tuning.html`
- `C:\Users\KentLefner\Desktop\game-repo\Holesy-clean\20_TESTS\Candidate_Builds\Master 15.35 - car-crash-visibility-tuning.css`
- `C:\Users\KentLefner\Desktop\game-repo\Holesy-clean\20_TESTS\Candidate_Builds\Master 15.35 - build-info.js`

Baseline:

- `C:\Users\KentLefner\Desktop\game-repo\Holesy-clean\20_TESTS\Candidate_Builds\Master 15.34 - skyscraper-collapse-size-gate.html`

Backlog / issue basis:

- User validation confirmed cars visibly speed up to escape the player.
- User did not observe cars losing control or crashing.
- Tuning goal: make crashes visible during normal playtesting without making every escape car crash.

Implementation review:

- created `Master 15.35 - car-crash-visibility-tuning.html`
- advanced build marker to `Master 15.35`
- increased panic crash chance from `0.045` to `0.22` per panic-second
- lowered panic crash minimum speed from `13.5` to `11.25`
- increased panic wobble from `0.9` to `1.75`
- extended loss-of-control duration from `0.85s` to `1.05s`

Risk controls:

- no source master was changed
- no website release package was changed
- normal non-panicked traffic behavior remains unchanged
- crash tuning still applies only to panicked cars above the minimum speed gate

Validation required:

1. Launch candidate and confirm build marker shows `Master 15.35`.
2. Confirm cars still speed up away from nearby holes.
3. Confirm at least some panicked cars visibly wobble, leave control, and crash during repeated chases.
4. Confirm crashes are not constant or universal.
5. Confirm crashed cars still smoke/flame/explode according to existing variation.
6. Confirm no browser console errors during traffic panic and crashes.

Validation performed on 2026-05-12:

- Candidate file exists.
- CSS file exists.
- Build-info module exists.
- Candidate imports `Master 15.35` module files.
- Candidate content confirms increased crash chance, lower crash speed gate, stronger wobble, and longer loss-of-control duration.
- Local preview returned HTTP 200 for the candidate HTML, CSS, build-info, difficulty-profiles, and game-stats files.
- Extracted module script passed JavaScript syntax check.

Validation still open:

- Player-facing gameplay validation.
- User validation.

Status:

- ready for gameplay validation
