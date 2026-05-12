## Holesy Product Backlog

This backlog reflects the current project direction, approved gameplay work, the standalone website deployment workflow, and the decision to work on wave-system performance before returning to new gameplay elements.

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

## Completed Foundation — Codebase Stabilization

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

- completed

Progress:

- promoted transition-pause support into `Master 9`
- extracted round-lifecycle UI show / hide behavior into explicit helpers as the first section-boundary cleanup pass
- promoted `Master 10` with validated Waves payoff / wind / timing polish while keeping rollback-safe candidate iteration
- stabilized the soldier-unit-clear feedback loop around explicit helper paths instead of one-off inline reactions
- recent candidate work extracted unit-clear reward math and debug bookkeeping into explicit helper functions instead of leaving the whole path bundled inside one reward routine
- recent candidate work also extracts soldier-consumption audio, score mutation, and wave-roster award bookkeeping into explicit helper functions so the reward path is less entangled with the main consume loop

Outcome:

- the remaining high-friction reward, roster, and local UI/control seams were pulled behind explicit helpers well enough that Priority 1 no longer needs to track section-boundary cleanup as an open backlog item
- further structure work can continue later as normal maintenance or as support work for larger future systems

### P1.8 Add lightweight debug tools

Status:

- completed

Initial target:

- add a low-overhead debug overlay and toggle so balancing passes can inspect live wave, player, and reward state without guessing from feel alone
- validated and promoted the first overlay slice into `Master 11`, including proper cleanup when leaving a run
- validated and promoted `Master 12` with mobile audio startup behavior and shorter unit-clear speed boost tuning
- validated and promoted `Master 13` with player-facing effect timers, player-anchored wave-end warning, bullet-resistance rim thickening, and hole wind-pull ring visualization
- validated and promoted `Master 14` with unit-clear growth retuning so successful clears more reliably refund recent soldier damage and come out net-positive
- validated and promoted `Master 15` with the stable narrow-slice Waves rebuild: transition clarity, transition-safe buff timing, non-music menu audio cleanup, bonus-mass text clarity, and stronger rival aid contesting / score pressure
- recent candidate work extends the debug overlay so unit-clear tuning can show refund-cap, tracked-damage, refund-mass, and post-clear damage-bank details directly during play
- recent candidate work also adds active-input, aid-drop countdown, and live wave-roster summary visibility to the debug overlay so balancing passes can see control ownership and roster state without reading code
- build lineage for candidate testing is now expected to use explicit sub-build numbering beneath the current master (`Master 12.3`, `Master 12.4`, `Master 12.5`, etc.) so defect reports map to one exact candidate

Outcome:

- the project now has a lightweight live-debug surface for reward tuning, input ownership, aid-drop timing, and wave-roster inspection, which satisfies the original low-overhead instrumentation goal for Priority 1
- future debug additions can continue opportunistically without keeping this backlog epic open

## Priority 1 — Wave System Performance

Goal: bound wave-system load, reduce memory churn, and create a durable performance-profile foundation before resuming gameplay expansion.

Status:

- in progress

Working decision:

- performance work temporarily takes priority over new gameplay elements
- once this priority is complete, return to Waves tuning and gameplay-element work
- performance profile and difficulty setting are separate axes:
  - performance profile = what the machine can handle
  - difficulty = how hard the game should push within those caps

Suggested implementation order:

1. `PERF-001`
2. `PERF-003`
3. `PERF-004`
4. `PERF-002`
5. `PERF-005`
6. `PERF-006`
7. `PERF-007`

`PERF-008` remains deferred until heavier unit types are ready. `PERF-009` is an ongoing standing review rule.

### PERF-001 Establish Performance Profile System

Type:

- Foundation / Infrastructure

Priority:

- highest; blocks several other performance tasks

Status:

- in progress; `Master 15.9 - performance-profile-system.html` candidate ready for browser validation

Description:

- create a centralized performance profile system with named tiers such as `low`, `medium`, `high`, and `ultra`
- each tier defines values for every performance-sensitive variable in the game
- the active profile is detected or selected at launch
- every subsystem reads performance-sensitive values from the active profile instead of hardcoded constants

Acceptance criteria:

- a profile object exists with at least `low`, `medium`, `high`, and `ultra` tiers
- every performance-sensitive value is sourced from the active profile, not a hardcoded number
- profile is selected at launch by auto-detection, manual choice, or both
- new performance-sensitive values added in future code default to being profile-controlled

