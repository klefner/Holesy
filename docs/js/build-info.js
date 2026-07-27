export const BUILD_MASTER = 16;
export const BUILD_SUB = 183;
export const BUILD_LABEL = BUILD_SUB > 0 ? `Master ${BUILD_MASTER}.${BUILD_SUB}` : `Master ${BUILD_MASTER}`;

export const BUILD_CHANGELOG = Object.freeze([
  {
    label: 'Master 16.183',
    title: 'Medieval roadside theme repair',
    notes: [
      'Removes fire hydrants and their water jets from Medieval Village while preserving hydrants in modern cities.',
      'Replaces the wider modern roadside-prop pool on Medieval native parcels with baskets, sacks, crates, barrels, hay, carts, trees, and torch-bearing villagers.',
      'Converts any legacy saved Medieval hydrant into a themed barrel during restoration.',
    ],
  },
  {
    label: 'Master 16.182',
    title: 'Harvest County and strict town themes',
    notes: [
      'Adds Harvest County with CC0 farm buildings, crops, horses, cows, pigs, sheep, orchards, wells, coops, and dense farmyard props.',
      'Builds a continuous opening-to-building consumption ladder across every Harvest County parcel instead of leaving broad decorative dead space.',
      'Uses the shared authored-facade destruction pipeline for barns, silos, the water tower, and windmill.',
      'Removes modern cars from Medieval Village and Harvest County while guaranteeing all four Medieval commons scenes plus more roaming animals and horses.',
    ],
  },
  {
    label: 'Master 16.181',
    title: 'Reusable authored-model destruction pipeline',
    notes: [
      'Replaces Medieval proxy rubble with offline-converted fragments that retain the original roofs, windows, timber, stone, and facade materials through collapse and settling.',
      'Applies explicit small, medium, large, and tower breach thresholds while preserving the established per-fragment fit and jam rules.',
      'Establishes one manifest-driven conversion, runtime, validation, and exception procedure for every future city built from externally authored models.',
      'Raises normal Medieval parcel edibles from six to fourteen and preserves the windmill blades through an explicit asset-only conversion rule.',
    ],
  },
  {
    label: 'Master 16.180',
    title: 'Living Medieval commons and street life',
    notes: [
      'Replaces modern Medieval park parcels with two rotating farms, pastures, barnyards, or sword-training yards.',
      'Adds edible vegetable rows, cows, sheep, chickens, hay, carts, barrels, training targets, and sparring villagers.',
      'Populates the wider village with roaming animals, torch walkers, and additional sword-fighting pairs while preserving the 32/4 pack-to-native parcel mix.',
    ],
  },
  {
    label: 'Master 16.179',
    title: 'Resilient Medieval asset loading',
    notes: [
      'Retries each Medieval model independently when the host returns a transient loading error.',
      'Continues building the rest of the village if one model remains unavailable.',
      'Always completes the 192-object growth ladder instead of aborting the town on one failed model request.',
    ],
  },
  {
    label: 'Master 16.178',
    title: 'Medieval growth ladder and physical-fit building entry',
    notes: [
      'Adds six staged medieval edibles around every pack-building parcel so the village supports continuous early growth.',
      'Lets ordinary village buildings open into their smaller façade pieces like original-city small and medium buildings.',
      'Uses the established width, depth, height, and jam checks on each exposed piece; true skyscraper-caliber structures retain a whole-building gate.',
    ],
  },
  {
    label: 'Master 16.177',
    title: 'Five-voice building destruction audio cap',
    notes: [
      'Limits all simultaneous building destruction sounds to five across the whole city.',
      'Applies the shared cap to ordinary building bites, imported-building collapse hits, and individual chunk impacts.',
      'Keeps the existing per-building cooldown underneath the global cap so dense collapses remain readable.',
    ],
  },
  {
    label: 'Master 16.176',
    title: 'Pack-authored Medieval town and detailed destruction',
    notes: [
      'Rebuilds Medieval Village so roughly 90 percent of its parcels use the authored village pack and roughly 10 percent retain native Holesy buildings or objects.',
      'Replaces oversized flat debris with smaller exterior-shell pieces carrying roof, timber, wall, stone, and window detail.',
      'Classifies imported buildings by their actual rendered proportions, so short village landmarks no longer count as skyscraper-caliber.',
      'Labels blocked devours as building, tower, or skyscraper according to the structure that was actually touched.',
    ],
  },
  {
    label: 'Master 16.175',
    title: 'Skip arbitrary presentation waits',
    notes: [
      'Adds one in-game <Skip> HUD control whenever presentation timing is the only thing preventing play from continuing.',
      'Covers wave-contract docking, rebuilt-district briefings, and the consumed-player return delay.',
      'Preserves round, Mandate, boss, aid, cooldown, and asset-loading clocks because those affect gameplay or real readiness.',
    ],
  },
  {
    label: 'Master 16.174',
    title: 'Temporary city override control',
    notes: [
      'Restores a start-menu city selector so the current city recipes can be called up directly during review.',
      'Includes Automatic Rotation, Classic City, MegaKit Downtown, and Medieval Village choices.',
      'Marks the selector as temporary and keeps the release-language gate blocking publication until the control is removed again.',
    ],
  },
  {
    label: 'Master 16.173',
    title: 'Composable city packs and MegaKit streets',
    notes: [
      'Introduces city recipes that can use no asset packs, one pack, or several packs as the city roster grows.',
      'Adds MegaKit crosswalks, lane arrows, STOP and SLOW markings, bike-lane art, drains, and concrete entrances to MegaKit Downtown.',
      'Keeps the new street layer stagger-loaded and independently removable so future packs can be combined without changing the core city generator.',
    ],
  },
  {
    label: 'Master 16.172',
    title: 'Medieval Village district rotation',
    notes: [
      'Adds Medieval Village as the third automatically rotated district without allowing the same city theme twice in a row.',
      'Stages five rotating CC0 village landmarks from the ten-building Quaternius roster and adds lightweight barrels, hay, carts, stalls, earth roads, and stone edges.',
      'Keeps imported village shells intact until breached, then transfers them into Holesy solid-block destruction and collision behavior.',
    ],
  },
  {
    label: 'Master 16.171',
    title: 'Market-ready district rotation and language cleanup',
    notes: [
      'Removes the player-facing environment selector and automatically rotates eligible districts without immediately repeating the same city theme.',
      'Replaces development-state wording in menus, feedback, haptics, the field manual, and public update history with player-ready language.',
      'Adds a release check that rejects prohibited development wording when it reaches player-facing surfaces.',
    ],
  },
  {
    label: 'Master 16.170',
    title: 'Frame pacing and city workload pass',
    notes: [
      'Dormant imported-building debris is detached until destruction, and fully settled piles no longer repeat thousands of useless collision checks each frame.',
      'MegaKit loading is spread across browser frames while inactive collapse detection and ordinary debris contacts use bounded spatial checks.',
      'Park turf uses fewer independently consumable tiles, shared ball geometry, and staggered nearby animation updates while frightened visitors remain fully responsive.',
    ],
  },
  {
    label: 'Master 16.169',
    title: 'Solid facade blocks and right-sized parks',
    notes: [
      'Imported MegaKit buildings now break into closed six-faced structural blocks with exterior facade, roof, and newly exposed interior faces assigned deliberately.',
      'Imported debris now uses box-based block contact physics, and each wave contains only one or two parks selected from full-parcel, mixed compact, or spacious showcase layouts.',
      'Adds volleyball, miniature golf, seesaws, four-hole golf with carts, and an amusement park to the park rotation.',
    ],
  },
  {
    label: 'Master 16.168',
    title: 'Park-wide fear response',
    notes: [
      'When a hole first consumes part of a park, every visitor in that parcel sprints in the direction they were already facing.',
      'Visitors keep that committed escape direction until they clear the attacking hole, then resume their normal park activity.',
    ],
  },
  {
    label: 'Master 16.167',
    title: 'Complete MegaKit building roster',
    notes: [
      'Adds destructible conversions for the remaining medium and large MegaKit buildings alongside the validated small building.',
      'Rotates every imported building front toward the player spawn and cycles all three authored models across the five test parcels.',
    ],
  },
  {
    label: 'Master 16.166',
    title: 'Truly immutable MegaKit asset revision',
    notes: [
      'Publishes corrected model and texture paths under new v2.3.1 URLs instead of overwriting cached v2.3.0 files.',
      'Keeps the exclusive imported-building parcel cleanup from Master 16.164.',
    ],
  },
  {
    label: 'Master 16.165',
    title: 'Versioned MegaKit texture-path repair',
    notes: [
      'Regenerates the immutable v2.3.0 model with texture paths relative to its versioned directory.',
      'Restores the authentic brick, window, door, trim, roof, and interior materials in the deployed building.',
    ],
  },
  {
    label: 'Master 16.164',
    title: 'Immutable MegaKit building deployment',
    notes: [
      'Moves the converted MegaKit building to a versioned asset URL so GitHub Pages cannot reuse an older brown-lattice model.',
      'Makes each imported test building own its full parcel, removing park, pool, and fixture overlap before placement.',
    ],
  },
  {
    label: 'Master 16.163',
    title: 'Wave briefing shows only unfinished work',
    notes: [
      'Removes completed Run Goals and satisfied Mandate targets from the large centered wave-start briefing.',
      'Hides an entire briefing card when that category has no unfinished items, while preserving the full progress HUD during play.',
    ],
  },
  {
    label: 'Master 16.162',
    title: 'Post-breach MegaKit skin visibility repair',
    notes: [
      'Separates authentic facade fragments from the opaque structural backing so the real windows, doors, trim, roof, and brickwork remain visible after breach.',
      'Insets the watertight backing to 96 percent and renders the authentic clipped surfaces ahead of it, eliminating the coplanar brown-lattice masking defect.',
    ],
  },
  {
    label: 'Master 16.161',
    title: 'Authentic rigid MegaKit fragments',
    notes: [
      'Clips every authentic model triangle to its owning destruction cell while interpolating original UVs and normals, preserving the kit artwork without folding facade sheets.',
      'Validates all 96 solid chunks against their colliders with zero overhanging primitives or vertices.',
      'Reserves government and park parcels so imported buildings cannot overlap park equipment or civic buildings.',
    ],
  },
  {
    label: 'Master 16.160',
    title: 'Authentic MegaKit shell and granular park showcase',
    notes: [
      'Displays the complete authored MegaKit building with its real windows, doors, trim, brickwork, roof, UVs, and materials until the first breach.',
      'Swaps the intact shell for the aligned 96-piece solid physics stack at impact so the existing collapse and consumption behavior remains intact.',
      'Breaks the baseball diamond into individually consumable turf, dirt, bases, mound, and backstop sections.',
      'Adds a guaranteed parcel showcasing five compact park designs: baseball, basketball, playground, fountain, and skate park.',
    ],
  },
  {
    label: 'Master 16.159',
    title: 'MegaKit building lattice blocker repair',
    notes: [
      'Preserves every mesh primitive in each converted block instead of discarding the roof, underside, windows, doors, trim, and authentic surface materials.',
      'Matches each visible closed block to its 98 percent collision body, removing the intentional 28 percent gaps that exposed the brown construction lattice.',
    ],
  },
  {
    label: 'Master 16.158',
    title: 'Authoritative Mandate success, giant arrow, readable boss names',
    notes: [
      'Recalculates Mandate completion directly from target progress so early and late success always turn the card green immediately.',
      'Runs exactly four bright green card-and-screen border pulses across four seconds after a red-warning completion.',
      'Makes the warning arrow explode into view at 360px in the center, travel and shrink to its HUD pointer size, then flash with synchronized sounds.',
      'Triples boss-name label dimensions and doubles its texture resolution.',
      'Rebuilds the MegaKit conversion as authentic UV-preserved surface fragments over 96 closed physical cores.',
      'Reduces park-object growth value by 80 percent and gives completed Goals and the full Goals panel green success states.',
    ],
  },
  {
    label: 'Master 16.157',
    title: 'Dense closed kit blocks with skyscraper collapse',
    notes: [
      'Re-converts Building_Small_1 from 12 large blocks into 96 smaller closed cubes arranged across six floors.',
      'Corrects outward triangle winding and forces all retained kit materials two-sided so exposed faces cannot disappear.',
      'Runs the cubes through the same topple, pancake, split, and twist collapse planner used by skyscrapers.',
    ],
  },
  {
    label: 'Master 16.156',
    title: 'Longer centered wave briefing',
    notes: [
      'Keeps the required Mandate and optional Run Goals centered for 5.5 seconds before docking them into the HUD.',
      'Keeps gameplay and the wave timer frozen through the full 6.5-second briefing and docking sequence.',
    ],
  },
  {
    label: 'Master 16.155',
    title: 'First offline-converted destructible kit building',
    notes: [
      'Ships Building_Small_1 as a preconverted 12-block glTF instead of rebuilding its destruction geometry in the browser.',
      'Each converted block is a closed six-face cube with brick facade, roof cap, exposed-interior underside, and box-collision metadata.',
      'Adds a deterministic converter, source hash, fixed recipe, validation record, and strict runtime block-count check.',
      'Duplicates the converted building across five test parcels for destruction and settling review.',
    ],
  },
  {
    label: 'Master 16.154',
    title: 'Mandate success state and staged warning arrow',
    notes: [
      'Turns a completed Mandate card light green with high-contrast dark text.',
      'Replaces an active red deadline border with a four-second green card-and-screen confirmation pulse when the Mandate is solved.',
      'Explodes the warning arrow into the screen center, sends it to the Mandate card, then begins five synchronized flashes and alert sounds.',
    ],
  },
  {
    label: 'Master 16.153',
    title: 'Ten animated full-parcel parks',
    notes: [
      'Randomly reserves four to six complete parcels for ten park archetypes instead of placing buildings there.',
      'Adds playground, basketball, baseball, tennis, running, swimming, picnic, fountain, dog, and skate parks.',
      'Adds normal, rundown, and fancy park variants plus animated players, balls, fans, swimmers, runners, dogs, smoke, and water.',
      'Builds park surfaces, fences, equipment, people, maintenance structures, stands, lights, and props as individually consumable objects.',
    ],
  },
  {
    label: 'Master 16.152',
    title: 'Solid kit blocks and permanent boss names',
    notes: [
      'Rebuilds imported MegaKit buildings from closed, roofed cubic pieces using materials from the original kit model.',
      'Keeps unusual collapse panels exclusive to skyscrapers unless a future building is explicitly exempted.',
      'Adds a permanent skin-linked name above every boss and includes that name in its incoming announcement.',
    ],
  },
  {
    label: 'Master 16.151',
    title: 'Debris sleep and starting-hole precision',
    notes: [
      'Stops grounded building pieces from spinning indefinitely and prevents tiny settled contacts from repeatedly waking debris.',
      'Shortens the small starting hole steering target and softens short touch gestures without changing movement speed.',
    ],
  },
  { label: 'Master 16.150', title: 'HUD+ mobile default', notes: ['Starts mobile players with the expanded HUD and saves their explicit HUD+/HUD- preference locally.'] },
  {
    label: 'Master 16.149',
    title: 'Mobile layout repair and mandatory-wave briefing',
    notes: [
      'Stacks mobile menu actions and prevents horizontal page scrolling.',
      'Keeps transient boss messaging clear of the mobile Pause controls.',
      'Freezes every wave for a concise centered Mandate-versus-optional-Goals contract, then docks both toward their HUD positions before the timer starts.',
    ],
  },
  {
    label: 'Master 16.148',
    title: 'Cosmetic achievement hook startup repair',
    notes: ['Moves the cosmetic reward hook from audio loading into the achievement unlock path, repairing the Master 16.147 gameplay-start failure.'],
  },
  {
    label: 'Master 16.147',
    title: 'Parcel exhibition deck and selectable achievement cosmetics',
    notes: [
      'Adds extra residential trees and cycles MegaKit small, medium, and large models without mixing archetypes on a test parcel.',
      'Adds permanent selectable Tin-Foil Halo, Bellmar Seal, and Condemned Chic rewards for The Forum User, Bellmar, and Linden Street achievements.',
      'Adds a persistent cosmetic registry and menu selector while preserving Prism Orbit as the all-goals-plus-Mandate reward.',
    ],
  },
  {
    label: 'Master 16.146',
    title: 'Holesy icon, earned cosmetic, MegaKit parcels, and rival hunts',
    notes: [
      'Adds the approved Holesy home-screen icon and install metadata.',
      'Requires all Run Goals and the Mandate in one wave to permanently unlock Prism Orbit.',
      'Reserves real parcels for MegaKit buildings, adds collapse motion, and strengthens larger-rival hunts.',
    ],
  },
  {
    label: 'Master 16.145',
    title: 'Authentic MegaKit section spawn repair',
    notes: [
      'Removes a stale box-grid gap reference that blocked the new clipped authentic model sections from spawning in Master 16.144.',
    ],
  },
  {
    label: 'Master 16.144',
    title: 'Authentic break-apart MegaKit model',
    notes: [
      'Replaces approximated brown boxes with clipped sections of the actual Building_Small_1 mesh, preserving its genuine UV-mapped brickwork, windows, doors, trim, roof, and silhouette.',
      'Divides each authentic model into twelve independently consumable visible sections while sharing the original model geometry and textures.',
    ],
  },
  {
    label: 'Master 16.143',
    title: 'Objectified MegaKit building skin',
    notes: [
      'Rebuilds each MegaKit preview from 36 individually consumable structural chunks instead of one monolithic object.',
      'Reuses the source kit brick, trim, concrete, roof, and illuminated-interior materials across the breakable pieces.',
    ],
  },
  {
    label: 'Master 16.142',
    title: 'First earnable hole cosmetic',
    notes: [
      'Completing the first Run Goal permanently unlocks and auto-equips Prism Orbit: seven colored motes moving inside the player hole.',
      'Persists the cosmetic locally and replaces vague cosmetic-progress claims with exact unlock or equipped status.',
    ],
  },
  {
    label: 'Master 16.141',
    title: 'Longer, larger Mandate attention warning',
    notes: [
      'Doubles the warning arrow to 184px and stretches its five flashes across 5.5 seconds so players have time to notice it.',
      'Keeps the comic alert synchronized with each slower flash.',
    ],
  },
  {
    label: 'Master 16.140',
    title: 'GitHub Pages MegaKit asset-path repair',
    notes: [
      'Keeps the Master 16.139 preview and corrects MegaKit model and texture requests so they remain inside the deployed /Holesy/ package.',
    ],
  },
  {
    label: 'Master 16.139',
    title: 'MegaKit building preview and louder Mandate pressure',
    notes: [
      'Loads one authentic MegaKit small-building model five times in MegaKit Downtown so its real materials and silhouette are easy to evaluate.',
      'Enlarges the five-flash Mandate arrow and gives every flash a short comic wobble-horn alert.',
    ],
  },
  {
    label: 'Master 16.125',
    title: 'Plain-language objectives and ten more street objects',
    notes: [
      'Shortens the Mandate warning and replaces ambiguous target language such as Edge People with direct map language.',
      'Converts Run Goal titles into literal Eat N object instructions.',
      'Adds ten more edible street-object families using the existing impact sound bank.',
    ],
  },
  {
    label: 'Master 16.124',
    title: 'Fair-score economy, rival escalation, new props, and boss test cadence',
    notes: [
      'Removes competitive score from Mandates, Run Goals, Goal Sweep, achievements, and lore multipliers; consumed objects, units, and rivals now own score.',
      'Makes rivals pursue edible soldiers more often and hunt rival holes more aggressively as waves rise.',
      'Adds ten new edible street-fixture families using the existing metal/impact sound bank.',
      'Temporarily spawns a boss every Endless wave and replaces the boss victory beeps with a seven-note brass fanfare.',
    ],
  },
  {
    label: 'Master 16.123',
    title: 'Time-safe messaging and runaway speed repair',
    notes: [
      'Replaces overlapping banner writes with immediate priority arbitration: critical current messages replace routine notices, and stale notices are discarded rather than delayed.',
      'Shortens banner copy and reduces mobile banner size and font.',
      'Ducks gameplay audio for boss cues and reduces non-refreshing unit-clear speed boosts to stop soldier-group chaining.',
    ],
  },
  {
    label: 'Master 16.122',
    title: 'Boss warning and defeat spectacle',
    notes: [
      'Adds a distinct low warning sequence and red danger pulse when a true boss is inbound.',
      'Makes player boss defeats a major reward moment with a victory stinger, rim pulse, camera kick, gold-red flash, score callout, and offensive-drop message.',
    ],
  },
  {
    label: 'Master 16.121',
    title: 'Run Goal and Goal Sweep reward pass',
    notes: [
      'Adds a brief glow/pop and a distinct three-note chime when an individual Run Goal completes.',
      'Makes Goal Sweep materially larger with the strong stinger, rim pulse, and short camera kick while preserving its score and speed reward.',
    ],
  },
  {
    label: 'Master 16.120',
    title: 'Immediate game audio and earned Mandate row rewards',
    notes: [
      'Starts recorded-sound decoding immediately from the player gesture instead of waiting until after gameplay begins.',
      'Rewards each completed Mandate row with 250 points, a compact score pop, a crisp two-note tick, and light feedback.',
    ],
  },
  {
    label: 'Master 16.119',
    title: 'Transparent first-run adaptive assistance',
    notes: [
      'Adds a small red HUD square whenever player results trigger an adaptive gameplay adjustment.',
      'Adds a persistent adjustment history with date, time, build version, adjustment, and reason; click the square to export it as a text file.',
      'Adds one bounded first-run rule: after 20 active seconds with no objective progress and under 250 points, Endless Wave 1 grants a 10% movement boost for 10 seconds.',
    ],
  },
  {
    label: 'Master 16.118',
    title: 'Wave 1 soldier-free mandate repair',
    notes: [
      'Keeps Wave 1 soldier-free on every difficulty, including Ultra.',
      'Prevents Wave 1 from offering Soldier or Military Unit Mandates when no military targets exist.',
    ],
  },
  {
    label: 'Master 16.117',
    title: 'Capacitor native app shell',
    notes: [
      'Adds a Capacitor native shell scaffold for iOS and Android using the governed modular release package as the web source.',
      'Installs the Capacitor Haptics plugin so the Master 16.115 native haptics bridge can trigger real device haptics in the native shell.',
      'Documents the native app sync/build workflow and keeps the browser latest.html testing path intact.',
    ],
  },
  {
    label: 'Master 16.116',
    title: 'iOS web haptic fallback attempt',
    notes: [
      'Adds a last-resort iOS WebKit switch-control haptic fallback before browser vibration reports unsupported.',
      'Reports iOS Tick when the fallback is triggered so iPhone Chrome testing can distinguish fallback attempts from missing APIs.',
      'Keeps the native haptics bridge as the real commercial iPhone haptics path.',
    ],
  },
  {
    label: 'Master 16.115',
    title: 'Native haptics bridge',
    notes: [
      'Adds a native haptics bridge path that uses Capacitor Haptics before falling back to browser vibration.',
      'Maps existing gameplay haptic events to native impact, notification, or vibrate calls so iPhone haptics can work inside a packaged native shell.',
      'Keeps the browser fallback and stable latest.html test launcher intact for normal web testing.',
    ],
  },
  {
    label: 'Master 16.114',
    title: 'Unsupported haptics clarity',
    notes: [
      'Changes the unsupported haptics diagnostic from No API to No Haptics so mobile browser limitations are clearer during testing.',
      'Explains that browsers without navigator.vibrate cannot make the device vibrate, and points testers toward Android Chrome or Samsung Internet.',
      'Keeps latest.html as the stable cache-refresh mobile test entry point.',
    ],
  },
  {
    label: 'Master 16.113',
    title: 'Stable latest URL and stronger haptic diagnostics',
    notes: [
      'Adds latest.html as a stable cache-refresh launcher that reads build metadata and opens the current build without changing the phone URL.',
      'Strengthens the explicit Haptics test pulse and makes the button itself report Sent, No API, Blocked, or Failed.',
      'Increases ordinary devour and heavy-object vibration durations so supported Android browsers should feel the gameplay haptic events more clearly.',
    ],
  },
  {
    label: 'Master 16.112',
    title: 'Mobile haptics test and mute placement repair',
    notes: [
      'Makes the Haptics menu test respond through click, pointer, and touch activation with visible status for sent, blocked, unsupported, or failed vibration attempts.',
      'Bypasses the haptic cooldown for the explicit test button so a player tap always attempts the diagnostic pulse.',
      'Moves the Music mute control to the mobile gameplay bottom-left stack just above HUD+ while keeping it in the menu corner outside active play.',
    ],
  },
  {
    label: 'Master 16.111',
    title: 'Randomized Mandate variety',
    notes: [
      'Replaces the fixed Mandate list with a randomized objective pool spanning people, props, trees, cars, buildings, rival holes, soldiers, military units, and bosses.',
      'Varies how many Mandate rows appear by wave tier: early waves ask for one target type, then later waves randomly grow toward the five-row maximum.',
      'Randomizes required counts within supply-capped limits so Mandates feel less repetitive while staying achievable with focused play.',
    ],
  },
  {
    label: 'Master 16.110',
    title: 'Mobile playfield HUD and haptic diagnostics',
    notes: [
      'Adds a mobile-first compact HUD that keeps the playfield clear by default while preserving an expandable objectives view.',
      'Moves detailed Run Goals, Mandates, and live scores out of the default mobile playfield overlay behind a HUD toggle.',
      'Adds a Haptics test/status control and loosens vibration dispatch so supported mobile browsers can prove whether haptics are available.',
    ],
  },
  {
    label: 'Master 16.109',
    title: 'Mandate fairness and scaling repair',
    notes: [
      'Retunes Mandate counts away from high first-wave inventory percentages so people and car requirements are achievable with focused play.',
      'Adds a clearer per-wave count curve with supply caps so Mandates scale upward without asking for too much of scarce categories.',
      'Keeps Mandates mandatory while making the first wave a fair survival contract instead of a near-total category sweep.',
    ],
  },
  {
    label: 'Master 16.108',
    title: 'Mobile haptic feedback pass',
    notes: [
      'Adds vibration haptic pings for player devours, with slightly heavier pulses for large object and building-piece consumption.',
      'Adds distinct mobile haptic patterns for Mandates, Run Goals, Goal Sweep, powerups, wave transitions, boss warnings, boss defeats, unit clears, rival devours, and player damage/death.',
      'Uses feature detection and cooldowns so unsupported browsers safely no-op and rapid consumption does not overwhelm mobile vibration.',
    ],
  },
  {
    label: 'Master 16.107',
    title: 'Mandatory Mandates and wave-long reward',
    notes: [
      'Makes wave-based Mandates truly mandatory: an incomplete Mandate at wave end now ends the run.',
      'Completing every Mandate row grants the score bonus plus a Mandate Surge speed/protection reward that lasts until the wave ends.',
      'Clarifies that Run Goals remain optional reward goals while Mandates are the survival contract for each wave.',
    ],
  },
  {
    label: 'Master 16.106',
    title: 'Endless flagship and boss pressure',
    notes: [
      'Makes Endless Waves the default flagship run on the mode-selection screen.',
      'Makes true bosses harder to casually swallow by requiring more hole mass and deeper center coverage.',
      'Adds close-range boss damage pressure and evasive kiting so bosses keep shooting while backing away from the nearest hole.',
    ],
  },
  {
    label: 'Master 16.105',
    title: 'Building weight and voxel fall repair',
    notes: [
      'Makes building debris feel heavier by increasing gravity and terminal fall speed while reducing upward hop and ground bounce.',
      'Repairs medium-building voxel cubes so the missed-hole settle path only runs after a cube has actually reached ground height.',
      'Keeps PC and mobile on the same modular package by mirroring the updated runtime files into the release package.',
    ],
  },
  {
    label: 'Master 16.104',
    title: 'Mobile mode picker button repair',
    notes: [
      'Changes the four game-mode choices from clickable cards into real button controls for mobile tap reliability.',
      'Adds pressed-state updates so the selected mode is exposed through button state as well as styling.',
      'Preserves the existing mode-selection layout while improving touch, keyboard, and accessibility behavior.',
    ],
  },
  {
    label: 'Master 16.103',
    title: 'Mandate pressure tuning',
    notes: [
      'Raises Mandate counts from small fixed targets to pressure targets based on a large share of the live district inventory.',
      'Increases Mandate pressure by wave so later districts ask for more of each listed category.',
      'Preserves the count-based Mandate HUD and readable font sizing from Master 16.101 and Master 16.102.',
    ],
  },
  {
    label: 'Master 16.102',
    title: 'Mandate font readability repair',
    notes: [
      'Increases Mandate objective row and progress text to match the Run Goals row font scale.',
      'Widens the Mandate card slightly so the larger count text remains readable without crowding.',
      'Keeps the count-based Mandate rules from Master 16.101 unchanged.',
    ],
  },
  {
    label: 'Master 16.101',
    title: 'Count-based mandate clarity',
    notes: [
      'Changes Mandates from hidden exact-object targets into explicit category counts such as Eat People, Eat Props, Eat Cars, Eat Offices, and Eat Towers.',
      'Shows live progress for each Mandate row so players know exactly how many matching objects are still needed.',
      'Moves the Mandate card lower under the timer/control cluster so the title and instructions remain readable.',
    ],
  },
  {
    label: 'Master 16.100',
    title: 'Mandate readability repair',
    notes: [
      'Moves the Mandate panel out of the wave-warning banner lane by anchoring it under the timer cluster.',
      'Adds readable target labels so players know the Mandate requires a Person, Prop, Car, Office, and Tower.',
      'Keeps the five-dot collected/warning state while making the objective actionable at a glance.',
    ],
  },
  {
    label: 'Master 16.99',
    title: 'Devour Mandate target system',
    notes: [
      'Selects one person, one prop, one car, one mid building, and one skyscraper as Mandate targets after each district is populated.',
      'Adds a centered Mandate HUD with five dots, remaining-count text, collected target state, and late-round warning pulse.',
      'Awards +2,500 score and shows Mandate Complete feedback when all current targets are consumed by the player.',
      'Shows Mandate Failed feedback on incomplete round end while leaving Locator Pulse and modal choices to later PBIs.',
    ],
  },
  {
    label: 'Master 16.98',
    title: 'Nonlinear offensive pressure and stats panel',
    notes: [
      'Changes offensive-unit wave scaling from a straight per-wave line to a stronger wave-pressure curve so later waves ramp harder.',
      'Keeps soldiers slower than vehicles while letting boss-derived vehicles reach higher speed and damage caps as waves climb.',
      'Moves Game Stats into an in-page modal so the Stats button works without popup permissions and refreshes after recorded runs.',
    ],
  },
  {
    label: 'Master 16.97',
    title: 'Rival score persistence and offensive-unit escalation',
    notes: [
      'Stops Endless rival respawns from reducing rival scores after the player eats them; competitive scores never go down mid-run.',
      'Adds per-run randomized rival AI difficulty so rivals vary by game instead of being rewritten each Endless wave.',
      'Retunes offensive units so soldiers are slowest, vehicles and boss-derived units move faster, and unit speed/damage scale upward by wave.',
      'Makes rival AI remember its own building collapses and stay near rubble long enough to collect the pieces.',
    ],
  },
  {
    label: 'Master 16.96',
    title: 'Boss swallow visibility and boss spotlight',
    notes: [
      'Keeps consumed soldiers and boss-derived offensive units visible as they fall into the hole instead of disappearing on first contact.',
      'Makes true fifth-wave boss forms 40% larger than their later random-drop versions.',
      'Adds a red neon boss glow to the fifth-wave boss form so players can immediately identify the featured boss archetype.',
    ],
  },
  {
    label: 'Master 16.95',
    title: 'Ten-boss roster and reset cadence repair',
    notes: [
      'Expands the offensive boss roster to ten archetypes with distinct visuals, ranges, cadence, damage shape, and tracer colors.',
      'Adds Mortar Carrier, Rail Sniper, Drone Marshal, Grenade Captain, Flame Rig, Railgun Tripod, and Shock Bruiser to the existing Siege Tank, Twin-Gun Mech, and Shield Commander roster.',
      'Corrects Endless world-shift resizing so each block gets five waves of growth, the boss appears on the fifth wave, and the reset happens on the sixth wave.',
    ],
  },
  {
    label: 'Master 16.94',
    title: 'Boss roster and offensive drops',
    notes: [
      'Removes the temporary tank-every-level testing flag so bosses return to fifth-wave cadence.',
      'Adds a rotating boss roster: Siege Tank, Twin-Gun Mech, and Shield Commander each have distinct models, damage, attack cadence, and rewards.',
      'Unlocks each revealed boss archetype as a later-wave random plane-drop option at reduced damage while preserving soldier-only Wave 2 through Wave 4 pressure.',
    ],
  },
  {
    label: 'Master 16.93',
    title: 'Skyscraper outward debris repair',
    notes: [
      'Corrects skyscraper collapse direction so chunks launch from the building center toward the hit side instead of being shoved back inward.',
      'Preserves the existing chaotic lateral scatter while removing the reversed center-seeking trajectory.',
      'Keeps medium-office voxel containment behavior and the temporary tank-testing visibility from Master 16.91 intact.',
    ],
  },
  {
    label: 'Master 16.92',
    title: 'Randomized wave start corners',
    notes: [
      'Randomizes each alive hole corner assignment at the start of every wave.',
      'Avoids putting a hole back into its previous wave-start corner when another corner is available.',
      'Keeps score, radius, and temporary tank-testing behavior from Master 16.91 intact.',
    ],
  },
  {
    label: 'Master 16.91',
    title: 'Temporary tank testing visibility',
    notes: [
      'Temporarily forces the green tank army boss into every playable mode and wave so player testing can see it immediately.',
      'Shortens the first testing deployment delay while the temporary tank test flag is enabled.',
      'Keeps the Master 16.90 tank model, push behavior, and building-pressure collapse behavior intact.',
    ],
  },
  {
    label: 'Master 16.90',
    title: 'Green tank army boss',
    notes: [
      'Replaces the red soldier-body army boss with a green tank body sized at roughly twice a car footprint.',
      'Lets the tank drive across road and off-road terrain while pushing loose props, people, trees, and cars it contacts.',
      'Adds tank pressure against building pieces so deeper contact activates progressive building collapse through the existing physics systems.',
    ],
  },
  {
    label: 'Master 16.89',
    title: 'Wave transition boss-trigger fix',
    notes: [
      'Fixes the Wave 1 to Wave 2 lock caused by the army boss late-mode trigger referencing a non-existent timed-mode flag.',
      'Keeps the Master 16.88 army boss behavior intact while allowing Wave 2 soldier deployment to start normally.',
      'Refreshes the local test cache label so player testing can confirm the fixed runtime is loaded.',
    ],
  },
  {
    label: 'Master 16.88',
    title: 'Army boss escalation',
    notes: [
      'Adds a red army boss unit that is four times larger than normal soldiers and deals three times soldier bullet damage.',
      'Spawns the boss every fifth Endless wave, late in Timed rounds, late in regular Waves, and during Last Man Standing endgames.',
      'Preserves army boss identity through planes, parachutes, soldier AI, devouring rewards, and Endless save/load restores.',
    ],
  },
  {
    label: 'Master 16.87',
    title: 'Comprehensive runtime performance pass',
    notes: [
      'Stops repeated vortex geometry uploads, global lighting scans on each bite, and duplicate object-to-hole distance passes.',
      'Adds shared district materials/geometries, staggered inactive-building checks, idle government physics, and lighter gameplay overlays.',
      'Moves sample-bank decoding out of input handlers and adds conservative automatic resolution/shadow fallback after sustained slow frames.',
    ],
  },
  {
    label: 'Master 16.86',
    title: 'Startup and frame-pacing repair',
    notes: [
      'Removes the duplicate city build that blocked the mode-selection screen during startup.',
      'Builds the playable city in small animation-frame batches so Begin provides responsive progress instead of one long main-thread freeze.',
      'Caps HUD and live-score DOM refreshes at 10 updates per second while gameplay rendering remains uncapped.',
    ],
  },
  {
    label: 'Master 16.85',
    title: 'Run goal instruction tooltips',
    notes: [
      'Adds high-contrast hover and keyboard-focus instructions to every displayed Run Goal.',
      'Explains the exact object family, target count, and score reward while preserving the existing goal titles.',
      'Uses lightweight CSS tooltips so the performance hotfix remains intact.',
    ],
  },
  {
    label: 'Master 16.84',
    title: 'Run goal performance hotfix',
    notes: [
      'Batches object-family mastery saves instead of writing local storage during every bite.',
      'Limits per-object goal bookkeeping to the active displayed goal families.',
      'Keeps the larger randomized goals and Goal Sweep reward from Master 16.83.',
    ],
  },
  {
    label: 'Master 16.83',
    title: 'Run goal tuning and performance repair',
    notes: [
      'Stops the Run Goals HUD from rewriting every frame so movement stays smooth.',
      'Expands the objective pool to 50 larger goals and chooses three different families per set.',
      'Adds a visible Goal Sweep reward for completing all displayed goals: bonus score plus a short speed surge.',
    ],
  },
  {
    label: 'Master 16.82',
    title: 'Run goals and object-family mastery',
    notes: [
      'Adds three visible run goals per round so players have fast, readable targets beyond raw score.',
      'Records object-family mastery locally for people, vehicles, props, trees, buildings, soldiers, and MegaKit manholes without granting permanent power yet.',
      'Completing a run goal awards immediate score while preserving the longer-term balance plan where defenses can catch up as holes improve.',
    ],
  },
  {
    label: 'Master 16.81',
    summary: 'MegaKit detail visibility repair.',
    changes: [
      'Widened and brightened MegaKit Downtown block-edge and sidewalk trim so it reads from the normal gameplay camera.',
      'Enlarged MegaKit road manholes and added brighter metal rings and surface bars so they are visible during evening and night lighting.',
      'Kept the detail non-colliding and below the hole render layer so holes remain visually authoritative.'
    ],
  },
  {
    label: 'Master 16.80',
    summary: 'MegaKit readable ground detail.',
    changes: [
      'Added thin non-colliding block-edge and sidewalk trim to make the MegaKit Downtown test environment read as a city grid without adding fake terrain.',
      'Replaced tiny box manholes with larger circular road manholes that are normal consumable props.',
      'Kept themed buildings out of MegaKit Downtown until future theme architecture routes them through validated breakable/destruction systems.'
    ],
  },
  {
    label: 'Master 16.79',
    summary: 'MegaKit environment safety repair.',
    changes: [
      'Removed MegaKit visual-only road, sidewalk, and ground patch overlays so holes cannot appear under fake terrain.',
      'Removed MegaKit showcase buildings because they did not use the validated breakable building/object destruction paths.',
      'MegaKit Downtown is now limited to small consumable prop dressing until future themed buildings are rebuilt through the voxel/destruction systems.'
    ],
  },
  {
    label: 'Master 16.78',
    summary: 'MegaKit Downtown test environment.',
    changes: [
      'Added a title-screen Environment selector while keeping Classic Aldine as the default.',
      'Added a MegaKit Downtown test district that uses imported CC0 Downtown City MegaKit textures on Holesy-authored roads, props, and showcase buildings.',
      'MegaKit-flavored roads, sidewalks, props, and showcase buildings render as an isolated environment test while Holesy collision and destruction physics remain on the validated baseline.'
    ],
  },
  {
    label: 'Master 16.77',
    summary: 'Government and house voxel repair.',
    changes: [
      'Government building pieces now use the same voxel-stack collapse path as medium buildings while keeping their government visual style.',
      'Small house/shop buildings now wake the whole compact stack on first contact so breakup is visible immediately.',
      'Government voxel pieces restore through the voxel save/load path instead of the older separate government physics world.'
    ],
  },
  {
    label: 'Master 16.76',
    summary: 'Government and house debris breakup.',
    changes: [
      'Government-building blast separation now becomes the staged physics base, preventing pieces from easing back toward the original grid before release.',
      'Small house/shop buildings now break into compact voxel chunks instead of being swallowed as one block.',
      'Saved Endless runs preserve restored small-building chunks as small debris pieces.'
    ],
  },
  {
    label: 'Master 16.75',
    summary: 'Natural skyscraper debris spread.',
    changes: [
      'Removed the artificial inward spread limiter from non-voxel skyscraper chunks so debris no longer nudges back toward the original building center.',
      'Preserved the existing medium-office voxel spread guard and behavior.'
    ],
  },
  {
    label: 'Master 16.74',
    summary: 'PERF-012 modular package completion.',
    changes: [
      'Completed the PERF-012 modular production package migration evidence with a governed package manifest and refreshed release notes.',
      'No gameplay behavior changed from Master 16.73.'
    ],
  },
  {
    label: 'Master 16.73',
    summary: 'Short building-window power flicker.',
    changes: [
      'Building windows now flicker only one to three short random times after power is cut, then stay dark.',
      'Destroyed-building windows still cannot be relit by later Time button cycling.'
    ],
  },
  {
    label: 'Master 16.72',
    summary: 'Building window power-off flicker.',
    changes: [
      'Lit building windows now flicker briefly before going dark as the building starts coming apart.',
      'Time-of-day cycling still cannot relight windows after their building has lost power.'
    ],
  },
  {
    label: 'Master 16.71',
    summary: 'Local test cache refresh.',
    changes: [
      'Bumped the local test build label and asset version so Chrome reloads the repaired Time button handler instead of reusing an older wave-lock module.',
      'No gameplay rule changed from Master 16.70: the Time button should cycle looks manually, while devoured streetlamps flicker off as they fall.'
    ],
  },
  {
    label: 'Master 16.70',
    summary: 'Streetlamp power-off polish.',
    changes: [
      'Devoured streetlamps now lose power as they fall, turning off the lamp head and glow.',
      'Time-of-day lighting no longer relights a streetlamp after it has been pulled into the hole.'
    ],
  },
  {
    label: 'Master 16.69',
    summary: 'Time button cycle repair.',
    changes: [
      'The Time button once again cycles morning, mid day, evening, and night when clicked.',
      'Wave-based modes still assign the starting look for each wave; manual Time cycling remains available for further visual testing during a wave.'
    ],
  },
  {
    label: 'Master 16.68',
    summary: 'Wave time-of-day sequence lock.',
    changes: [
      'Wave-based modes now lock each wave to the fixed morning, mid day, evening, night sequence.',
      'Endless waves repeat the sequence after night so wave 5 returns to morning.',
      'The Time button now snaps back to the active wave time during wave-based play instead of overriding the wave look.'
    ],
  },
  {
    label: 'Master 16.67',
    summary: 'Destroyed building lights cleanup.',
    changes: [
      'Lit building windows now extinguish when their building piece starts collapsing, falling, or being removed.',
      'Skyscraper, voxel-office, and government-building collapse activation now turns off affected building windows immediately.',
      'The Time button no longer relights windows on buildings that have already been destroyed.'
    ],
  },
  {
    label: 'Master 16.66',
    summary: 'Evening and night city lights.',
    changes: [
      'Evening and night now turn on street lamp glow, sparse lit building windows, and car headlights/taillights.',
      'Less than half of registered building windows are selected to light up, keeping the city readable without making every facade bright.',
      'Lighting is tied to the existing Time button and uses cheap mesh/material visibility changes instead of expensive dynamic light spam.'
    ],
  },
  {
    label: 'Master 16.65',
    summary: 'Government debris physics visibility.',
    changes: [
      'Government building pieces now get a short debris-escape window after first breach so shake, topple, bounce, and collision impulses are visible before the hole can swallow them.',
      'Strengthened the government physics staged activation with larger shake, lean, blast separation, and topple-release impulses.',
      'Improved government cube side-impact reactions with extra scatter, hop, and angular torque while preserving the separate government physics path.'
    ],
  },
  {
    label: 'Master 16.64',
    summary: 'Weather performance rollback.',
    changes: [
      'Removed the in-game Weather button and weather particle system after the visual weather pass caused severe runtime slowdown.',
      'Kept the Time button and morning, mid day, evening, and night lighting cycle because it does not run a per-frame particle field.',
      'Removed the weather frame-update path so gameplay no longer pays weather costs in the main loop.'
    ],
  },
  {
    label: 'Master 16.63',
    summary: 'Government building impact physics.',
    changes: [
      'Government building breaches now start with a visible shake-and-lean phase before pieces release.',
      'Touched columns receive stronger upward/outward impulses so pieces can jump out and scatter instead of only dropping into the hole.',
      'Government building cube collisions now add stronger bounce, side-impact hop, and angular spin while staying inside the separate government physics path.'
    ],
  },
  {
    label: 'Master 16.62',
    summary: 'Weather cycle cleanup.',
    changes: [
      'Removed the Ash weather effect from the in-game Weather cycle.',
      'Restored the richer Rain and Snow particle behavior from the time/weather preview pass.',
      'Kept Clear, Rain, and Snow as the available weather preview effects.'
    ],
  },
  {
    label: 'Master 16.61',
    summary: 'Time and weather preview controls.',
    changes: [
      'Added in-game Time and Weather buttons for cycling visual looks during play.',
      'Added Clear, Rain, Snow, and Ash weather looks with lightweight scene particles.',
      'Weather now adjusts fog, sky tint, ambient light, sun intensity, and ground tint without changing gameplay.'
    ],
  },
  {
    label: 'Master 16.60',
    summary: 'Time-of-day lighting pass.',
    changes: [
      'Added distinct morning, mid day, evening, and night scene looks.',
      'Wave-based modes now rotate sky, fog, ambient light, sun angle, and ground tint by wave.',
      'Timed and Last Man Standing modes keep the readable mid day look as their stable baseline.'
    ],
  },
  {
    label: 'Master 16.59',
    summary: 'Government building column-shock collapse.',
    changes: [
      'Government building breaches now identify the touched column and give it the strongest upward/outward shock.',
      'Neighboring government-building cubes receive softer randomized impulses so collisions create different collapse patterns each time.',
      'Preserves the separate government-building physics path while making its collapse effect the preferred reference for future building work.'
    ],
  },
  {
    label: 'Master 16.58',
    summary: 'Government building touch-crash fix.',
    changes: [
      'Fixed the first-contact government building activation crash caused by calling a non-existent impact-sound helper.',
      'Government building breach feedback now uses the existing budgeted voxel impact sound gateway.',
      'Preserves the separate government-building physics and spy/tuxedo visual treatment.'
    ],
  },
  {
    label: 'Master 16.57',
    summary: 'Government building visibility pass.',
    changes: [
      'Changed government buildings from civic gray to a high-contrast spy/tuxedo palette so they are immediately distinguishable from offices and skyscrapers.',
      'Added black, white, charcoal, and silver facade details, including shirt-and-bowtie-style front pieces and bright roof striping.',
      'Saved government building pieces now restore with the same distinct visual identity.'
    ],
  },
  {
    label: 'Master 16.56',
    summary: 'Government building physics prototype.',
    changes: [
      'Added a distinct government building that uses a separate fixed-step physics world instead of the existing building-collapse rules.',
      'Government building pieces use collider separation, impulse response, gravity, friction, bounce, and sleep behavior for more natural object reactions.',
      'The prototype appears once per rebuilt city and preserves its state through Endless save/load.'
    ],
  },
  {
    label: 'Master 16.55',
    summary: 'Endless score continuity and flat cube settle.',
    changes: [
      'Endless world shifts now reset hole growth size without wiping the cumulative live-score ledger.',
      'AI rival holes shot down by soldiers now respawn in Endless so the board does not run out of scoring rivals.',
      'Settled medium-office cubes snap to a flat ground face after coming to rest so debris no longer stays half-buried on diagonals.'
    ],
  },
  {
    label: 'Master 16.54',
    summary: 'Continuous medium office cube flow.',
    changes: [
      'Medium-office cubes now keep drifting and rotating during the support-delay window after impact.',
      'The first jarring impulse now flows into the falling phase instead of pausing and restarting as a separate drop.',
      'Released cubes inherit their current outward/upward motion while preserving the support-gated column collapse behavior.'
    ],
  },
  {
    label: 'Master 16.53',
    summary: 'Mouse exit steering carry.',
    changes: [
      'Desktop mouse steering now preserves the last intended direction when the cursor leaves the game canvas.',
      'The player hole keeps moving through brief browser-window exits during chases, escapes, and soldier pressure.',
      'Mouse carry clears on focus loss, reset, keyboard takeover, touch input, or normal mouse re-entry so other input modes stay predictable.'
    ],
  },
  {
    label: 'Master 16.52',
    summary: 'Medium office impact jarring.',
    changes: [
      'Medium-office impacts now jolt nearby cubes out of their perfect grid before the falling column releases.',
      'Jarred active cubes inherit small offset, lift, spin, and lateral impulse so each strike starts from a less uniform state.',
      'Settled medium-office cubes remain solid collision participants, reducing graphical overlap and making debris piles push apart more naturally.'
    ],
  },
  {
    label: 'Master 16.51',
    summary: 'Medium office pool-break physics.',
    changes: [
      'Medium-office cubes now fan outward with more varied first-impact vectors instead of falling in one tidy column.',
      'Cube-to-cube contact transfers more force, sideways scatter, and spin so collapses read more like a pool break.',
      'Ground bounces and local spread are stronger but still capped so office debris stays near the building and performance remains bounded.'
    ],
  },
  {
    label: 'Master 16.50',
    summary: 'Medium office impact kick.',
    changes: [
      'Medium-office cube columns now get a brief upward hop and outward shove when first impacted.',
      'Released cubes inherit a small upward/outward velocity so collapse motion reads more chaotic and reactive.',
      'The effect is intentionally smaller than skyscraper collapse and does not change scoring, growth, or save/load rules.'
    ],
  },
  {
    label: 'Master 16.49',
    summary: 'Restore readable classic hole.',
    changes: [
      'Removed the failed 3D/depth hole center experiments because they blocked visibility of objects falling into the mouth.',
      'Restored the original readable hole treatment: a flat black circular mouth with the existing colored border.',
      'Keeps the 16.40-16.45 falling-object behavior intact while deferring hole-depth visuals for a cleaner future approach.'
    ],
  },
  {
    label: 'Master 16.48',
    summary: 'Deep shaft hole visual.',
    changes: [
      'Replaced the still-flat abyss texture attempt with a tapering dark shaft whose bottom is physically lower and smaller than the mouth.',
      'Added a stronger vertical wall texture and deeper nested rings to make the hole read more like a descending shaft.',
      'Visual-only: devour, scoring, growth, and hole-mouth clipping behavior are unchanged.'
    ],
  },
  {
    label: 'Master 16.47',
    summary: 'Abyss-style hole depth illusion.',
    changes: [
      'Removed the gray recessed-well geometry from Master 16.46 because it did not read as real depth during play.',
      'Changed the hole center to a dark abyss texture with asymmetric inner shading and a black center.',
      'Added very subtle animated interior bands to suggest depth inside the mouth without changing gameplay.'
    ],
  },
  {
    label: 'Master 16.46',
    summary: '3D hole well visual.',
    changes: [
      'The hole center now renders as a recessed well instead of a flat black disc.',
      'Added a sloped dark inner wall, lower depth surface, and subtle animated interior bands.',
      'This is a visual-only upgrade; scoring, devouring, growth, and hole-mouth clipping behavior are unchanged.'
    ],
  },
  {
    label: 'Master 16.45',
    summary: 'Screen-space hole-mouth clipping.',
    changes: [
      'Falling objects now use the projected visible mouth of the hole, not only a ground-space radius check, to decide whether they can render as descending.',
      'Medium-office cubes that visually leave the black hole mouth now stop their hole-descent behavior and settle instead of continuing to fall on the street.',
      'This directly targets the outside-hole falling artifact still visible after Master 16.44.'
    ],
  },
  {
    label: 'Master 16.44',
    summary: 'Voxel hole-miss cleanup.',
    changes: [
      'Medium-office cubes that start falling but miss the live hole now settle as normal ground debris instead of continuing an orphaned descent.',
      'The active voxel physics path now uses the same visible-mouth rule as formal swallowed-object descent.',
      'Preserves the 16.42-16.43 hole-depth effect for objects that are actually inside the live mouth.'
    ],
  },
  {
    label: 'Master 16.43',
    summary: 'Hole-mouth visibility mask.',
    changes: [
      'Objects still fall from the fixed mouth-entry point, but only render while that descent column is inside the visible hole.',
      'When the hole moves away, already-swallowed objects continue their descent invisibly instead of falling through normal street.',
      'The visible well-depth behavior from Master 16.42 remains intact while preventing outside-hole falling artifacts.'
    ],
  },
  {
    label: 'Master 16.42',
    summary: 'Visible well-depth hole descent.',
    changes: [
      'Swallowed objects now render above the black hole surface while descending so they do not vanish at an invisible mouth barrier.',
      'Objects continue falling from their fixed entry point with a subtle screen-down drift that reads as depth inside the well.',
      'Save/load preserves the fixed descent direction for objects already falling into a hole.'
    ],
  },
  {
    label: 'Master 16.41',
    summary: 'Fixed-point vertical hole descent.',
    changes: [
      'Objects now keep falling at the exact world-space mouth contact point instead of drifting inward after entry.',
      'Objects remain visible longer and are removed much deeper below the hole mouth.',
      'Falling-object save/load now preserves the fixed entry point without carrying an obsolete lower-mouth target.'
    ]
  },
  {
    label: 'Master 16.40',
    summary: 'Same-side hole descent paths.',
    changes: [
      'Objects keep the mouth contact point where they enter the hole instead of snapping toward a center drain.',
      'Falling objects drift toward a same-side lower-mouth point as they descend.',
      'Save/load preserves active falling-object entry and lower-mouth targets.'
    ]
  },
  {
    label: 'Master 16.39',
    summary: 'Restore menu startup after modular extraction.',
    changes: [
      'Corrected the PERF-012 extraction boundary so renderer setup remains in js/main.js.',
      'Restored game-mode selection and Begin button behavior after the 16.38 blocker.',
      'Kept build metadata, patch notes, difficulty profiles, and Archive lore data in separate modules.'
    ]
  },
  {
    label: 'Master 16.38',
    summary: 'Low-risk modular data extraction.',
    changes: [
      'Moved build metadata and patch notes into js/build-info.js.',
      'Moved difficulty profiles into js/difficulty-profiles.js.',
      'Moved Archive lore documents and starter unlocks into data/lore-documents.js.'
    ]
  },
  {
    label: 'Master 16.37',
    summary: 'Save repair and collapse tuning.',
    changes: [
      'Endless saves now use compact object records so voxel-heavy boards do not exceed browser storage as easily.',
      'Skyscraper collapse and chunk audio now uses the same short sparse sound budget as medium-office voxels.',
      'Medium-office cubes drop faster, release lower floors sooner, and pick up more sideways motion so debris piles spread beyond the original footprint.'
    ]
  },
  {
    label: 'Master 16.36',
    summary: 'Cleaner office-cube falls.',
    changes: [
      'Medium-office cubes no longer shrink during hole-entry falls, so roof cubes keep their physical size.',
      'Voxel cube scoring no longer triggers the full building-collapse sound for every cube.',
      'Medium-office column and cube audio is now a single sparse thunk budget instead of layered building-crumble noise.'
    ]
  },
  {
    label: 'Master 16.35',
    summary: 'Voxel performance guardrails.',
    changes: [
      'Made medium-office voxels larger and reduced procedural cube counts so dense office collapses stay playable.',
      'Replaced unbounded voxel collision checks with a capped local contact budget per building.',
      'Clamped voxel impact velocity and spin so cubes settle more naturally instead of launching or jittering.'
    ]
  },
  {
    label: 'Master 16.34',
    summary: 'Slower voxel descent and reliable hole entry.',
    changes: [
      'Slowed medium-office voxel descent with lower gravity, lower terminal velocity, and longer teeter/release timing.',
      'Columns now commit to a slow side-fall after teetering instead of sometimes holding a permanent lean.',
      'Voxel cubes remember whether they crossed the floor inside the hole so eaten cubes cannot reappear after the hole moves away.'
    ]
  },
  {
    label: 'Master 16.33',
    summary: 'Voxel cube audio and missed-hole cleanup.',
    changes: [
      'Replaced full building-demolition spam from medium-office cubes with short budgeted cube-impact sounds.',
      'Limited simultaneous voxel cube impact sounds per building so collapsing offices stay crunchy instead of turning into static.',
      'Voxel cubes that miss the hole after the player moves away now land as visible debris instead of disappearing into the ground.'
    ]
  },
  {
    label: 'Master 16.32',
    summary: 'Slower office-cube gravity and column teetering.',
    changes: [
      'Added dedicated configurable voxel gravity so medium-office cubes fall slower than skyscraper chunks.',
      'Medium-office columns now teeter and lean before releasing, with upper cubes inheriting sideways motion from the failing column.',
      'Grounded voxel cubes keep their full size and settle as debris when they miss the hole instead of visually vanishing.'
    ]
  },
  {
    label: 'Master 16.31',
    summary: 'Support-gated voxel falling and oversized object jams.',
    changes: [
      'Medium-office upper cubes now wait for support to fail before falling, then accelerate under gravity.',
      'Active medium-office cubes push apart in 3D so falling pieces collide instead of visually overlapping.',
      'Oversized objects can jam in a hole, get dragged, and require enough smaller-object impacts to knock loose.'
    ]
  },
  {
    label: 'Master 16.30',
    summary: 'Procedural office voxels and rim-based falling.',
    changes: [
      'Made medium-office voxel cubes 25% larger so each piece reads better during collapse.',
      'Procedurally varies medium office length, width, and height from 5 to 10 cubes per axis.',
      'Changed falling-object drift to preserve the entry point inside the visible hole instead of pulling everything into a tiny center drain.'
    ]
  },
  {
    label: 'Master 16.29',
    summary: 'Medium office buildings now fall as voxel columns.',
    changes: [
      'Rebuilt medium office buildings from aligned cube floors instead of one giant consumable block.',
      'Made only the columns above the hole drop first, so moving under the footprint peels the building apart column by column.',
      'Saved and restored medium-building cube state for Endless saves instead of rebuilding offices as generic blocks.'
    ]
  },
  {
    label: 'Master 16.28',
    date: '2026-05-22',
    summary: 'Traffic orientation and soldier-damage growth repair.',
    changes: [
      'Re-aligned active traffic to its lane every frame so cars cannot keep driving while visually stuck sideways.',
      'Removed the collision yaw drift that could make normal driving look like an endless skid.',
      'Changed soldier damage to shrink the current hole radius without creating hidden score debt, so later devouring grows normally.'
    ]
  },
  {
    label: 'Master 16.27',
    date: '2026-05-21',
    summary: 'Mobile menu actions and post-soldier growth repair.',
    changes: [
      'Bound menu action controls through the same touch-safe activation path as desktop click handling.',
      'Made the How to Play control use the same explicit menu-action button class as Stats, Archive, and Load Endless.',
      'Changed soldier damage from hidden negative-growth debt to a lowered growth baseline, so devouring objects visibly grows the hole after being shot.',
    ],
  },
  {
    label: 'Master 16.26',
    date: '2026-05-21',
    summary: 'How to Play graphic restoration and hard traffic skid cap.',
    changes: [
      'Restored the full-height How to Play summary graphic layout.',
      'Added a hard crash-slide movement clamp so cars cannot skid multiple city blocks after a bad frame.',
      'Reduced panic-car crash duration, slide distance, and initial crash velocity.',
    ],
  },
  {
    label: 'Master 16.25',
    date: '2026-05-21',
    summary: 'How to Play upload dependency fix.',
    changes: [
      'Made all menu action controls use the same button styling path.',
      'Expanded the GoDaddy delta package to include missing unchanged dependencies when live needs them.',
      'Included the How to Play summary image in the upload delta so the field manual renders correctly on production.',
    ],
  },
  {
    label: 'Master 16.24',
    date: '2026-05-21',
    summary: 'Archive map moved out of the game and into project artifacts.',
    changes: [
      'Removed the Archive Map topic from the player-facing How to Play popup.',
      'Kept the archive-to-achievement spider diagram as governed project documentation rather than shipped game UI.',
      'Established changed-files-only GoDaddy delta packages as the default manual upload artifact.',
    ],
  },
  {
    label: 'Master 16.23',
    date: '2026-05-21',
    summary: 'Endless save confirmation now appears on the Pause screen.',
    changes: [
      'Added a visible status line inside the pause overlay.',
      'Moved Endless save success and failure feedback into that pause status area so it is not hidden behind the blurred game board.',
    ],
  },
  {
    label: 'Master 16.22',
    date: '2026-05-21',
    summary: 'How to Play now includes an Archive spider map.',
    changes: [
      'Added an Archive Map topic to the How to Play popup.',
      'Added a spider diagram linking archive threads, recovered documents, and their associated achievement or buff hints.',
    ],
  },
  {
    label: 'Master 16.21',
    date: '2026-05-21',
    summary: 'How to Play opens on a visual Game Summary.',
    changes: [
      'Added a top Game Summary topic to the How to Play popup.',
      'Added a modular summary graphic that explains the core loop: move, eat, grow, and avoid enemies.',
    ],
  },
  {
    label: 'Master 16.20',
    date: '2026-05-21',
    summary: 'How to Play now opens with the popup-window pattern.',
    changes: [
      'Changed the How to Play menu control from a plain new-tab link to an explicit popup window opener.',
      'Kept the field manual as its own modular page while matching the Stats window launch behavior.',
    ],
  },
  {
    label: 'Master 16.19',
    date: '2026-05-20',
    summary: 'Separate How to Play field manual window.',
    changes: [
      'Added a menu-accessible How to Play window with a left-side topic index and concise lore-forward instructions.',
      'Covered controls, game modes, win conditions, Archive access, Endless save/load, Stats, buffs, and survival notes.',
      'Kept the help surface as a separate lightweight popup so the main menu remains focused and fast.',
    ],
  },
  {
    label: 'Master 16.18',
    date: '2026-05-20',
    summary: 'Modular menu startup hotfix.',
    changes: [
      'Removed custom global/window state writes that could halt modular startup before menu listeners were wired.',
      'Kept music and stats-reset state inside the module so Begin, mode selection, and build notes remain clickable.',
      'Added cache-busted modular asset references for the source and release package.',
    ],
  },
  {
    label: 'Master 16.16',
    date: '2026-05-20',
    summary: 'Saved Endless visual restore and soldier-damage recovery hotfix.',
    changes: [
      'Restored saved skyscraper chunks with windowed skyscraper-piece geometry instead of generic block placeholders.',
      'Prevented soldier damage from creating a permanent negative-growth debt after the player escapes.',
      'Made consumed objects repair bullet-gouged radius debt before applying new growth.',
      'Tightened panic-car loss-of-control slide distance and initial crash velocity again.',
      'Added the current build label to the Pause menu.',
    ],
  },
  {
    label: 'Master 16.15',
    date: '2026-05-20',
    summary: 'Player death now hard-stops Endless simulation instead of letting the run continue underneath.',
    changes: [
      'Stopped world simulation immediately when the player is eaten by a rival.',
      'Stopped world simulation immediately when soldiers kill the player.',
      'Preserved the existing eaten/fade return flow while preventing indefinite gameplay after death.',
    ],
  },
  {
    label: 'Master 16.14',
    date: '2026-05-19',
    summary: 'Endless Waves can now be saved from Pause and loaded later from the menu.',
    changes: [
      'Added a Pause-menu Save Endless action that snapshots the current Endless run into local storage.',
      'Added a menu Load Endless action that restores wave, timers, holes, active buffs, aid, soldiers, planes, and board contents.',
      'Saved timed effects as remaining time so an hour away from the browser does not drain the saved run.',
    ],
  },
  {
    label: 'Master 16.13',
    date: '2026-05-19',
    summary: 'Rival-hole devours now scale by victim size and cannot balloon Endless beyond half-board scale.',
    changes: [
      'Tempered hole-eating score and direct radius gains so tiny recycled rivals give much smaller rewards.',
      'Reduced bonus-radius inheritance from devoured holes to stop rival recycling from compounding runaway size.',
      'Added an Endless radius cap at 50% of the active board width between fifth-wave world shifts.',
    ],
  },
  {
    label: 'Master 16.12',
    date: '2026-05-19',
    summary: 'Crowd Magnet is now a shorter, tighter pull burst instead of an early-wave growth engine.',
    changes: [
      'Reduced Pedestrian Pull / Crowd Magnet duration from 15 seconds to 5 seconds.',
      'Reduced Crowd Magnet bonus reach from +75% hole radius to +35%, with the Block Party combo reduced from +105% to +55%.',
      'Updated active-buff text and banner copy to show the shorter timer.',
      'Kept the five-second post-expiry reacquire cooldown from Master 16.11.',
    ],
  },
  {
    label: 'Master 16.11',
    date: '2026-05-19',
    summary: 'Timed lore buffs no longer refresh themselves into runaway growth loops.',
    changes: [
      'Ignored repeat pattern triggers while the matching timed lore buff is already active.',
      'Kept each timed lore buff on a five-second cooldown after it expires before it can be acquired again.',
      'Stopped Building Chain from granting repeated instant mass while its timed buff is already running.',
    ],
  },
  {
    label: 'Master 16.10',
    date: '2026-05-19',
    summary: 'Endless Waves now fully resets the land economy every fifth wave.',
    changes: [
      'Reset every hole score and size on Waves 5, 10, 15, and so on.',
      'Cleared bonus-radius, recent soldier-damage size modifiers, and temporary effects during each fifth-wave world shift.',
      'Added Endless world-shift messaging as the future hook for new lands and theme changes.',
    ],
  },
  {
    label: 'Master 16.8',
    date: '2026-05-19',
    summary: 'Fixed replaying wave-based modes from the scoreboard.',
    changes: [
      'Reset all hole life/state when a fresh Waves or Endless Waves run begins from the scoreboard.',
      'Stopped dead rival state from immediately ending or re-ending the next wave-based run.',
      'Kept the existing between-wave survivor behavior unchanged once a wave run is already in progress.',
    ],
  },
  {
    label: 'Master 16.7',
    date: '2026-05-19',
    summary: 'Endless Waves now stays endless after rival holes are eaten.',
    changes: [
      'Respawned rival holes smaller and far from the player after they are consumed in Endless Waves.',
      'Prevented Endless Waves from ending just because only the player remains briefly.',
      'Tightened panic-car loss-of-control timing and crash slide caps to reduce long skids.',
    ],
  },
  {
    label: 'Master 16.6',
    date: '2026-05-19',
    summary: 'Endless Waves opens the lockdown into an unbounded survival run.',
    changes: [
      'Added Endless Waves as a fourth selectable game mode.',
      'Generated wave pressure indefinitely, with current Ultra lockdown pressure reached around Wave 75.',
      'Added Endless-specific HUD, end-state copy, stats tracking, and voluntary cashout handling.',
    ],
  },
  {
    label: 'Master 16.5',
    date: '2026-05-19',
    summary: 'Grounded skyscraper debris settle fix.',
    changes: [
      'Stopped fallen skyscraper chunks from spinning in place after their position has settled.',
      'Added stronger ground friction for all rotation axes on collapsed building debris.',
      'Added a grounded idle cutoff so low-motion chunks snap fully to rest.',
    ],
  },
  {
    label: 'Master 16.4',
    date: '2026-05-19',
    summary: 'Buff cooldown and stronger skyscraper collapse variety.',
    changes: [
      'Added a 5-second reacquire cooldown after timed lore buffs expire.',
      'Prevented expired buffs from immediately retriggering the same effect during their cooldown window.',
      'Gave skyscrapers distinct collapse styles: toppling, pancake drop, split shear, and twisting failure.',
      'Added staggered floor collapse timing so buildings fail differently instead of all bursting at once.',
    ],
  },
  {
    label: 'Master 16.3',
    date: '2026-05-19',
    summary: 'Replay flow blocker patch.',
    changes: [
      'Fixed the post-game Begin button so it starts the selected mode instead of rebuilding the town behind the score screen.',
      'Reset stale end-screen, input, and transient round state before each new run begins.',
      'Guarded the Begin button against duplicate pointer, touch, and click activations from a single press.',
    ],
  },
  {
    label: 'Master 16.2',
    date: '2026-05-19',
    summary: 'Skyscraper collapse variation and shorter panic-car skids.',
    changes: [
      'Shortened panic-car skid duration and stopping distance.',
      'Changed skyscraper collapse from radial burst to impact-side directional failure.',
      'Added contiguous floor-band shearing so debris falls in related but varied directions.',
      'Capped collapse spread so chunks stay near the building footprint instead of exploding outward.',
    ],
  },
  {
    label: 'Master 16.1',
    date: '2026-05-18',
    summary: 'Mobile readability and end-of-round reward patch.',
    changes: [
      'Compressed and scroll-enabled the mobile mode-selection screen.',
      'Made the Found Documents archive scroll as one mobile page instead of letting the reader block most of the screen.',
      'Restricted found-document drops to live player wins only, with at most one document roll per completed run.',
      'Capped runaway car skids with distance, speed, and friction limits.',
    ],
  },
  {
    label: 'Master 16',
    date: '2026-05-13',
    summary: 'Production promotion candidate for mobile testing. Includes the lore and buff foundation plus the cleaned-up end screen.',
    changes: [
      'Added clickable in-game patch notes from the build badge.',
      'Promoted the validated lore, archive, buff, drop-rate, aid-drop, and end-screen fixes into the production package.',
      'Bundled the website package so the mobile test URL loads the full game from index.html.',
    ],
  },
  {
    label: 'Master 15.45',
    date: '2026-05-13',
    summary: 'Build notes candidate. The version badge now opens the future in-game patch-note surface.',
    changes: [
      'Added the Recovered Build Notes modal.',
      'Made the bottom-left version badge keyboard and pointer accessible.',
    ],
  },
  {
    label: 'Master 15.44',
    date: '2026-05-13',
    summary: 'End-screen and feedback cleanup after playtest findings.',
    changes: [
      'Replaced the duplicate post-game mode screen with the same Begin flow used at startup.',
      'Stopped skyscraper collapse warnings unless the building is actually inside the hole.',
      'Removed the brief traffic crash text while keeping crash behavior, smoke, fire, and audio.',
    ],
  },
  {
    label: 'Master 15.43',
    date: '2026-05-13',
    summary: 'Starter Field Patterns in the archive.',
    changes: [
      'Shows three early buff patterns immediately in the scrapbook.',
      'Renamed the starter hints to fit the recovered-archive tone.',
    ],
  },
  {
    label: 'Master 15.42',
    date: '2026-05-13',
    summary: 'Buff clarity and document pacing.',
    changes: [
      'Extended buff messaging so players can understand what changed.',
      'Made found documents rare, capped to one per round, and often absent.',
    ],
  },
  {
    label: 'Master 15.41',
    date: '2026-05-13',
    summary: 'Archive mood music.',
    changes: [
      'Added a separate document-screen music bed with whimsical conspiracy undertones.',
    ],
  },
  {
    label: 'Master 15.40',
    date: '2026-05-13',
    summary: 'Lore archive and achievement buffs.',
    changes: [
      'Added the found-document archive and first lore corpus integration.',
      'Added lore-triggered achievement buffs and player-facing feedback.',
    ],
  },
  {
    label: 'Master 15.39',
    date: '2026-05-13',
    summary: 'Idle lifecycle cleanup.',
    changes: [
      'Improved pause, visibility, unload, and animation cleanup to reduce long-idle browser risk.',
    ],
  },
]);
