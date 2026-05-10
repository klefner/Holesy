# QA Review — Master 14.5 AI Rivals Aid Search And Score Pressure

Date:

- 2026-04-30

Candidate under review:

- `C:\Users\KentLefner\Desktop\game-repo\Holesy\20_TESTS\Candidate_Builds\Master 14.5 - ai-rivals-aid-search-and-score-pressure.html`

Baseline:

- `C:\Users\KentLefner\Desktop\game-repo\Holesy\20_TESTS\Candidate_Builds\Master 14.4 - waves-narrative-and-pacing-pass.html`

Reason for candidate:

- user desktop Waves playtest on `Master 14.4` reported that one alien speed aid could still make Wave 1 too easy
- rival holes were not intentionally contesting alien aid
- leaderboard spread remained extreme, with the player finishing far ahead of all rivals
- rival flee behavior was predictable enough that the player could steer holes into bad escape lines
- plane audio could still be heard on the scoreboard / game-mode screen after the round ended

Scope of this pass:

- strengthen rival aid-drop contest behavior without giving rivals perfect omniscience
- strengthen rival score pressure through better object-routing heuristics
- reduce steerable flee-path behavior
- hard-stop non-music audio on scoreboard and mode-select states

Implementation review:

- candidate updates AI personalities so `Gulp` evaluates more aggressively and all rivals can choose aid-search behavior with different commitment levels
- candidate adds noisy aid-search targeting so rivals investigate likely aid zones rather than exact coordinates unless the pickup is directly seen
- candidate replaces the prior coarse object-evaluation path with a stronger reach/value heuristic that better favors meaningful intake
- candidate replaces straight-opposite flee logic with breakout-route selection
- candidate adds explicit non-music menu-state cleanup that:
  - stops the alien-aid loop
  - clears aid ships
  - stops and removes active planes
  - mutes non-music gain buses while title / mode-select / game-over states are active

QA assessment:

- design intent appears aligned with the reported defects
- rollback safety is preserved because the work is isolated to a new candidate build
- no browser playtest was executed in this QA pass

Open validation required:

- confirm rival holes now visibly contest alien aid in live Waves play
- confirm the stronger rival routing compresses the leaderboard enough to improve score-race pressure
- confirm flee behavior is less player-steerable in repeated close-chase scenarios
- confirm all non-music sounds are silent on scoreboard and mode-select screens while music continues
- confirm no regression to in-round audio behavior after non-music buses are restored when gameplay resumes

Additional findings from user validation:

- candidate line developed a blocker startup regression:
  - mode selection became non-responsive
  - `Begin` animated but did not start a run
- repeated repair attempts inside the `14.5` line did not restore reliable startup behavior
- later transition-focused changes therefore cannot be trusted as a stable baseline even where their design intent was sound

Status:

- candidate failed validation due to blocker startup regression
- not approved for promotion
- preserve only as a design-intent / reimplementation reference, not as a base for further gameplay work
