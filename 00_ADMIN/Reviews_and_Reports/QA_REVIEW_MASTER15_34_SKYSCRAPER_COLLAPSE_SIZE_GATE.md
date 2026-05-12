# QA Review - Master 15.34 Skyscraper Collapse Size Gate

Date:

- 2026-05-12

Candidate under review:

- `C:\Users\KentLefner\Desktop\game-repo\Holesy-clean\20_TESTS\Candidate_Builds\Master 15.34 - skyscraper-collapse-size-gate.html`
- `C:\Users\KentLefner\Desktop\game-repo\Holesy-clean\20_TESTS\Candidate_Builds\Master 15.34 - skyscraper-collapse-size-gate.css`
- `C:\Users\KentLefner\Desktop\game-repo\Holesy-clean\20_TESTS\Candidate_Builds\Master 15.34 - build-info.js`

Baseline:

- `C:\Users\KentLefner\Desktop\game-repo\Holesy-clean\20_TESTS\Candidate_Builds\Master 15.33 - car-panic-escape.html`

Backlog / issue basis:

- User observed that split skyscraper chunks lost the original size gate, letting small holes collapse/eat skyscrapers too early.
- User observed that rivals who collapse buildings do not always value the resulting debris spill enough.
- User observed that eating collapsed building chunks sounded too much like the full building coming down.

Implementation review:

- created `Master 15.34 - skyscraper-collapse-size-gate.html`
- advanced build marker to `Master 15.34`
- carried forward modular CSS, difficulty profiles, and stats module
- added `stackCollapseSize` based on the original skyscraper footprint size
- blocks collapse only when the triggering hole can satisfy the original large-building consume size requirement
- collapsed chunks remain individually edible after valid collapse
- AI scoring now gives active/settled skyscraper chunks extra attraction and route lookahead value
- full collapse audio remains large and layered
- individual chunk consumption now uses a smaller single building-break sound

Risk controls:

- no source master was changed
- no website release package was changed
- correction is scoped to skyscraper stack pieces
- whole-building gate does not increase chunk size after collapse
- AI attraction is a bias, not a hard requirement, so rivals can still choose higher-priority threats, aid, or prey

Validation required:

1. Launch candidate and confirm build marker shows `Master 15.34`.
2. Confirm small holes cannot collapse skyscrapers.
3. Confirm sufficiently large holes can collapse skyscrapers.
4. Confirm collapsed chunks remain small enough to consume individually.
5. Confirm rivals sometimes remain near a fresh collapse to gather pieces.
6. Confirm full collapse sound is larger than individual chunk-eating sounds.
7. Confirm no browser console errors during collapse, chunk consumption, and rival collection.

Validation performed on 2026-05-12:

- Candidate file exists.
- CSS file exists.
- Build-info module exists.
- Candidate imports `Master 15.34` module files.
- Candidate content confirms `stackCollapseSize` exists.
- Candidate content confirms invalid small-hole collapse returns without activating the stack.
- Candidate content confirms fresh/settled collapse AI scoring bias exists.
- Candidate content confirms separate chunk-consumption audio exists.
- Local preview returned HTTP 200 for the candidate HTML, CSS, build-info, difficulty-profiles, and game-stats files.
- Extracted module script passed JavaScript syntax check.

Validation still open:

- Player-facing gameplay validation.
- User validation.

Status:

- ready for gameplay validation