Notes:

- future code reviews will flag hardcoded performance-sensitive constants for promotion into this profile
- first candidate evidence:
  - `20_TESTS/Candidate_Builds/Master 15.9 - performance-profile-system.html`
  - `00_ADMIN/Reviews_and_Reports/QA_REVIEW_MASTER15_9_PERFORMANCE_PROFILE_SYSTEM.md`
  - foundation now covers named launch-selected profiles and routes the first renderer, wave, traffic, people, music, and military knobs through the active tier
  - remaining acceptance work is browser validation plus continued promotion of newly flagged performance-sensitive constants into the profile during later performance slices

### PERF-002 Concurrent Wave Cap With Soft Pressure Valve

Type:

- Performance / Gameplay system

Priority:

- high

Depends on:

- `PERF-001`

Status:

- in progress; `Master 15.25 - remaining-performance-pass.html` candidate ready for validation

Description:

- implement a profile-controlled ceiling for concurrent planes and wave pressure
- when the ceiling is reached, the wave timer pauses rather than firing, resetting, or decrementing
- the timer resumes when active count drops below the ceiling

Acceptance criteria:

- a profile-controlled `MAX_CONCURRENT_PLANES` value gates new wave spawns
- when the cap is reached, the wave timer pauses rather than firing or resetting
- timer resumes when a plane exits the playfield or a wave is fully eaten
- player experience: surviving longer feels like waves arrive faster, but the engine never exceeds its budget

Notes:

- this is a soft pressure valve, not a hard wall; design intent is preserved while engine load is bounded
- the cap is a performance ceiling, not a difficulty cap
- difficulty tuning still controls wave intervals, soldier counts, and accuracy beneath this ceiling
- first candidate evidence:
  - `20_TESTS/Candidate_Builds/Master 15.25 - remaining-performance-pass.html`
  - `00_ADMIN/Reviews_and_Reports/QA_REVIEW_MASTER15_25_REMAINING_PERFORMANCE_PASS.md`
  - performance profiles now define max concurrent planes; wave deployment timer pauses while at the active profile cap

### PERF-003 Geometry Pooling For Wave Units

Type:

- Performance / Memory

Priority:

- high

Status:

- complete for current candidate; user validation passed for `Master 15.23 - wave-unit-geometry-pooling.html`

Description:

- `makePlaneMesh()`, `makeSoldierMesh()`, and `makeParachuteMesh()` currently allocate fresh geometry on every spawn
- this creates GPU upload churn and garbage-collection spikes, and is the likely cause of observed wave-loading hitches
- create wave-unit geometries once at startup and reuse them across all instances

Acceptance criteria:

- geometries for plane parts, soldier parts, parachute canopy, and parachute cords are created once at startup and reused across all instances
- new spawns use shared geometry references; only mesh wrappers and transforms are per-instance
- visual output is unchanged

Notes:

- this is a pure performance win with no intended gameplay change
- pooling now also makes future heavier unit types, such as tanks and jets, cheaper to introduce
- first candidate evidence:
  - `20_TESTS/Candidate_Builds/Master 15.23 - wave-unit-geometry-pooling.html`
  - `00_ADMIN/Reviews_and_Reports/QA_REVIEW_MASTER15_23_WAVE_UNIT_GEOMETRY_POOLING.md`
  - plane, soldier, parachute canopy, and parachute cord geometries now reuse shared geometry references while mesh wrappers remain per spawn
  - user validation passed on 2026-05-11

### PERF-004 Proper Disposal On Unit Cleanup

Type:

- Performance / Memory leak

Priority:

- high

Depends on:

- `PERF-003`

Status:

- complete for current candidate; user validation passed for `Master 15.24 - wave-unit-cleanup-disposal.html`

Description:

- planes, soldiers, and paratroopers are removed from the scene without consistently disposing GPU resources
- tracers already follow the intended disposal pattern
- cleanup must explicitly dispose short-lived per-instance resources when units leave the playfield, are eaten, or convert state

Acceptance criteria:

- all needed geometry and material disposals happen on plane exit, soldier consumption, and paratrooper-to-soldier conversion
- after `PERF-003`, only per-instance resources are disposed; pooled shared geometries are not disposed during normal unit cleanup
- memory profiling over a 5+ minute session shows no unbounded geometry growth

Notes:

