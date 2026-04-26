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

## Pass Criteria

The work should only be treated as ready when the QA pass has no material findings left open.

## Holesy-Specific Focus Areas

- branch and backup state
- source of truth clarity
- platform-specific regressions
- user-facing instructions
- release readiness
