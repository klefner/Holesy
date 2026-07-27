# QA Review: Master 16.185 Crownlands Secret Unlock

Date: 2026-07-27

## Scope

- Track a four-achievement constellation inside one whole game rather than one wave.
- Ignore achievements earned before the current run.
- Preserve incomplete progress only in an explicit Endless save.
- Permanently record completion while keeping unfinished Crownlands hidden.
- Add cryptic Archive clues.

## Verification

- `node --check` passed for `js/main.js`, `js/build-info.js`, and `data/lore-documents.js`.
- SHA-256 parity passed for all four changed runtime files.
- Browser boot confirmed Version 16.185.
- The city selector contained no Crownlands entry.
- The Archive exposed *Margin Note in a Road Atlas* from the start.
- Browser text and screenshot review confirmed the clue describes four proofs, one traveler, and that old medals do not satisfy the gate.
- Code-path review confirmed `startLoreRunTracking()` creates a fresh `runAchievements` array, wave transitions do not call that reset, `buildEndlessSaveState()` serializes `roundLoreState`, and `restoreRoundLoreState()` restores the array.
- Completion persists only the final Crownlands gate and unlocks the final Archive routing slip.

## Result

Pass for the unlock contract and clue presentation. Crownlands remains intentionally unavailable until its environment package is complete.
