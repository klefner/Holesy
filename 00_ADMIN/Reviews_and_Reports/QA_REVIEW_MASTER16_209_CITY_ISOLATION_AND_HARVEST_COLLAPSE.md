# QA Review - Master 16.209 City Isolation And Harvest Collapse

Date: 2026-08-07

## Scope

Prevent cancelled city-generation work and district-less Endless saves from mixing town recipes. Replace Harvest County's sparse sampled shell fragments and instantaneous radial building collapse with compact closed-volume structures, local support failure, progressive toppling, and coherent landmark parts.

## Acceptance Checks

- Every yielding core-city population path aborts when its immutable city-build token, round, or environment is stale.
- Mandates and wave scheduling begin only after the current city build completes.
- Endless saves persist and validate their environment and pack recipe; unidentified legacy records fail safely.
- A completed Harvest build reports no incompatible downtown, government, voxel, or skyscraper building pieces.
- Dedicated Harvest buildings use exactly 8–20 closed-volume breakup pieces without reducing their authored food or scoring value.
- A support strike activates the local structural column instead of applying a whole-building radial explosion.
- Windmill and water-tower landmarks retain their intact authored shell during the progressive topple.
- The windmill rotor and water-tower tank remain coherent authored landmark pieces when released at impact.
- Source and release modular packages remain byte-identical.

## Static And Asset Results

- `node --check` passed for `js/main.js`, `js/build-info.js`, and `scripts/convert-imported-building.mjs`.
- `city-build-isolation-static.test.mjs` passed all five guarded core-yield assertions.
- All six dedicated Harvest destructible glTF files passed the repository validator with exact counts: 8, 12, 18, 16, 16, and 20.
- Every generated structural piece has a closed core at `0.96` scale; no converted primitive exceeds its assigned partition.
- The water-tower tank and windmill rotor are excluded from structural partitioning and restored through authored landmark meshes.

## Browser And Package Results

- Local browser smoke loaded `Version 16.209` directly into Harvest County with no console errors.
- The completed world reported `harvestCounty`, city-build state `ready`, integrity `pass`, and `0` incompatible building pieces.
- Harvest preload completed and the world reported 17 buildings with 228 compact structural pieces, preserving the populated county rather than removing content.
- Saving and reloading Endless restored `harvestCounty` as both the active environment and override and reported `restored:harvestCounty`.
- A screenshot review confirmed the playable Harvest scene and unobstructed HUD at the representative Wave 1 baseline.
- SHA-256 parity passed for 29 changed browser-package files across source and release, including all converted Harvest structure assets.

## Publication Verification

- GitHub Pages commit `608eac3` completed successfully in workflow `31206461856`.
- Live browser verification confirmed `Master 16.209` / `Version 16.209` with no console errors.
- Public test URL: `https://klefner.github.io/Holesy/?v=16.209-608eac3`
