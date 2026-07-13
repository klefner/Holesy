# QA Review - Master 16.154 Mandate Success and Arrow

## Requested Behavior

- A completed Mandate box must become green without sacrificing text contrast.
- If the red game border is active when the Mandate is solved, both the card and game border must become green and pulse for four seconds.
- The deadline arrow must explode into the center, move to its existing pointer position, and only then begin flashing.

## Implementation

- Completion uses a light mint-green card, dark green-black text, dark completion dots, and translucent light target rows.
- Late completion removes the red card and screen warning immediately.
- A synchronized green card and full-screen border pulse runs for 4000 milliseconds.
- The arrow uses a center pop, overshoot, settle, destination travel, and five 1.1-second flashes.
- Flash alert sounds receive the same 1.1-second arrival delay.
- Arrow and success timers are cleared at wave start and transition.
- Reduced-motion preference removes travel and pulsing while retaining state color and destination clarity.

## Validation Focus

- Complete a Mandate before 15 seconds and verify the card remains green without a screen pulse.
- Enter the final 15 seconds incomplete, then finish the Mandate and time the green confirmation at four seconds.
- Verify no red border remains beneath the green state.
- Verify the arrow begins centered, moves beside the Mandate box, then flashes five times with synchronized sounds.
- Check desktop, iPhone portrait, and iPhone landscape arrow destinations.
