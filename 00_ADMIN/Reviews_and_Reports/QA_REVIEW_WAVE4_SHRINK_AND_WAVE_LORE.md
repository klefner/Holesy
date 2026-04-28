# QA Review — Wave 4 Shrink And Wave Messaging

## Scope Reviewed

- Wave 4 battlefield shrink behavior
- Fog-of-war boundary usage
- Hole spawn placement relative to the sealed outer haze
- Wave-by-wave adversary messaging
- Wave-transition lore messaging
- Repopulation timing relative to the active arena scale

## Critical Findings

- First QA pass found one critical implementation issue: the next-wave world was being repopulated before the new arena scale was applied, which could preserve the old battlefield footprint during transitions.

## Remediation

- Applied the next wave's arena scale before repositioning holes and repopulating the city during transitions.
- Applied the initial wave arena scale before the first Waves-mode repopulation as well, for consistency.
- Tightened Wave 4 to a `0.30` active battlefield scale so the sealed-haze concept actually changes play rather than just tinting the outskirts.

## Post-Remediation Result

No remaining critical issues found.

## QA Conclusion

This update is acceptable for user testing. The battlefield shrink now operates as an actual gameplay constraint, and the player is explicitly told what each wave adds plus why the city repopulates between stages.