- short-lived units should clean up aggressively the moment they leave the playfield
- order matters: pooling lands first so disposal logic correctly distinguishes shared from per-instance resources
- first candidate evidence:
  - `20_TESTS/Candidate_Builds/Master 15.24 - wave-unit-cleanup-disposal.html`
  - `00_ADMIN/Reviews_and_Reports/QA_REVIEW_MASTER15_24_WAVE_UNIT_CLEANUP_DISPOSAL.md`
  - plane exit, soldier consumption, paratrooper landing conversion, wave teardown, and menu cleanup now use one shared wave-unit cleanup helper that protects pooled geometries
  - user validation passed on 2026-05-12

### PERF-005 Reuse Paratrooper Soldier Mesh On Landing

Type:

- Performance / Allocation

Priority:

- medium

Depends on:

- `PERF-003`

Status:

- in progress; `Master 15.25 - remaining-performance-pass.html` candidate ready for validation

Description:

- when a paratrooper lands, the code removes the full paratrooper mesh and creates a new soldier mesh
- instead, reparent the existing soldier mesh from the paratrooper group to the scene root

Acceptance criteria:

- paratrooper-to-soldier transition reparents the existing mesh
- no new soldier mesh allocation occurs at landing
- parachute mesh is the only thing disposed at landing

Notes:

- this becomes simpler after `PERF-003` because soldier mesh structure is already pooled
- first candidate evidence:
  - `20_TESTS/Candidate_Builds/Master 15.25 - remaining-performance-pass.html`
  - `00_ADMIN/Reviews_and_Reports/QA_REVIEW_MASTER15_25_REMAINING_PERFORMANCE_PASS.md`
  - paratrooper landing now reparents the existing descending soldier mesh instead of allocating a new soldier mesh

### PERF-006 Throttle Per-Plane Engine Audio Updates

Type:

- Performance / Audio

Priority:

- medium

Depends on:

- `PERF-001`

Status:

- in progress; `Master 15.25 - remaining-performance-pass.html` candidate ready for validation

Description:

- `updatePlaneEngineAudio` currently runs at 60Hz per plane and makes multiple audio API calls per plane per frame
- throttle updates to a profile-controlled rate, defaulting around 10Hz, with no audible behavior change

Acceptance criteria:

- engine audio updates execute at a profile-controlled rate, default around 10Hz
- no audible change in engine drone behavior at any plane count
- per-frame audio API call count is reduced, especially with multiple planes active

Notes:

- update frequency should be profile-controlled so low-end hardware can reduce it further
- first candidate evidence:
  - `20_TESTS/Candidate_Builds/Master 15.25 - remaining-performance-pass.html`
  - `00_ADMIN/Reviews_and_Reports/QA_REVIEW_MASTER15_25_REMAINING_PERFORMANCE_PASS.md`
  - performance profiles now define the plane-engine audio update rate; movement remains per-frame while audio API updates are throttled

### PERF-007 Clear waveRosters On Game End

Type:

- Cleanup / Minor leak

Priority:

- low

Status:

- in progress; `Master 15.25 - remaining-performance-pass.html` candidate ready for validation

Description:

- `waveRosters` entries are only deleted when a wave is fully eaten
- partially eaten waves at game end can leave orphaned entries that persist across runs

Acceptance criteria:

- `waveRosters` is cleared when the game ends
- verified that no roster entries persist between runs

Notes:

- first candidate evidence:
  - `20_TESTS/Candidate_Builds/Master 15.25 - remaining-performance-pass.html`
  - `00_ADMIN/Reviews_and_Reports/QA_REVIEW_MASTER15_25_REMAINING_PERFORMANCE_PASS.md`
  - `waveRosters` now clears through a shared helper on reset, wave teardown, and game end

- this is a small leak but cheap to address alongside the broader cleanup pass

### PERF-008 Future Review: Weighted Pressure Budget For Mixed Unit Types

Type:

- Deferred / Design hook

Priority:

- deferred; revisit when heavier units are introduced

Status:

- deferred

Description:

- when heavier unit types such as tanks or missile-firing jets are added, evaluate whether the concurrent cap should become a weighted budget
- under a weighted budget, each unit type costs a different amount against one profile-controlled ceiling

Acceptance criteria when revisited:

- decision documented: weighted budget, per-type caps, or hybrid
- if weighted, each unit type has a defined cost and the cap is profile-controlled
- profiling data informs the decision

Notes:

