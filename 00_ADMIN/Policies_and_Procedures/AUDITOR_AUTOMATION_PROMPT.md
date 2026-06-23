# Auditor Automation Prompt

## Purpose

This file records the required prompt intent for the recurring Daily QA Audit automation so the automation can be checked against governed repo expectations.

## Current Required Prompt Intent

The Daily QA Audit automation must:

- perform a Holesy QA audit as described in the unified QA workplan once every 24 hours
- study the current process and procedure governance corpus every time before drawing conclusions
- include, at minimum:
  - root `AGENTS.md`
  - QA review standard
  - risk/control matrix
  - risk and controls policy
  - unified audit workplan
  - GitHub operating model
  - standalone publish workflow
  - release source-of-truth manifest
  - product intent gate
  - new-chat team sync protocol
  - active handoff
  - current basis
  - product backlog
  - architecture decision records
  - issue log
- assess whether project controls, processes, procedures, backlog, handoff, and issue log need to change based on current evidence
- make appropriate governed repo updates when process/procedure changes are needed
- write a dated audit report
- update the issue log
- verify tracked-state expectations
- clearly distinguish local state, branch/GitHub state, `main`, release package, and live-site state
- use the governed Team Sync script as a state-verification input when appropriate, including automation drift, governance corpus inventory, issue-log counts, backlog readiness, and source/package integrity checks
- verify that `AGENTS.md` remains present and referenced by the new-chat startup protocol, active handoff, and release/governance gates so Codex, Claude, and other AI systems inherit the same role definitions
- before completing each audit, compare the current date, the newest dated daily-audit report in the governed repo, and the automation memory
- if any expected daily-audit date since the previous governed audit has no report, create an explicit governed missing-run or skipped-run note for the missing date range, update the issue log, and state whether the gap was caused by automation not running, an unavailable worktree, Git publication failure, or unknown scheduler behavior
- if the current audit cannot write or publish its dated report, report that as a blocker rather than treating automation memory as durable evidence

## Automation Updated

The local Codex automation `daily-qa-audit` was updated on 2026-05-28 to reflect this prompt intent, including the missed-run backstop added after `QA-018`.

The local Codex automation `daily-qa-audit` was updated again on 2026-06-23 after the daily QA cadence gap for 2026-06-19 through 2026-06-22. The update changed the automation execution environment from `worktree` to `local` for the governed repo path `C:\Users\KentLefner\Desktop\game-repo\Holesy`, and refreshed the automation prompt so it explicitly includes root `AGENTS.md`, the governed startup protocol, the auditor automation prompt, architecture alignment review, Team Sync usage, and local/branch/main/release/live-state separation.
