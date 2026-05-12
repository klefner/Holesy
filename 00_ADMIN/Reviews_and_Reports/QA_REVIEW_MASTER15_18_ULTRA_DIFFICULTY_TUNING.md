# QA Review - Master 15.18 Ultra Difficulty Tuning

Date:

- 2026-05-11

Candidate under review:

- `C:\Users\KentLefner\Desktop\game-repo\Holesy-clean\20_TESTS\Candidate_Builds\Master 15.18 - ultra-difficulty-tuning.html`

Baseline:

- `C:\Users\KentLefner\Desktop\game-repo\Holesy-clean\20_TESTS\Candidate_Builds\Master 15.17 - difficulty-descriptions.html`

Backlog / issue basis:

- User reported `Ultra` did not feel ultra.
- User reported the difference across `Normal`, `Hard`, and `Ultra` was not discernible.
- Product backlog requires the hardest setting to be materially harder to survive while still beatable.

Reason for candidate:

- make difficulty differences immediately perceptible
- make `Ultra` meaningfully harder without changing the performance profile system
- preserve `Normal` as the current baseline

Implementation review:

- build marker advanced to `Master 15.18`
- `Normal` remains baseline
- `Hard` increases soldier damage, speed, count, deployment cadence, hit chance, aid delay, and rival AI efficiency
- `Ultra` now enables Wave 1 troop pressure
- `Ultra` substantially increases soldier damage, speed, count, deployment cadence, and hit chance
- `Ultra` makes aid meaningfully scarcer
- `Ultra` makes rivals scan farther, decide faster, and contest aid more aggressively
- debug overlay now shows active wave hit-chance multiplier

Risk controls:

- no `Master 15` source file was changed
- no website release package was changed
- candidate is isolated to difficulty tuning after the selector UI passed

Validation required:

1. Launch the candidate and confirm the build marker shows `Master 15.18`.
2. Start Waves on `Normal` and confirm it preserves the current baseline feel.
3. Start Waves on `Hard` and confirm it is more pressured than `Normal`.
4. Start Waves on `Ultra` and confirm pressure is immediately discernible, including Wave 1 troop pressure.
5. Confirm `Ultra` is materially harder to survive than `Normal`.
6. Confirm `Ultra` remains beatable with strong play.
7. Confirm no browser console errors occur during the flow.

Validation performed on 2026-05-11:

- Extracted module script syntax check passed.
- Local preview server returned HTTP 200 for the candidate URL.
- Candidate content confirms the `Master 15.18` build marker.
- Candidate content confirms the `Ultra` description and Wave 1 troop-pressure flag are present.
- Diff hygiene check passed with line-ending warnings only.

Validation still open:

- User confirmation of `Normal`, `Hard`, and `Ultra` feel.
- User confirmation that `Ultra` is hard but beatable.

Status:

- candidate prepared for validation
- not approved for promotion
