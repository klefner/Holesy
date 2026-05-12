# QA Review - Master 15.15 New Wave Text And Audio Stop

Date:

- 2026-05-11

Candidate under review:

- `C:\Users\KentLefner\Desktop\game-repo\Holesy-clean\20_TESTS\Candidate_Builds\Master 15.15 - new-wave-text-and-audio-stop.html`

Baseline:

- `C:\Users\KentLefner\Desktop\game-repo\Holesy-clean\20_TESTS\Candidate_Builds\Master 15.14 - synced-race-light-countdown.html`

Backlog / issue basis:

- User requested countdown proof text change from `BEEP X` to `NEW WAVE IN X`.
- Next backlog item requires non-music audio to stop on game-over and mode-select screens.
- Issue log: `QA-005`.

Reason for candidate:

- keep the accepted race-light countdown sound and synchronization path
- replace the visible countdown proof wording
- harden non-music audio cleanup when leaving active gameplay

Implementation review:

- build marker advanced to `Master 15.15`
- countdown banner and stage pop now use `NEW WAVE IN X`
- fallback wave-end warning text also uses `NEW WAVE IN X`
- active non-music sample sources are now tracked
- menu/game-over cleanup now stops already-playing non-music samples
- existing aid loop, radar timer, aid ship, plane engine, and non-music gain cleanup remains in place

Risk controls:

- no `Master 15` source file was changed
- no website release package was changed
- candidate is isolated to countdown wording and non-music audio cleanup

Validation required:

1. Launch the candidate and confirm the build marker shows `Master 15.15`.
2. Start Waves mode with audio enabled.
3. Confirm the final countdown visible proof reads `NEW WAVE IN 3`, `NEW WAVE IN 2`, and `NEW WAVE IN 1`.
4. Confirm the race-light beep still fires with the visible timer countdown.
5. Return to game-mode selection and confirm aid radar, soldier callouts, gunshots, and plane engines stop.
6. Reach game-over and confirm the same non-music sounds stop while menu music remains allowed.
7. Confirm no browser console errors occur during the flow.

Validation performed on 2026-05-11:

- Extracted module script syntax check passed.
- Local preview server returned HTTP 200 for the candidate URL.
- Candidate content confirms the `Master 15.15` build marker.
- Candidate content confirms `NEW WAVE IN` countdown wording is present.
- Diff hygiene check passed with line-ending warnings only.
- User reported all tests passed.

Validation still open:

- None.

Status:

- user-validated
- approved as the current candidate basis for the next backlog slice
