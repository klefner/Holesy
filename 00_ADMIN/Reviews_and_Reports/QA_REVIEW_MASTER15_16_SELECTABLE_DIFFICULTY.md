# QA Review - Master 15.16 Selectable Difficulty

Date:

- 2026-05-11

Candidate under review:

- `C:\Users\KentLefner\Desktop\game-repo\Holesy-clean\20_TESTS\Candidate_Builds\Master 15.16 - selectable-difficulty.html`

Baseline:

- `C:\Users\KentLefner\Desktop\game-repo\Holesy-clean\20_TESTS\Candidate_Builds\Master 15.15 - new-wave-text-and-audio-stop.html`

Backlog / issue basis:

- Product backlog requires player-selectable difficulty.
- Difficulty must remain separate from the performance profile.
- Hardest setting must be materially harder to survive while still beatable.

Reason for candidate:

- add a simple dropdown above game-mode selection
- persist the selected gameplay difficulty
- centralize difficulty-linked tuning so future balancing can happen in one place

Implementation review:

- build marker advanced to `Master 15.16`
- mode-select screen now includes a `Difficulty` dropdown with `Normal`, `Hard`, and `Ultra`
- selection persists with `localStorage` key `holesyDifficulty`
- `Normal` preserves baseline wave and AI values
- `Hard` and `Ultra` tune soldier damage, soldier speed, soldier counts, soldier deployment cadence, aid-drop delay, rival scan budget, rival decision speed, and rival aid interest
- debug overlay reports active difficulty
- performance profile selection remains separate and unchanged

Risk controls:

- no `Master 15` source file was changed
- no website release package was changed
- candidate is isolated to selectable difficulty and central difficulty tuning

Validation required:

1. Launch the candidate and confirm the build marker shows `Master 15.16`.
2. Confirm the difficulty dropdown appears above the game modes.
3. Change the selected difficulty and confirm it persists after reload.
4. Start Waves on `Normal` and confirm it matches the current baseline feel.
5. Start Waves on `Ultra` and confirm it is materially harder than `Normal`.
6. Confirm `Ultra` remains possible to survive with strong play.
7. Confirm no browser console errors occur during the flow.

Validation performed on 2026-05-11:

- Extracted module script syntax check passed.
- Local preview server returned HTTP 200 for the candidate URL.
- Candidate content confirms the `Master 15.16` build marker.
- Candidate content confirms the difficulty dropdown and `Ultra` option are present.
- Diff hygiene check passed with line-ending warnings only.

Validation still open:

- User confirmation of dropdown placement and persistence.
- User confirmation of `Normal` baseline feel.
- User confirmation of `Ultra` difficulty.

Status:

- candidate prepared for validation
- not approved for promotion
