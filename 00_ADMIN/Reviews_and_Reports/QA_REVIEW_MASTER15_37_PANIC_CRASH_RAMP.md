# QA Review - Master 15.37 Panic Crash Ramp

Date:

- 2026-05-12

Candidate under review:

- `C:\Users\KentLefner\Desktop\game-repo\Holesy-clean\20_TESTS\Candidate_Builds\Master 15.37 - panic-crash-ramp.html`
- `C:\Users\KentLefner\Desktop\game-repo\Holesy-clean\20_TESTS\Candidate_Builds\Master 15.37 - panic-crash-ramp.css`
- `C:\Users\KentLefner\Desktop\game-repo\Holesy-clean\20_TESTS\Candidate_Builds\Master 15.37 - build-info.js`

Baseline:

- `C:\Users\KentLefner\Desktop\game-repo\Holesy-clean\20_TESTS\Candidate_Builds\Master 15.36 - car-crash-trigger-fix.html`

Backlog / issue basis:

- User validation continued to show speed-up and wobble but no crashes.
- User clarified desired design: people/drivers should be terrified, crashes should be more common than only speeding up, and crash potential should increase as cars reach top speed.

Implementation review:

- created `Master 15.37 - panic-crash-ramp.html`
- advanced build marker to `Master 15.37`
- changed car panic to trigger from nearby-hole threat, not only from clean escape direction
- increased base panic crash chance from `0.22` to `0.55` per second
- lowered crash speed gate from `9.75` to `8.75`
- shortened panic warm-up from `0.65s` to `0.35s`
- shortened forced-crash window from `1.45s` to `1.1s`
- increased forced crash rate from `1.25` to `2.6` per second
- added top-speed crash multiplier so risk rises sharply as cars approach panic max speed
- added wrong-way crash multiplier so cars not actually escaping are more likely to lose control

Risk controls:

- no source master was changed
- no website release package was changed
- crash behavior still requires nearby-hole panic and the speed gate
- non-panicked traffic still follows baseline movement

Validation required:

1. Launch candidate and confirm build marker shows `Master 15.37`.
2. Confirm nearby cars speed up and wobble.
3. Confirm crashes are now common enough to observe during repeated chases.
4. Confirm cars near top panic speed crash more often than slower panic cars.
5. Confirm cars not cleanly escaping are more crash-prone.
6. Confirm calm traffic away from holes does not crash.
7. Confirm no browser console errors during panic, crash, smoke/flame/explosion, and consumption.

Validation performed on 2026-05-12:

- Candidate file exists.
- CSS file exists.
- Build-info module exists.
- Candidate imports `Master 15.37` module files.
- Candidate content confirms nearby-hole panic trigger.
- Candidate content confirms top-speed and wrong-way crash multipliers.
- Local preview returned HTTP 200 for the candidate HTML, CSS, build-info, difficulty-profiles, and game-stats files.
- Extracted module script passed JavaScript syntax check.

Validation still open:

- Player-facing gameplay validation.
- User validation.

Status:

- ready for gameplay validation
