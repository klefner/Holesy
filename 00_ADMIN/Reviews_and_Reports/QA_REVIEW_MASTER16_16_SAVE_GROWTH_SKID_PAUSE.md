## QA Review: Master 16.16 Saved Visuals, Growth Recovery, Skid Tuning, Pause Version

Date: 2026-05-20

Reviewer: Codex

Scope:

- Source master: `10_SOURCE/Masters/Master 16.html`
- Release package: `40_RELEASE/Website_Publish_Package/holesy/index.html`
- GoDaddy upload copy: `C:\Users\KentLefner\Downloads\holesy-godaddy-upload-master-16.16\holesy\index.html`
- In-game build label: `Master 16.16`

## Product Intent Gate

Request was a narrow gameplay hotfix and release package refresh:

- restore saved Endless skyscraper visuals after loading a save
- prevent soldier damage from leaving the player unable to grow after escaping
- further reduce unreasonable car skid distances
- show the current game version in the Pause menu

This did not conflict with the current source-of-truth manifest, modular architecture target, or backlog recommendation. The work stayed on the approved `Master 16` source file and refreshed the governed release package from that source.

## Changes Reviewed

- Saved skyscraper chunks now persist stack-piece dimensions and restore through a skyscraper-specific loader that recreates windowed/capped skyscraper geometry instead of generic block placeholders.
- Soldier damage now applies as bounded negative `bonusRadius` against base score-derived radius, with devoured objects repairing that damage debt before applying new growth.
- `radiusFromScore()` now lower-clamps to the survivable radius floor and upper-clamps to the Endless board-share cap.
- Panic car crashes now cap initial skid velocity, shorten crash duration, lower slide distance, and reduce spin impulse.
- Pause menu now displays the active `BUILD_LABEL`.
- `BUILD_CHANGELOG` includes a `Master 16.16` entry.

## Validation Performed

- Extracted the module script from `Master 16.html` and ran `node --check` against it as `.mjs`: passed.
- Refreshed `40_RELEASE/Website_Publish_Package/holesy/index.html` from source master.
- Refreshed `C:\Users\KentLefner\Downloads\holesy-godaddy-upload-master-16.16\holesy\index.html` from source master.
- SHA256 comparison:
  - source: `11021F8144DE738C612CBCE980E6C61D4474F8EE51C1B779C2502AB7FC0A091F`
  - release: `11021F8144DE738C612CBCE980E6C61D4474F8EE51C1B779C2502AB7FC0A091F`
  - upload copy: `11021F8144DE738C612CBCE980E6C61D4474F8EE51C1B779C2502AB7FC0A091F`
- Ran `00_ADMIN/Tools/holesy_team_sync.ps1` after packaging: source/release hash OK, automation drift OK, issue counts open=0 monitor=3 resolved=9, status verified current with stated limitations.
- Attempted automated browser smoke via Node/Playwright, but this local environment does not have the `playwright` module installed; live browser validation remains a user-playtest item for the specific observed saved-game and soldier-damage paths.

## Residual Risk

- Real browser validation is still needed for the exact user-observed saved-skyscraper visual state because it depends on loading an Endless save that contains skyscraper chunks.
- Soldier growth recovery should be validated through live play by allowing soldiers to shrink the player, escaping, and then eating objects before the next five-wave reset.
- Car skid tuning should be judged in live gameplay because traffic interactions are probabilistic.

## Result

Approved for user gameplay testing as `Master 16.16`.
