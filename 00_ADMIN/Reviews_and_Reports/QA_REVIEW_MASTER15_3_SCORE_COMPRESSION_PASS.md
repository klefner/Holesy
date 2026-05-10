# QA Review — Master 15.3 Score Compression Pass

Date:

- 2026-04-30

Candidate under review:

- `C:\Users\KentLefner\Desktop\game-repo\Holesy\20_TESTS\Candidate_Builds\Master 15.3 - score-compression-pass.html`

Baseline:

- `C:\Users\KentLefner\Desktop\game-repo\Holesy\20_TESTS\Candidate_Builds\Master 15.2 - aid-intel-and-rival-pressure.html`

Reason for candidate:

- `15.2` improved rival aid contesting, but user still observed a large leaderboard gap in Waves, with the player around `40k` and second place around `10k`

Implementation review:

- build marker advanced to `Master 15.3`
- AI personalities now re-decide more often, wander less, and scan more of the city
- high-value object scoring was increased further so rivals should convert more map value into score instead of leaving too much on the table
- rival prey-hunt thresholds and hunt likelihood were raised so they should pressure each other and capitalize on scoring opportunities more often
- non-flee rival movement speed increased modestly again to help target conversion without changing the player speed

Risk controls:

- startup / mode-select / `Begin` path untouched
- aid-intel logic from `15.2` preserved
- transition, audio, and round-lifecycle behavior untouched

Open validation required:

1. Confirm rival aid contesting from `15.2` still feels present.
2. Confirm final score gaps compress more than the `40k` versus `10k` result seen on `15.2`.
3. Confirm rivals still feel fair rather than unnaturally perfect.

Status:

- candidate ready for user browser validation
- not approved for promotion
