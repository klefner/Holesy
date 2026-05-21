# Website Publish Package

## Purpose

This package is the website-ready build for the live `/holesy/` directory.

## Current Modular Package To Upload

- upload the full contents of `holesy/`

Important architecture note:

- this package now follows the accepted modular browser-client package shape
- `index.html` is only the entry point; it is not the whole game package
- `css/styles.css` and `js/main.js` must be uploaded with it
- see `00_ADMIN/Requirements/ARCHITECTURE_DECISION_MODULAR_CLIENT_SPLIT.md`
- see `00_ADMIN/Policies_and_Procedures/RELEASE_SOURCE_OF_TRUTH_MANIFEST.md`

## Source Of Truth

This package was generated from:

- `10_SOURCE/Masters/Master 16/`

## Current Package Notes

- `index.html` is the modular production/mobile-test entry point.
- `css/styles.css` contains the primary game stylesheet.
- `js/main.js` contains the primary game module.
- The bottom-left build badge opens in-game build notes for future patch-note publication.
- This package includes the promoted lore Archive, found-document drops, achievement buffs, Archive music, starter Field Patterns, and the end-screen/feedback cleanup from the `Master 15.39` through `Master 15.45` candidate lineage.
- Current patch label is `Master 16.22`.
- The current package also includes the `Master 16.x` Endless Waves line through player-death stop handling, Endless save/load, rival respawn behavior, buff cooldown tuning, rival-devour growth tempering, and Endless growth reset/tuning fixes.
- The current package completes `PERF-012` Phase 1 by aligning the source, release, and upload package folders to the committed modular browser-client structure.
- `Master 16.22` opens the separate How to Play field manual through the same explicit popup-window pattern used by the stats surface.

## Publish Rule

Upload this package file to:

- `https://ptbooksinc.com/holesy/`

using the GoDaddy File Browser.

Recommended upload structure:

- upload the contents of the local `holesy/` folder into the live GoDaddy `/holesy/` directory
- the live `/holesy/` directory should contain `index.html`, `css/`, `js/`, `assets/`, and `data/` at its root

## Important

- This package is intended for public website publishing
- Candidate builds should not replace it unless explicitly approved
- When a newer master is approved, regenerate this package from the new master
- Future packages must preserve the modular structure captured in the architecture decision; a single bundled file is no longer the normal publish shape
