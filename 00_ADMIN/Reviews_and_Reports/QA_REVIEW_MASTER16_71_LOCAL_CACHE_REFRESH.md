# QA Review - Master 16.71 Local Cache Refresh

Date: 2026-06-11

## Scope

- Source: `10_SOURCE/Masters/Master 16/`
- Release package: `40_RELEASE/Website_Publish_Package/holesy/`
- Build label: `Master 16.71`

## Change Reviewed

- Bumped the local test build label and asset query strings after Chrome continued running an already-loaded `Master 16.68` wave-time module.
- No gameplay rule changed from `Master 16.70`.

## Verification

- Restarted the local server on `http://127.0.0.1:4173/`.
- HTTP check confirmed `index.html?fresh=16.71` contains `Master 16.71` and `v=16.71`.
- HTTP check confirmed served `js/main.js?v=16.71` contains the manual Time cycle handler and does not contain the old `WAVE ${waveNum} TIME` banner branch.
- `node --check` passed for source and release `js/main.js` and `js/build-info.js`.
- SHA256 parity passed for source/release `index.html`, `js/main.js`, and `js/build-info.js`.
- User validation on 2026-06-11 confirmed the Time button advances to the next time-of-day look.
- User validation on 2026-06-11 confirmed the broader `Master 16.71` regression pass, government-building debris feel, and medium-office voxel behavior passed.
