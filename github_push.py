"""
github_push.py - Push any local file to GitHub via API, no git client required.

Usage:
    python github_push.py <local_file> <repo_path> [branch] [commit_message]

Examples:
    python github_push.py "20_TESTS\\Candidate_Builds\\Master 16.html" "20_TESTS/Candidate_Builds/Master 16.html"
    python github_push.py "00_ADMIN\\Requirements\\PRODUCT_BACKLOG.md" "00_ADMIN/Requirements/PRODUCT_BACKLOG.md" codex/publish-master4-structure "Update backlog"

Arguments:
    local_file      Path to the file on disk (Windows or Unix style)
    repo_path       Path inside the GitHub repo (forward slashes)
    branch          Branch to push to (default: codex/publish-master4-structure)
    commit_message  Commit message (default: auto-generated from filename)

Setup (one time):
    Set environment variable GITHUB_TOKEN to a Personal Access Token with repo scope.
    Windows: setx GITHUB_TOKEN ghp_yourtoken
    Or create a file called .github_token in the same directory as this script.
"""

import sys
import os
import json
import base64
import urllib.request
import urllib.error
from pathlib import Path


OWNER = "klefner"
REPO  = "Holesy"
DEFAULT_BRANCH = "codex/publish-master4-structure"
API_BASE = "https://api.github.com"


def get_token():
    token = os.environ.get("GITHUB_TOKEN")
    if token:
        return token
    token_file = Path(__file__).parent / ".github_token"
    if token_file.exists():
        return token_file.read_text().strip()
    sys.exit(
        "ERROR: No GitHub token found.\n"
        "Set the GITHUB_TOKEN environment variable or create a .github_token file.\n"
        "Get a token at: https://github.com/settings/tokens (needs 'repo' scope)"
    )


def api(method, path, token, body=None):
    url = f"{API_BASE}{path}"
    data = json.dumps(body).encode() if body else None
    req = urllib.request.Request(
        url, data=data, method=method,
        headers={
            "Authorization": f"token {token}",
            "Accept": "application/vnd.github+json",
            "Content-Type": "application/json",
            "User-Agent": "holesy-push-script",
        }
    )
    try:
        with urllib.request.urlopen(req) as resp:
            return json.loads(resp.read())
    except urllib.error.HTTPError as e:
        body = e.read().decode()
        sys.exit(f"GitHub API error {e.code} on {method} {path}:\n{body}")


def push_file(local_path, repo_path, branch, message):
    token = get_token()
    local_path = Path(local_path)

    if not local_path.exists():
        sys.exit(f"ERROR: File not found: {local_path}")

    print(f"Reading {local_path} ({local_path.stat().st_size:,} bytes)...")
    content_b64 = base64.b64encode(local_path.read_bytes()).decode()

    # Get current branch tip
    print(f"Fetching branch {branch}...")
    ref = api("GET", f"/repos/{OWNER}/{REPO}/git/ref/heads/{branch}", token)
    base_commit_sha = ref["object"]["sha"]

    # Get the base tree SHA from that commit
    commit = api("GET", f"/repos/{OWNER}/{REPO}/git/commits/{base_commit_sha}", token)
    base_tree_sha = commit["tree"]["sha"]

    # Create a blob for the file content
    print("Uploading file content...")
    blob = api("POST", f"/repos/{OWNER}/{REPO}/git/blobs", token, {
        "content": content_b64,
        "encoding": "base64",
    })
    blob_sha = blob["sha"]

    # Create a new tree with this blob at the target path
    print("Creating tree...")
    tree = api("POST", f"/repos/{OWNER}/{REPO}/git/trees", token, {
        "base_tree": base_tree_sha,
        "tree": [{"path": repo_path, "mode": "100644", "type": "blob", "sha": blob_sha}],
    })
    new_tree_sha = tree["sha"]

    # Create a commit
    print("Creating commit...")
    new_commit = api("POST", f"/repos/{OWNER}/{REPO}/git/commits", token, {
        "message": message,
        "tree": new_tree_sha,
        "parents": [base_commit_sha],
    })
    new_commit_sha = new_commit["sha"]

    # Update the branch ref
    print("Updating branch...")
    api("PATCH", f"/repos/{OWNER}/{REPO}/git/refs/heads/{branch}", token, {
        "sha": new_commit_sha,
        "force": False,
    })

    print(f"\nDone. Committed {repo_path} to {branch}")
    print(f"Commit: {new_commit_sha[:12]}")


if __name__ == "__main__":
    args = sys.argv[1:]
    if len(args) < 2:
        print(__doc__)
        sys.exit(1)

    local_file    = args[0]
    repo_path     = args[1].replace("\\", "/")
    branch        = args[2] if len(args) > 2 else DEFAULT_BRANCH
    auto_message  = f"Add {Path(local_file).name}"
    commit_msg    = args[3] if len(args) > 3 else auto_message

    push_file(local_file, repo_path, branch, commit_msg)
