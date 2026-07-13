# QA Review - Master 16.88 Army Boss

Date: 2026-06-23

Scope:

- Add an army boss escalation to the governed Master 16 source and release package.
- Preserve the existing modular source/release package shape.
- Confirm source and release package parity for changed entry/runtime files.

Behavior Reviewed:

- Endless Waves spawn one army boss every fifth wave through the existing troop-drop plane path.
- Timed mode spawns one late-round army boss when the timer reaches the final 30 seconds.
- Regular Waves mode spawns one late-run army boss during the final configured wave.
- Last Man Standing spawns one army boss when the match reaches the endgame with two surviving holes.
- Army bosses use a red uniform, four-times soldier scale, three-times bullet damage, larger eat requirement, and larger soldier-family reward credit.
- Army boss identity is serialized and restored for soldiers, paratroopers, and undeployed planes in Endless saves.
- Endless saves also preserve a pending army boss drop before the boss plane has deployed.

Verification:

- Passed: JavaScript syntax check for source and release `js/main.js` and `js/build-info.js`.
- Passed: source/release hash parity check for `index.html`, `js/main.js`, `js/build-info.js`, and `PACKAGE_MANIFEST.md`.

Defects Detected:

- None at artifact creation time.
