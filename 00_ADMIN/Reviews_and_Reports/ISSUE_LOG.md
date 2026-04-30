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
| QA-001 | Unified audit-plan merge not yet on GitHub branch | GitHub completeness | resolved | 2026-04-29 | 2026-04-29 | Remediation commit `3fbeb53` was pushed to branch `codex/publish-master4-structure`; remote verification confirmed the unified workplan is the active reference, the issue log is present on the branch, and the superseded GitHub/release workplan file is removed from the branch. | Codex / Project Manager | yes | yes | Completed: branch push succeeded and remote references were checked against the unified file and issue log. |
| QA-002 | Unrelated untracked files create pre-commit contamination risk | Staging hygiene | resolved | 2026-04-29 | 2026-04-29 | `20_TESTS/Candidate_Builds/index.html` was identified as a stale local `Master 6` artifact in the wrong folder and removed. `20_TESTS/Exploratory_Builds/Stack-collapse exploration - cannon-es prototype.html` was identified as a real exploratory project artifact in the correct governed location and is being committed intentionally. This removes the recurring accidental-untracked noise from the two known files that were contaminating status reviews. | Codex / Project Manager | yes | yes | Verify branch push includes the exploratory prototype, confirm `Candidate_Builds/index.html` remains absent, and confirm `git status --short --branch` no longer shows either file as accidental untracked noise. |

## QA Review Checklist For This Log

During each material QA review:

1. Identify any relevant existing issues.
2. Verify each issue marked `resolved` is truly resolved.
3. Verify each unresolved material issue is escalated to the Project Manager persona and the user.
4. Add new issues found by the audit, or state clearly that issue logging is required before closure.
