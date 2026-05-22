# QA Review - Master 16.28 Traffic and Soldier Growth

Date: 2026-05-22

## Scope

Defect repair for two user-reported issues in the modular Master 16 package:

- moving cars can appear to skid for unreasonable distances
- soldier damage can suppress normal object-devour growth until the next wave reset

## Product Intent Gate

Result: pass.

- The repair preserves the modular browser-client architecture.
- No new feature scope was added.
- The release package remains `index.html` plus modular `css/`, `js/`, `assets/`, and `data/` paths.

## Code Review Notes

- Root cause found for the traffic issue: non-crash car contact permanently altered `mesh.rotation.y`, while lane movement continued along the original axis. This allowed a car to keep driving normally while visually sideways.
- Fix: active moving cars now re-align to their lane direction every frame unless they are in the short crash animation or already a wreck.
- Root cause found for the growth issue: soldier shots increased `sizeResetScoreFloor`, creating hidden score debt that suppressed visible growth until a later reset.
- Fix: soldier shots now reduce the current radius/target radius directly without increasing the hidden growth floor.

## Validation

- Static JavaScript syntax check required before promotion.
- Source/release hash parity required for changed files.
- Local browser smoke recommended at the provided Master 16.28 URL.

## Residual Risk

- Traffic and soldier-growth behavior are interaction-heavy. Real gameplay confirmation should remain the final acceptance signal.
