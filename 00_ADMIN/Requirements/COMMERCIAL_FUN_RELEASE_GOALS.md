# Holesy Commercial Fun Release Goals

Status: active planning artifact
Created: 2026-07-06
Current build basis: Master 16.106

## North Star

Holesy should sell itself in the first minute: the player understands that they are a growing hole, sees immediate chaos, earns a reward quickly, feels threatened by rivals and bosses, and wants to start one more Endless run after losing.

Endless Waves is the flagship mode. Timed, Waves, and Last Man Standing remain supported, but commercial polish should optimize the first-run default around Endless survival, escalation, rewards, bosses, and replayable personal-best pressure.

## Flagship Endless Goals

- Default presentation: Endless Waves is the first selected game mode and is explicitly labeled as the flagship run.
- First promise: the first run should communicate "eat, grow, survive the lockdown, beat your best wave" without requiring How to Play.
- Session loop: each Endless block should have a readable arc: early feast, Mandate pressure, rival contest, offensive-unit pressure, boss climax, district reset, reward/save moment.
- Commercial value: Endless should feel like the mode players show friends because it has the most spectacle, chaos, and score-chasing.
- Other modes: keep Timed, Waves, and LMS available as alternate rule sets, but do not let their UI or tuning dilute the flagship onboarding.

## Reward Juice Pass

Goal: every meaningful accomplishment should produce a short, legible, satisfying reward burst without blocking gameplay or overwhelming mobile screens.

### Reward Events To Juice

- Mandate row complete: a compact row flash, small score pop, and a crisp tick sound.
- Full Mandate complete: banner, quick radial pulse at the player hole, score burst, short screen kick, and a triumphant stinger.
- Run Goal complete: objective row glow, score pop, and a distinct success chime that is quieter than a full Mandate.
- Goal Sweep complete: bigger banner, stronger stinger, and one-second celebratory particle/rim burst.
- Boss inbound: danger banner, deeper warning hit, boss spotlight pulse, and a low-frequency rumble.
- Boss defeated: heavier devour sound, red glow collapse, score burst, camera kick, and a short victory sting.
- Rival eaten or rival death: screen-side notification, bite sound, score movement, and quick ranking update.
- Endless wave milestone: wave-complete banner, save reminder, score/rank summary, and pressure-reset audio.
- Personal best: unique sound, badge flash, and persistent end-screen callout.
- Rare lore drop: paper/archive sound, subtle gold flash, and non-blocking Archive notification.

### Implementation Goals I Can Execute Without New Audio

- Add a reward event router in `js/main.js` that maps gameplay events to reusable visual and audio feedback presets.
- Use existing WebAudio helpers for generated placeholder tones: tick, chime, stinger, danger hit, boss rumble, and personal-best sparkle.
- Reuse existing banner, flash, tracer, rim, and score-pop systems before adding new rendering systems.
- Add intensity tiers: minor, major, danger, boss, milestone, personal-best.
- Add cooldowns so repeated row completions or soldier clears do not spam the screen or stack audio harshly.
- Verify desktop and mobile screenshots for text fit, no HUD overlap, and no modal-like reward blocks during play.

### Implementation Goals With User Audio Files

- Add `assets/audio/reward/` with a manifest naming each cue by gameplay event, not by raw filename.
- Support user-provided files for:
  - `mandate_tick`
  - `mandate_complete`
  - `run_goal_complete`
  - `goal_sweep`
  - `boss_inbound`
  - `boss_defeated`
  - `rival_eaten`
  - `wave_milestone`
  - `personal_best`
  - `lore_drop`
- Keep generated WebAudio fallbacks when a file is missing or muted.
- Normalize volume in code per cue so one loud file does not dominate the mix.
- QA each cue with music muted and music enabled.

### Acceptance Criteria

- The first completed reward in a run is unmistakable without reading text.
- Full Mandate completion feels materially bigger than one Mandate row completion.
- Boss defeated feels like one of the loudest and most valuable moments in a wave.
- Reward effects do not stop steering, block visibility, or cause mobile taps to miss.
- Reward bursts remain readable during high-chaos building collapse.

## First 60 Seconds Pass

Goal: a new player should understand the core verb, see growth, complete or nearly complete a clear objective, encounter pressure, and receive a satisfying reward inside the first minute of default Endless.

### Target Timeline

- 0-5 seconds: spawn near easy edible objects with visible movement space and no instant lethal pressure.
- 5-10 seconds: first growth moment should happen naturally from nearby people, props, or small objects.
- 10-20 seconds: first Mandate or Run Goal progress should visibly advance; avoid five simultaneous confusing priorities.
- 20-30 seconds: first reward pop should fire, preferably a Mandate row or Run Goal completion.
- 30-45 seconds: introduce rival pressure or soldier pressure clearly enough that the player notices a threat.
- 45-60 seconds: show the player the next escalation promise: wave pressure, boss tease, save value, or visible personal-best chase.

### Implementation Goals I Can Execute Without New Audio

- Tune Endless Wave 1 default spawn area so the player starts near a reliable food lane.
- Ensure the first visible Mandate counts are not so low that they complete instantly and not so high that nothing happens for 60 seconds.
- Gate early pressure so the first damage event teaches danger rather than feeling like random punishment.
- Add a first-minute debug overlay or console summary for QA only: first bite time, first growth time, first reward time, first damage time, and first objective completion time.
- Verify first-minute runs on desktop and mobile viewports with screenshots and console checks.

### Goals That Need Player Feel Review

- Decide whether the first visible objective should prioritize Mandates, Run Goals, or one combined "first feast" prompt.
- Decide how hard the first rival should push in the first minute of Endless.
- Decide whether boss foreshadowing should appear in Wave 1 as a distant siren/banner or wait until the first actual boss wave.
- Review whether reward audio should be playful, arcade-heavy, horror-comic, or gritty urban.

### Acceptance Criteria

- A first-time player can infer movement and eating from play, not instructions.
- The player sees at least one clear positive feedback moment by 30 seconds in most normal runs.
- The player is not asked to track more than two major HUD goals during the first 20 seconds.
- The first minute contains a visible reason to keep playing Endless.
- Performance remains smooth enough that first impressions are not framed by hitching, audio pops, or delayed input.

## Boss Fun Goals

- Bosses should be dangerous to approach, not only dangerous at range.
- Bosses should try to survive by backing away or strafing while maintaining weapon range.
- Speed boost should help the player maneuver into position but should not trivialize boss defeat.
- True fifth-wave bosses should feel meaningfully tougher than later reduced boss-derived drops.
- Boss defeat should become a high-juice reward moment in the Reward Juice Pass.

## Release Readiness Gate

Before commercial release, the flagship Endless path should pass:

- first 60 seconds desktop smoke
- first 60 seconds mobile smoke
- 10-minute Endless stability smoke
- boss encounter smoke
- reward-audio mute/unmute smoke
- release package parity check
- live upload verification after publish
