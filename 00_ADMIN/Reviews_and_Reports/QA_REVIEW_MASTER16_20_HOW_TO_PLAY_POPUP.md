# QA Review: Master 16.20 How to Play Popup Hotfix

Date: 2026-05-21

## Scope

- Changed the How to Play menu control from a `_blank` link to a button wired through `window.open`.
- Preserved `how-to-play.html` as a separate modular page in the source, release package, and upload package.
- Refreshed source, release package, and GoDaddy upload convenience folder to `Master 16.20`.

## Static Checks

- Passed: `Get-Content -Raw "10_SOURCE/Masters/Master 16/js/main.js" | node --check --input-type=module`.
- Passed: source, release package, and GoDaddy upload convenience folder hashes match for `index.html`, `how-to-play.html`, and `js/main.js`.
- Passed: team-sync source-of-truth check reports `Master 16.20` with matching source/release/upload integrity for the modular package.
- Passed: source and release package both contain `<button id="how-to-play-btn" type="button">How to Play</button>`.
- Passed: source and release package both bind the control to `window.open('how-to-play.html', 'holesyHowToPlayWindow', 'width=920,height=760')`.

## Runtime Check

- Passed: `http://127.0.0.1:8788/index.html?v=16.20-popup` returns HTTP 200.
- Passed: `http://127.0.0.1:8788/how-to-play.html?v=16.20` returns HTTP 200.
- Passed: browser DOM check confirmed the menu control is a `BUTTON`, not a `_blank` link, on `Master 16.20`.
- Note: the in-app browser automation could not directly inspect the popup event, so final visual confirmation should be performed in Chrome by clicking `How to Play` and confirming it opens as the named popup-style window rather than a new tab.
