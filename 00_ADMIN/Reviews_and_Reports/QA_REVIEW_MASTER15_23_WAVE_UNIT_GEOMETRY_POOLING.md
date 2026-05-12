# QA Review - Master 15.23 Wave Unit Geometry Pooling

Date:

- 2026-05-11

Candidate under review:

- `C:\Users\KentLefner\Desktop\game-repo\Holesy-clean\20_TESTS\Candidate_Builds\Master 15.23 - wave-unit-geometry-pooling.html`

Baseline:

- `C:\Users\KentLefner\Desktop\game-repo\Holesy-clean\20_TESTS\Candidate_Builds\Master 15.22 - player-eaten-return.html`

Backlog / issue basis:

- `PERF-003 Geometry Pooling For Wave Units`
- Wave unit constructors were allocating fresh plane, soldier, parachute canopy, and parachute cord geometries on every spawn.

Reason for candidate:

- reduce GPU upload churn and garbage-collection pressure during wave spawns
- preserve visual output while preparing the cleanup path for later performance slices

Implementation review:

- build marker advanced to `Master 15.23`
- added one shared `WAVE_UNIT_GEOMETRIES` set for wave unit geometry
- plane body, wing, tail, fin, and engine geometries now reuse shared geometry references
- soldier body, legs, helmet, head, and rifle geometries now reuse shared geometry references
- parachute canopy and cord geometries now reuse shared geometry references
- mesh wrappers, transforms, and per-spawn placement remain per instance

Risk controls:

- no `Master 15` source file was changed
- no website release package was changed
- candidate is isolated to geometry reuse for existing wave units
- no intended gameplay or visual behavior change
- tracer geometry remains per instance because tracers are short-lived and independently disposed

Validation required:

1. Launch the candidate and confirm the build marker shows `Master 15.23`.
2. Start Waves mode and confirm troop planes render normally.
3. Confirm paratroopers render normally during descent.
4. Confirm landed soldiers render normally and continue moving/firing.
5. Confirm no browser console errors occur during wave spawns and cleanup.

Validation performed on 2026-05-11:

- Extracted module script syntax check passed.
- Local preview server returned HTTP 200 for the candidate URL.
- Candidate content confirms the `Master 15.23` build marker.
- Candidate content confirms shared wave-unit geometry controls are present.
- Diff hygiene check passed with line-ending warnings only.
- In-app browser smoke check loaded the candidate with the `Master 15.23` marker visible and no console errors.
- In-app browser smoke check started Waves mode on `Ultra` and waited through the first troop-spawn window with no console errors.

Validation still open:

- None.

User validation on 2026-05-11:

- All tests passed.

Status:

- user validation passed
- accepted as the active basis for the next candidate slice
