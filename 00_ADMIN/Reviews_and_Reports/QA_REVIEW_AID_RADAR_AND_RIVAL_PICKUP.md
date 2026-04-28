# QA Review — Aid Radar Audio And Rival Pickup Messaging

## Scope Reviewed

- Aid proximity audio behavior
- Volume scaling logic as player approaches the drop
- Rival-hole pickup announcement behavior
- Preservation of existing aid-drop timing and effect behavior

## Critical Findings

No critical issues found.

## Non-Critical Notes

- The radar ping now uses a brighter dual-tone sonar-style pulse with a much wider gain range, and the far-distance floor was removed so it can meaningfully fade with distance.
- Rival pickup messaging now uses the existing event banner path, which keeps the feedback visible without adding another modal or HUD lane.

## QA Conclusion

The requested fixes are addressed without introducing any critical logic gaps in the reviewed audio, messaging, or pickup flow.
