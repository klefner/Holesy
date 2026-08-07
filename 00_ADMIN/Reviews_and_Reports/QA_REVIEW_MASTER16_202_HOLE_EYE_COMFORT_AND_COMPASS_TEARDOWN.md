# QA Review - Master 16.202 Hole-Eye Comfort And Compass Teardown

Date: 2026-08-07

## Scope

- Reduce first-person motion discomfort without slowing gameplay movement.
- Preserve north orientation through the first-person compass without fixing the camera itself to north.
- Remove the compass and comfort overlay whenever gameplay ends.
- Repair the reported Begin button animation-without-start behavior.

## Implementation evidence

- Keyboard Hole-Eye movement is camera-relative, but A/D strafing and S backpedaling no longer rewrite desired camera heading.
- Camera yaw uses shortest-angle rotation with acceleration, braking, deadband, and a hard profile limit.
- Balanced is the default; Comfort, Immediate, and 30-degree Snap Turn profiles remain player-selectable and persist locally.
- Hole movement speed and gameplay steering targets are unchanged.
- First-person positional shake is disabled; small-hole camera separation is increased and blended away with growth.
- Peripheral Hunger shading follows angular speed and clears when rotation stops.
- Shared gameplay teardown calls `setHoleEyeView(false)` and removes the gameplay-active class.
- The first Begin activation timestamp starts at negative infinity rather than zero, so an immediate post-load press is accepted.
- Begin uses an explicit in-flight guard and recovers visibly from asynchronous city-build errors.

## Verification

- Source and release JavaScript pass `node --check`.
- Source and release package files match by SHA-256.
- Browser smoke confirmed Version 16.202, all four comfort profiles, active Hole-Eye compass and comfort frame, and no console warnings/errors.
- Browser game-over verification confirmed an empty body class, hidden compass, hidden comfort frame, and `VIEW: 3RD` after ending a Hole-Eye run.
- An automated immediate Begin press after `DOMContentLoaded` entered gameplay successfully; this reproduced and closed the reported first-500-ms rejection.

## Player validation

Recheck sustained Hole-Eye turning with mouse, keyboard strafing/backpedaling, Comfort and Balanced profiles, large-hole late-wave frame pacing, and personal nausea response. Motion comfort varies by player, so the profiles are deliberately adjustable.
