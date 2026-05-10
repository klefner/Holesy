# Master 14.5 Reapply Plan From Stable Baseline

Purpose:

- preserve the good design intent explored during the failed `14.5` line so it can be rebuilt from a known-working startup path

Do not use as a source baseline:

- `Master 14.5 - ai-rivals-aid-search-and-score-pressure.html` is not a safe codebase baseline because the startup path became unreliable

Safe baseline to rebuild from:

- `C:\Users\KentLefner\Desktop\game-repo\Holesy\20_TESTS\Candidate_Builds\Master 14.6 - rollback-to-14.4-stable-baseline.html`

Reapply items worth keeping:

1. Rival aid-drop contest behavior
- rivals should intentionally investigate alien aid
- they should not have perfect omniscience
- preferred approach:
  - exact pursuit only when the aid object is directly detectable
  - otherwise search a noisy estimated drop zone

2. Rival score-pressure improvements
- the player still scores several multiples above rivals in normal Waves play
- `Gulp` especially should route toward stronger reachable value over time
- preserve this as tunable difficulty-linked logic, not one hardcoded personality spike

3. Rival flee improvement
- current flee behavior is too readable and can be steered by player positioning
- desired replacement:
  - breakout-route selection
  - less cardinally deterministic escape headings

4. Scoreboard / mode-select non-music audio cleanup
- all non-music sounds should die when the round ends and the user is on scoreboard or mode-select screens
- music should continue

5. Waves transition readability
- the transition pause should feel intentional, not hung
- preserve the fiction cue that the current district is spent and the next battlefield is being rebuilt

6. Buff-timer behavior during transitions
- effect timers should decrement only while the player is actually on the playfield
- transitions between waves should not consume buff time

7. Waves end-state wording
- avoid contradictory-feeling copy where the title implies a surviving breach while the subtitle says the player lost
- losing copy should explicitly read as a loss for the player

Process guardrails for the rebuild:

- reapply one slice at a time from `14.6`
- browser-validate after each slice
- avoid mixing startup-path changes with gameplay-tuning changes unless strictly necessary
- treat menu and startup responsiveness as a control gate before any gameplay QA
