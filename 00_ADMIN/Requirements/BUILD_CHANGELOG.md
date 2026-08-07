# Holesy Build Changelog

Purpose: maintain concise build notes that can become player-facing patch notes inside the game.

## Master 16.202 - 2026-08-07

Added motion-comfort controls without slowing Hole-Eye movement.

- Makes keyboard A/D strafe and S backpedal without forcing camera yaw.
- Replaces unbounded direction-vector turning with angle-based yaw acceleration, braking, deadband, and profile-specific maximum speed.
- Removes first-person impact shake and moves the camera modestly higher/farther back for small holes.
- Adds turn-triggered peripheral Hunger shading and Balanced, Comfort, Immediate, and 30-degree Snap Turn profiles.
- Keeps the compass north-aware while guaranteeing it and the comfort overlay disappear when gameplay ends.
- Fixes an immediate Begin press being discarded during the first 500 milliseconds after page load and adds recoverable startup error handling.

## Master 16.201 - 2026-08-07

Reduced the extreme overhead rendering cliff without removing Harvest gameplay content.

- Keeps every Harvest object present, simulated, edible, and score-bearing.
- Omits only distant loose details projected below roughly three screen pixels once the player reaches Void/Abyss scale in overhead view.
- Keeps buildings, people, animals, powerups, active falls, physics pieces, and everything near the player visible.
- Restores full detail automatically at smaller hole sizes and in Hole-Eye view.
- Adds a short synthesized pop for consumed objects that do not have an explicit category sound, including tumbleweeds.
- Routes fallback pops through the existing 24-voice SFX ceiling and explicit node cleanup.
- Adds runtime telemetry for culled Harvest micro-details and generic consume-pop playback.

## Master 16.200 - 2026-08-06

Removed the cumulative procedural-score source leak responsible for wave-five audio collapse.

- Replaces the append-only music source array with a live-source set.
- Removes and disconnects each completed note source automatically.
- Disconnects shared filters, envelopes, noise filters, and vibrato nodes once every source in that note group has ended.
- Keeps emergency stop behavior while preventing normal play from accumulating roughly 22 ended source references per second.
- Adds current and peak music-source telemetry for endurance verification.
- Preserves the full Master 16.198 Harvest baseline and the Master 16.199 SFX cap and scheduler-stall protections.

## Master 16.199 - 2026-08-06

Protected late-wave audio continuity while retaining the full Master 16.198 Harvest baseline.

- Keeps every Master 16.198 Harvest parcel cluster, starter object, actor, intact building, destruction tier, and food value unchanged.
- Detects a delayed procedural-music scheduler and advances the musical playhead instead of scheduling every missed note simultaneously.
- Limits non-music playback to 24 simultaneous voices with three priority reserve voices for player-kill and cannon cues.
- Explicitly disconnects completed source, gain, and reverb nodes so five waves of consumption cannot leave an ever-growing audio graph.
- Adds live telemetry for active SFX voices, dropped overflow voices, music-scheduler recoveries, and AudioContext state.

## Master 16.198 - 2026-08-06

Rebuilt Harvest performance around functioning-town design without removing its food economy.

- Replaces the uniform 11x11 scatter grid with 44 overlapping farmstead clusters and a guaranteed 80-object starter feeding trail.
- Preserves 17 intact authored buildings and their combined 8,804-point food value while reducing dormant breakup bookkeeping from about 1,760 pieces to 228 readable pieces.
- Uses 8–20-piece destruction tiers, batches field rows, disables unnecessary small-prop shadow casting, sleeps distant rural actors, and rebuilds the interaction grid once per four frames.
- Caps Run Goals against generated category supply and scales rewards proportionally.
- Adds comparative frame-time, draw-call, triangle, object, building-piece, and awake-actor telemetry.

## Master 16.197 - 2026-07-31

Restored the crowded Harvest population and removed unexplained Mandate terminology.

- Reverses the Master 16.195/16.196 object cap, density cuts, actor cuts, and building-fragment reductions.
- Restores the exact Harvest generation logic last used in the crowded Master 16.194 test build.
- Retains the higher Hole-Eye camera framing introduced after 16.194.
- Replaces Mandate Surge wording with explicit wave-long speed-boost and damage-protection language.
- Establishes that future performance work must preserve population and instead reduce rendering and simulation cost.

## Master 16.196 - 2026-07-31

Rebalanced the fixed Harvest budget around first-wave growth and raised the close camera.

- Keeps Harvest at exactly 1,000 generated gameplay objects and 28 buildings.
- Reduces dormant destruction pieces from 20 to 12 per building, freeing 224 slots for visible edible content.
- Provides 240 small starter-food objects worth 4,240 points near the player before distributing larger food across the county.
- Models a wave-one reachable radius of at least 5.44 with seven building stacks reachable across three tested generations.
- Raises and backs off Hole-Eye View so the full hole rim remains visible with the surrounding landscape.

## Master 16.195 - 2026-07-31

Established the first exact town object-budget trial in Harvest County.

- Generates exactly 1,000 Harvest gameplay objects every run.
- Increases Harvest from 17 to 28 buildings while reducing each building to 20 destruction pieces.
- Removes the 11x11 mass-scatter population and 96-worker excess.
- Reduces individually simulated crops, animals, signs, wagons, road actors, and tumbleweeds while preserving immediate starter-area activity.
- Exposes the target, generated total, and building-piece count as document telemetry for repeatable QA.

## Master 16.194 - 2026-07-30

Added the responsive Hole-Eye first-person view.

- Adds size-aware camera sensitivity and camera-relative WASD/arrow movement.
- Keeps the mouse cursor and HUD controls available, adds `P` pause access, and lets `V`, Escape, the HUD button, or a debounced mouse-wheel gesture change views.
- Adds a north-pointing first-person compass for location-based Mandates.
- Reduces first-person camera range and fog distance to limit rendering cost.

## Master 16.193 - 2026-07-30

Improved crowded-scene frame pacing without thinning the town.

- Replaces the full-list interaction starting point with a nearby world-cell index.
- Updates passive pedestrians, animals, and tumbleweeds at a stable 30 Hz while preserving responsive panic and gameplay systems.
- Makes adaptive resolution react after one sustained slow window and recover gradually after stable performance.
- Preserves every Harvest object, building, sign, road, trail, and gameplay rule from `Master 16.192`.

## Master 16.192 - 2026-07-29

Mounted frontier storefront signs.

- Sizes each storefront sign conservatively against its imported building facade.
- Mounts signs at low storefront-fascia height instead of above the roofline.
- Removes detached synthetic awnings while preserving the curved, weathered Harvest trail network.

## Master 16.191 - 2026-07-29

Living rural sites, weathered trails, and Railgate catalog foundation.

- Replaces Harvest County's displaced hidden-grid parcels with collision-spaced rural sites placed directly in world space.
- Converts historic routes into curved spline ribbons with continuously varying widths, irregular borders, dry soil, mud, wet patches, and wagon tracks.
- Adds 11 visibly distinct intact/broken wagon and wheel forms, with 26 randomly distributed consumable instances per Harvest generation.
- Adds an exactly-100-entry Railgate-owned object catalog and a separate 20-entry explicitly shared modern-object catalog.
- Records which Railgate objects can use confirmed supplied models versus procedural or compound authoring.

## Master 16.190 - 2026-07-29

Mandates no longer end runs.

- Removes only the rule that ends a run when the Mandate target list is incomplete.
- Retains every Mandate category, target, progress row, completion reward, and Mandate Surge.
- Lets Waves and Endless advance normally when a Mandate is incomplete.

## Master 16.189 - 2026-07-29

Organic frontier town topology.

- Establishes topology as a town-recipe decision instead of forcing every environment onto the modern street grid.
- Hides asphalt, yellow lane markings, and concrete sidewalk parcels in Medieval Village and Harvest County.
- Adds four irregular dirt-road routes whose branches bend, merge, and stop inside the board.
- Offsets Harvest farm and building sites so the settlement no longer reads as 36 identical square parcels.
- Adds six smaller frontier buildings: a saloon, sheriff office and jail, general store, livery stable, feed-and-grain shop, and frontier house.
- Reuses the governed imported-model destruction pipeline for every added structure.
- Leaves Classic City and MegaKit Downtown on the existing modern grid.

## Master 16.188 - 2026-07-29

