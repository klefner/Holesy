# QA Review — Master 15.7 Pre-Drop Aid Intel Fix

Date:

- 2026-04-30

Candidate under review:

- `C:\Users\KentLefner\Desktop\game-repo\Holesy\20_TESTS\Candidate_Builds\Master 15.7 - predrop-aid-intel-fix.html`

Baseline:

- `C:\Users\KentLefner\Desktop\game-repo\Holesy\20_TESTS\Candidate_Builds\Master 15.6 - transition-camera-and-soldier-escape.html`

Reason for candidate:

- user directly observed AI holes, including Gulp and Maw, waiting on the exact spot where aid would later be dropped
- that behavior confirms an unfair pre-drop intel leak rather than merely strong search behavior

Implementation review:

- build marker advanced to `Master 15.7`
- shared aid intel no longer records the exact future landing coordinates before touchdown
- ship-arrival intel now stores rough current ship position and travel direction only
- in-air aid intel now tracks the falling aid’s current position plus rough drift, not the final target location
- exact grounded intel is still restored once the pickup actually lands, so post-landing contests remain strong

Risk controls:

- change is isolated to aid-intel generation and consumption
- approved transition, camera, soldier-escape, and startup behavior remain untouched
- the intent is to preserve contested aid while removing unfair anticipation

Open validation required:

1. Confirm AI no longer waits on the exact drop point before the aid exits the plane.
2. Confirm aid still gets contested after it lands.
3. Confirm rivals still feel active, but less unfairly predictive than before.

Status:

- candidate ready for user browser validation
- not approved for promotion
