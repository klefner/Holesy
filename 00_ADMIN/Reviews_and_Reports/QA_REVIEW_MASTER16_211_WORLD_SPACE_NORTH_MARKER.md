# QA Review — Master 16.211 World-Space North Marker

Date: 2026-08-07

## Scope

Replace the first-person north-facing DOM overlay with a stationary three-dimensional landmark in the far northern sky while preserving the compact compass arrow.

## Acceptance Checks

- The north `N` is composed of extruded world-space geometry rather than DOM text.
- The marker is fixed beyond the northern edge of the current arena and is repositioned only when arena scale changes.
- Normal depth testing allows terrain, buildings, towers, and other game geometry to occlude the marker.
- The marker has no direction-driven opacity fade and remains visible throughout active Hole-Eye gameplay.
- Turning away naturally moves the marker outside the camera frustum.
- Overhead view and non-gameplay screens hide the marker.
- The existing compact compass arrow still rotates toward north.

## Performance Boundary

The marker adds one group containing three static box meshes, no textures, no animation, no physics bodies, and no per-frame placement work. Its material does not write depth and does not participate in fog or shadows.

## Verification

- `node --check` passed for `js/main.js` and `js/build-info.js`.
- Browser visual QA confirmed the marker is world-positioned, changes perspective as the player turns, naturally leaves the view frustum, and no longer behaves like a centered HUD layer.
- The initial near-boundary placement was rejected during screenshot review because it appeared oversized and clipped; the final marker sits 120 world units beyond the north boundary at sky height 70 with a 400-unit Hole-Eye far plane.
- Browser console error inspection returned no errors.
