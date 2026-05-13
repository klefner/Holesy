# QA Review - Master 15.38 Car Collision And Stats Window

Date:

- 2026-05-12

Candidate under review:

- `C:\Users\KentLefner\Desktop\game-repo\Holesy-clean\20_TESTS\Candidate_Builds\Master 15.38 - car-collision-system.html`
- `C:\Users\KentLefner\Desktop\game-repo\Holesy-clean\20_TESTS\Candidate_Builds\Master 15.38 - car-collision-system.css`
- `C:\Users\KentLefner\Desktop\game-repo\Holesy-clean\20_TESTS\Candidate_Builds\Master 15.38 - build-info.js`

Baseline:

- `C:\Users\KentLefner\Desktop\game-repo\Holesy-clean\20_TESTS\Candidate_Builds\Master 15.37 - panic-crash-ramp.html`

Backlog / issue basis:

- User observed a long-standing graphical anomaly: cars can pass through each other.
- User wants cars to crash into each other or drive around each other, but not pass through.
- User also reported the temporary front-screen stats tracker is causing load delay.
- Stats should be hidden unless opened via button, open in a separate window, and refresh whenever a game ends.

Implementation review:

- created `Master 15.38 - car-collision-system.html`
- advanced build marker to `Master 15.38`
- added traffic collision tuning knobs for collision radius, avoidance radius, crash impact speed, contact braking, and avoidance nudging
- added car-to-car separation so overlapping cars are pushed apart
- added low-speed contact braking / deflection
- added high-speed, panic, crashing, or wreck contact conversion to crash state
- retained same-lane following behavior and traffic-light behavior
- removed the front-screen stats panel from startup rendering
- added a `Stats` button on the game select screen
- moved stats rendering into a separate popup window
- game-end stat writes now refresh the stats popup only when it is open
- closed popup behavior is passive: stored stats continue updating and render fresh on reopen

Risk controls:

- no source master was changed
- no website release package was changed
- collision pass is scoped to cars tracked in `movingCars`
- stats data storage remains localStorage-backed through the existing stats module
- stats popup rendering is opt-in and should not add front-screen load cost

Validation required:

1. Launch candidate and confirm build marker shows `Master 15.38`.
2. Confirm front screen no longer displays the full stats tracker.
3. Confirm `Stats` button opens a separate stats window.
4. Confirm stats window can be closed and reopened.
5. Confirm completed/abandoned game writes refresh the stats window when open.
6. Confirm stats still update while the stats window is closed.
7. Confirm normal cars no longer pass through each other.
8. Confirm low-speed car contact brakes/separates instead of overlap.
9. Confirm high-speed/panic car contact can produce crash/wreck behavior.
10. Confirm no browser console errors during stats popup open/reset, game-end stats update, car collision, and crash.

Validation performed on 2026-05-12:

- Candidate file exists.
- CSS file exists.
- Build-info module exists.
- Candidate imports `Master 15.38` module files.
- Candidate content confirms car collision separation / avoidance / high-impact crash logic exists.
- Candidate content confirms front-screen stats panel is removed.
- Candidate content confirms optional stats popup open/refresh/reset logic exists.
- Local preview returned HTTP 200 for candidate HTML, CSS, build-info, difficulty-profiles, and game-stats files.
- Extracted module script passed JavaScript syntax check.

Validation still open:

- Player-facing gameplay validation.
- User validation.

Status:

- ready for gameplay validation
