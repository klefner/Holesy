# QA Review - Master 16.168 Park Visitor Panic

Date: 2026-07-13

## Behavior

- Every park object and visitor carries its parcel identity.
- The first non-person park object consumed by a hole panics all visitors in that parcel.
- Each visitor captures the direction it was facing at that instant and keeps that direction while fleeing.
- Visitors calm when they are at least the greater of 14 world units or the attacking hole radius plus 9 units away, then resume their prior park animation from the new position.

## Verification

- JavaScript syntax validation passed.
- Live player acceptance remains required after GitHub Pages deployment.

