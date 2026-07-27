# QA Review: Master 16.184 Town-Era Response Profiles

Date: 2026-07-27

## Scope

- Remove modern and futuristic combat response from Medieval Village.
- Remove electric streetlights and prevent saved modern objects from returning.
- Increase the agricultural and animal presence of the historical town.
- Make the Medieval boss a meaningful progression gate.
- Establish a scalable town-and-pack composition standard.

## Implementation

- Added positive town theme profiles for Classic City, MegaKit Downtown, Medieval Village, and Harvest County.
- Medieval Village uses ground warbands with swordsmen, longbow archers, mounted lancers, and the mounted Iron Reeve boss.
- Medieval waves do not create response planes or parachutes. The alien aid ship remains the explicit cross-era exception.
- The Iron Reeve requires a hole radius of 6.2 and has stronger close-range damage and charge speed.
- Medieval streetlight positions produce baskets; saved lamps, cars, benches, hydrants, trash, cones, fixtures, aircraft, paratroopers, and incompatible enemies normalize into Medieval equivalents.
- Satellite crop plots and livestock raise Medieval loose themed edibles from 448 to 544.
- Added `TOWN_THEME_AND_ASSET_COMPOSITION_STANDARD.md` with required recipe fields and owned-pack combinations for eight town concepts.

## Verification

- `node --check` passed for source and release `js/main.js` and `js/build-info.js`.
- SHA-256 parity passed for source/release `index.html`, `js/main.js`, and `js/build-info.js`.
- Browser boot confirmed `Version 16.184`.
- Medieval Last Man Standing browser run confirmed:
  - response style `ground_warband`
  - response planes `0`
  - actual spawned roster contained `medieval_archer` and `medieval_cavalry`
  - configured boss roster `medieval_warlord`
  - configured boss eat radius `6.2`
  - live lamps `0`
  - live hydrants `0`
  - cars `0`
  - loose themed edibles `544`
  - all four Medieval commons
- Screenshot review confirmed the populated Medieval playfield and unobstructed HUD rendered successfully.
- Classic City regression boot confirmed `82` hydrants and `96` electric streetlights, so modern-town content was not removed globally.

## Result

Pass for the Medieval positive-profile vertical slice and modern-city regression. Boss feel still requires player validation during an actual fifth-wave encounter.
