# QA Review — Master 16.210 Uniform Harvest Cubes

Date: 2026-08-07

## Scope

- Restore the previously governed imported-building fragment standard after the Master 16.209 compact 8–20-piece experiment produced oversized collapse slabs.
- Apply the same small closed-cube rule to dedicated Harvest structures and reused frontier buildings.
- Preserve the user-approved progressive water-tower and windmill topple behavior and the authored non-cubic tank/rotor exceptions.

## Implementation Evidence

- Ordinary Harvest structures use 4 columns × 4 rows × 6 floors: 96 closed pieces.
- Silo and water-tower support structures use 4 columns × 4 rows × 8 floors: 128 closed pieces.
- Harvest frontier definitions no longer specify a reduced `breakupCount`; every converted 96-piece structure is used.
- Converted pieces remain detached/dormant before breach, and total authored food/score value remains conserved.
- Water-tower tank and windmill rotor remain coherent authored landmark meshes rather than cubes.

## Verification

- `node --check` passed for source and release `js/main.js` and `js/build-info.js`.
- `harvest-collapse-contract-static.test.mjs` passed with exact 96/128-piece expectations and full frontier-cube enforcement.
- `city-build-isolation-static.test.mjs` passed, preserving all five guarded async city-build yields.
- All six regenerated dedicated Harvest glTF files passed `validate-destructible-gltf.mjs`; every fragment has a closed core and no geometry exceeds its collision cell.
- Browser startup smoke reached the playable mode-selection screen without a startup failure; rendered collapse appearance remains a required player-facing validation because deterministic collapse spawning is not exposed in the production browser build.

## Performance Boundary

This patch increases dormant converted-piece registrations but does not reintroduce intact-model draw calls for those pieces: they remain detached until breach. Active collapse cost is bounded by the existing spatial contacts, local support-column activation, sleeping, and landmark assembly behavior. Late-wave player testing remains necessary before claiming performance parity under several simultaneous collapses.