Living Harvest economy and fair mandates.

- Raises Harvest County's verified edible inventory from 792 to 1,111.
- Adds 14 visible cultivated fields, 58 farm workers, eight mounted riders, eight horse-drawn carriages, seven tractors with plows, and a larger livestock population.
- Increases themed crop, prop, and livestock value so a strong opening clear reaches the smallest farm structures while larger landmarks remain later progression.
- Filters building-piece Mandates against the radius a player can realistically earn from a contested board.
- Replaces generic urban Mandate labels with farm-specific wording in Harvest County.
- Prevents imported animals from enlarging at consume start and makes them shrink earlier during their descent.

## Master 16.187 - 2026-07-29

Fast Harvest County startup.

- Replaces sequential Harvest County network/decode waits with one parallel asset-preload job.
- Begins preloading immediately when Harvest County is selected, before Begin is pressed.
- Reuses the warmed loader caches during town construction.
- Preserves all buildings, destructible fragments, crops, animals, props, and themed density.
- Reduced measured Begin-to-gameplay time from 10.8 seconds on live 16.185 to 1.17 seconds under a deliberately delayed local asset network.

## Master 16.186 - 2026-07-27

Medieval sound, scale, and growth repair.

- Repairs the gameplay-audio transition that faded the existing soundtrack out immediately after Begin.
- Preserves each swallowed object's authored scale instead of forcing imported animals to scale `1`.
- Makes animals shrink continuously from near the hole mouth to sell the long fall.
- Increases Medieval parcel forage from 14 to 20 staged small edibles per building parcel.
- Adds period-specific road life using torch walkers, loose horses, and horse-drawn carts.
- Retains staged structure progression: small buildings are opening-wave targets; medium, large, and tower structures remain earned escalation targets.

## Master 16.185 - 2026-07-27

The hidden road to Crownlands.

- Designates Crownlands as the secret town and adds a four-achievement constellation that must be earned during one continuous game rather than assembled from permanent achievement history.
- Preserves the current constellation across waves, world shifts, and an explicit Endless save/load; a new unsaved game begins with an empty constellation.
- Permanently records the completed Crownlands gate while keeping the unfinished town out of automatic rotation and the temporary city selector.
- Adds three cryptic Archive records describing the four proofs as a living threshold, five leafy witnesses, an emptied square, and the fifth bell.

## Master 16.184 - 2026-07-27

Town-era response profiles.

- Replaces Medieval aircraft, paratroopers, soldiers, robots, vehicles, and futuristic bosses with ground-arriving swordsmen, longbow archers, mounted lancers, and the mounted Iron Reeve warlord.
- Gives the Iron Reeve a 6.2 minimum devour gate, faster close-range pressure, and materially higher damage so it cannot be erased by an under-grown hole.
- Removes electric streetlights from fresh and restored Medieval state while retaining the alien aid ship as the one intentional future anomaly.
- Adds satellite crop plots and livestock across the village, raising the verified loose themed population from 448 to 544.
- Establishes the positive town-theme profile and owned-pack composition matrix used to build future towns without inheriting incompatible modern content.

## Master 16.183 - 2026-07-27

Medieval roadside theme repair.

- Removes fire hydrants and their water-jet devour response from Medieval Village without changing modern-city hydrants.
- Replaces all modern roadside-prop rolls on Medieval native parcels with baskets, sacks, crates, barrels, hay, carts, trees, and torch-bearing villagers.
- Converts any hydrant found in a legacy Medieval save into a barrel during restoration so old state cannot reintroduce the mismatch.

## Master 16.182 - 2026-07-27

Harvest County and strict town-specific content.

- Adds the first town from the July model portfolio with eight rotating farm structures, seven crop varieties, horses, cows, pigs, sheep, orchards, wells, coops, and dense farmyard props.
- Processes barns, silos, the water tower, and windmill through the governed authored-facade destruction pipeline.
- Supplies 792 themed devourables, including a guaranteed 20-object starter ring, 42 roaming livestock, and zero cars in the tested Endless Wave 1 build.
- Removes modern cars from Medieval Village, guarantees all four Medieval commons scenes, adds horses, and raises the verified Medieval population to 448 loose edibles plus 43 ambient actors.
- Filters town-incompatible Run Goals and adds animal goals so car-free towns cannot demand cars.

## Master 16.181 - 2026-07-22

Reusable imported-city model destruction.

- Replaces Medieval Village's procedural colored destruction blocks with validated fragments clipped from all ten authored building models.
- Preserves original materials and distinctive surfaces over closed inset cores throughout breach, collapse, and settled debris.
- Restores whole-building progression gates at 4.25 for houses, 5.25 for medium structures, 6.00 for the mill, and 6.50 for the bell tower; exposed fragments retain the shared fit and jam rules.
- Increases normal Medieval parcel edibles from six to fourteen and preserves windmill blades with an explicit model-only conversion override.
- Establishes the governed manifest, normalization, conversion, runtime, and validation procedure for future imported-model cities.

## Master 16.180 - 2026-07-22

Living Medieval commons and street life.

- Replaces both modern park/sports parcels in Medieval Village with two distinct rotating commons selected from farms, cow/sheep pastures, barnyards, and sword-training yards.
- Adds edible colored vegetable rows, cows, sheep, chickens, hay, barrels, crates, carts, and training targets.
- Adds animated free-roaming animals, torch-bearing walkers, and sparring villagers across the broader town.
- Keeps the authored Blacksmith and Stable in the pack-building rotation and preserves the established 32 pack parcels / four non-pack parcels composition.
- Uses bounded lightweight animation and shared geometry/material caches to control browser cost.

## Master 16.179 - 2026-07-22

Resilient Medieval asset loading.

- Retries each Medieval GLB independently up to three times after transient host failures.
- Evicts rejected cache entries so a retry performs a real new request.
- Continues loading the remaining town if one model stays unavailable and still completes the 192-object edible ladder.

## Master 16.178 - 2026-07-22

Medieval growth ladder and physical-fit building entry.

- Adds six medieval edibles around every one of the 32 pack-building parcels: baskets, sacks, crates, barrels, and hay.
- Supplies 192 themed loose objects across the pack-authored portion of the town, staged from starter-hole food to larger mid-run objects.
- Removes the arbitrary 43-percent-of-whole-footprint gate from non-skyscraper Medieval structures.
- Matches original-city small and medium building behavior: proximity exposes the façade pieces, then the established physical-fit check measures each piece's width, depth, height, and size against the hole.
- Preserves the jam/eject behavior when a specific piece does not physically fit; only genuinely skyscraper-caliber imported structures retain a whole-building collapse threshold.

## Master 16.177 - 2026-07-22

Five-voice building destruction audio cap.

- Adds one global maximum of five simultaneous building destruction sounds.
- Covers ordinary building consumption, imported-building collapse starts, and individual imported chunk impacts.
- Retains the narrower per-stack cooldown so multiple collapsing buildings cannot each claim their own five-voice allowance.

## Master 16.176 - 2026-07-22

Pack-authored Medieval town and detailed imported-building destruction.

- Rebuilds Medieval Village with 32 authored pack-building parcels and four native Holesy parcels, an approximately 90/10 visual mix.
- Reduces native traffic and loose native scenery in the Medieval recipe so the pack remains visually dominant.
- Replaces solid 3x3 and 4x4 proxy volumes with smaller 5x5 and 6x6 exterior-shell pieces.
- Carries roof, timber frame, wall-tone, stone-tone, and window-dark details into the damaged state.
- Uses measured rendered height and proportions for building classification instead of treating a named bell tower as skyscraper-caliber.
- Corrects the shared size-gate message so ordinary imported buildings and towers are no longer described as skyscrapers.

## Master 16.175 - 2026-07-22

Universal HUD skip for arbitrary presentation waits.

- Adds a compact `<Skip>` HUD action backed by a single reusable skippable-wait contract.
- Makes the 6.5-second wave-contract dock immediately skippable.
- Makes the post-rebuild district briefing delay immediately skippable.
- Makes the five-second consumed-player return delay immediately skippable.
- Clears scheduled callbacks before advancing so clicking Skip cannot double-start a wave or end screen.
- Keeps gameplay-rule clocks authoritative: round and wave timers, Mandates, boss deployment, aid timing, buffs, cooldowns, and real asset loading are not skippable.

