## QA Review - Master 16.55 Endless Score and Cube Settle

Date: 2026-06-05

Scope:

- Preserve Endless live scores across world-shift hole-size resets.
- Keep AI rival holes active in Endless when soldiers shoot them down.
- Fix settled medium-office cubes that remain half-buried or diagonally balanced after coming to rest.

Change Summary:

- Endless world shifts now set each hole's `sizeResetScoreFloor` to the current score instead of setting score to zero.
- Soldier-killed AI rivals now respawn in Endless using the existing rival-recycle path so they can keep competing and scoring.
- Settled medium-office cubes snap to a cube-face orientation at `stackFloorY` after rest, including restored saved cubes.

Validation:

- `node --check 10_SOURCE/Masters/Master 16/js/main.js` passed.
- `node --check 10_SOURCE/Masters/Master 16/js/build-info.js` passed.
- Source and release package were refreshed to `Master 16.55`.

Local Test URL:

- `http://127.0.0.1:8798/index.html?v=16.55-endless-score-cube-settle`

Manual Test Focus:

- In Endless, advance through a world-shift wave and confirm live scores do not reset to zero.
- Let soldiers shoot AI rivals and confirm rivals return instead of staying greyed out forever.
- Collapse medium offices and confirm cubes still tumble while active, then settle flat on the ground once motion stops.
- Save/load an Endless run with settled cube debris and confirm restored settled cubes remain flat.

Residual Risk:

- Cumulative Endless score is now preserved at world shifts, while growth is still reset by using the score floor. Long-session score display should be monitored in real play beyond wave 20.
