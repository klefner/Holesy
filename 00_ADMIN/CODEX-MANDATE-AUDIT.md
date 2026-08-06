# Codex Work Order — Holesy Mandate / Run-Goal Audit & Fairness Fix

**Owner:** klefner
**Author of brief:** Claude (analysis of governed source)
**Governed target:** `10_SOURCE/Masters/Master 16/` — **Master 16.197**, commit `d26dd98`
**Files in scope:** `js/main.js`, `js/difficulty-profiles.js`, `index.html`, `how-to-play.html`
**Status:** ☐ Not started ☐ In progress ☐ Done

---

## Ground rules — read before touching anything

- Do **not** redefine terms. A **Mandate** is only what `selectMandateTargets()` builds from `MANDATE_TARGET_SLOTS` (main.js ~line 8239). A **Run Goal** is only what `beginRunObjectives()` builds from `RUN_OBJECTIVE_DEFS` (main.js ~line 7892). Separate systems, separate panels. Do not conflate them.
- Do **not** edit the 57 entries in `MANDATE_TARGET_SLOTS`. They are all `Eat/Devour N <category>` counts. None references goals.
- Make **no** behavior change until Task 1 evidence is posted in the Status Log below.
- Anchor every edit to a function name + exact code string, not a bare line number.

## Established facts (verify, don't re-derive)

1. `MANDATE_TARGET_SLOTS` has **57** slots; none contains goal / "sweep" / "all goals" text.
   - `grep -nE "mandateSlot\(\{" main.js | wc -l` -> expect `57`
   - `grep -niE "mandate" main.js | grep -iE "goal|sweep|all objectives|complete all"` -> expect **no** hit inside the slots array.
2. The only "complete all goals" strings in the repo are the **Prism Orbit cosmetic** (`prism_orbit`, main.js line 33; hint at ~8183) and the **Goal Sweep** Run-Goal reward (~6549 / ~8148 / ~8214). These are rewards, not mandates. Leave their wording alone unless Task 4 requires the mandate-panel copy.
3. Military mandates are already wave-gated: `soldiers`/`military_units` -> `minWave: 2`, `boss_units` -> `minWave: 5`; and `selectMandateTargets()` only picks slots with `availableCount > 0`. An impossible military **mandate** cannot roll on Wave 1. Do not "fix" this.
4. A missed mandate does **not** end the run in this source. `onWaveTimerExpired()` (~12249) never checks mandate state; `triggerMandateFailureGameOver()` (~8703) has **no callers** (dead code). `how-to-play.html` line 204 still says "a missed Mandate ends the run" — stale, contradicts the code.

---

## Task 1 — Locate the actual defect (AUDIT ONLY, no edits)

The user reports a Wave-1 "complete all goals / kill soldiers" objective that ends the run. That behavior does **not** exist in governed 16.197. So it is either the Run-Goal fairness bug (Task 3) or the **deployed/live build differs** from 16.197.

- [ ] Post grep output proving facts 1–4.
- [ ] Diff the live-deployed bundle against `Master 16.197`. If the live build has a mandate that references goals, or wires `triggerMandateFailureGameOver()`, **stop and report exact file/line** — do not edit; that is separate, out-of-governance.
- [ ] Post findings in the Status Log before proceeding.

## Task 2 — Remove goal-referencing mandates *only if Task 1 finds one*

- [ ] If a mandate slot referencing Run-Goal completion is found in any build, delete **only** that slot and report it verbatim.
- [ ] In governed 16.197 there is nothing to remove — state that explicitly. Do not invent a change.

## Task 3 — Fix Wave-1 soldier Run Goals (the one real impossibility vector)

Soldier Run Goals (`RUN_OBJECTIVE_DEFS` entries with `waveOnly: true`, ~7926–7945) are selectable on Wave 1, which is soldier-free (`soldiersEnabled: false`). `isRunObjectiveEligible()` (~8025) only excludes them in non-wave modes — it never checks soldier availability for the wave the set is built on.

- [ ] In `isRunObjectiveEligible(def)`, replace:
  ```js
  if (def.waveOnly && !isWaveBasedMode()) return false;
  ```
  with logic that also returns `false` for soldier-family goals when `getWaveConfig(currentWave).soldiersEnabled` is false **and** no wave in the current goal-set window (`currentWave` … `currentWave + RUN_OBJECTIVE_SET_REFRESH_INTERVAL - 1`, clamped to run length) has `soldiersEnabled === true` or `shouldWaveSpawnArmyBoss()` true.
- [ ] Do **not** hard-remove soldier goals — standard 4-wave runs must still be able to receive them (soldiers exist Waves 2–4). Keep `refreshRunObjectivesForWave` persistence behavior unchanged.
- [ ] Confirm Waves 2–4 can still receive soldier goals after the change.

## Task 4 — Reconcile docs with actual mandate behavior

**DECISION: Option A (default set by klefner — reversible; may be revisited later).**

Code is currently **reward-only**; how-to-play says it ends the run. Make the docs match the code.

- [ ] **Option A (APPROVED):** edit `how-to-play.html` lines 194–205 (and `index.html` mandate copy if needed) so mandates read as an optional completion **reward** (speed + damage protection), not run-ending. **No JS behavior change.**
- [ ] Option B (NOT approved — do not implement unless klefner explicitly switches to it later): make mandates truly mandatory by re-wiring `triggerMandateFailureGameOver()` into `onWaveTimerExpired()` **and** guaranteeing every selected mandate is solvable within the wave timer.

---

## Out of scope — do not touch

The 57 mandate slots' counts/labels, mandate wave-gating (`minWave`), `selectMandateTargets`, the Prism Orbit / Goal Sweep reward wording (unless Task 4 Option A needs mandate-panel copy), and any file outside the four listed.

## Required evidence to return with the PR

- [ ] Grep output proving facts 1–4.
- [ ] Task 1 live-vs-16.197 diff summary.
- [ ] Task 3 before/after of `isRunObjectiveEligible`, plus note confirming Waves 2–4 still receive soldier goals.
- [ ] Task 4 (Option A) — exact doc text changed.

---

## Status Log (Codex fills this in)

| Date | Task | Action taken | Evidence / link |
|------|------|--------------|-----------------|
|      |      |              |                 |
