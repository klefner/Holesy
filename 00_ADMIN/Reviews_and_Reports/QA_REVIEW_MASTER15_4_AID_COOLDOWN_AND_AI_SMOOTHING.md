# QA Review — Master 15.4 Aid Cooldown and AI Smoothing

Date:

- 2026-04-30

Candidate under review:

- `C:\Users\KentLefner\Desktop\game-repo\Holesy\20_TESTS\Candidate_Builds\Master 15.4 - aid-cooldown-and-ai-smoothing.html`

Baseline:

- `C:\Users\KentLefner\Desktop\game-repo\Holesy\20_TESTS\Candidate_Builds\Master 15.3 - score-compression-pass.html`

Reason for candidate:

- user reported a defect where Gulp collected both alien drops and appeared to stutter in place after the second pickup
- user also reported that `15.3` score compression improved, but the rival behavior felt initially somewhat unnatural

Implementation review:

- build marker advanced to `Master 15.4`
- AI holes now receive a short post-aid retarget cooldown so the same rival is less likely to monopolize consecutive aid drops immediately
- when an AI claims aid, it now clears the consumed target cleanly and gets a short smoothing wander target plus near-immediate re-decision instead of lingering on the just-claimed object state
- Gulp and the other rivals were softened slightly from the `15.3` peak tuning while preserving the stronger competition profile
- non-flee AI movement speed was trimmed slightly from `15.3`

Risk controls:

- startup / menu flow untouched
- Waves transition, audio cleanup, and approved local master basis remain untouched
- score-pressure improvements from `15.3` remain broadly in place, but the most robotic edge was reduced

Open validation required:

1. Confirm AI aid claimers no longer stutter after picking up alien aid.
2. Confirm aid does not keep getting monopolized by the same rival as often.
3. Confirm score compression remains materially better than the older `15.2` line.
4. Confirm rival behavior feels less unnatural than `15.3`.

Status:

- candidate ready for user browser validation
- not approved for promotion
