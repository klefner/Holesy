## Holesy GitHub Operating Model

This document explains how the Holesy repository should be operated in GitHub so the repo supports the SDLC instead of acting as a passive file bucket.

## Objectives

- Keep GitHub as the durable shared backup and history system
- Reduce the chance of accidental loss or overwrite
- Make work reviewable and easier to understand later
- Give the project a repeatable release path

## Decisions Made

- Canonical repository: `klefner/Holesy`
- Shared backup branch for the current reorganization and Master 4 work: `codex/publish-master4-structure`
- GitHub is now the system of record for backed-up project state after successful push
- Local test artifacts may still exist first, but approved work should be pushed promptly

## Required Working Pattern

1. Work locally in a branch.
2. Save meaningful checkpoints.
3. Push the branch to GitHub.
4. Open a pull request before merging meaningful changes into `main`.
5. Keep `main` as the stable shared baseline.

## Required Repository Features

These are the next-priority GitHub controls for this repo:

### 1. Pull Requests

- Use pull requests for meaningful merges into `main`
- Require a short description of what changed, why, risks, and test evidence

### 2. Branch Protection / Ruleset

Recommended configuration for `main`:

- Require a pull request before merging
- Require conversation resolution before merge
- Restrict direct pushes to `main`

### 3. Issues

Use GitHub Issues for:

- bugs
- feature requests
- technical debt
- release preparation tasks

### 4. Releases / Tags

When a Master is promoted and validated, consider creating:

- a Git tag
- a GitHub release entry

This should eventually become the official way to represent `Master 4`, `Master 5`, and later milestones.

## Assumptions Applied

- The project is currently single-owner and early-stage
- The user is new to GitHub-based SDLC practices
- Fast browser testing still matters, so not every exploratory file should be treated as a release artifact
- Not all GitHub settings can be changed automatically from this environment

## What Has Already Been Implemented

- Repository structure aligned to admin/source/tests/archive separation
- Shared risk and controls policy
- Root navigation guide
- Working backup branch pushed to GitHub
- Candidate build and source hierarchy preserved in version control

## What Still Requires GitHub Web UI

- Enabling 2FA on the account
- Creating a branch protection rule or ruleset for `main`
- Reviewing and merging pull requests
- Optionally creating GitHub releases

## Merge Guidance

Before merging a branch into `main`, confirm:

- the branch is backed up remotely
- the intended baseline is clear
- the user has validated the relevant build if gameplay is affected
- the PR description explains the scope and risks

## Technical Lead Commitment

Future GitHub recommendations and actions should follow this model unless the project explicitly adopts a newer shared-file version.
