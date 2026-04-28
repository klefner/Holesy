# Standalone Website Publish Workflow

## Purpose

This workflow publishes the approved Holesy master as a standalone `index.html` in the live `/holesy/` directory instead of relying on a WordPress snippet as the runtime host.

## Current Decision

- WordPress can remain as the content/admin system for the broader site
- The Holesy game should be deployed as a standalone file at:
  - `https://ptbooksinc.com/holesy/`
- The approved game file should be uploaded as:
  - `index.html`

## Canonical Publish Source

Unless a newer master is explicitly approved, publish from:

- `10_SOURCE/Masters/Master 6.html`

## Packaging Rule

For live website publication:

1. Copy the approved master into the release package
2. Rename it to `index.html`
3. Upload that `index.html` into the live `/holesy/` directory

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
3. Upload the release `index.html`
4. Replace the older file only after confirming the new publish package is the intended master

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
5. Upload the package to GoDaddy

## Recommendation

Do not publish directly from a candidate build unless it has been explicitly approved and promoted or explicitly designated as a temporary hotfix candidate.
