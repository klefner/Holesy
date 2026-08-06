# Codex Work Order — Holesy Mandate / Run-Goal Audit & Fairness Fix

**Owner:** klefner
**Author of brief:** Claude (analysis of governed source)
**Governed target:** `10_SOURCE/Masters/Master 16/` — **Master 16.197**, commit `d26dd98`
**Files in scope:** `js/main.js`, `js/difficulty-profiles.js`, `index.html`, `how-to-play.html`
**Status:** ☐ Not started ☐ In progress ☐ Done

---

## Ground rules — read before touching anything

- Do **not** redefine terms. A **Mandate** is only what `selectMandateTargets()` builds from `MANDATE_TARGET_SLOTS` (main.js ~line 8239). A **Run Goal** is only what `beginRunObjectives()` builds from `RUN_OBJECTIVE_DEFS` (main.js ~line 7892). Separate systems, separate panels. Do not conflate them.
- Do **not** edit the 57 entries in `MANDATE_TARGET_SLOTS`' category tests/labels. You MAY adjust their count-scaling fields only as required by Task 4b solvability (see that task).
- Make **no** behavior change until Task 1 evidence is posted in the Status Log below.
- Anchor every edit to a function name + exact code string, not a bare line number.

## Established facts (verify, don't re-derive)

1. `MANDATE_TARGET_SLOTS` has **57** slots; none contains goal / "sweep" / "all goals" text.
   - `grep -nE "mandateSlot\(\{" main.js | wc -l` -> expect `57`
   - `grep -niE "mandate" main.js | grep -iE "goal|sweep|all objectives|complete all"` -> expect **no** hit inside the slots array.
2. The only "complete all goals" strings in the repo are the **Prism Orbit cosmetic** (`prism_orbit`, main.js line 33; hint at ~8183) and the **Goal Sweep** Run-Goal reward (~6549 / ~8148 / ~8214). These are rewards, not mandates. Leave their wording alone unless Task 4 requires the mandate-panel copy.
3. Military mandates are already wave-gated: `soldiers`/`military_units` -> `minWave: 2`, `boss_units` -> `minWave: 5`; and `selectMandateTargets()` only picks slots with `availableCount > 0`. An impossible military **mandate** cannot roll on Wave 1. Keep this.
4. A missed mandate does **not** end the run in this source. `onWaveTimerExpired()` (~12249) never checks mandate state; `triggerMandateFailureGameOver()` (~8703) has **no callers** (dead code). `how-to-play.html` line 204 already says "a missed Mandate ends the run" — currently stale; Task 4 (Option B) makes it TRUE.

---

## Task 1 — Locate the actual defect (AUDIT ONLY, no edits)

The user reports a Wave-1 "complete all goals / kill soldiers" objective that ends the run. That behavior does **not** exist in governed 16.197. So it is either the Run-Goal fairness bug (Task 3) or the **deployed/live build differs** from 16.197.

- [ ] Post grep output proving facts 1–4.
- [ ] Diff the live-deployed bundle against `Master 16.197`. If the live build has a mandate that references goals, or already wires `triggerMandateFailureGameOver()`, **report exact file/line** in the Status Log.
- [ ] Post findings in the Status Log before proceeding.

## Task 2 — Remove goal-referencing mandates *only if Task 1 finds one*

- [ ] If a mandate slot referencing Run-Goal completion is found in any build, delete **only** that slot and report it verbatim.
- [ ] In governed 16.197 there is nothing to remove — state that explicitly. Do not invent a change.

## Task 3 — Fix Wave-1 soldier Run Goals (impossibility vector, independent of Task 4)

Soldier Run Goals (`RUN_OBJECTIVE_DEFS` entries with `waveOnly: true`, ~7926–7945) are selectable on Wave 1, which is soldier-free (`soldiersEnabled: false`). `isRunObjectiveEligible()` (~8025) only excludes them in non-wave modes — it never checks soldier availability for the wave the set is built on.

- [ ] In `isRunObjectiveEligible(def)`, replace:
  ```js
  if (def.waveOnly && !isWaveBasedMode()) return false;
  ```
  with logic that also returns `false` for soldier-family goals when `getWaveConfig(currentWave).soldiersEnabled` is false **and** no wave in the current goal-set window (`currentWave` … `currentWave + RUN_OBJECTIVE_SET_REFRESH_INTERVAL - 1`, clamped to run length) has `soldiersEnabled === true` or `shouldWaveSpawnArmyBoss()` true.
