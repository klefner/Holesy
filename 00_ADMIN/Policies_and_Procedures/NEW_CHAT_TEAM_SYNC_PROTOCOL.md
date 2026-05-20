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
.\00_ADMIN\Tools\holesy_team_sync.ps1
```

from:

```text
C:\Users\KentLefner\Desktop\game-repo\Holesy
```

## Required Assistant Behavior

Before making any recommendation or changing any file, the assistant must report:

1. repo root
2. active branch and upstream state
3. latest commits
4. whether `main` is current or stale
5. approved source master and in-game build label
6. release package state
7. accepted architecture target
8. open and monitor issue-log items
9. current backlog recommendation
10. Product Intent Gate result for the user's requested action

If any of those cannot be verified, the assistant must say so and stop before implementation.

## Required Files To Inspect

- `00_ADMIN/Policies_and_Procedures/RELEASE_SOURCE_OF_TRUTH_MANIFEST.md`
- `00_ADMIN/Policies_and_Procedures/PRODUCT_INTENT_GATE.md`
- `00_ADMIN/Requirements/ARCHITECTURE_DECISION_MODULAR_CLIENT_SPLIT.md`
- `00_ADMIN/Requirements/PRODUCT_BACKLOG.md`
- `00_ADMIN/Reviews_and_Reports/ISSUE_LOG.md`
- newest `00_ADMIN/Policies_and_Procedures/NEXT_CODEX_CHAT_HANDOFF_MASTER*.md`
- `10_SOURCE/Current/CURRENT_BASIS.md`

## Snapshot Artifact

When an audit or handoff needs a persisted artifact, run with `-WriteSnapshot`. The script then writes:

```text
00_ADMIN/Reviews_and_Reports/NEW_CHAT_CONTEXT_SNAPSHOT_CURRENT.md
```

That file is intentionally overwritten by each persisted startup sync. It is not a replacement for the governed source docs; it is a quick context packet that points back to them.

Do not use `-WriteSnapshot` casually in every chat unless the resulting file will be intentionally reviewed, staged, or removed. The normal new-chat startup should use stdout only to avoid accidental dirty-tree churn.

## Non-Negotiable Rule

No future chat may answer packaging, release, architecture, or backlog questions from memory alone. It must run this protocol or explicitly say it has not done so.
