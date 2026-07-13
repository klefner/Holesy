# Website Publish Package

## Purpose

This package is the website-ready build for the live `/holesy/` directory.

## Current Modular Release Package

The governed release package is the complete modular `holesy/` folder. Keep it intact as the source for clean installs, rollback, and package integrity checks.

For routine manual GoDaddy updates, provide a changed-files-only delta package by default. The delta package must preserve the same relative paths under `/holesy/` and should include only files changed since the previous approved master.

Current governed release baseline:

- `40_RELEASE/Website_Publish_Package/holesy/`

Current preserved GoDaddy convenience copies visible in Downloads may lag this baseline. Verify live state before preparing a fresh delta package.

Important architecture note:

- this package now follows the accepted modular browser-client package shape
- `index.html` is only the entry point; it is not the whole game package
- `css/styles.css` and `js/main.js` must be uploaded with it
- `holesy/PACKAGE_MANIFEST.md` enumerates the complete required package files and directories
- see `00_ADMIN/Requirements/ARCHITECTURE_DECISION_MODULAR_CLIENT_SPLIT.md`
- see `00_ADMIN/Policies_and_Procedures/RELEASE_SOURCE_OF_TRUTH_MANIFEST.md`

## Source Of Truth

This package was generated from:

- `10_SOURCE/Masters/Master 16/`

## Current Package Notes

- `index.html` is the modular production/mobile-test entry point.
- `css/styles.css` contains the primary game stylesheet.
- `js/main.js` contains the primary game module.
- `js/build-info.js` contains build metadata and patch notes.
- `js/difficulty-profiles.js` contains difficulty profile data.
- `data/lore-documents.js` contains Archive lore and starter unlock data.
- `js/government-physics.js` contains the isolated government-building physics prototype.
- `PACKAGE_MANIFEST.md` lists every required package file for clean install, rollback, and upload checks.
- The bottom-left build badge opens in-game build notes for future patch-note publication.
- This package includes the promoted lore Archive, found-document drops, achievement buffs, Archive music, starter Field Patterns, and the end-screen/feedback cleanup from the `Master 15.39` through `Master 15.45` candidate lineage.
- Current patch label is `Master 16.119`.
- The current package includes the `Master 16.x` Endless Waves, save/load, medium-office voxel, government-building physics, time-of-day lighting, natural skyscraper debris spread, small house/shop breakup line, optional MegaKit Downtown test environment, mobile HUD/haptic diagnostics, randomized Mandate variety, mobile haptics/mute placement repair, stronger haptic diagnostics, unsupported haptics clarity, native haptics bridge support, the iOS web haptic fallback attempt, the Capacitor native app shell scaffold, the Wave 1 soldier-free Mandate repair, and transparent first-run adaptive assistance through `Master 16.119`.
- `Master 16.74` completes the governed `PERF-012` modular production package migration evidence: source/release package shape is modular, low-risk data/config modules are extracted, and the package manifest records required upload files.
- `Master 16.77` keeps that modular package shape while applying the medium-style government voxel collapse repair and visible small-building breakup fix.
- `Master 16.78` keeps Classic Aldine as the default environment and adds a title-screen Environment selector for a MegaKit Downtown test district backed by imported CC0 Downtown City MegaKit textures.

## Publish Rule

Upload this package file to:

- `https://ptbooksinc.com/holesy/`

using the GoDaddy File Browser.

Recommended manual GoDaddy upload structure:

- if the live site is already on the previous approved master, upload only the changed-files delta package into the matching live GoDaddy `/holesy/` paths
- if the live site is missing older modular files, badly drifted, or being rebuilt, upload the full governed `holesy/` release package once to resync it
- the live `/holesy/` directory should contain `index.html`, `css/`, `js/`, `assets/`, and `data/` at its root

## Important

- This package is intended for public website publishing
- Candidate builds should not replace it unless explicitly approved
- When a newer master is approved, regenerate this package from the new master
- Future packages must preserve the modular structure captured in the architecture decision; a single bundled file is no longer the normal publish shape
- Delta packages are an upload convenience only; they do not replace the full modular release package as the governed baseline

