## Priority 1 Stabilization Plan

Purpose: stabilize the Holesy codebase before major feature expansion so future changes are faster, safer, and easier to validate.

## Why Priority 1 Comes First

The project has already shown fragility in:

- startup and mobile music handoff
- input ownership between mouse, keyboard, and touch
- HUD/UI overlap and placement changes
- mode-specific scoring behavior
- pause/exit/reset behavior
- Waves-mode progression logic

Those are foundation concerns, not content concerns. Building Endless Mode or a progression layer on top of unstable foundations would increase delivery time and regression risk.

## Priority 1 Goal

Create a cleaner game foundation with explicit state transitions, clearer ownership of input/audio/UI systems, safer reset behavior, and easier future refactors.

## Recommended Execution Order

### 1. Freeze the current gameplay baseline

Objective:

- preserve the currently working gameplay baseline before structural refactor work begins

Tasks:

- keep the current backup branch intact
- preserve the current approved gameplay candidate as a recovery target
- clearly name the active refactor basis in project docs

Why first:

- structural work without a trusted rollback point is high risk

### 2. Produce a current code map

Objective:

- document what exists before changing structure

Tasks:

- map startup/title flow
- map mode selection flow
- map in-game loop responsibilities
- map input paths for mouse, keyboard, and touch
- map audio startup and music controls
- map HUD/update responsibilities
- map reset/restart/end-state behavior

Deliverable:

- a code map document that names the major systems and their current coupling points

Why second:

- we should not refactor blind in a large single-file codebase

### 3. Introduce an explicit game state machine

Objective:

- replace implicit screen/flow behavior with named states

Target states:

- boot
- title
- mode_select
- loading_audio
- playing
- paused
- wave_transition
- game_over

Tasks:

- centralize current-state ownership
- define allowed transitions
- stop using scattered flags as the only source of truth for screen flow

Why third:

- startup, pause, resume, end screen, and mode switching are the highest-fragility paths

### 4. Separate input from gameplay behavior

Objective:

- make input interpretation independent from hole movement/game rules

Tasks:

- centralize input source ownership
- unify mouse, keyboard, and touch into one control model
- clarify handoff rules between input types
- isolate player-targeting logic from per-frame game simulation

Why fourth:

- input regressions have already cost time and are one of the biggest blockers to safe iteration

### 5. Separate audio startup from game progression

Objective:

- keep mobile/browser audio constraints from breaking gameplay progression

Tasks:

- isolate music/audio initialization behind a dedicated audio controller
- keep title-to-mode-select transition independent from audio success
- preserve the user-gesture-based mobile startup path as a guarded entry point

Why fifth:

- this was a real production defect already, so it needs to be treated as a protected seam

### 6. Centralize configuration values

Objective:

- stop scattering balance and UI constants throughout the file

Config groups:

- mode durations
- Waves difficulty
- soldier tuning
- scoring values
- HUD layout values
- movement/input tuning

Why sixth:

- balance changes like the recent Waves timing adjustment should be fast and low-risk

### 7. Refactor reset/restart/rebuild behavior

Objective:

- make restarts and transitions predictable

Tasks:

- centralize round reset
- centralize mode start
- centralize wave advance cleanup
- ensure entity teardown and respawn are deterministic

Why seventh:

- reset logic is where multi-mode games tend to silently corrupt state

### 8. Split major logic regions into clearer modules or module-like sections

Objective:

- improve maintainability even if the code remains in one HTML file for a while

Target boundaries:

- state machine
- input
- audio
- HUD/UI
- scoring
- waves/military
- entities/spawning
- game loop/update orchestration

Why eighth:

- once the dangerous seams are stabilized, structural cleanup becomes safer

### 9. Add lightweight debug tools

Objective:

- make future tuning and defect diagnosis faster

Examples:

- current game state display
- current mode display
- active input source display
- wave debug summary
- optional spawn/timer readouts

Why ninth:

- these help every future feature and reduce guesswork during regression testing

### 10. Only then move into larger feature expansion

Priority 2 after stabilization:

- Endless Mode

Priority 3 after that:

- reward/progression systems

Priority 4 after that:

- replayability and content expansion

## First Refactor Patch Recommendation

The first structural patch should be:

- introduce the explicit game state model
- route startup/title/mode-select/pause/game-over transitions through it
- do not mix that patch with Endless Mode, rewards, or monetization work

This creates the safest first architectural improvement because it reduces ambiguity without forcing a full rewrite.

## Guardrails

- Do not break the mobile startup/music path while refactoring
- Do not mix architecture work with broad gameplay rebalance in the same pass
- Preserve a testable candidate build before each structural step
- Push each structural checkpoint to GitHub before stacking the next one

## Technical Lead Recommendation

Do not start Endless Mode yet.

Finish Priority 1 first, beginning with:

1. code map
2. state machine introduction
3. input separation
4. audio separation

That order gives the highest stability return for the next block of work.
