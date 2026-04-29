## Holesy Handoff Briefing For The Next Codex Chat

This document is the takeover package for any new Codex chat that needs to continue Holesy development without relying on the full prior thread.

## 1. What A Complete Handoff Document Must Contain

At minimum, a replacement Codex chat needs:

- project identity and product intent
- current approved build / current master
- exact local repo path and working branch
- technical stack summary
- code architecture summary
- directory structure and what belongs where
- GitHub workflow and governance rules
- website deployment workflow
- testing workflow and how the user validates changes
- product backlog and current priorities
- current known assumptions, decisions, and constraints
- known pitfalls / regressions already encountered
- active operational status: what is stable, what is pending, what should be ignored

Additional factors that should also be included because they are important on this project:

- user working style and expectation management
- QA / risk-control requirements
- release naming rules (`Master` promotion rules)
- exact file paths for the most important docs and artifacts
- website-specific publishing realities (WordPress page slug versus physical directory)
- known untracked files that should not be swept into commits by accident

The rest of this document includes all of that.

## 2. Project Identity

Project name:

- `Holesy`
- game title shown to players: `Downtown Devour`

High-level concept:

- a competitive hole-eats-city game in the `hole.io` genre
- differentiators include:
  - military threats (planes, paratroopers, soldiers, bullets)
  - stronger AI personality differentiation
  - more strategic pressure than typical competitors
  - procedural music / event signaling
  - future powerups and physics-driven stacked object systems

## 3. User Context And Working Style

The user is new to GitHub-centered SDLC and needs plain-language instructions.

Important working expectations:

- do not assume the user knows GitHub mechanics
- explain operational steps concretely
- treat game design and product direction as collaborative, but take technical lead responsibility
- preserve rollback and avoid unsafe edits
- use a separate QA-minded pass before declaring material work ready
- the user does a lot of browser-based human testing and reports defects iteratively

## 4. Canonical Local Paths

Canonical repo root:

- `C:\Users\KentLefner\Desktop\game-repo\Holesy`

Current Codex temporary workspace for this thread:

- `C:\Users\KentLefner\Documents\Codex\2026-04-23-first-things-first-let-s-walk`

Most important project folders:

- `C:\Users\KentLefner\Desktop\game-repo\Holesy\00_ADMIN`
- `C:\Users\KentLefner\Desktop\game-repo\Holesy\10_SOURCE`
- `C:\Users\KentLefner\Desktop\game-repo\Holesy\20_TESTS`
- `C:\Users\KentLefner\Desktop\game-repo\Holesy\30_ARCHIVE`
- `C:\Users\KentLefner\Desktop\game-repo\Holesy\40_RELEASE`

## 5. Current Approved Baseline

Current promoted master:

- `C:\Users\KentLefner\Desktop\game-repo\Holesy\10_SOURCE\Masters\Master 13.html`

Current basis note:

- `C:\Users\KentLefner\Desktop\game-repo\Holesy\10_SOURCE\Current\CURRENT_BASIS.md`

What `Master 13` specifically represents:

- it includes the full approved `Master 12` gameplay baseline
- it keeps the visible master version on the title screen
- it preserves the correct universal startup flow:
  - first screen is the mode-select screen on all platforms
  - `Begin` primes audio and starts the game in the same gesture
- it adds the next validated `P1.8` slice:
  - player-facing active effect timer badges
  - player-anchored final-3-seconds Waves warning above the `You` label
  - smoothed thicker outer rim during bullet-resistance effects
  - clockwise wind-pull ring visualization tied to hole-wind modulation
- it establishes explicit candidate sub-build lineage beneath the current master so test builds can be referred to precisely:
  - accepted pre-promotion candidate lineage here was `Master 12.3`, `Master 12.4`, then `Master 12.5`

Last approved candidate promoted into `Master 13`:

- `C:\Users\KentLefner\Desktop\game-repo\Holesy\20_TESTS\Candidate_Builds\Master 12.5 - active-effect-timers-ui.html`

## 6. Technical Stack

Current runtime model:

- single-file HTML game
- Three.js-based rendering
- browser JavaScript
- browser CSS
- no current full module split
- no current physics engine integrated into the approved baseline

Operational stack:

- local file testing via `file:///...`
- Git for version control
- GitHub repo for remote backup and branch workflow
- GoDaddy Managed WordPress hosting for the public site
- standalone `index.html` deployment for the public `/holesy/` path

Important note:

- the approved baseline does **not** yet include the planned `cannon-es` stack/collapse system

## 7. Code Architecture Summary

The codebase is still a large single-file game with stabilization work partially completed.

Major architecture improvements already completed:

