# Holesy Build Changelog

Purpose: maintain concise build notes that can become player-facing patch notes inside the game.

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
