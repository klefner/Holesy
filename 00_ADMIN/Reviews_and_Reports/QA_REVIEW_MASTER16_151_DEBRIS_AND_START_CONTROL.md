# QA Review - Master 16.151 Debris and Starting Control

## Defects Addressed

- Fallen building pieces could spin indefinitely after reaching the ground.
- The smallest starting hole felt imprecise because pointer and touch input could place its steering target far away.

## Repair

- Building debris receives a guaranteed sleep after 0.75 seconds of low-speed ground contact.
- Sleeping explicitly clears linear and angular velocity; voxel pieces also snap to a stable ground face.
- Settled voxel pairs ignore harmless contacts and only wake for a material impact.
- Mouse targets are distance-limited while the hole is small.
- Touch uses the same radius-aware reach plus a gentler response curve for short gestures.
- Player movement speed and the growth-speed relationship are unchanged.

## Validation Focus

- Collapse houses, government buildings, skyscrapers, and imported voxel buildings; verify every grounded piece stops rotating.
- Nudge settled debris with other debris; verify minor resting contacts do not restart spinning.
- Compare mouse and touch precision during the first 15 seconds with later larger-hole control.
- Verify the hole retains its existing top movement speed.
