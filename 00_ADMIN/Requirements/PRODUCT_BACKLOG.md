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

- completed

Outcome:

- guarded delayed wave-start handoff so exiting, restarting, or reaching game over cannot accidentally resume a pending next wave
- promoted the validated fix into `Master 8`

### P1.7 Strengthen section boundaries / module-like organization

Status:

- in progress

Progress:

- promoted transition-pause support into `Master 9`
- extracted round-lifecycle UI show / hide behavior into explicit helpers as the first section-boundary cleanup pass
- promoted `Master 10` with validated Waves payoff / wind / timing polish while keeping rollback-safe candidate iteration
- stabilized the soldier-unit-clear feedback loop around explicit helper paths instead of one-off inline reactions

### P1.8 Add lightweight debug tools

Status:

- in progress

Initial target:

- add a low-overhead debug overlay and toggle so balancing passes can inspect live wave, player, and reward state without guessing from feel alone
- validated and promoted the first overlay slice into `Master 11`, including proper cleanup when leaving a run
- validated and promoted `Master 12` with mobile audio startup behavior and shorter unit-clear speed boost tuning
- validated and promoted `Master 13` with player-facing effect timers, player-anchored wave-end warning, bullet-resistance rim thickening, and hole wind-pull ring visualization
- validated and promoted `Master 14` with unit-clear growth retuning so successful clears more reliably refund recent soldier damage and come out net-positive
- build lineage for candidate testing is now expected to use explicit sub-build numbering beneath the current master (`Master 12.3`, `Master 12.4`, `Master 12.5`, etc.) so defect reports map to one exact candidate

## Priority 2 — Waves Mode Completion and Tuning

Goal: finish the mode already in flight and make it feel deliberately paced.

Backlog items:

- continue fine-tuning wave duration, cadence, and roster pressure
- confirm the final difficulty curve feels fair on desktop and mobile
- improve progression readability and signaling where still needed
- validate the final-wave active-arena pressure model against repeat play
- use the shared narrative reference before changing wave lore or pacing:
  - [Waves Narrative And Text Pack](WAVES_NARRATIVE_AND_TEXT_PACK.md)
- define the canonical Waves story concept so escalation feels intentional rather than decorative
  - recommended framing: the holes are part of a spreading extradimensional feeding event rather than ordinary sinkholes
  - recommended battlefield logic: the district keeps rebuilding or repopulating between waves because the `Parallax` is reconstituting matter inside the active breach zone
  - recommended escalation ladder:
    - Wave 1 = normal city life caught off guard
    - Wave 2 = first organized containment and troop deployment
    - Wave 3 = evacuation, hard containment, and more lethal resistance
    - Wave 4 = terminal district collapse and full crackdown
  - recommended tone target: sci-fi disaster with ominous military escalation
- replace current wave intro / transition copy with sharper lore-consistent messaging
  - avoid vague terms that do not clearly map to the fiction or gameplay stakes
  - each wave message should explain what changed in the city response, not just add flavor text
  - each between-wave message should reinforce why the battlefield is repopulated and why the response is escalating

## Priority 3 — Physics Stack And Collapse System

Goal: introduce `hole.io`-style stacked-object variety with convincing gravity-driven collapse while preserving the current battlefield systems.

Backlog items:

- use the shared tank/destructible-building concept before scoping military heavy-unit or building-damage work:
  - [Tanks And Destructible Buildings Concept](TANKS_AND_DESTRUCTIBLE_BUILDINGS_CONCEPT.md)
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
- explore tank units as a future military escalation layer
  - longer range than soldiers
  - dramatically higher radial damage than soldiers
  - slower reload than soldiers, with much larger per-shot damage and a distinct DPS profile
  - establish reload speed as an explicit variable for every offensive combat unit type
  - able to fire through buildings
  - road-limited navigation
  - movement speed slightly faster than soldiers
  - destroys cars on contact
  - able to reduce building score value by damaging structures before the player consumes them
  - fifth qualifying building hit destroys the structure
  - future stretch goal: shell impacts can blow visible building chunks outward, leave temporary zero-value debris on the ground, then flash and disappear

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
  - leaderboard compression needs dedicated tuning; current playtests show the player can finish several multiples above second place even in runs where the player deliberately sacrifices scoring time to test military interactions
  - investigate stronger rival early/mid-game intake, denser high-value routing, and possible trailing-AI catch-up pressure so Waves can feel like a real score race as well as a survival mode
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
- geographic combo and chain system
  - eating multiple objects in continuous succession along a city path awards escalating score multipliers
  - chain counter visible in HUD during an active chain
  - multiplier scales with chain length: 1.2x at 5, 1.5x at 10, 2x at 20
  - chain breaks on backtracking, idle timeout (>2s between eats), or soldier damage
  - cross-category chain (person → prop → car → building within 5s) awards a rare bonus or temporary ability
  - cross-category combo triggers distinct visual and audio feedback
  - works in all game modes
- contextual eat mechanics
  - eat outcomes vary based on the state of the target at the moment of consumption
  - moving cars transfer momentum (speed boost); parked cars give bonus score; crashing cars trigger collapse chains
  - people who have spotted the player and are fleeing are worth more than people caught off-guard
  - buildings collapsing near other buildings can chain-collapse
  - each contextual state has distinct visual or audio feedback so the player learns the system
  - reuse existing audio bank where possible
