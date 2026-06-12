# QA Review: Master 16.64 Weather Performance Rollback

Date: 2026-06-09

Scope:

- Removed the in-game Weather button from the game controls.
- Removed the weather particle definitions, `THREE.Points` particle field, weather material/geometry buffers, Weather button listener, and per-frame weather update call from `js/main.js`.
- Kept the Time button and morning, mid day, evening, and night lighting cycle.
- Mirrored the changed runtime files from `10_SOURCE/Masters/Master 16/` to `40_RELEASE/Website_Publish_Package/holesy/`.

Rationale:

- User reported severe performance slowdown after the time/weather preview pass and said one or both effects had to go.
- Weather was the higher-risk performance source because it created a particle field and updated its buffer every frame; time-of-day is a static lighting/fog preset cycle.

Verification completed:

- Confirmed no live Weather button/runtime references remain in source or release entry/runtime files.
- Ran JavaScript syntax checks for source and release `js/main.js`, `js/government-physics.js`, and `js/build-info.js`.
- Compared SHA-256 hashes for changed source/release runtime files; all checked pairs matched.
- Loaded `http://127.0.0.1:4173/index.html` in the in-app browser; the page reported `Master 16.64`, showed `Time: Mid Day Pause`, omitted the Weather button, and had no console errors.
