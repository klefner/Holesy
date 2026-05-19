# QA Review - Master 16.6 Endless Waves

Date:

- 2026-05-19

Candidate under review:

- `10_SOURCE/Masters/Master 16.html`
- `40_RELEASE/Website_Publish_Package/holesy/index.html`
- `C:\Users\KentLefner\Downloads\holesy-production-upload\index.html`

Backlog / issue basis:

- Priority 5 `Endless Waves mode`
- User-approved implementation plan for unbounded Endless Waves from `Master 16.5`

Implementation review:

- Added `Endless Waves` as a fourth selectable game mode.
- Added generated Endless wave configs with `ultraReferenceWave: 75`.
- Added Endless pressure scaling for soldiers, aid scarcity, rival AI, object density, score pressure, and growth pressure.
- Preserved standard four-wave `Waves` mode by routing only wave-based runtime reads through shared helpers.
- Added Endless HUD text, end-state copy, stats normalization, best-wave tracking, and voluntary pause-menu cashout.
- Refreshed the website release package and production-upload HTML from the source master.

Validation performed:

- Extracted the browser module script and ran `node --check` successfully.
- Confirmed source, release package, and download-upload HTML all show `Master 16.6`.
- Confirmed source, release package, and download-upload HTML are byte-identical by SHA-256 hash.
- Local server returned HTTP 200 for `http://127.0.0.1:8778/index.html`.
- Browser smoke confirmed the menu shows Timed, LMS, Waves, and Endless Waves.
- Browser smoke confirmed Endless Waves starts and displays `ENDLESS WAVE 1`.
- Browser smoke confirmed pause in Endless shows `End Game`.
- Browser smoke confirmed Endless voluntary cashout displays `Withdrawal Recorded` and did not award a document.
- Browser smoke confirmed standard Waves still starts as `WAVE 1/4`.

Validation still open:

- Long-run balancing beyond early Endless waves.
- Live mobile-device playtest after production upload.
- Deep-run confirmation that fewer than 1% of players pass Wave 100.

Status:

- Ready for user playtest and production promotion.
