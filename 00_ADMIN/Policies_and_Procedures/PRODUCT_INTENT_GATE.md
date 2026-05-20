# Product Intent Gate

## Purpose

This gate exists because Holesy product decisions are cumulative. A later implementation or packaging step must carry forward approved design and architecture decisions instead of treating the current file shape as permission to drift.

## Required Use

Run this gate before:

- release packaging
- GoDaddy upload preparation
- master promotion
- backlog reprioritization
- architecture or performance work
- new gameplay systems
- any answer that tells the user what files are or are not required

## Gate Questions

1. What did the user ask for right now?
2. What prior product, architecture, or governance decisions constrain this answer?
3. What governed document proves those constraints?
4. Does the requested action preserve those constraints?
5. If it does not, is this an explicit temporary exception approved by the user?
6. What artifact will future chats read so this decision survives context loss?

## Required Evidence

At minimum, inspect:

- `00_ADMIN/Requirements/PRODUCT_BACKLOG.md`
- `00_ADMIN/Requirements/ARCHITECTURE_DECISION_MODULAR_CLIENT_SPLIT.md`
- `00_ADMIN/Policies_and_Procedures/RELEASE_SOURCE_OF_TRUTH_MANIFEST.md`
- `00_ADMIN/Reviews_and_Reports/ISSUE_LOG.md`
- newest `NEXT_CODEX_CHAT_HANDOFF_MASTER*.md`
- `10_SOURCE/Current/CURRENT_BASIS.md`

## Release Packaging Rule

The accepted architecture is modular browser-client assets. The current single-file package is a temporary exception only.

For every release/package/upload answer, the final answer must say both:

- what is required for the immediate upload artifact
- whether that artifact aligns with or temporarily diverges from the modular architecture target
- whether `PERF-012 Incremental Modular Production Package Migration` remains the next required architecture alignment task

Never describe a bundled single-file package as the project direction unless the architecture decision has been explicitly changed.

Do not recommend a single-file production upload as the normal path. A single-file upload can only be presented as a temporary exception for the specific release being discussed.
