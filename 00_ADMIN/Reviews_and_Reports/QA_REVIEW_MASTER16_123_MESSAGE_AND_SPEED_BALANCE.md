# QA Review - Master 16.123 Message And Speed Balance

Date: 2026-07-11

## Messaging

- No delayed message queue.
- One banner at a time.
- Current critical messages replace routine messages immediately.
- Routine messages are discarded while a critical message is active.
- Boss, failure, district, and new-wave messages are critical.
- Copy is compacted and capped; mobile uses a smaller banner and font.

## Balance and audio

- Boss warning/defeat cues temporarily duck ordinary gameplay audio.
- Unit-clear speed reward is reduced from 1.30-1.55x for 3.75-6 seconds to 1.12-1.22x for 2-3.5 seconds.
- A live unit-clear speed boost cannot be refreshed by clearing another soldier group.

## Verification

- Source/release syntax and parity checks pass.
- Served entry point reports `Master 16.123`.
- Live-play timing, readability, audio hierarchy, and Hard-mode runaway pressure remain for player validation.
