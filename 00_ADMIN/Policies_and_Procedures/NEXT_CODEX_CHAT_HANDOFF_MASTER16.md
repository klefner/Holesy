## Holesy Handoff Briefing For The Next Codex Chat

This document is the takeover package for any new Codex chat that needs to continue Holesy development without relying on the full prior thread.

For Codex, Claude, or any other AI system, the role contract starts at the repo-root `AGENTS.md`. Read it before using this handoff so the Team Sync, implementation, QA audit, release, and handoff roles are applied consistently.

## 1. Active Handoff Status

- this is the active handoff document because the approved local basis is now `Master 16`
- treat prior handoff files, including `NEXT_CODEX_CHAT_HANDOFF_MASTER15.md`, as historical context unless a specific earlier decision needs review
- other developer chats inside the same project are also primary-source background and decision history for takeover work; review them when available instead of relying only on the current thread summary

## 2. Current Approved Baseline

Current promoted master:

- `C:\Users\KentLefner\OneDrive - Sandcastle Change\Desktop\game-repo\Holesy\10_SOURCE\Masters\Master 16\`
- playable entry point: `C:\Users\KentLefner\OneDrive - Sandcastle Change\Desktop\game-repo\Holesy\10_SOURCE\Masters\Master 16\index.html`
- current in-game label: `Master 16.93`

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
- `Master 16.40` adds same-side hole descent paths, `Master 16.41` refines them into fixed-point vertical descent, `Master 16.42` keeps swallowed objects visibly rendered inside the well with a subtle screen-down descent drift until they fall deeper out of sight, `Master 16.43` hides those fixed-column descents whenever the live hole no longer covers the descent column, `Master 16.44` settles active medium-office cubes that miss the live mouth before they become formally swallowed, `Master 16.45` switches the active/falling visibility test to the projected screen-space mouth so pieces cannot visibly descend outside the black disk, `Master 16.46` attempted recessed-well geometry but was rejected as not reading 3D, `Master 16.47` attempted a darker abyss texture but was still rejected as too 2D, `Master 16.48` attempted deeper-shaft geometry but blocked visibility into the mouth, `Master 16.49` restores the original flat black circular mouth with the existing colored rim, `Master 16.50` adds a smaller skyscraper-like upward/outward impact kick to medium-office cube releases, `Master 16.51` strengthens medium-office first-impact and cube-to-cube pool-break reactions, `Master 16.52` adds local impact jarring plus solid settled-cube collision participation, `Master 16.53` adds desktop mouse-exit steering carry, `Master 16.54` keeps medium-office impact motion continuous through support-delay and release, `Master 16.55` preserves Endless live scores through world shifts while respawning soldier-killed rivals and snapping settled medium cubes flat, `Master 16.56` adds a net-new government building prototype with an isolated fixed-step collider/impulse physics world rather than the existing building-collapse rules, `Master 16.57` improves government-building spy/tuxedo visibility, `Master 16.58` fixes the government-building touch crash, `Master 16.59` tunes government-building column-shock collapse, `Master 16.60` adds the morning, mid day, evening, and night scene-lighting cycle, `Master 16.61` adds in-game Time and Weather buttons plus Clear, Rain, Snow, and Ash weather effects, `Master 16.62` removes Ash while preserving the richer Rain/Snow visuals, `Master 16.63` makes government-building pieces shake, lean, release, bounce, spin, and side-hop through the separate government physics path, `Master 16.64` removes the Weather button and weather particle frame-update path after severe performance slowdown while keeping the Time button, `Master 16.65` gives government debris a short escape window plus stronger shake/topple/blast/contact impulses, `Master 16.66` turns on street lamp glow, sparse lit building windows, and car lights in evening/night, `Master 16.67` extinguishes destroyed building windows so the Time button cannot relight them, `Master 16.68` locks wave-based play to the morning, mid day, evening, night sequence with repeat after night, `Master 16.69` restores manual Time button cycling, `Master 16.70` makes devoured streetlamps flicker off as they fall, `Master 16.71` refreshes local test cache-busting, `Master 16.72` makes lit building windows flicker off as they come apart, `Master 16.73` shortens that power-loss effect to one to three random flickers, `Master 16.74` closes `PERF-012` with package-manifest and release-guidance evidence, `Master 16.75` removes the artificial inward spread limiter from non-voxel skyscraper debris while preserving medium-office voxel behavior, `Master 16.77` applies government medium-style voxel collapse plus visible small house/shop breakup, `Master 16.78` adds an optional MegaKit Downtown test environment while keeping Classic Aldine as the default, `Master 16.79` repairs MegaKit Downtown by removing fake ground patches and non-breakable showcase buildings, `Master 16.80` restores safe readable MegaKit Downtown ground detail, `Master 16.81` repairs that detail so block edges and manholes read clearly from the gameplay camera, `Master 16.82` adds visible run goals plus local object-family mastery without permanent power growth, `Master 16.83` repairs Run Goals performance while expanding and rewarding the goals, `Master 16.84` hotfixes remaining mastery/bookkeeping performance risk, `Master 16.85` adds readable goal instructions, `Master 16.86` repairs startup responsiveness and gameplay frame pacing, `Master 16.87` completes the comprehensive runtime performance pass, `Master 16.88` adds the army boss escalation, `Master 16.89` fixes the Wave 1 to Wave 2 army-boss lock, `Master 16.90` converts the boss into a green tank, `Master 16.91` forces that tank into all modes for temporary visibility testing, `Master 16.92` randomizes wave-start corner assignments, and `Master 16.93` repairs skyscraper outward debris direction

Last approved candidate promoted into `Master 16`:

- `C:\Users\KentLefner\Desktop\game-repo\Holesy\20_TESTS\Candidate_Builds\Master 15.45 - in-game-build-notes.html`

## 3. Current Working Rules

- Canonical repo root: `C:\Users\KentLefner\OneDrive - Sandcastle Change\Desktop\game-repo\Holesy`
- Always verify the repo root with `git rev-parse --show-toplevel`; in this session the non-OneDrive Desktop path resolved to a different folder and was not the governed source checkout.
- Active branch: `codex/publish-master4-structure`
- Required agent contract: `AGENTS.md`
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

- immediate focus is returning to gameplay/product intake now that `PERF-012` is complete; user validation on 2026-06-11 passed the broader `Master 16.71` regression, day/time-of-day behavior, Time button cycling, lighting effects, streetlamp flicker-off behavior, government-building debris feel, medium-office voxel regression, and `Master 16.73` short building-window flicker
- newly implemented Priority 1 gameplay/physics item is user-validated through `Master 16.71`: medium office buildings are procedurally generated cube/floor stacks whose individual columns can teeter/lean before dropping; upper cubes wait for support failure, active cubes separate in 3D, impacted columns get varied first-impact vectors, stronger pool-break collision transfer, capped local scatter, oversized objects can jam in the visible hole, and faster falling cubes carry more impact/roll/spin energy
- active monitor items carried from the latest QA evidence:
  - archive readability and music fit
  - starter buff-pattern guidance, end-screen flow, feedback-noise cleanup, skyscraper-warning timing, and the build-notes modal
- current governance defects:
  - `QA-028` remains open: the 2026-06-24 daily audit repaired stale `Master 16.87` baseline references locally, but branch-visible publication is still pending for the dirty `NEW_CHAT_TEAM_SYNC_PROTOCOL.md` and `PRODUCT_BACKLOG.md` files because they already contain unrelated local edits
- recently closed by user validation and governance repair:
  - `QA-023` resolved on 2026-06-09 after the 2026-06-08 missing-run note was included in the AGENTS/handoff publication cleanup; the cause classification remains automation not running unless later scheduler evidence proves otherwise
  - `QA-020` resolved on 2026-06-06 because commit `ea36e52` made the `2026-05-30` through `2026-06-01` missing-run note branch-visible; the cause classification remains unavailable worktree / wrong accessible checkout on `2026-05-30` and `2026-06-01`, plus unknown scheduler behavior on `2026-05-31`
  - `QA-021` resolved on 2026-06-06 because commit `ea36e52` made the `2026-06-03` missing-run note branch-visible; the cause classification remains automation not running
  - `QA-022` resolved on 2026-06-06 after the daily audit repaired stale `Master 16.55` build badges in the source/release entry points and stale `Master 16.52` / `Master 16.41` process-doc references
  - `QA-024` resolved on 2026-06-09 after the daily audit repaired the new `Master 16.59` source-of-truth drift across the source/release entry-point build labels, the release manifest, the startup protocol, the active handoff, and the backlog recommendation
  - `QA-025` resolved on 2026-06-18 after the 2026-06-16 through 2026-06-17 missing-run note and daily-audit package became branch-visible
  - `QA-026` resolved on 2026-06-23 after the 2026-06-19 through 2026-06-22 missing-run note and 2026-06-23 daily-audit report became branch-visible
  - `QA-027` resolved on 2026-06-23 after the actual `daily-qa-audit` automation was updated from detached worktree execution to local governed-repo execution and its prompt was refreshed to include root `AGENTS.md`
  - `QA-006` closed after user confirmed the long-idle tab-close defect no longer reproduces
  - `QA-007` closed for the current defect state; ongoing buff/lore wording clarity remains a product-backlog improvement item
  - `QA-016` and the `Master 16.28` traffic/soldier-growth regression set closed after user validation
  - `QA-018` resolved by documenting the unrecoverable 2026-05-23 through 2026-05-27 missing-run gap and adding a missed-run backstop to the actual automation prompt plus governed QA procedures
- current gameplay validation should also cover Endless Waves continuity, player-death exit behavior, rival respawn behavior, save/load expectations, and runaway growth tuning
- current gameplay validation should specifically re-check saved skyscraper visuals after loading, growth after soldier damage, and panic-car skid distance
- governance recovery added on 2026-05-20: product-intent continuity and modular architecture awareness are now monitor-controlled under `QA-011`
- broader lore expansion should wait until the current `Master 16` UX validates
- remaining Priority 1 wave-system performance work resumes after the modular production package path is under control and `Master 16` proves stable
- do not collapse the medium-office-building voxel item into generic collapse polish; it is the reusable stack/voxel pattern for future procedural stacked structures and theme objects

## 5. Immediate Next Move

- treat `Master 16.93` as the current source basis within `10_SOURCE/Masters/Master 16/`
- treat `index.html` as the package entry point only; the full modular `/holesy/` folder remains the governed release baseline
- for routine manual GoDaddy uploads, provide a changed-files-only delta package by default when live is already on the previous approved master
- treat commit `304db13` as proof that the previously unpublished `2026-06-06`, `2026-06-07`, `2026-06-08` missing-run note, and `2026-06-09` audit package became branch-visible; treat commit `a0df57e` as proof that `QA_REVIEW_DAILY_AUDIT_2026-06-10.md`, `QA_REVIEW_DAILY_AUDIT_2026-06-11.md`, and `QA_REVIEW_DAILY_AUDIT_2026-06-12.md` are branch-visible; treat commit `7be2c8e` as proof that the 2026-06-13 through 2026-06-18 daily-audit recovery package became branch-visible; treat commits `5935cac`, `f204873`, and `561ce3a` as proof that the 2026-06-19 through 2026-06-23 daily-audit gap, `QA-026`, and `QA-027` automation repair package became branch-visible
- treat the 2026-06-24 scheduled audit as proof that the local governed-repo automation fired again; future daily audits should compare governed reports, automation memory, and supplied last-run metadata together before closing cadence health
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
  - `Master 16.71` regression, day/time-of-day behavior, manual Time button cycling, the added lighting effects, government-building debris feel, medium-office voxel behavior, and devoured-streetlamp flicker-off behavior passed user validation on 2026-06-11
  - `Master 16.73` short building-window flicker passed live-play visual confirmation on 2026-06-11
  - `Master 16.77` government-building medium-style voxel collapse behavior and small house/shop breakup passed user live-play validation on 2026-06-13 with no defects
  - validate `Master 16.93` build label, immediate mode-selection interaction, responsive Begin/world building, smooth sustained movement and dense consumption, intact audio/SFX after deferred decoding, readable Run Goals, tank-boss visibility/behavior, randomized wave-start corners, corrected skyscraper debris direction, MegaKit Downtown/default Classic behavior, and no regression to validated building/destruction physics
- only after those validations:
  - return to the remaining gameplay intake: lore/buff wording clarity, improved skyscraper collapse variation, and daily/weekly quest/reward architecture
  - expand the lore data module with the remaining approved Rival / Response corpus when the user chooses lore/data expansion
  - resume the remaining Priority 1 performance backlog
  - then return to Waves tuning, physics stack work, and broader powerup/mode expansion

