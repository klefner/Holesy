# QA Review - Master 16.86 Startup And Frame Pacing

Date: 2026-06-18

## Scope

`Master 16.86` repairs the reported 10-second unresponsive mode-selection window, 14-second Begin delay, and early-wave stutter.

Changed behavior:

- The city is no longer populated synchronously before menu listeners are attached.
- City population yields every two blocks, eight cars, and ten park assets so the browser can render and process input between batches.
- Begin displays a disabled `Building City...` state before world construction starts.
- HUD and live-score DOM updates run at 10 Hz instead of every rendered frame.
- Simulation, physics, camera movement, and WebGL rendering remain uncapped.

## Implementation Validation

- Team Sync completed and the Product Intent Gate allowed the performance repair.
- `node --check` passed for source `js/main.js` and `js/build-info.js`.
- Fresh in-app browser load reached `menu-listeners-ready` with Begin enabled and produced no new console warnings or errors.
- Waves mode selection responded before city generation.
- Waves Begin completed through the chunked city build and rendered Wave 1, Run Goals, and live scores with no new console warnings or errors.
- Source and release hashes match for `index.html`, `PACKAGE_MANIFEST.md`, `js/main.js`, and `js/build-info.js`.

## Required Live-Play Validation

- Open `http://127.0.0.1:4173/index.html?fresh=16.86-startup-performance`.
- Confirm the mode-selection controls respond immediately after the screen appears.
- Confirm Begin shows immediate feedback and no long frozen interval.
- Confirm movement remains smooth at the start of Wave 1 and while eating dense blocks.
- Confirm goals, live scores, wave timer, building breakup, and government physics still behave normally.

## Result

Implementation smoke validation passed. Final device-specific smoothness remains a live-play validation item because the reported delays are hardware-dependent.
