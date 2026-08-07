# QA Review - Master 16.201 Large-Hole Rendering And Generic Pop

Date: 2026-08-07

## Scope

- Preserve the accepted Master 16.198 Harvest County population and progression supply.
- Reduce the synchronized graphics, movement, and sound stall observed when an Abyss-sized player camera exposes most of Harvest County.
- Give consumed objects without an explicit sound category a small generic pop.

## Implementation evidence

- The adaptive renderer now has a Harvest micro-detail stage in addition to its existing pixel-ratio and shadow controls.
- It activates only in overhead view at radius 7.3 or greater and evaluates visibility at a 220 ms cadence rather than every frame.
- It only culls Harvest objects no larger than 0.68 whose projected diameter is below 3.25 pixels and which are outside a player-radius-scaled protected area.
- Buildings, people, animals, powerups, falling objects, destructible building pieces, and physics-stack pieces are explicitly protected.
- Culling changes only `mesh.visible`; object records remain in the shared simulation, interaction, scoring, and consumption lists.
- Uncategorized consumption uses a 65 ms synthesized pop and the existing 24-voice non-music ceiling and cleanup registry.

## Verification

- `node --check` passes for source and release `main.js` and `build-info.js`.
- `git diff --check` passes for the scoped source, release, governance, and QA files.
- SHA-256 parity is required between the modular source and release copies of `index.html`, `main.js`, `build-info.js`, and `PACKAGE_MANIFEST.md`.
- Runtime telemetry exposes `data-holesy-perf-culled-harvest-micro-details`, `data-holesy-perf-harvest-detail-governor`, and `data-holesy-audio-generic-consume-pops`.

## Remaining live test

The user should repeat the late-wave Harvest run on the published Pages build. The pass condition is that Wave 5-6 remains controllable with continuous music while nearby content stays visually dense and silent categories produce a restrained pop.
