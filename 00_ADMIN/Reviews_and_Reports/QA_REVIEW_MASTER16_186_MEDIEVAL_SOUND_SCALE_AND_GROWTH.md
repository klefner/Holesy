# QA Review - Master 16.186 Medieval Sound, Scale, and Growth

Date: 2026-07-27

## Scope

- gameplay soundtrack continuity after Begin
- authored-scale preservation during ordinary object descent
- immediate animal perspective shrink
- Medieval Village forage density and period-specific road life

## Expected Behavior

- the existing music and sound-effect infrastructure remains audible after gameplay begins
- an animal enters the hole at its world scale, never enlarges at consume start, and progressively shrinks as it falls
- Medieval Village contains no modern traffic substitutes; its roads use villagers, horses, and horse-drawn carts
- clearing the opening forage budget can reach the smallest structure gate in Wave 1
- medium, large, and tower structures remain progressively later targets rather than all becoming Wave 1 food

## Verification

- JavaScript syntax passed for source and release `main.js` and source `build-info.js`.
- SHA-256 parity passed for `index.html`, `js/main.js`, and `js/build-info.js`.
- Local Chrome boot displayed `Version 16.186` with no console or page errors.
- Medieval runtime telemetry reported 32 imported buildings, 736 themed loose edibles, and 90 ambient actors.
- Theme exclusions remained intact: zero cars, hydrants, and electric lamps.
- Instrumented Web Audio reported one running context, 188 scheduled oscillators, 14 buffer sources, and an unmuted Music control after gameplay began.
- Visual review confirmed the Medieval world completed construction and the HUD/playfield rendered normally.
- Code-path review confirmed `beginConsume()` captures `mesh.scale` before descent and the fall loop multiplies the preserved vector by the perspective factor instead of assigning scale `1`.
- Animal descent starts shrinking at depth `0.35`; non-animal timing and full-size voxel behavior remain unchanged.

## Risk

The added low-poly road actors and parcel props increase active object count. Local Chrome completed city construction without an error, but extended real-device performance and a manually observed animal swallow remain player-validation items.
