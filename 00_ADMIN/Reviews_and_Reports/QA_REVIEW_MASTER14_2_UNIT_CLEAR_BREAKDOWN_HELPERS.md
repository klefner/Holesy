# QA Review: Master 14.2 - unit-clear breakdown helpers

Reviewed candidate:

- `20_TESTS/Candidate_Builds/Master 14.2 - debug-overlay-unit-clear-breakdown-helpers.html`

Baseline used:

- `20_TESTS/Candidate_Builds/Master 14.1 - debug-overlay-unit-clear-refund.html`

Objective:

- continue `P1.7` by separating unit-clear reward math and debug bookkeeping into explicit helper functions
- continue `P1.8` by exposing more of the unit-clear refund breakdown in the debug overlay

## What changed

- extracted refund-roll logic into `rollUnitClearRefundRatio()`
- extracted reward calculation into `buildUnitClearRewardBreakdown()`
- extracted debug state capture into `recordUnitClearDebug()`
- expanded debug overlay to show:
  - tracked damage cap
  - refunded damage mass
  - post-clear remaining soldier-damage bank
- preserved the existing player-facing unit-clear reward, banner, and speed-boost behavior

## QA findings

- status: pass for candidate handoff
- findings: no material code-path mismatch found in the static review

## Control check

- rollback safety:
  - preserved by creating a new candidate build instead of modifying `Master 14` or overwriting `Master 14.1`
- source-of-truth clarity:
  - backlog and handoff documents were updated to point at the active in-flight candidate
- change containment:
  - change stayed local to the unit-clear reward / debug-overlay seam

## Remaining verification needed

- manual browser validation is still required before any promotion decision
- focused test checks:
  - open the candidate locally
  - toggle the debug overlay with backtick
  - trigger a full soldier-unit clear in Waves mode
  - confirm overlay fields update coherently for:
    - tracked damage bank before clear
    - tracked damage cap
    - refund percentage
    - refunded damage mass
    - post-clear bank
  - confirm the visible reward pop, growth bump, shield effect, and speed boost still behave as in `Master 14.1`

## Recommendation

- keep this as the current in-flight `P1.7` plus `P1.8` candidate pending user validation
