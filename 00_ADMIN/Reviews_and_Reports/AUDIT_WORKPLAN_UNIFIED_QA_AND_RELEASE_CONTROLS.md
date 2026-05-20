# Holesy Audit Workplan: Unified QA And Release Controls

## Purpose

Use this workplan whenever the user triggers QA or when the project needs a control check over chat quality, branch state, masters, candidates, or live-release readiness.

This unified plan replaces separate chat-quality and GitHub/release-control workplans so the project has one durable audit method.

This workplan is designed to ensure reviews are:

- evidence-based
- repeatable
- tied to project controls
- clear about what is safe to rely on

Required inputs:

- `00_ADMIN/Policies_and_Procedures/QA_CHAT_RISK_AND_CONTROL_MATRIX.md`
- `00_ADMIN/Policies_and_Procedures/QA_REVIEW_STANDARD.md`
- `00_ADMIN/Policies_and_Procedures/RISK_AND_CONTROLS_POLICY.md`
- `00_ADMIN/Policies_and_Procedures/GITHUB_OPERATING_MODEL.md`
- `00_ADMIN/Policies_and_Procedures/STANDALONE_WEBSITE_PUBLISH_WORKFLOW.md`
- `00_ADMIN/Policies_and_Procedures/RELEASE_SOURCE_OF_TRUTH_MANIFEST.md`
- `00_ADMIN/Policies_and_Procedures/PRODUCT_INTENT_GATE.md`
- `00_ADMIN/Policies_and_Procedures/NEW_CHAT_TEAM_SYNC_PROTOCOL.md`
- `00_ADMIN/Policies_and_Procedures/AUDITOR_AUTOMATION_PROMPT.md`
- `00_ADMIN/Tools/holesy_team_sync.ps1` output when startup context, release state, automation drift, or package integrity is part of the audit
- newest `00_ADMIN/Policies_and_Procedures/NEXT_CODEX_CHAT_HANDOFF_MASTER*.md`
- `10_SOURCE/Current/CURRENT_BASIS.md`
- `00_ADMIN/Requirements/PRODUCT_BACKLOG.md`
- relevant architecture decision records in `00_ADMIN/Requirements`
- `00_ADMIN/Reviews_and_Reports/ISSUE_LOG.md`
- the specific chat, files, branches, and artifacts being evaluated

Every audit must reread the current process and procedure governance corpus. Do not rely on a prior audit's memory of these controls.

## When To Use

- when the user explicitly triggers QA for a chat
- before relying on a prior chat's recommendation for a risky action
- after every master promotion
- before any push to `main`
- before any live website update
- weekly if active development is happening
- after a problematic chat or release decision to capture lessons into the control system

## Audit Objectives

1. Determine whether the reviewed chat or workflow output was accurate, controlled, and appropriately evidenced.
2. Identify risks, control failures, missing governance updates, and misleading statements.
3. Decide whether the output is safe to rely on as-is, needs revision, or should be rejected.
4. Verify alignment across local state, working branch state, `main`, and live website state.
5. Determine whether the stated controls are effectively designed to prevent or detect the risk.
6. Determine whether any risks became actual issues and what lessons learned should update controls.
7. Verify resolved issues are truly resolved and unresolved issues are escalated to the Project Manager persona and the user.
8. Periodically improve the matrix and this workplan when new failure patterns appear.
9. Determine whether current process/procedure governance documents themselves need revision based on the audit evidence, and update them when needed.

## Unified Control Matrix

