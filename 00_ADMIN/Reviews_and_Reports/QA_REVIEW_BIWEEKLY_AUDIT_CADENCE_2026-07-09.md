# QA Review - Biweekly Audit Cadence Change

Date: 2026-07-09

## Scope

User requested changing the recurring Holesy QA audit cadence from daily to once every two weeks because daily execution consumes too many tokens for the current project phase.

## Automation Update

- Existing automation id: `daily-qa-audit`
- New in-app name: `Biweekly QA Audit`
- Status: `ACTIVE`
- Execution environment: `local`
- Workspace: `C:\Users\KentLefner\Desktop\game-repo\Holesy`
- New cadence: once every two weeks on Monday at 9:00 AM local time

## Governed Procedure Updates

- `AUDITOR_AUTOMATION_PROMPT.md` now records the biweekly cadence and non-scheduled-day rule.
- `QA_REVIEW_STANDARD.md` now treats only expected biweekly scheduled audit dates as cadence obligations.
- `AUDIT_WORKPLAN_UNIFIED_QA_AND_RELEASE_CONTROLS.md` now uses biweekly-audit cadence evidence.
- `holesy_team_sync.ps1` drift markers now match the biweekly automation prompt.
- `AGENTS.md`, the startup protocol, release manifest, and issue log now reference the biweekly control.

## Control Decision

Daily audit evidence remains valid historical evidence from the prior cadence. Going forward, ordinary days between scheduled biweekly audits should not produce missing-run notes. Missing-run or skipped-run notes should only be created when an expected biweekly scheduled audit date lacks governed evidence.
