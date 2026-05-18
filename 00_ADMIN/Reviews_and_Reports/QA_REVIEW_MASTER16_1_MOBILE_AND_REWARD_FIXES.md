# QA Review: Master 16.1 Mobile And Reward Fixes

Date: 2026-05-18

## Scope

- Promoted master: `10_SOURCE/Masters/Master 16.html`
- Website package: `40_RELEASE/Website_Publish_Package/holesy/index.html`
- Upload copy: `C:\Users\KentLefner\Downloads\holesy-production-upload\index.html`

## Defects Addressed

- Mobile mode-select screen could extend outside the readable phone viewport.
- Mobile Found Documents view was not practically scrollable and the reader blocked too much content.
- A player could receive a found document after losing if their dead score still ranked first.
- Stale pending document state could survive into the next menu cycle and cause repeated document wins.
- Panic cars could skid forever or for unreasonably long distances.

## Fixes Reviewed

- Added mobile overlay scrolling and compressed mobile menu spacing.
- Changed mobile Archive layout to one scrollable page with a capped document list.
- Changed found-document awards to require a live player win.
- Marked document award attempts as consumed per round, whether a drop occurs or not.
- Cleared pending lore drops when `Begin` starts a new run.
- Added car crash distance, friction, speed, and timeout stop conditions.
- Updated package label and in-game build notes to `Master 16.1`.

## Verification

- Extracted and syntax-checked the module script from:
  - `10_SOURCE/Masters/Master 16.html`
  - `40_RELEASE/Website_Publish_Package/holesy/index.html`
- Verified local HTTP 200 responses from repo-root server:
  - `http://127.0.0.1:8770/10_SOURCE/Masters/Master%2016.html`
  - `http://127.0.0.1:8770/40_RELEASE/Website_Publish_Package/holesy/index.html`
- Confirmed both promoted files contain:
  - `Master 16.1`
  - `const BUILD_SUB = 1`
  - `awardEndOfRoundLoreDrop(playerWon)`
  - `crashMaxSlideDistance`

## Remaining Validation

- Physical mobile testing after upload is still required.
- Gameplay validation should specifically force losses with high score, repeated Begin clicks from game-over, Found Documents opening on mobile, and panic-car pileups.

## Result

PASS for file-level defect patch readiness and local HTTP package availability.