| Risk area | Risk description | Required control | Core test steps | Expected evidence |
| --- | --- | --- | --- | --- |
| Scope control | The chat solves a different problem than the one requested | Restate objective and compare delivered scope to requested scope | 1. Read user ask. 2. Read completion. 3. Compare outputs to request. | User request, assistant completion, changed files |
| Source-of-truth clarity | Local files, branch state, `main`, GitHub, and live website state are confused | Status reporting must separate each state when relevant | 1. Inspect repo status and branch. 2. Review assistant status wording. 3. Confirm whether `main` or live site were mentioned accurately. | `git status --short --branch`, branch name, status message, publish note |
| Evidence quality | Material claims are unsupported | High-impact claims require inspected files, repo checks, tests, or explicit limitations | 1. List key claims. 2. Match each to evidence. 3. Flag unsupported conclusions. | Files opened, command outputs, test results |
| Risk disclosure | The chat hides risk or uncertainty | Open risks, unverified areas, and residual uncertainty must be stated plainly | 1. Review completion and QA language. 2. Check whether unknowns were disclosed. | Completion text, QA findings |
| Rollback safety | Source, master, candidate, or release action lacks recovery path | Baseline and rollback path must be identifiable before risky action | 1. Identify baseline file or branch. 2. Check rollback path. 3. Confirm recovery would be practical. | Baseline reference, branch, prior master, backup artifact |
| Master governance | Approved candidate was not promoted correctly or approved master was not preserved | Promotion must update master files and supporting governance docs | 1. Check newest master. 2. Check `CURRENT_BASIS.md`. 3. Check backlog and handoff. | Master file list, basis file, backlog, handoff |
| GitHub completeness | Approved artifacts exist locally but not on the intended GitHub branch | Important masters and candidates must be committed and pushed to the correct branch | 1. Check local artifact. 2. Check branch tree or history on GitHub. 3. Confirm push state. | Local file list, GitHub tree, commit history |
| Candidate traceability | Feedback cannot be tied to a precise build | Candidate files and in-game build labels must be unique and aligned | 1. Inspect candidate filename. 2. Launch or inspect build label. 3. Confirm notes tie to that build. | Candidate path, build badge, QA notes |
| Staging hygiene | Wrong or exploratory files get swept into commits | Staged scope must match intended scope and known junk must stay out unless intended | 1. Run `git status --short --branch`. 2. Review staged and untracked files. 3. Compare to intended scope. | Git status output, staged file list |
| Testing sufficiency | Readiness is implied without enough validation | Validation must match risk and affected systems | 1. Identify affected systems. 2. Check what was tested. 3. Flag missing testing or disclosure. | Test commands, manual test notes, stated gaps |
| Release readiness | Live website is updated from an unapproved or untraceable artifact | Only approved master or explicitly approved publish artifact may be used for live update | 1. Identify exact publish file. 2. Confirm approval state. 3. Confirm rollback copy. 4. Confirm GitHub branch contains the approved file. | Publish file path, rollback path, branch tree, live test note |
| Documentation governance | Control-significant actions are not reflected in basis, backlog, handoff, or reports | Governance docs must be updated when promotions or major process changes occur | 1. Check impacted docs. 2. Confirm they reference the same current state. | Basis file, backlog, handoff, report files |
| Process compliance | Project policies or conventions were bypassed | Work must be checked against governing docs and exceptions justified | 1. Compare actions to policies. 2. Note any exception handling. | Governing docs, chat actions, completion notes |
| Communication honesty | Limitations are hidden or softened into false confidence | The final message must disclose what was not done, not known, or blocked | 1. Compare tool results to completion claims. 2. Flag omissions. | Tool outputs, completion wording |
| Control design effectiveness | A control exists but is not specific or strong enough to work in practice | Controls must be testable, timely, and clearly linked to the risk they address | 1. Inspect the control design. 2. Ask whether it would really detect or prevent the failure. 3. Flag vague or late controls. | Control wording, timing, evidence requirement |
| Issue management | Risks that became issues are not logged, verified, or escalated | QA must review the shared issue log, retest claimed resolutions, and escalate unresolved material items | 1. Open issue log. 2. Review resolved and unresolved items. 3. Verify evidence. 4. Escalate open material items. | Issue log, retest evidence, escalation note |
| Continuous improvement | Repeated failure modes are not folded into controls | Each audit must consider whether the matrix or workplan needs enhancement | 1. Review findings. 2. Decide whether a control update is needed. | Matrix/workplan revision note |
| Product intent continuity | Approved product, design, or architecture decisions are lost across chats | Product Intent Gate must run before release, architecture, backlog, or material implementation decisions | 1. Open the Product Intent Gate. 2. Inspect relevant ADRs, backlog, basis, handoff, and issue log. 3. Confirm the requested action preserves or explicitly excepts prior decisions. | Product intent gate result, governing docs, exception note if any |
| Process/procedure currency | The auditor relies on stale knowledge of project controls | Every audit must study all current process/procedure docs and decide whether they need updates | 1. Open the governance corpus listed in Required inputs. 2. Compare controls to observed failure modes. 3. Update procedures, matrix, workplan, or issue log when controls are incomplete. | List of governance docs inspected, control-update decision, changed docs if needed |

