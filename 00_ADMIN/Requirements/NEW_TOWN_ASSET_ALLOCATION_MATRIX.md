# New Town Asset Allocation Matrix

Date: 2026-07-27

## Purpose

Allocate the July 2026 model intake into visually distinct new Holesy towns. The downloaded packs are not a general decoration pool for the existing towns. Each archive receives one primary town assignment; secondary reuse is deliberately limited so recognizable assets do not make different towns feel interchangeable.

Raw ZIP archives remain untouched under `50_ASSETS/Graphics and Art/`. Only approved, license-cleared, optimized assets may be promoted to `assets/environments/<themeId>/runtime/`.

## Shared town rules

- Each town owns its layout, terrain, routes, buildings, population, vehicles, enemies, ambient behavior, progression ladder, and landmark roster.
- Every town must contain a continuous consumption ladder from opening-radius objects through its largest structures.
- Every imported building uses `IMPORTED_CITY_MODEL_DESTRUCTION_STANDARD.md`.
- Animated people, animals, vehicles, trains, and enemies use normalized rigs and bounded update budgets.
- A pack has one primary visual home. Secondary use is limited to generic assets that do not define the pack's identity.
- City-specific exceptions are declared in that town's manifest and do not alter shared destruction, fit, collision, audio, loading, or cleanup contracts.
- One active town loads only its own optimized runtime bundles.
- Large combined Sketchfab scenes must be separated into named prefabs before runtime use.

## Proposed town portfolio

### T01 Harvest County

Identity: working agricultural county with farms, silos, orchards, crop rows, livestock, dirt lanes, water towers, barns, and a compact market center.

Primary packs:

- Farm Buildings
- Nature Crops Pack
- Farm Animals
- Ultimate Animated Animals
- Simple Nature Pack

Gameplay signature:

- crops and loose produce create the opening ladder
- chickens, sheep, pigs, crates, fences, and farm equipment bridge to barns
- barns, silos, windmills, and water towers form the large-object ladder
- roaming livestock and harvesting scenes make the town feel occupied

Explicit exceptions: windmill blades, water-tower tanks, fence chains, and crop-row batch placement.

### T02 Railgate

Identity: dense railway and public-transport town organized around stations, freight yards, bus routes, crossings, and mixed commercial blocks.

Primary packs:

- Train Pack
- Public Transport Pack
- Realistic Car Pack
- Buildings Pack - January 2019
- Buildings Pack - August 2017

Gameplay signature:

- cones, signs, bicycles, luggage, lights, and benches start progression
- cars, taxis, buses, ambulances, and train wagons create moving intermediate targets
- stations, apartment blocks, banks, hospitals, and freight structures finish progression
- scheduled trains create dangerous moving consumption opportunities

Explicit exceptions: articulated trains, track routing, crossing gates, and separable wagons.

### T03 Brickworks Borough

Identity: old brick commercial district distinct from MegaKit Downtown, built from the remaining modular facade vocabulary rather than reusing the existing five-building presentation.

Primary packs:

- Downtown City MegaKit Standard
- Ultimate Building Models Pack
- city-infrastructure-base-map
- low-poly-city-pack as a separable skyline and parcel source

Gameplay signature:

- loading docks, planters, vents, signs, storefront fixtures, and street furniture fill parcel edges
- modular shops, warehouses, offices, and civic blocks form a dense urban ladder
- a rail viaduct or industrial boulevard can distinguish its layout from the current grid

Explicit exceptions: the nested Ultimate Building Models archives require controlled second-level intake; the infrastructure scene must be separated into roads and props, and may not become a visual-only ground layer covering the hole.

### T04 Concrete Capital

Identity: severe civic/industrial town with monumental brutalist structures, broad plazas, service roads, and authoritarian military pressure.

Primary packs:

- brutalist-building
- sci-fi-mega-castle-by-jungle-jim
- Tank Pack

Gameplay signature:

- plaza furniture, barriers, lights, vents, checkpoints, and service equipment supply early growth
- armored traffic and tanks replace ordinary late-stage vehicles
- monumental concrete structures are true landmarks rather than mislabeled skyscrapers

Explicit exceptions: both Sketchfab structures are combined authored scenes requiring separation and license verification; tanks require turret/body hierarchy review.

### T05 Ruinfall

Identity: abandoned nature-reclaimed city with collapsed blocks, broken bridges, exposed rooms, flooded courtyards, and overgrown streets.

Primary packs:

- ruined-city-free
- Ultimate Modular Ruins Pack
- Textured Stylized Trees
- Textured Fantasy Nature

Gameplay signature:

- loose bricks, books, candles, plants, rubble, traps, barrels, and broken furniture create dense small targets
- ruined rooms and bridge sections form medium targets
- intact remnants and major ruined shells provide large destruction sequences

Explicit exceptions: pre-broken structures need consumption ownership rules so decorative rubble does not duplicate destruction rewards.

### T06 Lockdown

Identity: contemporary city under zombie quarantine, with barricades, damaged roads, evacuation vehicles, survivors, hostile creatures, and improvised defenses.

