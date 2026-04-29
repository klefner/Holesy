## Holesy Product Backlog

This backlog reflects the current project direction, approved gameplay work, the new standalone website deployment workflow, and the decision to treat codebase stabilization as Priority 1.

## Priority 0 — Operational Readiness

Goal: make the approved game easy to publish, recover, and operate outside the WordPress snippet path.

### P0.1 Deploy standalone `index.html` workflow

Status:

- completed

Outcome:

- documented the production workflow for publishing Holesy as a standalone `index.html` in `/holesy/`

### P0.2 Promote approved master to website publish package

Status:

- completed

Outcome:

- created a website-ready `index.html` package sourced from the approved master

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

- completed

Outcome:

- consolidated world, growth, traffic, input, HUD, audio, wave, and military tuning values into a single top-level config object so future balancing changes are no longer scattered across the file

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

Goal: finish the mode already in flight and make it feel deliberately paced.

Backlog items:

- continue fine-tuning wave duration, cadence, and roster pressure
- confirm the final difficulty curve feels fair on desktop and mobile
- improve progression readability and signaling where still needed
- validate the final-wave active-arena pressure model against repeat play

## Priority 3 — Physics Stack And Collapse System

Goal: introduce `hole.io`-style stacked-object variety with convincing gravity-driven collapse while preserving the current battlefield systems.

Backlog items:

- add a hybrid physics subsystem for stackable objects only
- create stack object registry / factory layer
- implement gravity-driven stacked object collapse and landing
- support hole consumption against physics-backed stack pieces
- build first stack families:
  - crate column
  - log jenga
  - melon pyramid
  - barrel arch
- add clean reset / teardown behavior for physics-backed stacks
- expand stackable object variety after prototype validation
- add break-apart structures that convert larger world objects into smaller collectible debris

## Priority 4 — Powerup Expansion And Strategic Depth

Goal: deepen moment-to-moment decision-making and add more shareable “wow” moments.

Backlog items:

- difficulty toggle
  - `Off` = current baseline values
  - `On` = Nightmare
  - applies to whichever game mode the player starts
  - selection persists until the player changes it or reloads the web app
  - appears above the game-mode choices on the first screen
  - centralize all difficulty-linked values for one-stop balancing
  - difficulty-linked values include:
    - soldier damage
    - soldiers per unit drop
    - aid-drop frequency
    - skyscraper / high-value object density
    - rival AI quality and target selection heuristics
    - rival intelligence / efficiency profiles
  - Nightmare AI should prioritize the best reachable point stream over time rather than simplistic nearest-object chasing
  - rival intelligence / efficiency must be variablized independently from other difficulty knobs so different game setups can field stronger or weaker enemies
  - every game should include at least one meaningfully smarter rival hole than the other two
  - smarter rivals should evaluate reachable object value over time, growth-gating constraints, and route efficiency rather than just local nearest-value opportunities
  - long-term balance target: it must be genuinely possible for the player to lose on points, not just on survival
- Magnet Surge powerup
  - temporarily doubles pull radius
  - strong visual suction moment
- Enemy Freeze powerup
  - freezes rival holes briefly
  - creates tactical gather window
- Gravity Vortex powerup
  - wide spiral pull / orbiting debris
  - standout signature power
- Time Dilation powerup
  - world slows while player remains full speed
  - includes music pitch-shift treatment
- Soldier Hijack powerup
  - soldiers target rivals instead of the player
  - leverages the unique military simulation

## Priority 5 — New Modes And Replayability

Goal: add modes and session structures that materially extend repeat play.

Backlog items:

- Solo 100% Clear mode
  - no rivals
  - timer pressure plus passive size decay
  - final score is percentage of total city mass consumed
  - aid ships drop enhancements more frequently than the standard cadence
- Endless Mode
  - endless escalation rules
  - scaling object density
  - scaling AI pressure
  - scaling hazard intensity
  - endless scoring framing
- camera / POV switch
  - gameplay UI toggle
  - `Off` = current slanted tactical “Diablo” perspective
  - `On` = first-person hover perspective
  - first-person keeps the forward-facing half of the hole visible while the world fills the rest of the frame
  - buildings should feel taller than the player at close range, with roofs and upper floors mostly hidden except at distance
  - object approach should read as natural relative-size growth as the hole moves closer
- procedural layout variation
- challenge variants
- alternative mode rules
- session goals

## Priority 6 — Traversal, Verticality, And Cross-Platform UX

Goal: add richer movement choices, support vertical object placement, and preserve intentional controls across PC and mobile.

Backlog items:

- jump / elevated collection system
  - hole can jump to consume planes and other sensible aerial targets
  - hole can jump to the top of buildings to reach rooftop-only objects
  - jump height should depend on the building being targeted
  - jump may also become an evasion tool against rival holes
  - supports future rooftop content such as people, radio towers, antennas, and similar high-value placements
- score grades
- streaks / combos
- between-wave bonuses or upgrade choices
- cosmetic unlockables
- unified control feel / powerup queue bar
  - same visual location across desktop and mobile
  - keyboard shortcuts on PC
  - intentional activation instead of forced immediate consumption

## Priority 7 — Monetization-Ready Layer

Goal: only after the core game is stable, replayable, and strategically differentiated.

Backlog items:

- cosmetic progression readiness
- account / progression considerations
- store-ready non-intrusive reward structures

## Current Recommendation

1. Finish Priority 1 stabilization
2. Then tackle the hardest content-system investment: the hybrid physics stack and collapse layer
3. Then deepen powerups and mode variety on top of that stronger foundation

The next active engineering task remains:

- P1.6 Refactor reset / restart / wave rebuild behavior