- per-frame active behavior is the right unit of weight: AI ticks, collision checks, projectile spawning, and similar active costs
- disposal-on-exit handles duration; the budget handles peak load
- do not implement a weighted system speculatively

### PERF-009 Standing Practice: Flag Hardcoded Performance Constants

Type:

- Process / Standing review rule

Priority:

- ongoing

Status:

- ongoing

Description:

- every future code review must flag hardcoded values that affect performance for promotion into the performance profile system
- examples include counts, ranges, intervals, distances, quality settings, audio voice limits, particle counts, and draw distance

Acceptance criteria:

- each performance review includes a `hardcoded constants flagged` section
- flagged constants are tracked as backlog items for promotion
- new code submitted after `PERF-001` lands defaults to profile-controlled values

Notes:

- this is a standing instruction for the performance-review role, not a one-time task

## Priority 2 — Waves Mode Completion and Tuning

Goal: finish the mode already in flight and make it feel deliberately paced.

Status:

- paused behind active Priority 1 performance work

Progress:

- `Master 14.4 - waves-narrative-and-pacing-pass.html` starts the Priority 2 pass from the approved narrative pack rather than ad hoc flavor text
- that candidate replaces wave intro banners, threat briefings, transition copy, and Waves-specific end-state messaging with the agreed containment / Parallax framing
- the same candidate lengthens briefing and transition dwell times and retunes per-wave durations / roster cadence toward a more deliberate escalation curve
- `Master 14.5 - ai-rivals-aid-search-and-score-pressure.html` explored the right design targets, but the line failed validation due to a blocker startup regression and is not the active base
- `Master 14.6 - rollback-to-14.4-stable-baseline.html` restores the last known working Waves candidate so testing and incremental rebuild can continue safely
- `Master 14.7 - transition-freeze-and-outcome-clarity.html` is the first narrow rebuild slice from the stable base and targets transition readability, transition-safe buff timing, and clearer Waves loss messaging
- `Master 14.8 - non-music-menu-audio-cleanup.html` is the next narrow rebuild slice and targets post-round / menu audio shutdown without reopening startup-path risk
- `Master 14.9 - powerup-text-clarity.html` is the next narrow rebuild slice and targets player-readable alien-drop text for the non-temporary mass pickup
- `Master 14.10 - rival-aid-and-score-pressure.html` is the next narrow rebuild slice and targets stronger rival aid contesting plus better score pressure without reintroducing the failed `14.5` startup-path risk
- `Master 14.10` has now been validated and promoted into `Master 15`
- `Master 15.1 - wave1-speed-burst-tuning.html` is the next candidate slice from the approved `Master 15` baseline and targets the remaining concern that one Speed Burst aid in Wave 1 may still be too decisive
- `Master 15.2 - aid-intel-and-rival-pressure.html` is the next candidate slice and targets the remaining observation that grounded paralax aid can still go uncontested and that rival scores still trail too far behind the player
- `Master 15.3 - score-compression-pass.html` is the next candidate slice and targets the remaining leaderboard-gap problem after `15.2` improved rival aid contesting
- `Master 15.4 - aid-cooldown-and-ai-smoothing.html` is the next candidate slice and targets the `15.3` follow-up issues: aid monopolization by one rival, post-aid AI stutter, and an initially too-robotic feel
- `Master 15.5 - transition-countdown-and-proof-of-life.html` is the next candidate slice and targets the remaining issue that intentional Waves transition delay still feels like a lockup
- `Master 15.6 - transition-camera-and-soldier-escape.html` is the next candidate slice and targets restoration of the liked transition wide-camera feel plus a defect where rivals can linger under concentrated soldier fire
- `Master 15.7 - predrop-aid-intel-fix.html` is the next candidate slice and targets a confirmed unfair defect where AI can wait on the exact future aid landing spot before the drop happens
- `Master 15.8 - aid-stutter-camera-and-mid-ai-buff.html` is the next candidate slice and targets post-aid loser stutter, a transition camera that rose too high, and slightly underpowered non-Gulp rivals
- `Master 15.9 - performance-profile-system.html` starts `PERF-001` from the approved `Master 15` baseline and adds launch-selected performance tiers before the remaining wave-system performance work
- `Master 15.10 - wave-end-countdown-audio.html` adds a 3-second audible countdown before each active Waves round timer reaches zero
- `Master 15.11 - audio-state-cleanup.html` responds to failed `15.10` audible validation by strengthening the countdown cue and gating aid, soldier, and plane sounds to active gameplay only
- `Master 15.12 - countdown-audio-proof.html` responds to failed `15.11` validation by routing the countdown cue directly to the audio destination and adding visible/debug proof whenever 3, 2, or 1 fires
- `Master 15.13 - race-light-countdown-audio.html` responds to `15.12` sound-design feedback by keeping the proven cue trigger/proof path but changing the audio to three identical race-start-light style beeps
- `Master 15.14 - synced-race-light-countdown.html` responds to `15.13` timing feedback by increasing beep volume and triggering the cue from the same HUD update that displays `0:03`, `0:02`, and `0:01`
- `Master 15.15 - new-wave-text-and-audio-stop.html` changes countdown proof text to `NEW WAVE IN X` and hardens menu/game-over cleanup so active non-music samples stop immediately
- User validation passed for `Master 15.15`: countdown timing, race-light audio, `NEW WAVE IN X` text, and non-music audio cleanup all passed
- `Master 15.16 - selectable-difficulty.html` adds a gameplay difficulty dropdown above game-mode selection with `Normal`, `Hard`, and `Ultra`; the setting persists and tunes soldier pressure, aid timing, and rival AI efficiency separately from the performance profile
- `Master 15.17 - difficulty-descriptions.html` adds explanatory text below the difficulty selector and updates it when the user changes difficulty
- User validation passed for `Master 15.17`: difficulty description placement/copy passed
- `Master 15.18 - ultra-difficulty-tuning.html` responds to playtest feedback that the three difficulty levels were not discernible by making `Hard` and especially `Ultra` materially stronger; `Ultra` now starts troop pressure in Wave 1 and scales soldier count, cadence, damage, hit chance, aid scarcity, and rival AI
- `Master 15.19 - difficulty-asset-scarcity.html` adds difficulty-scaled food scarcity: harder settings have fewer buildings, people, cars, props, park assets, and fewer skyscrapers relative to other buildings
- `Master 15.20 - rival-ai-difficulty-scaling.html` responds to playtest feedback that rivals still score too poorly and visibly dither by making higher-difficulty rivals faster, less random, more committed, better at value-stream routing, more willing to contest objects, and more decisive when chasing
- User validation passed for `Master 15.20` with continuing observation: `Ultra` is obviously harder, and rival AI improvements are accepted for continued tuning
- `Master 15.21 - difficulty-hole-eat-growth.html` adds difficulty-scaled rival-consumption rewards so easier modes grow more from eating another hole while harder modes grow less
- `Master 15.22 - player-eaten-return.html` adds a five-second post-consumption spectator window, fades to black, and returns to game-mode selection after the player is eaten
- User validation passed for `Master 15.22`: player-eaten fade-to-black return flow works perfectly
- `Master 15.23 - wave-unit-geometry-pooling.html` starts `PERF-003` by pooling plane, soldier, parachute canopy, and parachute cord geometries for wave units
- User validation passed for `Master 15.23`: wave-unit geometry pooling preserved visuals and behavior
- `Master 15.24 - wave-unit-cleanup-disposal.html` starts `PERF-004` by disposing per-instance wave-unit resources while protecting pooled geometries
- User validation passed for `Master 15.24`: wave-unit cleanup/disposal looked good
- `Master 15.25 - remaining-performance-pass.html` addresses the remaining active performance backlog by adding the profile plane cap, paratrooper soldier-mesh reparenting, plane-engine audio throttling, and game-end roster clearing
- `Master 15.26 - hole-wind-audio-disabled.html` disables the hole swirling wind audio by configuration while preserving the code for later redesign
- `Master 15.27 - difficulty-parachute-drop-time.html` makes paratrooper fall time difficulty-scaled: Normal baseline, Hard faster, Ultra fastest
- User validation passed for `Master 15.27`: 15-game run produced a reasonable three-layer difficulty pattern, with Normal 100%, Hard 60%, and Ultra 20% win rates

