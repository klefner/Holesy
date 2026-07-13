# Live Event Asset Rotation

## Purpose

Holesy should continuously gain new consumable objects, sounds, behaviors, environments, and short-lived event content. Variety and surprise are retention features, not decoration.

## Event Package Contract

Each event package should declare:

- stable event ID and content version;
- start and end timestamps;
- eligible towns and parcel archetypes;
- object, animation, sound, boss, cosmetic, and archive-drop catalogues;
- deterministic asset-conversion recipe and source hashes;
- download size and performance tier;
- post-event retention rule for earned cosmetics and archive entries;
- fallback behavior when the device is offline.

## Three-Day Event Behavior

- Event-only world assets spawn only during the configured window.
- Earned cosmetics and archive discoveries remain permanently available unless clearly identified otherwise before play.
- The base game never requires an expired package to boot.
- Previously downloaded event packages may remain cached but become spawn-ineligible after expiration.
- A signed manifest, not client clock alone, determines event eligibility when online.
- Offline play uses the most recently validated event window with a bounded grace period.

## Asset Memory

Every converted asset is reproduced from its committed source hash, pipeline version, deterministic recipe, fixed random seed, output manifest, and validation record. Conversational memory is never the source of truth.
