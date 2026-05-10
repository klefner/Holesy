# QA Review — Master 14.10 Rival Aid and Score Pressure

Date:

- 2026-04-30

Candidate under review:

- `C:\Users\KentLefner\Desktop\game-repo\Holesy\20_TESTS\Candidate_Builds\Master 14.10 - rival-aid-and-score-pressure.html`

Baseline:

- `C:\Users\KentLefner\Desktop\game-repo\Holesy\20_TESTS\Candidate_Builds\Master 14.9 - powerup-text-clarity.html`

Reason for candidate:

- user validated the rebuilt stability / UI slices through `14.9`
- the next Priority 2 gameplay gap is still weak rival competition, especially around alien aid and end-of-run score compression
- the failed `14.5` line already identified the right improvement themes, but those themes must now be reapplied from the stable rebuild path one narrow slice at a time

Implementation review:

- build marker advanced to `Master 14.10`
- AI personalities now carry narrow new tuning fields for:
  - object scan budget
  - aid interest
  - aid detection range
- rivals can now intentionally investigate active alien aid without using exact omniscient drop coordinates:
  - if a landed aid pickup is close enough, they can target it directly
  - otherwise they can investigate a noisy projected search point based on the visible aid ship path
- object targeting now uses a stronger per-AI scan budget instead of the older coarse shared sample stride
- powerups receive materially higher desirability in the AI object score so rivals should contest them more often
- this slice intentionally does not reintroduce the broader `14.5` flee-behavior overhaul

Risk controls:

- startup / mode-select / `Begin` path intentionally left untouched because that was the blocker regression source in `14.5`
- validated `14.7` transition logic, `14.8` non-music menu audio cleanup, and `14.9` text clarity are preserved as-is
- failed `14.5` remains design-intent reference only and is not reused as a technical base

QA assessment:

- medium gameplay-risk slice, but narrower than the abandoned `14.5` bundle
- no browser playtest was executed in this QA pass
- no promotion recommendation until user validation confirms both behavior improvement and startup safety
- post-validation defect patched in-place on 2026-04-30:
  - exiting to game modes during a Waves transition could leave the transition stage-pop visible on the menu / scoreboard screen
  - candidate now explicitly clears transient round UI during round reset and game-over teardown instead of relying on timeout-only cleanup

Open validation required:

- confirm mode select and `Begin` still work
- confirm at least one rival now visibly contests alien aid with intent
- confirm rivals do not feel omniscient when searching for aid
- confirm end-of-run scores compress meaningfully relative to prior Waves tests
- confirm Wave 1 does not become unfair while aid contest pressure increases
- confirm no Waves transition text remains visible after pausing and exiting to game modes mid-transition

Status:

- candidate ready for user browser validation
- not approved for promotion
