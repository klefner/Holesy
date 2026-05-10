# QA Review — Master 15.2 Aid Intel and Rival Pressure

Date:

- 2026-04-30

Candidate under review:

- `C:\Users\KentLefner\Desktop\game-repo\Holesy\20_TESTS\Candidate_Builds\Master 15.2 - aid-intel-and-rival-pressure.html`

Baseline:

- `C:\Users\KentLefner\Desktop\game-repo\Holesy\20_TESTS\Candidate_Builds\Master 15.1 - wave1-speed-burst-tuning.html`

Reason for candidate:

- user reported that grounded paralax aid could still sit uncontested while the player was the only hole pursuing it
- user also reported that end-of-run scores were still separating too widely despite the earlier rival-pressure pass

Implementation review:

- build marker advanced to `Master 15.2`
- AI personalities were tuned upward for:
  - lower wander bias
  - larger object scan budgets
  - stronger aid interest
  - larger aid detection ranges
- shared short-lived aid intel was added:
  - aid ships now publish a rough drop-zone hint
  - dropping and landed aid refresh that intel
  - rivals can investigate the remembered zone even when they are not already close enough for direct detection
- aid investigation still uses noisy search targets rather than exact perfect coordinates
- general score pressure was also raised by:
  - stronger powerup weighting
  - slightly better high-value object weighting
  - a modest non-flee AI movement-speed increase

Risk controls:

- startup / mode-select / `Begin` path was not touched
- approved `Master 15` transition, audio, and round-lifecycle behavior remains intact
- this slice stays inside AI targeting and pressure tuning rather than reopening broader systems

Open validation required:

- confirm grounded aid is now contested by at least one rival more reliably
- confirm rivals still do not feel omniscient
- confirm end-of-run scores compress more meaningfully
- confirm the game does not become unfair or feel like all rivals beeline every aid instantly

Status:

- candidate ready for user browser validation
- not approved for promotion
