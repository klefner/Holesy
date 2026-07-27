# Town Theme And Asset Composition Standard

## Principle

A Holesy town is a positive composition recipe. It declares its era, location, environment packs, civilian population, agriculture level, transportation, street furniture, enemy roster, boss roster, deployment method, objectives, audio language, and explicit anomalies. It must not inherit the complete modern city and then blacklist whichever mismatches happen to be noticed.

The alien aid ship is the one approved cross-era anomaly. It may enter any town because its contrast is intentional and gameplay-significant.

## Required Recipe Fields

- era and location fantasy
- allowed asset packs
- allowed buildings and landmarks
- ground, roads, paths, and parcel uses
- agriculture and animal weighting
- civilian roles and ambient actions
- transportation roster
- street-furniture roster
- ordinary enemy roster
- boss roster and required devour radius
- enemy arrival method
- compatible objectives and terminology
- audio and effects vocabulary
- explicit anomaly allowlist

Every new town must expose browser-test instrumentation for its active recipe, prohibited modern-object counts, enemy roster, deployment style, agriculture population, and boss gate.

## Current And Planned Town Matrix

| Town | Era/location | Recommended pack combination | Population and land use | Enemies and arrival |
| --- | --- | --- | --- | --- |
| Classic City | contemporary city | Holesy originals; Buildings Pack; realistic cars; public transport; animated characters; restrained modern nature | offices, homes, parks, cars, pedestrians, modern street furniture | current soldiers, vehicles, aircraft, and modern bosses |
| MegaKit Downtown | dense contemporary downtown | Downtown City MegaKit; Buildings Pack; realistic cars; public transport; animated characters | highest building/traffic weight and lowest agriculture weight | current modern/futuristic response roster |
| Medieval Village | European medieval settlement | Medieval Village MegaKit; Modular Medieval Buildings; Ultimate Fantasy RTS; Farm Buildings; Nature Crops; Farm Animals; Animated Character Pack; Textured Fantasy Nature | very high crops, pasture, livestock, carts, barrels, wells, smithing, markets, and foot traffic; no cars or electric streetlights | swordsmen, longbow archers, mounted lancers, and the mounted Iron Reeve; ground warbands only |
| Harvest County | contemporary rural county | Farm Buildings; Nature Crops; Farm Animals; Ultimate Nature; realistic rural vehicles; animated characters | highest agriculture weight, broad crop variety, livestock, barns, silos, farm machinery, and sparse town services | modern rural emergency/military response; aircraft allowed |
| Railgate | industrial rail city | Buildings Pack; city infrastructure base map; Train Pack; Public Transport; Realistic Cars; Ultimate Nature | stations, warehouses, rail yards, commuters, cargo, and industrial clutter | industrial security, armored response, and rail-themed bosses |
| Lockdown | contemporary ruined/quarantine city | Zombie Apocalypse Kit; Ultimate Modular Ruins; ruined city; Animated Monster Pack; Realistic Cars | abandoned stores, barricades, debris, survivors, contaminated lots, and limited food | infected creatures, survivor factions, emergency response, and monster bosses |
| Farstar Colony | off-world science-fiction settlement | Ultimate Space Kit; Ultimate Modular Sci-Fi; Ultimate Spaceships; Alien Planet skybox; Alien Animated; Animated Robot; Animated Mech | sealed habitats, alien terrain, rovers, cargo, technicians, and research equipment | aliens, robots, mechs, spacecraft, and science-fiction bosses |
| Crownlands | fortified fantasy capital | Medieval Village MegaKit; Modular Medieval Buildings; Ultimate Fantasy RTS; Updated Modular Dungeon; Textured Fantasy Nature; Farm Animals | farms outside walls, dense markets inside, horses, guards, guilds, siege stores, and castle districts | knights, archers, cavalry, siege engines, sorcerous elites, and ground assaults |

## Historical-Town Rules

- Agriculture and animals are primary progression content, not decorative accents.
- Cars, hydrants, electric streetlights, aircraft, guns, robots, tanks, mechs, modern uniforms, and modern objectives are prohibited unless explicitly named as a town-specific anomaly.
- Enemy travel must fit the period: foot patrols, cavalry, carts, ships, tunnels, gates, or siege approaches.
- Saved state must be normalized through the active town recipe so legacy objects cannot reintroduce prohibited content.
- Enemy labels, HUD messages, sound effects, and objectives must use period-compatible language.

## Scaling Procedure

1. Select a town fantasy and assign the recipe fields above.
2. Choose the smallest compatible combination of owned packs.
3. Inventory and license-record every selected source asset.
4. Normalize selected assets to the shared runtime GLB conventions.
5. Process imported buildings through the governed authored-facade destruction pipeline.
6. Build the small-to-large devour progression before visual polish.
7. Add town-specific civilians, agriculture, animals, enemies, bosses, and arrivals.
8. Run prohibited-content assertions and cross-town regression tests.
9. Record unusual exceptions in the town recipe; do not generalize them to other towns.
