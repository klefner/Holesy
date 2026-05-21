# QA Review: Master 16.19 How to Play Window

Date: 2026-05-20

## Scope

- Added a separate How to Play field manual page/window from the main menu.
- Preserved the modular browser-client package shape: `index.html`, `how-to-play.html`, `css/styles.css`, and `js/main.js`.
- Refreshed source, release package, and GoDaddy upload convenience folder to `Master 16.19`.

## Coverage Review

- Core play loop: covered movement, eating, growth, tiering, rivals, and soldier damage.
- Modes: covered Timed, Timed post-clock LMS choice, Last Man Standing, Waves, Endless Waves, and Endless fifth-wave reset.
- Difficulty: covered Normal, Hard, Ultra, and the risk/reward of higher pressure.
- Archive: covered document recovery, rare eligible drops, no loss drops, and hidden buff clues.
- Buffs: covered visible starter patterns, Parallax aid effects, active cards, timers, hidden Archive clues, and cooldowns.
- Persistence/UI: covered Endless save/load, Stats window, and build notes access.

## Static Checks

- `node --check --input-type=module` passed for `10_SOURCE/Masters/Master 16/js/main.js`.
- Source, release, and GoDaddy upload `index.html` hashes matched after packaging.

## Runtime Check

- Browser smoke passed on `http://127.0.0.1:8788/index.html?v=16.19-how-to-play-link`.
- Verified the menu `How to Play` control is a `_blank` link to `how-to-play.html`.
- Verified `http://127.0.0.1:8788/how-to-play.html?v=16.19` renders title `Holesy Field Manual`.
- Verified the manual includes the indexed topic frame, starter buff patterns, Archive drop rules, save/load instructions, and Stats guidance.
