# QA Review - Master 15.42 Buff Clarity And Rare Docs

Date:

- 2026-05-13

Candidate under review:

- `C:\Users\KentLefner\Desktop\game-repo\Holesy\20_TESTS\Candidate_Builds\Master 15.42 - buff-clarity-and-rare-docs.html`
- `C:\Users\KentLefner\Desktop\game-repo\Holesy\20_TESTS\Candidate_Builds\Master 15.42 - buff-clarity-and-rare-docs.css`
- `C:\Users\KentLefner\Desktop\game-repo\Holesy\20_TESTS\Candidate_Builds\Master 15.42 - build-info.js`
- `C:\Users\KentLefner\Desktop\game-repo\Holesy\20_TESTS\Candidate_Builds\Master 15.42 - difficulty-profiles.js`
- `C:\Users\KentLefner\Desktop\game-repo\Holesy\20_TESTS\Candidate_Builds\Master 15.42 - game-stats.js`
- `C:\Users\KentLefner\Desktop\game-repo\Holesy\20_TESTS\Candidate_Builds\Master 15.42 - lore-documents.js`

Baseline:

- `C:\Users\KentLefner\Desktop\game-repo\Holesy\20_TESTS\Candidate_Builds\Master 15.41 - archive-music.html`

Backlog / issue basis:

- User reported that lore-buff names such as town names did not explain what happened to the player.
- User reported that notification messages vanished too quickly to follow.
- User reported seeing a buff icon without knowing whether it meant speed, defense, score, slowdown, or another effect.
- User requested combinations where most combos help and a smaller number can harm.
- User reported collecting 8 documents in a single round and requested rare document drops, with a maximum of one document per round and no document in most rounds.

Implementation review:

- created `Master 15.42 - buff-clarity-and-rare-docs.html`
- advanced build marker to `Master 15.42`
- replaced lore-only achievement labels with player-readable names and explicit effect text
- changed center-screen achievement feedback to generic `BUFF ACTIVE` / `ACHIEVEMENT` copy and moved details to longer event banners
- added effect descriptions and `ready` / `round` states to the active effect tray
- added active banners for Tree Feast, Crowd Magnet, Block Sweep, Building Chain, and Quiet Bite
- added a beneficial combo: Crowd Magnet plus Block Sweep improves pull and scoring
- added a limited risky combo: Crowd Magnet plus Building Chain increases building scoring but slows the player temporarily
- removed achievement-triggered automatic document unlocks
- changed end-of-round document drops to a rare chance-based award
- capped document drops at one per round

Risk controls:

- no source master was changed
- no website release package was changed
- document unlock pacing is isolated to lore drop functions and round lore state
- achievement unlocks still persist separately from document unlocks
- combo harm is intentionally limited to a single readable slowdown case
- notification durations were increased without changing the core render loop

Validation performed on 2026-05-13:

- Candidate HTML exists.
- CSS file exists.
- Build-info module exists and reports `BUILD_SUB = 42`.
- Difficulty-profile, game-stats, and lore-documents modules exist.
- Extracted candidate module script passed JavaScript syntax check.
- Support JS modules passed syntax checks when checked as ES modules.
- Local HTTP preview returned HTTP 200 for candidate HTML.

Validation still open:

- Browser gameplay check that active buff tray entries explain what the buff does.
- Browser gameplay check that the risky combo is noticeable but not over-punishing.
- Browser gameplay check that document drops are rare across repeated rounds.
- User validation that the new buff language is obvious during play.

Status:

- ready for gameplay validation
