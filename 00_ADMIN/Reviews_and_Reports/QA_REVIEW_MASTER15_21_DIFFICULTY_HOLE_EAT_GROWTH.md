# QA Review - Master 15.21 Difficulty Hole-Eat Growth

Date:

- 2026-05-11

Candidate under review:

- `C:\Users\KentLefner\Desktop\game-repo\Holesy-clean\20_TESTS\Candidate_Builds\Master 15.21 - difficulty-hole-eat-growth.html`

Baseline:

- `C:\Users\KentLefner\Desktop\game-repo\Holesy-clean\20_TESTS\Candidate_Builds\Master 15.20 - rival-ai-difficulty-scaling.html`

Backlog / issue basis:

- User identified another difficulty throttle: growth from eating another hole should be higher on easier mode and lower on harder modes.
- User accepted the current difficulty-scaling tests as passed while continuing observation.

Reason for candidate:

- add difficulty scaling to rival-consumption rewards
- preserve easier-mode payoff while limiting snowballing on hard modes

Implementation review:

- build marker advanced to `Master 15.21`
- difficulty profiles now include hole-eat score and growth multipliers
- `Normal` gives a larger hole-eat reward than baseline
- `Hard` reduces both score and direct radius gain from eating another hole
- `Ultra` substantially reduces both score and direct radius gain from eating another hole
- victim bonus-radius absorption is scaled by the same growth multiplier

Risk controls:

- no `Master 15` source file was changed
- no website release package was changed
- candidate is isolated to difficulty scaling for hole-eat rewards

Validation required:

1. Launch the candidate and confirm the build marker shows `Master 15.21`.
2. Confirm eating another hole still works on all difficulty levels.
3. Confirm `Normal` gives the largest growth payoff.
4. Confirm `Hard` gives less growth than `Normal`.
5. Confirm `Ultra` gives the least growth.
6. Confirm no browser console errors occur during the flow.

Validation performed on 2026-05-11:

- Extracted module script syntax check passed.
- Local preview server returned HTTP 200 for the candidate URL.
- Candidate content confirms the `Master 15.21` build marker.
- Candidate content confirms hole-eat score and growth controls are present.
- Diff hygiene check passed with line-ending warnings only.

Validation still open:

- User confirmation of difficulty-scaled hole-eat growth.

Status:

- candidate prepared for validation
- not approved for promotion
