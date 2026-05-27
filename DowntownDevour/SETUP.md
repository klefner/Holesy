# Downtown Devour – Unity Setup Guide

## One-time setup (5 minutes)

### 1. Open the project in Unity Hub

1. Open **Unity Hub**
2. Click **Add → Add project from disk**
3. Select the `DowntownDevour/` folder in this repository
4. If Unity prompts about a version mismatch, click **Open with [your version]**
   - Update `ProjectSettings/ProjectVersion.txt` to match your installed version if needed

### 2. Create the game scene

Unity cannot auto-generate a scene from code, so you need to wire one up once:

1. In Unity, go to **File → New Scene → Basic (Built-in)**
2. **Delete** the default Directional Light object (GameManager creates its own)
3. **Keep** the Main Camera
4. Create an empty GameObject: **GameObject → Create Empty**, name it `GameManager`
5. With `GameManager` selected in the Hierarchy, click **Add Component** in the Inspector
6. Search for and add the **GameManager** script
7. Save the scene as `Assets/Scenes/Game.unity`

### 3. Configure Build Settings

1. **File → Build Settings**
2. Click **Add Open Scenes** to add `Game`
3. Platform: **PC, Mac & Linux Standalone**
4. Target Platform: **Windows**
5. Architecture: **x86_64**
6. Click **Build** and choose a destination folder

### 4. Play in the Editor

- Press **Play** in the Unity Editor
- Mouse: move cursor to steer your hole
- WASD / Arrow Keys: also steer
- Game ends after 120 seconds

---

## Controls

| Input | Action |
|---|---|
| Mouse cursor | Steer player hole |
| W A S D / Arrow keys | Alternative steering |
| (no other keys needed) | |

---

## Project structure

```
Assets/Scripts/
  GameManager.cs      – Singleton; game state, timer, consumption loop
  HoleBase.cs         – Shared hole data + visuals (disc + rim mesh)
  HoleFactory.cs      – (inside HoleBase.cs) creates player/AI holes
  PlayerHole.cs       – Reads mouse/keyboard, calls HoleBase.SetTargetPosition()
  AIHole.cs           – Per-personality state machine (Void / Maw / Gulp)
  AIConfig.cs         – Personality data struct + Defaults()
  ConsumableObject.cs – Any city object that can be eaten; fall animation
  CityGenerator.cs    – Procedural 7×7 block city with buildings, props, cars
  MilitarySystem.cs   – Wave spawner: planes → paratroopers → soldiers
  UIManager.cs        – Canvas HUD (timer, scores, tier label) + end screen
  AudioManager.cs     – Procedural PCM audio; no external audio files needed
  GameCamera.cs       – Smooth overhead follow camera
```

---

## Porting notes (from HTML original)

All formulas are exact ports:

- Growth: `MIN_RADIUS + 0.95 × ln(1 + score/40) + bonusRadius`
- Hole-eats-hole reward: `250 + floor(score×0.3) + floor(radius×100)`
- Radius bonus on eat: `eater.bonusRadius += victim.radius × 0.6`
- Tier labels: cosmetic only, not gameplay gates
- Session: exactly 120 seconds
- AI personalities: Void (red, greed=1.4, react=0.45s), Maw (yellow, lazy=1.3, react=0.6s),
  Gulp (green, omniscient, react=0s)
