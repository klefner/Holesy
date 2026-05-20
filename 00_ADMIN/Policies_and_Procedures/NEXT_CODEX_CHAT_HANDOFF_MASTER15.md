## Holesy Handoff Briefing For The Next Codex Chat

This document is the takeover package for any new Codex chat that needs to continue Holesy development without relying on the full prior thread.

## 1. Active Handoff Status

- this is the active handoff document because the approved local basis is now `Master 15`
- treat prior handoff files, including `NEXT_CODEX_CHAT_HANDOFF_MASTER14.md`, as historical context unless a specific earlier decision needs review
- other developer chats inside the same project are also primary-source background and decision history for takeover work; review them when available instead of relying only on the current thread summary

## 2. Current Approved Baseline

Current promoted master:

- `C:\Users\KentLefner\Desktop\game-repo\Holesy\10_SOURCE\Masters\Master 15.html`

Current basis note:

- `C:\Users\KentLefner\Desktop\game-repo\Holesy\10_SOURCE\Current\CURRENT_BASIS.md`

What `Master 15` specifically represents:

- it includes the full approved `Master 14` gameplay baseline
- it keeps the validated startup flow:
  - first screen is the mode-select screen on all platforms
  - `Begin` primes audio and starts the game in the same gesture
- it promotes the validated narrow-slice Waves rebuild from `14.6` through `14.10`
- approved additions now in baseline:
  - clearer Waves transition and defeat messaging
  - transition-safe buff timing
  - non-music scoreboard / mode-select audio cleanup while music continues
  - clearer bonus-mass alien-drop text
  - stronger rival aid contesting and better score pressure
  - explicit cleanup of transition overlays when exiting from paused Waves transitions back to game modes

Last approved candidate promoted into `Master 15`:

- `C:\Users\KentLefner\Desktop\game-repo\Holesy\20_TESTS\Candidate_Builds\Master 14.10 - rival-aid-and-score-pressure.html`

## 3. Current Working Rules

- Canonical repo root: `C:\Users\KentLefner\Desktop\game-repo\Holesy`
- Active branch: `codex/publish-master4-structure`
- Preserve rollback safety
- Do not work directly on `main`
- Distinguish local file state, git state, GitHub branch state, `main` state, and live website state
- Keep unrelated local modifications out of commits
- Every developer chat other than the current one is primary-source context when available

## 4. Current Priority

- immediate focus is validation of the latest monitored candidates before further backlog expansion
- new work should branch from `Master 15`
- active validation targets:
  - `Master 15.39 - idle-lifecycle-cleanup.html`
  - `Master 15.42 - buff-clarity-and-rare-docs.html`
- performance backlog continuation resumes after those validation passes close
- broader Waves tuning remains downstream work, not the current handoff target

## 5. Immediate Next Move

- treat `Master 15` as the current source basis
- validate the current monitored candidates before starting a new slice:
  - `C:\Users\KentLefner\Desktop\game-repo\Holesy\20_TESTS\Candidate_Builds\Master 15.39 - idle-lifecycle-cleanup.html`
  - `C:\Users\KentLefner\Desktop\game-repo\Holesy\20_TESTS\Candidate_Builds\Master 15.42 - buff-clarity-and-rare-docs.html`
- validation focus for `15.39`:
  - confirm the original long-idle Chrome tab-close stall is materially improved after a real idle wait
- validation focus for `15.42`:
  - confirm archive readability, Archive music fit, rare document pacing, buff-effect clarity, and lore-buff combo feel in real gameplay
- only after those validations:
  - expand the lore corpus if the reader UX is accepted
  - continue the remaining Priority 1 performance backlog
  - then return to Waves tuning, physics stack work, and broader powerup expansion
