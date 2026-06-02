## Holesy Handoff Briefing For The Next Codex Chat

This document is the takeover package for any new Codex chat that needs to continue Holesy development without relying on the full prior thread.

## 1. Active Handoff Status

- this is the active handoff document because the approved local basis is now `Master 16`
- treat prior handoff files, including `NEXT_CODEX_CHAT_HANDOFF_MASTER15.md`, as historical context unless a specific earlier decision needs review
- other developer chats inside the same project are also primary-source background and decision history for takeover work; review them when available instead of relying only on the current thread summary

## 2. Current Approved Baseline

Current promoted master:

- `C:\Users\KentLefner\OneDrive - Sandcastle Change\Desktop\game-repo\Holesy\10_SOURCE\Masters\Master 16\`
- playable entry point: `C:\Users\KentLefner\OneDrive - Sandcastle Change\Desktop\game-repo\Holesy\10_SOURCE\Masters\Master 16\index.html`
- current in-game label: `Master 16.48`

Current basis note:

- `C:\Users\KentLefner\OneDrive - Sandcastle Change\Desktop\game-repo\Holesy\10_SOURCE\Current\CURRENT_BASIS.md`

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
- `Master 16.23` opens the separate How to Play field manual through the popup-window pattern while preserving the modular package shape
- `Master 16.24` moves the archive/achievement spider map out of the player-facing game package and into `00_ADMIN/Requirements/ARCHIVE_ACHIEVEMENT_SPIDER_MAP.md`
- `Master 16.25` repairs the How to Play upload package by restoring matching menu-button styling and including the missing summary image dependency in the GoDaddy delta package
- `Master 16.26` restores the full-height How to Play summary graphic and adds a hard panic-car crash-slide movement cap
- `Master 16.27` makes menu action buttons touch-safe on mobile and changes soldier damage so normal object devours visibly grow the hole after being shot
- `Master 16.28` re-aligns active moving traffic to lane direction and removes hidden soldier-damage growth debt
- `Master 16.29` implements the Priority 1A medium-office-building voxel collapse slice: office buildings are aligned cube stacks and columns fall independently when the hole passes underneath
- `Master 16.30` refines that voxel pattern with 25% larger office cubes, procedural 5-to-10 cube dimensions on all three axes, and falling-object drift that preserves the visible-hole entry point instead of pulling pieces into a tiny center drain
- `Master 16.31` adds support-gated upper-cube release, 3D cube separation, and oversized-object jam/eject behavior for future larger cube content
- `Master 16.32` slows medium-office voxel gravity, adds column teeter/lean before release, and makes faster falling cubes transfer more momentum into rolls, spin, bounces, and nearby-object impacts
- `Master 16.33` rate-limits medium-office voxel consume audio and keeps cubes that miss the hole visible as settled debris instead of removing them below ground
- `Master 16.34` slows medium-office voxel descent/swallowing further, strengthens cube-to-cube contact response, forces teetering columns into side-fall, and prevents floor-entered cubes from reappearing on mobile after the hole moves away
- `Master 16.35` reduces the first major medium-office voxel performance regression by using larger/fewer cubes, capped per-building voxel contact checks, and velocity/spin clamps for more natural cube motion
- `Master 16.36` keeps medium-office cubes full-sized during hole-entry falls and replaces per-cube building-collapse audio with short sparse voxel impact sounds
- `Master 16.37` compacts Endless save object records, applies sparse short audio budgeting to skyscraper collapse/chunk sounds, and speeds medium-office cube descent while adding more sideways debris spread
- `Master 16.38` completes the next `PERF-012` Phase 2 extraction by moving build metadata/patch notes, difficulty profiles, and Archive lore documents into separate ES modules
- `Master 16.39` repairs the `Master 16.38` startup blocker by correcting the extraction boundary so game setup remains in `js/main.js` and menu controls work again
- `Master 16.40` adds same-side hole descent paths, `Master 16.41` refines them into fixed-point vertical descent, `Master 16.42` keeps swallowed objects visibly rendered inside the well with a subtle screen-down descent drift until they fall deeper out of sight, `Master 16.43` hides those fixed-column descents whenever the live hole no longer covers the descent column, `Master 16.44` settles active medium-office cubes that miss the live mouth before they become formally swallowed, `Master 16.45` switches the active/falling visibility test to the projected screen-space mouth so pieces cannot visibly descend outside the black disk, `Master 16.46` attempted recessed-well geometry but was rejected as not reading 3D, `Master 16.47` attempted a darker abyss texture but was still rejected as too 2D, and `Master 16.48` replaces it with a deeper tapering shaft whose bottom is physically lower and smaller than the mouth

Last approved candidate promoted into `Master 16`:

- `C:\Users\KentLefner\Desktop\game-repo\Holesy\20_TESTS\Candidate_Builds\Master 15.45 - in-game-build-notes.html`

## 3. Current Working Rules

- Canonical repo root: `C:\Users\KentLefner\OneDrive - Sandcastle Change\Desktop\game-repo\Holesy`
- Always verify the repo root with `git rev-parse --show-toplevel`; in this session the non-OneDrive Desktop path resolved to a different folder and was not the governed source checkout.
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

- immediate focus is regression testing `Master 16.48` through the modular package, then continuing the next `PERF-012` extraction slice after user validation
- newly implemented Priority 1 gameplay/physics item pending user validation: medium office buildings are procedurally generated cube/floor stacks whose individual columns can teeter/lean before dropping; upper cubes wait for support failure, active cubes separate in 3D, oversized objects can jam in the visible hole, and faster falling cubes carry more impact/roll/spin energy
- active monitor items carried from the latest QA evidence:
  - archive readability and music fit
  - starter buff-pattern guidance, end-screen flow, feedback-noise cleanup, skyscraper-warning timing, and the build-notes modal
- current governance blockers:
  - `QA-017` reopened on 2026-06-02 because the daily audit can read governed state but still cannot refresh `.git/FETCH_HEAD`, so the current governance package is local-only until a Git-writable governed session republishes it
  - `QA-020` opened on 2026-06-02 because governed daily-audit evidence was missing for `2026-05-30` through `2026-06-01`; the new missing-run note exists locally but is not yet branch-visible
- recently closed by user validation and governance repair:
  - `QA-006` closed after user confirmed the long-idle tab-close defect no longer reproduces
  - `QA-007` closed for the current defect state; ongoing buff/lore wording clarity remains a product-backlog improvement item
  - `QA-016` and the `Master 16.28` traffic/soldier-growth regression set closed after user validation
  - `QA-017` resolved after `git fetch --prune`, staging, commit, and push succeeded from the governed repo session
  - `QA-018` resolved by documenting the unrecoverable 2026-05-23 through 2026-05-27 missing-run gap and adding a missed-run backstop to the actual automation prompt plus governed QA procedures
- current gameplay validation should also cover Endless Waves continuity, player-death exit behavior, rival respawn behavior, save/load expectations, and runaway growth tuning
- current gameplay validation should specifically re-check saved skyscraper visuals after loading, growth after soldier damage, and panic-car skid distance
- governance recovery added on 2026-05-20: product-intent continuity and modular architecture awareness are now monitor-controlled under `QA-011`
- broader lore expansion should wait until the current `Master 16` UX validates
- remaining Priority 1 wave-system performance work resumes after the modular production package path is under control and `Master 16` proves stable
- do not collapse the medium-office-building voxel item into generic collapse polish; it is the reusable stack/voxel pattern for future procedural stacked structures and theme objects

## 5. Immediate Next Move

- treat `Master 16.48` as the current source basis within `10_SOURCE/Masters/Master 16/`
- treat `index.html` as the package entry point only; the full modular `/holesy/` folder remains the governed release baseline
- for routine manual GoDaddy uploads, provide a changed-files-only delta package by default when live is already on the previous approved master
- first publish the local 2026-06-02 governance package from a governed session that can write Git metadata:
  - `00_ADMIN/Reviews_and_Reports/QA_REVIEW_DAILY_AUDIT_2026-05-30_TO_2026-06-01_MISSING_RUN_NOTE.md`
  - `00_ADMIN/Reviews_and_Reports/QA_REVIEW_DAILY_AUDIT_2026-06-02.md`
  - `00_ADMIN/Reviews_and_Reports/ISSUE_LOG.md`
  - `00_ADMIN/Policies_and_Procedures/NEXT_CODEX_CHAT_HANDOFF_MASTER16.md`
  - `00_ADMIN/Tools/holesy_team_sync.ps1`
- after publication proof, return to regression testing `Master 16.48`, then continue the next `PERF-012` extraction slice
- validate the promoted website package and local master against the current recommendation:
  - `C:\Users\KentLefner\OneDrive - Sandcastle Change\Desktop\game-repo\Holesy\10_SOURCE\Masters\Master 16\index.html`
  - `C:\Users\KentLefner\OneDrive - Sandcastle Change\Desktop\game-repo\Holesy\40_RELEASE\Website_Publish_Package\holesy\index.html`
  - `C:\Users\KentLefner\OneDrive - Sandcastle Change\Desktop\game-repo\Holesy\40_RELEASE\Website_Publish_Package\holesy\css\styles.css`
  - `C:\Users\KentLefner\OneDrive - Sandcastle Change\Desktop\game-repo\Holesy\40_RELEASE\Website_Publish_Package\holesy\js\main.js`
- validation focus:
  - keep future daily audits checking for missing dated reports before completion
  - confirm Archive readability and that the Archive music still fits the intended funny, whimsical, conspiracy-undertone direction
  - confirm on a real mobile device that Stats and How to Play respond to taps through the intended popup/action path
  - confirm rare document pacing, buff-effect clarity, combo readability, starter Field Pattern usefulness, end-screen flow, feedback-noise cleanup, skyscraper-warning timing, and the build-notes modal in real gameplay
  - confirm in real gameplay that ordinary object devours visibly grow the hole after soldier damage in the same wave
  - confirm medium office buildings use larger readable cubes, vary from 4 to 7 cubes per footprint axis and 4 to 8 floors for performance, drop only the touched/under-hole cube columns, keep cubes full-sized while falling into the visible hole, do not visibly continue falling outside the projected black hole mouth after the hole moves away, avoid long/static cube audio, upper cubes wait for support failure, falling cubes collide/separate without tanking frame rate, oversized cubes can jam and eject, and skyscraper collapse behavior remains distinct
- only after those validations:
  - expand the lore data module with the remaining approved Rival / Response corpus
  - resume the remaining Priority 1 performance backlog
  - then return to Waves tuning, physics stack work, and broader powerup/mode expansion
