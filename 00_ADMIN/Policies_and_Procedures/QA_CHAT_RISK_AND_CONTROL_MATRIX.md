## Holesy QA Chat Risk And Control Matrix

This matrix is the required QA basis for evaluating project chats when the user triggers a quality inspection report.

Use it together with:

- `00_ADMIN/Policies_and_Procedures/QA_REVIEW_STANDARD.md`
- `00_ADMIN/Reviews_and_Reports/AUDIT_WORKPLAN_UNIFIED_QA_AND_RELEASE_CONTROLS.md`
- `00_ADMIN/Policies_and_Procedures/RISK_AND_CONTROLS_POLICY.md`

## Purpose

- Provide a repeatable control framework for reviewing chat quality rather than relying on intuition.
- Detect errors that can mislead the user about repo state, release state, risk, evidence, or readiness.
- Create a durable list of known failure modes that can be enhanced over time.

## Rating Guidance

- Inherent risk:
  - Low: wording, formatting, or minor clarity issue with limited downstream effect
  - Moderate: issue could misdirect next steps, create rework, or hide an unverified assumption
  - High: issue could cause regression, incorrect repo action, wrong release decision, or loss of control
  - Critical: issue could cause destructive action, false release confidence, or unrecoverable project confusion
- Control status:
  - Effective
  - Partially effective
  - Ineffective

## Control Matrix

| Risk ID | Risk area | Risk description | Typical failure pattern in chats | Required control | QA test questions | Expected evidence |
| --- | --- | --- | --- | --- | --- | --- |
| C01 | Scope control | The chat solves a different problem than the one requested | Assistant reframes task without saying so, or mixes repair with unrelated feature work | Restate requested objective and compare delivered scope to it | Did the output directly answer the user ask? Did scope expand without approval? | User request, assistant completion, changed files |
| C02 | Source-of-truth clarity | Chat confuses local files, branch state, `main`, GitHub, or live website state | Claims work is "done" without specifying where it exists | State reporting must separate local, branch, `main`, and live states when relevant | Were these states distinguished clearly? Was any status implied but not evidenced? | Chat summary, `git status`, branch references, publish notes |
| C03 | Evidence quality | Material claims are not supported by inspected files, commands, or tests | Assistant states conclusions from memory or assumption | High-impact claims require evidence from repo inspection, command output, or stated limitation | What evidence supported each key claim? Is any conclusion unsupported? | File contents, command outputs, cited paths, test results |
| C04 | Risk disclosure | The chat hides, understates, or fails to name meaningful risk | Assistant presents uncertain work as safe or complete | QA report must name open risks, unverified areas, and residual uncertainty | Were open risks stated plainly? Were confidence limits clear? | Completion text, QA findings section |
| C05 | Rollback safety | Recommended action lacks rollback path or backup thinking | Replacing baseline or master without preserving recovery path | Changes affecting source, masters, release, or publish flow must identify rollback path | If the change failed, could the user recover quickly? Was that path documented? | Baseline reference, branch, master history, backup artifact |
| C06 | Testing sufficiency | Chat implies readiness without enough verification for the risk level | "Fixed" is declared after inspection only, or after narrow testing | Validation should match risk class and affected systems | Was testing proportionate to the change? Were unrun tests disclosed? | Test commands, manual test list, stated gaps |
| C07 | Instruction quality | Guidance is hard to follow, ambiguous, or unsafe for the user | Vague next steps, buried warnings, or misleading wording | Instructions must be concrete, sequenced, and beginner-safe | Could a beginner follow this safely? Are key distinctions easy to miss? | Handoff docs, completion message, workflow steps |
| C08 | Change containment | Chat introduces unnecessary architectural or file-scope churn | Large edits for a narrow issue, or mixing cleanup with defect repair | Keep fixes local unless broader change is justified | Did the change stay near the problem? Was extra churn necessary? | Diff scope, touched files, rationale |
| C09 | Artifact governance | Docs, candidates, masters, and reports are updated inconsistently | New master or policy exists, but basis/handoff/report trail is stale | Governance artifacts must be updated when control-significant events occur | Were all required companion docs updated? Is the audit trail coherent? | `CURRENT_BASIS.md`, backlog, handoff, report files |
| C10 | Traceability | User feedback cannot be tied to exact build, file, or decision | Review references "latest version" without naming it | Name exact files, build labels, branches, and decision points | Can a third party reconstruct what was reviewed and why? | File paths, build labels, branch names, dates |
| C11 | Staging and commit hygiene | Chat risks committing wrong or extraneous files | Untracked exploratory files left ambiguous or staged accidentally | Pre-commit review must compare staged set to intended scope | Did the chat inspect staging hygiene where commit activity mattered? | `git status --short`, staged file list |
| C12 | Process compliance | Chat bypasses governing project policies or repo conventions | Works directly on wrong artifact type, skips required review layer | QA must compare work against project policy and workflow docs | Did the work comply with the stated policy set? If not, was the exception justified? | Governing docs, chat actions, completion notes |
| C13 | Communication honesty | Limitations are hidden or softened into false confidence | Assistant omits failed tests, unknowns, or blocked actions | Explicitly disclose what was not done, not known, or not accessible | Are any limitations missing from the final message? | Tool results, completion wording |
| C14 | Handoff readiness | Future chat would lack enough context to continue safely | No basis, no next-step framing, no open-issue capture | Material sessions should leave durable context when appropriate | Could a new chat resume with low confusion? | Handoff note, basis file, QA report |
| C15 | Continuous improvement | Repeated failure modes are noticed but not folded into controls | Same mistakes recur across reviews without matrix updates | Each audit should consider whether the matrix or workplan needs enhancement | Did this review add or refine any control for future use? | Matrix revision note, workplan update note |
| C16 | Control design effectiveness | A stated control exists on paper but is too weak, vague, or misaligned to actually prevent or detect the risk | Checklist item exists but would not catch the failure in practice | QA must assess whether the control design is specific, timely, testable, and matched to the risk | Would this control realistically prevent or detect the issue before damage occurs? Is the owner, trigger, and evidence clear? | Control wording, testability, timing, evidence requirement |
| C17 | Issue management | Known risks become actual issues but are not logged, retested, or escalated properly | Same defect recurs, resolved item is not truly fixed, or open issue lacks escalation | QA must review the shared issue log, verify resolved items, and escalate unresolved material items | Did any risk become an issue? Is it logged? Is "resolved" supported by evidence? Were open issues escalated to the Project Manager persona and the user? | Shared issue log, regression evidence, escalation note, retest evidence |

## Minimum Control Expectations For Every Chat Inspection

- Identify the exact chat or work item under review.
- Identify the files, branches, or artifacts referenced in the chat.
- Test the chat against this matrix, not just general impressions.
- Record findings with severity and supporting evidence.
- Assess whether the relevant controls are effectively designed, not just whether they exist.
- Identify whether any reviewed risks became actual issues.
- Review the shared issue log.
- State whether the matrix or workplan should be enhanced based on new failure patterns.

## Enhancement Rule

Enhance this matrix when a review finds any of the following:

- a material failure mode not already captured
- a control that was too vague to test consistently
- a repeated issue that needs sharper evidence requirements
- a project-specific governance risk that has become recurring

When enhancing the matrix:

- preserve existing IDs when possible
- add new IDs only for genuinely distinct risks
- keep controls testable, not philosophical
- update the linked audit workplan if the audit steps also need to change
