## QA Review — Startup Mode-Select Regression Fix

Scope reviewed:

- user-reported startup regression in the latest candidate
- startup-state analysis versus intended desktop behavior
- patched output behavior for desktop and mobile/coarse-pointer environments

Issue confirmed:

- the current candidate always showed the `Tap to Load Music` first screen before the mode picker
- that behavior was acceptable for mobile audio-gesture protection, but it was a regression for desktop where the intended experience is to land directly on the mode-select screen

Fix applied:

- introduced `requiresGestureGatedStartup` to distinguish coarse-pointer / touch-first startup from desktop startup
- introduced `syncStartupUi()` so initial load and `returnToModeSelect()` share one startup-screen decision path
- preserved gesture-gated audio prime behavior only when `requiresGestureGatedStartup` is true
- allowed desktop first click to prime audio and continue directly into gameplay

Critical findings during QA:

- none after patch

Non-critical notes:

- Chrome mobile emulation should still show the music-prime screen because it intentionally models a coarse-pointer mobile environment
- this startup fix should be carried forward into the next Priority 1 refactor slice so reset/restart work does not regress it again

Approval:

- approved for export as the current startup-regression-fix candidate
