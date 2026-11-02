# Contribution Guidelines

This repository uses [GitFlow] with default configuration.
Development is happening on `develop` branch.

To contribute:

* Fork this repository.
* Create a feature branch from `develop`.
* Implement your changes.
* Push your feature branch.
* Create a pull request.

## Build

To build this package we are using Cake.

On Windows PowerShell run:

```powershell
./build
```

On OSX/Linux run:

```bash
./build.sh
```

## Release

Both Cake.Issues.Recipe and Cake.Frosting.Issues.Recipe are released together using
[agent skills] in `.github/skills`, following the GitFlow branching model and the
[Cake.Recipe documentation]. The skills replace the `git flow release start` and
`git flow release finish` commands; **they do not create tags or publish releases**.

### Prerequisites

* GitHub CLI installed and authenticated (`gh auth login`).
* Permission to push to `release/*`, `master`, and `develop`, and to create GitHub releases.
* A milestone named after the version (`X.Y.Z`) containing the release's issues and PRs,
  with labels matching `GitReleaseManager.yaml`.
* A clean working tree and the build prerequisites described in the repository instructions.

### Start a release

Ask the agent to `Start release X.Y.Z` to use the
[start-release](.github/skills/start-release/SKILL.md) skill. It creates
`release/X.Y.Z` from the latest remote `develop`, generates a GitHub release draft from the
milestone, and adds a separate `Update release notes link` commit for both recipe packages
using [update-release-notes-link](.github/skills/update-release-notes-link/SKILL.md).
It pushes the release branch after confirmation.

Review the draft and release-branch CI. Fixes may be added to the release branch;
assign their issues or PRs to the release milestone. Unlike the Cake.Issues repository,
this repository has no website news post or pinned documentation example version to update.

### Finish a release

Ask the agent to `Finish release X.Y.Z` to use the
[finish-release](.github/skills/finish-release/SKILL.md) skill. It checks release notes,
milestone coverage, package metadata, and CI before merging after confirmation:

1. `release/X.Y.Z` into `master` with `Merge branch 'release/X.Y.Z'`.
2. `master` into `develop` with `Merge branch 'master' into develop`.

Both merges use `--no-ff`, preserving the history used by earlier releases.
Release branch deletion requires separate confirmation after both remote merges are verified.

### Publish the release

After reviewing the new CI results, publish the draft on the [GitHub releases page] with
`master` as target. Publishing the release creates the `X.Y.Z` tag, which triggers the build
that publishes the NuGet packages. Do not create the tag during release preparation or merging.

[agent skills]: https://docs.github.com/copilot/concepts/agents/about-agent-skills
[GitHub releases page]: https://github.com/cake-contrib/Cake.Issues.Recipe/releases

[GitFlow]: (http://nvie.com/posts/a-successful-git-branching-model/)
[Cake.Recipe documentation]: https://cake-contrib.github.io/Cake.Recipe/docs/usage/creating-release