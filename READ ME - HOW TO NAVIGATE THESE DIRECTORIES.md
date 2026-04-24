## READ ME - HOW TO NAVIGATE THESE DIRECTORIES

Use this file as the quick guide for the Holesy folder.

### The Main Idea

- `00_ADMIN` = project thinking and documentation
- `10_SOURCE` = real code versions
- `20_TESTS` = browser test builds
- `30_ARCHIVE` = old stuff we keep
- `99_TEMP` = scratch space

### What Goes Where

#### `00_ADMIN`

Put these here:

- policies
- procedures
- requirements
- feature ideas
- bug notes
- code reviews
- reports
- Word docs

If it explains the project instead of being the game itself, it usually goes here.

#### `10_SOURCE`

Put official code versions here.

- `Current`
  The active basis of work
- `Masters`
  Official promoted milestone versions like `Master 4.html`
- `Baselines`
  Imported or preserved comparison versions

If it is a real source version we may build from again, it belongs here.

#### `20_TESTS`

Put testable browser builds here.

- `Candidate_Builds`
  Good test builds that might become the next master
- `Exploratory_Builds`
  One-off or narrower experiments

If the point is “open this in browser and try it,” it usually belongs here.

#### `30_ARCHIVE`

Put older files here when we want to keep them but not actively work from them.

- `Legacy_Source`
  Old source files
- `Retired_Test_Builds`
  Old test builds

#### `99_TEMP`

Use this for throwaway files or temporary scratch work.

### Fast Rules

Before you save a file, ask:

1. Is this documentation?
2. Is this official source?
3. Is this a test build?
4. Is this old history?

Answers:

- documentation -> `00_ADMIN`
- official source -> `10_SOURCE`
- test build -> `20_TESTS`
- old history -> `30_ARCHIVE`

### Where You’ll Usually Look

Most often:

- current official version -> `10_SOURCE/Masters`
- current active basis note -> `10_SOURCE/Current`
- latest test builds -> `20_TESTS/Candidate_Builds`
- process/rules/docs -> `00_ADMIN`

### Good Naming Examples

- `Master 5.html`
- `Master 4 - HUD candidate.html`
- `Mouse control findings.md`
- `iPhone test notes Apr 24.md`

### One Important Habit

Do not leave important files loose in the Holesy root unless there is a specific reason.

The root should stay clean so we can find things fast.