Backlog items:

- redesign the hole swirling wind audio so it sounds more natural, less anxious, and less obtrusive before re-enabling it
- validate that `15.25` passes the remaining performance test set: plane cap pause/resume, paratrooper mesh reparent, engine-audio throttle, game-end roster cleanup, and no console errors
- validate that `Normal` preserves the current baseline feel
- validate that `15.21` keeps `Ultra` materially harder to survive than `Normal` while still beatable
- validate that higher difficulty makes targets harder to find and high-value towers rarer
- validate that higher difficulty makes rival holes score and route meaningfully better
- validate that hole-eat score/radius rewards scale down on harder difficulties
- continue fine-tuning wave duration, cadence, and roster pressure
- confirm the final difficulty curve feels fair on desktop and mobile
- improve progression readability and signaling where still needed
- validate the final-wave active-arena pressure model against repeat play
- validate whether one-aid-drop speed access is still too strong in Wave 1 even after rival contest pressure improves
- validate whether the `15.1` Waves-specific Speed Burst tuning is enough, or whether Wave 1 aid needs further softening
- validate whether `15.2` shared-but-imperfect aid intel plus stronger rival pressure produces more believable aid contests and tighter leaderboards
- validate whether `15.3` materially compresses leaderboard gaps without making rivals feel unfair
- validate whether `15.4` keeps the tighter leaderboard while reducing robotic behavior and repeated aid monopolies
- validate whether `15.5` makes the Waves transition feel intentionally staged instead of frozen
- validate whether `15.6` restores the transition camera feel and improves soldier-pressure escape behavior
- validate whether `15.7` removes exact pre-drop aid prediction while preserving post-landing aid contesting
- validate whether `15.8` removes stale-aid stutter, restores the lower transition camera feel, and lifts Void/Maw competitiveness
- future design note: randomize wave-start hole spawn positions while preventing holes from spawning too close to each other
- validate whether the stronger rival routing meaningfully compresses the end-of-run leaderboard without making survival feel cheap
- validate that all non-music sounds are silent on scoreboard and mode-select screens while music continues normally
- validate that rivals search for alien aid with intent but without feeling omniscient
- continue tuning the Waves difficulty curve from the now-approved `Master 15` baseline
- validate the `14.8` audio slice before reintroducing AI competition changes
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

