# QA Review - Master 16.12 Crowd Magnet Tuning

Date: 2026-05-19

## Basis

- Source: `10_SOURCE/Masters/Master 16.html`
- Release package: `40_RELEASE/Website_Publish_Package/holesy/index.html`
- Upload package: `C:\Users\KentLefner\Downloads\holesy-production-upload\index.html`
- Build label: `Master 16.12`

## Scope

This review covers early Endless Waves tuning after playtesting showed Crowd Magnet remained too powerful even after timed buffs stopped refreshing their active timers.

## Implementation Review

- Pedestrian Pull / Crowd Magnet duration was reduced from 15 seconds to 5 seconds.
- Crowd Magnet bonus reach was reduced from +75% hole radius to +35%, and the Block Party combo reach was reduced from +105% to +55%.
- Crowd Magnet achievement effect text and active banner copy now describe the 5-second duration.
- The `Master 16.11` timed buff behavior remains in place: no refresh while active and a five-second cooldown after expiry.

## Validation Performed

- `node --check` passed on the extracted module script from the source master.
- Source, release package, and upload package all show `Master 16.12`.
- Source, release package, and upload package are byte-identical after refresh.
- `git diff --check` passed.
- Browser smoke loaded the cache-busted local build and confirmed the visible badge shows `Master 16.12`.

## Open Validation

- Player playtest should confirm the shorter, tighter Crowd Magnet burst is still noticeable but no longer causes runaway early-wave growth by Wave 4.
