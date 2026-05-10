# QA Review — Master 15.6 Transition Camera and Soldier Escape

Date:

- 2026-04-30

Candidate under review:

- `C:\Users\KentLefner\Desktop\game-repo\Holesy\20_TESTS\Candidate_Builds\Master 15.6 - transition-camera-and-soldier-escape.html`

Baseline:

- `C:\Users\KentLefner\Desktop\game-repo\Holesy\20_TESTS\Candidate_Builds\Master 15.5 - transition-countdown-and-proof-of-life.html`

Reason for candidate:

- user said the new Waves transition pacing felt right, but a previously liked pulled-up camera view during transition had been lost
- user also reported a defect where Maw lingered under soldier pressure instead of either escaping or converting the nearby soldiers into score

Implementation review:

- build marker advanced to `Master 15.6`
- camera motion is now updated during transition states as well as active gameplay
- Waves transition now pulls the camera up into a wider district view again instead of leaving the game in a flat frozen-feeling play-camera hold
- soldier-threat escape now uses a stronger multi-soldier repulsion vector plus light edge bias, rather than only drifting away from a simple centroid
- AI under soldier pressure now gets a faster follow-up re-decision after escape targeting is chosen

Design note captured for future work:

- user wants future wave-start hole spawn positions randomized, with a control that prevents holes from spawning too close to each other at the start of a wave
- that change is not included in this slice

Open validation required:

1. Confirm the Waves transition again has the wider pulled-up camera feel you liked.
2. Confirm the transition still feels intentional and readable after restoring the camera motion.
3. Confirm rivals trapped near soldiers now escape more intelligently instead of wobbling in place under fire.
4. Confirm no new regressions appear in pause / exit during transition.

Status:

- candidate ready for user browser validation
- not approved for promotion
