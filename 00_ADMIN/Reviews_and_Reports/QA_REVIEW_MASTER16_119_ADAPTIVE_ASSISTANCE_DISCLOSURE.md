# QA Review - Master 16.119 Adaptive Assistance Disclosure

Date: 2026-07-10

## Scope

- One bounded player-performance adjustment in flagship Endless Wave 1.
- Visible red-square disclosure beside the timer.
- Persistent browser log and text-file export.

## Rule

After 20 active gameplay seconds, if the player has fewer than 250 points and zero combined Mandate/Run Goal progress, grant a 10% movement boost for 10 seconds. Trigger at most once per run.

## Transparency

- The red square appears for 12 seconds when the adjustment fires.
- Hover/focus text states the adjustment and reason.
- Clicking the square downloads `holesy-player-performance-adjustments.txt`.
- Each entry includes ISO date/time, build version, adjustment, and measured reason.

## Verification

- Source/release JavaScript syntax checks pass.
- Source/release runtime files match.
- The served entry point reports `Master 16.119`.
- Browser interaction and actual download behavior remain for live-play validation.
