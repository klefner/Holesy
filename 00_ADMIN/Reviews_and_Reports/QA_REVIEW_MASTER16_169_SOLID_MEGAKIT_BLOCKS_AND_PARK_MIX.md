# QA Review — Master 16.169 Solid MegaKit Blocks and Park Mix

Date: 2026-07-13

## Scope

- Repair imported MegaKit debris that visually read as hollow facade shells.
- Preserve the untouched authored model before breach.
- Preserve appropriate exterior facade/roof material on closed destructible blocks and use interior material on newly exposed faces.
- Route imported debris through box-overlap contact handling.
- Reduce park allocation to one or two parcels per generated town/wave.
- Support full-parcel, mixed compact, and spacious single-attraction park layouts.

## Implementation evidence

- The offline converter emits immutable `v2.4.0` assets for the small, medium, and large imported buildings.
- Each converted building contains 96 named blocks, each with six closed faces and a visible core within four percent of its declared collider dimensions.
- Runtime uses the untouched authored glTF as the intact shell and switches to the closed converted blocks only when the stack is breached.
- Imported pieces carry `usesBoxStackContacts` and use the existing voxel box-contact solver without changing whole-stack activation.
- Park generation chooses exactly one or two parcels, then selects a full-parcel attraction, four-attraction compact mix, or occasional spacious compact showcase.
- New park rotation includes volleyball, miniature golf, seesaws, four-hole golf with a golf cart, and an amusement park.

## Automated checks

- `node --check scripts/convert-megakit-building-small-1.mjs`: pass.
- `node --check 10_SOURCE/Masters/Master 16/js/main.js`: pass.
- Converter validation: 96 blocks per imported building; six closed faces per block; boundary-only facade; top-only roof; interior cut faces; visible/collider mismatch no greater than four percent.
- Source/release package parity: required before publication.

## Player validation requested

- Confirm each intact imported building still matches its authored model.
- Confirm broken pieces read as solid blocks from top, bottom, side, and three-quarter angles.
- Confirm exterior blocks retain recognizable facade/roof treatment while newly exposed faces read as structural interior.
- Confirm imported debris collides, settles, and sleeps like the successful medium-building blocks without persistent ground spinning.
- Confirm each wave contains only one or two park parcels and that mixed parks remain readable rather than cluttered.
