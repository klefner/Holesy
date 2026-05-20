# Website Publish Package

## Purpose

This package is the website-ready build for the live `/holesy/` directory.

## File To Upload

- `index.html`

Important architecture note:

- this package is currently a bundled single-file deployment artifact
- the accepted product architecture target is still modular browser assets, not a permanent single-file game
- see `00_ADMIN/Requirements/ARCHITECTURE_DECISION_MODULAR_CLIENT_SPLIT.md`
- see `00_ADMIN/Policies_and_Procedures/RELEASE_SOURCE_OF_TRUTH_MANIFEST.md`

## Source Of Truth

This package was generated from:

- `10_SOURCE/Masters/Master 16.html`

## Current Package Notes

- `index.html` is a bundled production/mobile-test build.
- The bundled shape is a temporary publish convenience until the modular production package migration is completed.
- The bottom-left build badge opens in-game build notes for future patch-note publication.
- This package includes the promoted lore Archive, found-document drops, achievement buffs, Archive music, starter Field Patterns, and the end-screen/feedback cleanup from the `Master 15.39` through `Master 15.45` candidate lineage.
- Current patch label is `Master 16.16`.
- The current package also includes the `Master 16.x` Endless Waves line through player-death stop handling, Endless save/load, rival respawn behavior, buff cooldown tuning, rival-devour growth tempering, and Endless growth reset/tuning fixes.

## Publish Rule

Upload this package file to:

- `https://ptbooksinc.com/holesy/`

using the GoDaddy File Browser.

Recommended upload structure:

- upload the contents of the local `holesy/` folder into the live GoDaddy `/holesy/` directory
- the live `/holesy/` directory should contain `index.html` at its root

## Important

- This package is intended for public website publishing
- Candidate builds should not replace it unless explicitly approved
- When a newer master is approved, regenerate this package from the new master
- Future packages should move toward the modular structure captured in the architecture decision; if a single bundled file is used again, call it a temporary exception or temporary artifact, not the project architecture target
