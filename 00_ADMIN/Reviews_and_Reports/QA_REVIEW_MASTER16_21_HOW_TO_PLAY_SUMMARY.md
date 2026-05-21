# QA Review: Master 16.21 How to Play Game Summary

Date: 2026-05-21

## Scope

- Added a top `Game Summary` link to the How to Play popup.
- Added a modular image asset at `assets/images/how-to-play-game-summary.svg`.
- Updated the How to Play content frame so the default first section displays the summary image.
- Refreshed source, release package, and GoDaddy upload convenience folder to `Master 16.21`.

## Static Checks

- Passed: `Get-Content -Raw "10_SOURCE/Masters/Master 16/js/main.js" | node --check --input-type=module`.
- Passed: source, release package, and GoDaddy upload convenience folder hashes match for `index.html`, `how-to-play.html`, `js/main.js`, and `assets/images/how-to-play-game-summary.svg`.
- Passed: source and release package both contain the top `Game Summary` index link and the matching `#summary` content section.
- Passed: source and release package both reference `assets/images/how-to-play-game-summary.svg` from the How to Play content frame.

## Runtime Check

- Passed: `http://127.0.0.1:8788/index.html?v=16.21-summary` returns HTTP 200.
- Passed: `http://127.0.0.1:8788/how-to-play.html?v=16.21` returns HTTP 200.
- Passed: `http://127.0.0.1:8788/assets/images/how-to-play-game-summary.svg?v=16.21` returns HTTP 200.

## Residual Risk

- Final visual confirmation should be performed in Chrome by opening How to Play and confirming the popup defaults to `Game Summary` with the summary graphic visible in the right content frame.
