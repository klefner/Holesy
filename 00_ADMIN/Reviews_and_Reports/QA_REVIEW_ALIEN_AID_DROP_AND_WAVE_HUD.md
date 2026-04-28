# QA Review — Alien Aid Drop And Wave HUD Relocation

## Scope Reviewed

- Input request and expected player-facing behavior
- Updated HUD placement for the troop deployment banner
- Alien aid-drop scheduler by game mode
- Powerup registry structure and pickup flow
- Hole speed / bullet-damage effect application
- Reset, pause, resume, wave-transition, and game-over cleanup interactions

## Critical Findings

No critical issues found.

## Non-Critical Notes

- Timed mode now schedules one aid drop on a random delay, while pure Last Man Standing schedules one every 60 seconds and Waves schedules one per wave. If a timed round converts into Last Man Standing before the timed drop has fired, the LMS cadence takes over from that point. This is a design assumption, not a defect.
- The three powerups are implemented through a registry-driven structure so additional entries can be added quickly, but only one active drop event is scheduled at a time.

## QA Conclusion

The feature set is acceptable for user testing. The HUD move is isolated and the alien aid-drop system appears structurally aligned with existing plane / soldier event patterns without introducing critical state-reset gaps.
