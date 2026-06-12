# QA Review - Master 16.68 Wave Time Sequence

Date: 2026-06-09

## Scope

- Source: `10_SOURCE/Masters/Master 16/`
- Release package: `40_RELEASE/Website_Publish_Package/holesy/`
- Build label: `Master 16.68`

## Change Reviewed

- Wave-based modes now use an explicit time-of-day sequence: Morning, Mid Day, Evening, Night.
- The sequence repeats after Night, so wave 5 returns to Morning in Endless Waves.
- During active wave-based play, the Time button snaps back to the current wave's assigned time instead of overriding the wave look.

## Verification

- `node --check` passed for source and release `js/main.js` and `js/build-info.js`.
- SHA256 parity passed for source/release `index.html`, `js/main.js`, and `js/build-info.js`.
- Local HTTP smoke at `http://127.0.0.1:4173/index.html` returned 200, served `Master 16.68`, and used `js/main.js?v=16.68`.
- Local HTTP checks confirmed the Time button is present and the removed Weather button remains absent.
- Served `js/main.js` contains the explicit `[0, 1, 2, 3]` wave time-of-day sequence and active wave-time lock.
- Served `js/build-info.js` contains `BUILD_SUB = 68`.
- User validation on 2026-06-11 confirmed the day/time-of-day behavior works and that the added lighting effects are approved.
