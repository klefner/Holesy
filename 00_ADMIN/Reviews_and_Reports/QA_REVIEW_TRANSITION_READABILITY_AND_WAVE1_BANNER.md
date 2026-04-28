# QA Review: Transition Readability And Wave 1 Banner

## Scope
- Review the wave-transition readability update
- Review the end-screen cleanup for the red troop banner
- Review the Wave 1 troop-banner regression against the established wave config

## Inputs Reviewed
- User defect report describing:
  - transition text is too brief for slow readers
  - white text is hard to read over the live battlefield
  - the red troop banner remains visible on the end-game scoreboard
  - Wave 1 incorrectly shows a static `99 seconds` troop countdown
- Active source file:
  - `Master 4.html`

## QA Analysis
- Wave 1 is configured as troop-free with:
  - `soldiersEnabled: false`
  - `spawnIntervalMin: 99.0`
  - `spawnIntervalMax: 99.0`
- That `99` value is a placeholder and should not be surfaced in the HUD when troop drops are disabled.
- The wave-HUD fix now hides the troop banner unless:
  - the game is in `PLAYING`
  - Waves mode is active
  - troop deployments are enabled for the current wave
- Transition messaging now has:
  - a longer briefing duration
  - a longer transition duration
  - a delayed next-wave start so the lore can be read before gameplay resumes
- Readability was improved by:
  - adding a full-screen translucent backdrop behind the event banner
  - keeping the battlefield visible but dimmed while the message is active
- End-game cleanup now explicitly hides:
  - the troop banner
  - any active transition/event banner
  - the transition backdrop

## Findings
- Critical findings: none
- Non-critical note:
  - the event banner still uses its existing bright card styling, but the added dimmed backdrop materially improves readability without over-obscuring the battlefield

## QA Approval
Approved. No critical gaps remain in the inputs, analysis, or patch behavior for this change set.