- active abilities / size-spending system
  - player can trigger 2-3 active abilities during play, each costing a tunable percentage of current hole size
  - proposed initial abilities: short dash (positional), panic pulse (scares targets in radius), target mark (doubles next eat value within 5s)
  - cooldowns prevent spam
  - mobile and desktop input both supported: tap zones on mobile, keyboard shortcuts on desktop
  - HUD shows current ability availability and size cost
  - each ability has distinct visual and audio feedback
- soldier behavior depth
  - soldiers in formation are more dangerous than isolated soldiers (group buff or visual indicator)
  - reload state is visually distinguishable and exploitable: eating mid-reload grants score or ability bonus
  - soldiers actively targeting AI rivals temporarily do not retarget the player
  - a soldier who witnesses a squadmate being eaten panics and drops their weapon, which becomes an environmental hazard or score pickup
  - all behaviors layer cleanly on top of the existing wave system
- rival hole asymmetric mechanics
  - each rival has one mechanically distinct trait beyond movement personality
  - proposed traits:
    - Void: leaves a small-hole trail that briefly traps the player
    - Maw: emits a shockwave when eating large objects, knocking rivals back
    - Gulp: detects the player during active chains and hunts them specifically
  - traits visibly change how the player approaches each rival
  - traits scale appropriately by wave and difficulty
  - player can recognize each rival's trait within 1-2 encounters
  - traits respect the existing personality framework: Void = predictable, Maw = circles, Gulp = feints
- mid-run risk/reward beacons
  - occasional beacon events appear on the map during waves, offering high-risk high-reward detours
  - examples: dense soldier squad guarding a large bonus, a giant target that fights back, a timed challenge rewarding a score multiplier
  - at least 3 beacon event types implemented
  - beacons appear at semi-predictable intervals with clear visual indicators and risk/reward telegraphing
  - engaging is always optional and never forced; rewards are meaningful but not mandatory for progression

## Priority 5 — New Modes And Replayability

Goal: add modes and session structures that materially extend repeat play.

Backlog items:

- Solo 100% Clear mode
  - no rivals
  - timer pressure plus passive size decay
  - final score is percentage of total city mass consumed
  - aid ships drop enhancements more frequently than the standard cadence
- Endless Mode
  - new mode option in the mode picker
  - uses the same core wave system as `Waves`; removes the automatic stop at the end of `4/4`
  - difficulty ramps on a fixed timer (every 60-90 seconds) with a visible warning before each ramp
  - at least 5 distinct escalation tiers: more soldiers, faster rivals, smaller board, weather or visibility effects
  - treat the mode as conceptually unbounded `N` waves, not a fixed cap, even if early versions only tune the first several dozen well
  - scaling object density, AI pressure, and hazard intensity
  - score and survival time persisted to localStorage
  - run ends only on player death
  - achievements and milestones tied to specific endless thresholds
- Endless Mode unlocks and milestones
  - 10+ milestone achievements defined (examples: survive 10 minutes, reach the 3rd ramp, eliminate 200 soldiers in a run)
  - each milestone unlocks something tangible: starting modifier, ability slot, or cosmetic
  - unlocks persist via localStorage
  - unlock progress is visible to the player
  - at least half of unlocks are gameplay-affecting, not purely cosmetic
  - depends on Endless Mode being live
- lore drip system
  - lore fragments unlock through gameplay milestones: run count, achievements, rare in-run events
  - 15-20 initial fragments written and documented before implementation begins
  - narrative direction: holes are transporting things to an unknown destination, or are something ancient reclaiming what was theirs; avoid political framings and generic alien invasion
  - fragments are short, environmental, and avoid exposition dumps
  - fragments viewable from the main menu
  - fragment storage and display system implemented in-game
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
- daily seed leaderboard
  - all players receive the same procedurally generated city, rivals, and wave config for a 24-hour period
  - seed rotates at a fixed UTC time
  - score submission to a hosted leaderboard backend (Dreamlo or Cloudflare Workers + KV, target $0/month at expected volumes)
  - leaderboard viewable in-game with personal best for the current seed displayed
  - anti-tamper consideration acceptable to defer for v1
  - depends on geographic combo, contextual eat, and active abilities being live
- meta-progression currency and unlock tree
  - persistent currency earned from runs based on score, combos, and milestones
  - persisted to localStorage
  - unlock tree with at least 20 nodes
  - first unlock reachable within 2-3 runs; late unlocks require 50+ runs
  - spent currency is not refundable
  - all unlocks affect gameplay or visibly change the experience
  - depends on Endless Mode being live
- playable rival holes
  - player can select Void, Maw, or Gulp as their playable hole instead of the default
  - each playable rival uses its asymmetric trait as a passive ability
  - selection UI integrated into the mode picker
  - unlocked through meta-progression or specific achievements
  - per-rival mastery tracking: runs played, best scores
  - depends on rival hole asymmetric mechanics and meta-progression being live

## Current Recommendation

1. Finish Priority 1 stabilization
2. Then tackle the hardest content-system investment: the hybrid physics stack and collapse layer
3. Then deepen powerups and mode variety on top of that stronger foundation

The next active engineering task remains:

- P1.6 Refactor reset / restart / wave rebuild behavior