## Standard Audit Steps

## A. Define The Inspection Target

1. Identify the exact chat, workflow step, branch state, or release decision under review.
2. State the apparent user objective.
3. State the material outputs being relied upon:
   - recommendation
   - code change
   - status statement
   - master promotion
   - release/publish decision
   - handoff or governance update

Pass evidence:

- named review target
- clear statement of intended reliance

## B. Gather Evidence

1. Read the relevant chat messages.
2. Open the current process/procedure governance corpus listed in Required inputs.
3. Open referenced repo files, governance docs, and existing reports.
4. Check current repo state when relevant:
   - `git status --short --branch`
   - current branch
   - staged and unstaged scope
5. Capture test evidence or explicit lack of it.
6. If GitHub or live website state matters, inspect those states or state the limitation clearly.
7. Open the shared issue log and identify items relevant to the review target.
8. Decide whether any process/procedure docs need updates; if yes, update them as part of the audit package.

Pass evidence:

- inspected files and repo state are listed
- process/procedure docs inspected are listed
- key claims can be traced to evidence
- procedure update decision is stated

## C. Run Chat-Quality Controls

Apply the chat-quality portions of the matrix, at minimum:

- scope control
- source-of-truth clarity
- product intent continuity
- evidence quality
- risk disclosure
- testing sufficiency
- process compliance
- communication honesty
- control design effectiveness
- issue management
- process/procedure currency

Pass evidence:

- each material chat finding ties to a matrix control area
- the audit states whether key controls are effectively designed or not
- the audit states whether current procedures needed updates and what changed

## D. Run Repository And Release Controls

Apply the release-governance portions of the matrix when relevant.

### Master Promotion Audit

Run this after any master promotion.

1. Open local folder:
   - `C:\Users\KentLefner\Desktop\game-repo\Holesy\10_SOURCE\Masters`
2. Confirm the newest approved file exists.
3. Open:
   - `C:\Users\KentLefner\Desktop\game-repo\Holesy\10_SOURCE\Current\CURRENT_BASIS.md`
4. Confirm:
   - current master number matches the newest approved master
   - last approved candidate is named
5. Open:
   - `C:\Users\KentLefner\Desktop\game-repo\Holesy\00_ADMIN\Requirements\PRODUCT_BACKLOG.md`
6. Confirm progress note references the same master.
7. Open the newest:
   - `C:\Users\KentLefner\Desktop\game-repo\Holesy\00_ADMIN\Policies_and_Procedures\NEXT_CODEX_CHAT_HANDOFF_MASTER*.md`
8. Confirm it also references the same master.

Pass evidence:

- matching master number across governance docs
- approved master file exists locally

### GitHub Completeness Audit

Run this after any push, and weekly during active development.

1. Open GitHub repo and verify selected branch.
2. Open:
   - `10_SOURCE/Masters`
3. Confirm expected masters exist.
4. Open:
   - `20_TESTS/Candidate_Builds`
5. Confirm significant approved candidates exist.
6. Review the most recent commit affecting masters or candidates.

Pass evidence:

- GitHub branch tree contains expected files
- commit history shows when they were added

### Pre-Commit Hygiene Audit

Run before any commit.

1. Run `git status --short --branch`.
2. Review staged files.
3. Confirm known junk or exploratory files are not included by accident.
4. Confirm the staged set matches intended scope.

Pass evidence:

- clean staged file list
- excluded known exploratory files remain unstaged unless explicitly intended

### Release Readiness Audit

Run before publishing the live website.

1. Identify exact file to publish.
2. Confirm whether it is:
   - approved master
   - release package
   - named exception approved for publish
3. Confirm rollback file exists.
4. Confirm GitHub branch already contains the approved file.
5. Confirm status note clearly distinguishes:
   - local approved file
   - GitHub branch state
   - `main` state
   - live website state
6. Run the Product Intent Gate.
7. If the publish artifact is bundled as `index.html`, confirm whether that is a temporary deployment artifact or the accepted architecture target.
8. If the accepted architecture target is modular, require the final publish guidance to distinguish immediate upload files from future modular package requirements.

Pass evidence:

- file path used for publish
- rollback copy path
- GitHub branch tree
- live URL test result or explicit limitation
- product intent gate result
- modular architecture alignment or exception statement

### Candidate Traceability Audit

Run whenever testing feedback is being collected.

