# QA Review - Master 15.31 Skyscraper Collapse Prototype

Date:

- 2026-05-12

Candidate under review:

- `C:\Users\KentLefner\Desktop\game-repo\Holesy-clean\20_TESTS\Candidate_Builds\Master 15.31 - skyscraper-collapse-prototype.html`
- `C:\Users\KentLefner\Desktop\game-repo\Holesy-clean\20_TESTS\Candidate_Builds\Master 15.31 - skyscraper-collapse-prototype.css`
- `C:\Users\KentLefner\Desktop\game-repo\Holesy-clean\20_TESTS\Candidate_Builds\Master 15.31 - build-info.js`

Baseline:

- `C:\Users\KentLefner\Desktop\game-repo\Holesy-clean\20_TESTS\Candidate_Builds\Master 15.30 - game-stats-tracker.html`

Backlog / issue basis:

- Priority 3 Physics Stack And Collapse System
- User wants hole-game style stacked objects where pieces have individual physics behavior, fall when disturbed, and can be consumed while falling.
- Initial scope is skyscrapers only, with tall buildings broken into many edible chunks instead of one large object.

Reason for candidate:

- create the first playable in-game proof of falling/collapsing skyscraper chunks
- preserve current modes and tuning while introducing a narrow stack-physics behavior
- learn whether the feel validates before broadening to full building destruction or a physics-library integration

Implementation review:

- created `Master 15.31 - skyscraper-collapse-prototype.html`
- advanced build marker to `Master 15.31`
- carried forward modular CSS, difficulty profiles, and stats module
- changed only skyscraper generation:
  - skyscrapers now spawn as 20-28 stacked chunks
  - each chunk is an individual consumable object
  - nearby holes destabilize the stack
  - active chunks fall, tumble, bounce, settle, and remain consumable
- added cleanup for active stack pieces during world teardown

Risk controls:

- no source master was changed
- no website release package was changed
- small and mid buildings keep existing behavior
- first implementation uses lightweight in-game stack physics only for skyscrapers
- full cannon-es / deeper physics architecture remains deferred until this gameplay feel validates

Validation required:

1. Launch candidate and confirm build marker shows `Master 15.31`.
2. Start a game and confirm normal mode select / controls / HUD still work.
3. Approach a skyscraper and confirm it is made of visible stacked chunks.
4. Confirm nearby hole movement destabilizes the skyscraper.
5. Confirm chunks fall/tumble/settle rather than disappearing as one block.
6. Confirm chunks can be consumed while falling or after landing.
7. Confirm scoring/growth occurs per chunk.
8. Confirm no browser console errors during collapse and consumption.

Validation performed on 2026-05-12:

- Candidate file exists.
- CSS file exists.
- Build-info module exists.
- Candidate imports `Master 15.31` module files.
- Candidate content confirms skyscraper stack-piece logic exists.
- Local preview browser smoke check loaded the candidate.
- Local preview browser smoke check confirmed CSS, build-info, difficulty-profiles, and game-stats modules returned HTTP 200.
- Local preview browser smoke check confirmed the `Master 15.31` marker was visible.
- Local preview browser smoke check started gameplay and confirmed HUD appeared.
- Browser console error check returned no errors during load and startup smoke testing.

Validation still open:

- Player-facing collapse feel validation.
- User validation.

Status:

- ready for gameplay validation
