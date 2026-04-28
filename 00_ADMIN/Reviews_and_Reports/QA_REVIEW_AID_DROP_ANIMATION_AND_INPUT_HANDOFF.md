# QA Review — Aid Drop Animation And Desktop Input Handoff

## Scope Reviewed

- Alien aid item descent behavior from ship to ground
- Object state during air-drop before pickup eligibility
- Desktop mouse / keyboard authority handoff
- Risk of stale mouse targets overriding keyboard intent

## Critical Findings

No critical issues found.

## Non-Critical Notes

- Desktop mouse control now requires a fresh mouse movement after keyboard use before the mouse can steer again. This is intentional and directly targets the stale-target drift the user described.
- Aid items are now non-pickup airborne objects until they land, which better matches player expectation and the military-drop precedent already in the game.

## QA Conclusion

The requested fixes are addressed and no critical gaps were found in the reviewed input, analysis, or implementation.
