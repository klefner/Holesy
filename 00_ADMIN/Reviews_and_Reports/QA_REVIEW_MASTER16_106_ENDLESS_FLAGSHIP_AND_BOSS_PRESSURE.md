# QA Review - Master 16.106 Endless Flagship And Boss Pressure

Date: 2026-07-06
Scope: `10_SOURCE/Masters/Master 16/` and mirrored release package.

## Summary

`Master 16.106` makes Endless Waves the default flagship mode and retunes true bosses so they are no longer trivial to defeat by driving under them, especially during speed boost.

## Changes Reviewed

- Endless Waves is the first selected game mode on the title screen.
- Runtime default `selectedMode` is now `endless`.
- Programmatic mode changes sync the visible button state and `aria-pressed` values.
- Mobile menu layout keeps the Music control from colliding with the title and uses refreshed asset cache keys.
- True boss forms use a higher required eat radius than reduced boss-derived drops.
- Speed-boosted holes must satisfy an extra eat-size tax before swallowing a true boss.
- Boss shot damage scales upward when the target hole is close.
- Bosses kite while firing by backing away and strafing inside weapon range.
- Commercial fun release goals were added for Reward Juice and First 60 Seconds passes.

## QA Notes

- Expected gameplay effect: bosses should still be edible when the player has earned enough mass, but skim-under kills should be much less common.
- Expected gameplay effect: closing distance on a firing boss now increases risk instead of only improving swallow odds.
- Expected gameplay effect: Endless is presented as the product's main repeatable mode without removing other modes.

## Verification

- Static source review completed for mode picker, boss damage, boss movement, and boss swallow checks.
- `node --check` passed for source and release `js/main.js` and `js/build-info.js`.
- Source/release SHA-256 parity passed for entry HTML, How to Play, package manifest, stylesheet, runtime modules, difficulty profiles, government physics, and lore data.
- Local release package served successfully at `http://127.0.0.1:4186/index.html?fresh=16.106`.
- Desktop browser smoke confirmed `Master 16.106`, four enabled mode buttons, Endless selected by default, and no console errors/warnings.
- Interaction smoke confirmed clicking Timed updates selection and `aria-pressed` state without console errors/warnings.
- Mobile browser smoke at 390 x 844 confirmed `Master 16.106`, four enabled mode buttons, Endless selected by default, and no Music/title overlap after refreshed asset cache keys.
- Live site was not verified in this review.
