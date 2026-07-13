# QA Review - Master 16.164 Immutable MegaKit Deployment

Date: 2026-07-13

## Defects addressed

- GitHub Pages continued serving an older converted MegaKit `.gltf` from the reused asset URL, leaving the live building as a brown hollow lattice despite newer game code.
- Imported test buildings removed only other building objects from their selected parcel, allowing park and pool pieces to remain underneath them.

## Implementation

- Published converter pipeline `v2.3.0` under an immutable versioned directory and changed the runtime loader to that path.
- Changed test-parcel cleanup to remove every existing consumable within the parcel footprint before installing the imported building.

## Verification

- `node --check` passed for `js/main.js` and `js/build-info.js`.
- `validate-destructible-gltf.mjs` passed all 96 blocks with zero overhanging primitives and zero overhanging vertices.
- Source and release-package copies use the same `v2.3.0` asset path and files.
- Live GitHub Pages must be checked after deployment for build `Master 16.164` and the versioned asset URL; player visual acceptance remains required.

