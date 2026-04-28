# QA Review: Local Transition Highlight

## Scope
- Review the refinement to the wave-transition readability treatment
- Confirm the highlight is local to the message area rather than dimming the full screen
- Confirm the previously fixed Wave 1 banner suppression and end-screen cleanup remain intact

## Inputs Reviewed
- User defect report: the translucent treatment dimmed the entire screen instead of only improving local contrast behind the transition text
- Active source file:
  - `Master 4.html`

## QA Analysis
- The transition backdrop no longer uses a full-screen `inset: 0` layout.
- The highlight plate is now centered behind the event banner with bounded width, padding, and radius.
- The backdrop still shares the same show/hide timing as the event banner, so there is no orphaned visual state.
- The Wave 1 troop banner suppression logic remains tied to `soldiersEnabledThisWave`, which preserves the earlier regression fix.
- End-game cleanup still explicitly hides the wave HUD and clears any active event banner state.

## Findings
- Critical findings: none
- Non-critical note:
  - the local highlight uses a fixed plate size rather than measuring text length dynamically, but it is materially closer to the requested behavior and avoids battlefield-wide dimming

## QA Approval
Approved. No critical gaps remain in the inputs, analysis, or output for this refinement.
