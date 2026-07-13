# QA Review - Master 16.83 Run Goal Tuning And Performance

Date: 2026-06-15

## Scope

`Master 16.83` repairs the `Master 16.82` Run Goals regression reported in live play: movement became choppy and object motion felt slower because the goals UI was being rebuilt too often.

Changed behavior:

- Run Goals UI updates are dirty-flagged instead of rewritten every HUD refresh.
- The goal pool now contains 50 larger predefined goals.
- Each goal set chooses three different object families when possible.
- Completing all displayed goals awards a visible Goal Sweep reward: bonus score plus a short speed surge.
- Endless refreshes goal sets every five waves; timed and standard wave runs receive goals at run start.

## Required Live-Play Validation

- Open `http://127.0.0.1:4173/index.html?fresh=16.83-run-goal-tuning`.
- Confirm the build label shows `Master 16.83`.
- Start a timed run and confirm movement feels smooth again.
- Confirm three larger goals appear, not tiny goals like 15 people.
- Confirm the all-goals benefit is visible in the Run Goals panel.
- Complete at least one goal and confirm score/progress updates without stutter.
- In Endless, confirm new goals are issued on wave 6 if the run reaches that point.
- Confirm MegaKit Downtown and Classic Aldine still load normally.

## Implementation-Session Validation

- Team Sync completed and allowed the implementation under the current product/architecture gates.
- `node --check` passed for source `js/main.js`.
- `node --check` passed for source `js/build-info.js`.
- Source and release hashes match for `index.html`, `css/styles.css`, `js/main.js`, and `js/build-info.js`.
- In-app browser smoke at `http://127.0.0.1:4173/index.html?fresh=16.83-run-goal-tuning-2` rendered `Master 16.83`.
- Browser gameplay start rendered three larger randomized goals, visible `All goals +750 + 12s speed` reward text, no Run Goals / live-score panel overlap, and no console warnings or errors.

Limitation:

- User live-play validation is still needed for smoothness, goal sizing, and whether Goal Sweep feels rewarding enough.

## Result

Implementation smoke validation passed with the live-play limitation above. Ready for user validation.