Primary packs:

- Zombie Apocalypse Kit
- street-city-for-games-free
- Animated Monster Pack

Gameplay signature:

- trash, weapons, cones, blood props, cases, furniture, and barricades start progression
- dogs, survivors, zombies, wrecks, pickups, and armored cars bridge upward
- quarantine installations and large infected enemies create district goals

Explicit exceptions: armed characters, giant infected enemies, vehicle armor, weapons, and combined Sketchfab city separation.

### T07 Crownlands

Identity: a full fantasy kingdom rather than another version of Medieval Village—age-progressing settlements, fortified districts, markets, farms, docks, castles, and rival factions.

Primary packs:

- Ultimate Fantasy RTS
- Modular Medieval Buildings
- Medieval Village MegaKit Standard for components not used by the existing Medieval Village
- Ultimate Animated Character Pack

Gameplay signature:

- produce, market goods, tools, livestock, barrels, crates, and villagers fill the opening economy
- houses, workshops, towers, walls, docks, and faction buildings create clear advancement
- castles and fortified compounds become late-run objectives
- the character roster supports workers, guards, nobles, fantasy races, and attackers

Explicit exceptions: walls and bridges use connected-section destruction; towers and castles require semantic size classes; faction/age variants must not inflate runtime memory.

### T08 Underkeep

Identity: subterranean dungeon settlement with torch-lit halls, prisons, treasure rooms, broken chambers, traps, and monsters.

Primary packs:

- Modular Dungeon Pack
- Updated Modular Dungeon
- Animated Monster Pack as its limited secondary home

Gameplay signature:

- coins, bones, books, candles, sacks, bottles, and loose stones start progression
- barrels, chests, furniture, traps, columns, and monsters bridge to rooms
- room sections, gates, walls, and vault structures become destructible architecture

Explicit exceptions: interior camera framing, ceiling visibility, room-to-room paths, torch lights, doors, traps, and non-grid wall destruction.

### T09 Neon Foundry

Identity: terrestrial futuristic manufacturing city with modular facilities, reactors, containers, robots, and industrial mechs.

Primary packs:

- Ultimate Modular Sci-Fi
- Animated Mech Pack
- Animated Robot

Gameplay signature:

- tools, panels, pipes, terminals, containers, and lab props create early growth
- robots, vessels, machinery, and small mechs bridge progression
- factories, reactors, large mechs, and modular industrial halls create late targets

Explicit exceptions: mech rigs, powered doors, reactor effects, emissive materials, and detachable industrial machinery.

### T10 Farstar Colony

Identity: off-world settlement with habitat domes, rovers, astronauts, alien vegetation, hostile aliens, spacecraft, and a visibly non-Earth landscape.

Primary packs:

- Ultimate Space Kit
- Ultimate Spaceships
- Alien Animated
- landscape-of-an-alien-planet-skybox
- Ultimate Stylized Nature

Gameplay signature:

- alien plants, tools, oxygen equipment, samples, pickups, and small robots start progression
- astronauts, aliens, rovers, drones, and habitat modules bridge upward
- domes, bases, large enemies, landed spacecraft, and colony infrastructure finish progression

Explicit exceptions: spacecraft landing gear and flight states, habitat connectors, domes, rover wheels, alien rigs, low-gravity-flavored debris, and skybox/background treatment.

### T11 Wildreach

Identity: sparsely settled wilderness town whose districts change across temperate, autumn, deadwood, and snow biomes.

Primary packs:

- Stylized Nature MegaKit Standard
- Ultimate Nature Pack
- the second Nature Crops download is not treated as additional visual content

Gameplay signature:

- flowers, mushrooms, grass, fruit, stones, and branches create the opening ladder
- bushes, crops, logs, tents, animals, and small cabins bridge progression
- mature trees, rock formations, lodges, and ranger structures provide large targets

Explicit exceptions: tree fall direction, seasonal material variants, vegetation batching, and ensuring dense foliage never obscures the hole.

## Archive allocation register

