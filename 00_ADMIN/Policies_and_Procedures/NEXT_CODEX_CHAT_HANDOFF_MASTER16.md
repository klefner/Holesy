## Holesy Handoff Briefing For The Next Codex Chat

This document is the takeover package for any new Codex chat that needs to continue Holesy development without relying on the full prior thread.

## 1. Active Handoff Status

- this is the active handoff document because the approved local basis is now `Master 16`
- treat prior handoff files, including `NEXT_CODEX_CHAT_HANDOFF_MASTER15.md`, as historical context unless a specific earlier decision needs review
- other developer chats inside the same project are also primary-source background and decision history for takeover work; review them when available instead of relying only on the current thread summary

## 2. Current Approved Baseline

Current promoted master:

- `C:\Users\KentLefner\Desktop\game-repo\Holesy\10_SOURCE\Masters\Master 16.html`

Current basis note:

- `C:\Users\KentLefner\Desktop\game-repo\Holesy\10_SOURCE\Current\CURRENT_BASIS.md`

What `Master 16` specifically represents:

- it includes the full approved `Master 15` gameplay baseline
- it carries forward the idle lifecycle cleanup from `Master 15.39`
- it includes the playable Archive / found-document / lore-achievement-buff system from `Master 15.40`
- it includes Archive-specific music from `Master 15.41`
- it includes clearer buff messaging and rare, capped document drops from `Master 15.42`
- it includes always-visible starter Field Pattern guidance from `Master 15.43`
- it includes end-screen, feedback-noise, skyscraper-warning, and aid-drop color cleanup from `Master 15.44`
- it includes clickable in-game build notes from `Master 15.45`
- it has since advanced through the local `Master 16.x` line with Endless Waves, Endless save/load, rival respawn behavior, buff cooldown tuning, rival-devour growth tempering, and player-death stop fixes

Last approved candidate promoted into `Master 16`:

- `C:\Users\KentLefner\Desktop\game-repo\Holesy\20_TESTS\Candidate_Builds\Master 15.45 - in-game-build-notes.html`

## 3. Current Working Rules

- Canonical repo root: `C:\Users\KentLefner\Desktop\game-repo\Holesy`
- Active branch: `codex/publish-master4-structure`
- Preserve rollback safety
- Do not work directly on `main`
- Distinguish local file state, git state, GitHub branch state, `main` state, and live website state
- Keep unrelated local modifications out of commits
- Every developer chat other than the current one is primary-source context when available

## 4. Current Priority

- immediate focus is validation of the promoted `Master 16` website package and the active `Master 16.x` gameplay fixes on real devices and real gameplay
- active monitor items carried from the latest QA evidence:
  - long-idle tab-close behavior descended from `Master 15.39`
  - archive readability and music fit
  - rare document pacing and buff-effect clarity descended from `Master 15.42`
  - starter buff-pattern guidance, end-screen flow, feedback-noise cleanup, skyscraper-warning timing, and the build-notes modal
- current gameplay validation should also cover Endless Waves continuity, player-death exit behavior, rival respawn behavior, save/load expectations, and runaway growth tuning
- broader lore expansion should wait until the current `Master 16` UX validates
- remaining Priority 1 wave-system performance work resumes after `Master 16` proves stable

## 5. Immediate Next Move

- treat `Master 16` as the current source basis
- validate the promoted website package and local master against the current recommendation:
  - `C:\Users\KentLefner\Desktop\game-repo\Holesy\10_SOURCE\Masters\Master 16.html`
  - `C:\Users\KentLefner\Desktop\game-repo\Holesy\40_RELEASE\Website_Publish_Package\holesy\index.html`
- validation focus:
  - confirm the original long-idle Chrome tab-close stall is materially improved after a real idle wait
  - confirm Archive readability and that the Archive music still fits the intended funny, whimsical, conspiracy-undertone direction
  - confirm rare document pacing, buff-effect clarity, combo readability, starter Field Pattern usefulness, end-screen flow, feedback-noise cleanup, skyscraper-warning timing, and the build-notes modal in real gameplay
- only after those validations:
  - expand the lore data module with the remaining approved Rival / Response corpus
  - resume the remaining Priority 1 performance backlog
  - then return to Waves tuning, physics stack work, and broader powerup/mode expansion
