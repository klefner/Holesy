## Holesy Directory Structure

This document defines the recommended directory structure for the local Holesy project folder.

### Top-Level Layout

```text
Holesy/
  00_ADMIN/
    INDEX.md
    Policies_and_Procedures/
    Requirements/
    Reviews_and_Reports/
  10_SOURCE/
    Current/
    Masters/
    Baselines/
  20_TESTS/
    Candidate_Builds/
    Exploratory_Builds/
  30_ARCHIVE/
    Legacy_Source/
    Retired_Test_Builds/
  99_TEMP/
```

### Folder Purposes

#### `00_ADMIN`

Project governance and operational references.

- `INDEX.md`
  Project file map and “where things go” guide.
- `Policies_and_Procedures/`
  Active governance documents such as risk/control and workflow policy.
- `Requirements/`
  Product requirements, feature requirements, UX decisions, and future backlog notes.
- `Reviews_and_Reports/`
  Word docs, defect investigations, code reviews, and assessment reports.

#### `10_SOURCE`

Canonical source assets only.

- `Current/`
  The one active current source file or files that the project is presently building from.
- `Masters/`
  Promoted milestone baselines such as `Master 3.html`, `Master 4.html`, and later masters.
- `Baselines/`
  GitHub pulls, untouched imported baselines, and preserved comparison sources.

#### `20_TESTS`

Temporary or validation-focused builds.

- `Candidate_Builds/`
  Serious test candidates likely to become a future master.
- `Exploratory_Builds/`
  Narrow experiments, defect probes, and one-off trial builds.

#### `30_ARCHIVE`

Historical items retained for recovery or traceability.

- `Legacy_Source/`
  Older source files that are no longer the active basis of work.
- `Retired_Test_Builds/`
  Test artifacts that were useful historically but are no longer current.

#### `99_TEMP`

Scratch files only. Anything worth keeping should be moved out promptly.

### Naming Guidance

- Keep canonical promoted versions as `Master N.html`
- Keep current working source in `10_SOURCE/Current/`
- Keep test artifacts descriptive and issue-focused:
  `Master 4 - unified-ui-mouse-fix.html`
- Keep policy and admin docs in normal readable names, not cryptic abbreviations

### Recommended Mapping for Current Files

#### Move to `00_ADMIN/Policies_and_Procedures/`

- `RISK_AND_CONTROLS_POLICY.md`
- `ENGINEERING_WORKFLOW.md`

#### Move to `00_ADMIN/Reviews_and_Reports/`

- `Holesy_LMS_Scoring_Diagnosis_and_Code_Review.docx`

#### Move to `10_SOURCE/Current/`

- the single file we decide is the active basis of work

#### Move to `10_SOURCE/Masters/`

- `Master 4.html`

#### Move to `10_SOURCE/Baselines/`

- `downtown_hole_MASTER 3 - github-baseline.html`
- `downtown_hole_MASTER 3 - github-lms-score-fix.html`

#### Move to `30_ARCHIVE/Legacy_Source/`

- `downtown_hole_MASTER 3.html`
- `Downtown_Devour_extracted.html`

#### Move to `20_TESTS/Candidate_Builds/`

- `Master 4 - unified-ui-mouse-fix.html`
- `Master 4 - mouse-control-fix.html`

#### Move to `20_TESTS/Exploratory_Builds/`

- `downtown_hole_MASTER 3 - hud-pause-modes-test.html`
- `downtown_hole_MASTER 3 - input-handoff-fix-test.html`
- `downtown_hole_MASTER 3 - iphone-start-fix.html`
- `downtown_hole_MASTER 3 - mode-fix-test.html`
- `downtown_hole_MASTER 3 - scoring-hotfix-test.html`

### Operating Rules

- Only one file should be treated as the active current source basis at a time.
- New masters should be copied into `10_SOURCE/Masters/`, not overwrite prior masters.
- New candidate builds should go to `20_TESTS/Candidate_Builds/`.
- Once a candidate is promoted or rejected, either archive it or clearly label it.
- Shared governance documents belong in `00_ADMIN`, not mixed with source files.
