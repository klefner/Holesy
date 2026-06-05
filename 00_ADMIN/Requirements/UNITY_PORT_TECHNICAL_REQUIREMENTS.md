# Unity Port Technical Requirements And Architecture

Date: 2026-06-05

Status: Draft for external AI implementation

Source basis: `Master 16.52`

Intended implementer: Unity engineering agent or team, including Claude

## Purpose

This document defines the technical requirements, architecture, key decisions, and implementation constraints for rebuilding Downtown Devour / Holesy in Unity with better graphics, sound, physics, and runtime performance.

The Unity version should not be a literal browser-code translation. It should preserve the approved product design, gameplay rules, lore systems, and feel targets while using Unity-native architecture, rendering, asset pipelines, profiling, and save systems.

## Current Source Of Truth

The governed browser implementation is the current reference for game rules and feature behavior:

- Source folder: `10_SOURCE/Masters/Master 16/`
- Current approved build label: `Master 16.52`
- Current modular source files:
  - `index.html`
  - `how-to-play.html`
  - `css/styles.css`
  - `js/main.js`
  - `js/build-info.js`
  - `js/difficulty-profiles.js`
  - `data/lore-documents.js`
  - `assets/images/`
  - `assets/audio/`

The Unity implementation should treat these files as reference material, not as the target architecture.

## Product Vision

Downtown Devour is an arcade action game where the player is a hungry hole in downtown Aldine. The player moves through a city, devours anything small enough to fit, grows larger, competes against rival holes, survives government containment, discovers lore, unlocks achievement buffs, and chases higher scores and deeper Endless Waves.

Tone:

- Funny and whimsical on the surface
- Conspiracy-theory undertones
- Light horror through bureaucracy, missing places, and impossible records
- Fast, readable arcade action first

The Unity version should make the city feel more physical, more reactive, and more alive than the browser build while preserving clear gameplay readability.

## Non-Negotiable Product Decisions

1. The player is a hole, not a character riding or controlling a creature.
2. The core verb is devour: move, position the hole, swallow eligible objects, grow, score, and survive.
3. Smaller objects can be eaten. Larger objects cannot be eaten until the hole is large enough.
4. Rival holes are active competitors. They can eat objects, grow, eat the player, and be eaten by the player when smaller.
5. Soldiers and containment systems can damage the player and eventually kill the player.
6. Endless Waves has no win condition. It ends only when the player voluntarily exits through pause/cashout/save flow, or the player is eaten/killed.
7. In Endless Waves, rival holes eaten by the player respawn smaller and away from the player. Endless does not run out of rivals.
8. Every fifth Endless wave resets all holes to starting size. This is the future hook for world/theme transitions.
9. Timed buffs cannot refresh their own timer while active. After expiry, the same buff has a five-second reacquire cooldown.
10. Documents are rare, capped to 0-1 eligible document per round, and never awarded on losses.
11. Lore is playable: the Archive and found documents hint at secret achievement buffs.
12. Starter field patterns are visible from the beginning and should be understandable without reading the full lore corpus.
13. Medium office buildings use a voxel/stacked-cube collapse model.
14. Skyscrapers use a distinct large-chunk collapse model, not the medium-office cube model.
15. The classic hole visual baseline is a readable black circular mouth with a colored rim. Depth visuals may be improved in Unity only if they preserve object visibility while swallowing.
16. Future world/theme packs must not require rewriting core consumption, scoring, AI, or wave logic.

## Target Platforms

Primary target:

- Windows desktop

Secondary targets:

- WebGL build if performance remains acceptable
- Mobile browser or mobile native build as future targets
- Controller/gamepad support from the start

The Unity project must be architected so target-specific input, haptics, graphics quality, and performance profiles can vary without changing game rules.

## Recommended Unity Stack

Unity version:

- Use current Unity LTS.

Rendering:

- Use URP for broad device support and scalable graphics.
- Use a top-down or tilted orthographic camera by default.
- Consider perspective only if readability stays strong.

