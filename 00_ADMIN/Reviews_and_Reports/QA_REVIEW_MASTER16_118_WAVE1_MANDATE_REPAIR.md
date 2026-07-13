# QA Review - Master 16.118 Wave 1 Mandate Repair

Date: 2026-07-10

## Defect

Ultra difficulty could enable soldiers during Wave 1, allowing the randomized Mandate system to offer a military objective during the intentionally soldier-free opening level.

## Repair

- Set Ultra `waveOneSoldiers` to `false`, matching every other difficulty and the Endless Wave 1 invariant.
- Preserved the existing military Mandate guards: `minWave: 2` and a positive estimated military supply.
- Mirrored the repair across governed source and release packages.

## Verification

- JavaScript syntax checks pass for source and release modules.
- All difficulty profiles now declare `waveOneSoldiers: false`.
- Wave 1 configuration and military Mandate selection gates contain no available path to a Soldier or Military Unit target.
- Source/release runtime hashes match.

## Result

PASS for static and configuration verification. Real-play Wave 1 randomized-Mandate validation remains recommended on desktop/mobile.
