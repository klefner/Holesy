# QA Review - Product Intent And Architecture Recovery

Date:

- 2026-05-20

## Trigger

The user identified a material partnership and governance failure: the assistant packaged and described the current bundled `index.html` release artifact without carrying forward the accepted modular client architecture decision and without clearly separating active branch state, `main`, release package state, and future architecture direction.

## Finding

The failure was real.

The current GoDaddy package can be a single bundled `index.html` as an immediate deployable artifact, but the project has an accepted architecture decision to move toward modular browser-native assets. Treating the current bundled artifact as if it were the architectural target violates the product intent that the user had already established.

## Evidence

- `00_ADMIN/Requirements/ARCHITECTURE_DECISION_MODULAR_CLIENT_SPLIT.md` accepts incremental client-side modularization.
- `00_ADMIN/Requirements/PRODUCT_BACKLOG.md` states `PERF-010` closed only the architecture decision gate and that full modular implementation remains future work.
- `40_RELEASE/Website_Publish_Package/README.md` previously described the current `index.html` package but did not make the temporary-bundle versus modular-target distinction strong enough.
- `main` is stale relative to `codex/publish-master4-structure`, so a user checking `main` or another checkout can reasonably conclude the local repo is not current unless the assistant separates those states.

## Control Response

This recovery adds:

- `RELEASE_SOURCE_OF_TRUTH_MANIFEST.md`
- `PRODUCT_INTENT_GATE.md`
- `NEW_CHAT_TEAM_SYNC_PROTOCOL.md`
- `AUDITOR_AUTOMATION_PROMPT.md`
- updated publish workflow language that distinguishes temporary bundled upload artifacts from the accepted modular architecture target
- updated QA controls for product-intent continuity and process/procedure governance review
- issue-log entries `QA-011` and `QA-012`
- updated local `daily-qa-audit` automation prompt requiring every audit to study the process/procedure governance corpus and change it when needed

## User-Reported Intake Preserved

These items were captured during the recovery trigger but are not implemented in this control slice:

- Defect: pause-menu `Save Game` confirmation appears behind the blurred pause overlay instead of on the pause screen.
- Defect/design gap: skyscraper collapse still lacks enough physically plausible variation.
- Product backlog: medium buildings and houses should break into small cubes that preserve local color/material appearance.
- Product backlog: daily/weekly quests and contest-style reward milestones should exist, with rewards stored outside the monolithic HTML as the modular architecture matures.

## Recovery Standard

Future work is not ready to resume until:

1. the recovery controls are committed and pushed
2. the backlog current recommendation no longer points at stale `Master 16.5` sequencing
3. future packaging answers explicitly distinguish immediate upload artifact from long-term modular architecture
