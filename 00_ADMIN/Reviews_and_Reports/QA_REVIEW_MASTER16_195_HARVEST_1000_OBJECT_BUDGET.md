# QA Review - Master 16.195 Harvest 1,000-Object Budget

> Historical trial only. Rejected by the user on 2026-07-31 because the reduced population blocked first-wave growth and building Mandates. Superseded by Master 16.197, which restores the crowded Master 16.194 population.

Date: 2026-07-31

## Scope

- Exact Harvest County generation target
- Building count and destruction-piece reduction
- Loose-object and autonomous-actor reduction
- Starter-area population and local browser stability
- Modular source/release parity

## Evidence

- Browser generation telemetry reported target `1000` and generated total `1000`.
- The tested run contained 28 buildings, 560 building pieces, 440 other edibles, 36 workers, and 49 animals.
- Browser screenshot review confirmed nearby buildings, roads, animals, people, and loose objects remained visible at the player start.
- Browser console inspection reported no runtime errors.
- `node --check` passed for source and release JavaScript.
- Source and release hashes matched for all changed package files.
- All 559 release-package files matched the GitHub Pages `docs/` deployment tree by SHA-256 before publication.
- GitHub Pages commit `0011355` was pushed to `claude/happy-clarke-ORWAI`, and local/remote commit hashes matched after fetch.
- Public requests for `index.html`, `js/main.js`, and `js/build-info.js` returned HTTP 200 after Pages propagation.
- Public `js/main.js` exposed the 1,000-object Harvest target, 20-fragment building target, and generated-object telemetry; public build metadata reported `Master 16.195`.

## Result

Pass for local review and the standard GitHub Pages test deployment.
