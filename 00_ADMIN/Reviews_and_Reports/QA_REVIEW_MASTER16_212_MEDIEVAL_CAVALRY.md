# QA Review — Master 16.212 Medieval Cavalry

Date: 2026-08-07

## Root Cause

The mounted-combatant body was 1.75 units long on X even though the unit movement vector, rider, and lance all face +Z. The rectangular proxy therefore travelled broadside. Four fixed legs, a single box head, no tail, and a single rider-leg block reinforced the appearance of a sideways flying desk.

## Acceptance Checks

- Horse torso length, head, muzzle, rider, and weapon all align to +Z forward.
- The silhouette includes a chest, neck, head, muzzle, ears, mane, tail, saddle, four legs, and hooves.
- Rider legs visibly straddle the saddle rather than forming one sideways block.
- Diagonal leg pairs swing while the unit moves and settle to a restrained idle motion when stopped.
- Tail motion follows the gait without allocating new objects per frame.
- Ordinary mounted lancers and the mounted Medieval warlord share the repaired model.

## Performance Boundary

The mount uses simple shared box materials and low-segment primitive geometry. Animation updates only pre-recorded leg pivots and one tail transform on live mounted combatants; it creates no per-frame geometry, materials, arrays, or physics bodies.

## Verification

- `node --check` passed for `js/main.js` and `js/build-info.js`.
- Local browser smoke loaded Master 16.212, selected Medieval Village, began an Endless Waves run, rendered gameplay, and advanced the Medieval deployment schedule without a startup failure.
- Static model review confirms the horse's longitudinal axis, head, rider, and weapon now share the same +Z travel axis; gait state is attached only to mounted units.