## Master 16.174 - 2026-07-22

Temporary city override for directed review.

- Restores a start-menu city selector with Automatic Rotation, Classic City, MegaKit Downtown, and Medieval Village.
- A specific selection remains active for every newly generated city during that run so a reviewer can repeatedly inspect one recipe.
- Labels the control for removal before release.
- Intentionally leaves the market-language release gate failing on the exposed environment selector, preventing accidental publication as a market candidate.

## Master 16.173 - 2026-07-22

Composable city-pack architecture and MegaKit street layer.

- Adds declarative city recipes whose pack list may contain zero, one, or multiple asset packs.
- Defines Classic as a no-pack city, MegaKit Downtown as a two-pack city, and Medieval Village as a one-pack city.
- Adds ten authored MegaKit road markings: four crosswalks, straight and turn arrows, STOP, SLOW, and bike-lane art.
- Adds four authored street drains and four concrete entrance/stair pieces as consumable street-scale assets.
- Stagger-loads imported street assets and cancels obsolete loads when a city is torn down.
- Adds runtime diagnostics for the selected recipe packs and completed MegaKit street-asset count.

## Master 16.172 - 2026-07-22

Third-city Medieval Village rotation.

- Adds Medieval Village to automatic district selection while preserving the no-immediate-repeat rule.
- Converts all ten CC0 Quaternius village buildings from source OBJ/MTL into 4.07 MB of browser-ready GLB runtime assets.
- Places five rotating village landmarks per generation, so successive medieval districts cycle through the complete building roster.
- Adds earth roads, stone edges, barrels, hay bales, carts, and market stalls with Holesy's existing lightweight geometry.
- Preserves intact imported shells until breach, then transfers destruction to solid coarse-block physics proxies.
- Adds invisible runtime diagnostics for selected district and completed medieval-model count.

## Master 16.171 - 2026-07-22

Market-ready district rotation and language cleanup.

- Removed the player-facing environment selector.
- Added automatic eligible-district selection with no immediate environment repeat.
- Replaced development-state language in normal player surfaces with production-ready copy.
- Added `scripts/check-player-facing-language.mjs` as a repeatable release gate.
- Recorded user acceptance of the `Master 16.170` performance pass and the `Master 16.169` MegaKit building/park presentation.

## Master 16.158 - 2026-07-12

Authoritative Mandate completion visuals, giant warning arrow, and readable boss names.

- Recalculates Mandate completion from the target rows every time the HUD updates instead of trusting cached counters.
- Makes early completion immediately and persistently turn the Mandate card light green with dark text.
- Makes late completion immediately replace red card/game borders with exactly four bright green pulses across four seconds.
- Enlarges the center-screen arrow to 360px, explodes it into view, moves and shrinks it to the existing pointer position, then begins five synchronized flashes and sounds.
- Keeps the critical arrow motion active even when the operating system requests reduced motion.
- Triples boss-name label dimensions and doubles their texture resolution for legibility.
- Replaces the plain brick-box conversion with all 18,344 original model triangles, original UVs, and all 13 original materials distributed across 96 solid physical blocks.
- Places a smaller closed visual core inside every block while retaining a full-size box collision proxy, preventing the core from hiding the authentic facade.
- Reduces park-object growth value by 80 percent; park people now award 3 growth points and balls award 4.
- Turns each completed Run Goal green and turns the containing Run Goals panel green when every Goal is complete.

## Master 16.157 - 2026-07-12

Smaller closed MegaKit blocks with skyscraper collapse behavior.

- Re-converts `Building_Small_1` from 12 large blocks into 96 smaller cubes: four columns, four rows, and six floors.
- Corrects triangle winding on every cube face so front-facing geometry points outward.
- Forces every retained kit material to render two-sided as a defensive guarantee against disappearing exposed faces.
- Keeps six closed physical faces, roof material, interior underside, and box collision metadata on every block.
- Uses the skyscraper collapse planner's randomized topple, pancake, split, and twist behavior.

## Master 16.156 - 2026-07-12

Longer centered Mandate and Run Goals briefing.

- Increases the fully centered reading period from 2.8 seconds to 5.5 seconds.
- Preserves the existing one-second docking motion into the HUD.
- Keeps movement, combat, and the wave timer frozen for the complete 6.5-second sequence.

## Master 16.155 - 2026-07-12

First reproducible offline-converted destructible kit building.

- Converts MegaKit `Building_Small_1` into a prebuilt 12-block glTF before the game runs.
- Each physical block has six closed faces, a roof cap, a textured facade, an exposed-interior underside, and box-collision metadata.
- Records the exact source SHA-256, pipeline version, fixed seed, subdivision, dimensions, materials, and physics preset in `recipe.json`.
- Ships a validation record and requires exactly 12 named blocks at runtime.
- Loads only this converted building and duplicates it across five parcels for focused destruction testing.

## Master 16.154 - 2026-07-12

Mandate completion state and staged warning arrow.

- Completed Mandates turn the full card light green with high-contrast dark typography.
- Solving a Mandate during the red deadline warning immediately replaces red with a four-second green card-and-game-border pulse.
- The warning arrow now explodes into the center, travels to its Mandate pointer position, and then flashes five times.
- The five comic alert sounds wait for arrival and synchronize with the arrow flashes.
- Reduced-motion users receive the destination arrow and static success treatment without the travel or pulse animations.

## Master 16.153 - 2026-07-12

Ten animated full-parcel park archetypes.

- Randomly reserves four to six complete parcels per city as parks instead of placing buildings on them.
- Adds playground, basketball, baseball, tennis, running-track, swimming-pool, picnic/BBQ, fountain-garden, dog, and skate parks.
- Gives every park a normal, rundown, or fancy presentation.
- Adds individually consumable turf, fences, courts, equipment, stands, lights, maintenance structures, tables, grills, planters, litter, and valet cars.
- Adds distinct animation for athletes, bouncing balls, cheering fans, swimmers, runners, dogs, smoke, and fountain water.

## Master 16.152 - 2026-07-12

Solid imported building blocks and permanent boss names.

- Replaced hollow clipped MegaKit shells with closed cubic pieces carrying the kit model's mapped materials.
- Added solid top and bottom faces so every imported building has a roof and no visible hollow interior.
- Reserved irregular collapse pieces for skyscrapers unless another building receives an explicit exception.
- Added one permanent name per boss skin above the boss and in its incoming announcement.

## Master 16.151 - 2026-07-12

Debris sleep and starting-hole precision.

- Grounded building pieces now stop all rotation after a short low-speed settling window.
- Tiny contacts no longer repeatedly wake already-settled building debris.
- Starting-hole mouse and touch steering uses a nearer target and softer short-drag response without changing movement speed.

## Master 16.125 - 2026-07-11

Plain-language objectives and expanded street objects.

- Replaced themed Run Goal labels with direct `Eat [count] [object]` instructions.
- Renamed unclear Mandate targets such as `Edge People` to explicit location language such as `People Near Border`.
- Added street kiosks, bollards, concrete planters, wood pallets, shopping carts, alarm boxes, scooters, cafe tables, construction drums, and parcel lockers.

## Master 16.121 - 2026-07-10

Run Goal and Goal Sweep reward pass.

- Added a brief objective-row glow/pop and distinct three-note chime for individual Run Goal completion.
- Strengthened Goal Sweep with the major stinger, player-rim pulse, and short camera kick while preserving its score and speed reward.

## Master 16.120 - 2026-07-10

Immediate game audio and earned Mandate-row rewards.

- Began recorded-sound decoding immediately from the player's start gesture rather than one second after world construction finishes.
- Added a 250-point reward, compact score pop, and crisp two-note tick when an individual Mandate row is completed.

## Master 16.119 - 2026-07-10

Transparent first-run adaptive assistance.

- Added a small red square beside the timer whenever player-performance data triggers an adjustment.
- Added persistent browser logging with date/time, version, adjustment, and reason; clicking the square exports the history as a local `.txt` file.
- Added one bounded rule for testing: after 20 active seconds in Endless Wave 1 with under 250 points and no objective progress, grant a 10% movement boost for 10 seconds.

## Master 16.118 - 2026-07-10

Wave 1 soldier-free Mandate repair.

