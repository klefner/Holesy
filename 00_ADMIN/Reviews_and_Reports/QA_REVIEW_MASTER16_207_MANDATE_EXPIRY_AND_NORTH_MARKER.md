# QA Review - Master 16.207 Mandate Expiry And North Marker

Date: 2026-08-07

## Scope

Restore the required game-over result when a wave expires before all Mandate targets are complete. Replace the first-person compass triangle with an arrow and add a low-cost north-facing sky marker visible only in Hole-Eye gameplay.

## Acceptance Checks

- `onWaveTimerExpired()` invokes Mandate failure enforcement before winner, final-wave, lore-drop, or next-wave branches.
- An incomplete Mandate marks unfinished rows failed, records `mandate_failed`, and calls `endGame()` without scheduling another wave.
- A completed Mandate continues through the existing winner/final-wave/transition logic.
- The compass uses an arrow glyph and preserves north-angle rotation.
- The giant sky `N` fades in as the Hole-Eye camera aligns with north and fades away outside the north-facing cone.
- The sky marker is visible only while Hole-Eye gameplay is active and is hidden in overhead view and after game end.
- The marker reuses the existing compass-angle calculation and adds no WebGL objects, textures, physics, simulation work, or draw calls.
- Source and release modular-package files remain byte-identical.

## Result

Passed local implementation and browser verification:

- `node --check` passed for source and release `main.js` and `build-info.js`.
- Static ordering confirmed Mandate failure enforcement precedes every wave winner, final-wave, lore-drop, and next-wave branch.
- A real incomplete Endless Wave 1 expired at `0/10`; the run ended on `Mandate Failed.` with `Endless Wave 1 closed before the Mandate was complete.` instead of entering Wave 2.
- Browser visual QA confirmed the compact compass renders `↑`, rotates toward north, and is visible only in Hole-Eye gameplay.
- The north-facing sky marker uses the existing north-angle value, reaches 22% opacity at true north, and has no WebGL object or asset cost.
- Returning to overhead view hid both orientation cues; game-over QA also confirmed the compass cleared.
- Browser console error check returned no errors.
- Source/release parity and publication remain to be verified after the final edits.
