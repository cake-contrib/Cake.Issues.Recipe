---
name: start-release
description: Start a Cake Issues Recipe release by creating a release branch from develop, creating a GitHub release draft, and updating both packages' release notes links. Use when asked to start or prepare a release or release branch.
---

Use this skill to start a release of Cake.Issues.Recipe and Cake.Frosting.Issues.Recipe.
It replaces `git flow release start`; both recipes are released together with the same version.
See [CONTRIBUTING.md](../../../CONTRIBUTING.md#release) for the overall process and
[copilot-instructions.md](../../copilot-instructions.md) for repository guidance.

This skill does **not** create tags or publish releases.
Publishing the GitHub release is a manual step after finishing the release.

## Inputs

The version to release in the format `X.Y.Z` (for example `6.1.0`).
If no version was given, list the open milestones and ask which one to release:

```powershell
gh api --paginate repos/cake-contrib/Cake.Issues.Recipe/milestones --jq '.[].title'
```

## 1. Verify prerequisites

* Require a clean working tree (`git status --porcelain` returns nothing).
  Otherwise stop and ask the user; never discard changes.
* Check `gh auth status` and permissions to push branches and create releases.
* Verify both fetch and push URLs with `git remote -v`.
  Commands below assume `origin` points to `cake-contrib/Cake.Issues.Recipe`.
  If it is a fork, substitute the upstream remote throughout; if none exists, stop and ask.
* Fetch with `git fetch origin --prune` before checking branches.
* Require a milestone named `X.Y.Z`. Report open milestone items and ask whether to proceed:

  ```powershell
  gh issue list --repo cake-contrib/Cake.Issues.Recipe --milestone X.Y.Z --state open --limit 1000
  gh pr list --repo cake-contrib/Cake.Issues.Recipe --search 'milestone:X.Y.Z' --state open --limit 1000
  ```

* Check local and remote branches, tags, and GitHub releases for the requested version.
  Do not overwrite an existing `release/X.Y.Z` branch, tag, or release draft.
  If preparation has already started, report its state and ask whether to resume it.
  A published release or existing version tag must not be reused.
* Check `git worktree list` and obey workspace branch restrictions.
  Never modify another worktree or rename an app-managed branch.
  If switching or creating a release branch is not permitted in this workspace, stop and ask
  for an appropriate workspace rather than changing the branch silently.

## 2. Create the release branch

Start from the latest remote `develop`, not a potentially stale local branch.
Use `--no-track` so the release branch does not track `develop`:

```powershell
git switch --no-track -c release/X.Y.Z origin/develop
```

## 3. Create the GitHub release draft

Cake.Recipe uses GitReleaseManager to generate the draft from milestone items and
[GitReleaseManager.yaml](../../../GitReleaseManager.yaml).
Run the release-notes target from the release branch with an authenticated token.
Never print or persist the token. Preserve and restore any previous `GITHUB_PAT` value.

On Windows:

```powershell
$previousToken = $env:GITHUB_PAT
try {
    $env:GITHUB_PAT = gh auth token
    if ($LASTEXITCODE -ne 0) { throw 'Could not obtain a GitHub token' }
    .\build.ps1 --target=releasenotes
    if ($LASTEXITCODE -ne 0) { throw 'Release draft generation failed' }
} finally {
    $env:GITHUB_PAT = $previousToken
}
```

On Linux/macOS:

```bash
GITHUB_PAT="$(gh auth token)" ./build.sh --target=releasenotes
```

Verify the release is a **draft** for `X.Y.Z` with `master` as target:

```powershell
gh release view X.Y.Z --repo cake-contrib/Cake.Issues.Recipe --json tagName,isDraft,targetCommitish,body
```

If generation fails, report the error and ask how to proceed. Do not claim preparation succeeded.
Do not publish the draft, create a tag, or change versioning tools to work around an error.

## 4. Update package release notes links

Use [update-release-notes-link](../update-release-notes-link/SKILL.md) for both recipe packages.
Commit only the two metadata files, with the message `Update release notes link`:

```powershell
git add -- nuspec/nuget/Cake.Issues.Recipe.nuspec Cake.Frosting.Issues.Recipe/Cake.Frosting.Issues.Recipe/Cake.Frosting.Issues.Recipe.csproj
git commit -m "Update release notes link"
```

Follow applicable commit trailer requirements.
Unlike Cake.Issues, this repository has no website news post or `docs/mkdocs.yml` example version
setting to update. Integration tests deliberately use prerelease/wildcard recipe references;
leave those and the bundled addin versions unchanged.
Do not edit generated `version.cake`, `cake-version.yml`, or the placeholder nuspec version.
Cake.Recipe/GitVersion determines package versions from Git history.

## 5. Verify and push

* Review `git diff --check` and the commit diff. Only the two release-note URLs should change.
* `git --no-pager log --oneline origin/develop..HEAD` should show the single
  `Update release notes link` commit for a newly prepared release.
  If the links already matched the requested version, report that and do not create an empty commit.
* Confirm the working tree is clean and the draft exists with the correct target.
* Ask for confirmation before pushing, unless the user already explicitly authorized the push:

  ```powershell
  git -c push.followTags=false push -u origin release/X.Y.Z
  gh run list --repo cake-contrib/Cake.Issues.Recipe --branch release/X.Y.Z --limit 10
  ```

Report the branch and draft, including any running or failing CI.
Fixes can be added to the release branch with their issues or PRs assigned to milestone `X.Y.Z`.
Use [finish-release](../finish-release/SKILL.md) after review and successful CI.
