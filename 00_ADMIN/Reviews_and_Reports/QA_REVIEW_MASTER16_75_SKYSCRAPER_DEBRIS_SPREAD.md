# QA Review - Master 16.75 Skyscraper Debris Spread

Date: 2026-06-12

## Scope

- Source: `10_SOURCE/Masters/Master 16/`
- Release package: `40_RELEASE/Website_Publish_Package/holesy/`
- Build label: `Master 16.75`

## Change Reviewed

- Removed the artificial inward spread limiter from non-voxel skyscraper chunks.
- Preserved the existing spread guard for medium-office voxel cubes.
- Confirmed the building-window flicker count remains equally weighted across one, two, and three flickers.

## Verification

- `node --check` passed for source and release `js/main.js` and `js/build-info.js`.
- SHA256 parity passed for source/release `index.html`, `js/main.js`, and `js/build-info.js`.
- Static grep confirmed source and release gate the spread pullback with `piece.isVoxelBuildingCube && spread > maxSpread`.
- Local HTTP check confirmed the release package serves `Master 16.75` and `v=16.75`.
- Live-play visual confirmation is still needed for skyscraper debris spread.
