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

## Frosting integration tests

Create packages with `./build.ps1 --target=Create-NuGet-Packages` on Windows
or `./build.sh --target=Create-NuGet-Packages` on macOS/Linux before running
an integration sample under `tests/frosting` with its build script.

The Frosting samples map `Cake.Frosting.Issues.Recipe` exclusively to
`BuildArtifacts/Packages/NuGet` so they test the locally built package even
when NuGet.org has a newer version. Other dependencies are restored from
NuGet.org. Generated reports are written to the sample's `BuildArtifacts`
directory, alongside its `build` directory.

## Release

See [Cake.Recipe documentation] how to create a new release of this addin.

[GitFlow]: (http://nvie.com/posts/a-successful-git-branching-model/)
[Cake.Recipe documentation]: https://cake-contrib.github.io/Cake.Recipe/docs/usage/creating-release