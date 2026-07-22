# Imported City Model Destruction Standard

## Purpose

Every Holesy city that uses externally authored building models must use one shared destruction pipeline. A new city may define explicit exceptions for unusual assets, but it must not replace the shared pipeline with runtime-generated proxy cubes, a one-off collapse system, or a visually unrelated debris substitute.

This standard generalizes the validated MegaKit workflow from `Master 16.155` through `Master 16.170`.

## Player-facing contract

An imported building must remain recognizably the same authored object through all three states:

1. intact
2. breached and collapsing
3. settled debris

Windows, doors, roof treatment, facade materials, trim, structural framing, and other distinctive surfaces must not disappear when destruction begins. Newly exposed faces must read as solid structure rather than hollow visual shells.

## Required offline conversion contract

Each imported building must be processed before runtime:

- Normalize the source to glTF 2.0 with external binary geometry.
- Record the source asset hash and deterministic conversion recipe version.
- Divide the configured destructible body into a bounded grid; the default recipe is 4 columns by 4 rows by 6 floors, or 96 fragments.
- Clip original source triangles to their owning fragment while interpolating original UVs and normals.
- Preserve original source materials on every authentic clipped surface.
- Add a closed six-face structural core to every fragment.
- Inset the generic structural core enough that it cannot win the depth test over authentic windows, doors, trim, roof, or recessed facade surfaces.
- Store box-collision dimensions and floor/row/column metadata on every fragment node.
- Validate that no fragment geometry overhangs its declared collider beyond the governed tolerance.
- Publish converted assets under an immutable recipe-version directory.

Runtime slicing or replacement with flat-color proxy blocks is prohibited for production imported cities.

## Required runtime contract

All imported buildings use the same runtime adapter:

- Render the untouched authored model while intact.
- Load converted destruction fragments as dormant templates.
- Keep dormant fragments detached from the scene graph until the first valid breach.
- Gate the first breach by the building's governed size class.
- On breach, remove the intact shell and attach the authentic closed fragments.
- Apply the shared topple, pancake, split, or twist collapse plan.
- Use box-overlap contacts, grounded angular damping, bounded collision work, and forced sleep for settled piles.
- Require each exposed fragment to pass the existing physical width/depth/height fit check before consumption.
- Preserve jam/eject behavior for fragments that do not fit.
- Use the shared global building-audio voice ceiling.
- Remove every runtime object, dormant template, actor reference, and light reference cleanly on city rebuild.

## Building progression classes

Pack manifests must assign one of these semantic classes. Thresholds are tuned in world units by validation rather than inferred from filenames.

| Class | Intended progression | Initial reference threshold |
| --- | --- | ---: |
| `small_structure` | house, cottage, small shop | 4.25 |
| `medium_structure` | inn, stable, blacksmith, sawmill | 5.25 |
| `large_structure` | mill, hall, large workshop | 6.00 |
| `tower_structure` | bell tower, keep, true vertical landmark | explicit override, at least 6.00 |

These are first-breach thresholds. Individual fragments still use the shared physical-fit rule. A zero whole-building threshold is not allowed for authored imported shells unless an explicit QA-approved exception proves that the intact object is already visibly piecewise.

## Pack manifest contract

Each imported-city pack must declare:

- pack id and license/source provenance
- normalized source and immutable converted output locations
- recipe version and default grid
- core inset
- material-selection rules for facade, roof, and exposed interior cores
- model footprint and semantic progression class
- authored front/orientation rule
- source mesh include/exclude rules
- explicit unusual-element overrides

The runtime consumes this manifest-derived metadata; it must not identify behavior by source filename alone.

## Explicit unusual-element overrides

Unusual elements remain city- or asset-specific and must be named in the pack manifest. Examples include:

- windmill blades
- drawbridges
- towers or spires
- cranes
- signs or awnings
- animated doors
- bridges spanning non-destructible space

An override must state whether the element is:

- included in the structural fragment grid
- converted as its own detachable prop
- retained as a cosmetic child until its parent fragment fails
- non-destructible for an explicit gameplay reason

Overrides may customize element handling, but not the shared fragment collision, fit, settle, audio, lifecycle, or validation contracts.

## Spatial-population contract

Building conversion and city population are separate systems. Imported cities must also supply a progression ladder and avoid broad dead space:

- starter objects visible and edible at the opening radius
- intermediate objects that bridge the player into small-building range
- large props and animals that bridge small to medium structures
- irregular clusters across parcel interiors, edges, and streets rather than repeated perimeter points
- city-specific economic and social scenes that explain why objects are present

Raw object count is not sufficient evidence. QA must judge visible density and progression from the gameplay camera.

## Required automated validation

For every converted model:

- deterministic source hash and recipe metadata present
- expected fragment count present
- six closed structural faces per fragment
- authentic surface primitives preserved
- collider dimensions present
- zero overhanging primitives and vertices at governed tolerance
- source/release hashes match
- no asset 404/503 aborts the remainder of the city population

## Required player validation

- intact building matches its authored source
- first breach retains recognizable authored facade detail
- settled debris still contains recognizable roof, wall, framing, window, and foundation treatment
- building cannot collapse before its intended progression class
- fragments that fit can be consumed; fragments that do not fit jam/eject
- collapse label names the correct structure class
- collapse and settled piles do not create sustained frame or audio spikes
- city contains a continuous visible growth ladder into each building class

## Rollout rule

No future imported city may ship from an intact-model-plus-proxy-block prototype. Prototype cities remain review-only until their entire required building roster passes this standard. City-specific exceptions must be explicit, documented, and tested independently.
