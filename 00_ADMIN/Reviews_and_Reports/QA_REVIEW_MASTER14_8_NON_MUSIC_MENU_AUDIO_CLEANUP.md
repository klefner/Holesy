# QA Review — Master 14.8 Non-Music Menu Audio Cleanup

Date:

- 2026-04-30

Candidate under review:

- `C:\Users\KentLefner\Desktop\game-repo\Holesy\20_TESTS\Candidate_Builds\Master 14.8 - non-music-menu-audio-cleanup.html`

Baseline:

- `C:\Users\KentLefner\Desktop\game-repo\Holesy\20_TESTS\Candidate_Builds\Master 14.7 - transition-freeze-and-outcome-clarity.html`

Reason for candidate:

- user validated the `14.7` transition / clarity slice
- next requested narrow slice was to kill all non-music sounds on scoreboard and mode-select screens while preserving music

Implementation review:

- candidate adds explicit non-music gain-bus mute helpers for SFX, ambience, and celebration channels
- candidate clears alien aid ships and stops the alien-aid loop when entering menu-state screens
- candidate stops and disconnects active plane engine drones so lingering aircraft audio cannot leak onto the scoreboard
- candidate restores the non-music audio mix when gameplay resumes or a new wave starts
- startup / mode-select control flow was intentionally left untouched to avoid repeating the abandoned `14.5` startup regression family

QA assessment:

- rollback safety preserved by using a new candidate cut from the validated `14.7` base
- scope remains narrow and audio-focused
- no browser playtest was executed in this QA pass

Open validation required:

- confirm mode selection and `Begin` still work
- confirm scoreboard and mode-select screens are silent except for music
- confirm gameplay audio returns normally when a round starts or resumes
- confirm no plane engine drone survives onto end-of-round screens

Status:

- candidate ready for user browser validation
- not approved for promotion
