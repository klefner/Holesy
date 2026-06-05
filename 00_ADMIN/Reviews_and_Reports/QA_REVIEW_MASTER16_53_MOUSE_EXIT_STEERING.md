## QA Review - Master 16.53 Mouse Exit Steering

Date: 2026-06-05

Scope:

- Preserve desktop mouse steering intent when the cursor briefly leaves the browser canvas.
- Keep keyboard, touch, focus-loss, and reset behavior predictable.
- Promote the modular Master 16 package to `Master 16.53`.

Change Summary:

- Added a mouse-carry steering state that stores the last normalized mouse steering direction when the mouse leaves the canvas.
- While mouse carry is active, the player target is projected ahead of the current player position so the hole keeps moving instead of stopping at the canvas edge.
- Mouse carry clears when the mouse re-enters, keyboard takes over, touch input is used, focus/visibility resets input, or the round resets.

Validation:

- `node --check 10_SOURCE/Masters/Master 16/js/main.js` passed.
- `node --check 10_SOURCE/Masters/Master 16/js/build-info.js` passed.
- Source and release package were refreshed to `Master 16.53`.

Local Test URL:

- `http://127.0.0.1:8798/index.html?v=16.53-mouse-exit-carry`

Manual Test Focus:

- On PC, start a game with mouse steering.
- Move the mouse out of the game browser viewport while the hole is moving.
- Confirm the hole continues in the last intended direction instead of stopping immediately.
- Move the mouse back into the game viewport and confirm normal mouse steering resumes.
- Press WASD/arrow keys after mouse exit and confirm keyboard control takes over cleanly.
- Alt-tab or blur the window and confirm transient input clears safely.

Residual Risk:

- Direction carry uses the last valid mouse-to-player vector. If the cursor exits while nearly centered on the player, carry may be minimal, which is preferable to inventing a false direction.

