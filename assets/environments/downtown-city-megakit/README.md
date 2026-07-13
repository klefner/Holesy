# Downtown City MegaKit

Imported on 2026-06-13 from the user's local download:

`C:\holesy\DowntownDevour\Assets\Graphics and Art\Downtown City MegaKit`

Source: Quaternius Downtown City MegaKit free version.

License: CC0 1.0 Universal, per `License_Standard.txt`.

## Imported Contents

- `source-gltf/`: browser-facing glTF 2.0 candidate exports, including `.gltf`, `.bin`, and shared `.png` textures.
- `previews/`: source preview images for quick visual reference.
- `License_Standard.txt`: upstream license and attribution/support note.

The duplicate FBX export folders and Unreal-normal texture folder were intentionally left outside the repo import because Holesy is a browser game and the glTF export is the useful runtime candidate set.

## Current Status

These assets are source assets only. They are not wired into `Master 16.77` gameplay and should not be copied into the release package until a specific environment slice is implemented and tested.

## Candidate Uses In Holesy

- New alternate downtown level/theme with assembled streets, sidewalks, and kitbashed buildings.
- Visual source for future procedural building skins while preserving existing voxel/destruction physics.
- Props and decals for richer road/intersection dressing.
- Optional GLB-optimized runtime subset after glTF Transform cleanup.

## Runtime Notes

- Default browser shipping format should be optimized `.glb` or glTF 2.0, not FBX.
- Build collision/proxy data in Holesy code instead of treating these visual meshes as physics bodies.
- Keep the current `Master 16.77` city as the default level until the new environment is behind an explicit selector or test flag.
- Before release packaging, generate a curated optimized subset and update package manifests with exact files.
