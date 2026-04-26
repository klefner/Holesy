# Holesy

Holesy is the active game repository for the project currently being developed and tested on desktop and mobile browsers.

## What This Repository Contains

- Current source baselines and promoted masters
- Candidate and exploratory browser test builds
- Project governance documents
- Requirements, reviews, and operating procedures

## How We Use GitHub For This Project

This repository now supports the project SDLC in a more deliberate way:

- `main` should represent the closest stable shared baseline.
- Active changes should happen in branches, not directly in `main`.
- A pull request should be opened before merging significant changes to `main`.
- Requirements, bug reports, and feature requests should be tracked in GitHub Issues.
- Approved milestones such as `Master 4` should eventually be represented by Git tags/releases, not only by filenames.

## Directory Map

- `00_ADMIN`
  Project guidance, policies, reviews, and requirements
- `10_SOURCE`
  Canonical source files, masters, and imported baselines
- `20_TESTS`
  Candidate builds and exploratory test artifacts
- `30_ARCHIVE`
  Historical source and retired artifacts kept for recovery
- `99_TEMP`
  Temporary scratch area

## Current Operating Model

- The current shared backup branch is `codex/publish-master4-structure`.
- The current promoted master is stored in `10_SOURCE/Masters/Master 4.html`.
- The current mouse-recovery candidate build is stored in `20_TESTS/Candidate_Builds/Master 4 - master1-mouse-reuse.html`.

## Start Here

If you are trying to understand the project quickly, read these files first:

- `READ ME - HOW TO NAVIGATE THESE DIRECTORIES.md`
- `00_ADMIN/INDEX.md`
- `00_ADMIN/Policies_and_Procedures/RISK_AND_CONTROLS_POLICY.md`
- `00_ADMIN/Policies_and_Procedures/GITHUB_OPERATING_MODEL.md`

## Important Note

Some GitHub protections and account settings cannot be fully enforced by repo files alone. Those items are documented in `00_ADMIN/Policies_and_Procedures/GITHUB_OPERATING_MODEL.md` and should be configured in the GitHub web UI.
