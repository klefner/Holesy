# QA Review - Master 16.200 Wave-Five Music Source Leak

Date: 2026-08-06

## Player finding

Master 16.199 still began stuttering in wave five for roughly eight seconds before all sound disappeared. The 16.199 scheduler recovery and non-music voice cap were therefore insufficient.

## Confirmed cumulative cause

- The procedural score creates approximately 22 oscillator or buffer sources per second.
- Every scheduled score source was appended to `music.activeSources`.
- Unlike Archive sources and Master 16.199 non-music sources, completed score sources had no `onended` cleanup.
- Their shared filters, envelopes, noise nodes, and vibrato nodes also remained connected.
- A five-wave session could therefore retain thousands of ended source references and connected audio nodes, matching the late cumulative onset and permanent audio failure reported by the player.

## Correction

- `music.activeSources` is now a set containing only currently scheduled or sounding score sources.
- Each note group removes and disconnects every ended source.
- Shared filter, envelope, and modulation nodes disconnect after the group's final source ends.
- Emergency music shutdown still stops and cleans every active source.
- Current and peak music-source counts are exported as runtime telemetry.
- No Harvest object, actor, parcel, building, food value, or visual-density rule changed.

## Required validation

- Source and release syntax and exact parity.
- Browser Harvest smoke with running AudioContext and no boot error.
- Endurance sampling must show current scheduled source count returning to a small bounded range rather than growing with elapsed play time.
- Scheduler-stall recovery and Master 16.199 SFX caps remain present.
- Public Pages metadata and code verification after deployment.

## Validation result

- Source and release `main.js` and `build-info.js` pass `node --check` and changed player-facing files match exactly after line-ending normalization.
- Harvest loaded as Version 16.200 with a running AudioContext and no boot error.
- Six samples over 30 seconds reported active music-source counts of 16, 12, 11, 11, 16, and 11. Peak count remained 19 instead of increasing with elapsed play time.
- After an induced 700 ms main-thread stall, active music sources returned to 11, peak remained 19, scheduler recovery incremented, and AudioContext remained running.
- Visual browser smoke showed the normal dense Harvest playfield and unobstructed HUD.

## Release status

Pass for implementation and bounded-source endurance review. GitHub Pages commit `a18ccd5` completed successfully; public HTTP 200 verification confirmed build 200 metadata, the live-source set, per-note group cleanup, the retained SFX ceiling, and music-source telemetry. Player wave-five confirmation remains required.