- Disabled Ultra's unintended Wave 1 soldier override so every difficulty preserves the designed soldier-free opening wave.
- Kept military Mandates gated until a wave can actually supply soldiers.

## Master 16.117 - 2026-07-09

Capacitor native app shell.

- Added a root Capacitor project pointing at `40_RELEASE/Website_Publish_Package/holesy`.
- Installed `@capacitor/core`, `@capacitor/cli`, `@capacitor/ios`, `@capacitor/android`, and `@capacitor/haptics`.
- Generated native `ios/` and `android/` shell projects.
- Added Android `VIBRATE` permission and documented the native app workflow in `00_ADMIN/Requirements/NATIVE_APP_CAPACITOR_WORKFLOW.md`.

## Master 16.116 - 2026-07-09

iOS web haptic fallback attempt.

- Added a last-resort iOS WebKit switch-control haptic fallback after native haptics and before browser vibration reports unsupported.
- Changed the Haptics test result to report `iOS Tick` when this fallback is triggered.
- Preserved the native haptics bridge as the required commercial iPhone haptics path.

## Master 16.115 - 2026-07-09

Native haptics bridge.

- Added a native haptics bridge path so `triggerHaptic()` tries Capacitor Haptics before falling back to browser vibration.
- Mapped existing gameplay haptic events to native impact, notification, or vibrate calls without changing each gameplay call site.
- Added support for an optional `window.HolesyNativeHaptics` custom bridge for a non-Capacitor native wrapper.
- Documented the native haptics bridge contract in `00_ADMIN/Requirements/NATIVE_HAPTICS_BRIDGE.md`.

## Master 16.114 - 2026-07-09

Unsupported haptics clarity.

- Changed the unsupported mobile haptics result from `No API` to `No Haptics`.
- Updated the status text to explain that browsers without `navigator.vibrate` cannot make the device vibrate.
- Preserved `latest.html` as the stable mobile test entry point.

## Master 16.113 - 2026-07-09

Stable latest URL and stronger haptic diagnostics.

- Added `latest.html` as a stable cache-refresh launcher that reads `js/build-info.js` and opens the current build automatically.
- Strengthened the explicit Haptics test pulse and made the button itself report `Sent`, `No API`, `Blocked`, or `Failed`.
- Increased ordinary devour and heavy-object vibration durations so supported Android browsers should feel gameplay haptic events more clearly.
- Documented that if the button reports `Sent` but no vibration is felt, the browser/device is likely silently ignoring the Vibration API.

## Master 16.112 - 2026-07-09

Mobile haptics test and mute placement repair.

- Made the Haptics menu test respond through click, pointer, and touch activation instead of relying on desktop-style click only.
- Added visible status for haptic test outcomes: sent, blocked, unsupported, cooldown, or failed.
- Bypassed the haptic cooldown for the explicit test button so a tap always attempts the diagnostic pulse.
- Moved the Music mute control to the bottom-left mobile gameplay control stack just above `HUD+`, while keeping it in the menu corner outside active play.

## Master 16.111 - 2026-07-09

Randomized Mandate variety.

- Randomized Mandates instead of repeating the same fixed People/Props/Cars/Offices/Towers contract every wave.
- Added a larger Mandate target pool across people variants, street props, trees, cars, buildings, rival holes, soldiers, military units, and boss units.
- Ramped active Mandate row count by wave tier: early waves ask for one target type, later waves randomly grow toward the five-row maximum.
- Randomized required counts inside supply-capped limits so scarcity-sensitive targets such as moving cars, soldiers, and bosses stay achievable when selected.

## Master 16.110 - 2026-07-09

Mobile playfield HUD and haptic diagnostics.

- Added a compact mobile HUD default that keeps the playfield clear by showing only tier, timer, and Mandate summary during active play.
- Added a mobile `HUD+` / `HUD-` toggle so Run Goals, Mandate details, and other dense HUD information are available without permanently covering the board.
- Added a Haptics menu test/status control and loosened vibration dispatch to use `navigator.vibrate` whenever the browser exposes it.
- Documented that unsupported mobile browsers or non-secure LAN URLs may still block vibration even when the game sends haptic events.

## Master 16.109 - 2026-07-06

Mandate fairness and scaling repair.

- Retuned Mandate counts away from high first-wave category-inventory percentages so people and car requirements are achievable with focused play.
- Added a clearer per-wave count curve with actual-supply caps so Mandates scale upward without over-demanding scarce categories on harder difficulties.
- Kept Mandates mandatory while making wave 1 a fair survival contract instead of a near-total category sweep.

## Master 16.108 - 2026-07-06

Mobile haptic feedback pass.

- Added mobile vibration haptic pings for every player object devour, with heavier pulses for building-like object consumption.
- Added distinct haptic patterns for Mandate completion/failure, Run Goal completion, Goal Sweep, powerups, wave starts/transitions, boss inbound/defeated, unit clears, rival devours, and player damage/death.
- Guarded haptics with `navigator.vibrate` feature detection and cooldowns so unsupported browsers safely no-op and dense consumption does not over-vibrate.

## Master 16.107 - 2026-07-06

Mandatory Mandates and wave-long reward.

- Made wave-based Mandates truly mandatory: if the wave timer expires before every Mandate row is complete, the run ends as `Mandate Failed`.
- Completing all Mandate rows now grants the score bonus plus a Mandate Surge speed/protection reward that lasts until the wave ends.
- Updated player-facing guidance so Mandates are the required survival contract and Run Goals remain optional reward goals.

## Master 16.106 - 2026-07-06

Endless flagship and boss pressure.

- Made Endless Waves the default selected flagship mode on the title screen.
- Raised true-boss eat requirements and added extra resistance when the player is under a speed boost.
- Added close-range boss damage scaling so getting greedy under a boss is dangerous.
- Added boss kiting while firing so bosses backpedal and strafe to stay in shooting range instead of waiting to be swallowed.

## Master 16.105 - 2026-06-30

Building weight and medium-voxel fall repair.

- Increased building debris gravity and terminal fall speed so objects read heavier during collapse.
- Reduced medium-building voxel upward hop, release delay, teeter time, and ground bounce.
- Prevented the missed-hole medium-voxel settle path from snapping a cube to the ground until the cube has actually reached ground height.
- Mirrored the updated runtime package to the release folder for PC/mobile parity.

## Master 16.104 - 2026-06-30

Mobile mode picker button repair.

- Changed the four visible game-mode choices from clickable cards into real `button` controls.
- Added pressed-state updates so mobile taps, keyboard activation, and accessibility state all select the same mode.
- Preserved the existing title-screen layout and styling while making the game-mode choices behave like actual buttons.

## Master 16.103 - 2026-06-26

Mandate pressure tuning.

- Raised Mandate counts from small fixed targets to pressure targets based on a large share of the live district inventory.
- Added wave-based Mandate pressure growth so later waves demand more of each listed category.
- Preserved the count-based Mandate HUD and readable font sizing from `Master 16.101` and `Master 16.102`.

## Master 16.102 - 2026-06-26

Mandate font readability repair.

- Increased Mandate row labels and progress counts to match the Run Goals row font scale.
- Widened the Mandate panel slightly so the larger count text remains readable without crowding.
- Preserved the `Master 16.101` count-based Mandate behavior.

## Master 16.101 - 2026-06-26

Count-based Mandate clarity.

- Changed Mandates from hidden exact-object targets into explicit category counts.
- The Mandate panel now shows rows such as `Eat People 0/8`, `Eat Props 0/7`, `Eat Cars 0/2`, `Eat Offices 0/2`, and `Eat Towers 0/1`.
- Any matching object in the listed category advances the row; order does not matter.
- Moved the Mandate panel lower in the right HUD stack so the title and instructions stay readable under the timer/control cluster.

## Master 16.99 - 2026-06-25

Devour Mandate target system.

- Added a five-target Devour Mandate selected from the freshly populated district: one person, one prop, one car, one mid building, and one skyscraper.
- Added a centered Mandate HUD panel with five dots, remaining-count text, amber collection state, and red warning pulse when two or more targets remain under 45 seconds.
- Awarded a +2,500 score bonus and `MANDATE COMPLETE` feedback when all targets are consumed by the player.
- Added incomplete-Mandate failure feedback on round end while leaving Locator Pulse, city transition, failure modal, and settings preferences to later PBIs.

## Master 16.98 - 2026-06-24

