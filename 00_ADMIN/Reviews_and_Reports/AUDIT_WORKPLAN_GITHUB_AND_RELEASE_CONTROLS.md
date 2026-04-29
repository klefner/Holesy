# Holesy Audit Workplan: GitHub, Masters, Candidates, And Release Controls

## Purpose

Use this workplan on a semi-regular basis to verify that:

- approved masters exist locally and on GitHub
- the branch state, local state, and published understanding are aligned
- candidate history is preserved
- important governance documents were updated when a master was promoted
- live website publishing decisions are traceable and rollback-safe

Recommended cadence:

- after every master promotion
- before any push to `main`
- before any live website update
- weekly if active development is happening

## Control Matrix

| Risk category | Risk description | Control | Test steps | Evidence to inspect |
| --- | --- | --- | --- | --- |
| Change management | Approved local master exists but was never committed or pushed to GitHub | Every approved master promotion must end with a git status check, commit, push, and explicit confirmation of GitHub branch state | 1. Open local `10_SOURCE/Masters`. 2. Note newest approved master. 3. Open GitHub branch view for `10_SOURCE/Masters`. 4. Confirm same master file exists there. 5. Confirm commit history shows when it was added. | Local file list, GitHub branch file list, commit history, push confirmation |
| Change management | Candidate build accepted by user but never promoted to master | Promotion checklist requires linking accepted candidate to the new approved master in repo docs | 1. Identify accepted candidate file. 2. Open `CURRENT_BASIS.md`. 3. Confirm it names the approved master and last approved candidate. | `CURRENT_BASIS.md`, approved candidate file, approved master file |
| Branch governance | Work exists only on a feature branch and user mistakenly assumes it is on `main` | Every status report must distinguish local, branch/GitHub, `main`, and live website state | 1. Open GitHub and confirm selected branch. 2. Compare `main` vs working branch. 3. Confirm whether the change is only on the branch or merged. | GitHub branch selector, compare view, assistant status message |
| Documentation governance | Master promoted but handoff/basis/backlog docs not updated | Master promotion requires updating basis note, backlog progress, and next handoff | 1. Open `CURRENT_BASIS.md`. 2. Open `PRODUCT_BACKLOG.md`. 3. Open the latest `NEXT_CODEX_CHAT_HANDOFF_MASTER*.md`. 4. Confirm all reference the same current master. | Those three files, matching master number, matching last approved candidate note |
| Release traceability | Build numbers are ambiguous and feedback cannot be tied to the correct candidate | Every candidate file must have a unique filename and on-screen build label | 1. Open candidate file name. 2. Launch candidate in browser. 3. Confirm bottom-left build badge matches file lineage. | Candidate filename, in-game build badge screenshot |
| Repository completeness | Candidate history or masters are missing from GitHub branch | Approved masters and meaningful candidate builds must be committed to the working branch | 1. Open GitHub branch `10_SOURCE/Masters`. 2. Confirm master sequence is continuous. 3. Open `20_TESTS/Candidate_Builds`. 4. Confirm significant candidates exist. | GitHub tree listing, commit history |
| Staging hygiene | Unrelated exploratory files are swept into commits | Known exclude files are reviewed before commit | 1. Run `git status --short`. 2. Confirm `20_TESTS/Candidate_Builds/index.html` and `20_TESTS/Exploratory_Builds/Stack-collapse exploration - cannon-es prototype.html` are not staged unless explicitly intended. | `git status --short`, staged file list |
| Version accuracy | On-screen version label does not match actual approved master or candidate | Build label must be checked before acceptance/promotion | 1. Launch the local file under test. 2. Check build badge. 3. Confirm it matches file name and promotion state. | Browser screenshot, local file path |
| Rollback safety | Live website updated from an unapproved or untraceable file | Only approved master or explicitly named publish file may be used for live update | 1. Confirm the exact local file used for upload. 2. Confirm it matches approved master or approved release package. 3. Confirm prior live file was archived or can be restored. | Publish note, local file path, GoDaddy upload evidence, rollback copy |
| Website deployment | GitHub state, local state, and live website state diverge without anyone noticing | Separate state reporting is mandatory after each material change | 1. Ask or inspect: what is local approved master, what is pushed branch state, what is in `main`, what is live? 2. Confirm differences are explicitly documented. | Status message, GitHub branch, live site test |
| Testing assurance | A master is promoted without recorded user validation | Promotion should only follow explicit user acceptance or documented QA rationale | 1. Find the candidate file. 2. Find the user acceptance note. 3. Confirm the promoted master matches that candidate. | Conversation log, candidate file, master file |
| Process transparency | Assistant gives incomplete status in a way that hides missing GitHub updates | End-of-step status must explicitly state whether commit/push happened | 1. Review the latest completion message. 2. Confirm it says whether git commit happened, whether push happened, and branch name. | Assistant summary message, git log, GitHub branch |

## Semi-Regular Audit Steps

## A. Master Promotion Audit

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

- matching master number across all three documents
- approved master file exists locally

## B. GitHub Completeness Audit

Run this after any push, and weekly during active development.

1. Open GitHub repo.
2. Verify selected branch.
3. Open:
   - `10_SOURCE/Masters`
4. Confirm expected masters exist.
5. Open:
   - `20_TESTS/Candidate_Builds`
6. Confirm significant approved candidates exist.
7. Review the most recent commit affecting masters/candidates.

Pass evidence:

- GitHub branch tree contains expected files
- commit history shows when they were added

## C. Pre-Commit Hygiene Audit

Run before any commit.

1. Run `git status --short --branch`.
2. Review staged files.
3. Confirm known junk/untracked files are not included by accident.
4. Confirm the staged set matches the intended scope.

Pass evidence:

- clean staged file list
- excluded known exploratory files remain unstaged

## D. Release Readiness Audit

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

Pass evidence:

- file path used for publish
- rollback copy path
- GitHub branch tree
- live URL test result

## E. Candidate Traceability Audit

Run whenever testing feedback is being collected.

1. Confirm candidate filename is unique.
2. Confirm in-game build badge matches candidate lineage.
3. Record feedback against that exact build.
4. If the candidate changes materially, require a new sub-build number and file.

Pass evidence:

- candidate file path
- screenshot of in-game build badge
- notes tied to exact build number

## Minimum Evidence Pack To Save

For important checkpoints, keep:

- screenshot of local in-game build badge
- screenshot of GitHub branch tree for `10_SOURCE/Masters`
- `git status --short --branch` output
- commit hash after push
- note of exact file used for live publish

## Fast 2-Minute Audit

If time is short, do these five checks:

1. What is the current approved local master?
2. Does `CURRENT_BASIS.md` name that same master?
3. Is that master visible on the correct GitHub branch?
4. Did the latest completion message explicitly say commit/push happened?
5. Are the known exploratory files still unstaged?

If any answer is `no`, stop and resolve before treating the work as fully controlled.
