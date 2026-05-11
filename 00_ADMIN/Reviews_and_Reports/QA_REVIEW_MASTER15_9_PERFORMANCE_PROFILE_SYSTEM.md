# QA Review - Master 15.9 Performance Profile System

Date:

- 2026-05-10

Candidate under review:

- `C:\Users\KentLefner\Desktop\game-repo\Holesy-clean\20_TESTS\Candidate_Builds\Master 15.9 - performance-profile-system.html`

Baseline:

- `C:\Users\KentLefner\Desktop\game-repo\Holesy-clean\10_SOURCE\Masters\Master 15.html`

Backlog item:

- `PERF-001 Establish Performance Profile System`

Reason for candidate:

- active Priority 1 work has shifted to wave-system performance before new gameplay expansion
- later performance tasks need one controlled place for machine-capacity tiers before spawn caps, pooling, audio throttles, and debug counters are added
- the slice is intentionally narrow so the approved `Master 15` gameplay baseline remains easy to compare against

Implementation review:

- build marker advanced to `Master 15.9`
- added centralized `PERFORMANCE_PROFILES` with `low`, `medium`, `high`, and `ultra` tiers
- launch selection now checks a saved `holesyPerformanceProfile` value first, then falls back to simple device / viewport auto-detection
- renderer pixel ratio, antialiasing, shadow enablement, shadow-map size, fog distance, traffic movement controls, people panic bounds, music scheduler cadence, global gunshot spacing, wave soldier cadence / counts, tracer duration, plane speed, and plane-audio hear radius now read from the active profile
- debug overlay now reports the active performance profile so browser validation can confirm which tier is running

Risk controls:

- `high` preserves the approved `Master 15` wave, renderer, military, traffic, people, and audio baseline values
- `low` and `medium` reduce load through caps and cadence values without changing the core Waves rules or adding new game mechanics
- no master file or release package was changed in this candidate

Open validation required:

1. Launch the candidate on desktop and confirm the build marker shows `Master 15.9`.
2. Open the debug overlay and confirm the active performance profile is visible.
3. Force `localStorage.holesyPerformanceProfile` to `low`, `medium`, `high`, and `ultra` in browser testing and confirm the candidate still starts cleanly.
4. Play into Waves mode and confirm soldier waves still deploy, fight, and clear normally under the default profile.
5. Check low-profile Waves behavior specifically for smoother load and no broken transition, aid-drop, or soldier lifecycle behavior.

Status:

- candidate ready for browser validation
- not approved for promotion