# QA Review - Master 16.14 Endless Save / Load

Date: 2026-05-19

## Basis

- Source: `10_SOURCE/Masters/Master 16.html`
- Release package: `40_RELEASE/Website_Publish_Package/holesy/index.html`
- Upload package: `C:\Users\KentLefner\Downloads\holesy-production-upload\index.html`
- Build label: `Master 16.14`

## Scope

This review covers the first durable save/load slice for Endless Waves, allowing a player to save from Pause and resume later from the main menu.

## Implementation Review

- Added `Save Endless` to the Pause menu. It is only enabled during a paused Endless Waves run.
- Added `Load Endless` to the menu action row. It is enabled when a local Endless save exists.
- Save data is retained in local storage under a schema-versioned Endless save key.
- The save snapshot includes mode, difficulty, wave, score, live timers, arena scale, holes, active effects, lore buffs/cooldowns, round lore state, board objects, moving cars, soldiers, paratroopers, planes, aid ships, and active run elapsed time.
- Timed effects are saved as remaining milliseconds and restored relative to load time so offline breaks do not drain saved timers.

## Validation Performed

- `node --check` passed on the extracted module script from the source master.
- Source, release package, and upload package all show `Master 16.14`.
- Source, release package, and upload package are byte-identical after refresh.
- `git diff --check` passed.
- Browser smoke loaded the cache-busted local build and confirmed the visible badge shows `Master 16.14`.
- Browser smoke exercised the Endless path: start Endless, pause, save, return to menu, load, and confirm gameplay resumes on an Endless Wave with HUD visible.

## Open Validation

- Extended playtest should confirm a long Endless run can be saved after many waves, the browser can be closed/reopened, and the restored run feels continuous enough for production.