| Archive | Primary town | Secondary use | License state | Intake decision |
| --- | --- | --- | --- | --- |
| Farm Animals | Harvest County | Crownlands livestock | Included CC0 | approve for normalization |
| Ultimate Animated Animals | Harvest County | Wildreach wildlife | Included CC0 | approve; audit animation clips |
| brutalist-building | Concrete Capital | none | Free Standard; NoAI | cleared for incorporated use; exclude from generative-AI workflows |
| Buildings Pack - August 2017 | Railgate | none | Quaternius CC0 | cleared |
| Buildings Pack - January 2019 | Railgate | Brickworks only if needed | Included CC0 | approve |
| Downtown City MegaKit Standard | Brickworks Borough | existing MegaKit Downtown lineage only | Included CC0 | approve unused roster |
| Farm Buildings | Harvest County | Crownlands farms | Included CC0 | approve |
| low-poly-city-pack | Brickworks Borough | none | CC BY 4.0 | cleared with attribution; separate combined scene |
| Medieval Village MegaKit Standard | Crownlands | existing Medieval lineage only | Included CC0 | use only roster/components absent from the existing town |
| Modular Dungeon Pack | Underkeep | none | Quaternius CC0 | cleared |
| Modular Medieval Buildings | Crownlands | Underkeep fortifications | Quaternius CC0 | cleared |
| ruined-city-free | Ruinfall | Lockdown skyline only | Free Standard; AI-assisted textures disclosed | hold for provenance/content review |
| sci-fi-mega-castle | Concrete Capital | Neon Foundry landmark candidate | CC BY 4.0; AI-model provenance disclosed | hold for provenance review |
| street-city-for-games-free | Lockdown | none | Free Standard | cleared for incorporated use; separate scene and review signage |
| Ultimate Building Models Pack | Brickworks Borough | Railgate only if needed | Included CC0 | inspect nested archives |
| Ultimate Fantasy RTS | Crownlands | Harvest County generic farm props only | Included CC0 | approve |
| Ultimate Modular Ruins | Ruinfall | Underkeep limited props | Included CC0 | approve |
| Ultimate Modular Sci-Fi | Neon Foundry | Farstar connectors only | Included CC0 | approve |
| Ultimate Space Kit | Farstar Colony | Neon Foundry generic machinery only | Included CC0 | approve |
| Updated Modular Dungeon | Underkeep | none | Included CC0 | approve |
| Zombie Apocalypse Kit | Lockdown | Ruinfall generic rubble only | Included CC0 | approve |
| Alien Animated | Farstar Colony | none | Included CC0 | approve; audit rig |
| Animated Mech Pack | Neon Foundry | Concrete Capital boss candidate | Included CC0 | approve; audit rigs |
| Animated Monster Pack | Underkeep | Lockdown limited infected variants | Included CC0 | approve; avoid repeated hero silhouettes |
| Animated Robot | Neon Foundry | Farstar small enemy candidate | Included CC0 | approve; audit rig |
| Tank Pack | Concrete Capital | Lockdown limited military response | Included CC0 | approve |
| city-infrastructure-base-map | Brickworks Borough | Railgate layout reference | CC BY 4.0 | cleared with attribution; separate combined scene |
| alien-planet landscape/skybox | Farstar Colony | none | CC BY 4.0 | cleared with attribution |
| Nature Crops Pack copy A | Harvest County | Crownlands crops | Included CC0 | approve canonical copy |
| Nature Crops Pack copy B | Harvest County duplicate candidate | none | Included CC0 | archive hashes differ; compare internal payload before deleting or promoting |
| Simple Nature Pack | Harvest County | lightweight fallback only | Quaternius CC0 | cleared |
| Stylized Nature MegaKit Standard | Wildreach | Farstar recolor prohibited unless separately approved | Included CC0 | approve |
| Textured Fantasy Nature | Ruinfall | Underkeep vegetation | Quaternius CC0 | cleared |
| Textured Stylized Trees | Ruinfall | Lockdown overgrowth | Included CC0 | approve |
| Ultimate Nature Pack | Wildreach | seasonal Crownlands outskirts | Included CC0 | approve |
| Ultimate Stylized Nature | Farstar Colony | Wildreach limited secondary | Included CC0 | approve; large source requires strict selection |
| Ultimate Animated Character Pack | Crownlands | Railgate civilian subsets | Included CC0 | approve; partition character families |
| Ultimate Modular Women | Railgate | Neon Foundry/Farstar role subsets | Included CC0 | approve; normalize modular combinations |
| Train Pack | Railgate | none | Included CC0 | approve |
| Public Transport Pack | Railgate | Brickworks buses only | Quaternius CC0 | cleared |
| Realistic Car Pack | Railgate | Lockdown undamaged-car subset | Included CC0 | approve |
| Ultimate Spaceships | Farstar Colony | none | Included CC0 | approve; strict size/texture budget |

## Implementation order

1. License register and archive hashes
2. Harvest County vertical slice
3. Railgate vertical slice
4. Lockdown vertical slice
5. Farstar Colony vertical slice
6. Crownlands vertical slice
7. Ruinfall
8. Neon Foundry
9. Underkeep
10. Brickworks Borough
11. Concrete Capital
12. Wildreach

Harvest County is first because it has the cleanest CC0 evidence, the easiest continuous consumption ladder, many separate low-poly prefabs, and limited need for new pathing architecture. Railgate follows because trains require a reusable articulated-route system. Lockdown, Farstar, and Crownlands then exercise increasingly unusual enemy, layout, animation, and destruction contracts.

## Vertical-slice gate for every town

Before converting a complete pack, each town must prove:

- one layout block or equivalent non-grid district
- one starter-object family
- one intermediate moving or animated family
- one small, one medium, and one landmark building
- one city-specific ambient scene
- one explicit unusual-element override
- intact-to-destroyed visual continuity
- mobile and desktop frame budget
- complete teardown and town-switch cleanup

No town proceeds to full-roster conversion until its vertical slice passes this gate.