Physics:

- Use Unity Physics/Rigidbody selectively for hero interactions and large visible collapses.
- Use custom lightweight physics for high-count medium-office cubes and debris where full Rigidbody simulation is too expensive.
- Use object pooling for all frequently spawned objects, particles, audio emitters, bullets, tracers, cars, soldiers, cubes, and collapse chunks.

Audio:

- Use AudioMixer groups for Music, SFX, UI, Collapse, Vehicle, Soldier, Archive, and Master.
- Use event budgeting so many falling cubes do not create static-like overlapping noise.

Data:

- Use ScriptableObjects for stable design-time definitions.
- Use JSON or Unity-addressable data files where live tuning, external editing, or future downloadable content is likely.

Assets:

- Use prefabs and Addressables or equivalent asset grouping.
- Use stable asset keys, not filenames, as gameplay references.

## Core Game Loop

The game loop should support:

1. Load selected game mode, difficulty, performance profile, settings, and unlocked progression.
2. Generate or load the city board.
3. Spawn player, rivals, objects, cars, people, buildings, props, soldiers, aid, and wave state.
4. Run real-time simulation:
   - player input
   - rival AI
   - object eligibility
   - devouring
   - growth and scoring
   - containment pressure
   - buffs and cooldowns
   - collapse physics
   - audio/VFX
5. Resolve mode-specific end or transition conditions.
6. Persist stats, achievements, best scores, best Endless wave, discovered documents, and optional Endless save state.
7. Return to score/menu/archive/help surfaces.

## Game Modes

### Timed

Two-minute score chase. Highest score wins when the timer ends.

End states:

- Player ranks first at time expiry: win
- Rival ranks first at time expiry: loss
- Player is eaten/killed: loss

### Last Man Standing

No timer. The goal is to survive until only one hole remains.

End states:

- Player is last hole standing: win
- Player is eaten/killed: loss

### Waves

Four progressively harder waves. Score accumulates. Standard Waves keeps a finite endpoint.

End states:

- Player survives/completes Wave 4 according to browser rules: win
- Player is eaten/killed: loss

### Endless Waves

Unbounded wave mode. There is no winning by clearing rival holes and no final wave.

Rules:

- Wave number increases forever.
- Difficulty scales gently from early waves toward roughly current Ultra Wave 4 pressure around wave 70-80.
- After that point, difficulty continues rising under performance ceilings.
- Rival holes respawn smaller and away from the player when eaten.
- Every fifth wave resets every hole to starting size and resets live land score.
- Player can voluntarily exit/cash out from pause.
- Player can save and later load an Endless run.

End states:

- Player voluntarily exits/cashouts: survival/withdrawal state, not a win
- Player is eaten/killed: loss
- Eating all rival holes must not end the mode

## Difficulty And Scaling

Difficulty settings:

- Normal
- Hard
- Ultra

Difficulty affects:

- soldier count
- soldier damage
- soldier hit chance
- soldier speed
- soldier drop cadence
- paratrooper fall time
- aid timing and scarcity
- rival AI scan speed
- rival decision speed
- rival route quality
- rival aggression
- rival flee quality
- object scarcity
- skyscraper/high-value object density
- score/growth compression

Endless scaling:

- Use a generated pressure profile per wave.
- Wave 1 starts near Normal Wave 1.
- Wave 70-80 should approximate current Ultra four-wave pressure.
- Wave 100+ should be extremely difficult but still technically playable.
- Difficulty growth and performance ceilings are separate concerns.
- Do not spawn more active agents or debris than the active performance profile can handle.

## Performance Profiles

Unity must include performance profiles independent from game difficulty.

Suggested profiles:

- Low
- Medium
- High
- Ultra

Performance profile controls:

- max active soldiers
- max active planes
- max active cars
- max active pedestrians
- max active physics cubes
- max active collapse chunks
- max active particles
- max active audio voices by category
- AI update frequency
- physics/contact update budget
- VFX quality
- shadow quality
- draw distance / LOD thresholds
- mobile-specific caps

