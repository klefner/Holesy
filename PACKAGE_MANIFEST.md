# Holesy Modular Package Manifest

Build label: `Master 16.166`

Purpose: enumerate the complete governed modular package so release and upload work no longer treats `index.html` as the whole game.

## Required Package Files

- `index.html`
- `how-to-play.html`
- `PACKAGE_MANIFEST.md`
- `css/styles.css`
- `js/main.js`
- `js/build-info.js`
- `js/difficulty-profiles.js`
- `js/government-physics.js`
- `data/lore-documents.js`
- `assets/images/how-to-play-game-summary.svg`
- `assets/environments/downtown-city-megakit/` source assets for the optional MegaKit Downtown test environment

## Required Package Directories

- `assets/audio/`
- `assets/environments/`
- `assets/images/`
- `css/`
- `data/`
- `js/`

## Upload Rule

- Clean install or live drift recovery: upload the full governed `holesy/` folder with this structure intact.
- Routine update when live is already on the prior approved master: upload a changed-files-only delta that preserves these same relative paths under `/holesy/`.
- Removed live files must be deleted manually; a delta upload cannot remove old live files by itself.

## PERF-012 Completion Evidence

- Phase 1 package shape is complete: entry HTML, stylesheet, JS modules, assets, and data are separate files in source and release.
- Phase 2 low-risk extraction is complete: build metadata, difficulty profiles, lore documents, starter unlock data, and government-building physics are external modules.
- Source and release package paths mirror this manifest for the current governed baseline.
- `Master 16.81` repairs MegaKit Downtown detail visibility by widening/brightening non-colliding block-edge trim and enlarging the circular road manholes while keeping fake terrain and non-breakable showcase buildings out; themed buildings must use the validated destruction paths before returning.
- `Master 16.82` adds visible run goals and local object-family mastery for people, vehicles, props, trees, buildings, soldiers, and MegaKit manholes; goal completion grants immediate score, while mastery remains feedback-only until the defense-scaling investment loop is designed.
- `Master 16.83` repairs the Run Goals performance regression by dirty-flagging HUD updates, expands the pool to 50 larger randomized goals, and adds the visible Goal Sweep reward for clearing all displayed goals.
- `Master 16.84` hotfixes the remaining Run Goals/mastery performance regression by limiting per-bite bookkeeping to active goal families and batching local mastery saves instead of writing storage during every bite.
- `Master 16.85` adds readable hover/focus instructions to each Run Goal, including the exact target objects, required count, and score reward, without adding gameplay-loop work.
- `Master 16.86` removes duplicate startup city construction, batches world generation across frames, and caps HUD/live-score DOM refreshes.
- `Master 16.87` completes the comprehensive runtime pass across rendering resources, lighting registries, object/physics scans, audio warmup, gameplay overlays, and adaptive renderer fallback.
- `Master 16.88` adds the red army boss escalation: one four-times-larger, three-times-deadlier command unit appears every fifth Endless wave, late in Timed rounds, late in regular Waves, and during Last Man Standing endgames.
- `Master 16.89` fixes the Wave 1 to Wave 2 lock by correcting the army boss late-mode trigger to use the existing timed-mode state model.
- `Master 16.90` replaces the red command-unit boss with a green tank that is roughly twice a car footprint, pushes loose objects, and pressure-collapses buildings through the existing physics systems.
- `Master 16.91` temporarily forces the green tank army boss into every playable mode and wave so player testing can see it immediately; remove the temporary test flag after validation.
- `Master 16.92` randomizes alive-hole corner assignment at each wave start and avoids repeating a hole's previous corner when another corner is available.
- `Master 16.93` corrects skyscraper collapse direction so chunks launch outward from the building center toward the hit/source side instead of being shoved inward before scatter.
- `Master 16.94` removes temporary tank-every-level testing, adds the Siege Tank / Twin-Gun Mech / Shield Commander boss roster, and unlocks each revealed boss archetype as a later-wave random drop at reduced damage.
- `Master 16.95` expands the boss roster to ten archetypes and repairs Endless world-shift resizing so the reset occurs on the sixth wave after each five-wave growth block.
- `Master 16.96` keeps consumed boss-derived offensive units visible while falling into the hole and spotlights true fifth-wave boss forms with a 40% size boost plus red neon glow.
- `Master 16.97` preserves all player/rival scores during Endless respawns, rolls rival AI difficulty once per run, makes rivals collect their own collapse rubble, and escalates offensive-unit speed/damage by wave.
- `Master 16.98` replaces the straight offensive-unit wave ramp with a stronger nonlinear wave-pressure curve and moves Game Stats into an in-page modal so the Stats button no longer depends on popup permissions.
- `Master 16.99` adds the Devour Mandate target system: each populated district selects one person, prop, car, mid building, and skyscraper target, tracks five HUD dots, awards completion bonus score, and shows failure feedback when time or survival runs out.
- `Master 16.101` repairs Mandate clarity by changing hidden exact-object targets into explicit category counts, showing live row progress such as `Eat People 0/8`, and moving the panel lower under the timer/control cluster so the title and instructions stay readable.
- `Master 16.102` repairs Mandate font readability by matching the Mandate objective rows and progress counts to the Run Goals row font scale while preserving the count-based rules.
- `Master 16.125` replaces themed Mandate and Run Goal labels with direct instructions and adds ten more distinct street-object types.
- `Master 16.124` removes non-consumption score rewards, scales rival aggression, adds ten street fixtures, adds a seven-note boss fanfare, and temporarily enables a boss every Endless wave for testing.
- `Master 16.123` adds immediate priority-based message arbitration, compact mobile banners, boss audio ducking, and a non-refreshing reduced unit-clear speed boost.
- `Master 16.122` adds distinct boss-inbound warning feedback and a major player boss-defeat reward moment.
- `Master 16.121` adds distinct Run Goal completion feedback and a larger Goal Sweep reward moment.
- `Master 16.120` starts recorded audio decoding from the initial player gesture and adds earned Mandate-row reward feedback.
- `Master 16.119` adds transparent first-run adaptive assistance, a red HUD indicator, and persistent/exportable reason logging.
- `Master 16.118` keeps Wave 1 soldier-free on every difficulty and prevents impossible Wave 1 military Mandates.
- `Master 16.117` adds a Capacitor native app shell scaffold for iOS and Android, installs `@capacitor/haptics`, and points the shell at the governed modular release package so the native haptics bridge can run on real devices.
- `Master 16.116` adds a last-resort iOS WebKit switch-control haptic fallback before browser vibration reports unsupported; the Haptics button reports `iOS Tick` when that fallback is triggered.
- `Master 16.115` adds a native haptics bridge path that uses Capacitor Haptics or a `HolesyNativeHaptics` custom bridge before falling back to browser vibration, so existing gameplay haptic events can work in a native iOS/Android app shell.
- `Master 16.114` clarifies unsupported browser haptics by changing the diagnostic label from `No API` to `No Haptics` and explaining that browsers without `navigator.vibrate` cannot make the device vibrate.
- `Master 16.113` adds a stable `latest.html` launcher for mobile testing, strengthens the explicit Haptics test pulse, exposes the haptic result on the button itself, and increases devour/heavy-object vibration durations for supported mobile browsers.
- `Master 16.112` repairs the mobile Haptics test control with touch-safe activation and visible status, and moves the Music mute control to the mobile gameplay bottom-left stack above `HUD+`.
- `Master 16.111` randomizes Mandates across a larger target pool including people variants, street props, trees, cars, buildings, rival holes, soldiers, military units, and bosses; the number of active rows ramps from one early target type toward the five-row maximum, with randomized supply-capped counts.
- `Master 16.110` adds a mobile-first compact HUD, an expandable HUD toggle, and haptic support diagnostics so mobile play keeps the field visible and exposes unsupported vibration browsers.
- `Master 16.109` repairs Mandate fairness by lowering first-wave people/car pressure, adding a clearer per-wave count curve, and capping requirements against the actual category supply.
- `Master 16.108` adds mobile haptic pings for player devours and distinct haptic patterns for special gameplay events, guarded by feature detection and cooldowns.
- `Master 16.107` makes wave-based Mandates truly mandatory by ending the run on incomplete wave expiry, and rewards completed Mandates with a wave-long Mandate Surge speed/protection burst.
- `Master 16.106` makes Endless Waves the default flagship mode, raises true-boss swallow requirements, adds speed-boost resistance to boss eating, and makes bosses hit harder while kiting away from nearby holes to keep firing.
- `Master 16.105` makes building debris heavier, lowers artificial upward hop/bounce, and prevents medium-building voxel cubes from snapping to the ground before they actually fall there.
- `Master 16.104` repairs the mobile mode picker by making each visible game-mode choice a real button with touch-friendly styling and pressed-state updates.
- `Master 16.103` retunes Mandate pressure so required counts are based on a larger share of the live district inventory and increase by wave, making the Mandate feel urgent through most of the timer.
