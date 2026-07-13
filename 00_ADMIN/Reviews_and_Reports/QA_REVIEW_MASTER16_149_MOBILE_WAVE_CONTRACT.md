# QA Review - Master 16.149 Mobile Wave Contract

## Beta Feedback Addressed

- Mobile signage overlapped Pause and boss messaging.
- Menu could scroll horizontally.
- Menu actions consumed excessive width.
- New player did not understand the timer, mandatory Mandate, or optional Run Goals.

## Repair

- Mobile menu action buttons stack vertically at full available width.
- Root/menu surfaces suppress horizontal overflow.
- Mobile event banner reserves the right-side Pause/control area.
- Every wave begins with gameplay and timer frozen.
- Centered red card: `MANDATE — DO THIS OR THE RUN ENDS` and exact targets.
- Centered gold card: `RUN GOALS — OPTIONAL, BUT SHINY` and exact goals.
- Supporting copy says goals provide buffs, mastery, and unlocks but are not required for survival.
- Cards hold for 2.8 seconds, dock toward their HUD sides, and release gameplay at 3.7 seconds.
- Reduced-motion preference removes nonessential card transition.

## Validation Focus

- iPhone portrait and landscape: no horizontal scrolling.
- Menu actions form a vertical stack.
- Pause remains visible while boss messages display.
- Wave timer does not decrement during the contract briefing.
- Mandate and Goal copy fits without clipping.
- Controls begin only after the docking transition.