Game difficulty should never directly exceed performance profile ceilings.

## Simulation And Rendering Boundaries

The Unity project must separate simulation state from rendering.

Simulation owns:

- mode state
- wave state
- timers
- score
- growth
- hole size and position
- rival state
- soldier state
- cars and panic state
- object states
- building piece states
- buff state
- achievement progress
- document reward eligibility
- save/load snapshots

Rendering owns:

- scene objects
- mesh renderers
- animations
- particles
- trails
- UI presentation
- audio emitters
- camera effects
- interpolation
- visual-only debris

Save files must serialize simulation state, not Unity scene object references.

## Scene Architecture

Recommended scenes:

- `Boot`: load settings, services, registries, data definitions
- `MainMenu`: mode selection, difficulty, stats, archive, help, load Endless
- `Game`: active playfield
- `Archive`: optional separate scene or overlay for found documents
- `HowToPlay`: optional overlay/popup content

If using additive scenes:

- Keep shared services in a persistent bootstrap object.
- Load playfield theme scenes additively.
- Unload inactive theme assets to protect memory.

## Core Systems

### Game State Manager

Responsibilities:

- mode lifecycle
- transitions
- pausing
- game over
- wave advancement
- voluntary Endless cashout
- save/load calls

### Board Generator

Responsibilities:

- generate downtown blocks and roads
- place buildings, props, people, cars, parks, and skyscrapers
- support non-grid future themes
- expose spawn budgets based on difficulty and performance profile

### Theme Registry

Responsibilities:

- current downtown theme
- future sci-fi, hellscape, wild west, medieval, space, prehistoric, cartoon, black-and-white, and modified downtown themes
- theme-specific object families
- theme-specific terrain/path rules
- theme-specific sounds/music
- theme-specific visual palette
- theme-specific spawn budgets

Theme swaps must not require rewriting core devour, score, AI, save, or buff logic.

### Hole Controller

Responsibilities:

- movement
- input action consumption
- collision radius and rim state
- devour eligibility
- swallow/fall visual origin
- growth
- damage and death
- active buffs
- rival/player shared hole behavior

### Devour System

Responsibilities:

- determine if object fits
- initiate falling/swallowing state at the mouth-contact point
- keep swallowed objects visually inside the live hole mouth
- award points/growth only when final devour completes
- apply object-specific consequences
- update achievements and buff triggers

Important visual rule:

- Objects should look like they fall into and down the hole, not snap to a center point.
- Objects enter at the point where they touch the hole mouth.
- Their visible fall should be clipped/masked to the visible hole mouth.
- If the hole moves away, the object should not visibly keep falling on the street.

### Growth System

Responsibilities:

- convert object consumption into hole size growth
- apply score/growth compression by difficulty
- temper rival-hole growth rewards
- enforce Endless radius caps between fifth-wave resets
- reset holes every fifth Endless wave

### AI Rival System

Responsibilities:

- rival target scanning
- route selection
- aid competition
- chase/flee logic
- eating smaller objects and holes
- getting eaten
- Endless respawn smaller and far from player

Rivals should have recognizable names and player memory support in future progression.

### Military/Containment System

Responsibilities:

- soldier/paratrooper spawn cadence
- soldier pursuit and firing
- bullet/tracer simulation
- damage and shrink/kill logic
- planes and deployment warnings
- aid-drop competition
- future poison-pill/containment-bait hazards

### Traffic System

Responsibilities:

- lane-following cars
- panic behavior near holes
- crashes
- stopped wrecks
- occasional fire/smoke
- prevent cars from skidding across multiple city blocks or driving sideways in skid pose

### Audio System

Responsibilities:

- procedural music
- menu music
- Archive music
- gameplay music
- SFX budgeting
- haptics trigger coordination
- settings gates

Audio must avoid noisy overlap:

