# QA Review - Master 16.11 Buff Cooldown

Date: 2026-05-19

## Basis

- Source: `10_SOURCE/Masters/Master 16.html`
- Release package: `40_RELEASE/Website_Publish_Package/holesy/index.html`
- Upload package: `C:\Users\KentLefner\Downloads\holesy-production-upload\index.html`
- Build label: `Master 16.11`

## Scope

This review covers the timed lore buff lifecycle fix requested after Endless Waves playtesting showed runaway growth caused by repeat pattern triggers refreshing active timed buffs.

## Implementation Review

- Active timed lore buffs now ignore repeat triggers while the existing timer is still running.
- Expired timed lore buffs now enforce a five-second same-buff cooldown before reacquisition.
- Building Chain's instant mass side effect is limited to fresh buff acquisition, preventing repeated mass stacking while its timed buff is active.
- Fifth-wave Endless reset behavior from `Master 16.10` remains unchanged.

## Validation Performed

- `node --check` passed on the extracted script from the source master.
- Targeted lifecycle simulation passed: first acquisition succeeds, active retrigger does not change expiry, cooldown retrigger does not change expiry, and reacquisition succeeds after five seconds.
- Source, release package, and upload package all show `Master 16.11`.
- Source, release package, and upload package are byte-identical after refresh.
- `git diff --check` passed.
- Browser smoke loaded the cache-busted local build and confirmed the visible badge shows `Master 16.11` with all four modes present.

## Open Validation

- Player playtest should confirm that Crowd Magnet, Block Sweep, Tree Feast, and Building Chain do not extend while active and cannot be reacquired until five seconds after expiry.
