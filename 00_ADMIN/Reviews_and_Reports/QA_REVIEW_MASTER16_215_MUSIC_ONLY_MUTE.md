# QA Review — Master 16.215 Music-Only Mute

Date: 2026-08-12

## Reported Issue

The top-right control is labeled `Music`, but switching it off also silenced devour, combat, UI, environmental, reward, building, wave, and Pause sounds.

## Root Cause

- The Web Audio graph already had separate procedural-music, Archive-music, SFX, ambience, celebration, and Pause gain nodes.
- The shared `music.muted` preference was nevertheless checked inside the gameplay-audio permission helper and throughout individual SFX call sites.
- `updateAidDrops()` also returned when gameplay audio was disallowed, so the Music preference could freeze aid ships and falling or hovering pickups.
- Begin always called the score scheduler after initializing Web Audio, which could schedule an inaudible score when Music had been disabled before play.

## Correction

- Retain `music.muted` as the single preference for procedural and Archive music only.
- Remove that preference from gameplay SFX, ambience, celebration, wave, building, combat, devour, and Pause owners.
- Keep focus, active-game, and Pause-state controls as the independent gameplay-audio safety boundary.
- Always initialize Web Audio and warm sample banks from the Begin gesture; start the score only when Music is enabled.
- Prevent `startMusic()` from creating a silent scheduler while Music is disabled or focus-suspended.
- Run aid movement and pickup simulation regardless of audio permission while continuing to gate only its chimes, radar, and loop audio.
- Advance the `build-info.js` module cache key with the build so the public badge and changelog cannot be rewritten by a cached prior module.

## Acceptance Matrix

- Music on → off during play: procedural and Archive scores stop; non-music buses remain available.
- With Music off, devour samples and generic consume pops, combat, collapse, vehicle, warning, reward, wave-completion, and hole-wind sounds remain eligible.
- Music off before Begin: AudioContext and samples initialize, score scheduler remains stopped, and gameplay SFX work immediately.
- Music off during Pause: gameplay audio is stopped before the isolated Pause cue; Resume restores SFX and ambience but does not restart music.
- Music on after Resume or during play: score starts without interrupting or duplicating non-music sources.
- Focus loss: the whole AudioContext still suspends; focus return honors the Music choice.
- Aid ships, falling aid, hovering pickups, and drop countdowns continue regardless of the Music preference.

## Architecture and Performance

The repair uses the existing separated Web Audio buses and removes invalid preference coupling. It adds no new audio nodes, render-loop work, geometry, physics bodies, or object scans. Preventing silent score scheduling slightly reduces work when Music is off.

## Verification Evidence

- `node --check` passed for source and release `js/main.js` and `js/build-info.js`.
- `audio-mute-separation-static.test.mjs` passed and rejects future `music.muted` coupling in every audited non-music owner.
- Existing city-build isolation and Harvest collapse contract tests passed.
- Source/release SHA-256 parity passed for the changed package files.
- Local browser, Music off before Begin: AudioContext reached `running`, both supplied priority samples decoded, gameplay bus remained open, and active score sources stayed at zero.
- Local browser, Music still off: steering through Harvest objects increased generic consume-pop telemetry from 0 to 3 while active score sources remained zero.
- Local browser, Pause/Resume with Music off: the Pause cue count increased once, the gameplay bus changed true → false across Pause/Resume, and music remained stopped.
- Local browser, Music restored: active score sources reached 14 while the gameplay bus remained open and the consume-pop count remained intact.
- Local browser console error count was zero. Focus lifecycle and aid-drop preference independence are covered by the existing whole-context suspension path plus the new static ownership contract; publication proof is recorded separately after GitHub Pages verification.
- The first public probe correctly returned the new 16.215 HTML and main script but exposed a cached 16.214 build-info module; advancing its import key and issuing a one-time `main.js?v=16.215.1` cache token corrected the package. The static contract now compares both module cache keys with `BUILD_SUB`.
- GitHub Pages workflow `31619921544` completed successfully for deployment commit `32bd222`.
- Final cache-busted public verification returned `Master 16.215`, visible `Version 16.215`, and `main.js?v=16.215.1`, with no boot error and zero console errors.