- medium-office cube drops should use short, sparse, rate-limited sounds
- skyscraper collapse sounds should be budgeted similarly
- many small pieces falling should create texture, not static

### UI System

Surfaces:

- main menu / mode select
- difficulty selector
- settings
- pause
- score/game-over
- stats
- Archive / found documents
- How to Play
- build notes / version notes
- active buff tray
- notifications
- Endless save/load prompts

All non-gameplay screens should have consistent access to help/configuration according to current backlog decisions.

## Object Taxonomy

The Unity build should define object types through data.

Required initial families:

- people
- trees
- benches
- trash cans
- hydrants
- traffic cones
- cars
- crashed cars/wrecks
- houses / small buildings
- medium office buildings
- skyscrapers
- skyscraper chunks
- medium-office cubes
- soldiers
- aid drops
- parallax/gold beacon drop
- visual-only debris/dust/smoke
- rival holes
- player hole

Each object definition should include:

- stable ID
- display name
- theme family
- score value
- growth value
- size/fit requirement
- collision proxy
- render prefab
- audio cues
- haptic category
- achievement tags
- save serialization fields
- pooling category
- performance cost estimate

## Building Collapse Models

### Small Buildings / Houses

Current state:

- Basic whole-object consumption or simple building behavior.

Future requirement:

- Break into small cubes roughly 20 percent of a car length per cube.
- Cubes retain the graphical color/look of the building region they represented.
- This should reuse the stacked/voxel object architecture where practical.

### Medium Office Buildings

Current approved requirement:

- Not whole-object consumption.
- Made from equal cube units.
- Each cube has consistent X/Y/Z dimensions.
- Standing cubes align into a building with visible windows/floors.
- Dimensions vary procedurally between 5 and 10 cubes per axis.
- Only columns over/under the hole begin releasing.
- Cubes above lower cubes wait for support failure.
- Cubes can fall into the hole or miss, settle, bounce, roll, collide, and remain visible.
- Cubes should not overlap in final settled positions.
- On impact, nearby cubes should be jarred out of their perfect grid before falling.
- First impact should read like a pool-table break: visible scatter, collision transfer, and different outcomes per strike.
- Save/load must preserve standing, falling, settled, and consumed cube states.

Unity implementation target:

- Use a custom stack/voxel simulation or ECS-like pooled body system.
- Use simplified collision, spatial hashing, or broadphase grids for cube contacts.
- Avoid full Rigidbody simulation on every cube unless profiling proves it is acceptable.
- Use pooled dust/smoke and decorative debris as visual-only augmentation.

### Skyscrapers

Current approved requirement:

- Distinct from medium offices.
- Require sufficient hole size before destabilizing.
- Collapse as large segmented chunks.
- Collapse should vary by impact side, fall direction, stagger, debris spread, and audio.
- Chunks can be eaten individually after collapse.

Unity implementation target:

- Use authored or procedurally assembled chunk prefabs.
- Use controlled physics impulses and staggered release.
- Allow rare chunk impacts to knock into nearby structures if performance allows.
- Keep this visually larger and more dramatic than medium-office cube collapse.

## Hole Visual Requirements

Current accepted visual:

- Flat black circular mouth with colored rim.

Known rejected experiments:

- recessed gray well
- abyss texture that looked 2D
- deeper shaft that blocked object visibility

Unity target:

- Preserve the readable black mouth/rim baseline first.
- Any depth illusion must not cover swallowed objects.
- Prefer a shader/material solution that clips swallowed objects to the hole mouth and gives subtle interior darkness.
- Do not add a visible center object that hides objects falling into the hole.

## Buffs And Achievements

The current game has lore-based achievement buffs and starter Field Patterns.

Architecture requirements:

- Buff definitions must be data-driven.
- Achievement triggers must be separate from buff effects.
- Trigger progress must be observable for debugging.
- Active buffs must show clear effect text.
- Secret triggers can remain hinted through Archive lore, but unlocked effects must be understandable.
- Timed buffs must not refresh while active.
- Timed buffs must enter a five-second cooldown after expiry.

