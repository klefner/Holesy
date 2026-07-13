# Quality Inspection Report: Master 16.104 Mobile Mode Buttons

## Scope

- user-reported defect: on mobile, some game-mode selection screen controls looked like buttons but were not real buttons
- source package: `10_SOURCE/Masters/Master 16/`
- release mirror: `40_RELEASE/Website_Publish_Package/holesy/`

## Evidence Reviewed

- mobile viewport browser check at `http://127.0.0.1:4175/index.html?v=mobile-before`
- mobile viewport browser check after the fix at `http://127.0.0.1:4175/index.html?v=mobile-after`
- DOM inspection of `#mode-picker .mode-option`
- console warning/error logs during the mode-selection flow

## Findings

- The four visible game-mode choices were styled as buttons but implemented as plain `div.mode-option` elements.
- The container click handler still allowed desktop clicks, but the controls were not native buttons and did not expose pressed state.
- This matched the user report that some mobile mode-selection "buttons" were not buttons.

## Fix

- Converted each game-mode choice to `button type="button"` while preserving the existing `.mode-option` styling.
- Added `aria-pressed` updates when the selected mode changes.
- Added button reset styling, focus-visible styling, disabled-state coverage, and `touch-action: manipulation`.
- Cache-busted the `build-info.js` module import so the in-page build badge cannot be rewritten from a stale cached metadata module.

## Validation

- Confirmed the mode picker renders at mobile width.
- Confirmed all four mode choices are native `BUTTON` elements after the fix.
- Confirmed tapping `Waves` updates the selected button and `aria-pressed` state.
- Confirmed tapping `Begin` after selecting `Waves` starts the run and hides the title overlay.
- Confirmed no console errors or warnings were reported during the checked mobile interaction.

## Result

- Passed with local browser validation.
- Live site was not checked in this review.
