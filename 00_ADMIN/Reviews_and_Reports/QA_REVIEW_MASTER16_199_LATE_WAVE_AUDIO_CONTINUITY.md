# QA Review - Master 16.199 Late-Wave Audio Continuity

Date: 2026-08-06

## Player finding

In Harvest wave 5, a very large hole and high overhead view exposed nearly the whole populated playfield. Sound disappeared under that combined render and consumption load.

## Root cause and correction

- The procedural score used a main-thread timer and attempted to schedule every missed note after a delayed tick. A late render frame could therefore create a catch-up burst in the Web Audio graph.
- Ordinary consumption samples had no shared non-music ceiling. A large hole could start many long samples together even though building and gunshot categories had narrower limits.
- Finished sample gain and reverb nodes were not explicitly disconnected.
- Master 16.199 skips missed musical steps after a stall, resumes scheduling ahead of the current hardware clock, limits ordinary non-music playback to 24 simultaneous sources, reserves three additional priority voices, and disconnects completed audio graphs.

## Product-intent protection

- Master 16.198 is accepted as the new Harvest town baseline.
- No objects, actors, parcels, buildings, food value, progression supply, or visual density were removed or reduced.
- The correction changes only audio scheduling, audio resource lifetime, and diagnostic telemetry.

## Required validation

- Source and release JavaScript syntax checks.
- Exact source/release parity for all changed player-facing files.
- Browser smoke with Harvest selected, AudioContext running, no console errors, and new audio telemetry present.
- Stress evidence that active non-music voices do not exceed 27 including the priority reserve and that scheduler recovery does not enqueue a missed-note backlog.
- GitHub Pages publication and public 16.199 metadata/content verification.

## Validation result

- Source and release `main.js` and `build-info.js` pass `node --check`.
- Changed source and release package files match exactly after line-ending normalization.
- Harvest browser smoke loaded Version 16.199 with 3,019 current object records, a running AudioContext, no boot error, and the new telemetry present.
- An induced 700 ms main-thread stall incremented scheduler recovery exactly once; the AudioContext remained running and active SFX voices returned to zero after recovery.
- Source inspection confirms ordinary sources stop at 24 concurrent voices and priority playback stops at 27.

## Release status

Pass for local implementation review. GitHub Pages verification pending publication.