Nonlinear offensive pressure and stats panel.

- Replaced the straight offensive-unit wave ramp with a stronger nonlinear wave-pressure curve.
- Kept soldiers slower while letting vehicles and boss-derived units reach higher speed and damage caps as waves climb.
- Moved Game Stats into an in-page modal so the Stats button works without popup permissions and refreshes after recorded runs.

## Master 16.97 - 2026-06-24

Rival score persistence and offensive-unit escalation.

- Removed the Endless rival respawn score penalty so player and rival scores never go down mid-run.
- Added per-run randomized rival AI difficulty so the three rival holes vary by game, not by wave.
- Retuned offensive units so soldiers are slowest, vehicles and boss-derived units move faster, and speed/damage increase as wave number rises.
- Added rival collapse-focus behavior so AI holes remember buildings they just demolished and stay near the rubble long enough to collect pieces.

## Master 16.96 - 2026-06-24

Boss swallow visibility and boss spotlight.

- Keeps consumed soldiers and boss-derived offensive units visible as they fall into the hole instead of disappearing on first contact.
- Makes true fifth-wave boss forms 40% larger than their later random-drop versions.
- Adds a red neon boss glow to the fifth-wave boss form so players can immediately identify the featured boss archetype.

## Master 16.95 - 2026-06-24

Ten-boss roster and reset cadence repair.

- Expanded the boss roster to ten archetypes: Siege Tank, Twin-Gun Mech, Shield Commander, Mortar Carrier, Rail Sniper, Drone Marshal, Grenade Captain, Flame Rig, Railgun Tripod, and Shock Bruiser.
- Added distinct visual variants and combat tuning for the seven new bosses, including splash rounds, long-range heavy shots, fast laser pressure, short-range flame pressure, charged rail shots, and close-range shock pulses.
- Corrected Endless world-shift resizing so each block has five waves of growth, the boss appears on the fifth wave, and the hole reset happens on the sixth wave.

## Master 16.94 - 2026-06-24

Boss roster and offensive drops.

- Removed the temporary tank-every-level testing flag.
- Added three distinct boss archetypes: Siege Tank with slow cannon fire, Twin-Gun Mech with dual heavy machine guns, and Shield Commander with heavy bursts and armor.
- Every fifth Endless wave now drops one randomly selected boss by itself.
- Each revealed boss archetype becomes a later-wave random plane-drop option at half boss damage, while Wave 1 stays soldier-free and Waves 2 through 4 remain soldier-led.
- Updated the How to Play build label from stale `Master 16.55` to `Master 16.94`.

## Master 16.93 - 2026-06-23

Skyscraper outward debris repair.

- Corrects skyscraper collapse direction so chunks launch from the building center toward the hit/source side.
- Removes the reversed inward shove that made large debris appear to arc out and then return toward the footprint.
- Keeps medium-office voxel containment, existing lateral scatter, and temporary tank-testing visibility intact.

## Master 16.92 - 2026-06-23

Randomized wave start corners.

- Randomizes each alive hole's corner assignment at the start of every wave.
- Avoids placing a hole back into its previous wave-start corner when another corner is available.
- Preserves score, radius, wave-state reset, and temporary `Master 16.91` tank-testing behavior.

## Master 16.91 - 2026-06-23

Temporary tank testing visibility.

- Temporarily forces the green tank army boss into every playable mode and wave so player testing can see it immediately.
- Shortens the first testing deployment delay while the temporary tank test flag is enabled.
- Keeps the `Master 16.90` tank model, push behavior, and building-pressure collapse behavior intact.
- This change is intentionally temporary and should be removed after tank validation.

## Master 16.90 - 2026-06-23

Green tank army boss.

- Replaced the red command-unit boss body with a green tank model sized at roughly twice a car footprint.
- The tank keeps the army boss spawn cadence and three-times soldier bullet damage, while driving across road and off-road terrain toward holes.
- Added tank contact behavior that pushes loose props, people, trees, and cars.
- Added tank pressure against building pieces so deeper contact activates progressive collapse through the existing voxel, skyscraper, and government-building physics paths.

## Master 16.89 - 2026-06-23

Wave transition boss-trigger fix.

- Fixed the Endless Wave 1 to Wave 2 lock caused by the new army boss late-mode trigger referencing a non-existent `timedMode` flag.
- Replaced that reference with the existing selected-mode state check so Wave 2 soldier deployment can run normally.
- Preserved the `Master 16.88` army boss behavior and refreshed player-test cache labels to `Master 16.89`.

## Master 16.88 - 2026-06-23

Army boss escalation.

- Added a red army boss command unit that is four times larger than regular soldiers.
- Boss bullets deal three times normal soldier bullet damage while preserving the existing soldier fire cadence.
- Bosses appear every fifth Endless wave, late in Timed rounds, late in regular Waves, and during Last Man Standing endgames.
- Preserved boss identity through plane deployment, parachute landing, soldier AI, devouring reward, and Endless save/load restore paths.

## Master 16.87 - 2026-06-18

Comprehensive runtime performance pass.

- Removed per-frame vortex geometry replacement and reused stable arc geometry through transforms.
- Replaced global window/streetlight scans with direct mesh registries and active-flicker sets.
- Combined duplicate object-to-hole scans and removed allocation-heavy dimension checks.
- Staggered inactive building trigger checks and removed idle government-physics/contact work.
- Reused box materials and geometries across district objects, with geometry disposal on world rebuild.
- Deferred embedded sample decoding and reverb construction until after input handling and world startup.
- Reduced gameplay-overlay blur, capped expensive DOM updates, lowered default render costs, and added sustained-slow-frame resolution/shadow fallback.

## Master 16.86 - 2026-06-18

Startup and frame-pacing repair.

- Removed the duplicate module-load city build that delayed mode-selection interaction.
- Split city population into animation-frame batches so Begin and wave rebuilds do not monopolize the main thread.
- Limited HUD/live-score DOM rebuilding to 10 Hz while leaving simulation and rendering uncapped.

## Master 16.85 - 2026-06-18

Run goal instruction tooltips.

- Added a readable dark-background tooltip to every displayed Run Goal.
- Hovering a goal title explains the exact object family, required count, and score reward.
- Keyboard focus exposes the same instructions for accessibility.
- Kept the implementation CSS-driven to preserve the `Master 16.84` performance hotfix.

## Master 16.84 - 2026-06-15

Run goal performance hotfix.

- Batches object-family mastery saves so the game no longer writes local storage during every bite.
- Limits per-object Run Goals bookkeeping to the families currently shown in the active goals.
- Preserves the larger randomized goals and Goal Sweep reward from `Master 16.83`.

## Master 16.83 - 2026-06-15

Run goal tuning and performance repair.

- Replaced repeated Run Goals HUD rewrites with dirty-flagged updates to remove the movement stutter introduced by `Master 16.82`.
- Expanded the run-goal pool to 50 larger predefined goals and randomly selects three different object families per goal set.
- Added the visible Goal Sweep reward for completing all displayed goals: bonus score plus a short speed surge.
- Endless refreshes run goals every five waves; timed and standard wave runs receive a set at run start.

## Master 16.82 - 2026-06-15

Run goals and object-family mastery.

- Added a Run Goals HUD panel with three active objectives per run.
- Added local object-family mastery feedback for people, vehicles, props, trees, buildings, soldiers, and MegaKit manholes.
- Completing a run goal grants immediate score, while mastery remains feedback-only so future defense scaling can be designed before any persistent growth loop.

## Master 16.81 - 2026-06-15

MegaKit detail visibility repair.

- Widened and brightened MegaKit Downtown block-edge and sidewalk trim so it reads from the normal gameplay camera.
- Enlarged MegaKit road manholes and added brighter metal rings and surface bars so they are visible during evening and night lighting.
- Kept the detail non-colliding and below the hole render layer so holes remain visually authoritative.

## Master 16.80 - 2026-06-14

MegaKit readable ground detail.

- Added thin non-colliding block-edge and sidewalk trim to make the MegaKit Downtown test environment read as a city grid without adding fake terrain.
- Replaced tiny box manholes with larger circular road manholes that are normal consumable props.
- Kept themed buildings out of MegaKit Downtown until future theme architecture routes them through validated breakable/destruction systems.

## Master 16.79 - 2026-06-13

