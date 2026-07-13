# QA Review - Master 16.85 Run Goal Tooltips

Date: 2026-06-18

## Scope

`Master 16.85` makes the flavored Run Goal titles understandable without renaming them.

Changed behavior:

- hovering a Run Goal opens a high-contrast instruction tooltip
- the tooltip states the exact object family, required count, and score reward
- keyboard focus opens the same tooltip
- the implementation is CSS-driven and does not add gameplay-loop or persistence work

## Required Live-Play Validation

- Open `http://127.0.0.1:4173/index.html?fresh=16.85-goal-tooltips`.
- Confirm the build label shows `Master 16.85`.
- Start a run and hover each displayed goal.
- Confirm each tooltip is readable and explains exactly how to complete that goal.
- Confirm the tooltip does not cover the goal title or live-score panel incoherently.
- Confirm movement performance remains consistent with validated `Master 16.84`.

## Implementation-Session Validation

- Governed Team Sync was attempted but timed out during state collection; local source, release, branch, basis, and manifest state were checked directly.
- `node --check` passed for source `js/main.js` and `js/build-info.js`.
- Source and release hashes match for the five edited package files.
- In-app browser loaded `Master 16.85` with no console warnings or errors and confirmed generated help text such as `Devour 18 cars or trucks. Reward: +390 score.`.
- Browser focus inspection confirmed the goal row receives focus and the tooltip uses a dark `rgba(8, 12, 24, 0.98)` background with white text.
- The final browser reload timed out after the browser service stopped accepting fresh page loads; served-file and selector validation continued locally.

## Result

Implementation validation passed with the browser-service limitation above. Ready for user live-play validation.
