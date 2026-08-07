# Holesy Release Source Of Truth Manifest

## Purpose

This manifest prevents future chats from collapsing local files, branch state, release packages, live-site state, and architecture direction into one misleading answer.

Before any release packaging, live upload, master promotion, backlog sequencing, or major feature implementation, the acting developer must check this manifest and report the relevant states separately.

The acting developer must also read the repo-root `AGENTS.md` so release, implementation, QA, and handoff responsibilities are applied consistently across Codex, Claude, and other AI systems.

## Current State As Of 2026-08-07

| Layer | Current value | Meaning |
| --- | --- | --- |
| Canonical repo | `C:\Users\KentLefner\OneDrive - Sandcastle Change\Desktop\game-repo\Holesy` | Primary governed working checkout for current Holesy work in the current Windows shell. Verify with `git rev-parse --show-toplevel`; the non-OneDrive Desktop path can appear as a different folder in some sessions. |
| Active branch | `codex/publish-master4-structure` | Branch carrying the current governed history and recent Master 16.x work. |
| Stable `main` | stale relative to active branch | `main` is not the current gameplay source of truth until an explicit PR/merge updates it. |
| Current approved source master | `10_SOURCE/Masters/Master 16/` | Governed modular source package for the current playable build line. |
| Current source entry point | `10_SOURCE/Masters/Master 16/index.html` | Browser entry point for the modular source package. |
| Current in-game build label | `Master 16.208` | Current governed review build; normal player surfaces display `Version 16.208`. It preserves the complete Master 16.207 baseline while ending all gameplay audio before the isolated Pause cue begins and rebuilding only appropriate continuous audio on Resume. |
| Current release package | `40_RELEASE/Website_Publish_Package/holesy/` | Current modular browser-client release package and source for the GitHub Pages `docs/` deployment tree. |
| Current Pages deployment branch | `claude/happy-clarke-ORWAI` | GitHub Pages publishes from this branch's `docs/` folder; commit `fecc865` publishes the complete `Master 16.207` package. Pages workflow `31199780768` completed successfully, and live browser verification on 2026-08-07 confirmed public `Version 16.207`, the arrow glyph, north-marker element, and a clean console. |
| Legacy GoDaddy packages | obsolete | GoDaddy publishing was discontinued when GitHub Pages became the production host; retained historical folders must not drive release decisions. |
| Live site | `https://klefner.github.io/Holesy/` | GitHub Pages production URL. Verify this URL separately after every deployment; do not infer live state from local package state. |
| Accepted architecture target | modular browser client split | The committed target is not a single-file game. See `ARCHITECTURE_DECISION_MODULAR_CLIENT_SPLIT.md`. |
| Current architecture alignment review | `00_ADMIN/Reviews_and_Reports/ARCHITECTURE_ALIGNMENT_REVIEW_MASTER16_16_MODULAR_PACKAGE.md` | Current PERF-012 closure evidence and future modularization guidance. |

## Non-Negotiable Distinctions

- A bundled `index.html` was a temporary publish artifact before `Master 16.17`; it is not the accepted architecture target. `Master 16.208` is the current modular review patch on that architecture line.
- `Master 16.74` closes `PERF-012` as a production package migration item; future UI, save/load, level/theme, reward/quest, and gameplay-system extraction should be tracked as separate feature-support architecture work.
- The production package must preserve the modular folder shape unless an emergency temporary bundled hotfix is explicitly approved and documented.
- GitHub Pages deployment must preserve the full modular package shape under the configured `docs/` source while commits remain narrowly scoped to changed files.
- The modular browser-client split is a committed architecture constraint, not an optional future preference.
- A local file existing is not the same as being branch-visible on GitHub.
- A branch being current is not the same as `main` being current.
- A release package or Pages commit being generated is not the same as the public GitHub Pages deployment completing.
- A completed architecture decision gate is not the same as completed modular implementation.

## Required Pre-Action Gate

Before any future Codex chat packages, publishes, or begins a meaningful implementation slice, it must state:

1. whether `AGENTS.md` was read and which agent role or roles apply
2. current repo root
3. current branch and whether it matches its upstream
4. latest local commit
5. whether `main` is current or stale
6. approved source master and in-game build label
7. release package path and whether it matches the source master
8. live-site verification status if the user is discussing production
9. relevant architecture decisions and the current modular alignment review
10. relevant open or monitor issue-log items
11. whether the actual Biweekly QA Audit automation still matches governed prompt intent
12. whether any required process/procedure governance documents are missing
13. whether the requested action conflicts with any of the above

If a conflict exists, stop and surface it before creating files, packaging, or coding.

Use `00_ADMIN/Tools/holesy_team_sync.ps1` as the preferred implementation of this gate. For production or live-site questions, run it with `-VerifyLive`.
