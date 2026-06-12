# QA Review - Master 16.73 Building Window Short Flicker

Date: 2026-06-11

## Scope

- Source: `10_SOURCE/Masters/Master 16/`
- Release package: `40_RELEASE/Website_Publish_Package/holesy/`
- Build label: `Master 16.73`

## Change Reviewed

- Building-window power loss now uses a counted random burst instead of a longer time-window flicker.
- Each lit pane gets one to three short flickers after its building loses power, then stays dark.
- Time-of-day cycling still cannot relight windows after their building has lost power.

## Verification

- `node --check` passed for source and release `js/main.js` and `js/build-info.js`.
- SHA256 parity passed for source/release `index.html`, `js/main.js`, and `js/build-info.js`.
- Static grep confirmed source and release contain the counted `flickerFlashesRemaining`, `nextFlickerAt`, and `flickerOn` power-cut path.
- Local served-package check at `http://127.0.0.1:4173/index.html?fresh=16.73` confirmed `Master 16.73`, `v=16.73`, the counted one-to-three window flicker, and no stale pane-duration flicker path.
- User live-play visual confirmation passed on 2026-06-11: the shorter one-to-three-flicker building-window power cutoff looks good.
