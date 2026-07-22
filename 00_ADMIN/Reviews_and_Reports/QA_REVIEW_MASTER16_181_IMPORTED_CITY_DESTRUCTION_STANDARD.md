# QA Review - Master 16.181 Imported City Destruction Standard

Date: 2026-07-22

## Scope

Replace the Medieval Village proxy-block destruction prototype with the already-proven MegaKit authored-surface and closed-core approach, and establish that approach as the routine for future cities that use externally authored models.

## Implementation evidence

- Added `IMPORTED_CITY_MODEL_DESTRUCTION_STANDARD.md` as the governed cross-city contract.
- Added a manifest-driven normalization and conversion pipeline under `scripts/`.
- Converted all ten Quaternius Medieval buildings into immutable `v1.0.0` destruction assets.
- Runtime now presents the untouched authored shell until a valid breach, then activates dormant authored fragments using shared imported-building collapse, box contact, fit, jam, settle, lifecycle, and five-voice audio behavior.
- The Mill manifest explicitly includes its blade mesh in owning fragments; this exception applies only to that model.
- Normal Medieval parcels now receive fourteen loose edibles instead of six.

## Automated conversion results

All ten release-package models passed with zero collider overhangs and zero missing structural cores. Fragment counts are 96 per model except Bell Tower at 128. Together they retain 2,983 authentic primitives and 135,212 clipped authored triangles.

## Static verification

- `node --check` passes for `main.js`, the converter, pack preparer, and validator.
- Runtime references no removed procedural Medieval fields (`floors`, `renderedHeight`, or `color`).
- Player-facing classification uses building or tower labels; Medieval models are not marked as skyscraper chunks.
- Local browser QA loaded `Version 16.181`, all 32 pack-building sites, 448 normal-parcel edibles, and 24 ambient actors with no console warnings or errors.

## Remaining directed test

Player-directed testing must still judge facade continuity during first breach and settled debris, progression feel from starter objects through each class, and stable frame/audio behavior with multiple simultaneous collapses.

## Publication evidence

- GitHub Pages branch commit: `33dd0a3`
- Public page reports `Version 16.181` and `Master 16.181`.
- A fresh public Medieval run loaded all 32 converted-building sites, 448 normal-parcel edibles, and 23 ambient actors with no browser warnings or errors.

Status: source/release implementation and Pages smoke test passed; pending player-directed destruction-feel review.
