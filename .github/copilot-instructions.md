This is a .NET based repository containing recipes for the Cake Build Automation System bundling individual Cake Issues addins.
The individual recipes are published as NuGet packages.

See https://cakeissues.net/ for documentation and more information about Cake Issues.

Follow these guidelines when contributing:

## Before committing code
- Ensure no warning or error messages from Roslyn analyzers are present in the code.
- Ensure Unit Tests are passing.
- Ensure Integration Tests are passing.

## Development Flow
- Only building: `build.sh --target=DotNetCore-Build`
- Publish NuGet Packages: `build.sh --target=Create-NuGet-Packages`
- Run Unit Tests: `build.sh --target=Test`
- Run Integration Tests:
  - Run first `build.sh --target=Create-NuGet-Packages` in the root directory to create the NuGet packages.
  - Run `build.sh` in the `tests/<RECIPE-NAME>/<TEST>` directory to run the integration tests for the corresponding recipe
- Full CI check: `build.sh` (includes build, publish, test)

To have verbose output of build.sh the following parameter can be add `--verbosity=diagnostic`

## Repository Structure
- `Cake.Frosting.Issues.Recipe`: Contains the source code for the recipe for use with Cake Frosting
- `Cake.Issues.Recipe`: Contains the source code for the recipe for use with Cake Scripting
- `nuspec/nuget`: Contains the NuGet specification files for the Cake Scripting addin
- `tests`: Contains integration tests for the recipes
- `.github/workflows`: Contains GitHub Actions workflows for CI/CD which need to be maintained
- `.azuredevops/pipelines/templates`: Contains Azure Pipelines templates which need to be maintained. The main file is `azure-pipelines.yml` in the root directory.

## Key Guidelines
1. Maintain existing code structure and organization
2. Write unit tests for new functionality.
3. Write integration tests for new functionality which can't be tested with unit tests.
4. Ensure Cake.Issues.Recipe and Cake.Frosting.Issues.Recipe are kept in sync regarding the features they provide.
