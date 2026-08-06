# QA Review - Master 16.198 Harvest Density-Preserving Performance

Date: 2026-08-06

## Scope

- Preserve Harvest visual fullness, food economy, first-wave growth, and intact authored buildings
- Bring Harvest generation closer to the functioning parcel-led towns
- Reduce dormant destruction, ambient simulation, repeated scenery, shadow, and spatial-index costs
- Keep Mandates and Run Goals supply-feasible
- Verify modular source/release parity and browser stability

## Local browser evidence

- Harvest generated 3,243 loose edibles versus the Master 16.197 smoke baseline of 3,194.
- All 17 intact authored buildings remained present.
- Building destruction records fell from approximately 1,760 to 228 while combined building food value remained 8,804.
- Density telemetry reported 44 overlapping farmstead clusters and an 80-object starter trail.
- Harvest retained 211 workers and 156 ambient animals/riders/carts/tumbleweeds; distance activation reported only nearby actors awake.
- Final smoke telemetry reported median frame time 16.70 ms and p95 33.50 ms in the in-app test environment, with 292 draw calls and 13,414 rendered triangles at the sampled camera position.
- Run Goal telemetry capped selected targets to generated supply; the tested set reported buildings 132/228, trees 42/184, and animals 10/126.
- The tested Mandate used projected reachable supply and selected 11 north farm-building pieces.
- Screenshot review confirmed a dense starter feeding area and nearby farmstead clusters rather than an empty corner spawn.
- Browser console inspection reported no errors or warnings.
- A same-session Classic City regression smoke completed without console errors; its sampled median/p95 frame times were 33.30/66.60 ms, so the optimized Harvest sample was not slower than the functioning-town comparison in this environment.

## Static verification

- `node --check` passes for source and release `js/main.js` and `js/build-info.js`.
- Changed modular source/release package files match by content and SHA-256 after line-ending normalization.

## Result

Pass for local review. GitHub Pages verification pending publication.

## Limitations

- Frame measurements are browser-environment samples, not a substitute for a physical mobile-device playtest.
- The active handoff and several governance files contained unrelated pre-existing work and were intentionally excluded from this implementation commit.
