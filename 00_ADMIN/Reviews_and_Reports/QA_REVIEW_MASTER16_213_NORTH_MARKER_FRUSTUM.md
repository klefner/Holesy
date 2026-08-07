# QA Review — Master 16.213 North Marker Frustum

Date: 2026-08-07

## Finding

The player screenshot shows the compass aligned north while the world-space `N` is absent. Hole-Eye points downward to retain the complete hole rim, leaving only a shallow strip of sky. The marker was centered at Y=70 with a 34-unit height, placing it above the camera's visible frustum rather than merely behind scenery.

## Correction

- Lower marker center from Y=70 to Y=17.
- Reduce height from 34 to 28 and half-width from 11.5 to 9.5.
- Move the marker from 120 units beyond the north edge to 34 units beyond it.
- Retain ordinary scene depth testing and keep the landmark outside the playable arena.

## Acceptance

- Facing north in Hole-Eye shows the landmark in the distant sky/horizon band.
- Terrain and buildings can occlude it.
- The landmark remains absent from overhead view and non-game screens.
- Turning away from north moves it naturally out of view because it is world-space geometry.

## Verification

- Local Master 16.213 loaded a Classic City run and entered Hole-Eye successfully; the first-person compass and pause lifecycle remained intact.
- Frustum calculation at the representative small-hole camera places the corrected marker from roughly 17 to 28 degrees above the camera look vector, inside the 27.5-degree vertical half-FOV with only the upper edge eligible for natural clipping. The former marker began above roughly 40 degrees and was wholly outside that view.
- Browser screenshot capture was not reliable under the active WebGL load, so final visual placement remains a player-facing acceptance check on the published build.
