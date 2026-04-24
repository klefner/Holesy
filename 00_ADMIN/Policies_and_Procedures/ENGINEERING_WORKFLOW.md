## Holesy Engineering Workflow

This file is the operating policy for Codex and Claude work on this project.

### Version Control

- Make gameplay changes on a dedicated branch, never directly on `main`.
- Keep `main` as the last known stable baseline.
- Save work in small, named checkpoints so each risky change has a rollback point.
- Use descriptive artifact names for browser-test HTML files so test scope is obvious.

### Recovery

- Preserve the latest GitHub baseline before patching a defect.
- Create a fresh test artifact from that baseline instead of overwriting the only known-good file.
- When a patch regresses behavior, revert to the previous checkpoint rather than layering more guesses on top.

### Testing Artifacts

- One issue cluster per test artifact when practical.
- Prefer a consolidated artifact only after the underlying fixes are individually stable.
- Keep diagnosis notes close to the artifact so the reason for a file exists in writing.

### Response Discipline

- Refer to this workflow when making branch, backup, rollback, or test-artifact decisions.
- Default to the safest reversible path when the user has not specified a version-control preference.
