# QA Review - Master 16.77 Government House Voxel Repair

Date: 2026-06-12

## Scope

- Source: `10_SOURCE/Masters/Master 16/`
- Release package: `40_RELEASE/Website_Publish_Package/holesy/`
- Build label: `Master 16.77`

## Change Reviewed

- Supersedes the `Master 16.76` government-building attempt because live play showed government debris still shimmying under the separate government physics path.
- New government-building pieces now use the same voxel-stack collapse path as medium buildings, while preserving government/tuxedo visual geometry and metadata.
- New small house/shop buildings now wake all compact stack columns on first contact so their breakup is immediately visible.
- Government voxel save/load restores through stack metadata instead of re-entering the older separate government physics world.

## Verification

- `node --check` passed for source and release `js/main.js`, `js/government-physics.js`, and `js/build-info.js`.
- SHA256 parity passed for source/release `index.html`, `js/main.js`, `js/government-physics.js`, `js/build-info.js`, and `PACKAGE_MANIFEST.md`.
- Static grep confirmed source and release carry `Master 16.77`, `v=16.77`, `BUILD_SUB = 77`, `governmentVoxel`, `activateSmallVoxelBuilding`, and legacy small-building restore compatibility.
- Local HTTP smoke at `http://127.0.0.1:4173/index.html?fresh=16.77` confirmed the release package serves `Master 16.77`, `v=16.77`, the government voxel route, the small-house whole-stack activation route, and legacy small-building restore compatibility.
- User live-play validation on 2026-06-13 confirmed testing is complete with no defects for government-building non-shimmy fall behavior and small house/shop breakup visibility.
