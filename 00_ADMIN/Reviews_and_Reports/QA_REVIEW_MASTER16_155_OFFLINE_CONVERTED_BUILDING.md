# QA Review - Master 16.155 Offline-Converted Building

## Conversion Executed

- Source: `Building_Small_1.gltf` plus its binary geometry and referenced MegaKit textures.
- Converter: `scripts/convert-megakit-building-small-1.mjs`.
- Output: `Building_Small_1_destructible.gltf` and `Building_Small_1_destructible.bin`.
- Recipe version: `holesy-offline-destructible 1.0.0`.
- Source SHA-256: `8e1235397ee3905d0bfbbe8f64a5197246a9be1af700983920c4f1a691c95c18`.

## Converted Contract

- 2 columns by 2 rows by 3 floors.
- 12 individually named and consumable blocks.
- Six closed faces per block.
- Original `MI_RedBrick_Pale` material for facades.
- Original `MI_Trim_Dark` material for roof faces.
- Original `MI_InteriorWall` material for newly exposed undersides.
- One box collision proxy per block.
- Standard building fall and grounded-sleep physics presets.

## Runtime Integration

- The browser loads the converted glTF directly.
- Runtime geometry construction for this asset has been removed.
- Loading fails loudly unless exactly 12 converted block nodes are present.
- Five parcels receive the same converted model at different rotations for visual and destruction testing.

## Validation Focus

- Confirm genuine MegaKit brick, trim, and other referenced textures load without 404 errors.
- Inspect the building before destruction for solid walls and a closed roof.
- Consume individual blocks and confirm there is no hollow shell.
- Verify all pieces fall as rectangular solid blocks, collide, stop spinning, and settle.
- Confirm the same output is reproduced by rerunning the converter with an unchanged source.
