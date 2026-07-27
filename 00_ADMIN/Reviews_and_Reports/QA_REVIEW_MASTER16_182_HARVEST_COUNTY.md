# QA Review - Master 16.182 Harvest County

Date: 2026-07-27

## Scope

- First new town from `NEW_TOWN_ASSET_ALLOCATION_MATRIX.md`
- Harvest County consumption density and theme integrity
- Medieval Village density, animal visibility, and modern-car exclusion
- Shared imported-building destruction compliance
- Source/release parity

## Implementation evidence

- Harvest County is a declarative city recipe and pack populator, not a replacement population branch for the existing towns.
- Runtime assets come from the license-cleared Quaternius Farm Buildings, Farm Animals, Nature Crops, and Simple Nature packs.
- Runtime ships 22 optimized GLBs. Raw ZIP archives remain under `50_ASSETS/Graphics and Art/` and raw OBJ intake is excluded from the release package.
- Small Barn, Barn, Big Barn, Silo, Water Tower, and Windmill use immutable `v1.0.0` authored-surface destruction conversions.
- Harvest County explicitly spawns zero cars and filters car/person Run Goals.
- Medieval Village explicitly spawns zero cars, guarantees farm, pasture, barnyard, and training-yard commons, and includes horses.

## Automated checks

- `node --check` passed for source and release `js/main.js`.
- All six Harvest County destructible glTF files passed `scripts/validate-destructible-gltf.mjs`.
- Validator results: expected 96/128 blocks, zero overhanging primitives, zero overhanging vertices, closed cores present, and authentic surface triangles retained.
- Source/release hashes match for `index.html`, `js/main.js`, and `js/build-info.js`.
- Browser console produced zero errors or warnings for directed Harvest County and Medieval Village starts.

## Browser playtest evidence

Harvest County, Endless Wave 1:

- environment: `harvestCounty`
- building sites loaded: 8
- themed edibles: 792
- roaming animals: 42
- cars: 0
- immediate starter ring: 20 visible props around the opening hole
- observed goal families contained no cars or people

Medieval Village, Endless Wave 1:

- authored building models loaded: 32
- loose parcel edibles: 448
- ambient actors: 43
- guaranteed commons: `training_yard`, `farm`, `pasture`, `barnyard`
- cars: 0 by recipe
- observed goals: soldiers, street objects, and animals; no cars

## Result

Pass for directed live review as `Master 16.182`.

The temporary City Override remains intentionally visible so Harvest County can be selected directly. This remains a known release gate and must be removed before final market release.
