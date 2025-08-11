# Cake.Issues.Recipe

Cake.Issues.Recipe is a .NET repository containing recipes for the Cake Build Automation System bundling individual Cake Issues addins. The individual recipes are published as NuGet packages. The repository provides recipes for both Cake Scripting and Cake Frosting.

Always reference these instructions first and fallback to search or bash commands only when you encounter unexpected information that does not match the info here.

## Working Effectively

### Prerequisites
- Install .NET 8 and .NET 9 SDKs
- For integration tests: Install `markdownlint-cli` globally: `npm install -g markdownlint-cli`

### Bootstrap, build, and test the repository
- Make build script executable: `chmod +x build.sh`
- Full build and test: `./build.sh` -- takes 40 seconds. NEVER CANCEL. Set timeout to 120+ seconds.
- Build only: `./build.sh --target=DotNetCore-Build` -- takes 2.5 minutes. NEVER CANCEL. Set timeout to 300+ seconds.
- Run tests: `./build.sh --target=Test` -- takes 30 seconds. NEVER CANCEL. Set timeout to 120+ seconds.
- Create NuGet packages: `./build.sh --target=Create-NuGet-Packages` -- takes 30 seconds. NEVER CANCEL. Set timeout to 120+ seconds.

### Check available build targets
- View all targets: `./build.sh --tree`
- Get help: `./build.sh --help`
- Verbose output: `./build.sh --verbosity=diagnostic`

### Run integration tests
- ALWAYS run `./build.sh --target=Create-NuGet-Packages` first in the root directory to create NuGet packages
- Run specific integration test:
  - `cd tests/script-runner/net8.0-git-cli/`
  - `chmod +x build.sh && ./build.sh` -- takes 20 seconds. NEVER CANCEL. Set timeout to 120+ seconds.
- Integration tests validate the recipes work correctly with sample projects

## Validation

### Build validation steps
- ALWAYS run the full build after making changes: `./build.sh`
- Run unit tests: `./build.sh --target=Test`
- For recipe changes, ALWAYS run integration tests to ensure recipes work correctly
- Check build artifacts in `BuildArtifacts/Packages/NuGet/` after package creation

### Code quality requirements
- Ensure no warning or error messages from Roslyn analyzers are present
- Unit tests must pass
- Integration tests must pass
- Build artifacts must be created successfully

## Repository Structure

### Key projects
- `Cake.Issues.Recipe/`: Contains the source code for the recipe for use with Cake Scripting
- `Cake.Frosting.Issues.Recipe/`: Contains the source code for the recipe for use with Cake Frosting
  - `Cake.Frosting.Issues.Recipe.csproj`: Main Frosting recipe project
  - `Cake.Frosting.Issues.Recipe.Tests.csproj`: Unit tests for Frosting recipe
- `nuspec/nuget/`: Contains NuGet specification files for the Cake Scripting addin
- `tests/`: Contains integration tests for both recipes
  - `script-runner/`: Integration tests for Cake Scripting recipe
  - `frosting/`: Integration tests for Cake Frosting recipe
- `.github/workflows/`: GitHub Actions workflows for CI/CD
- `.azuredevops/pipelines/`: Azure Pipelines templates
- `azure-pipelines.yml`: Main Azure DevOps pipeline configuration

### Build system
- Uses Cake.Recipe (meta-build system for Cake addins)
- Main build configuration: `recipe.cake`
- Build scripts: `build.sh` (Linux/macOS), `build.ps1` (Windows)
- Uses GitVersion for semantic versioning

### Common commands reference

#### Repository root contents
```
.appveyor.yml
.azuredevops/
.config/
.github/
.gitignore
.vscode/
CONTRIBUTING.md
Cake.Frosting.Issues.Recipe/
Cake.Issues.Recipe/
GitReleaseManager.yaml
LICENSE
README.md
azure-pipelines.yml
build.ps1
build.sh
nuspec/
recipe.cake
tests/
```

#### Available build targets (most commonly used)
- `Build`: Build all projects
- `Test`: Run unit tests
- `Package`: Create NuGet packages and run full validation
- `Create-NuGet-Packages`: Create NuGet packages only
- `DotNetCore-Build`: Build without tests or packaging
- `DotNetCore-Restore`: Restore NuGet packages only

#### Integration test structure
- Each test has variants for .NET 8/9 and different repository providers (git-cli, cake-git)
- Tests contain sample projects that demonstrate recipe functionality
- Tests validate issues are detected and reported correctly

## Development Guidelines

### Key principles
1. Maintain existing code structure and organization
2. Write unit tests for new functionality
3. Write integration tests for functionality that can't be tested with unit tests
4. Ensure Cake.Issues.Recipe and Cake.Frosting.Issues.Recipe are kept in sync regarding features

### Making changes
- ALWAYS test both unit tests and integration tests after changes
- Both Cake Scripting and Cake Frosting recipes should provide equivalent functionality
- Recipe changes often require updates to both variants
- Integration tests must pass to ensure recipes work in real scenarios

### CI/CD compatibility
- The build runs on Windows, macOS, and Ubuntu across multiple .NET versions
- GitHub Actions and Azure DevOps are both used for CI
- AppVeyor is also used for some builds

### Common tasks timing reference
- Tool restoration: ~5 seconds
- .NET restore: ~30 seconds  
- Build (first time): ~2.5 minutes
- Build (incremental): ~10 seconds
- Unit tests: ~30 seconds
- NuGet package creation: ~30 seconds
- Integration test: ~20 seconds per test
- Full build: ~40 seconds

### Troubleshooting
- If markdownlint fails in integration tests: Install `markdownlint-cli` with `npm install -g markdownlint-cli`
- Build warnings about deprecated packages are expected and can be ignored
- Some assembly load warnings during build are expected and can be ignored
- Integration tests create sample code with intentional issues for validation
