# QA Review - Master 16.35 Voxel Performance Guardrails

Date: 2026-05-30

## Scope

Validate the defect response for the first major medium-office voxel performance regression introduced by richer cube physics.

## Changes Reviewed

- Medium-office voxel cubes increased from `1.1` to `1.48` world units.
- Procedural medium-office dimensions reduced from `5-10` per axis to `4-7` footprint cubes and `4-8` floors.
- Voxel contact resolution no longer runs an unbounded all-pairs active-cube loop.
- Voxel contact resolution now uses a capped per-building contact budget.
- Voxel impact checks against nearby objects now use per-frame and per-cube budgets.
- Voxel horizontal speed and angular spin are clamped after physics updates and impacts.

## Validation

- `node --check 10_SOURCE/Masters/Master 16/js/main.js` passed.
- `git diff --check` passed with only Git line-ending warnings.
- Source, release package, full GoDaddy package, and delta GoDaddy package hashes match for changed game files.
- Browser smoke passed at:
  - `http://127.0.0.1:8798/index.html?v=16.35-voxel-perf`
- Browser smoke confirmed:
  - title screen loads
  - build label displays `Master 16.35`
  - Begin starts gameplay
  - canvas renders
  - console warnings/errors are empty during smoke

## Remaining Risk

Real gameplay performance during deliberate large office-cube collection still needs user device validation. This patch reduces body count and caps the most expensive loops, but the final feel of cube falling and contact remains a gameplay-tuning surface.
