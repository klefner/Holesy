# QA Review - Master 16.208 Pause Audio Exclusivity

Date: 2026-08-07

## Scope

End all gameplay audio, including procedural music and continuous world loops, before the Pause cue begins. Keep the cue audible through an isolated pause-only output and restore only appropriate continuous systems on Resume.

## Acceptance Checks

- A valid pause calls the complete gameplay-audio teardown before `playPauseHumSound()` and before Pause state/overlay changes.
- Teardown stops scheduled music sources and scheduler, active one-shot SFX, hole wind, aid chimes, and plane-engine drones.
- Gameplay SFX, ambience, and celebration buses reach zero immediately rather than fading over the Pause cue.
- The shared music/SFX/reverb output reaches zero before teardown so no scheduled source or convolution tail overlaps the Pause cue.
- The Pause cue connects to a dedicated output that is not muted with gameplay buses.
- Resume stops the Pause cue before restoring any gameplay audio.
- Resume restarts procedural music when enabled and rebuilds wind/engine audio only when active gameplay requires it.
- Interrupted one-shot gameplay effects are not replayed.
- Wave-transition Pause/Resume restarts the score without starting active-play-only wind or plane audio.
- Source and release modular packages remain byte-identical.

## Result

Passed local implementation and browser verification:

- `node --check` passed for source and release `main.js` and `build-info.js`.
- Static ordering confirmed complete gameplay-audio teardown precedes the isolated Pause cue, which precedes Pause state/overlay changes.
- Browser telemetry measured 13 active music sources before Pause and 0 during Pause.
- During Pause, the gameplay bus reported muted, the Pause cue reported active, and exactly one non-music source remained: the isolated cue itself.
- Resume stopped the Pause cue, reopened the gameplay bus, left interrupted one-shots at zero, and repopulated scheduled music from 0 to 17 sources within 1.55 seconds.
- A second sample remained healthy at 14 scheduled music sources after 3.05 seconds.
- Browser console checks returned no errors.
- Source/release SHA-256 parity passed for every changed browser-package file.

## Publication Verification

- GitHub Pages deployment commit `05d9b42` completed successfully in workflow `31203444233`.
- The public test URL returned HTTP 200 and displayed `Version 16.208`.
- The deployed `js/main.js` returned HTTP 200 and contained both `silenceGameplayAudioForPause()` and the isolated `getPauseDestination()` path.
- Public test URL: `https://klefner.github.io/Holesy/?v=16.208-05d9b42`
