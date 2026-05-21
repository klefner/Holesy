# Standalone Website Publish Workflow

## Purpose

This workflow publishes the approved Holesy game in the live `/holesy/` directory instead of relying on a WordPress snippet as the runtime host.

This workflow must be read together with:

- `00_ADMIN/Policies_and_Procedures/RELEASE_SOURCE_OF_TRUTH_MANIFEST.md`
- `00_ADMIN/Policies_and_Procedures/PRODUCT_INTENT_GATE.md`
- `00_ADMIN/Requirements/ARCHITECTURE_DECISION_MODULAR_CLIENT_SPLIT.md`

## Current Decision

- WordPress can remain as the content/admin system for the broader site
- The Holesy game should be deployed as a standalone package at:
  - `https://ptbooksinc.com/holesy/`
- The governed release baseline is the full modular `holesy/` package containing:
  - `index.html`
  - `css/styles.css`
  - `js/main.js`
  - `assets/`
  - `data/`
- The default manual GoDaddy upload artifact is a changed-files-only delta package when the live site is already on the previous approved master.
- `index.html` is the package entry point only, not the whole game package.
- The accepted architecture target remains incremental client-side modularization into browser-native assets such as `css/`, `js/`, `assets/`, and `data/`.

## Canonical Publish Source

Unless a newer master is explicitly approved, publish from:

- `10_SOURCE/Masters/Master 16/`

Before publishing, confirm the in-game build label inside the publish artifact matches the intended approved build line.

## Packaging Rule

For live website publication:

1. Run the Product Intent Gate.
2. Confirm the publish package contains `index.html`, `css/`, `js/`, `assets/`, and `data/`.
3. Confirm `index.html` is treated as the entry point only.
4. Copy the approved source into the release package.
5. Compare the current release package to the previous approved master.
6. Create a GoDaddy delta package that includes only changed files, preserving their relative paths under `/holesy/`.
7. Upload the delta package contents into the matching live `/holesy/` directory paths.

The live `/holesy/` directory must contain `index.html` at its root. If the package is modular, the live directory must also contain every referenced external file and subdirectory.

Use the full modular package instead of the delta package only when:

- the live site is missing older modular files
- the live site is drifted or unknown
- a clean rebuild or rollback is requested
- a deletion cannot be completed through the delta upload alone

If the current master deletes a file that exists on the live site, the release answer must call out the deleted path explicitly because a delta upload cannot remove it by itself.

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
3. Upload the changed-files delta package contents into the matching paths
4. Replace older files only after confirming the new publish package is the intended master
5. Delete any explicitly listed removed files if the current master no longer ships them

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

1. Re-upload the previous full `holesy/` package
2. Or temporarily restore the WordPress slug path if necessary

## Release Discipline

For every public publish:

1. Approve a master in local testing
2. Promote it into `10_SOURCE/Masters`
3. Refresh the website publish package
4. Push the updated repo state to GitHub
5. Confirm the package respects the modular architecture target
6. Create a changed-files-only GoDaddy delta package unless a full package is needed for resync
7. Upload the package to GoDaddy

## Recommendation

Do not publish directly from a candidate build unless it has been explicitly approved and promoted or explicitly designated as a temporary hotfix candidate.

Do not answer "only `index.html` is required." Check the Release Source Of Truth Manifest and the accepted modular architecture decision, then identify the immediate delta upload files and the full modular package baseline they belong to.
