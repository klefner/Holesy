# Holesy Issue Log

Use this shared issue log during QA reviews to track material issues, confirm true resolution, and escalate unresolved items.

## Status Definitions

- `open`: issue exists and needs action
- `monitor`: change was made, but more evidence is needed before calling it resolved
- `resolved`: evidence supports that the issue is fixed
- `closed`: resolved and no further monitoring is needed
- `deferred`: acknowledged but intentionally postponed

## Escalation Rule

Any material unresolved issue reviewed during QA must be explicitly escalated to:

- the Project Manager persona
- the user

If that escalation has not happened, the QA review should record it as a control failure.

## Remediation Update Rule

When a previously logged issue is believed to be remediated:

1. update `Status`
2. update `Last reviewed`
3. replace stale evidence with the actual remediation evidence
4. state the exact verification needed or completed in `Resolution check`
5. if the risk still exists in general but the control worked on the latest pass, use `monitor` instead of `resolved`

Do not leave an issue at `open` after a real remediation attempt without updating the evidence field to explain what changed and what remains.

## Issue Register

| Issue ID | Title | Area | Status | First noted | Last reviewed | Evidence | Owner | Escalated to PM | Escalated to user | Resolution check |
| --- | --- | --- | --- | --- | --- | --- | --- | --- | --- | --- |
| QA-001 | Unified audit-plan merge not yet on GitHub branch | GitHub completeness | resolved | 2026-04-29 | 2026-05-10 | Remediation commit `3fbeb53` remains present in local tracked branch history, and the unified audit workplan plus shared issue log remain in the governed repo path on branch `codex/publish-master4-structure`. Daily audit on 2026-05-10 found no contradictory repo evidence. | Codex / Project Manager | yes | yes | Re-verified on 2026-05-10 against local branch history and governed file paths; no regression found. |
| QA-002 | Unrelated untracked files create pre-commit contamination risk | Staging hygiene | resolved | 2026-04-29 | 2026-05-10 | `20_TESTS/Candidate_Builds/index.html` remains absent locally, and `20_TESTS/Exploratory_Builds/Stack-collapse exploration - cannon-es prototype.html` remains the intentionally governed exploratory artifact added by commit `39bbd94`. The original accidental-noise pattern from 2026-04-29 did not recur in the 2026-05-10 `git status --short --branch` output. | Codex / Project Manager | yes | yes | Re-verified on 2026-05-10 by file existence checks plus `git status --short --branch`; original contamination pattern remains resolved even though a separate mixed-scope dirty-tree issue is now open under `QA-004`. |
| QA-003 | Approved Master 15 lineage is local-only and not backed up on the working branch | GitHub completeness / Documentation governance | resolved | 2026-04-30 | 2026-05-10 | Branch `codex/publish-master4-structure` was pushed from `39bbd94` to `94d97cf` with split commits for the `Master 15` promotion, the `Master 14.10` through `Master 15.8` candidate lineage, and matching QA / handoff / backlog governance docs. Verification confirmed `10_SOURCE/Masters/Master 15.html` and `20_TESTS/Candidate_Builds/Master 15.8 - aid-stutter-camera-and-mid-ai-buff.html` are tracked at branch head, and no `99_TEMP` path is present in the committed tree. | Codex / Project Manager | yes | yes | Completed: remote branch history now contains the approved `Master 15` source, candidate lineage, and governance package; temp scratch was excluded. |
| QA-004 | Mixed dirty tree creates active commit-scoping risk | Staging hygiene / Process compliance | open | 2026-05-10 | 2026-05-10 | `git status --short --branch` shows modified governance docs, many untracked candidate and QA files, and `?? 99_TEMP/`; local inspection confirms `99_TEMP/master14_5_script.js` exists alongside the governed artifacts. That mix makes it easy for the next commit to sweep together temporary files, governance updates, and gameplay lineage unless scope is controlled deliberately. | Codex / Project Manager | yes | yes | Before the next commit, exclude `99_TEMP/master14_5_script.js` and split governed artifacts into narrow batches. Re-verify with `git status --short --branch` and the staged file list that no temp or unrelated files are included. |
| QA-005 | Gameplay audio continues outside active play | Audio state / Gameplay QA | resolved | 2026-05-11 | 2026-05-11 | User reported failed audible validation for `Master 15.10` and `Master 15.11`; `Master 15.12` proved the countdown can now be heard; `Master 15.13` established the right race-light beep sound but failed timer synchronization; `Master 15.14` synchronized the cue to the HUD timer. `Master 15.15 - new-wave-text-and-audio-stop.html` changes visible proof to `NEW WAVE IN X` and stops active non-music sample sources when entering menu/game-over states. User reported all tests passed for `15.15`. | Codex / Project Manager | yes | yes | Completed: user validation passed for countdown timing, race-light tone/volume, `NEW WAVE IN 3/2/1` proof, and non-music audio cleanup on game-over/mode-select. |
| QA-006 | Long-idle menu tab close stalls browser UI | Performance / Lifecycle cleanup | monitor | 2026-05-13 | 2026-05-13 | User observed that a Chrome tab left on the game menu for hours took roughly 20 seconds to close and blocked other browser UI during that delay. `Master 15.39 - idle-lifecycle-cleanup.html` adds tracked animation scheduling, throttles static menu/game-over rendering to 1 frame per second, clears idle frame timers, stops WebAudio scheduler / wind / non-music sources, releases the stats-window opener reference, and disposes the WebGL renderer on page exit. In-app browser smoke evidence: candidate loaded with no console errors and close returned in ~197 ms. | Codex / Technical Lead | yes | yes | Monitor until user performs a real long-idle Chrome validation. Current automated evidence supports remediation but cannot compress the original multi-hour idle condition. |
| QA-007 | Lore buffs unclear and document drops too frequent | Gameplay UX / Lore progression | monitor | 2026-05-13 | 2026-05-13 | User reported that buff messages such as town names did not make the effect obvious, the messages disappeared too quickly, buff icons did not explain whether the player gained speed, defense, score, slowdown, or another effect, and 8 documents dropped in one round. `Master 15.42 - buff-clarity-and-rare-docs.html` adds player-readable buff names, explicit effect text, longer event banners, active-effect tray descriptions, beneficial / risky combo labels, and rare end-of-round document drops capped at one per round. | Codex / Product Manager | yes | yes | Monitor until user validates in real gameplay that buff effects are obvious and document progression feels rare enough. |

## QA Review Checklist For This Log

During each material QA review:

1. Identify any relevant existing issues.
2. Verify each issue marked `resolved` is truly resolved.
3. Verify each unresolved material issue is escalated to the Project Manager persona and the user.
4. Add new issues found by the audit, or state clearly that issue logging is required before closure.
