# QA Review: Master 14.3 - input and wave-roster helpers

Reviewed candidate:

- `20_TESTS/Candidate_Builds/Master 14.3 - debug-overlay-input-and-wave-roster-helpers.html`

Baseline used:

- `20_TESTS/Candidate_Builds/Master 14.2 - debug-overlay-unit-clear-breakdown-helpers.html`

Objective:

- continue `P1.7` by separating soldier-consumption side effects and wave-roster bookkeeping into explicit helpers
- continue `P1.8` by exposing active input source, aid-drop timing, and live wave-roster state in the debug overlay

## What changed

- added debug overlay visibility for:
  - active input source and input detail
  - aid-drop countdown
  - live wave-roster summary
- extracted soldier-consumption responsibilities into helpers for:
  - consumed-soldier audio
  - soldier score/radius update
  - wave-roster update and reward dispatch
  - consumed-soldier removal

## QA findings

- status: pass for candidate handoff
- findings: no material static-review regression found in the touched seam

## Control check

- rollback safety:
  - preserved by creating `Master 14.3` as a new candidate instead of overwriting `Master 14.2`
- source-of-truth clarity:
  - backlog and handoff were updated to point at the new latest candidate
- change containment:
  - change stayed local to the debug-overlay seam and the soldier-consumption / wave-roster seam

## Remaining verification needed

- manual browser validation is still required before any promotion decision
- focused checks:
  - open the candidate locally
  - toggle the debug overlay with backtick
  - verify the overlay changes between keyboard, mouse, and touch ownership correctly
  - verify aid-drop countdown is sensible in relevant modes
  - trigger soldier consumption in Waves mode and confirm roster summary updates coherently
  - confirm unit-clear reward still fires once on a full wave clear

## Recommendation

- keep this as the active in-flight `P1.7` plus `P1.8` candidate pending user validation