Starter Field Patterns:

- short speed boost
- temporary/instant hole size increase
- crowd/pedestrian pull effect

Other known lore buff concepts:

- First Bite
- Pedestrian Pull
- Tree Hugger
- The Forum User
- The Quiet Block
- Linden Street
- Bellmar
- The Quiet
- Free Lunch
- The Returning Hole

The Unity version should preserve the names/ideas but can tune values.

## Lore And Archive

The lore system is a major product feature, not optional flavor.

Requirements:

- Found documents are persistent collectibles.
- Archive displays discovered and undiscovered documents.
- Archive music should be funny, whimsical, and conspiracy-adjacent.
- Documents hint at secret achievement patterns.
- Starter patterns are visible immediately.
- Document drops are rare and capped.
- No documents on loss.
- Higher difficulty may increase document drop rates.
- Some future achievements may require higher difficulty.

Data requirements:

- document ID
- thread
- title
- format
- rarity
- hint/buff association
- body text
- unlock condition or drop pool
- discovered state

## Save And Load

Persistent data categories:

- settings
- stats
- achievements/unlocks
- discovered lore documents
- best scores
- best Endless wave
- Endless save slot
- future inventory/rewards
- future daily/weekly quest progress

Endless save must preserve:

- mode = Endless
- current wave
- wave timer
- score
- player position, size, speed/state
- rival positions, sizes, scores, AI state
- active buffs and remaining timers
- buff cooldowns
- soldiers, planes, aid, and deployment timers
- board object states
- medium-office cube states
- skyscraper chunk states
- cars and crash/wreck states
- armed hazards if poison-pill system exists

Save file must include:

- schema version
- build/content version
- migration path
- validation checksum or sanity check

Never serialize Unity object references as authoritative save state.

## Settings, Help, Haptics, And Gamepad

Settings sections:

- Audio: music toggle, SFX toggle
- Haptics: toggle
- How to Play: button
- About: game title, version, creator, procedural music attribution, Three.js/browser credits for browser build; Unity credits for Unity build

Locked decisions:

- Display haptics setting on all devices.
- If unsupported, show disabled toggle and text: "Haptics are not supported on this device/browser."
- Game-over screen includes Settings and How To Play.

Gamepad:

- Left stick steers hole.
- Left stick navigates menus.
- A button selects focused item.
- Deadzone required.
- A-button debounce required.
- Gamepad can connect/disconnect mid-game or mid-menu without freezing.

Unity input should use the new Input System or an equivalent explicit action map.

## Poison-Pill / Containment-Bait Hazard System

Future high-value system to include in architecture now.

Concept:

- The government places hazardous bait in buildings or objects.
- Example visual tell: crooked wire from building to sidewalk where a human in an orange vest holds a detonator.
- Detonation happens when a hole devours the armed building/object.

Possible effects:

- damage/shrink hole by a percentage
- lethal if minimum damage exceeds hole size
- slow movement
- suppress pull radius
- disable timed lore-buff acquisition
- scramble HUD labels
- reduce score multiplier
- temporary sensory/audio distortion

Scaling:

- higher difficulty and deeper Endless waves increase frequency, damage, blast radius, and reaction speed
- system must be data/config driven
- save/load must preserve armed hazard state

## Long-Term Progression Hooks

The Unity architecture should reserve space for:

- game-mode-specific achievements
- daily quests
- weekly quests
- contests/milestones
- reward chests or lore-themed delivery vessels
- hole skins
- titles
- new towns/worlds
- sound packs
- permanent buffs for Endless/future modes
- lore-named inventory/equipment surface
- rival memory: track which rival eats the player most
- rival taunt/chat system
- adaptive difficulty recommendations

Do not build all of these in the first Unity slice, but do not block them architecturally.

## World And Theme Architecture

The current theme is downtown Aldine.

Future themes may include:

