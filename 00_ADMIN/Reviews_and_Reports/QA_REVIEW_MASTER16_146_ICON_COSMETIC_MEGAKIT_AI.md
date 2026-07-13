# QA Review - Master 16.146 Icon, Cosmetic, MegaKit, and Rival AI

## Icon

- Approved generated Holesy artwork exported at 32, 180, 192, and 512 pixels.
- Adds favicon, Apple touch icon, standalone web-app metadata, theme colors, and a manifest.
- Existing iPhone shortcuts may retain their cached generic icon and should be deleted and added again for validation.

## Prism Orbit

- Legacy `source: run_goal` prototype unlocks are removed during local save migration.
- Permanent unlock requires every current Run Goal and the Mandate to be complete concurrently in one wave.
- Saved record uses `source: wave_goal_mandate_sweep` plus timestamp and wave number.
- Unlock remains local to the player's browser/device through `holesy.objectMastery.v1`.

## MegaKit Placement and Physics

- Chooses five valid parcel centers from `blockPositions` rather than hard-coded world coordinates.
- Removes pre-existing building/stack pieces within each reserved footprint before adding the MegaKit model.
- Marks clipped authentic model regions as skyscraper stack pieces so contact activates outward collapse, tumble, fall, and settle behavior.

## Rival AI

- Fixes `hunt_hole` speed being multiplied before initialization.
- Expands player-prey hunt range and makes visible smaller players a priority target.
- Increases hunt speed and wave scaling while retaining vision and range limits.

## Validation

- JavaScript syntax check: pass.
- Live browser smoke test required after GitHub Pages publication.
- Player validation requested for icon refresh, Prism challenge persistence, MegaKit parcel isolation/collapse, and rival hunt pressure.
