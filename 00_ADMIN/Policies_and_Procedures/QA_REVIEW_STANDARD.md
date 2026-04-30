## Holesy QA Review Standard

This document defines the separate QA mindset that should be applied before material conclusions are acted on.

## Purpose

The QA pass exists to challenge implementation and analysis before action is taken.

## QA Questions

Before acting, the reviewer should ask:

- Did we miss anything important?
- Are any claims misleading, overstated, or false?
- Are we drawing conclusions that are not supported by evidence?
- Are we confusing local state, Git state, GitHub state, and released state?
- Did we preserve rollback safety?
- Did we create a regression risk without naming it?
- Are instructions understandable to a beginner?

## Required QA Output

For each meaningful change or recommendation, the QA pass should determine:

- findings: what is wrong, risky, unclear, or unsupported
- status: pass or needs revision
- action: what must be corrected before proceeding

For chat-quality inspections triggered by the user, the QA pass must also:

- use `00_ADMIN/Policies_and_Procedures/QA_CHAT_RISK_AND_CONTROL_MATRIX.md`
- follow `00_ADMIN/Reviews_and_Reports/AUDIT_WORKPLAN_UNIFIED_QA_AND_RELEASE_CONTROLS.md`
- tie material findings to a specific risk/control area
- determine whether key controls are effectively designed to prevent or detect the stated risks
- determine whether any risks became actual issues
- identify lessons learned and any needed control updates
- review the shared issue log and verify:
  - resolved issues are truly resolved
  - unresolved issues are escalated to the Project Manager persona and the user
- state whether the matrix or workplan should be enhanced based on what was learned

## Pass Criteria

The work should only be treated as ready when the QA pass has no material findings left open.

## Holesy-Specific Focus Areas

- branch and backup state
- source of truth clarity
- platform-specific regressions
- user-facing instructions
- release readiness
