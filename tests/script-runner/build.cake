#load nuget:?package=Cake.Issues.Recipe&prerelease
#load "buildData.cake"

#addin "Cake.Markdownlint"

#tool "nuget:?package=JetBrains.ReSharper.CommandLineTools"
#tool "nuget:?package=MSBuild.Extension.Pack"

//////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////

var target = Argument("target", "Default");

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
    DotNetCoreRestore(solutionFile.FullPath);

    var settings =
        new DotNetCoreMSBuildSettings()
            .WithTarget("Rebuild")
            .WithLogger(
                "BinaryLogger," + Context.Tools.Resolve("Cake.Issues.MsBuild*/**/StructuredLogger.dll"),
                "",
                data.MsBuildLogFilePath.FullPath
            );

    DotNetCoreBuild(
        solutionFile.FullPath,
        new DotNetCoreBuildSettings
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
    IssuesParameters.InputFiles.AddMsBuildBinaryLogFile(data.MsBuildLogFilePath);
});

Task("Run-InspectCode")
    .WithCriteria((context) => context.IsRunningOnWindows(), "InspectCode is only supported on Windows.")
    .Does<BuildData>((data) =>
{
    var settings = new InspectCodeSettings() {
        OutputFile = data.InspectCodeLogFilePath
    };

    InspectCode(
        data.IssuesData.BuildRootDirectory.Combine("src").CombineWithFilePath("ClassLibrary1.sln"),
        settings);

    // Pass path to InspectCode log file to Cake.Issues.Recipe
    IssuesParameters.InputFiles.AddInspectCodeLogFile(data.InspectCodeLogFilePath);
});

Task("Run-DupFinder")
    .WithCriteria((context) => context.IsRunningOnWindows(), "DupFinder is only supported on Windows.")
    .Does<BuildData>((data) =>
{
    var settings = new DupFinderSettings() {
        OutputFile = data.DupFinderLogFilePath
    };

    DupFinder(
        data.IssuesData.BuildRootDirectory.Combine("src").CombineWithFilePath("ClassLibrary1.sln"),
        settings);

    // Pass path to dupFinder log file to Cake.Issues.Recipe
    IssuesParameters.InputFiles.AddDupFinderLogFile(data.DupFinderLogFilePath);
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
    IssuesParameters.InputFiles.AddMarkdownlintCliLogFile(data.MarkdownlintCliLogFilePath);
});

Task("Lint")
    .IsDependentOn("Run-InspectCode")
    .IsDependentOn("Run-DupFinder")
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