1. Confirm candidate filename is unique.
2. Confirm in-game build badge matches candidate lineage.
3. Record feedback against that exact build.
4. If the candidate changes materially, require a new sub-build number and file.

Pass evidence:

- candidate file path
- screenshot or observation of in-game build badge
- notes tied to exact build number

## E. Rate Findings

Use these severity levels:

- Critical: unsafe to rely on without correction
- High: major control gap or misleading conclusion
- Moderate: meaningful weakness that can cause rework or confusion
- Low: minor clarity or completeness issue

For each finding, record:

- risk area or matrix ID
- severity
- statement of the issue
- why it matters
- supporting evidence
- corrective action

Pass evidence:

- findings are concrete and supported
- severity is proportional to impact

## F. Review Issue Log Status

1. Open:
   - `C:\Users\KentLefner\Desktop\game-repo\Holesy\00_ADMIN\Reviews_and_Reports\ISSUE_LOG.md`
2. Identify issues relevant to the audit target.
3. For each issue marked `resolved`:
   - verify the fix or control actually exists
   - verify no contradictory evidence shows the issue is still open
4. For each issue marked `open`, `monitor`, or equivalent and still material:
   - confirm it is explicitly escalated to the Project Manager persona
   - confirm it is explicitly escalated to the user
5. If an audit finds a new issue, add it to the issue log or state clearly that logging is required before closure.

Pass evidence:

- resolved issues were retested or otherwise verified
- unresolved material issues show explicit escalation
- new issues are logged or called out for immediate logging

## G. Conclude Reliance Status

Set one overall outcome:

- Approved: safe to rely on
- Approved with cautions: usable, but with named limitations
- Needs revision: do not rely on until corrected
- Rejected: materially unsafe or misleading

Pass evidence:

- outcome matches findings
- any conditions for safe reliance are explicit

## H. Lessons Learned And Control Enhancements

After the report is drafted, ask:

1. Did this audit reveal a new risk pattern?
2. Was any existing control too weak or hard to apply?
3. Did the report need evidence or steps that this workplan did not require?
4. Did any risk become an actual issue because a control failed, was absent, or was poorly designed?

If yes:

- update the matrix
- update this workplan
- update the issue log if needed
- record the enhancement in the report

Pass evidence:

- enhancement decision is stated, even if the answer is no change

## Required Report Format

Use this structure for future inspections:

```md
# Quality Inspection Report: [short title]

## Scope

- chat, workflow step, or release decision reviewed
- user objective
- material outputs reviewed

## Evidence Reviewed

- chat segments
- files opened
- repo or branch checks
- GitHub or live-site checks
- test evidence reviewed

## Control Assessment

| Risk Area | Result | Notes |
| --- | --- | --- |
| [area or ID] | Effective / Partial / Ineffective | Short evidence-based assessment |

## Findings

- [Severity] [Risk area or ID]: finding
  Evidence:
  Corrective action:

## Control Design Assessment

- which controls are effectively designed
- which controls are weak, late, vague, or not likely to work

## Issue Log Review

- resolved issues verified
- unresolved issues escalated to Project Manager persona and user
- new issues logged or required

## Overall Outcome

- Approved / Approved with cautions / Needs revision / Rejected

## Residual Risks

- remaining uncertainty

## Control Enhancements

- matrix or workplan updates made, or "none"

## Lessons Learned

- what should change going forward
```

## Minimum Evidence Pack To Save

For important checkpoints, keep:

- screenshot of local in-game build badge when build lineage matters
- screenshot of GitHub branch tree for `10_SOURCE/Masters` when promotion or release matters
- `git status --short --branch` output
- commit hash after push when push state matters
- note of exact file used for live publish when release matters

## Fast Audit Variant

If time is short, still do these checks:

1. Did the chat or workflow address the actual request?
2. Did it clearly distinguish local, branch, `main`, and live state where relevant?
3. Were major claims evidenced?
4. Were open risks and unverified areas disclosed?
5. Was rollback or recovery thinking preserved?
6. Were master, candidate, and GitHub governance checks satisfied if those areas were in scope?
7. Are the key controls effectively designed to prevent or detect the risks?
8. Did any risk become an actual issue, and if so was it logged and escalated?
9. Is there any reason the matrix or workplan should be enhanced?

If any answer is `no`, the report should not be fully approving without a caution or revision note.
