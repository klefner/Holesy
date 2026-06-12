# QA Review - Master 16.72 Building Window Flicker

Date: 2026-06-11

## Scope

- Source: `10_SOURCE/Masters/Master 16/`
- Release package: `40_RELEASE/Website_Publish_Package/holesy/`
- Build label: `Master 16.72`

## Change Reviewed

- Lit building windows now flicker briefly before going dark when their building starts coming apart.
- Time-of-day cycling still cannot relight windows after their building has lost power.

## Verification

- `node --check` passed for source and release `js/main.js` and `js/build-info.js`.
- SHA256 parity passed for source/release `index.html`, `js/main.js`, and `js/build-info.js`.
- Static grep confirmed source and release contain the building-window `powerCut`, `flickerUntil`, and `updateBuildingLightPowerCut()` path.
- Live-play visual confirmation is still needed for the exact building-window flicker feel.
