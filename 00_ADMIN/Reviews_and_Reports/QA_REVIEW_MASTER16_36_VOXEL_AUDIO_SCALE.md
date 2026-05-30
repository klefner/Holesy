# QA Review - Master 16.36 Voxel Audio And Scale Cleanup

Date: 2026-05-30

## Scope

Validate the medium-office voxel cleanup for excessive/static audio and shrinking roof cubes during hole-entry falls.

## Changes Reviewed

- Voxel cubes no longer scale down during the falling-into-hole animation.
- Voxel cube scoring routes to the sparse voxel impact sound instead of the full building-collapse sound.
- Voxel column activation routes to the sparse voxel impact sound instead of `playBuildingSound('mid')`.
- Voxel audio is capped at one active voice per building stack with a 650ms minimum interval.
- Voxel audio duration is capped at 0.18 seconds with lower gain and higher playback rate.

## Validation

- `node --check 10_SOURCE/Masters/Master 16/js/main.js` passed.
- `git diff --check` passed with only Git line-ending warnings.
- Source, release package, full GoDaddy package, and delta GoDaddy package hashes match for changed game files.
- Browser smoke passed at:
  - `http://127.0.0.1:8798/index.html?v=16.36-voxel-audio`
- Browser smoke confirmed:
  - title screen loads
  - build label displays `Master 16.36`
  - Begin starts gameplay via DOM click
  - console warnings/errors are empty during smoke

## Remaining Risk

Audio feel still requires real playtesting while actively consuming medium-office voxel columns. Browser automation can verify wiring and runtime health, but not subjective sound quality.
