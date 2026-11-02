---
name: update-release-notes-link
description: Update the GitHub release notes URLs for both Cake Issues Recipe packages. Use when preparing a release or updating release notes links in the scripting nuspec and Frosting project.
---

Point both packages to the GitHub release for the version being prepared.
This skill only edits package metadata; it does not create releases or tags.

## Inputs

The version in the format `X.Y.Z` (for example `6.1.0`).
Use the version from the current `release/X.Y.Z` branch when available.
Otherwise ask for the version; do not infer it from bundled Cake Issues dependencies.

## Files to update

| File | XML element |
|------|-------------|
| [Cake.Issues.Recipe.nuspec](../../../nuspec/nuget/Cake.Issues.Recipe.nuspec) | `package/metadata/releaseNotes` |
| [Cake.Frosting.Issues.Recipe.csproj](../../../Cake.Frosting.Issues.Recipe/Cake.Frosting.Issues.Recipe/Cake.Frosting.Issues.Recipe.csproj) | `Project/PropertyGroup/PackageReleaseNotes` |

Replace only the version at the end of each URL with the requested version:

```text
https://github.com/cake-contrib/Cake.Issues.Recipe/releases/tag/X.Y.Z
```

Read existing contents before editing. Require exactly one matching metadata element per file;
if it is missing, duplicated, or points to another repository, stop and report it rather than
silently succeeding. Preserve each file's encoding, byte order mark (if any), line endings,
indentation, and final newline. Avoid serializing the entire XML document, which can reformat it.
If a URL already matches, leave that file unchanged.

Do not change `<version>0.0.0</version>`, package dependencies, target frameworks, or generated files.

## Validation

* Both files must contain exactly one release notes element with the exact requested URL.
* `git diff --check` must pass.
* Review `git diff --` for both files: only the release notes URL lines may change.
* Do not build or create packages solely to validate these metadata-only edits.

## Commit

When invoked by [start-release](../start-release/SKILL.md), let that skill stage only these two
files and create the separate `Update release notes link` commit.
For standalone use, follow the user's requested branch/PR workflow; do not push or commit unrelated edits.