MegaKit environment safety repair.

- Removed MegaKit visual-only road, sidewalk, and ground patch overlays so holes cannot appear under fake terrain.
- Removed MegaKit showcase buildings because they did not use the validated breakable building/object destruction paths.
- Kept MegaKit Downtown as a small-props-only test until future themed buildings are rebuilt through validated voxel/destruction systems.

## Master 16.78 - 2026-06-13

MegaKit Downtown test environment.

- Added a title-screen Environment selector while keeping Classic Aldine as the default.
- Added a MegaKit Downtown test district that uses imported CC0 Downtown City MegaKit textures on Holesy-authored roads, props, and showcase buildings.
- MegaKit-flavored roads, sidewalks, props, and showcase buildings render as an isolated environment test while Holesy collision and destruction physics remain on the validated baseline.

## Master 16.77 - 2026-06-12

Government and house voxel repair.

- Government building pieces now use the same voxel-stack collapse path as medium buildings while keeping their government visual style.
- Small house/shop buildings now wake the whole compact stack on first contact so breakup is visible immediately.
- Government voxel pieces restore through the voxel save/load path instead of the older separate government physics world.

## Master 16.76 - 2026-06-12

Government and house debris breakup.

- Government-building blast separation now becomes the staged physics base so pieces do not ease back toward the original grid before release.
- Small house/shop buildings now break into compact voxel chunks instead of being swallowed as one block.
- Endless save/load preserves restored small-building chunks as small debris pieces.

## Master 16.75 - 2026-06-12

Natural skyscraper debris spread.

- Removed the artificial inward spread limiter from non-voxel skyscraper chunks so debris no longer nudges back toward the original building center.
- Preserved the existing medium-office voxel spread guard and behavior.

## Master 16.74 - 2026-06-11

PERF-012 modular package completion.

- Added a governed package manifest to the source and release `holesy/` folders so upload checks enumerate the full modular package instead of treating `index.html` as the game.
- Refreshed the release package README to `Master 16.74` and documented the completed modular source/release baseline.
- No gameplay behavior changed from `Master 16.73`.

## Master 16.73 - 2026-06-11

Short building-window power flicker.

- Building windows now flicker only one to three short random times after power is cut, then stay dark.
- Time-of-day cycling still cannot relight windows after their building has lost power.

## Master 16.72 - 2026-06-11

Building window power-off flicker.

- Lit building windows now flicker briefly before going dark as the building starts coming apart.
- Time-of-day cycling still cannot relight windows after their building has lost power.

## Master 16.71 - 2026-06-11

Local test cache refresh.

- Bumped the local test build label and asset version so Chrome reloads the repaired Time button handler instead of reusing an older wave-lock module.
- No gameplay rule changed from Master 16.70: the Time button should cycle looks manually, while devoured streetlamps flicker off as they fall.

## Master 16.70 - 2026-06-11

Streetlamp power-off polish.

- Devoured streetlamps now flicker out as they fall into the hole.
- Time-of-day lighting no longer relights a streetlamp after it has been disconnected from power.

## Master 16.69 - 2026-06-11

Time button cycle repair.

- The Time button once again cycles morning, mid day, evening, and night when clicked.
- Wave-based modes still assign the starting look for each wave; manual Time cycling remains available for further visual testing during a wave.

## Master 16.68 - 2026-06-09

Wave time-of-day sequence lock.

- Wave-based modes now lock each wave to the fixed morning, mid day, evening, night sequence.
- Endless waves repeat the sequence after night so wave 5 returns to morning.
- The Time button now snaps back to the active wave time during wave-based play instead of overriding the wave look.

## Master 16.67 - 2026-06-09

Destroyed building lights cleanup.

- Lit building windows now extinguish when their building piece starts collapsing, falling, or being removed.
- Skyscraper, voxel-office, and government-building collapse activation now turns off affected building windows immediately.
- The Time button no longer relights windows on buildings that have already been destroyed.

## Master 16.66 - 2026-06-09

Evening and night city lights.

- Evening and night now turn on street lamp glow, sparse lit building windows, and car headlights/taillights.
- Less than half of registered building windows are selected to light up, keeping the city readable without making every facade bright.
- Lighting is tied to the existing Time button and uses cheap mesh/material visibility changes instead of expensive dynamic light spam.

## Master 16.65 - 2026-06-09

Government debris physics visibility.

- Government building pieces now get a short debris-escape window after first breach so shake, topple, bounce, and collision impulses are visible before the hole can swallow them.
- Strengthened the government physics staged activation with larger shake, lean, blast separation, and topple-release impulses.
- Improved government cube side-impact reactions with extra scatter, hop, and angular torque while preserving the separate government physics path.

## Master 16.64 - 2026-06-09

Weather performance rollback.

- Removed the in-game Weather button and weather particle system after the visual weather pass caused severe runtime slowdown.
- Kept the Time button and morning, mid day, evening, and night lighting cycle because it does not run a per-frame particle field.
- Removed the weather frame-update path so gameplay no longer pays weather costs in the main loop.

## Master 16.63 - 2026-06-09

Government building impact physics.

- Government building breaches now start with a visible shake-and-lean phase before pieces release.
- Touched columns receive stronger upward/outward impulses so pieces can jump out and scatter instead of only dropping into the hole.
- Government building cube collisions now add stronger bounce, side-impact hop, and angular spin while staying inside the separate government physics path.

## Master 16.62 - 2026-06-09

Weather cycle cleanup.

- Removed the Ash weather effect from the in-game Weather cycle.
- Restored the richer Rain and Snow particle behavior from the time/weather preview pass.
- Kept Clear, Rain, and Snow as the available weather preview effects.

## Master 16.61 - 2026-06-09

Time and weather preview controls.

- Added in-game Time and Weather buttons for cycling visual looks during play.
- Added Clear, Rain, Snow, and Ash weather looks with lightweight scene particles.
- Weather now adjusts fog, sky tint, ambient light, sun intensity, and ground tint without changing gameplay.

## Master 16.60 - 2026-06-09

Time-of-day lighting pass.

- Added distinct morning, mid day, evening, and night scene looks.
- Wave-based modes now rotate sky, fog, ambient light, sun angle, and ground tint by wave.
- Timed and Last Man Standing modes keep the readable mid day look as their stable baseline.

## Master 16.59 - 2026-06-07

Government building column-shock collapse.

- Government building breaches now identify the touched column and give it the strongest upward/outward shock.
- Neighboring government-building cubes receive softer randomized impulses so collisions create different collapse patterns each time.
- Preserves the separate government-building physics path while making its collapse effect the preferred reference for future building work.

## Master 16.58 - 2026-06-07

Government building touch-crash fix.

- Fixed the first-contact government building activation crash caused by calling a non-existent impact-sound helper.
- Government building breach feedback now uses the existing budgeted voxel impact sound gateway.
- Preserves the separate government-building physics and spy/tuxedo visual treatment.

## Master 16.57 - 2026-06-07

Government building visibility pass.

- Changed government buildings from civic gray to a high-contrast spy/tuxedo palette so they are immediately distinguishable from offices and skyscrapers.
- Added black, white, charcoal, and silver facade details, including shirt-and-bowtie-style front pieces and bright roof striping.
- Saved government building pieces now restore with the same distinct visual identity.

## Master 16.56 - 2026-06-06

Government building physics prototype.

- Added a distinct government building that uses a separate fixed-step physics world instead of the existing building-collapse rules.
- Government building pieces use collider separation, impulse response, gravity, friction, bounce, and sleep behavior for more natural object reactions.
- The prototype appears once per rebuilt city and preserves its state through Endless save/load.

## Master 16.52 - 2026-06-05

Medium office impact jarring.

- Medium-office impacts now jolt nearby cubes out of their perfect grid before the falling column releases.
- Jarred active cubes inherit small offset, lift, spin, and lateral impulse so each strike starts from a less uniform state.
- Settled medium-office cubes remain solid collision participants, reducing graphical overlap and making debris piles push apart more naturally.

## Master 16.51 - 2026-06-02

Medium office pool-break physics.

- Medium-office cubes now fan outward with more varied first-impact vectors instead of falling in one tidy column.
- Cube-to-cube contact transfers more force, sideways scatter, and spin so collapses read more like a pool break.
- Ground bounces and local spread are stronger but still capped so office debris stays near the building and performance remains bounded.

