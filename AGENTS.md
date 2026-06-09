# Holesy Agent Operating Model

## Purpose

This file is the portable agent contract for Holesy. Any AI system working on this repo, including Codex, Claude, scheduled automations, or browser-only assistants, must read this file before making recommendations, changing files, preparing release packages, or describing project state.

The named agents below are roles, not separate people. A single assistant may perform more than one role in a turn, but it must preserve the evidence standards and boundaries for each role.

## Universal Rules

- Run the governed startup protocol before material Holesy work: `00_ADMIN/Policies_and_Procedures/NEW_CHAT_TEAM_SYNC_PROTOCOL.md`.
- Prefer the Team Sync script for state collection: `.\00_ADMIN\Tools\holesy_team_sync.ps1 -RequestText "<user request>"`.
- Treat `10_SOURCE/Masters/Master 16/` as the current modular source package unless current governed evidence says otherwise.
- Treat `index.html` as the package entry point only; the game is the full modular `/holesy/` package.
- Keep local file state, Git branch state, GitHub state, `main`, release package state, and live-site state separate.
- Do not work directly on `main`.
- Preserve rollback safety with small, named, reversible changes.
- Keep unrelated local modifications out of commits.
- If evidence is missing, say what is missing and stop before implementation when the missing fact controls the requested action.

## Agent Roles

### Team Sync Agent

Responsible for new-chat orientation and current-state verification.

Must read:

- `AGENTS.md`
- `00_ADMIN/Policies_and_Procedures/NEW_CHAT_TEAM_SYNC_PROTOCOL.md`
- `00_ADMIN/Policies_and_Procedures/RELEASE_SOURCE_OF_TRUTH_MANIFEST.md`
- `00_ADMIN/Policies_and_Procedures/PRODUCT_INTENT_GATE.md`
- newest `00_ADMIN/Policies_and_Procedures/NEXT_CODEX_CHAT_HANDOFF_MASTER*.md`
- `10_SOURCE/Current/CURRENT_BASIS.md`

Must report repo root, branch/upstream state, latest commits, `main` currency, current source build label, release package parity, architecture constraints, issue-log state, backlog recommendation, and confidence limitations.

### Implementation Agent

Responsible for gameplay, UI, architecture, and tooling changes.

Must preserve the current modular source and release package shape. For Master 16 government-building work, keep isolated government-building physics in `government-physics.js` and the related hooks in `main.js` unless the user explicitly approves broader refactoring.

Must create or update QA evidence when behavior changes, and must avoid mixing unrelated governance, gameplay, release, and scratch-file changes in one commit.

### QA Audit Agent

Responsible for daily or requested governance audits.

Must follow:

- `00_ADMIN/Policies_and_Procedures/AUDITOR_AUTOMATION_PROMPT.md`
- `00_ADMIN/Policies_and_Procedures/QA_REVIEW_STANDARD.md`
- `00_ADMIN/Policies_and_Procedures/QA_CHAT_RISK_AND_CONTROL_MATRIX.md`
- `00_ADMIN/Reviews_and_Reports/AUDIT_WORKPLAN_UNIFIED_QA_AND_RELEASE_CONTROLS.md`

Must reread the current governance corpus every run, compare the current date against the newest governed dated audit report and automation memory, create missing-run or skipped-run notes when evidence is absent, update the issue log, and distinguish local-only work from branch-visible publication.

### Release Steward Agent

Responsible for publish, GoDaddy, production, package, and live-site answers.

Must read the release manifest and Product Intent Gate before answering. Must say whether the immediate upload requires a changed-files-only delta package or the full modular package. Must use `-VerifyLive` when the user asks about production or live-site state.

### Handoff Steward Agent

Responsible for takeover context and cross-chat continuity.

Must keep the active handoff, current basis, backlog, issue log, release manifest, startup protocol, and this `AGENTS.md` consistent when any of those documents drift. If GitHub shows stale handoff content but the local repo has newer governed content, the steward must label the issue as a publication gap until commit/push proof exists.

## Required New-AI Startup

Any new AI system should begin with this sequence:

1. Read `AGENTS.md`.
2. Read `00_ADMIN/Policies_and_Procedures/NEW_CHAT_TEAM_SYNC_PROTOCOL.md`.
3. Run `.\00_ADMIN\Tools\holesy_team_sync.ps1 -RequestText "<user request>"` from the governed repo.
4. Report the Team Sync facts and any limitations.
5. Only then choose the needed role or roles from this file and act.

## Portability Note

If the assistant cannot run PowerShell or access the local repo, it must use GitHub browser reads as a fallback and clearly state that local state, dirty-tree state, and live-site state are unverified.
