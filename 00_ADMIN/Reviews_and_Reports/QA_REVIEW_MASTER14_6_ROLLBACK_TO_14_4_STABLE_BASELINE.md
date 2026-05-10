# QA Review — Master 14.6 Rollback To 14.4 Stable Baseline

Date:

- 2026-04-30

Candidate under review:

- `C:\Users\KentLefner\Desktop\game-repo\Holesy\20_TESTS\Candidate_Builds\Master 14.6 - rollback-to-14.4-stable-baseline.html`

Baseline:

- `C:\Users\KentLefner\Desktop\game-repo\Holesy\20_TESTS\Candidate_Builds\Master 14.4 - waves-narrative-and-pacing-pass.html`

Reason for candidate:

- the `14.5` candidate line introduced a blocker startup regression that made mode selection and `Begin` non-functional
- repeated in-line repair attempts did not restore reliable startup behavior
- the safest recovery action was to restore the last known working Waves candidate baseline and resume from there

Implementation review:

- `Master 14.6` is a rollback-safe copy of the previously working `Master 14.4` candidate
- only the candidate identity / build labeling was advanced so testing can continue on a fresh governed file without pretending the broken `14.5` line is still active
- no `14.5` gameplay or audio changes were retained in this rollback candidate

QA assessment:

- rollback safety preserved
- startup path expected to match the previously working `14.4` behavior
- this candidate is intentionally conservative and exists to restore testability first

Deferred reimplementation items from the abandoned `14.5` line:

- rival aid-drop investigation behavior
- stronger rival score-pressure heuristics
- less steerable flee-path logic
- scoreboard / mode-select non-music audio cleanup
- Waves transition readability improvements
- effect-timer freeze during transition
- Waves end-state wording cleanup

Status:

- candidate ready for user browser validation as the new stable working branch-off point
- not promoted
