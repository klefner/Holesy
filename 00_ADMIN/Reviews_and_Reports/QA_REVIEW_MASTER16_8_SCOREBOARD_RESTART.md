# QA Review - Master 16.8 Scoreboard Restart

Date:

- 2026-05-19

Candidate under review:

- `10_SOURCE/Masters/Master 16.html`
- `40_RELEASE/Website_Publish_Package/holesy/index.html`
- `C:\Users\KentLefner\Downloads\holesy-production-upload\index.html`

Backlog / issue basis:

- After an Endless scoreboard, clicking `Begin` could rebuild the town in the background but return to the same final-score screen.
- Root cause: fresh wave-based starts reset the world, but did not reset all hole alive/dead state before the wave rebuild.

Implementation review:

- Promoted the build to `Master 16.8`.
- Added a build-note entry for the scoreboard restart fix.
- Fresh `Waves` and `Endless Waves` starts now reset all hole life/state before rebuilding the wave arena.
- Between-wave survivor behavior remains unchanged because this reset only runs through `beginWaveRun()`, not `enterWaveTransition()`.

Validation performed:

- Extracted the browser module script and ran `node --check` successfully.
- Confirmed source, release package, and download-upload HTML all show `Master 16.8`.
- Confirmed source, release package, and download-upload HTML are byte-identical by SHA-256 hash.
- Local browser smoke confirmed `Master 16.8` loads.
- Local browser smoke started Endless, used pause-menu `End Game`, reached the scoreboard, clicked `Begin`, and confirmed gameplay restarted with the final screen hidden and `ENDLESS WAVE 1` visible.

Validation still open:

- User playtest confirmation after longer Endless runs with multiple rival deaths.

Status:

- Ready for user playtest and production promotion.
