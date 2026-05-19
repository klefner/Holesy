# QA Review - Master 16.5 Debris Settle Fix

Date: 2026-05-19

## Scope

- Source master: `10_SOURCE/Masters/Master 16.html`
- Release package: `40_RELEASE/Website_Publish_Package/holesy/index.html`
- Upload copy: `C:\Users\KentLefner\Downloads\holesy-production-upload\index.html`

## Defect

Some collapsed skyscraper chunks could stop translating on the ground while continuing to rotate in place.

## Fix

- Ground impact now damps yaw rotation as well as roll/pitch rotation.
- Grounded chunks now receive stronger angular damping every frame while touching the floor.
- Low-motion grounded chunks snap fully to settled after a short idle period, preventing endless spin.

## Verification

- `node --check` passed for the extracted module script in `10_SOURCE/Masters/Master 16.html`.
- Release package and upload copy were regenerated from the same master file.
- Build label updated to `Master 16.5` in source, release package, upload copy, and in-game build notes.

## Residual Risk

Manual playtest should confirm collapsed skyscraper chunks still feel lively while falling but come fully to rest shortly after landing.
