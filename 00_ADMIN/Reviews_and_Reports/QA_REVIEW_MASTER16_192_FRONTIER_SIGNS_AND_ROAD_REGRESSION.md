# QA Review - Master 16.192 Frontier Signs And Road Regression

Date: 2026-07-29

## Scope

Player testing of the published Harvest County build found:

- storefront signs floating above and away from the frontier buildings instead of reading as mounted facade signs
- broad straight road slabs that regressed from the intended organic Wild West trail treatment

## Changes Reviewed

- Frontier storefront signs now use each normalized imported building's measured facade bounds.
- Sign width and height scale conservatively against the actual facade rather than the generic building footprint.
- Each sign is centered at low storefront-fascia height and mounted with only its shallow physical depth plus a small z-fighting clearance beyond the wall.
- Sign edges use a dark wood material while the lettered texture is limited to the outward face.
- The synthetic awnings were removed after player evidence showed that they also floated beyond the imported architecture.
- Harvest County retains the pending `Master 16.192` curved Catmull-Rom trail meshes, variable widths, irregular edges, weathered textures, wagon ruts, and hidden modern grid roads.

## Verification

- `node --check` passed for source and release `js/main.js`.
- Source and release `js/main.js` SHA-256 hashes match:
  - `02C5B6886F8888DECB1BDD9706B9F2D7A468B53E4FCDBEDA05EF18FC20045373`
- Local browser smoke at `http://127.0.0.1:4193/?fresh=16.192-sign-fascia-fix` loaded `Master 16.192`.
- Harvest County entered active Endless gameplay.
- Runtime state reported:
  - `data-holesy-town-topology="organic-dirt-routes"`
  - `data-holesy-harvest-sign-mounting="low-fascia-fit"`
  - all six frontier building roles present
- Browser console reported no warnings or errors.
- A rendered WebGL screenshot was inspected as required for the visual smoke pass.

## Deployment Finding

The public GitHub Pages URL remains on published `Master 16.189`. That deployed tree has `docs/index.html` but does not yet contain the pending `Master 16.192` organic-trail and facade-fit sign changes. Publication is a separate action and was not performed during this fix.
