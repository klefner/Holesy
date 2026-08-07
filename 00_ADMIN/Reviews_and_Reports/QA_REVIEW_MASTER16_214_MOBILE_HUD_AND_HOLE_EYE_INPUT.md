# QA Review — Master 16.214 Mobile HUD and Hole-Eye Input

Date: 2026-08-07

## Screenshot Finding

The supplied portrait-phone screenshot shows expanded state (`HUD-`) occupying both upper corners, a three-line deployment banner overlapping Tier/Timer, two rows of game controls, a full Mandate card, and a large active-effect card clipped by browser chrome. The world is reduced to a narrow central strip.

## Root Causes

- The v1 mobile HUD preference defaulted to expanded when no preference existed.
- The deployment HUD used desktop copy and dimensions on phones.
- active-effect cards used large inline styles at `top:75vh`, bypassing responsive CSS.
- First-person touch converted the complete travel vector into a new camera heading every frame. Small horizontal thumb noise therefore created continuous yaw.
- Touch release stopped movement but retained the pending camera heading and yaw velocity.

## Correction

- Reset the mobile preference with a v2 key and compact default.
- Preserve Tier, Timer, View, Pause, and one current Mandate row; disclose secondary surfaces through `HUD+`.
- Force compact presentation on initial Hole-Eye entry without deleting the player's saved third-person preference.
- Shorten mobile deployment copy and add responsive IDs/classes for deployment and active-effect UI.
- Make vertical touch displacement drive forward/back travel and horizontal displacement drive camera turning.
- Give Comfort turning a 24px horizontal deadzone, nonlinear 1.85 response, and a 16-degree rolling target range.
- Scale mobile first-person travel by deliberate vertical drag magnitude instead of normalizing every drag to full speed.
- Track the initiating touch identifier and stop desired yaw/yaw velocity on release, cancellation, blur, and visibility loss.
- Preserve the established heading when Hole-Eye opens instead of automatically turning toward the arena center; Pause also clears all held touch/look intent before its overlay opens.

## Acceptance Matrix

- 320x568, 390x844, and 430x932 portrait: no overlap among Tier, Timer, deployment chip, View/Pause, and compact Mandate.
- Fresh and prior-v1 sessions begin with `HUD+`; tapping expands and collapses the complete information surfaces.
- Hole-Eye always enters compact; the player can explicitly expand it again.
- One to four active effects remain inside the phone safe area without horizontal overflow.
- Pure vertical Comfort drag produces no yaw; sub-24px horizontal wobble produces no yaw.
- Deliberate horizontal drag turns progressively and full drag retains useful emergency turning.
- Releasing/canceling the active finger immediately stops camera rotation; adding/removing a second finger does not change the active joystick vector.
- Third-person touch and desktop keyboard/mouse behavior remain unchanged.

## Architecture and Performance

This is a DOM/CSS and existing camera-input correction inside the governed modular package. It adds no render-loop allocations, geometry, physics bodies, or new per-frame object scans. The request conflicts with neither product intent nor the modular architecture decision.

## Verification Evidence

- `node --check` passed for `js/main.js` and `js/build-info.js`; `git diff --check` passed.
- Local browser smoke passed at 320x568, 390x844, and 430x932 portrait viewports.
- A fresh 390x844 session opened in compact state with Tier, Timer, View, Pause, one current Mandate, and `HUD+` visible; the expanded drawer remained available on demand.
- Hole-Eye forced the compact layout at each tested size. The 320px top row used shortened deployment copy and showed no Tier/deployment/Timer overlap.
- Hole-Eye yaw telemetry read `0.0` at entry, after 2.5 seconds idle, and after Pause/Resume with no user steering.
- Browser console error count was zero.
- Active-touch identity, deadzone, nonlinear response, analog travel scaling, and release/cancel behavior were inspected directly in the input path. Final thumb-feel remains a real-device acceptance item because the desktop browser harness cannot emit native multi-touch gestures.
