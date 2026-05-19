# Architecture Decision - Modular Client Split

Date:

- 2026-05-12

Status:

- accepted; PERF-010 closure evidence complete

Context:

- Holesy currently ships candidate builds as large browser-run HTML files containing the full game.
- The file size is becoming a maintainability and change-control risk.
- Browser runtime performance problems should be solved through profiling, targeted optimization, asset management, and client-side architecture, not by moving the moment-to-moment game loop to a Python/server process.

Decision:

- Keep the game client-side.
- Split the giant HTML file over time into browser-native assets.
- Do not use a Python/server rewrite as a direct FPS or game-loop performance fix.
- Preserve the existing governed candidate-build workflow during the transition.

Target Structure:

```text
index.html
css/styles.css
js/main.js
js/gameLoop.js
js/player.js
js/enemies.js
js/levels.js
js/ui.js
js/saveSystem.js
assets/images/
assets/audio/
data/levels.json
```

Client Responsibilities:

- game loop
- rendering
- input
- animation
- collision
- audio
- AI and immediate gameplay decisions

Backend / Server Responsibilities, If Added Later:

- accounts
- cloud saves
- leaderboards
- analytics
- downloadable content
- multiplayer coordination
- anti-cheat

Future Escalation Options:

- Use JavaScript modules for organization.
- Use code splitting / lazy loading after startup payload becomes a measured problem.
- Use Web Workers only for heavy background work such as pathfinding, procedural generation, AI calculations, map generation, or large save/load compression.
- Use OffscreenCanvas only if rendering becomes the measured bottleneck.

First Safe Migration Slice:

- Create `Master 15.28 - modular-css-proof.html` from `Master 15.27 - difficulty-parachute-drop-time.html`.
- Move the inline stylesheet into `Master 15.28 - modular-css-proof.css`.
- Leave gameplay JavaScript unchanged except the build marker.
- User validation passed on 2026-05-12.

Second Safe Migration Slice:

- Create `Master 15.29 - modular-js-data-proof.html` from the validated `Master 15.28` proof.
- Keep the extracted stylesheet external.
- Move build/version metadata into `Master 15.29 - build-info.js`.
- Move difficulty-profile data into `Master 15.29 - difficulty-profiles.js`.
- Leave the game loop, rendering, input, collision, scoring, audio, and AI behavior unchanged.
- Browser smoke validation passed through `QA_REVIEW_MASTER15_29_MODULAR_JS_DATA_PROOF.md`; user validation of this extra proof remains optional follow-up evidence, not a blocker to the architecture decision.

Acceptance:

- candidate loads in the browser through the existing local preview workflow
- CSS loads from the adjacent file
- build marker shows the current proof candidate version
- game mode selector remains styled
- gameplay starts normally
- no console errors caused by the CSS extraction
- no console errors caused by the JS module extraction

Architectural Rationale:

- Modularization improves maintainability, reviewability, and load control.
- Modularization does not automatically improve runtime FPS.
- Python/server work should be reserved for server problems, not browser-frame problems.
- The right path is incremental extraction with proof at each step.

Closure:

- PERF-010 is closed as the architecture decision gate.
- The accepted direction is incremental client-side modularization with browser-native assets.
- Full modular implementation remains future work and should be sized as feature-support work rather than left as an open Priority 1 blocker.
