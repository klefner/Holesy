## Holesy Product Backlog

This backlog reflects the current project direction, recent gameplay work, and the decision to treat codebase stabilization as Priority 1.

## Priority 1 — Codebase Stabilization

Goal: make the code safer to change, easier to reason about, and less likely to regress.

### P1.1 Current code map

Status:

- completed

Outcome:

- documented the current control-flow and fragility seams

### P1.2 Explicit game state foundation

Status:

- completed

Outcome:

- introduced named states for title, mode select, play, pause, LMS choice, wave transition, and game over

### P1.3 Separate input interpretation from gameplay loop

Status:

- completed

Outcome target:

- keyboard, mouse, and touch ownership handled through explicit helper logic rather than inline branching in the main loop

### P1.4 Separate audio startup from broader UI/game progression

Status:

- completed

### P1.5 Centralize config and balancing values

Status:

- pending

### P1.6 Refactor reset / restart / wave rebuild behavior

Status:

- pending

### P1.7 Strengthen section boundaries / module-like organization

Status:

- pending

### P1.8 Add lightweight debug tools

Status:

- pending

## Priority 2 — Waves Mode Completion and Tuning

Goal: finish the mode we already started and make it feel deliberately paced.

Backlog items:

- continue fine-tuning wave duration, cadence, and roster pressure
- decide whether true board-size reduction should be implemented or replaced with other pressure systems
- improve wave readability and progression signaling
- confirm the final difficulty curve feels fair on desktop and mobile

## Priority 3 — Endless Mode

Goal: unlock long-session replayability once the foundation is stable.

Backlog items:

- endless escalation rules
- scaling object density
- scaling AI pressure
- scaling hazard intensity
- endless end-condition / scoring framing

## Priority 4 — Rewards and Progression

Goal: make sessions more satisfying beyond raw score.

Backlog items:

- score grades
- streaks / combos
- between-wave bonuses or upgrade choices
- cosmetic unlockables

## Priority 5 — Replayability Expansion

Goal: increase variety across sessions.

Backlog items:

- procedural layout variation
- challenge variants
- alternative mode rules
- session goals

## Priority 6 — Monetization-Ready Layer

Goal: only after the core game is stable and replayable.

Backlog items:

- cosmetic progression readiness
- account/progression considerations
- store-ready non-intrusive reward structures

## Current Recommendation

Work Priority 1 to completion before serious Endless Mode investment.

The next active task after the current input patch should be:

- audio/startup separation
