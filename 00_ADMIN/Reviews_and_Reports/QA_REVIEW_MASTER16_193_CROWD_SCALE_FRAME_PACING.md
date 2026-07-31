# QA Review - Master 16.193 Crowd-Scale Frame Pacing

Date: 2026-07-30

## Scope

Improve frame pacing when Harvest County contains many visible objects without reducing the town population or changing gameplay rules.

## Changes Reviewed

- Per-frame object interaction begins from a 12-unit world-cell proximity index rather than the complete active-object collection.
- Each living hole queries only the cells that can contain valid eating, pulling, jam, or blocked-object candidates.
- Passive pedestrians, animals, and tumbleweeds update at 30 Hz using accumulated elapsed time.
- Adaptive rendering responds after one two-second slow window, can temporarily reduce pixel ratio to 0.75, and recovers gradually after stable frame windows.
- Source and release `main.js` files remain byte-identical.

## Verification

- `node --check` passed for source and release `js/main.js`.
- Source/release SHA-256 comparison passed after the implementation.
- Harvest County booted in Endless Waves through the local no-cache server.
- The crowded scene rendered at 1280 x 720 with no browser console errors.
- Run Goals, Mandate, rivals, timer, Pause, Time, and Music HUD surfaces remained active.
- Debug state confirmed active gameplay, the low performance profile, DPR 1.00, shadows off, live rivals, and normal wave timers.

## Risk Notes

- Passive ambient movement has at most about 33 milliseconds of scheduling latency; panic detection and movement remain visually responsive.
- Interaction query radius is derived from the maximum valid fit-and-pull distance, so distant objects that cannot interact are intentionally excluded.
- Full subjective smoothness still requires user validation on the original crowded hardware/browser combination.

## Result

Pass for local review. Publication remains pending.
