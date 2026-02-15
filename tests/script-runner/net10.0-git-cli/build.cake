#load nuget:?package=Cake.Issues.Recipe&prerelease
#load "buildData.cake"

#addin "Cake.Markdownlint"

#tool nuget:?package=JetBrains.ReSharper.CommandLineTools&version=2023.1.2
#tool nuget:?package=MSBuild.Extension.Pack&version=1.9.1

//////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////

var target = Argument("target", "Default");

RepositoryInfoProvider = Argument("repositoryInfoProvider", RepositoryInfoProviderType.Cli);

//////////////////////////////////////////////////
// SETUP / TEARDOWN
//////////////////////////////////////////////////

Setup<BuildData>(setupContext =>
{
	return new BuildData(setupContext);
});

//////////////////////////////////////////////////
// TARGETS
//////////////////////////////////////////////////

Task("Configure-IssuesManagement")
    .Does<BuildData>((data) =>
{
    var platform = Context.Environment.Platform.Family.ToString();
    var runtime = Context.Environment.Runtime.IsCoreClr ? ".NET Core" : ".NET Framework";

    IssuesParameters.BuildIdentifier = $"{platform} ({runtime})";

    Information("Build identifier: {0}", IssuesParameters.BuildIdentifier);
});

Task("Build")
    .Does<BuildData>((data) =>
{
    var solutionFile =
        data.IssuesData.BuildRootDirectory
            .Combine("src")
            .CombineWithFilePath("ClassLibrary1.sln");

#if NETCOREAPP
    DotNetRestore(solutionFile.FullPath);

    var settings =
        new DotNetMSBuildSettings()
            .WithTarget("Rebuild")
            .WithLogger(
                "BinaryLogger," + Context.Tools.Resolve("Cake.Issues.MsBuild*/**/StructuredLogger.dll"),
                "",
                data.MsBuildLogFilePath.FullPath
            );

    DotNetBuild(
        solutionFile.FullPath,
        new DotNetBuildSettings
        {
            MSBuildSettings = settings
        });
#else
    NuGetRestore(solutionFile);

    var settings =
        new MSBuildSettings()
            .WithTarget("Rebuild")
            .WithLogger(
                Context.Tools.Resolve("Cake.Issues.MsBuild*/**/StructuredLogger.dll").FullPath,
                "BinaryLogger",
                data.MsBuildLogFilePath.FullPath);
    MSBuild(solutionFile, settings);
#endif

    // Pass path to MsBuild log file to Cake.Issues.Recipe
    IssuesParameters.InputFiles.AddMsBuildBinaryLogFilePath(data.MsBuildLogFilePath);
});

Task("Run-InspectCode")
    .WithCriteria((context) => context.IsRunningOnWindows(), "InspectCode is only supported on Windows.")
    .Does<BuildData>((data) =>
{
    var settings = new InspectCodeSettings() {
        OutputFile = data.InspectCodeLogFilePath,
        ArgumentCustomization = x => x.Append("--no-build")
    };

    InspectCode(
        data.IssuesData.BuildRootDirectory.Combine("src").CombineWithFilePath("ClassLibrary1.sln"),
        settings);

    // Pass path to InspectCode log file to Cake.Issues.Recipe
    IssuesParameters.InputFiles.AddInspectCodeLogFilePath(data.InspectCodeLogFilePath);
});

Task("Lint-Documentation")
    .Does<BuildData>((data) =>
{
    var settings =
        MarkdownlintNodeJsRunnerSettings.ForDirectory(
            data.IssuesData.BuildRootDirectory.Combine("docs"));
    settings.OutputFile = data.MarkdownlintCliLogFilePath;
    settings.ThrowOnIssue = false;
    RunMarkdownlintNodeJs(settings);

    // Pass path to markdownlint-cli log file to Cake.Issues.Recipe
    IssuesParameters.InputFiles.AddMarkdownlintCliLogFilePath(data.MarkdownlintCliLogFilePath);
});

Task("Lint")
    .IsDependentOn("Run-InspectCode")
    .IsDependentOn("Lint-Documentation");

// Make sure build and linters run before issues task.
IssuesBuildTasks.ReadIssuesTask
    .IsDependentOn("Build")
    .IsDependentOn("Lint");

// Run issues task by default.
Task("Default")
    .IsDependentOn("Configure-IssuesManagement")
    .IsDependentOn("Issues");

//////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////

RunTarget(target);