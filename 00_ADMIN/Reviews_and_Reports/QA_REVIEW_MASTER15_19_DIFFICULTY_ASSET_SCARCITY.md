# QA Review - Master 15.19 Difficulty Asset Scarcity

Date:

- 2026-05-11

Candidate under review:

- `C:\Users\KentLefner\Desktop\game-repo\Holesy-clean\20_TESTS\Candidate_Builds\Master 15.19 - difficulty-asset-scarcity.html`

Baseline:

- `C:\Users\KentLefner\Desktop\game-repo\Holesy-clean\20_TESTS\Candidate_Builds\Master 15.18 - ultra-difficulty-tuning.html`

Backlog / issue basis:

- User reported higher difficulty should also mean fewer things to eat.
- User requested fewer people, cars, objects, and fewer tallest buildings relative to other buildings as difficulty increases.

Reason for candidate:

- make difficulty affect both threat pressure and food scarcity
- make `Ultra` feel harder through survival pressure and a tighter scoring/growth economy
- preserve `Normal` as the baseline economy

Implementation review:

- build marker advanced to `Master 15.19`
- difficulty profiles now include asset scarcity multipliers
- higher difficulty reduces populated building blocks
- higher difficulty reduces skyscraper probability relative to other buildings
- higher difficulty reduces small-building cluster density
- higher difficulty reduces sidewalk props and lamps
- higher difficulty reduces car count
- higher difficulty reduces park people/tree count and person spawn odds
- non-Waves starts now rebuild the city after the selected difficulty is applied
- Waves rebuilds continue to use the active difficulty economy between waves

Risk controls:

- no `Master 15` source file was changed
- no website release package was changed
- candidate is isolated to difficulty economy tuning after `15.18`

Validation required:

1. Launch the candidate and confirm the build marker shows `Master 15.19`.
2. Start `Normal` and confirm it preserves the familiar asset density.
3. Start `Hard` and confirm targets are somewhat scarcer than `Normal`.
4. Start `Ultra` and confirm targets are clearly harder to find than `Normal`.
5. Confirm `Ultra` has fewer obvious high-value/tall buildings relative to other buildings.
6. Confirm `Ultra` still remains beatable with strong play.
7. Confirm no browser console errors occur during the flow.

Validation performed on 2026-05-11:

- Extracted module script syntax check passed.
- Local preview server returned HTTP 200 for the candidate URL.
- Candidate content confirms the `Master 15.19` build marker.
- Candidate content confirms the difficulty asset-scarcity controls are present.
- Candidate content confirms difficulty description text mentions rare towers.
- Diff hygiene check passed with line-ending warnings only.

Validation still open:

- User confirmation of difficulty-scaled food scarcity.
- User confirmation that `Ultra` is hard but beatable.

Status:

- candidate prepared for validation
- not approved for promotion
