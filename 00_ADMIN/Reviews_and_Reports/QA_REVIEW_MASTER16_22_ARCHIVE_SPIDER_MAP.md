# QA Review: Master 16.22 Archive Spider Map

Date: 2026-05-21

## Scope

- Added a How to Play `Archive Map` topic.
- Added a modular spider diagram asset at `assets/images/archive-achievement-spider.svg`.
- Mapped current archive groups to recovered documents and achievement/buff hints:
  - Witnesses: Pedestrian Pull / Crowd Magnet, The Forum User, Tree Hugger
  - The Pattern: Linden Street / Building Chain, The Quiet Block / Block Sweep, Bellmar
  - Origins: First Bite, The Quiet / Quiet Bite
- Refreshed source, release package, and GoDaddy upload convenience folder to `Master 16.22`.

## Static Checks

- Passed: `Get-Content -Raw "10_SOURCE/Masters/Master 16/js/main.js" | node --check --input-type=module`.
- Passed: source, release package, and GoDaddy upload convenience folder hashes match for `index.html`, `how-to-play.html`, `js/main.js`, `assets/images/how-to-play-game-summary.svg`, and `assets/images/archive-achievement-spider.svg`.

## Runtime Checks

- Passed: `http://127.0.0.1:8788/index.html?v=16.22-archive-map` returns HTTP 200.
- Passed: `http://127.0.0.1:8788/how-to-play.html?v=16.22` returns HTTP 200.
- Passed: `http://127.0.0.1:8788/assets/images/archive-achievement-spider.svg?v=16.22` returns HTTP 200.

## Residual Risk

- Final visual review should confirm the spider diagram is readable inside the How to Play popup at the target popup size and on mobile-width browsers.