- explicit game state foundation
- input interpretation separated from the main gameplay loop
- audio startup separated from broader progression logic
- top-level config centralization for balancing values

Important architecture status:

- Priority 1 stabilization is **not fully finished**
- the next active stabilization task is still:
  - `P1.8 Add lightweight debug tools`

Important active architectural seam:

- startup flow was recently corrected after regression confusion
- the correct intended behavior is:
  - all platforms start on the mode-select screen
  - no separate standalone `Tap to Load Music` first screen

## 8. Directory Structure

Top-level layout:

- `00_ADMIN`
  - policies, procedures, requirements, reviews, reports
- `10_SOURCE`
  - official source baselines and masters
- `20_TESTS`
  - candidate and exploratory builds
- `30_ARCHIVE`
  - older historical assets kept for recovery/reference
- `40_RELEASE`
  - website publish package and release-oriented artifacts
- `99_TEMP`
  - disposable scratch space

Important specific docs:

- risk policy:
  - `C:\Users\KentLefner\Desktop\game-repo\Holesy\00_ADMIN\Policies_and_Procedures\RISK_AND_CONTROLS_POLICY.md`
- GitHub operating model:
  - `C:\Users\KentLefner\Desktop\game-repo\Holesy\00_ADMIN\Policies_and_Procedures\GITHUB_OPERATING_MODEL.md`
- standalone website workflow:
  - `C:\Users\KentLefner\Desktop\game-repo\Holesy\00_ADMIN\Policies_and_Procedures\STANDALONE_WEBSITE_PUBLISH_WORKFLOW.md`
- product backlog:
  - `C:\Users\KentLefner\Desktop\game-repo\Holesy\00_ADMIN\Requirements\PRODUCT_BACKLOG.md`
- Waves narrative reference:
  - `C:\Users\KentLefner\Desktop\game-repo\Holesy\00_ADMIN\Requirements\WAVES_NARRATIVE_AND_TEXT_PACK.md`

## 9. GitHub / Git Configuration And Governance

Canonical repository:

- `klefner/Holesy`

Current working branch:

- `codex/publish-master4-structure`

Important GitHub state:

- push access works
- this branch is backed up on GitHub
- `main` has branch protection / ruleset guardrails
- `main` is treated as the stable shared baseline

Required working model:

1. work locally on a branch
2. test in browser
3. preserve rollback
4. commit meaningful checkpoints
5. push to GitHub
6. merge to `main` only intentionally

Do not:

- work directly on `main`
- casually stage unrelated files
- sweep exploratory files into commits

Known untracked files that have repeatedly existed and should be treated carefully:

- `C:\Users\KentLefner\Desktop\game-repo\Holesy\20_TESTS\Exploratory_Builds\Stack-collapse exploration - cannon-es prototype.html`
- `C:\Users\KentLefner\Desktop\game-repo\Holesy\20_TESTS\Candidate_Builds\index.html`

The next chat should inspect `git status` before every commit and avoid staging those unless the user explicitly wants them included.

## 10. Risk / QA / Control Requirements

The governing policy is:

- `C:\Users\KentLefner\Desktop\game-repo\Holesy\00_ADMIN\Policies_and_Procedures\RISK_AND_CONTROLS_POLICY.md`

Non-negotiable operating expectations from that policy:

- preserve rollback
- prefer narrow fixes over sprawling rewrites
- separate diagnosis, remediation, validation, and release decisions
- use named test artifacts for risky changes
- perform a separate QA-style review before presenting material work as ready
- clearly distinguish:
  - local file state
  - Git state
  - GitHub state
  - live website state

Every material completion should substantively state:

- baseline used
- rollback path
- what changed
- what remains unverified

## 11. Website Deployment Reality

Public site:

- `https://ptbooksinc.com`

Standalone game path:

- `https://ptbooksinc.com/holesy/`

Important history:

- originally there was a WordPress page with slug `holesy`
- the game was being injected via a WordPress snippet
- later, a real physical `/holesy/` directory was created in hosting and a standalone `index.html` upload workflow was made to work

Important operational truth:

- the live `/holesy/` path should now be treated as a standalone uploaded file path, not primarily as a WordPress snippet path

Website publish package:

- `C:\Users\KentLefner\Desktop\game-repo\Holesy\40_RELEASE\Website_Publish_Package\holesy\index.html`

This package is meant to be uploaded into the hosting directory `/holesy/`.

GoDaddy navigation that worked:

1. from the GoDaddy dashboard
2. click `Manage Hosting`
3. go to `Settings`
4. go to `Tools`
5. open `File Browser`
6. open or create the `holesy` folder
7. upload `index.html`

Important WordPress / hosting distinction:

