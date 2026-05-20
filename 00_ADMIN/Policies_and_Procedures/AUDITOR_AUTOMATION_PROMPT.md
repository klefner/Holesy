# Auditor Automation Prompt

## Purpose

This file records the required prompt intent for the recurring Daily QA Audit automation so the automation can be checked against governed repo expectations.

## Current Required Prompt Intent

The Daily QA Audit automation must:

- perform a Holesy QA audit as described in the unified QA workplan once every 24 hours
- study the current process and procedure governance corpus every time before drawing conclusions
- include, at minimum:
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

## Automation Updated

The local Codex automation `daily-qa-audit` was updated on 2026-05-20 to reflect this prompt intent.
