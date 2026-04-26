## Current Code Map

Baseline analyzed: `Master 4 - waves-difficulty-tuning-v2.html`

Purpose: document the current structure and fragility points before larger stabilization refactors continue.

## High-Level Structure

The game currently lives in one large HTML file containing:

- DOM structure
- CSS
- startup/title UI
- mode selection UI
- HUD and overlays
- audio generation/playback
- input handling
- entity setup
- AI behavior
- waves/military systems
- score handling
- game loop and rendering

## Current Control-Flow Map

### 1. Title / startup flow

Primary elements:

- `overlay`
- `titleEl`
- `subtitleEl`
- `playBtn`
- `modePickerWrap`

Current behavior:

- first click primes audio and reveals mode selection
- second click starts the game
- title music is tied to title-like overlay states

Fragility:

- state was previously inferred mostly from DOM visibility plus flags
- mobile music policy handling and title progression were tightly coupled

### 2. Mode selection flow

Primary elements:

- `selectedMode`
- mode picker click handler
- `startGame()`

Modes:

- `timed`
- `lms`
- `waves`

Fragility:

- mode choice influences timer behavior, score logic, wave flow, and end conditions
- branching starts early in `startGame()` and remains distributed through the file

### 3. Active gameplay flow

Primary elements:

- `running`
- `animate()`
- `updateHUD()`
- movement/input paths
- object consumption
- military/waves systems

Current behavior:

- `animate()` drives most simulation when `running` is true
- timer countdown, input, AI, movement, traffic, waves, and consumption all happen in the same central loop

Fragility:

- `running` has been overloaded as both simulation control and implicit UI-state control
- unrelated systems share the same loop without strong boundaries

### 4. Pause flow

Primary elements:

- `pauseBtn`
- `pauseOverlay`
- `pauseGame()`
- `resumeGame()`
- `returnToModeSelect()`

Fragility:

- pause eligibility historically depended on a mix of DOM visibility and flags
- pause, LMS choice, wave transition, and end screen are adjacent but distinct states

### 5. LMS choice flow

Primary elements:

- `showLmsChoice()`
- `enterLms()`
- `cashoutFromLmsChoice()`
- `lmsChoice`

Fragility:

- this is a real intermediate state, but historically behaved like “not running”
- that made end-flow and pause logic harder to reason about

### 6. Waves flow

Primary elements:

- `wavesMode`
- `currentWave`
- `wavesTransitioning`
- `WAVE_CONFIGS`
- `startWave()`
- `onWaveTimerExpired()`
- `updateWaves()`
- military spawn/update code

Fragility:

- waves progression mixes content tuning with flow control
- transition state and playing state were previously not formally separated

### 7. End-game flow

Primary elements:

- `endGame()`
- `overlay`
- `finalWrap`
- `leaderboardEl`

Fragility:

- end-game entry conditions were dependent on `running`
- cash-out flow needed special handling to route into end-game safely

## Current System Boundaries

### Input

- keyboard
- mouse ground targeting
- touch drag targeting

Status:

- partially improved, but still co-located inside the main animation loop

### Audio

- title/game music
- SFX
- mobile gesture-resume handling

Status:

- safer than before, but still mixed into UI state logic

### UI / HUD

- top-left status
- top-right timer/music/pause
- mini leaderboard
- pause overlay
- LMS choice overlay
- final overlay
- wave warning banner

Status:

- visually stronger than before, but update ownership is still distributed

### Reset / restart

- `resetRoundState()`
- `returnToModeSelect()`
- `tearDownWorld()`
- `repositionHolesForNewWave()`

Status:

- much safer than earlier project state, but still deserving of dedicated cleanup

## Critical Fragility Findings

1. State ownership has historically been split across:
   - `running`
   - DOM visibility
   - `lmsMode`
   - `wavesMode`
   - `wavesTransitioning`

2. Simulation and UI state were too tightly coupled.

3. Startup/audio compliance and title progression were adjacent enough that one could break the other.

4. Pause, LMS choice, wave transition, and end-game are distinct states that need explicit handling.

## First Refactor Patch Executed

This stabilization pass introduces an explicit named game-state layer so flow can be reasoned about more directly:

- `TITLE`
- `MODE_SELECT`
- `PLAYING`
- `PAUSED`
- `LMS_CHOICE`
- `WAVE_TRANSITION`
- `GAME_OVER`

This is not a full architecture rewrite yet. It is a control-improving first step.

## Recommended Next Refactor Step

Separate input ownership from gameplay simulation so mouse, keyboard, and touch transitions are controlled in one place instead of being inferred inside the main frame loop.
