# QA Review - Master 16.95 Ten Bosses Reset Cadence

Date: 2026-06-24

Change:

- Expanded the offensive boss roster to ten archetypes:
  - Siege Tank, Twin-Gun Mech, Shield Commander, Mortar Carrier, Rail Sniper, Drone Marshal, Grenade Captain, Flame Rig, Railgun Tripod, and Shock Bruiser.
- Added distinct visual variants and combat tuning for the seven new boss archetypes.
- Added attack distinctions for splash rounds, long-range heavy shots, fast lasers, close-range flame pressure, charged rail shots, and shock pulses.
- Corrected Endless world-shift resizing so each block has five waves of growth, the boss appears on the fifth wave, and the hole reset happens on the sixth wave.

Implementation Note:

- Boss waves still use the fifth-wave cadence from `armyBossWaveInterval`.
- World-shift resizing now keys off `(waveNum - 1) % interval === 0`, so wave 5 is the boss wave and wave 6 is the reset/world-shift wave.
- The boss unlock system still favors unrevealed boss archetypes first before repeats.

Verification:

- Passed: JavaScript syntax check for source and release `js/main.js` and `js/build-info.js`.
- Passed: source/release parity check for `index.html`, `how-to-play.html`, `js/main.js`, `js/build-info.js`, and `PACKAGE_MANIFEST.md`.
- Passed: static check confirmed `BOSS_UNIT_TYPES` contains ten archetypes and `isEndlessWorldShiftWave` uses the sixth-wave reset expression.
- Passed: local test URL `http://127.0.0.1:4174/` served `Master 16.95`.
- Passed: browser smoke test loaded `Master 16.95`, clicked Begin, reached gameplay HUD, and reported no console warnings/errors.

Defects Detected:

- None from mechanical and smoke verification. Player QA should validate the full wave cadence in Endless: growth through waves 1-5, boss on 5, reset on 6, then repeat at 10/11 and 15/16.