## Master 16.50 - 2026-06-02

Medium office impact kick.

- Medium-office cube columns now get a brief upward hop and outward shove when first impacted.
- Released cubes inherit a small upward/outward velocity so collapse motion reads more chaotic and reactive.
- The effect is intentionally smaller than skyscraper collapse and does not change scoring, growth, or save/load rules.

## Master 16.49 - 2026-06-02

Restore readable classic hole.

- Removed the failed 3D/depth hole center experiments because they blocked visibility of objects falling into the mouth.
- Restored the original readable hole treatment: a flat black circular mouth with the existing colored border.
- Keeps the `Master 16.40` through `Master 16.45` falling-object behavior intact while deferring hole-depth visuals for a cleaner future approach.

## Master 16.48 - 2026-06-02

Deep shaft hole visual.

- Replaced the still-flat abyss texture attempt with a tapering dark shaft whose bottom is physically lower and smaller than the mouth.
- Added a stronger vertical wall texture and deeper nested rings to make the hole read more like a descending shaft.
- Visual-only: devour, scoring, growth, and hole-mouth clipping behavior are unchanged.

## Master 16.47 - 2026-06-02

Abyss-style hole depth illusion.

- Removed the gray recessed-well geometry from `Master 16.46` because it did not read as real depth during play.
- Changed the hole center to a dark abyss texture with asymmetric inner shading and a black center.
- Added very subtle animated interior bands to suggest depth inside the mouth without changing gameplay.

## Master 16.46 - 2026-06-02

3D hole well visual.

- The hole center now renders as a recessed well instead of a flat black disc.
- Added a sloped dark inner wall, lower depth surface, and subtle animated interior bands.
- This is a visual-only upgrade; scoring, devouring, growth, and hole-mouth clipping behavior are unchanged.

## Master 16.45 - 2026-06-02

Screen-space hole-mouth clipping.

- Falling objects now use the projected visible mouth of the hole, not only a ground-space radius check, to decide whether they can render as descending.
- Medium-office cubes that visually leave the black hole mouth now stop their hole-descent behavior and settle instead of continuing to fall on the street.
- This directly targets the outside-hole falling artifact still visible after `Master 16.44`.

## Master 16.44 - 2026-06-02

Voxel hole-miss cleanup.

- Medium-office cubes that begin falling but miss the live hole now settle as ordinary ground debris.
- Active voxel physics now checks the same visible-mouth boundary as formal swallowed-object descent.
- Preserves the visible well-depth effect for objects actually inside the hole while preventing orphaned cube falls outside it.

## Master 16.43 - 2026-06-02

Hole-mouth visibility mask.

- Swallowed objects still fall from the fixed mouth-entry point, but only render while that descent column is inside the visible hole.
- Already-swallowed objects continue their descent invisibly when the hole moves away instead of falling through normal street.
- The visible well-depth behavior from `Master 16.42` remains intact while preventing outside-hole falling artifacts.

## Master 16.42 - 2026-06-02

Visible well-depth hole descent.

- Swallowed objects now render above the black hole surface while descending so they do not vanish at an invisible mouth barrier.
- Objects continue falling from their fixed entry point with a subtle screen-down drift that reads as depth inside the well.
- Save/load preserves the fixed descent direction for objects already falling into a hole.

## Master 16.41 - 2026-06-02

Fixed-point vertical hole descent.

- Objects now keep falling at the exact world-space mouth contact point instead of drifting inward after entry.
- Objects remain visible longer and are removed much deeper below the hole mouth.
- Falling-object save/load now preserves the fixed entry point without carrying an obsolete lower-mouth target.

## Master 16.40 - 2026-06-02

Same-side hole descent paths.

- Objects now keep the mouth contact point where they enter the hole instead of snapping toward a center drain.
- Falling objects drift toward a same-side lower-mouth point as they descend, making them read as dropping down into the hole.
- Save/load now preserves active falling-object entry and lower-mouth targets.

## Master 16.39 - 2026-06-01

Startup blocker repair.

- Corrected the `PERF-012` extraction boundary so renderer setup remains in `js/main.js`.
- Restored game-mode selection and Begin button behavior after the `Master 16.38` module-startup blocker.
- Kept build metadata, patch notes, difficulty profiles, and Archive lore data in separate modules.

## Master 16.38 - 2026-06-01

Modular data extraction.

- Moved build metadata and player-facing patch notes into `js/build-info.js`.
- Moved difficulty profiles into `js/difficulty-profiles.js`.
- Moved Archive lore documents and starter Field Pattern unlocks into `data/lore-documents.js`.

## Master 16.37 - 2026-05-30

Save repair and collapse tuning.

- Endless saves now use compact object records so voxel-heavy boards do not exceed browser storage as easily.
- Skyscraper collapse and chunk audio now uses the same short sparse sound budget as medium-office voxels.
- Medium-office cubes drop faster, release lower floors sooner, and pick up more sideways motion so debris piles spread beyond the original footprint.

## Master 16.36 - 2026-05-30

Cleaner office-cube falls.

- Medium-office voxel cubes no longer shrink during hole-entry falling, so roof blocks keep their full physical size.
- Voxel cube scoring and column activation no longer trigger full building-collapse audio.
- Medium-office cube audio now allows only one short impact voice per building stack every 650ms to avoid layered static.

## Master 16.35 - 2026-05-30

Voxel performance guardrails.

- Made medium-office cubes larger and reduced generated cube counts per building to address collapse performance.
- Replaced unbounded active voxel contact checks with a capped per-building contact budget.
- Added voxel velocity/spin clamps and softer contact impulses so cubes settle more naturally instead of launching or jittering.

## Master 16.34 - 2026-05-30

Slower voxel falls and stronger cube contact.

- Slowed medium-office cube descent by lowering voxel gravity, lowering terminal velocity, and adding slower swallow gravity for voxel cubes.
- Column teetering now commits into a slow side-fall instead of sometimes holding a permanent leaning pose.
- Cubes remember whether they crossed the floor inside the hole, so mobile movement cannot make already-eaten cubes reappear.
- Strengthened cube-to-cube contact impulses so falling cubes bounce and shove each other more visibly.

## Master 16.33 - 2026-05-29

Voxel cube audio and missed-hole cleanup.

- Replaced full building-demolition spam from medium-office cubes with short budgeted cube-impact sounds.
- Limited simultaneous voxel cube impact sounds per building so collapsing offices stay crunchy instead of turning into static.
- Voxel cubes that miss the hole after the player moves away now land as visible debris instead of disappearing into the ground.

## Master 16.32 - 2026-05-29

Slower office-cube gravity and column teetering.

- Added dedicated configurable voxel gravity so medium-office cubes fall slower than skyscraper chunks.
- Medium-office cube columns can teeter/lean before releasing, and upper cubes inherit sideways motion from the lean.
- Grounded voxel cubes keep full size and settle as debris when they miss the hole instead of visually vanishing.

## Master 16.31 - 2026-05-29

Support-gated voxel falling and oversized object jams.

- Medium-office upper cubes now wait for support to fail before falling and accelerate under gravity.
- Active medium-office cubes now push apart in 3D so falling pieces collide instead of visually overlapping.
- Oversized objects can jam in a hole, get dragged, and require enough smaller-object impacts to knock loose.

## Master 16.30 - 2026-05-28

Procedural office voxels and visible-hole falling.

- Made medium-office voxel cubes 25% larger so individual pieces read better during collapse.
- Procedurally varies medium office length, width, and height from 5 to 10 cubes per axis.
- Changed falling-object drift to preserve the visible-hole entry point instead of snapping everything toward a tiny center drain.

## Master 16.29 - 2026-05-28

Medium office building voxel collapse.

- Rebuilt medium office buildings from aligned cube floors instead of one giant consumable block.
- Made only the columns above the hole drop first, so moving under the footprint peels the building apart column by column.
- Saved and restored medium-building cube state for Endless saves instead of rebuilding offices as generic blocks.

## Master 16.28 - 2026-05-22

Traffic orientation and soldier-damage growth repair.

- Re-aligned active traffic to its lane every frame so cars cannot keep driving while visually stuck sideways.
- Removed the collision yaw drift that could make normal driving look like an endless skid.
- Changed soldier damage to shrink the current hole radius without creating hidden score debt, so later devouring grows normally.

