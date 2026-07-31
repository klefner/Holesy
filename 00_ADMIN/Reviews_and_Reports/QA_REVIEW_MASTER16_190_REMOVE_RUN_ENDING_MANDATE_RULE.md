# QA Review - Master 16.190 Remove Run-Ending Mandate Rule

Date: 2026-07-29

## Scope

- production `Master 16.189` as the exclusive code basis
- the run-ending consequence for an incomplete Mandate
- preservation of the Mandates system and every Mandate category

## Expected Behavior

- an incomplete Mandate does not end a Waves or Endless run when the wave timer expires
- wave progression continues through the existing path
- every Mandate category remains eligible, including all people categories
- Mandate target selection, progress tracking, HUD rows, completion feedback, rewards, and Mandate Surge remain intact
- player-facing instructions no longer claim that an incomplete Mandate ends the run

## Verification

- JavaScript syntax passed.
- Source and release hashes match for `index.html`, `how-to-play.html`, `js/main.js`, and `js/build-info.js`.
- Static scope check confirms the production timer-expiry path no longer invokes `triggerMandateFailureGameOver()`.
- Static scope check confirms there is no people-group exclusion in Mandate target selection.
- Browser verification must confirm an incomplete Mandate advances normally and a completed Mandate still grants its existing reward.

## Publication

Pending package parity, randomized browser verification, scoped commit, Pages deployment, and live verification.
