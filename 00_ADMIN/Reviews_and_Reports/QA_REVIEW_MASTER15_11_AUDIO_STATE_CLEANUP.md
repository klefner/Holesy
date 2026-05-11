# QA Review - Master 15.11 Audio State Cleanup

Date:

- 2026-05-11

Candidate under review:

- `C:\Users\KentLefner\Desktop\game-repo\Holesy-clean\20_TESTS\Candidate_Builds\Master 15.11 - audio-state-cleanup.html`

Baseline:

- `C:\Users\KentLefner\Desktop\game-repo\Holesy-clean\20_TESTS\Candidate_Builds\Master 15.10 - wave-end-countdown-audio.html`

Backlog / issue basis:

- Failed user validation: the 3-second Waves countdown sound was not heard.
- User-reported defect: aid radar, soldier warning, and plane sounds could continue or mismatch after leaving active gameplay.
- Issue log: `QA-005`.

Reason for candidate:

- keep the failed `15.10` candidate traceable
- strengthen the countdown cue so it should be clearly audible over the game mix
- stop gameplay-world audio from leaking onto game-over and game-mode selection screens

Implementation review:

- build marker advanced to `Master 15.11`
- countdown cue gain, tone length, and routing were strengthened
- countdown cue now requires active Waves gameplay before playing
- aid radar pings and alien-aid chimes now require active gameplay
- soldier voice, gunshot, and plane-engine audio now require active gameplay
- plane-engine drones now route through the governed sound-effects gain path instead of bypassing it
- round reset and menu return explicitly silence non-music gameplay audio

Risk controls:

- no `Master 15` source file was changed
- no website release package was changed
- candidate is isolated to the audio-state defect and countdown validation failure

Validation required:

1. Launch the candidate and confirm the build marker shows `Master 15.11`.
2. Start Waves mode with audio enabled.
3. Confirm one audible cue plays near 3, 2, and 1 seconds before a wave timer reaches zero.
4. Confirm the cue repeats correctly on the next wave.
5. Return to game-mode selection after gameplay and confirm aid radar, soldier warnings, gunshots, and plane engine audio stop.
6. Confirm no browser console errors occur during the flow.

Validation performed on 2026-05-11:

- Extracted module script syntax check with `node --check`; result passed.
- Confirmed the candidate launches locally at `http://127.0.0.1:8765/20_TESTS/Candidate_Builds/Master%2015.11%20-%20audio-state-cleanup.html`.
- Confirmed the visible build marker reports `Master 15.11`.
- Started Waves mode and confirmed the UI entered Wave 1.
- Browser console error log remained clear during launch and Waves start.

Validation still open:

- Audible confirmation by the user.
- Audio-leak confirmation by the user.

Status:

- candidate prepared for validation
- not approved for promotion
