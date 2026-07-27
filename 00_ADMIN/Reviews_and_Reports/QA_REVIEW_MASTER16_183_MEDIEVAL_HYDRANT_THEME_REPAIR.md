# QA Review: Master 16.183 Medieval Hydrant Theme Repair

Date: 2026-07-27

## Scope

- Remove fire hydrants and their water-jet response from Medieval Village.
- Preserve fire hydrants and their special response in modern cities.
- Prevent a legacy Medieval save from restoring a hydrant into the themed town.
- Replace other modern roadside rolls on Medieval native parcels with appropriate small devourables.

## Implementation

- Medieval native parcels now roll baskets, sacks, crates, barrels, hay, carts, trees, and torch-bearing villagers instead of the modern sidewalk pool.
- Saved hydrants restore as barrels when the active environment is Medieval Village.
- Runtime instrumentation reports the actual surviving hydrant population after city construction and arena pruning.

## Verification

- `node --check` passed for `js/main.js` and `js/build-info.js`.
- SHA-256 comparisons confirmed source/release parity for `index.html`, `js/main.js`, and `js/build-info.js`.
- Automated local browser boot selected Medieval Village and confirmed:
  - environment: `medievalVillage`
  - live hydrants: `0`
  - cars: `0`
  - loose Medieval edibles: `448`
  - Medieval models: `32`
  - Medieval commons: farm, pasture, training yard, and barnyard
- A separate Classic City boot confirmed `83` live hydrants, proving the modern-city behavior remains populated rather than being removed globally.
- Screenshot review confirmed the gameplay scene and HUD rendered after the Medieval boot.

## Result

Pass. Medieval Village no longer creates or restores fire hydrants, so the associated water jet cannot occur there. Modern cities retain hydrants.
