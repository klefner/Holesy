# QA Review - Master 16.76 Government and House Debris

Date: 2026-06-12

## Scope

- Source: `10_SOURCE/Masters/Master 16/`
- Release package: `40_RELEASE/Website_Publish_Package/holesy/`
- Build label: `Master 16.76`

## Change Reviewed

- Government-building staged activation now keeps blast-separated pieces at that separated base instead of easing them back toward the original grid before release.
- Small house/shop buildings now spawn as compact voxel chunks with windows, doors, roof caps, support delay, and small-building save/load identity.
- Existing medium-office voxel behavior and approved skyscraper debris spread remain on their prior paths.

## Verification

- `node --check` passed for source and release `js/main.js`, `js/government-physics.js`, and `js/build-info.js`.
- SHA256 parity passed for source/release `index.html`, `js/main.js`, `js/government-physics.js`, `js/build-info.js`, and `PACKAGE_MANIFEST.md`.
- Static grep confirmed source and release carry `Master 16.76`, `v=16.76`, `BUILD_SUB = 76`, the `smallVoxel` / `smallVoxelCube` save-load path, legacy `smallBuilding` restore compatibility, and the government staged-base update after blast separation.
- Local HTTP smoke at `http://127.0.0.1:4173/index.html?fresh=16.76` confirmed the release package serves `Master 16.76`, `v=16.76`, the small-building voxel path, legacy small-building restore compatibility, the government staged-base update, and the existing voxel-only skyscraper spread guard.
- Superseded by `Master 16.77` after live play showed government pieces still visibly shimmied and small house/shop breakup was not visible enough.