## Master 16.27 - 2026-05-21

Mobile menu actions and post-soldier growth repair.

- Bound Stats, Archive, How to Play, and Load Endless through the same touch-safe menu-action button handler.
- Made the How to Play control use the same explicit menu-action button styling as the neighboring menu controls.
- Changed soldier damage from hidden negative-growth debt to a lowered growth baseline, so devouring objects visibly grows the hole after being shot.

## Master 16.26 - 2026-05-21

How to Play graphic restoration and hard traffic skid cap.

- Restored the full-height How to Play summary graphic from the user-approved layout instead of the cropped/rebuilt short asset.
- Added a hard movement cap for panic-car crash slides so a car cannot skid multiple city blocks or across the city after a bad frame.
- Reduced panic-car crash duration, slide distance, and initial loss-of-control velocity.

## Master 16.25 - 2026-05-21

How to Play upload dependency fix.

- Made all menu action controls use the same button styling path.
- Included the How to Play summary image in the GoDaddy delta package because the live site was missing that dependency.
- Clarified that delta packages may include unchanged dependencies when live upload testing proves the server is missing them.

## Master 16.24 - 2026-05-21

Project artifact and GoDaddy delta packaging cleanup.

- Removed the Archive Map spider diagram from the player-facing How to Play popup.
- Preserved the archive-to-achievement spider diagram as a governed project artifact.
- Established changed-files-only GoDaddy delta upload packages as the default manual publish artifact when live is already on the previous master.

## Master 16.5 - 2026-05-19

Grounded skyscraper debris settle fix.

- Stopped fallen skyscraper chunks from spinning in place after their position has settled.
- Added stronger ground friction for all rotation axes on collapsed building debris.
- Added a grounded idle cutoff so low-motion chunks snap fully to rest.

## Master 16.4 - 2026-05-19

Buff cooldown and stronger skyscraper collapse variety.

- Added a 5-second reacquire cooldown after timed lore buffs expire.
- Prevented expired buffs from immediately retriggering the same effect during their cooldown window.
- Gave skyscrapers distinct collapse styles: toppling, pancake drop, split shear, and twisting failure.
- Added staggered floor collapse timing so buildings fail differently instead of all bursting at once.

## Master 16.3 - 2026-05-19

Replay flow blocker patch.

- Fixed the post-game `Begin` button so it starts the selected mode instead of rebuilding the town behind the score screen.
- Reset stale end-screen, input, and transient round state before each new run begins.
- Guarded the `Begin` button against duplicate pointer, touch, and click activations from a single press.

## Master 16.2 - 2026-05-19

Skyscraper collapse variation and shorter panic-car skids.

- Shortened panic-car skid duration and stopping distance.
- Changed skyscraper collapse from radial burst to impact-side directional failure.
- Added contiguous floor-band shearing so debris falls in related but varied directions.
- Capped collapse spread so chunks stay near the building footprint instead of exploding outward.

## Master 16.1 - 2026-05-18

Mobile readability and end-of-round reward patch.

- Compressed and scroll-enabled the mobile mode-selection screen.
- Made the Found Documents archive scroll as one mobile page instead of letting the reader block most of the screen.
- Restricted found-document drops to live player wins only, with at most one document roll per completed run.
- Cleared stale pending document drops when a new run begins.
- Capped runaway car skids with distance, speed, and friction limits.

## Master 16 - 2026-05-13

Production promotion candidate for mobile testing.

- Added clickable in-game patch notes from the bottom-left build badge.
- Promoted the validated lore, Archive, achievement-buff, rare-document-drop, aid-drop, and end-screen cleanup lineage.
- Bundled the website package as `40_RELEASE/Website_Publish_Package/holesy/index.html`.

## Master 15.45 - 2026-05-13

Build notes candidate.

- Added the `Recovered Build Notes` modal.
- Made the version badge keyboard and pointer accessible.

## Master 15.44 - 2026-05-13

End-screen and feedback cleanup.

- Replaced the duplicate post-game mode-selection path with the normal `Begin` flow.
- Suppressed skyscraper size warnings unless the building overlaps the player hole.
- Removed traffic-crash text while preserving crash behavior, smoke, fire, and audio.

## Master 15.43 - 2026-05-13

Starter Field Patterns.

- Added always-visible Archive guidance for the three starter buff patterns.
- Used lore-aligned wording while keeping trigger and effect text clear.

## Master 15.42 - 2026-05-13

Buff clarity and document pacing.

- Extended buff messaging and active-effect descriptions.
- Made found documents rare and capped at one per round.

## Master 15.41 - 2026-05-13

Archive music.

- Added a separate found-document music bed with whimsical conspiracy undertones.

## Master 15.40 - 2026-05-13

Lore archive and achievement buffs.

- Added the Archive, found-document unlocks, and initial lore corpus integration.
- Added lore-triggered achievement buffs and player-facing feedback.

## Master 15.39 - 2026-05-13

Idle lifecycle cleanup.

- Improved pause, visibility, unload, and animation cleanup to reduce long-idle browser risk.
# Master 16.164

- Moves the converted MegaKit building to immutable asset path `v2.3.0`, preventing GitHub Pages and browser caches from retaining an older brown-lattice model under a reused filename.
- Gives each imported test building exclusive ownership of its parcel by removing prior park, pool, fixture, and building objects before placement.
# Master 16.165

- Regenerates the immutable MegaKit model with texture URLs relative to its versioned directory.
- Restores the kit's authentic brick, windows, doors, trim, roof, concrete, and interior materials on GitHub Pages.
# Master 16.167

- Converts the remaining MegaKit medium and large building models into the same 96-solid-piece destruction format.
- Cycles small, medium, and large authored buildings across five test parcels and turns each building front toward the player spawn.
# Master 16.139

- Loads one authentic MegaKit small-building model five times in MegaKit Downtown for an unmistakable in-game asset evaluation.
- Enlarges the Mandate deadline arrow and synchronizes a short comic wobble-horn alert with each of its five flashes.
# Master 16.140

- Corrects the MegaKit runtime asset base so models and textures load from the deployed `/Holesy/assets/` package on GitHub Pages.
# Master 16.141

- Doubles the Mandate deadline arrow to 184px and slows its five flashes from 3.2 seconds to 5.5 seconds.
- Resynchronizes the comic alert so one sound plays with each slower flash.
# Master 16.142

- Adds Prism Orbit, a seven-color moving hole cosmetic permanently earned by completing the first Run Goal.
- Persists the unlock locally and replaces vague cosmetic-progress text with the exact locked/equipped state.
# Master 16.143

- Converts every MegaKit preview building from one whole object into 36 separately consumable structural chunks.
- Reuses the kit's authentic brick, trim, concrete, roof, and interior materials on those chunks.
# Master 16.144

- Replaces the approximated MegaKit boxes with clipped sections of the actual UV-mapped `Building_Small_1` mesh.
- Preserves the genuine building windows, doors, brickwork, trim, roof, and silhouette while making twelve visible sections independently consumable.
# Master 16.145

- Repairs the authentic MegaKit section spawn failure caused by a stale box-grid gap reference in Master 16.144.
# Master 16.146

- Adds the approved Holesy icon as favicon, Apple touch icon, and installable web-app artwork.
- Changes Prism Orbit into a persistent reward for completing all Run Goals and the Mandate in the same wave; resets the earlier one-goal prototype unlock.
- Places five authentic MegaKit buildings only on reserved parcel centers and removes conflicting buildings from those parcels.
- Routes MegaKit model sections through skyscraper collapse motion.
- Fixes an AI hunt-speed initialization defect and makes larger rivals prioritize hunting the player when visible and in range.
# Master 16.147

- Adds substantially more trees to residential parcels.
- Cycles authentic MegaKit small, medium, and large building archetypes across district generations while keeping a single archetype on each reserved test parcel.
- Adds persistent selectable Tin-Foil Halo, Bellmar Seal, and Condemned Chic achievement cosmetics plus a menu selector.
# Master 16.149

- Removes mobile horizontal overflow and stacks menu action buttons vertically.
- Reserves space between transient/boss messages and mobile Pause controls.
- Adds a timer-frozen wave contract that presents mandatory do-or-die Mandates above optional benefit-bearing Run Goals, then animates both toward their normal HUD positions before play begins.
