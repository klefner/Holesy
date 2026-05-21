# New Chat Team Sync Protocol

## Purpose

Holesy must behave like one development team across many Codex chats. A new chat is not a new team, a new memory, or permission to rediscover prior decisions.

This protocol gives every new chat a mandatory startup routine that loads the governed product, architecture, release, and issue context before recommendations, packaging, or implementation.

## Trigger Phrase

Use this exact phrase at the start of a new Codex chat:

```text
Holesy Team Sync. Run the new-chat startup protocol from the governed repo before doing anything else.
```

The assistant must then run:

```powershell
.\00_ADMIN\Tools\holesy_team_sync.ps1 -RequestText "<paste the user's opening request here>"
```

from:

```text
C:\Users\KentLefner\Desktop\game-repo\Holesy
```

## Required Assistant Behavior

Before making any recommendation or changing any file, the assistant must report:

1. repo root
2. active branch, upstream, and ahead/behind state after `git fetch --prune`
3. latest commits
4. whether `main` is current, stale, or divergent
5. approved source master and in-game build label
6. release package state, including hash/build-label comparison against the source master
7. GoDaddy upload-copy state, if the manifest names one
8. accepted architecture target and current modular alignment review
9. actual Daily QA Audit automation drift check against the governed automation prompt intent
10. governance corpus inventory, including missing expected process/procedure docs
11. open and monitor issue-log items, including counts
12. current backlog recommendation
13. started/not-complete backlog signals and Priority 1 item list
14. Product Intent Gate result for the user's requested action
15. confidence footer that says whether the result is fully verified, verified with stated limitations, or blocked by missing evidence

If any of those cannot be verified, the assistant must say so and stop before implementation.

## Optional Verification Flags

Use these flags when the user's request makes the extra evidence relevant:

```powershell
.\00_ADMIN\Tools\holesy_team_sync.ps1 -RequestText "<request>" -VerifyLive
```

- `-VerifyLive` checks the public Holesy URL and reports the live build label and content hash. Use it for production, GoDaddy, upload, live-site, or release-verification questions.
- `-WriteSnapshot` writes `00_ADMIN/Reviews_and_Reports/NEW_CHAT_CONTEXT_SNAPSHOT_CURRENT.md`. Use it for audit/handoff evidence, not as routine chat startup.
- `-SkipFetch` is allowed only when network/Git remote checks are blocked; if used, the assistant must report that remote freshness was not verified.

## Required Files To Inspect

- `00_ADMIN/Policies_and_Procedures/RELEASE_SOURCE_OF_TRUTH_MANIFEST.md`
- `00_ADMIN/Policies_and_Procedures/PRODUCT_INTENT_GATE.md`
- `00_ADMIN/Policies_and_Procedures/AUDITOR_AUTOMATION_PROMPT.md`
- `00_ADMIN/Requirements/ARCHITECTURE_DECISION_MODULAR_CLIENT_SPLIT.md`
- `00_ADMIN/Reviews_and_Reports/ARCHITECTURE_ALIGNMENT_REVIEW_MASTER16_16_MODULAR_PACKAGE.md`
- `00_ADMIN/Requirements/PRODUCT_BACKLOG.md`
- `00_ADMIN/Reviews_and_Reports/ISSUE_LOG.md`
- newest `00_ADMIN/Policies_and_Procedures/NEXT_CODEX_CHAT_HANDOFF_MASTER*.md`
- `10_SOURCE/Current/CURRENT_BASIS.md`
- actual automation file at `C:\Users\KentLefner\.codex\automations\daily-qa-audit\automation.toml`, when available

## Snapshot Artifact

When an audit or handoff needs a persisted artifact, run with `-WriteSnapshot`. The script then writes:

```text
00_ADMIN/Reviews_and_Reports/NEW_CHAT_CONTEXT_SNAPSHOT_CURRENT.md
```

That file is intentionally overwritten by each persisted startup sync. It is not a replacement for the governed source docs; it is a quick context packet that points back to them.

Do not use `-WriteSnapshot` casually in every chat unless the resulting file will be intentionally reviewed, staged, or removed. The normal new-chat startup should use stdout only to avoid accidental dirty-tree churn.

## Non-Negotiable Rule

No future chat may answer packaging, release, architecture, or backlog questions from memory alone. It must run this protocol or explicitly say it has not done so.

The modular browser-client split is a committed architecture constraint. No future chat may describe `index.html` as the whole game package. `Master 16.21` is the current modular source baseline, with `Master 16.18` completing `PERF-012` Phase 1 and `Master 16.21` opening the separate How to Play field manual through the popup-window pattern. Release/package answers must identify the full modular package contents and route remaining architecture work to the next `PERF-012` phase.

## Completeness Boundary

This protocol is considered complete for current startup awareness only at the moment it runs. It cannot prove future changes made by another chat after the sync. When production state matters, the assistant must rerun the sync with `-VerifyLive` rather than inferring live state from local files.