- sci-fi city
- hellscape
- wild west town
- medieval settlement
- space colony
- prehistoric settlement
- cartoon world
- black-and-white town
- modified downtown variants

Theme packs define:

- terrain/path layout rules
- object families
- collectibles
- building families
- hazards
- vehicles or equivalent moving targets
- ambient music
- SFX palette
- visual palette
- lighting
- skybox/background
- spawn budgets
- lore labels

Every fifth Endless wave should be the natural future transition point for changing themes.

## Audio Requirements

Music:

- title/menu theme
- gameplay theme
- Archive/found-document theme
- future theme-specific music

SFX:

- devour small object
- devour person
- devour car
- car crash/fire
- building cube fall
- medium office collapse
- skyscraper collapse
- soldier shots
- player damage
- rival eaten
- player eaten
- aid drop
- buff unlock/activate/expire
- document found
- UI navigation

Performance and readability:

- Use audio voice budgets.
- Use random pitch/variation.
- Use distance/priority rules.
- Use short samples for many repeated cube events.
- Collapse events may use one controlled macro sound plus sparse small-piece accents.

## Visual Effects Requirements

Required:

- hole rim states
- swallow/fall path
- score popups
- buff notifications
- soldier tracers
- car crash smoke/fire
- medium-office dust/smoke
- skyscraper dust/chunk impacts
- aid drop effects

Guidelines:

- VFX must be pooled.
- Decorative debris must not be collectible unless it is a real gameplay object.
- Dust/smoke should fade after collapse finishes or after settled cubes are collected.
- Performance profile should scale particle counts and lifetimes.

## UI / UX Requirements

Menu:

- difficulty selector
- Timed
- Last Man Standing
- Waves
- Endless Waves
- Stats
- Archive
- How to Play
- Load Endless if save exists
- Begin

Gameplay HUD:

- tier/growth meter
- live scores
- mode/wave/timer
- pause
- active buff tray
- containment warnings
- event notifications

Pause:

- resume
- save Endless if in Endless
- end game/cashout
- settings
- how to play
- current version

Game over:

- outcome text must match actual result
- no win text on losses
- no document reward on losses
- final score/ranking
- mode picker or return flow
- settings
- how to play

Archive:

- discovered documents
- undiscovered placeholders
- starter Field Patterns
- readable document pane
- lore thread organization

Stats:

- per-mode scores
- best Endless wave
- difficulty stats
- future rival memory and achievements

## Input Requirements

Supported inputs:

- mouse
- keyboard
- touch/mobile drag
- gamepad

Action map:

- move
- confirm
- cancel/back
- pause
- open settings
- open archive
- save Endless
- load Endless

Input ownership:

- Gameplay input and menu input must be explicitly separated.
- Modal open state must block gameplay controls as needed.
- Gamepad focus model must always have a focused element when a menu/modal is open.

## Data Architecture

Use stable IDs for:

- objects
- object categories
- buildings
- themes
- buffs
- achievements
- lore documents
- rewards
- quests
- audio events
- VFX events
- difficulty profiles
- performance profiles

Suggested ScriptableObject sets:

- `ObjectDefinition`
- `BuildingDefinition`
- `ThemeDefinition`
- `BuffDefinition`
- `AchievementDefinition`
- `LoreDocumentDefinition`
- `DifficultyProfile`
- `PerformanceProfile`
- `AudioEventDefinition`
- `VfxEventDefinition`
- `RewardDefinition`
- `QuestDefinition`

Use JSON/import tooling for large lore and quest data so non-engineering edits are practical.

## Performance Requirements

Targets should be measured on real devices, but the architecture should support:

- stable 60 FPS on reasonable desktop hardware
- graceful profile scaling on weaker devices
- no unbounded active physics growth
- no unbounded audio voice growth
- no memory leak during long idle or long Endless play
- no frame spikes during wave transitions
- no frame spikes during large building collapses beyond acceptable short effects

Key practices:

