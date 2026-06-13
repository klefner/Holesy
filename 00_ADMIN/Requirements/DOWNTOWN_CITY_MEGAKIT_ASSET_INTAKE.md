# Downtown City MegaKit Asset Intake

Date: 2026-06-13

## Request

Import the user's downloaded Downtown City MegaKit assets into the Holesy repo/GitHub and assess how they can be used in the game without disturbing the current `Master 16.77` gameplay baseline.

## Source Location

`C:\holesy\DowntownDevour\Assets\Graphics and Art\Downtown City MegaKit`

## Repo Location

`10_SOURCE/Masters/Master 16/assets/environments/downtown-city-megakit/`

## License

The included `License_Standard.txt` identifies the free Downtown City MegaKit as CC0 1.0 Universal / Public Domain Dedication. The upstream note credits Quaternius and links to `https://quaternius.com`.

## Imported Scope

Imported:

- glTF browser candidate exports from `Exports/glTF (Godot)`
- shared PNG textures used by those glTF files
- source preview JPGs
- upstream license text
- local README documenting intended use

Not imported:

- duplicate FBX exports for Unity and Unreal
- Unreal-normal texture folder
- Unity-style `.meta` sidecar files

## Imported Inventory

- 153 `.gltf` files
- 153 `.bin` files
- 29 `.png` textures
- 3 preview `.jpg` files
- 1 license `.txt`
- 1 local `README.md`

Approximate imported size: 88 MB.

## Asset Character

The pack is a modular downtown construction kit rather than a complete ready-to-play Holesy level. It includes assembled building examples, road and intersection tiles, sidewalks, decals, doors, brick/metal/roof/trim modules, stairs, floors, and small props.

## Recommended Holesy Use

1. Keep these files as source-only assets until a specific environment slice is built.
2. Build a new environment module or test flag that loads a curated subset, not the entire pack.
3. Use the visual meshes for rendering only; use Holesy-authored simple boxes/columns/proxies for collision, destruction, swallowing, and falling physics.
4. Preserve the existing `Master 16.77` city as the default environment until the new level has passed local and user validation.
5. Before release packaging, convert the chosen subset to optimized `.glb` or cleaned glTF and update package manifests with exact shipped files.

## Candidate Implementation Paths

- Handcrafted alternate downtown level: quickest path to a visible new environment. Assemble roads, sidewalks, and a few building shells from the kit behind an environment selector.
- Procedural skin library: use kit pieces to theme existing Holesy building types while preserving current physics behavior.
- Hybrid approach: start with a handcrafted test district, then promote the best pieces into reusable procedural skins.

## Current Decision

No runtime wiring was added in this intake. This avoids loading a large asset set in the current playable build and keeps `Master 16.77` behavior unchanged.
