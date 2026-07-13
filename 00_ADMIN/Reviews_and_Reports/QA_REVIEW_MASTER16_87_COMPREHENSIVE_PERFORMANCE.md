# QA Review - Master 16.87 Comprehensive Performance Pass

Date: 2026-06-18

## Scope

Complete performance-oriented review of the governed modular game package:

- `index.html` and `css/styles.css`
- `js/main.js`
- `js/government-physics.js`
- `js/difficulty-profiles.js`
- `js/build-info.js`
- `data/lore-documents.js`
- source/release package parity and startup wiring

The static difficulty, lore, and build-data modules contain no ongoing frame work. Runtime costs were concentrated in rendering, object consumption, building physics, lighting registries, audio initialization, and gameplay DOM overlays.

## High-Impact Findings And Repairs

1. Vortex arcs replaced and disposed GPU geometry for every hole on every rendered frame.
   - Reused stable arc geometry and animated transforms/materials instead.
2. Every consumed object scanned the complete building-window registry, including people, cars, trees, and props.
   - Limited power-cut work to building pieces and added direct mesh-to-light registries.
3. Lighting updates scanned every window and streetlight every frame even when no light was flickering.
   - Added active power-cut sets that contain only current flicker work.
4. Object consumption performed two full hole-distance passes per object and allocation-heavy dimension checks.
   - Combined eating/pulling into one squared-distance pass and removed temporary arrays.
5. Inactive voxel/building pieces checked every hole every frame.
   - Staggered inactive trigger checks by performance profile while keeping active physics full-rate.
6. Government physics built contact structures and synchronized all bodies while the building was idle.
   - Added an awake-body fast path and reused contact maps/sets.
7. District factories recreated identical box geometry and materials across hundreds of objects and every world rebuild.
   - Added shared material/geometry caches with explicit geometry disposal during teardown.
8. Mode selection synchronously generated reverb data and converted every embedded audio sample during the input event.
   - Kept gesture-safe AudioContext creation lightweight, deferred reverb setup, and moved sample conversion/decoding to sequential asynchronous warmup.
9. Active effects, lore ranking, wave text, HUD, translucent gameplay overlays, and shadow/render resolution performed more work than their visible update rate required.
   - Cached/throttled DOM and ranking work, removed gameplay backdrop blur, reduced default DPR/shadow cost, and added a conservative sustained-slow-frame renderer fallback.

## Preserved Behavior

- Simulation, input, camera motion, active collapse physics, and WebGL rendering remain frame-driven.
- Government physics stays isolated in `government-physics.js`.
- Run Goals, mastery, time-of-day, window/streetlight power loss, Waves, Endless, building breakup, and audio remain present.
- The modular source and release package shape is unchanged.

## Validation

- Team Sync and Product Intent Gate: passed.
- `node --check` for source/release `main.js`, `government-physics.js`, and `build-info.js`: passed.
- Browser page identity and nonblank mode-selection screen: passed.
- Mode selection interaction returned in approximately 359 ms in the clean browser run.
- Begin interaction returned in approximately 355 ms before asynchronous world construction completed.
- Wave 1, Run Goals, and live scores remained present after sustained smoke play.
- New browser console warnings/errors: none.
- Source/release hashes match for every changed package file.

Screenshot capture limitation:

- The in-app browser's CDP screenshot command timed out even after pausing gameplay. DOM state and interactions remained responsive, so screenshot evidence is unavailable from this run.

## Required Device Validation

- Open `http://127.0.0.1:4173/index.html?fresh=16.87-performance-review`.
- Play Waves through dense consumption and at least one major building collapse.
- Confirm movement stays smooth as debris, soldiers, traffic, lighting effects, goals, and sound overlap.
- Confirm recorded SFX begin appearing after background audio warmup without blocking input.
- Confirm lower renderer resolution or disabled shadows are acceptable if a slow device triggers the adaptive fallback.

## Residual Risk

- Destructible buildings intentionally remain many independent meshes, so extreme simultaneous collapses still carry an irreducible draw-call and collision cost.
- Final frame pacing must be confirmed on the slower device that produced the original report.

## Result

Implementation and browser interaction validation passed with the screenshot and device-specific limitations above.