- a WordPress page slug can make a URL exist even without a physical directory
- that caused confusion earlier
- do not confuse the WordPress `holesy` page with the real `/holesy/` hosting directory

## 12. Testing Workflow

The user primarily tests in-browser and reports defects.

Typical testing pattern:

- local file testing first from candidate builds under `20_TESTS\Candidate_Builds`
- then promotion to a new `Master`
- then optional website publish package update

Testing environments used:

- PC browser heavily
- mobile behavior also matters
- the user may not test every version equally on every platform unless prompted

Good testing prompts:

- keep them narrow
- ask the user to verify only the touched seam(s)
- avoid bundling many unrelated asks in one test instruction

## 13. Product / Gameplay State Summary

Broad gameplay systems already present:

- multiple game modes including Waves
- rival AI personalities
- military threat system
- aid-drop / powerup system
- wave transitions and lore messaging
- HUD and pause/menu flow

Some major completed content/status highlights:

- visible version label on first screen
- universal mode-select startup fixed
- wave transition readability improvements
- alien aid-drop system
- Wave 4 battlefield pressure concept

## 14. Current Product Backlog

Primary backlog document:

- `C:\Users\KentLefner\Desktop\game-repo\Holesy\00_ADMIN\Requirements\PRODUCT_BACKLOG.md`

Key backlog truths right now:

- Priority 1 stabilization is still active
- `P1.6` is complete and promoted into `Master 8`
- `P1.7` remains in progress and `Master 10` includes additional validated cleanup-adjacent payoff/timing polish
- `P1.8` is now in progress and `Master 13` carries the next validated instrumentation/audio-tuning-adjacent slice
- the next active engineering task is:
  - `P1.8 Add lightweight debug tools`
- hardest remaining major system is still:
  - `Priority 3 — Physics Stack And Collapse System`

Important backlog additions already captured:

- difficulty toggle with `Off` / `Nightmare`
- variablized AI intelligence / efficiency
- at least one materially smarter rival per game
- player should be able to lose on points
- first-person / tactical camera switch concept
- jump / elevated collection system
- Solo 100% Clear mode
- strategic powerup expansion

## 15. Hardest Remaining Item

The hardest remaining item is still:

- the physics stack and collapse system

Why:

- it introduces a new runtime subsystem
- requires hybrid object simulation
- complicates reset / teardown behavior
- creates performance risk
- creates collision and consumption edge cases
- touches future object variety, debris, and building-break systems

This is harder than:

- difficulty toggle
- AI intelligence tuning
- POV switch
- jump system

## 16. Important Recent Decisions

- the user explicitly confirmed the `Master 12.5` active-effect / pull-ring candidate had no defects
- that build was promoted to `Master 13`
- current canonical baseline is therefore `Master 13`
- the Waves narrative direction and replacement text pack were captured for future Priority 2 work in:
  - `C:\Users\KentLefner\Desktop\game-repo\Holesy\00_ADMIN\Requirements\WAVES_NARRATIVE_AND_TEXT_PACK.md`

Important corrected product behavior:

- first screen on all platforms is the mode-select screen
- `Begin` handles audio priming and starts the game
- the old separate load-music-first screen is not the intended product behavior anymore

## 17. Known Traps / Regressions To Watch For

- startup flow regressions around mode-select vs audio gate
- mobile / touch event handling around the `Begin` button
- confusing local-versus-live website state
- accidentally staging exploratory physics files
- wave / reset state integrity should still be watched as `P1.7` and later structure work continue
- AI balance currently still too easy for the player on points

## 18. Recommended Next Moves For The Replacement Chat

1. Read:
   - `CURRENT_BASIS.md`
   - `PRODUCT_BACKLOG.md`
   - `RISK_AND_CONTROLS_POLICY.md`
   - `GITHUB_OPERATING_MODEL.md`
2. Treat `Master 13` as the baseline
3. Confirm `git status` before any new work
4. Keep the known exploratory files out of commits unless explicitly asked
5. Continue either:
   - the user’s next gameplay request, or
  - `P1.8` if the user wants backlog-driven stabilization next

## 19. If The New Chat Needs A Fast Mental Model

Use this summary:

- Single-file Three.js game
- repo is organized and governed now
- GitHub push works
- `main` is protected
- `Master 13` is current baseline
- user tests frequently and expects careful regression control
- hardest future system is physics-based stacked-object collapse
- current backlog already includes difficulty scaling, stronger AI, first-person POV, jump/verticality, new modes, and strategic powerups

## 20. Final Handoff Status

This document is intended to let a new Codex chat take over without needing the full prior thread.

If anything conflicts with later repo files, the repo files are the source of truth.
