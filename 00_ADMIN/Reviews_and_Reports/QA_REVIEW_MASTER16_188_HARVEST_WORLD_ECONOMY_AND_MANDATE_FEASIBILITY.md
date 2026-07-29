# QA Review - Master 16.188 Harvest World Economy and Mandate Feasibility

Date: 2026-07-29

## Scope

- Harvest County opening growth economy
- visible and edible rural activity
- Wave 1 Mandate feasibility
- imported-animal consume scale
- source and release-package parity

## Expected Behavior

- Harvest County contains enough reachable, period-appropriate small content for a strong player to reach the smallest farm structures during the opening wave
- cultivated parcels visibly read as fields rather than empty lawns
- workers, mounted riders, horse-drawn carriages, farm equipment, and livestock populate parcels and roads
- taller central landmarks remain earned later targets
- a building-piece Mandate is offered only when at least one matching structure is reachable at a realistic projected player radius
- Harvest objectives use rural rather than urban wording
- imported animals never enlarge when consumption begins and shrink progressively during descent

## Local Verification

- JavaScript syntax passed for source and release `main.js` and `build-info.js`.
- SHA-256 parity passed for `index.html`, `js/main.js`, and `js/build-info.js`.
- Eight fresh randomized Harvest County starts completed without browser or page errors.
- All eight starts displayed Version 16.188 and completed Begin-to-populated-town construction in 1.47 to 1.90 seconds.
- Every start reported 1,111 edibles, 8 buildings, 86 animated animals/road-life actors, 14 fields, 58 workers, 8 riders, 8 carriages, and 7 tractors/plows.
- The projected contested-board player radius was 5.96.
- Sampled Mandates covered standing workers, roof chunks, north farm objects, east workers, north workers, east farm objects, moving workers, and small farm-building pieces.
- No run selected the previously impossible central tall-building target.
- Code-path verification confirmed animal entry begins at 92 percent of authored scale and uses an 11-unit full-shrink depth rather than the normal 27-unit descent.

## Risk

The feasibility model intentionally assumes the player captures 34 percent of reachable loose value because four holes compete for the board. This is safer than assuming a perfect clear, but live player testing should still evaluate whether the 5.96 projected opening radius feels too generous or restrictive.

## Publication

- Source and release candidate committed as `4b0da3f` and pushed to `codex/publish-master4-structure`.
- GitHub Pages package committed as `0569e0b` and pushed to `claude/happy-clarke-ORWAI`.
- Public verification at `https://klefner.github.io/Holesy/?v=16.188-0569e0b` displayed Version 16.188.
- A fresh public Harvest start completed in 5.70 seconds on a cold network run with preload ready, the full expected content telemetry, a reachable-radius estimate of 5.96, and the themed Mandate `Eat 11 South Farm Building Pieces`.
- The public browser run produced no page errors.
