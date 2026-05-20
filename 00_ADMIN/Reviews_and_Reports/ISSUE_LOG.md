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
| QA-004 | Mixed dirty tree creates active commit-scoping risk | Staging hygiene / Process compliance | resolved | 2026-05-10 | 2026-05-18 | Daily audit on 2026-05-18 again found `99_TEMP` absent. The repo now contains only governance-scope local edits rather than the prior mixed temp/gameplay contamination pattern that originally triggered this issue. | Codex / Project Manager | yes | yes | Re-verified on 2026-05-18 with current repo status and local temp-folder existence check; contamination risk is no longer the original temp-file issue, but future commits should still keep governance edits intentionally scoped. |
| QA-005 | Gameplay audio continues outside active play | Audio state / Gameplay QA | resolved | 2026-05-11 | 2026-05-11 | User reported failed audible validation for `Master 15.10` and `Master 15.11`; `Master 15.12` proved the countdown can now be heard; `Master 15.13` established the right race-light beep sound but failed timer synchronization; `Master 15.14` synchronized the cue to the HUD timer. `Master 15.15 - new-wave-text-and-audio-stop.html` changes visible proof to `NEW WAVE IN X` and stops active non-music sample sources when entering menu/game-over states. User reported all tests passed for `15.15`. | Codex / Project Manager | yes | yes | Completed: user validation passed for countdown timing, race-light tone/volume, `NEW WAVE IN 3/2/1` proof, and non-music audio cleanup on game-over/mode-select. |
| QA-006 | Long-idle menu tab close stalls browser UI | Performance / Lifecycle cleanup | monitor | 2026-05-13 | 2026-05-20 | User observed that a Chrome tab left on the game menu for hours took roughly 20 seconds to close and blocked other browser UI during that delay. `Master 15.39 - idle-lifecycle-cleanup.html` and the promoted `Master 16.html` still contain the intended cleanup and throttling changes, and the 2026-05-20 daily audit found no contradictory file-level evidence. A real long-idle Chrome rerun is still not available. | Codex / Technical Lead | yes | yes | Monitor until user performs a real long-idle Chrome validation against the promoted `Master 16` package or equivalent build. File-level evidence alone is still not enough to call this resolved. |
| QA-007 | Lore buffs unclear and document drops too frequent | Gameplay UX / Lore progression | monitor | 2026-05-13 | 2026-05-20 | User reported that buff messages such as town names did not make the effect obvious, the messages disappeared too quickly, buff icons did not explain whether the player gained speed, defense, score, slowdown, or another effect, and 8 documents dropped in one round. `Master 15.42 - buff-clarity-and-rare-docs.html` changes remain included in `Master 16.html`, and the 2026-05-20 daily audit found no contradictory governance evidence. Real gameplay validation is still pending. | Codex / Product Manager | yes | yes | Monitor until user validates in real gameplay that buff effects are obvious, combo language is understandable, and document progression feels rare enough in the promoted `Master 16` experience. |
| QA-008 | Active handoff was stale against the promoted basis and current recommendation | Documentation governance / Source-of-truth clarity | resolved | 2026-05-13 | 2026-05-20 | Remediation on 2026-05-20 included the `Master 16` handoff package, including `NEXT_CODEX_CHAT_HANDOFF_MASTER16.md` and the historical `NEXT_CODEX_CHAT_HANDOFF_MASTER15.md` update, so the active handoff is no longer local-only. | Codex / Project Manager | yes | yes | Resolved by staging, committing, and pushing the handoff package, with tracked-state verification included in the 2026-05-20 closure review. |
| QA-009 | Daily audit evidence can be lost between automation memory and governed repo artifacts | Evidence retention / Documentation governance | resolved | 2026-05-18 | 2026-05-20 | Remediation on 2026-05-20 included the available dated daily audit reports for 2026-05-13, 2026-05-18, and 2026-05-19, and added `QA_REVIEW_DAILY_AUDIT_2026-05-12_MISSING_ARTIFACT_NOTE.md` to explicitly document the unrecoverable May 12 artifact without inventing missing evidence. | Codex / Project Manager | yes | yes | Resolved by staging, committing, and pushing the available audit reports plus the missing-artifact note, with future daily audits still required to verify each dated report is tracked before completion. |
| QA-010 | Backlog current recommendation is stale against the active Master 16 basis | Documentation governance / Source-of-truth clarity | resolved | 2026-05-20 | 2026-05-20 | `00_ADMIN/Requirements/PRODUCT_BACKLOG.md` was updated on 2026-05-20 so the Current Recommendation now treats `10_SOURCE/Masters/Master 16.html` with in-game label `Master 16.15` as the current governed production-test baseline and no longer routes future work back to `Master 16.5`. | Codex / Project Manager | yes | yes | Resolved by updating the backlog recommendation in the same governance recovery package; future audits should continue comparing backlog, basis, and active handoff for drift. |
| QA-011 | Product intent and modular architecture continuity broke during release packaging guidance | Product governance / Architecture continuity | monitor | 2026-05-20 | 2026-05-20 | User identified that the assistant described the current GoDaddy package as only requiring `index.html` without preserving the accepted modular client architecture target. Recovery added `RELEASE_SOURCE_OF_TRUTH_MANIFEST.md`, `PRODUCT_INTENT_GATE.md`, updated the publish workflow, enhanced QA controls, captured `PERF-012`, and upgraded `holesy_team_sync.ps1` so new chats check package hashes, build labels, automation drift, governance corpus, issue counts, backlog readiness, and request-specific Product Intent Gate signals. | Codex / Product Manager | yes | yes | Monitor until the next packaging or implementation turn proves the Product Intent Gate is actually used before acting; this is a behavior-control issue, so documentation and script updates are necessary but not sufficient for closure. |
| QA-012 | Daily auditor automation did not explicitly require full process/procedure governance review | QA automation / Process governance | monitor | 2026-05-20 | 2026-05-20 | User identified that the auditor automation must study all current process/procedure documents every time, not only audit artifacts. The local `daily-qa-audit` automation prompt was updated, `AUDITOR_AUTOMATION_PROMPT.md` records the required prompt intent, the unified audit workplan plus QA review standard require process/procedure corpus review, and Team Sync v2 now checks the actual automation TOML for required governed prompt markers. | Codex / QA Lead | yes | yes | Monitor until the next scheduled audit proves the updated automation actually rereads the governance corpus, updates procedures when needed, writes a dated report, and updates this issue log. |

## QA Review Checklist For This Log

During each material QA review:

1. Identify any relevant existing issues.
2. Verify each issue marked `resolved` is truly resolved.
3. Verify each unresolved material issue is escalated to the Project Manager persona and the user.
4. Add new issues found by the audit, or state clearly that issue logging is required before closure.
