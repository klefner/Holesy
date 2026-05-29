# QA Review - Master 16.29 Medium Office Building Voxel Collapse

Date:

- 2026-05-28

Scope:

- `10_SOURCE/Masters/Master 16/`
- `40_RELEASE/Website_Publish_Package/holesy/`

Purpose:

- validate the first Priority 1A medium-office-building voxel collapse slice
- preserve the modular browser-client package shape while changing gameplay behavior

Implementation reviewed:

- `Master 16.29` replaces medium office buildings with aligned cube stacks
- each medium-office cube is a saved and consumable stack piece
- columns activate independently when a hole moves underneath the cube footprint
- skyscrapers keep their existing large-piece whole-stack collapse path
- houses remain unchanged

Validation performed:

- `node --check 10_SOURCE/Masters/Master 16/js/main.js`
- desktop browser smoke through `http://127.0.0.1:8792/index.html?v=16.29-medium-voxels`
- mobile viewport browser smoke through the same modular package URL
- source and release package files were refreshed for `index.html`, `how-to-play.html`, and `js/main.js`

Observed results:

- desktop menu loads with `Master 16.29`
- desktop Begin starts gameplay and shows the HUD
- mobile viewport menu loads with `Master 16.29`
- mobile viewport Begin starts gameplay and shows the HUD
- medium office buildings are visibly composed of aligned cube/floor units
- no console page errors were observed in the smoke run; favicon 404 noise was ignored

Known remaining validation:

- user should regression-test the tactile feel of column-by-column collapse in normal play
- user should confirm that falling cubes can be missed if the hole moves away
- user should confirm skyscraper collapse remains visually and mechanically distinct
- user should confirm Endless save/load restores medium-building cube state after a real save/resume cycle

Release notes:

- changed-files-only GoDaddy delta package is appropriate when live is already on `Master 16.28`
- full package remains available for clean resync or rollback
