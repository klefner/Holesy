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

- `Priority 2 — Waves Mode Completion and Tuning` remains open
- new work should branch from `Master 15`
- the next development focus is continued Waves difficulty / pacing tuning from the now-approved stronger baseline rather than reviving the failed `14.5` branch wholesale

## 5. Immediate Next Move

- treat `Master 15` as the current source basis
- continue Priority 2 from a fresh candidate under `20_TESTS/Candidate_Builds`
- keep future Waves changes narrow and browser-validated before promotion
- current in-flight candidate:
  - `C:\Users\KentLefner\Desktop\game-repo\Holesy\20_TESTS\Candidate_Builds\Master 15.8 - aid-stutter-camera-and-mid-ai-buff.html`
- current candidate focus:
  - remove stale post-aid stutter, lower the transition camera, and make Void/Maw feel more competitive
