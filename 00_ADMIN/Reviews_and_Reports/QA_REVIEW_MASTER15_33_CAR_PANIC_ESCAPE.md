# QA Review - Master 15.33 Car Panic Escape

Date:

- 2026-05-12

Candidate under review:

- `C:\Users\KentLefner\Desktop\game-repo\Holesy-clean\20_TESTS\Candidate_Builds\Master 15.33 - car-panic-escape.html`
- `C:\Users\KentLefner\Desktop\game-repo\Holesy-clean\20_TESTS\Candidate_Builds\Master 15.33 - car-panic-escape.css`
- `C:\Users\KentLefner\Desktop\game-repo\Holesy-clean\20_TESTS\Candidate_Builds\Master 15.33 - build-info.js`

Baseline:

- `C:\Users\KentLefner\Desktop\game-repo\Holesy-clean\20_TESTS\Candidate_Builds\Master 15.32 - collapse-variation-and-audio.html`

Backlog / issue basis:

- User wants cars to react like people: shift from normal driving to faster escape when a hole is nearby.
- Escape should be noticeable but not unreasonably fast.
- Some panicked cars should lose control, leave the road, crash into people/buildings/lightposts, stop moving, smoke, sometimes flame, and sometimes explode.
- Moving cars should be worth more than crashed wrecks because they are harder to catch.

Implementation review:

- created `Master 15.33 - car-panic-escape.html`
- advanced build marker to `Master 15.33`
- carried forward modular CSS, difficulty profiles, and stats module
- added traffic panic tuning to `HOLESY_CONFIG.traffic`
- added moving-car threat detection from nearby holes
- cars accelerate only when their current lane direction moves them away from the threat
- panicked cars wobble slightly and can lose control at higher speed
- loss-of-control cars leave road-constrained movement and can crash into world objects
- crashed cars stop, become easy to catch, emit smoke, sometimes flame, and rarely explode
- moving cars now carry a higher value than crashed cars

Risk controls:

- no source master was changed
- no website release package was changed
- normal traffic-light and follow-distance behavior remains in place for non-panicked cars
- crash behavior is probabilistic and gated by panic speed to avoid every car becoming chaos
- smoke/explosion visuals use short-lived lightweight meshes and are cleared on teardown

Validation required:

1. Launch candidate and confirm build marker shows `Master 15.33`.
2. Confirm mode select and begin flow still work.
3. Confirm normal cars still follow roads and lights when no hole is nearby.
4. Confirm some cars accelerate when a nearby hole is behind or beside them and the lane direction carries them away.
5. Confirm panicked cars do not accelerate unrealistically.
6. Confirm some fast panicked cars wobble, leave the lane, and crash.
7. Confirm crashed cars stop moving and become easier to consume.
8. Confirm smoke appears from crashed cars.
9. Confirm some wrecks show flame and rare explosion.
10. Confirm moving cars award more points than crashed cars.
11. Confirm no browser console errors during traffic panic, crash, and consumption.

Validation performed on 2026-05-12:

- Candidate file exists.
- CSS file exists.
- Build-info module exists.
- Candidate imports `Master 15.33` module files.
- Candidate content confirms traffic panic tuning exists.
- Candidate content confirms car crash, smoke, flame, and explosion handling exists.
- Candidate content confirms moving/crashed car point values exist.
- Local preview returned HTTP 200 for the candidate HTML, CSS, build-info, difficulty-profiles, and game-stats files.
- Extracted module script passed JavaScript syntax check.

Validation still open:

- Player-facing traffic behavior validation.
- User validation.

Status:

- ready for gameplay validation
