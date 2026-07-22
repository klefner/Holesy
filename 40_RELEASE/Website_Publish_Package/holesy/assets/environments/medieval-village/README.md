# Medieval Village Runtime Assets

Browser-ready GLB conversions for Holesy's automatically selected Medieval Village district.

Source: Quaternius Medieval Village, LowPoly Models by @Quaternius.

License: CC0 1.0 Universal / Public Domain Dedication.

Conversion:

- source format: OBJ/MTL from `assets/environments/source-packs/medieval-village-quaternius/Buildings/OBJ/`
- runtime format: binary glTF 2.0 (`.glb`)
- converter: `obj2gltf`
- options: Y-up input/output, embedded buffers/materials, double-sided materials, secure local dependency reads
- runtime collision: Holesy coarse solid-block destruction proxies; the imported GLB is presentation geometry only

The raw OBJ, FBX, and Blender files remain source intake. Runtime code must load only the files under `runtime/`.
