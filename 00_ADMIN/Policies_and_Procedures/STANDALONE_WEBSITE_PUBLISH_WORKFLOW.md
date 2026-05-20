# Standalone Website Publish Workflow

## Purpose

This workflow publishes the approved Holesy game in the live `/holesy/` directory instead of relying on a WordPress snippet as the runtime host.

This workflow must be read together with:

- `00_ADMIN/Policies_and_Procedures/RELEASE_SOURCE_OF_TRUTH_MANIFEST.md`
- `00_ADMIN/Policies_and_Procedures/PRODUCT_INTENT_GATE.md`
- `00_ADMIN/Requirements/ARCHITECTURE_DECISION_MODULAR_CLIENT_SPLIT.md`

## Current Decision

- WordPress can remain as the content/admin system for the broader site
- The Holesy game should be deployed as a standalone file at:
  - `https://ptbooksinc.com/holesy/`
- The currently approved publish artifact is bundled as:
  - `index.html`
- This bundled artifact is a current release convenience, not the long-term architecture target.
- The accepted architecture target remains incremental client-side modularization into browser-native assets such as `css/`, `js/`, `assets/`, and `data/`.

## Canonical Publish Source

Unless a newer master is explicitly approved, publish from:

- `10_SOURCE/Masters/Master 16.html`

Before publishing, confirm the in-game build label inside the publish artifact matches the intended approved build line.

## Packaging Rule

For live website publication:

1. Run the Product Intent Gate.
2. Confirm whether this publish is using:
   - the current temporary bundled `index.html` artifact, or
   - a modular package containing `index.html`, `css/`, `js/`, `assets/`, and `data/`.
3. If publishing the current bundled artifact, explicitly state that the bundle is a temporary deployment artifact and does not replace the accepted modular architecture target.
4. Copy the approved source into the release package.
5. Upload the package contents into the live `/holesy/` directory.

The live `/holesy/` directory must contain `index.html` at its root. If the package is modular, the live directory must also contain every referenced external file and subdirectory.

## GoDaddy / WordPress Workflow

### 1. Protect the URL path

If WordPress already owns `/holesy` with a page slug:

1. Open the WordPress page
2. Change the slug from `holesy` to a backup name such as:
   - `holesy-wp`
   - or `holesy-old`
3. Update the page

This prevents WordPress from intercepting the standalone path.

### 2. Keep rollback available

Do **not** immediately delete:

- the WordPress page
- the WordPress snippet

Keep them as a rollback option until the standalone deployment is confirmed stable.

### 3. Upload the standalone package

In GoDaddy File Browser:

1. Open the site root
2. Open the `holesy` directory
3. Upload the release package contents
4. Replace older files only after confirming the new publish package is the intended master or explicitly approved temporary exception

### 4. Test the live URL

Validate:

- `https://ptbooksinc.com/holesy/`

Check:

- startup flow
- mode selection
- timed mode
- waves mode
- HUD placement
- latest approved gameplay changes

### 5. Rollback if needed

If the standalone file fails:

1. Re-upload the previous `index.html`
2. Or temporarily restore the WordPress slug path if necessary

## Release Discipline

For every public publish:

1. Approve a master in local testing
2. Promote it into `10_SOURCE/Masters`
3. Refresh the website publish package
4. Push the updated repo state to GitHub
5. Confirm the package respects, or explicitly documents a temporary exception from, the modular architecture target
6. Upload the package to GoDaddy

## Recommendation

Do not publish directly from a candidate build unless it has been explicitly approved and promoted or explicitly designated as a temporary hotfix candidate.

Do not answer "only `index.html` is required" without also checking the Release Source Of Truth Manifest and the accepted modular architecture decision.
