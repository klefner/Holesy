# QA Review - Master 16.147 Population and Cosmetics

## Population

- Residential parcels receive up to six additional trees around their house clusters.
- MegaKit building selection cycles through small, medium, and large source models by wave/district generation.
- Reserved MegaKit test parcels use one selected archetype and clear conflicting building pieces first.

## Permanent Cosmetic Registry

- `Tin-Foil Halo` unlocks from `The Forum User`.
- `Bellmar Seal` unlocks from `Bellmar`.
- `Condemned Chic` unlocks from `Linden Street`.
- `Prism Orbit` remains tied to completing every Run Goal and the Mandate in one wave.
- Unlocks and equipped choice persist in `holesy.cosmetics.v1` on the current browser/device.
- Existing qualifying achievement saves are reconciled into cosmetic unlocks.

## Lifetime Architecture

- Initial batch uses `permanent` lifetime and is selectable between runs.
- Registry entries carry lifetime metadata so later rewards can be wave-long, run-long, town-gated, or meta-achievement gated.

## Visual Treatment

- Tin-Foil Halo: animated silver rim shimmer.
- Bellmar Seal: restrained archive-blue/silver pulse.
- Condemned Chic: alternating caution-yellow and black rim.

## Validation

- Verify locked cosmetics do not appear in the selector.
- Verify existing qualifying achievements grant their mapped cosmetic.
- Verify equipped selection survives reload.
- Verify Prism Orbit appears only while selected.
- Verify residential parcels read greener than office/tower parcels.
- Verify the three MegaKit source models appear across successive district generations.
