# QA Review - Master 15.39 Idle Lifecycle Cleanup

Date:

- 2026-05-13

Candidate under review:

- `C:\Users\KentLefner\Desktop\game-repo\Holesy\20_TESTS\Candidate_Builds\Master 15.39 - idle-lifecycle-cleanup.html`
- `C:\Users\KentLefner\Desktop\game-repo\Holesy\20_TESTS\Candidate_Builds\Master 15.39 - idle-lifecycle-cleanup.css`
- `C:\Users\KentLefner\Desktop\game-repo\Holesy\20_TESTS\Candidate_Builds\Master 15.39 - build-info.js`
- `C:\Users\KentLefner\Desktop\game-repo\Holesy\20_TESTS\Candidate_Builds\Master 15.39 - difficulty-profiles.js`
- `C:\Users\KentLefner\Desktop\game-repo\Holesy\20_TESTS\Candidate_Builds\Master 15.39 - game-stats.js`

Baseline:

- `C:\Users\KentLefner\Desktop\game-repo\Holesy\20_TESTS\Candidate_Builds\Master 15.38 - car-collision-system.html`

Backlog / issue basis:

- User reported leaving a Chrome tab open on the game menu for hours and then seeing an approximately 20-second delay between clicking the tab close button and the tab closing.
- The browser UI was unresponsive during that delay.
- Risk assessment: the game was continuing to maintain render/audio/window resources while sitting in a static menu state and did not have an explicit page-exit teardown path.
- Tracked issue: `QA-006`.

Implementation review:

- created `Master 15.39 - idle-lifecycle-cleanup.html`
- advanced build marker to `Master 15.39`
- copied the `Master 15.38` CSS and support modules to a matching `15.39` candidate set
- replaced unconditional animation-loop scheduling with tracked frame scheduling
- throttled static title, mode-select, and game-over rendering to 1 frame per second when the debug overlay is not visible
- added explicit wake behavior when game state changes so starting or resuming play is not delayed by the idle throttle
- tracked and cleared idle frame timers
- tracked music stop timers so repeated stop/start calls do not accumulate delayed cleanup callbacks
- added immediate WebAudio cleanup for scheduled music sources, the persistent sub-bass nodes, and the hole-wind loop
- added page-exit cleanup for gameplay timers, non-music audio, alien aid loops, plane engine drones, stats-window opener references, animation frames, and WebGL renderer resources
- preserved `pageshow` restoration for browser back-forward-cache restores

Risk controls:

- no source master was changed
- no website release package was changed
- candidate remains a narrow successor to `Master 15.38`
- gameplay simulation still uses normal requestAnimationFrame cadence whenever active play, pause, LMS choice, or wave transition is not in an idle menu state
- debug overlay disables idle throttling so live debug inspection remains responsive
- active run abandon recording still occurs before page-exit cleanup

Validation performed on 2026-05-13:

- Candidate HTML exists.
- CSS file exists.
- Build-info module exists and reports `BUILD_SUB = 39`.
- Difficulty-profile and game-stats modules exist.
- Candidate imports the matching `Master 15.39` module files.
- Extracted candidate module script passed JavaScript syntax check.
- Support JS modules passed syntax checks when checked as ES modules.
- Local HTTP preview returned HTTP 200 for the candidate route.
- In-app browser smoke check loaded the candidate with visible build marker `Master 15.39`.
- Browser console smoke check showed no errors or warnings on load.
- In-app browser tab close smoke check returned in approximately 197 ms after candidate load.

Validation still open:

- Long-idle Chrome validation matching the original user observation window.
- Player-facing gameplay validation after the lifecycle patch.
- Regression check that starting a run from the menu remains immediate despite the idle render throttle.

Status:

- ready for gameplay and long-idle validation
