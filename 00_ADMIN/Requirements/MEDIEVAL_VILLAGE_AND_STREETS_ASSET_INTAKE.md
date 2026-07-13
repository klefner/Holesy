# Medieval Village And Streets Asset Intake

Date: 2026-06-13
Status: source-only intake for future theme/world work
Current runtime impact: none
Current release package impact: none

## Sources

Medieval village:

- Drive folder: https://drive.google.com/drive/folders/18T92VcNldHG0ugWo1u0hSJGDLua-n7o5
- Root inventory: `Buildings`, `Props`, `Preview.jpg`, `License.txt`
- License text: LowPoly Models by @Quaternius, CC0 1.0 Universal / Public Domain Dedication

Streets pack:

- Drive folder: https://drive.google.com/drive/folders/1-YvMpLDYBIy-0Ms7ZlFmUpChTbHku8IJ
- Root inventory: `Blends`, `FBX`, `OBJ`, `Preview.png`, `License.txt`
- License text: Street Pack by Quaternius, CC0 1.0 Universal / Public Domain Dedication

## Local Source Locations

- `10_SOURCE/Masters/Master 16/assets/environments/source-packs/medieval-village-quaternius/`
- `10_SOURCE/Masters/Master 16/assets/environments/source-packs/streets-pack-quaternius/`

These are source-intake folders only. Do not load them in gameplay until a future implementation pass creates optimized browser-ready subsets, likely GLB/glTF or Holesy-authored procedural equivalents.

## Captured Local Coverage

Medieval village:

- Complete building OBJ/MTL set captured: `Bell_Tower`, `Blacksmith`, `House_1`, `House_2`, `House_3`, `House_4`, `Inn`, `Mill`, `Sawmill`, `Stable`.
- Complete building FBX and Blender duplicate source sets captured.
- Partial props Blender source captured before Google Drive blocked one public file URL during folder download.
- Medieval props OBJ and FBX folders remain Drive-linked source for a later targeted fetch/conversion pass.

Streets pack:

- Full Drive inventory was visible for `Blends`, `FBX`, and `OBJ`.
- Public folder-style download failed before file transfer with a Drive public-link retrieval error.
- Usable OBJ inventory seen in Drive includes street modules (`Street_Straight`, `Street_3Way`, `Street_4Way`, curves, deadends, bridges, elevated/water variants), streetlights, traffic lights, and road signs.

## Candidate Holesy Uses

- Medieval settlement theme: houses, inn, mill, blacksmith, bell tower, stable, market/well/prop dressing once props are fetched or converted.
- Streets/environment toolkit: modular road pieces, streetlights, traffic lights, signs, bridges, elevated roads, and water-road variants.
- Future theme architecture validation: use these packs to prove that environment packs can define roads/paths, props, object families, and lighting behavior without rewriting consumption, scoring, AI, or wave logic.

## Implementation Rules

- Keep Classic Aldine and the optional MegaKit Downtown test environment unchanged.
- Do not add these raw source folders to the active scene or release package.
- Convert only selected assets for browser runtime use, and verify size/performance before adding them to `/holesy/`.
- Preserve the original Drive links and license text in any future optimized subset.
