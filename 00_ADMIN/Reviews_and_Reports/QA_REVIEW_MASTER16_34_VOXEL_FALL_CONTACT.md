# QA Review - Master 16.34 Voxel Fall And Contact Tuning

Date: 2026-05-30

## Scope

- Fix mobile cases where voxel cubes appeared eaten, then reappeared on the floor after the hole moved away.
- Slow medium-office voxel descent and swallow motion.
- Make teetering columns fall to the side instead of holding a permanent lean.
- Strengthen cube-to-cube contact so falling cubes separate, bounce, and shove each other more visibly.

## Changes

- Lowered medium-office voxel gravity and terminal velocity.
- Added separate slower voxel swallow gravity so consumed cubes do not snap downward through the hole.
- Added per-cube terminal velocity scaling by height, so higher cubes can accelerate differently from lower cubes.
- Changed voxel column activation so teetering always commits into a side-fall.
- Strengthened voxel contact impulses by removing frame-time damping from collision velocity response.
- Captured whether a pending voxel cube crossed the floor while inside the target hole, then used that captured state for final consume cleanup.
- Missed cubes settle immediately at floor contact instead of briefly dipping below the ground.

## Validation

- `node --check 10_SOURCE\Masters\Master 16\js\main.js` passed.
- Source / release / full upload / delta upload hash parity passed for changed files.
- Local package responded at:
  - `http://127.0.0.1:8798/index.html?v=16.34-voxel-fall-contact`
- In-app browser smoke passed:
  - build label displayed `Master 16.34`
  - `Begin` button was present and clickable
  - no browser console errors were captured during startup / begin smoke
- `git diff --check` passed with only line-ending normalization warnings.

## Release Package

- Full GoDaddy package:
  - `C:\Users\KentLefner\Downloads\holesy-godaddy-upload-master-16.34\holesy\`
- Changed-files-only GoDaddy delta from `Master 16.33`:
  - `C:\Users\KentLefner\Downloads\holesy-godaddy-delta-master-16.34-from-16.33\holesy\`

Delta files:

- `index.html`
- `how-to-play.html`
- `js\main.js`

## Residual Risk

- Real mobile playtest should confirm floor-entered cubes do not reappear.
- Real playtest should confirm the slower descent feels more natural and that cube contact is visible without becoming chaotic.
