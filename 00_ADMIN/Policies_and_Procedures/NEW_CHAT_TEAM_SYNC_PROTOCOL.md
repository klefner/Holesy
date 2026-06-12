# New Chat Team Sync Protocol

## Purpose

Holesy must behave like one development team across many Codex chats. A new chat is not a new team, a new memory, or permission to rediscover prior decisions.

This protocol gives every new chat a mandatory startup routine that loads the governed product, architecture, release, and issue context before recommendations, packaging, or implementation.

The portable agent role contract lives at the repo root in `AGENTS.md`. Any AI system that cannot run this PowerShell startup routine must still read `AGENTS.md` and the governed documents below, then state which local checks it could not perform.

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

- `AGENTS.md`
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

No future AI system may substitute model memory for `AGENTS.md`. The file is the cross-tool agent contract and must be treated as part of the governed startup corpus.

The modular browser-client split is a committed architecture constraint. No future chat may describe `index.html` as the whole game package. `Master 16.75` is the current modular source baseline, with `Master 16.18` completing `PERF-012` Phase 1, `Master 16.24` keeping the archive/achievement spider map as a project artifact rather than shipped game UI, `Master 16.25` repairing the How to Play upload package by including the missing summary image dependency in the GoDaddy delta, `Master 16.26` restoring the full-height How to Play summary graphic while hard-capping panic-car crash slides, `Master 16.27` repairing mobile menu action taps plus post-soldier visible growth, `Master 16.28` re-aligning moving traffic to lane direction while removing hidden soldier-damage growth debt, `Master 16.29` implementing the first medium-office voxel-collapse slice, `Master 16.30` enlarging those office cubes and preserving the visible-hole entry point during falling-piece drift, `Master 16.34` repairing voxel reappearance after floor-entry consumption, `Master 16.35` reducing medium-office voxel collapse performance cost with larger/fewer cubes plus capped contact checks, `Master 16.36` keeping voxel cubes full-sized during hole-entry falls while replacing per-cube building-collapse audio with short sparse voxel impact sounds, `Master 16.37` compacting Endless saves, rate-limiting skyscraper collapse audio, and speeding medium-office cube descent with more sideways debris spread, `Master 16.38` extracting build metadata, patch notes, difficulty profiles, and Archive lore data into separate ES modules, `Master 16.39` repairing the 16.38 startup blocker by keeping renderer/game setup in `js/main.js`, `Master 16.40` adding same-side hole descent paths, `Master 16.41` refining them into fixed-point vertical descent, `Master 16.55` preserving Endless live scores through world shifts while respawning soldier-killed rivals and snapping settled medium cubes flat, `Master 16.56` adding the government-building separate-physics prototype, `Master 16.57` improving government-building spy and tuxedo visibility, `Master 16.58` fixing the government-building touch crash, `Master 16.59` tuning government-building column-shock collapse, `Master 16.60` adding morning, mid day, evening, and night scene lighting, `Master 16.61` adding in-game time/weather preview controls plus Clear, Rain, Snow, and Ash weather effects, `Master 16.62` removing Ash while preserving richer Rain/Snow weather visuals, `Master 16.63` improving government-building shake/topple/impulse collision behavior, `Master 16.64` removing the Weather button and weather particle frame-update path after severe performance slowdown while keeping Time cycling, `Master 16.65` giving government debris a short escape window plus stronger shake/topple/blast/contact impulses, `Master 16.66` turning on street lamp glow, sparse lit building windows, and car lights in evening/night, `Master 16.67` extinguishing destroyed building windows so Time cycling cannot relight them, `Master 16.68` locking wave-based play to morning, mid day, evening, night, then repeating after night, `Master 16.69` restoring manual Time button cycling, `Master 16.70` making devoured streetlamps flicker off as they fall, `Master 16.71` refreshing local test cache-busting, `Master 16.72` making building windows flicker off as buildings come apart, `Master 16.73` shortening that power-loss effect to one to three random flickers, `Master 16.74` closing `PERF-012` with a governed package manifest plus refreshed release guidance, and `Master 16.75` removing the artificial inward spread limiter from non-voxel skyscraper debris while preserving medium-office voxel behavior. Release/package answers must identify the full modular package baseline and provide a changed-files-only GoDaddy delta package by default when the live site is already on the prior master; future modular extraction should use new named architecture backlog items rather than reopening `PERF-012`.

## Completeness Boundary

This protocol is considered complete for current startup awareness only at the moment it runs. It cannot prove future changes made by another chat after the sync. When production state matters, the assistant must rerun the sync with `-VerifyLive` rather than inferring live state from local files.
