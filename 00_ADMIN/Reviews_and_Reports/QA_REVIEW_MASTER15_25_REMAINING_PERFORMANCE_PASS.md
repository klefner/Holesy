# QA Review - Master 15.25 Remaining Performance Pass

Date:

- 2026-05-12

Candidate under review:

- `C:\Users\KentLefner\Desktop\game-repo\Holesy-clean\20_TESTS\Candidate_Builds\Master 15.25 - remaining-performance-pass.html`

Baseline:

- `C:\Users\KentLefner\Desktop\game-repo\Holesy-clean\20_TESTS\Candidate_Builds\Master 15.24 - wave-unit-cleanup-disposal.html`

Backlog / issue basis:

- `PERF-002 Concurrent Wave Cap With Soft Pressure Valve`
- `PERF-005 Reuse Paratrooper Soldier Mesh On Landing`
- `PERF-006 Throttle Per-Plane Engine Audio Updates`
- `PERF-007 Clear waveRosters On Game End`

Reason for candidate:

- complete the remaining active wave-system performance backlog as one coordinated candidate
- remove avoidable troop-deployment allocation while preserving 15.24 cleanup/disposal protections
- bound plane pressure by performance profile and reduce per-frame audio API work
- prevent leftover wave roster state after game end

Implementation review:

- build marker advanced to `Master 15.25`
- performance profiles now define `maxConcurrentPlanes` and `planeEngineAudioHz`
- wave deployment timer pauses while the active plane count is at the profile cap
- plane engine audio updates are throttled by profile while plane movement remains per-frame
- paratrooper landing now reparents the existing descending soldier mesh instead of creating a new soldier mesh
- paratrooper landing disposes the parachute portion only and keeps pooled shared geometries protected
- `waveRosters` is cleared through a shared helper on reset, wave teardown, and game end
- debug overlay now shows active plane count/cap and plane audio update rate

Risk controls:

- no `Master 15` source file was changed
- no website release package was changed
- candidate is isolated to the remaining active performance backlog items
- gameplay difficulty values remain separate from performance profile caps
- pooled shared geometries remain protected from normal cleanup disposal

Validation required:

1. Launch the candidate and confirm the build marker shows `Master 15.25`.
2. Start Waves mode and confirm paratroopers descend, land, and become soldiers without flicker or duplicates.
3. Confirm troop deployment continues across repeated waves after soldier mesh reparenting.
4. Confirm the debug overlay shows plane count/cap and plane audio update rate.
5. In a profile with a low plane cap, confirm troop deployment pauses while at cap and resumes after a plane exits.
6. Confirm plane engine audio still sounds continuous enough and stops when planes exit or the menu/scoreboard appears.
7. Confirm ending or leaving a run clears wave roster debug state.
8. Confirm no browser console errors occur during spawns, landing conversion, plane exit, round end, or menu return.

Validation performed on 2026-05-12:

- Extracted module script syntax check passed.
- Local preview server returned HTTP 200 for the candidate URL.
- Candidate content confirms the `Master 15.25` build marker.
- Candidate content confirms profile plane caps, plane-audio throttle controls, mesh reparenting, and roster clearing are present.
- In-app browser smoke check loaded the candidate with the `Master 15.25` marker visible and no console errors.

Validation still open:

- User gameplay validation of the full 15.25 performance test set.

Status:

- candidate prepared for validation
- not approved for promotion