- [ ] Do **not** hard-remove soldier goals — standard 4-wave runs must still receive them (soldiers exist Waves 2–4). Keep `refreshRunObjectivesForWave` persistence behavior unchanged.
- [ ] Confirm Waves 2–4 can still receive soldier goals after the change.

## Task 4 — Make mandates truly MANDATORY  (Option B — APPROVED by klefner)

klefner chose Option B: **a missed mandate ends the run.** This restores the original design and makes `how-to-play.html` line 204 true.

> **This is only safe if every mandate the game hands the player is guaranteed completable in the wave timer.** Otherwise the unfair, unexplained game-overs return — the exact bug this entire work order exists to kill. **The solvability guarantee (4b) is the primary deliverable. Do not ship 4a without 4c evidence.**

### 4a — Wire enforcement
- [ ] In `onWaveTimerExpired()` (~12249), BEFORE advancing (`enterWaveTransition(currentWave + 1)`), check `isMandateSatisfied()`. If false, call `triggerMandateFailureGameOver()` and `return` without advancing.
- [ ] Confirm the chain: `triggerMandateFailureGameOver()` sets `pendingPlayerEndReason='mandate_failed'`, shows the failure banner, calls `endGame()`, and the game-over screen (~12757) renders "Mandate Failed." with the wave subtitle.
- [ ] Applies to all wave-based modes (`isWaveBasedMode()` = Waves + Endless). In Endless, an incomplete mandate ends the run at wave expiry (matches "Complete each Mandate to survive").

### 4b — Guarantee solvability (HARD GATE)
Every selected mandate must be completable by an average player in the wave's remaining time. Enforce ALL of:
- [ ] **Guaranteed supply, not estimated.** Cap required counts against the *minimum guaranteed* live supply, never averages/projections. For soldiers/military, base the cap on the wave's guaranteed floor (`soldierCountMin` + guaranteed drops), not `estimateMandateSoldierSupply()`'s average — RNG must never yield fewer objects than the mandate demands.
- [ ] **Competition + attrition margin.** Rivals eat and objects get destroyed. Apply a safety margin so realized supply can't drop below the requirement (tighten the ~0.34 reachable-share assumption near `estimatePlayerReachableRadiusForMandate` ~8406 if needed).
- [ ] **Time-feasibility cap.** Cap required count to what is physically eatable within the wave timer given starting hole size and speed — not just what exists on the map. Extend the existing ratio caps (`baseCapRatio`/`maxRatio` in `getMandateRequiredCount` ~8374) rather than replacing them.
- [ ] **Keep existing guards:** `minWave` gating, `availableCount > 0`, building reachability (`isMandateObjectReachable`).

### 4c — Prove it (required before merge)
- [ ] Add/run a validation harness that simulates many wave-starts across every mode, difficulty, and environment, asserting **every** generated mandate is completable within its timer under worst-case RNG. Report sample size and **zero** failures. If any config can generate an unwinnable mandate, fix generation — do not ship.
- [ ] Update `how-to-play.html` / `index.html` mandate copy to clearly state mandates are required and end the run if missed, with the reward for completing them.

---

## Out of scope — do not touch

Mandate category tests/labels in `MANDATE_TARGET_SLOTS`, `minWave` gating, the Prism Orbit / Goal Sweep reward wording (unless Task 4c needs mandate-panel copy), and any file outside the four listed. (Count-scaling fields may change only where Task 4b requires.)

## Required evidence to return with the PR

- [ ] Grep output proving facts 1–4.
- [ ] Task 1 live-vs-16.197 diff summary.
- [ ] Task 3 before/after of `isRunObjectiveEligible`, plus note confirming Waves 2–4 still receive soldier goals.
- [ ] Task 4a: the enforcement diff in `onWaveTimerExpired`, and a confirmed "Mandate Failed." game-over path.
- [ ] Task 4c: solvability harness results (sample size + zero unwinnable mandates) and the updated player-facing copy.

---

## Status Log (Codex fills this in)

| Date | Task | Action taken | Evidence / link |
|------|------|--------------|-----------------|
|      |      |              |                 |
