# QA Review — Master 15.8 Aid Stutter, Camera, and Mid AI Buff

Date:

- 2026-04-30

Candidate under review:

- `C:\Users\KentLefner\Desktop\game-repo\Holesy\20_TESTS\Candidate_Builds\Master 15.8 - aid-stutter-camera-and-mid-ai-buff.html`

Baseline:

- `C:\Users\KentLefner\Desktop\game-repo\Holesy\20_TESTS\Candidate_Builds\Master 15.7 - predrop-aid-intel-fix.html`

Reason for candidate:

- user reported that after narrowly losing an aid race, Maw could stand near the pickup spot stuttering for several seconds
- user also reported that the restored transition camera had gone too high, showing nearly the full map instead of the lower cinematic view they wanted
- user wants Void and Maw pushed up so the non-Gulp rivals still feel like they are trying to win, not just serving as passive fodder

Implementation review:

- build marker advanced to `Master 15.8`
- when a powerup is claimed, stale aid intel is cleared immediately
- nearby non-claiming AI holes now drop the dead objective, get a short anti-retarget cooldown, and re-enter motion quickly instead of lingering on the consumed pickup
- transition camera height and distance were reduced from the `15.6/15.7` high overview to a lower district-scale cinematic view
- Void and Maw were both strengthened moderately through:
  - tighter decision intervals
  - better scan budgets
  - slightly stronger greed / aggression
  - lower wander bias

Risk controls:

- Gulp was not pushed harder in this slice
- exact pre-drop aid prediction fix from `15.7` remains intact
- startup, transition timing, and hole-wind audio were not touched here

Open validation required:

1. Confirm a rival that loses an aid race no longer stands there stuttering for several seconds.
2. Confirm the transition camera is lower and closer to the old liked feel.
3. Confirm Void and Maw feel a bit more competitive without becoming obviously scripted or unfair.
4. Confirm the pre-drop aid camping defect stays fixed.

Status:

- candidate ready for user browser validation
- not approved for promotion