Goal: deepen moment-to-moment decision-making and add more shareable "wow" moments.

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

## Priority 5 — New Modes And Replayability

Goal: add modes and session structures that materially extend repeat play.

Backlog items:

- Solo 100% Clear mode
  - no rivals
  - timer pressure plus passive size decay
  - final score is percentage of total city mass consumed
  - aid ships drop enhancements more frequently than the standard cadence
- Endless Mode
  - use the same core wave system as `Waves`
  - remove the automatic stop at the end of `4/4`
  - difficulty should continue scaling upward wave after wave with config-driven tuning
  - treat the mode as conceptually unbounded `N` waves, not a fixed cap, even if early versions only tune the first several dozen well
  - the same AI pressure knobs used in current Waves tuning should remain variablized so Endless can progressively tighten:
    - rival aid-drop interest / search behavior
    - rival object-routing quality / scoring efficiency
    - rival flee intelligence / survivability
  - long-term target: players are chasing their own best wave reached and best score, not merely seeing a finite ending
- Persistent records and run-history surface
  - retain and recall best score, best Waves round reached, and future Endless round milestones
  - celebrate new personal bests with explicit UI feedback such as `You broke a record`
  - expose remembered targets on the front-end so replay goals stay visible between sessions
- Achievements / milestone tracking
  - retain accomplishment state and show what has been completed versus what remains
  - candidate examples:
    - reach Wave 10 / 20 / 30
    - consume large cumulative totals such as `100000 humans eaten`
  - celebration and recall are both required; achievements should improve replayability, not just trigger one-time popups
- Menu and UI audio feedback
  - add lightweight sonic feedback for menu and control interactions
  - candidate examples from live direction:
    - short alien chirp when a game mode is selected
    - coin-drop sound when `Begin` is clicked
    - sharp braking sound when pause is triggered
  - keep these sounds distinct from gameplay-only SFX and make sure they obey title / menu audio-state rules cleanly
  - scaling object density
  - scaling AI pressure
  - scaling hazard intensity
  - endless scoring framing
- camera / POV switch
  - gameplay UI toggle
  - `Off` = current slanted tactical "Diablo" perspective
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

1. Finish the new Priority 1 wave-system performance backlog.
2. Return to Priority 2 Waves tuning and readability validation on the latest candidate.
3. Then tackle the hardest content-system investment: the hybrid physics stack and collapse layer.
4. Then deepen powerups and mode variety on top of that stronger foundation.

The next active engineering task remains:

- validate the `PERF-001` candidate (`Master 15.9 - performance-profile-system.html`), then continue the remaining Priority 1 wave-system performance backlog
