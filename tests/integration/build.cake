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
    if (IsRunningOnWindows())
    {
        IssuesParameters.BuildIdentifier = "Windows";
    }
    else if (IsRunningOnUnix())
    {
        IssuesParameters.BuildIdentifier = "Unix";
    }
    else
    {
        IssuesParameters.BuildIdentifier = "Unknown platform";
    }
});

Task("Build")
    .Does<BuildData>((data) =>
{
    var solutionFile =
        data.IssuesData.RepositoryRootDirectory
            .Combine("src")
            .CombineWithFilePath("ClassLibrary1.sln");

    Information("Restoring NuGet package for {0}...", solutionFile);
    NuGetRestore(solutionFile);

    var settings =
        new MSBuildSettings()
            .WithTarget("Rebuild");

    // XML File Logger
    settings =
        settings.WithLogger(
            Context.Tools.Resolve("MSBuild.ExtensionPack.Loggers.dll").FullPath,
            "XmlFileLogger",
            string.Format(
                "logfile=\"{0}\";verbosity=Detailed;encoding=UTF-8",
                data.MsBuildLogFilePath));

    Information("Creating directory {0}...", IssuesParameters.OutputDirectory);
    EnsureDirectoryExists(IssuesParameters.OutputDirectory);

    Information("Building {0}...", solutionFile);
    MSBuild(solutionFile, settings);

    // Pass path to MsBuild log file to Cake.Issues.Recipe
    IssuesParameters.InputFiles.MsBuildXmlFileLoggerLogFilePath = data.MsBuildLogFilePath;
});

Task("Run-InspectCode")
    .WithCriteria((context) => context.IsRunningOnWindows(), "InspectCode is only supported on Windows.")
    .Does<BuildData>((data) =>
{
    var settings = new InspectCodeSettings() {
        OutputFile = data.InspectCodeLogFilePath
    };

    InspectCode(
        data.IssuesData.RepositoryRootDirectory.Combine("src").CombineWithFilePath("ClassLibrary1.sln"),
        settings);

    // Pass path to InspectCode log file to Cake.Issues.Recipe
    IssuesParameters.InputFiles.InspectCodeLogFilePath = data.InspectCodeLogFilePath;
});

Task("Lint-Documentation")
    .Does<BuildData>((data) =>
{
    var settings =
        MarkdownlintNodeJsRunnerSettings.ForDirectory(
            data.IssuesData.RepositoryRootDirectory.Combine("docs"));
    settings.OutputFile = data.MarkdownlintCliLogFilePath;
    settings.ThrowOnIssue = false;
    RunMarkdownlintNodeJs(settings);

    // Pass path to markdownlint-cli log file to Cake.Issues.Recipe
    IssuesParameters.InputFiles.MarkdownlintCliLogFilePath = data.MarkdownlintCliLogFilePath;
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