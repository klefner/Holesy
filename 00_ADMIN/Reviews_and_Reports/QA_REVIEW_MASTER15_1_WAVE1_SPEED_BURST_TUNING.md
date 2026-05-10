# QA Review — Master 15.1 Wave 1 Speed Burst Tuning

Date:

- 2026-04-30

Candidate under review:

- `C:\Users\KentLefner\Desktop\game-repo\Holesy\20_TESTS\Candidate_Builds\Master 15.1 - wave1-speed-burst-tuning.html`

Baseline:

- `C:\Users\KentLefner\Desktop\game-repo\Holesy\10_SOURCE\Masters\Master 15.html`

Reason for candidate:

- after validating the `Master 15` promotion line, the next concrete Priority 2 concern still open is that a single Speed Burst aid in Wave 1 may make the player too dominant too quickly

Implementation review:

- build marker advanced to `Master 15.1`
- Speed Burst is now tuned more conservatively in Waves mode:
  - Wave 1 applies `x1.65` speed for `6` seconds
  - later Waves apply `x1.85` speed for `8` seconds
- non-Waves modes still keep the original global Speed Burst values
- player-facing Speed Burst text now reflects the actual applied Waves tuning instead of the original global `x2` copy

Risk controls:

- change is limited to aid-effect application logic
- approved `Master 15` startup, transition, audio-cleanup, and rival-score-pressure behavior is otherwise preserved
- no new AI, menu, or round-lifecycle code was introduced in this slice

Open validation required:

- confirm mode select and `Begin` still work
- confirm Wave 1 Speed Burst feels meaningfully less overwhelming
- confirm the new Speed Burst text matches the actual effect
- confirm later-Waves Speed Burst still feels worthwhile

Status:

- candidate ready for user browser validation
- not approved for promotion
