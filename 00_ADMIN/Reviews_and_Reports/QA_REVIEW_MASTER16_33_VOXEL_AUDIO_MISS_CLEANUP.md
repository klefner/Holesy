# QA Review - Master 16.33 Voxel Audio And Missed-Hole Cleanup

Date: 2026-05-29

## Scope

- Fix medium-office voxel cube consume audio overlap that could sound like static.
- Fix voxel cubes disappearing below the ground after the player moved away from the original hole entry point.
- Preserve the modular `Master 16` package structure.

## Changes

- Added a short, budgeted voxel cube impact sound path.
- Limited medium-office voxel impact audio to five simultaneous voices per building, with a small per-stack trigger interval.
- Deferred scoring / reward application for voxel cubes until final hole entry instead of the initial fall trigger.
- Added a final still-over-hole check for voxel cubes before removal.
- If a pending voxel cube misses the hole, it now lands at ground height as visible settled debris.

## Validation

- `node --check 10_SOURCE\Masters\Master 16\js\main.js` passed.
- Source / release / full upload / delta upload hash parity passed for changed files.
- Local package responded at:
  - `http://127.0.0.1:8797/index.html?v=16.33-voxel-audio-miss-cleanup`
- In-app browser smoke passed:
  - build label displayed `Master 16.33`
  - `Begin` button was present and clickable
  - no browser console errors were captured during startup / begin smoke
- `git diff --check` passed with only existing line-ending normalization warnings.

## Release Package

- Full GoDaddy package:
  - `C:\Users\KentLefner\Downloads\holesy-godaddy-upload-master-16.33\holesy\`
- Changed-files-only GoDaddy delta from `Master 16.32`:
  - `C:\Users\KentLefner\Downloads\holesy-godaddy-delta-master-16.33-from-16.32\holesy\`

Delta files:

- `index.html`
- `how-to-play.html`
- `js\main.js`

## Residual Risk

- Real playtest should confirm the new short cube impact audio reads as crunchy building debris instead of becoming too quiet.
- Real playtest should confirm missed-hole cubes remain visible and consumable after landing.
