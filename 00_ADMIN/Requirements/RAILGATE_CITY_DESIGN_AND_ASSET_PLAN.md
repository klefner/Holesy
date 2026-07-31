# Railgate City Design and Asset Plan

Date: 2026-07-29
Status: governed design and catalog foundation; runtime town not yet implemented

## Decision

Railgate is the next new city after Harvest County. It is already identified as T02 in `NEW_TOWN_ASSET_ALLOCATION_MATRIX.md` and follows Harvest in the governed execution order. Railgate provides a dense rail/public-transport contrast to rural Harvest and exercises the reusable articulated-route system before later towns.

## Required Roles

1. Chief of Staff: sequencing, dependency gates, and cross-role scope control.
2. Product/World Design Lead: identity, density, progression ladder, and object roster.
3. Theme/Systems Architect: modular city recipe, catalog, eligibility, and supply contracts.
4. Procedural Layout and Route Engineer: rail graph, stations, yards, crossings, and articulated paths.
5. 3D Asset Technical Artist: selective extraction, scale/axis/material normalization, GLB optimization, LODs, and collision metadata.
6. Destruction/Physics Engineer: destructible buildings, couplings, separable wagons, swallowing, and debris.
7. Gameplay/Economy Designer: values, sizes, supply guarantees, Mandates, and Run Goals.
8. Performance and Audio Owners: mobile budgets plus coherent rail/station ambience and lifecycle teardown.
9. QA/Release Steward: deterministic seeds, feasibility, desktop/mobile smoke, parity, and publication evidence.
10. Accessibility/UI Writer: readable names, instructions, diagnostics, and player-facing labels.

## Confirmed Supplied Model Sources

- `50_ASSETS/Graphics and Art/trains/Train Pack - April 2019-20260727T160206Z-1-001.zip`
  - straight and curved railway track
  - locomotive, tender, passenger, cargo, container, coal, open, and high-speed train components
- `50_ASSETS/Graphics and Art/vehicles/Public Transport Pack - Feb 2017-20260727T160216Z-1-001.zip`
  - bus, school bus, ambulance, taxi, train, bicycles, cones, traffic lights, and signs
- `50_ASSETS/Graphics and Art/vehicles/Realistic Car Pack - Nov 2018-20260727T160158Z-1-001.zip`
  - sedans, SUV, taxi, police car, and sports cars
- `50_ASSETS/Graphics and Art/buildings/Buildings Pack - Aug 2017-20260727T155544Z-1-001.zip`
  - shop, bank, hospital, flats, and houses
- `50_ASSETS/Graphics and Art/buildings/Buildings Pack - Jan 2019-20260727T155520Z-1-001.zip`
  - multiple small/large buildings and houses

The governed allocation matrix marks the Railgate packs approved. Intake must still retain exact per-pack license evidence. The public-transport archive requires explicit source-license verification before promotion because its displayed ZIP listing did not include `License.txt`.

## Object Contract

`10_SOURCE/Masters/Master 16/data/theme-object-catalogs.js` is authoritative for the exactly-100-entry Railgate-owned catalog and the separate shared-modern catalog.

- Theme ownership and cross-theme eligibility are separate.
- A run selects a budgeted weighted subset; it does not spawn all catalog entries.
- Minimum supply is satisfied before optional weighted selection.
- Recolors alone do not count as distinct objects.
- Imported, procedural, compound, articulated, and destructible objects use explicit factory/destruction metadata.
- Inactive-theme assets must not load.
- Mandates and Run Goals must cap targets to actual spawned eligible inventory.

## Sequencing

1. Validate the Harvest rural-site/trail contract.
2. Normalize selected Railgate vertical-slice assets and preserve license metadata.
3. Implement a 20-to-30-object graybox with rail graph, station, yard, and crossings.
4. Add articulated trains and separable wagons under mobile performance budgets.
5. Expand to the 100-entry catalog using procedural/compound objects where no supplied model exists.
6. Add population, response profile, objectives, ambience, deterministic QA, and release evidence.
