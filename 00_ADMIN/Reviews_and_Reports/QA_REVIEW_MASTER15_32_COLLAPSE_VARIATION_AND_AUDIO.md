# QA Review - Master 15.32 Collapse Variation And Audio

Date:

- 2026-05-12

Candidate under review:

- `C:\Users\KentLefner\Desktop\game-repo\Holesy-clean\20_TESTS\Candidate_Builds\Master 15.32 - collapse-variation-and-audio.html`
- `C:\Users\KentLefner\Desktop\game-repo\Holesy-clean\20_TESTS\Candidate_Builds\Master 15.32 - collapse-variation-and-audio.css`
- `C:\Users\KentLefner\Desktop\game-repo\Holesy-clean\20_TESTS\Candidate_Builds\Master 15.32 - build-info.js`

Baseline:

- `C:\Users\KentLefner\Desktop\game-repo\Holesy-clean\20_TESTS\Candidate_Builds\Master 15.31 - skyscraper-collapse-prototype.html`

Backlog / issue basis:

- Priority 3 Physics Stack And Collapse System
- User validation found the 15.31 prototype behaved like stacked floor slabs:
  - floors were too large but still edible
  - chunks fell too straight down and too similarly
  - floor alignment weakened the visual integrity of the standing skyscraper
  - collapse audio needs variation but must always sound like a building breaking apart

Implementation review:

- created `Master 15.32 - collapse-variation-and-audio.html`
- advanced build marker to `Master 15.32`
- preserved modular CSS, difficulty profiles, and stats module
- changed skyscraper generation so each floor is composed of 4, 6, or 8 smaller square-ish blocks
- kept the standing tower aligned to one clean rectangular footprint
- reduced chunk consume size so visual block size better matches hole consumption rules
- added varied collapse forces based on:
  - source-hole direction
  - outward fracture direction
  - random spill
  - nearby object obstruction
  - block-to-block contact
- kept one gravity constant for all stack pieces
- added layered building-collapse audio variation using building samples

Risk controls:

- no source master was changed
- no website release package was changed
- small and mid buildings keep existing behavior
- physics remains scoped to skyscraper chunks only
- collision is lightweight contact response, not a full physics-library integration

Validation required:

1. Launch candidate and confirm build marker shows `Master 15.32`.
2. Confirm mode select, difficulty select, stats panel, and begin flow still work.
3. Confirm standing skyscrapers look aligned and tower-like before collapse.
4. Confirm each floor is broken into smaller square-ish chunks, not one floor slab.
5. Confirm chunks vary collapse paths across runs and do not always fall straight down.
6. Confirm chunks bounce/contact each other lightly and feel heavy.
7. Confirm chunks remain consumable while falling and after settling.
8. Confirm collapse sounds vary but remain building-breaking-apart sounds.
9. Confirm no browser console errors during load, collapse, and consumption.

Validation performed on 2026-05-12:

- Candidate file exists.
- CSS file exists.
- Build-info module exists.
- Candidate imports `Master 15.32` module files.
- Candidate content confirms 4/6/8 block floor segmentation exists.
- Candidate content confirms collapse obstacle influence and block contact response exist.
- Candidate content confirms layered skyscraper collapse audio exists.
- Local preview returned HTTP 200 for the candidate HTML, CSS, build-info, difficulty-profiles, and game-stats files.
- Extracted module script passed JavaScript syntax check.

Validation still open:

- continue observation during future feature validation

Status:

- passed user validation; accepted for continued observation
