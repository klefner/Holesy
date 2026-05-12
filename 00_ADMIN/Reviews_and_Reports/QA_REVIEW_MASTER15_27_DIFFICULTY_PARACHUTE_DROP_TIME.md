# QA Review - Master 15.27 Difficulty Parachute Drop Time

Date:

- 2026-05-12

Candidate under review:

- `C:\Users\KentLefner\Desktop\game-repo\Holesy-clean\20_TESTS\Candidate_Builds\Master 15.27 - difficulty-parachute-drop-time.html`

Baseline:

- `C:\Users\KentLefner\Desktop\game-repo\Holesy-clean\20_TESTS\Candidate_Builds\Master 15.26 - hole-wind-audio-disabled.html`

Backlog / issue basis:

- User asked whether soldier drop time was variablized and inversely scaled as difficulty increases.
- It was not; `parachuteFallTime` was fixed at the performance config value.

Reason for candidate:

- make troop landing pressure scale with difficulty
- preserve the existing performance profile base fall time while applying gameplay difficulty on top

Implementation review:

- build marker advanced to `Master 15.27`
- difficulty profiles now include `parachuteFallMult`
- Normal keeps the baseline fall time
- Hard shortens paratrooper fall time
- Ultra shortens paratrooper fall time further
- paratrooper instances now receive difficulty-scaled `totalT` when deployed
- debug overlay now shows the active drop fall duration

Risk controls:

- no `Master 15` source file was changed
- no website release package was changed
- performance profile and difficulty remain separate axes
- a minimum fall-time floor prevents extreme values

Validation required:

1. Launch the candidate and confirm the build marker shows `Master 15.27`.
2. Start Waves mode on Normal and observe baseline paratrooper descent.
3. Start Waves mode on Hard and confirm paratroopers land faster than Normal.
4. Start Waves mode on Ultra and confirm paratroopers land faster than Hard.
5. Confirm troop visuals, landing conversion, and soldier behavior still work.
6. Confirm no browser console errors occur during troop deployment and landing.

Validation performed on 2026-05-12:

- Extracted module script syntax check passed.
- Local preview server returned HTTP 200 for the candidate URL.
- Candidate content confirms the `Master 15.27` build marker.
- Candidate content confirms `parachuteFallMult` values exist for Normal, Hard, and Ultra.
- Candidate content confirms deployed paratroopers use difficulty-scaled fall duration.
- In-app browser smoke check loaded the candidate with the `Master 15.27` marker visible and no console errors.

Validation still open:

- None.

User validation on 2026-05-12:

- Everything seemed good.
- User ran 15 games, five per game mode / difficulty layer:
  - Normal: 5 wins, 0 losses, 100% win rate
  - Hard: 3 wins, 2 losses, 60% win rate
  - Ultra: 1 win, 4 losses, 20% win rate
- User assessed the win-rate pattern as reasonable for a three-layer difficulty model.

Status:

- user validation passed
- accepted as the active basis for the next candidate slice
