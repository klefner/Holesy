# QA Review - Master 16.158 Mandate State, Arrow, and Boss Names

## Defects Addressed

- Completing a Mandate early did not reliably turn the card green.
- Completing during the red deadline warning did not reliably replace both red borders with the full green confirmation.
- The arrow did not visibly explode into the center and travel to the Mandate card.
- Boss names remained too small to read.

## Root Cause and Repair

- HUD visuals depended on cached `mandateCollected` and `mandateComplete` state.
- The visible objective rows are now authoritative on every HUD refresh.
- Completed-row count is recalculated before completion and warning decisions.
- A completed set corrects the cached gameplay completion flag immediately, preventing a later false red warning.
- The pre-completion red body/card state is captured before switching state, guaranteeing the late-success path starts.

## Motion and Contrast

- Normal completion uses a persistent mint-green card with forced dark high-contrast text.
- Late completion runs eight alternating half-cycles, producing four bright green peaks in exactly four seconds.
- The arrow begins at 360px, expands to approximately 486px, travels to the Mandate card, shrinks to approximately 187px, then flashes five times.
- Arrow sounds use the same 1.45-second arrival delay as the visual.
- Boss sprites increase from 6.2×1.16 to 18.6×3.48 world units with a 1024×192 canvas.

## Validation Focus

- Complete a one-object Mandate within ten seconds and verify immediate persistent green.
- Enter the final 15 seconds incomplete, then finish and verify red disappears immediately.
- Count four bright green screen/card pulses lasting four seconds.
- Verify no red warning can begin after the Mandate is already complete.
- Observe the arrow's large center explosion, travel, shrink, and five destination flashes.
- Read every boss name during descent and ground combat at desktop and mobile sizes.
