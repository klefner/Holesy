# Holesy QA Review: Repo State Verification 2026-04-29

Inspection target:

- takeover verification pass for current Holesy project state, branch state, approved basis, candidate state, issue-log claims, and live-state separation

Evidence used:

- governing docs in `00_ADMIN\Policies_and_Procedures`
- `00_ADMIN\Reviews_and_Reports\ISSUE_LOG.md`
- `10_SOURCE\Current\CURRENT_BASIS.md`
- `00_ADMIN\Requirements\PRODUCT_BACKLOG.md`
- `git branch --show-current`
- `git status --short --branch`
- local file existence checks
- local git history and tree inspection
- local file hashes
- allowed web inspection of `https://ptbooksinc.com/holesy/`

## Current Project State

- active repo root is `C:\Users\KentLefner\Desktop\game-repo\Holesy`
- current approved local source basis is `Master 14`
- current working branch is `codex/publish-master4-structure`
- branch head is commit `39bbd94`
- two unrelated local modified files are present and unstaged

## Approved Master

- verified approved local master: `10_SOURCE\Masters\Master 14.html`
- supporting evidence:
  - `10_SOURCE\Current\CURRENT_BASIS.md` names `Master 14`
  - `10_SOURCE\Masters` contains `Master 14.html`
- mismatch found:
  - `NEXT_CODEX_CHAT_HANDOFF_MASTER13.md` still states `Master 13` as current and is therefore historical, not current source of truth

## Current Branch

- verified current branch: `codex/publish-master4-structure`
- verified local and remote branch heads align at `39bbd94`

## Current Candidate

- latest in-flight candidate present locally:
  - `20_TESTS\Candidate_Builds\Master 14.1 - debug-overlay-unit-clear-refund.html`
- last approved candidate promoted into current master:
  - `20_TESTS\Candidate_Builds\Master 13.1 - unit-clear-growth-retune.html`

## Local Modified Files

- `00_ADMIN\Policies_and_Procedures\NEXT_CODEX_CHAT_HANDOFF_MASTER7.md`
- `00_ADMIN\Reviews_and_Reports\QA_REVIEW_NEXT_CHAT_HANDOFF_MASTER7.md`
- both remain locally modified and were left untouched by this pass

## QA Issue Status

- `QA-001`: verified resolved
  - evidence in issue log matches branch history and remote tree state
- `QA-002`: verified resolved in the narrow sense claimed
  - commit `39bbd94` exists and added the governed exploratory prototype to version control
  - `20_TESTS\Candidate_Builds\index.html` is absent locally
  - `git status --short --branch` no longer shows either file as accidental untracked noise
- control note:
  - the current `Master 14` handoff had stale wording implying both files were still recurring untracked noise, so handoff governance needed refresh even though `QA-002` itself was remediated

## GitHub Branch State

- `origin/codex/publish-master4-structure` contains:
  - `Master 14.html`
  - `Master 14.1 - debug-overlay-unit-clear-refund.html`
  - the governed exploratory prototype
- remote branch head matches local head at `39bbd94`

## Main State

- local `main` and `origin/main` both resolve to commit `9c3eba1`
- `main` was not updated with the later master lineage or governance additions now present on `codex/publish-master4-structure`

## Live Website State

- local publish package `40_RELEASE\Website_Publish_Package\holesy\index.html` hashes to the same content as `10_SOURCE\Masters\Master 6.html`
- local publish package does not match `Master 14`
- commit `39bbd94` changed only:
  - `00_ADMIN\Reviews_and_Reports\ISSUE_LOG.md`
  - `20_TESTS\Exploratory_Builds\Stack-collapse exploration - cannon-es prototype.html`
- therefore there is no repo evidence that the `QA-002` remediation updated the local publish package or changed what should be live
- limitation:
  - direct shell fetch of the live URL was blocked by environment socket restrictions
  - allowed web inspection reached `https://ptbooksinc.com/holesy/` but did not expose enough game-source detail to hash-match the deployed runtime in this pass

## Recommended Next Step

- keep `Master 14` as the approved local gameplay basis
- treat `NEXT_CODEX_CHAT_HANDOFF_MASTER14.md` plus `CURRENT_BASIS.md` as the active handoff path, not the older `Master 13` handoff
- if the next goal is release movement rather than more gameplay work, run a separate release-readiness pass before changing `40_RELEASE\Website_Publish_Package\holesy\index.html` or the live `/holesy/` site
- if the next goal is development, continue from `Master 14.1 - debug-overlay-unit-clear-refund.html` with the same branch and preserve the two unrelated local modified files

QA finding summary:

- status: pass with governance mismatch corrected
- material finding addressed:
  - historical handoff drift could mislead a replacement chat about the current approved basis and local-status hygiene
- residual risk:
  - live website runtime could not be fully source-matched from this environment during this pass
