# QA Review - Master 15.24 Wave Unit Cleanup Disposal

Date:

- 2026-05-11

Candidate under review:

- `C:\Users\KentLefner\Desktop\game-repo\Holesy-clean\20_TESTS\Candidate_Builds\Master 15.24 - wave-unit-cleanup-disposal.html`

Baseline:

- `C:\Users\KentLefner\Desktop\game-repo\Holesy-clean\20_TESTS\Candidate_Builds\Master 15.23 - wave-unit-geometry-pooling.html`

Backlog / issue basis:

- `PERF-004 Proper Disposal On Unit Cleanup`
- Planes, soldiers, and paratroopers were removed from the scene without a consistent disposal path after `PERF-003` introduced shared wave-unit geometries.

Reason for candidate:

- dispose short-lived per-instance resources when wave units leave the scene
- protect pooled shared geometries so later waves still render correctly

Implementation review:

- build marker advanced to `Master 15.24`
- added a shared-geometry protection set for pooled wave-unit geometries
- added `disposeWaveUnitObject()` and `removeWaveUnitObject()` helpers
- plane exit now stops engine audio, removes the mesh, and disposes per-instance resources
- soldier consumption now removes the mesh and disposes per-instance resources
- paratrooper landing conversion now removes the descent group and disposes per-instance resources before creating the landed soldier mesh
- wave teardown and menu cleanup now use the same wave-unit cleanup path

Risk controls:

- no `Master 15` source file was changed
- no website release package was changed
- shared pooled geometries are explicitly excluded from normal disposal
- tracer disposal remains unchanged because tracers own their short-lived geometry/material instances

Validation required:

1. Launch the candidate and confirm the build marker shows `Master 15.24`.
2. Start Waves mode and confirm troop planes render and exit without lingering visuals/audio.
3. Confirm paratroopers descend, land, and convert to soldiers without flicker or duplicates.
4. Eat landed soldiers and confirm they disappear immediately while score/reward behavior still works.
5. Play through repeated troop deployments and confirm later planes, paratroopers, and soldiers still render.
6. Confirm return-to-menu or round reset clears active wave units and non-music audio.
7. Confirm no browser console errors occur during spawns, conversions, consumption, exit, or reset.

Validation performed on 2026-05-11:

- Extracted module script syntax check passed.
- Local preview server returned HTTP 200 for the candidate URL.
- Candidate content confirms the `Master 15.24` build marker.
- Candidate diff confirms pooled shared geometries are protected from normal disposal.
- Candidate diff confirms plane, soldier, paratrooper, wave teardown, and menu cleanup paths use the new cleanup helper.
- In-app browser smoke check loaded the candidate with the `Master 15.24` marker visible and no console errors.

Validation still open:

- User gameplay validation of the full PERF-004 test set.

Status:

- candidate prepared for validation
- not approved for promotion
