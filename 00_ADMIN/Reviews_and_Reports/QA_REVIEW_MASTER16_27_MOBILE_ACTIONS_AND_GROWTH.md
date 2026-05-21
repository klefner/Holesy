# QA Review - Master 16.27 Mobile Actions And Growth

Date: 2026-05-21

## Scope

Defect repair for the `Master 16` modular package after `Master 16.26` PC validation:

- mobile Stats and How to Play menu controls did not visibly respond
- How to Play needed to use the same button format as the neighboring menu controls
- taking soldier damage appeared to disable normal object-based growth until the next wave

## Files Reviewed

- `10_SOURCE/Masters/Master 16/index.html`
- `10_SOURCE/Masters/Master 16/css/styles.css`
- `10_SOURCE/Masters/Master 16/js/main.js`
- `10_SOURCE/Masters/Master 16/how-to-play.html`
- `40_RELEASE/Website_Publish_Package/holesy/`

## Control Results

- Source and release package remain modular and hash-aligned for changed game files.
- `Master 16.27` is reflected in the source entry point, pause menu, field manual, and runtime build label.
- Menu action controls now share the explicit `menu-action-btn` class and the same CSS path.
- Stats, Archive, How to Play, and Load Endless now use one touch-safe activation helper with duplicate-event guarding.
- Touch devices open popup windows without desktop feature strings, reducing mobile popup failure risk.
- Soldier damage no longer stores ordinary damage as long-lived negative `bonusRadius`; it lowers the growth score baseline instead, so future object score can visibly grow the hole.

## Validation Performed

- `git diff --check`
- module syntax check by copying `js/main.js` to `.mjs` and running `node --check`
- source/release hash comparison for `index.html`, `how-to-play.html`, `css/styles.css`, `js/main.js`, and the existing How to Play image
- browser smoke at `http://127.0.0.1:8792/index.html?v=16.27-mobile-growth-fix`
- DOM/style check confirmed all four menu action controls are real buttons using `menu-action-btn`, `appearance: none`, matching background, and matching border radius

## Remaining Manual Validation

- real mobile tap confirmation for Stats and How to Play on the user's device
- real gameplay confirmation that normal object devours visibly grow the player after soldier damage in the same wave
