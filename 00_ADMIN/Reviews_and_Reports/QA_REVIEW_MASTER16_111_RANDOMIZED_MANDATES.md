# QA Review - Master 16.111 Randomized Mandates

Date: 2026-07-09

## Scope

User playtest feedback reported that Mandates were always the same and became boring.

## Changes Reviewed

- `10_SOURCE/Masters/Master 16/js/main.js`
  - Replaces the fixed five-row Mandate contract with a randomized target pool.
  - Adds mandate categories for people variants, street props, trees, cars, buildings, rival holes, soldiers, military units, and boss units.
  - Ramps active Mandate row count by wave tier: waves 1-5 select one target type, later tiers randomly expand toward the five-row maximum.
  - Randomizes required counts within each category while preserving actual-supply caps from the fairness repair.
  - Wires rival-hole and soldier/military/boss consumption into Mandate progress.
- `10_SOURCE/Masters/Master 16/js/build-info.js`
  - Updates the governed player-facing build label and changelog to `Master 16.111`.

## Verification

- `node --check 10_SOURCE/Masters/Master 16/js/main.js` passed.
- `node --check 10_SOURCE/Masters/Master 16/js/build-info.js` passed.
- Source files were mirrored to `40_RELEASE/Website_Publish_Package/holesy/`.
- Fresh full upload copy created at `C:\Users\KentLefner\Downloads\holesy-godaddy-upload-master-16.111\holesy\`.

## Notes

- This pass expands Mandate gameplay variety using existing object families, positions, movement states, rivals, and military units.
- It does not add 100 new art/model object types. The larger visual object pool remains a future content-production task.
- Scarce categories such as moving cars, soldiers, and bosses use availability and supply caps so they should not be selected when impossible.
