# QA Review - Master 16.96 Boss Swallow Visibility And Boss Spotlight

Date: 2026-06-24

Build under review: `Master 16.96`

Scope:

- Defect repair for boss-derived offensive units disappearing immediately when the hole contacts them.
- Player-readability improvement for fifth-wave bosses that were hard to identify when first introduced.
- Source and local release package parity for the playable modular package.

Changes reviewed:

- Consumed soldiers and boss-derived offensive units now enter a transient descent queue and reuse the hole-descent path before their mesh is removed below the hole.
- True fifth-wave boss forms are scaled 40% larger than their later random-drop versions.
- True fifth-wave boss forms receive a red neon glow and ground ring. Later random-drop versions remain readable but do not carry the boss spotlight.
- Regular post-unlock tank drops no longer mark their tank mesh as the true army boss form.

Verification:

- `node --check` passed for source and release `js/main.js` and `js/build-info.js`.
- SHA-256 source/release parity matched for `index.html`, `how-to-play.html`, `PACKAGE_MANIFEST.md`, `js/main.js`, and `js/build-info.js`.
- Browser smoke passed on `http://127.0.0.1:4174/?v=16.96`: loaded `Master 16.96`, entered gameplay, rendered the WebGL canvas, and produced no console errors.
- Screenshot evidence saved at `C:\Users\KentLefner\AppData\Local\Temp\holesy-master-16-96-smoke.png`.

Release note:

- This updates the governed local source package and the local release package for player testing. It does not verify or update the live GoDaddy site.
