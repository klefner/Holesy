# Holesy Build Changelog

Purpose: maintain concise build notes that can become player-facing patch notes inside the game.

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
