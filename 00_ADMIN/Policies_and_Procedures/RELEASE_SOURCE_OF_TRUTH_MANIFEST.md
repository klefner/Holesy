# Holesy Release Source Of Truth Manifest

## Purpose

This manifest prevents future chats from collapsing local files, branch state, release packages, live-site state, and architecture direction into one misleading answer.

Before any release packaging, live upload, master promotion, backlog sequencing, or major feature implementation, the acting developer must check this manifest and report the relevant states separately.

## Current State As Of 2026-05-21

| Layer | Current value | Meaning |
| --- | --- | --- |
| Canonical repo | `C:\Users\KentLefner\Desktop\game-repo\Holesy` | Primary governed working checkout for current Holesy work. |
| Active branch | `codex/publish-master4-structure` | Branch carrying the current governed history and recent Master 16.x work. |
| Stable `main` | stale relative to active branch | `main` is not the current gameplay source of truth until an explicit PR/merge updates it. |
| Current approved source master | `10_SOURCE/Masters/Master 16/` | Governed modular source package for the current playable build line. |
| Current source entry point | `10_SOURCE/Masters/Master 16/index.html` | Browser entry point for the modular source package. |
| Current in-game build label | `Master 16.29` | Latest current patch label inside the approved Master 16 source package. |
| Current release package | `40_RELEASE/Website_Publish_Package/holesy/` | Current GoDaddy upload package; now follows the modular browser-client folder shape. |
| Current full GoDaddy upload folder | `C:\Users\KentLefner\Downloads\holesy-godaddy-upload-master-16.29\holesy\` | Full convenience copy for clean resync or rollback; not itself the source of truth. |
| Current GoDaddy delta upload folder | `C:\Users\KentLefner\Downloads\holesy-godaddy-delta-master-16.29-from-16.28\holesy\` | Default manual upload package when the live site already has the previous approved master. |
| Live site | `https://ptbooksinc.com/holesy/` | Must be verified separately after upload; do not infer live state from local package state. |
| Accepted architecture target | modular browser client split | The committed target is not a single-file game. See `ARCHITECTURE_DECISION_MODULAR_CLIENT_SPLIT.md`. |
| Current architecture alignment review | `00_ADMIN/Reviews_and_Reports/ARCHITECTURE_ALIGNMENT_REVIEW_MASTER16_16_MODULAR_PACKAGE.md` | Current gap analysis and required migration plan for `PERF-012`. |

## Non-Negotiable Distinctions

- A bundled `index.html` was a temporary publish artifact before `Master 16.17`; it is not the accepted architecture target. `Master 16.29` is the current modular patch on that architecture line.
- The production package must preserve the modular folder shape unless an emergency temporary bundled hotfix is explicitly approved and documented.
- The default manual GoDaddy package should include only files changed since the previous approved master, while preserving the modular folder paths.
- The modular browser-client split is a committed architecture constraint, not an optional future preference.
- A local file existing is not the same as being branch-visible on GitHub.
- A branch being current is not the same as `main` being current.
- A release package being generated is not the same as the live GoDaddy site being updated.
- A completed architecture decision gate is not the same as completed modular implementation.

## Required Pre-Action Gate

Before any future Codex chat packages, publishes, or begins a meaningful implementation slice, it must state:

1. current repo root
2. current branch and whether it matches its upstream
3. latest local commit
4. whether `main` is current or stale
5. approved source master and in-game build label
6. release package path and whether it matches the source master
7. live-site verification status if the user is discussing production
8. relevant architecture decisions and the current modular alignment review
9. relevant open or monitor issue-log items
10. whether the actual Daily QA Audit automation still matches governed prompt intent
11. whether any required process/procedure governance documents are missing
12. whether the requested action conflicts with any of the above

If a conflict exists, stop and surface it before creating files, packaging, or coding.

Use `00_ADMIN/Tools/holesy_team_sync.ps1` as the preferred implementation of this gate. For production or live-site questions, run it with `-VerifyLive`.
