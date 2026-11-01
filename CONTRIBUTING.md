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

Install the .NET 8, 9, and 10 SDKs at or above the versions specified in the
project and integration test `global.json` files. AppVeyor pins matching SDK
versions in `.appveyor.yml`; keep these pins aligned when updating `global.json`.
.NET 5 is also required by the build's GitVersion tool.

The PowerShell bootstraps check `$LASTEXITCODE` after each native command.
Native stderr output alone does not fail the build, since successful commands
can write warnings there; nonzero exit codes still stop the build.

On Windows PowerShell run:

```powershell
./build
```

On OSX/Linux run:

```bash
./build.sh
```

## Release

See [Cake.Recipe documentation] how to create a new release of this addin.

[GitFlow]: (http://nvie.com/posts/a-successful-git-branching-model/)
[Cake.Recipe documentation]: https://cake-contrib.github.io/Cake.Recipe/docs/usage/creating-release