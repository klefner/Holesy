# QA Review - Master 16.191 Rural Sites, Trails, and Railgate Catalog

Date: 2026-07-29

## Scope

- Harvest County rural placement and opening density
- historic/frontier trail geometry and surface treatment
- busted wagon and wagon-wheel consumables
- Railgate-owned and shared-theme object catalogs
- source/release modular package parity

## Chief of Staff Gate

- The governed next-town sequence selects Railgate after Harvest County.
- Required roles include world design, systems architecture, procedural layout/routes, technical art, destruction physics, economy, performance, audio, QA/release, and accessibility/UI writing.
- Supplied train, public-transport, realistic-car, and building archives contain confirmed reusable Railgate models.
- Supplied archives do not contain 100 distinct Railgate meshes; the catalog therefore distinguishes confirmed imported assets from procedural and compound objects.

## Expected Behavior

- Harvest rural sites are generated directly in world space with collision spacing and trail affinity, not from `blockPositions`.
- Historic trails curve smoothly, vary continuously in width, and show multiple dry/muddy/wet/rutted treatments with irregular edges.
- Harvest spawns intact and busted wagon/wheel objects without removing existing buildings, animals, crops, workers, or Mandates.
- The Railgate-owned catalog contains exactly 100 unique stable IDs.
- Shared modern objects remain a separate allowlisted catalog and are not duplicated as theme ownership.
- A generated run selects a budgeted subset of a theme catalog rather than loading or spawning every catalog entry.

## Verification

- JavaScript syntax passed for source and release `main.js`, `build-info.js`, and `data/theme-object-catalogs.js`.
- Catalog test reported 100 Railgate entries, 100 unique Railgate IDs, 20 shared entries, and a 36-entry budgeted Railgate selection.
- Desktop browser smoke displayed Version 16.191 with no page errors.
- Desktop Harvest telemetry reported 17 destructible buildings, 1,530 edibles, 26 wagon/wheel objects, Railgate catalog size 100, and shared catalog size 20.
- Three additional desktop seeds completed in 2.18 to 2.49 seconds with identical supply totals, varied feasible Mandates, and no page errors.
- Mobile 390x844 smoke completed with 1,530 edibles and no page errors.
- Screenshot review confirmed curved junctions, width variation, wheel ruts, tonal soil variation, and increased visual density.

## Remaining Validation

- User visual approval of trail surface variety, field density, wagon readability, and overall rural spacing.
- Longer real-device play for frame pacing and wagon consumption.
- Railgate remains a catalog and asset-reuse foundation; its runtime town, articulated train routes, normalized runtime GLBs, and destruction mappings are separate implementation slices.

## Publication

Local source/release candidate only. No commit, push, Pages deployment, or production verification has been performed.
