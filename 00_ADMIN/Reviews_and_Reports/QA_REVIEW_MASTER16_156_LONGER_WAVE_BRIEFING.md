# QA Review - Master 16.156 Longer Wave Briefing

## Feedback Addressed

The centered Mandate and Run Goals cards moved into the HUD before the player could finish reading them.

## Repair

- Centered hold increased from 2800 milliseconds to 5500 milliseconds.
- Docking completes at 6500 milliseconds.
- Gameplay, input consequences, combat, and the wave timer remain frozen until docking completes.
- The docking animation itself remains approximately one second, avoiding an unnecessarily slow visual transition.

## Validation Focus

- Read every Mandate and all three Run Goals before movement begins.
- Verify cards stay stationary and centered for 5.5 seconds.
- Verify the timer does not decrement during the briefing.
- Check desktop, iPhone portrait, and iPhone landscape layouts.
