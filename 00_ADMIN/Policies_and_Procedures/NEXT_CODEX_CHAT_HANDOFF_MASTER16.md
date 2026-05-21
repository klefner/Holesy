## Holesy Handoff Briefing For The Next Codex Chat

This document is the takeover package for any new Codex chat that needs to continue Holesy development without relying on the full prior thread.

## 1. Active Handoff Status

- this is the active handoff document because the approved local basis is now `Master 16`
- treat prior handoff files, including `NEXT_CODEX_CHAT_HANDOFF_MASTER15.md`, as historical context unless a specific earlier decision needs review
- other developer chats inside the same project are also primary-source background and decision history for takeover work; review them when available instead of relying only on the current thread summary

## 2. Current Approved Baseline

Current promoted master:

- `C:\Users\KentLefner\Desktop\game-repo\Holesy\10_SOURCE\Masters\Master 16\`
- playable entry point: `C:\Users\KentLefner\Desktop\game-repo\Holesy\10_SOURCE\Masters\Master 16\index.html`
- current in-game label: `Master 16.22`

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
- `Master 16.16` adds saved-skyscraper visual restoration after loading Endless saves, post-soldier growth recovery, tighter car skid tuning, and always-visible build version in the Pause menu
- `Master 16.18` completes `PERF-012` Phase 1 by moving the governed source and publish package to the modular browser-client folder shape, with `css/styles.css` and `js/main.js` externalized from the former single-file build
- `Master 16.22` opens the separate How to Play field manual through the popup-window pattern while preserving the modular package shape

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
- Before release packaging or material implementation, run:
  - `00_ADMIN/Policies_and_Procedures/RELEASE_SOURCE_OF_TRUTH_MANIFEST.md`
  - `00_ADMIN/Policies_and_Procedures/PRODUCT_INTENT_GATE.md`
- Do not treat `index.html` as the whole game package; the accepted target remains modular browser assets under `ARCHITECTURE_DECISION_MODULAR_CLIENT_SPLIT.md`
- The modular architecture target is a committed constraint, not an optional preference. `Master 16.18` is the first production-scope modular package alignment.
- Before answering any production/upload/package question, read `ARCHITECTURE_ALIGNMENT_REVIEW_MASTER16_16_MODULAR_PACKAGE.md` and state whether the requested upload aligns with the modular target.

## 4. Current Priority

- immediate focus is regression testing `Master 16.22` through the modular package, then continuing `PERF-012` Phase 2 data/config extraction
- active monitor items carried from the latest QA evidence:
  - long-idle tab-close behavior descended from `Master 15.39`
  - archive readability and music fit
  - rare document pacing and buff-effect clarity descended from `Master 15.42`
  - starter buff-pattern guidance, end-screen flow, feedback-noise cleanup, skyscraper-warning timing, and the build-notes modal
- current gameplay validation should also cover Endless Waves continuity, player-death exit behavior, rival respawn behavior, save/load expectations, and runaway growth tuning
- current gameplay validation should specifically re-check saved skyscraper visuals after loading, growth after soldier damage, and panic-car skid distance
- governance recovery added on 2026-05-20: product-intent continuity and modular architecture awareness are now monitor-controlled under `QA-011`
- broader lore expansion should wait until the current `Master 16` UX validates
- remaining Priority 1 wave-system performance work resumes after the modular production package path is under control and `Master 16` proves stable

## 5. Immediate Next Move

- treat `Master 16.22` as the current source basis within `10_SOURCE/Masters/Master 16/`
- treat `index.html` as the package entry point only; production upload requires the full modular `/holesy/` folder contents
- start with regression testing `Master 16.22`, then continue `PERF-012` Phase 2
- validate the promoted website package and local master against the current recommendation:
  - `C:\Users\KentLefner\Desktop\game-repo\Holesy\10_SOURCE\Masters\Master 16\index.html`
  - `C:\Users\KentLefner\Desktop\game-repo\Holesy\40_RELEASE\Website_Publish_Package\holesy\index.html`
  - `C:\Users\KentLefner\Desktop\game-repo\Holesy\40_RELEASE\Website_Publish_Package\holesy\css\styles.css`
  - `C:\Users\KentLefner\Desktop\game-repo\Holesy\40_RELEASE\Website_Publish_Package\holesy\js\main.js`
- validation focus:
  - confirm the original long-idle Chrome tab-close stall is materially improved after a real idle wait
  - confirm Archive readability and that the Archive music still fits the intended funny, whimsical, conspiracy-undertone direction
  - confirm rare document pacing, buff-effect clarity, combo readability, starter Field Pattern usefulness, end-screen flow, feedback-noise cleanup, skyscraper-warning timing, and the build-notes modal in real gameplay
- only after those validations:
  - expand the lore data module with the remaining approved Rival / Response corpus
  - resume the remaining Priority 1 performance backlog
  - then return to Waves tuning, physics stack work, and broader powerup/mode expansion