- object pooling
- spatial partitioning for collisions
- fixed/update separation
- AI tick throttling
- LOD or impostors for distant/low-priority objects
- capped active debris/cube budgets
- pooled particles
- AudioMixer voice budgeting
- async/addressable asset loading
- profiler markers around devour, AI, physics, collapse, save/load, and audio systems

## QA And Test Requirements

Core smoke tests:

- game boots
- all modes selectable
- Begin starts selected mode
- mode selection works on desktop and mobile
- pause/resume works
- game-over text matches result
- no document on loss
- documents drop at 0-1 per eligible round
- Archive opens and scrolls
- How to Play opens and scrolls
- Stats opens
- settings persist
- audio toggles work independently

Gameplay tests:

- player grows from normal object devour
- soldier damage does not permanently block growth
- rivals eat objects and grow
- rivals can eat player
- player can eat smaller rivals
- Endless does not end when rivals are eaten
- Endless rival respawn works away from player
- Endless fifth-wave reset works
- Endless save/load restores state
- cars do not skid across the whole city
- medium-office cubes fall, collide, settle, and do not overlap
- skyscrapers collapse distinctly from medium offices

Performance tests:

- long idle menu test
- long Endless test
- multi-collapse stress test
- mobile lower-profile test
- audio voice stress test
- save/load stress test

Regression requirement:

- Any Unity improvement may change visuals, sound, and performance, but must not silently change product rules without recording the decision.

## Migration Plan

### Phase 1 - Unity Prototype

Build a vertical slice:

- one downtown board
- player hole
- rival holes
- basic objects
- growth/scoring
- one mode, preferably Timed or LMS
- readable hole devour visuals

### Phase 2 - Core Mode Parity

Add:

- all four modes
- difficulty profiles
- soldiers
- cars
- aid drops
- game-over/state correctness

### Phase 3 - Buildings And Physics

Add:

- skyscraper chunk collapse
- medium-office voxel collapse
- cube collision/settle behavior
- performance profile budgets

### Phase 4 - Lore And Progression

Add:

- Archive
- found documents
- lore buffs
- starter Field Patterns
- stats
- save/load

### Phase 5 - Polish And Expansion Hooks

Add:

- settings/haptics/gamepad
- improved music/SFX
- theme registry
- quest/reward skeleton
- poison-pill hazard skeleton

## Key Risks

1. Full Rigidbody simulation for every cube may become too expensive.
2. Better graphics can reduce readability if hole mouth and object fit rules are unclear.
3. Audio overlap can become harsh during cube-heavy collapses.
4. Endless save/load can become fragile if renderer objects are serialized instead of simulation state.
5. Theme expansion can become technical debt if theme data is not separated from core rules.
6. AI-driven or procedurally generated lore/taunts can break tone unless governed by curated templates.

## Unity Acceptance Criteria

The first Unity MVP is acceptable when:

- The player can play at least Timed, LMS, Waves, and Endless Waves.
- The hole movement and devour feel are at least as responsive as the browser build.
- Medium-office and skyscraper collapses are visually distinct and performant.
- Endless continues indefinitely unless the player dies or voluntarily exits.
- Save/load works for Endless.
- Lore Archive and starter Field Patterns are present.
- Buffs are readable and obey active/cooldown rules.
- Game-over outcomes and document drops follow approved rules.
- Performance is measurably better than the browser build under comparable object/collapse load.
- The architecture has clear extension points for themes, quests, rewards, poison pills, and progression.

## Claude Implementation Instruction

If this document is given to Claude or another AI implementation agent, instruct it to:

1. Read this document first.
2. Treat `Master 16.52` browser source as behavioral reference, not implementation style.
3. Use Unity-native systems and data assets.
4. Preserve the non-negotiable product decisions.
5. Build in vertical slices with playable tests after each slice.
6. Keep simulation, rendering, UI, audio, and save data separated.
7. Add profiling and debug overlays early.
8. Do not implement future features in bulk before core gameplay parity is fun and performant.

