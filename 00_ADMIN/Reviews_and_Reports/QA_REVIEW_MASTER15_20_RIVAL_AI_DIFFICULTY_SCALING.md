# QA Review - Master 15.20 Rival AI Difficulty Scaling

Date:

- 2026-05-11

Candidate under review:

- `C:\Users\KentLefner\Desktop\game-repo\Holesy-clean\20_TESTS\Candidate_Builds\Master 15.20 - rival-ai-difficulty-scaling.html`

Baseline:

- `C:\Users\KentLefner\Desktop\game-repo\Holesy-clean\20_TESTS\Candidate_Builds\Master 15.19 - difficulty-asset-scarcity.html`

Backlog / issue basis:

- User confirmed `Ultra` is now obviously harder.
- User reported rival hole scores remain far behind by large multiples.
- User observed ineffective left-right-left dithering before a rival chose a useful direction.
- Backlog requires higher difficulty to improve rival AI quality and target selection.

Reason for candidate:

- make higher-difficulty rivals better at scoring, collecting, and chasing
- reduce visible dithering on harder settings
- preserve baseline rival feel on `Normal`

Implementation review:

- build marker advanced to `Master 15.20`
- difficulty profiles now scale rival movement speed
- higher difficulty reduces random wander bias
- higher difficulty improves route lookahead by valuing nearby follow-on food around a target
- higher difficulty lets rivals more aggressively contest reserved objects when they are closer
- higher difficulty increases chase decisiveness against smaller holes
- `Ultra` rivals scan farther, decide faster, move faster, and choose richer value streams

Risk controls:

- no `Master 15` source file was changed
- no website release package was changed
- candidate is isolated to rival AI difficulty scaling after `15.19`

Validation required:

1. Launch the candidate and confirm the build marker shows `Master 15.20`.
2. Start `Normal` and confirm rival behavior still feels like the current baseline.
3. Start `Ultra` and confirm rival holes visibly route with less dithering.
4. Confirm rivals collect objects more effectively on `Ultra`.
5. Confirm rival scores are meaningfully closer to the player on `Ultra`.
6. Confirm rivals chase/contest better without feeling unfair or omniscient.
7. Confirm no browser console errors occur during the flow.

Validation performed on 2026-05-11:

- Extracted module script syntax check passed.
- Local preview server returned HTTP 200 for the candidate URL.
- Candidate content confirms the `Master 15.20` build marker.
- Candidate content confirms route-lookahead and wander-bias difficulty controls are present.
- Diff hygiene check passed with line-ending warnings only.
- User accepted the tests as passed with continued observation for difficulty scaling.

Validation still open:

- Continued observation of rival scoring and routing behavior in later builds.

Status:

- user-validated for continuation
- approved as the current basis for the next difficulty-scaling slice
