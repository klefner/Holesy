# QA Review - Master 16.4 Buff Cooldown And Collapse Variety

Date: 2026-05-19

## Scope

- Source master: `10_SOURCE/Masters/Master 16.html`
- Release package: `40_RELEASE/Website_Publish_Package/holesy/index.html`
- Upload copy: `C:\Users\KentLefner\Downloads\holesy-production-upload\index.html`

## Changes

- Timed lore buffs now enter a 5-second reacquire cooldown after they expire.
- Active timed buffs can still be refreshed while active, but once the timer reaches zero, the same buff cannot be acquired again until the cooldown ends.
- Linden Street's instant mass/score bonus no longer fires during the expired-buff cooldown window.
- Skyscraper collapse plans now choose a visible style: forward topple, pancake drop, split shear, or twisting failure.
- Stack pieces now use staggered floor delays so buildings fail in waves instead of all chunks moving at the same time.

## Verification

- `node --check` passed for the extracted module script in `10_SOURCE/Masters/Master 16.html`.
- Release package and upload copy were regenerated from the same master file.
- Build label updated to `Master 16.4` in source, release package, upload copy, and in-game build notes.
- Local release package served at `http://127.0.0.1:8778/index.html` returned HTTP 200.
- In-app browser load smoke confirmed the local package opens on `Master 16.4` with the expected `Begin` entry point.

## Residual Risk

Manual playtest should focus on whether the collapse styles are now visibly different during normal play and whether buff cooldowns feel understandable without adding noisy UI.
