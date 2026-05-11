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

Validation performed on 2026-05-11:

- Extracted module script syntax check with `node --check`; result passed.
- Confirmed the candidate contains `low`, `medium`, `high`, and `ultra` profile tiers.
- Confirmed launch selection reads saved `localStorage.holesyPerformanceProfile` before fallback detection.
- Confirmed the candidate launches locally in browser at build marker `Master 15.9`.
- Started Waves mode and confirmed the HUD enters `Wave 1/4` without console errors.
- Opened the debug overlay and confirmed it reports `build: Master 15.9` and the active performance profile.
- Let the run advance into `Wave 2/4` under the active `low` profile; the debug overlay reported active soldiers, wave roster state, aid-drop state, and no browser console errors.

Validation still open:

- Forced browser verification for saved `medium`, `high`, and `ultra` profile values remains open because the browser automation security policy blocked programmatic `localStorage` injection. Manual console entry or another approved browser path is needed to complete that evidence.
- Capture before/after frame-time evidence before promotion.

Status:

- candidate partially browser-validated
- not approved for promotion
