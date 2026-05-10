# QA Review — Master 14.4 Waves Narrative And Pacing Pass

Date:

- 2026-04-29

Candidate under review:

- `20_TESTS/Candidate_Builds/Master 14.4 - waves-narrative-and-pacing-pass.html`

Scope reviewed:

- Priority 2 Waves copy alignment against `WAVES_NARRATIVE_AND_TEXT_PACK.md`
- wave briefing and transition dwell timing changes
- wave-duration and soldier-pressure tuning changes
- Waves-specific end-state messaging

Verified in file review:

- build label updated to `Master 14.4`
- wave intro banners now use the approved labels:
  - `WAVE 1 - DISTRICT OPEN`
  - `WAVE 2 - CONTAINMENT BREACH`
  - `WAVE 3 - EVACUATION ZONE`
  - `WAVE 4 - FINAL LOCKDOWN`
- threat briefings and between-wave transition text now route through explicit lookup tables instead of one-off inline copy
- event-banner timing now gives longer reading windows:
  - wave briefings `6800 ms`
  - wave transitions `8200 ms`
  - next-wave delay `6600 ms`
- per-wave pacing changed from a flat `60` second cadence to:
  - wave 1 `65`
  - wave 2 `65`
  - wave 3 `70`
  - wave 4 `75`
- Waves end-state messaging now distinguishes:
  - player clears all four waves
  - player eliminated while a rival is sole survivor
  - player eliminated under general containment failure framing
  - timer expiry at the end of the Waves run

QA findings from static review:

- no immediate document-to-code mismatch found in the implemented text pack
- no obvious spillover was introduced into Timed or LMS title copy
- no syntax issue was detected in the touched blocks during file inspection

Open QA requirements before promotion:

- run desktop browser playtest through all four waves
- verify the longer transition dwell still feels readable without dragging
- verify wave 4 still feels dangerous enough after the slight pressure softening
- verify the new Waves end-state text matches actual outcomes during:
  - player victory
  - player death with a single rival survivor
  - player death before full wave completion
  - timer-based completion of wave 4
- run at least one mobile-focused pass for readability and pacing comfort

Conclusion:

- `Master 14.4` is a valid rollback-safe candidate for Priority 2 testing
- not yet approved for promotion to `10_SOURCE/Masters` without browser validation
