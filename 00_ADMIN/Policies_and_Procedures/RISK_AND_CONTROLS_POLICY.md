## Holesy Risk and Controls Policy

This document is the governing policy for Codex and Claude work on Holesy. It is written to reduce regression risk, preserve recoverability, and create a consistent control framework for gameplay, UI, and release changes.

### 1. Purpose

- Protect the project from avoidable regressions, data loss, unrecoverable edits, and unstable releases.
- Ensure changes are traceable, reversible, reviewable, and tested against known risks.
- Keep project management, technical leadership, and implementation aligned across agents.

### 2. Scope

- Applies to all code changes, test artifacts, UI changes, gameplay changes, file handling, version control, release preparation, and defect response work.
- Applies to both direct source edits and temporary browser-test HTML artifacts.

### 3. Operating Principles

- Prefer reversible actions over clever actions.
- Prefer a narrow fix over a broad rewrite unless the broader change is explicitly justified.
- Preserve the last known good state before applying a risky change.
- Do not overwrite the only stable artifact with an experimental change.
- Treat user-tested behavior as a key source of truth.
- Separate diagnosis, remediation, validation, and release decisions.

### 4. Version Control Policy

- Never work directly on `main` for active fixes.
- Create or use a dedicated working branch for each issue cluster or repair stream.
- Save changes in small checkpoints with meaningful commit intent.
- Keep `main` as the closest available stable baseline.
- Do not collapse multiple unrelated fixes into a single irreversible step.
- Preserve a clean rollback path before introducing structural changes.

### 5. Backup and Recovery Controls

- Before patching a defect, preserve the current source baseline or GitHub baseline.
- When testing risky changes, create a named test artifact rather than replacing the only trusted file.
- Name artifacts so their purpose is obvious, for example by defect, scope, or test goal.
- If a change creates uncertainty about state integrity, stop layering fixes and recover to the most recent trusted checkpoint.
- Recovery speed is a control objective, not an afterthought.

### 6. Change Classification

Each requested change should be classified before implementation:

- Low risk: isolated text, styling, copy, or simple UI placement changes with limited state impact.
- Moderate risk: gameplay rule adjustments, HUD logic, input handling, score logic, or mode-specific behavior.
- High risk: state machine changes, reset logic, mode transitions, audio/input startup flow, AI behavior changes, save/restart behavior, or broad refactors.
- Critical risk: any change that can strand the project without rollback, corrupt the baseline, break multiple modes, or block basic play.

High and Critical risk work requires tighter controls: preserved baseline, explicit artifact naming, and targeted regression review.

### 7. Control Requirements Before Implementation

Before implementing a change:

- Identify the baseline file or branch being used.
- Identify the specific defect or objective being changed.
- Identify adjacent systems likely to regress.
- Decide whether the work belongs in source, a test artifact, or both.
- Confirm the rollback path exists.

If any of those are unclear, default to the safest reversible approach.

### 8. Implementation Controls

- Keep edits as local as possible to the affected system.
- Avoid mixing feature work with defect repair unless the user explicitly wants both in the same pass.
- Avoid hidden behavioral changes.
- Prefer explicit state transitions over implicit side effects.
- If the codebase is clumsy, do not “improve” architecture casually during an urgent defect repair unless doing so is necessary for control or correctness.

### 9. Testing and Validation Controls

Every completion should include a control-minded review of likely regressions. At minimum, check:

- entry flow
- mode selection flow
- in-game HUD and overlays
- input behavior across relevant platforms
- scoring, growth, and win conditions
- pause, exit, restart, or reset behavior if affected
- interaction with any previously repaired defect in the same area

Where human testing is required, say so explicitly and keep the requested test focused.

### 10. Artifact Management Policy

- Shared project files should contain governing documents and stable references.
- Test artifacts should remain distinguishable from canonical source files.
- Documents used to guide agent behavior belong in shared files so both agents can reference the same policy.
- When a newer artifact supersedes an older one, say that clearly rather than implying all files are current.

### 10A. Master File Management

- A new `Master` should only be declared after the user validates the current build or explicitly directs promotion.
- The newly declared `Master` becomes the baseline for subsequent work.
- Previous masters remain archived recovery references and should not be overwritten.
- New experimental work should branch from the current master, not from an older master unless recovery requires it.
- If a master is superseded, say which file is current and which older masters are historical only.

### 11. Completion Review Standard

In every completion, Codex should review work against this policy and confirm:

- what baseline was used
- what rollback path exists
- what was changed
- what remains unverified
- what risks still exist

This review can be concise, but it must be present in substance even when not labeled formally.

### 12. Cross-Agent Coordination

- This document is the shared policy for Codex and Claude.
- If another agent’s prior change appears risky or non-compliant, respond by restoring control, not by hiding the issue.
- Use shared documents in project files as the durable coordination layer rather than relying on memory of past chat threads alone.

### 13. Exception Handling

- Urgent gameplay defects may justify fast test artifacts, but not loss of rollback safety.
- If tool or permission limits block a control objective, state the limitation and choose the next safest path.
- If the user explicitly chooses a riskier path, document the tradeoff and keep the recovery path intact where possible.

### 14. Enforcement

This policy should be treated as active project operating guidance until replaced by a newer shared-file version.

Codex commitment:

- I will execute future actions in compliance with this policy.
- I will review my completions against this policy going forward.
- I will use this shared-file document as the primary reference for version control, backup, recovery, and risk-control decisions on this project.
