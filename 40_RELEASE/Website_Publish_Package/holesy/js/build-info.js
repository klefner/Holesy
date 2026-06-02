export const BUILD_MASTER = 16;
export const BUILD_SUB = 41;
export const BUILD_LABEL = BUILD_SUB > 0 ? `Master ${BUILD_MASTER}.${BUILD_SUB}` : `Master ${BUILD_MASTER}`;

export const BUILD_CHANGELOG = Object.freeze([
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
