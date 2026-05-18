# Holesy Build Changelog

Purpose: maintain concise build notes that can become player-facing patch notes inside the game.

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
