---
name: finish-release
description: Finish a Cake Issues Recipe release by checking release notes and CI, merging the release branch into master, and merging master back into develop without creating a tag. Use when asked to finish, complete, or merge a release.
---

Use this skill for a release prepared with [start-release](../start-release/SKILL.md).
See [CONTRIBUTING.md](../../../CONTRIBUTING.md#release) and
[copilot-instructions.md](../../copilot-instructions.md) for repository guidance.
This replaces `git flow release finish`, but **never creates tags or publishes the release**.
The user publishes the GitHub release afterward to create the tag.

## Inputs and expected history

Use version `X.Y.Z` supplied by the user or from the current `release/X.Y.Z` branch.
Otherwise list `git branch -r --list "origin/release/*"` and ask which release to finish.

Follow the topology used for releases such as `5.9.1` and `6.0.0`:

```text
*   Merge branch 'master' into develop    <- develop
|\
| *   Merge branch 'release/X.Y.Z'        <- master (tag created later)
| |\
| | * Update release notes link
```

Use the two merge messages shown above with `--no-ff` and applicable commit trailers.
Do not use a `Merge tag` message when no tag exists.

## 1. Verify prerequisites

* Require a clean working tree; stop and ask if `git status --porcelain` reports changes.
* Check `gh auth status`, repository permissions, branch protection, and `git remote -v`.
  Commands below assume both URLs of `origin` point to `cake-contrib/Cake.Issues.Recipe`.
  Substitute the actual upstream remote if needed; stop if none exists.
* Run `git fetch origin --prune`. Record the current remote `master`, `develop`, and release
  commit IDs. Check `git worktree list`; never modify another worktree or move its local branch.
* Require `origin/release/X.Y.Z`. If a local release branch exists with a different tip,
  ask whether unpushed work must be included before continuing.
* Require an unpublished GitHub draft for `X.Y.Z` targeting `master`:

  ```powershell
  gh release view X.Y.Z --repo cake-contrib/Cake.Issues.Recipe --json tagName,isDraft,targetCommitish,body
  git ls-remote origin refs/tags/X.Y.Z
  ```

  If the tag exists or the release is published, stop and report that the version is already
  tagged/published. Do not recreate or move tags.
* Review release-branch CI, including GitHub Actions and external checks:

  ```powershell
  gh run list --repo cake-contrib/Cake.Issues.Recipe --branch release/X.Y.Z --limit 10
  gh api repos/cake-contrib/Cake.Issues.Recipe/commits/<release-sha>/status
  gh api repos/cake-contrib/Cake.Issues.Recipe/commits/<release-sha>/check-runs
  ```

  Report failing, missing, or running checks and ask whether to continue before merging.
  Never disable protection or force push. If required checks prevent a push, stop and report it.

## 2. Review release contents and notes

Compare all changes intended for the release, not just the initial metadata commit:

```powershell
git --no-pager log --no-merges --format="%h %ad %s" --date=short origin/master..origin/release/X.Y.Z
git --no-pager log --format="%h %s" --grep="Update release notes link" origin/master..origin/release/X.Y.Z
gh issue list --repo cake-contrib/Cake.Issues.Recipe --milestone X.Y.Z --state all --limit 1000 --json number,title,labels,state
gh pr list --repo cake-contrib/Cake.Issues.Recipe --search 'milestone:X.Y.Z' --state all --limit 1000 --json number,title,labels,state
gh release view X.Y.Z --repo cake-contrib/Cake.Issues.Recipe
```

* Inspect commits added after `Update release notes link`, including merges from `develop`.
* For PR references in release commits, inspect the PR and linked issues:
  `gh pr view <number> --repo cake-contrib/Cake.Issues.Recipe --json number,title,labels,milestone,closingIssuesReferences`.
  Check that each relevant PR or its closing issue belongs to milestone `X.Y.Z`.
* Apply [GitReleaseManager.yaml](../../../GitReleaseManager.yaml): `Build` changes are excluded;
  review breaking changes, features, improvements, bugs, dependencies, and documentation.
* Compare the draft with the release commits and milestone. Check that user-facing changes,
  Cake/.NET requirements, and breaking changes for both scripting and Frosting are covered.
  Verify both package release-note URLs using
  [update-release-notes-link](../update-release-notes-link/SKILL.md).
* Report unmentioned changes, missing milestone assignments, and open milestone items.
  If anything is missing or still open, ask how to proceed before merging.

This repository has no website news post to update. Do not create one here or edit the sibling repo.
If the user requests draft regeneration, use the `releasenotes` target and token handling in
[start-release](../start-release/SKILL.md#3-create-the-github-release-draft) from a clean checkout
of the release tip in this workspace. Follow workspace restrictions; never enter another worktree.
Push approved metadata fixes, wait for CI, and repeat the review before proceeding.

If the release is already merged into `master`, do not treat an empty commit range as proof that
notes are complete. Inspect the existing release merge's first-parent range instead and verify
its second parent matches the intended release tip. Resume only the missing steps.

## 3. Prepare the two merges

Ask for confirmation before merging and pushing unless explicitly authorized already.
Keep newer dependency updates on `develop` when merging release-only fixes and release-note URLs
back into it. Stop and ask if conflicts cannot be resolved unambiguously.

In a workspace which permits detached checkouts, prepare both merges before pushing:

```powershell
git switch --detach origin/master
git merge --no-ff origin/release/X.Y.Z -m "Merge branch 'release/X.Y.Z'"
$releaseMerge = git rev-parse HEAD
git switch --detach origin/develop
git merge --no-ff $releaseMerge -m "Merge branch 'master' into develop"
```

Run commands one at a time and stop on any failure. Include applicable commit trailers.
If workspace policy requires remaining on the session branch, do not switch or rename it.
Instead prepare the master merge with `git commit-tree`, using `origin/master` as first parent,
`origin/release/X.Y.Z` as second parent, and the release tip's tree, **only after verifying** that
`origin/master` is an ancestor of the release tip. Then merge that commit into the session branch,
which must initially equal `origin/develop`. If these conditions are not met, ask for a suitable
workspace rather than constructing an unverified merge tree.

Verify both commits' parents, messages, and trees with `git show` and `git diff --check`.
The master merge tree must equal the release tip's tree when master is its ancestor.
Confirm the back-merge preserves the newer changes on `develop` and includes all release changes.

## 4. Push and verify

Fetch again and ensure all recorded remote tips are unchanged. If any moved, stop and reevaluate;
do not overwrite concurrent work. Prefer an atomic push so both branches advance together:

```powershell
git -c push.followTags=false push --atomic origin "${releaseMerge}:refs/heads/master" HEAD:refs/heads/develop
```

Here `HEAD` must still be the prepared develop back-merge. Do not restore the checkout until after
pushing it, or use its explicit commit ID instead. If atomic push is unsupported, report that and
ask before pushing the branches separately. Never force push or bypass failing required checks
without explicit user authorization.

Verify the remote tips with `git ls-remote origin refs/heads/master refs/heads/develop`.
Verify `refs/tags/X.Y.Z` is still absent and the release is still a draft targeting `master`.
Check the new CI runs and report their state without claiming queued/running builds have passed.
Restore the original checkout after pushing if workspace policy permits and it is safe to do so.

## 5. Optional release branch cleanup

Ask separately before deleting the release branch. Only delete it after both remote merges are
verified and the release tip is an ancestor of both destination tips.
Check the remote release tip has not changed since review; if it has, stop and reevaluate.

```powershell
git push origin --delete release/X.Y.Z
```

Delete a local branch only if requested, fully merged, and not checked out in any worktree.
Use `git branch -d release/X.Y.Z`, never force deletion.

## Publish later

Tell the user to publish the `X.Y.Z` draft on the
[GitHub releases page](https://github.com/cake-contrib/Cake.Issues.Recipe/releases), targeting
`master`, after reviewing the new CI results. Publishing creates tag `X.Y.Z` and triggers the
CI workflow responsible for publishing the NuGet packages.
Do not create the tag or publish the release as part of this skill.
