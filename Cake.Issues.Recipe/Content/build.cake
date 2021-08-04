#load addins.cake
#load IssuesBuildTasksDefinitions.cake
#load version.cake
#load data/data.cake
#load parameters/parameters.cake

///////////////////////////////////////////////////////////////////////////////
// GLOBAL VARIABLES
///////////////////////////////////////////////////////////////////////////////

/// <summary>
/// Object for accessing the tasks provided by this script.
/// </summary>
var IssuesBuildTasks = new IssuesBuildTaskDefinitions();

/// <summary>
/// Defines how information about the Git repository should be determined.
/// </summary>
var RepositoryInfoProvider = RepositoryInfoProviderType.CakeGit;

///////////////////////////////////////////////////////////////////////////////
// SETUP / TEARDOWN
///////////////////////////////////////////////////////////////////////////////

Setup<IssuesData>(setupContext =>
{
    Information("Initializing Cake.Issues.Recipe (Version {0})...", BuildMetaDataCakeIssuesRecipe.Version);
    return new IssuesData(setupContext, RepositoryInfoProvider);
});

///////////////////////////////////////////////////////////////////////////////
// TASK DEFINITIONS
///////////////////////////////////////////////////////////////////////////////

IssuesBuildTasks.IssuesTask = Task("Issues")
    .Description("Main tasks for issue management integration.")
    .IsDependentOn("Publish-IssuesArtifacts")
    .IsDependentOn("Report-IssuesToBuildServer")
    .IsDependentOn("Create-SummaryIssuesReport")
    .IsDependentOn("Report-IssuesToPullRequest")
    .IsDependentOn("Set-PullRequestIssuesState");

IssuesBuildTasks.ReadIssuesTask = Task("Read-Issues")
    .Description("Reads issues from the provided log files.")
    .Does<IssuesData>((data) =>
{
    // Define default settings.
    var defaultSettings = new ReadIssuesSettings(data.BuildRootDirectory);

    if (data.PullRequestSystem != null)
    {
        defaultSettings.FileLinkSettings =
            data.PullRequestSystem.GetFileLinkSettings(Context, data);
    }

    // Read MSBuild log files created by XmlFileLogger.
    foreach (var logFile in IssuesParameters.InputFiles.MsBuildXmlFileLoggerLogFilePaths)
    {
        data.AddIssues(
            ReadIssues(
                MsBuildIssuesFromFilePath(
                    logFile.Key,
                    MsBuildXmlFileLoggerFormat),
                GetSettings(logFile.Value, defaultSettings)));
    }

    // Read MSBuild binary log files.
    foreach (var logFile in IssuesParameters.InputFiles.MsBuildBinaryLogFilePaths)
    {
        data.AddIssues(
            ReadIssues(
                MsBuildIssuesFromFilePath(
                    logFile.Key,
                    MsBuildBinaryLogFileFormat),
                GetSettings(logFile.Value, defaultSettings)));
    }

    // Read InspectCode log files.
    foreach (var logFile in IssuesParameters.InputFiles.InspectCodeLogFilePaths)
    {
        data.AddIssues(
            ReadIssues(
                InspectCodeIssuesFromFilePath(logFile.Key),
                GetSettings(logFile.Value, defaultSettings)));
    }

    // Read dupFinder log files.
    foreach (var logFile in IssuesParameters.InputFiles.DupFinderLogFilePaths)
    {
        data.AddIssues(
            ReadIssues(
                DupFinderIssuesFromFilePath(logFile.Key),
                GetSettings(logFile.Value, defaultSettings)));
    }

    // Read markdownlint-cli log files.
    foreach (var logFile in IssuesParameters.InputFiles.MarkdownlintCliLogFilePaths)
    {
        data.AddIssues(
            ReadIssues(
                MarkdownlintIssuesFromFilePath(
                    logFile.Key,
                    MarkdownlintCliLogFileFormat),
                GetSettings(logFile.Value, defaultSettings)));
    }

    // Read markdownlint log files in version 1.
    foreach (var logFile in IssuesParameters.InputFiles.MarkdownlintV1LogFilePaths)
    {
        data.AddIssues(
            ReadIssues(
                MarkdownlintIssuesFromFilePath(
                    logFile.Key,
                    MarkdownlintV1LogFileFormat),
                GetSettings(logFile.Value, defaultSettings)));
    }

    // Read ESLint log files in JSON format.
    foreach (var logFile in IssuesParameters.InputFiles.EsLintJsonLogFilePaths)
    {
        data.AddIssues(
            ReadIssues(
                EsLintIssuesFromFilePath(
                    logFile.Key,
                    EsLintJsonFormat),
                GetSettings(logFile.Value, defaultSettings)));
    }

    Information("{0} issues are found.", data.Issues.Count());
});

IssuesBuildTasks.CreateFullIssuesReportTask = Task("Create-FullIssuesReport")
    .Description("Creates issue report.")
    .WithCriteria(() => IssuesParameters.Reporting.ShouldCreateFullIssuesReport, "Creating of full issues report is disabled")
    .IsDependentOn("Read-Issues")
    .Does<IssuesData>((data) =>
{
    var reportFileName = "report";
    if (!string.IsNullOrWhiteSpace(IssuesParameters.BuildIdentifier))
    {
        reportFileName += $"-{IssuesParameters.BuildIdentifier}";
    }
    reportFileName += ".html";

    data.FullIssuesReport =
        IssuesParameters.OutputDirectory.CombineWithFilePath(reportFileName);
    EnsureDirectoryExists(IssuesParameters.OutputDirectory);

    // Create HTML report using DevExpress template.
    var settings =
        GenericIssueReportFormatSettings
            .FromEmbeddedTemplate(GenericIssueReportTemplate.HtmlDxDataGrid)
            .WithOption(HtmlDxDataGridOption.Theme, DevExtremeTheme.MaterialBlueLight);
    CreateIssueReport(
        data.Issues,
        GenericIssueReportFormat(settings),
        data.BuildRootDirectory,
        data.FullIssuesReport);
});

IssuesBuildTasks.PublishIssuesArtifactsTask = Task("Publish-IssuesArtifacts")
    .Description("Publish issue artifacts to build server.")
    .WithCriteria(() => !BuildSystem.IsLocalBuild, "Not running on build server")
    .IsDependentOn("Create-FullIssuesReport")
    .Does<IssuesData>((data) =>
{
    if (data.BuildServer == null)
    {
        Information("Not supported build server.");
        return;
    }

    data.BuildServer.PublishIssuesArtifacts(Context, data);
});

IssuesBuildTasks.ReportIssuesToBuildServerTask = Task("Report-IssuesToBuildServer")
    .Description("Report issues to build server.")
    .WithCriteria(() => !BuildSystem.IsLocalBuild, "Not running on build server")
    .WithCriteria(() => IssuesParameters.BuildServer.ShouldReportIssuesToBuildServer, "Reporting of issues to build server is disabled")
    .IsDependentOn("Read-Issues")
    .Does<IssuesData>((data) =>
{
    if (data.BuildServer == null)
    {
        Information("Not supported build server.");
        return;
    }

    data.BuildServer.ReportIssuesToBuildServer(Context, data);
});

IssuesBuildTasks.CreateSummaryIssuesReportTask = Task("Create-SummaryIssuesReport")
    .Description("Creates a summary issue report.")
    .WithCriteria(() => !BuildSystem.IsLocalBuild, "Not running on build server")
    .WithCriteria(() => IssuesParameters.BuildServer.ShouldCreateSummaryIssuesReport, "Creating of summary issues report is disabled")
    .IsDependentOn("Read-Issues")
    .Does<IssuesData>((data) =>
{
    if (data.BuildServer == null)
    {
        Information("Not supported build server.");
        return;
    }

    data.BuildServer.CreateSummaryIssuesReport(Context, data);
});

IssuesBuildTasks.ReportIssuesToPullRequestTask = Task("Report-IssuesToPullRequest")
    .Description("Report issues to pull request.")
    .WithCriteria(() => !BuildSystem.IsLocalBuild, "Not running on build server")
    .WithCriteria(() => IssuesParameters.PullRequestSystem.ShouldReportIssuesToPullRequest, "Reporting of issues to pull requests is disabled")
    .WithCriteria<IssuesData>((context, data) => data.BuildServer != null ? data.BuildServer.DetermineIfPullRequest(context) : false, "Not a pull request build")
    .IsDependentOn("Read-Issues")
    .Does<IssuesData>((data) =>
{
    if (data.PullRequestSystem == null)
    {
        Information("Not supported pull request system.");
        return;
    }

    data.PullRequestSystem.ReportIssuesToPullRequest(Context, data);
});

IssuesBuildTasks.SetPullRequestIssuesStateTask = Task("Set-PullRequestIssuesState")
    .Description("Set pull request status.")
    .WithCriteria(() => !BuildSystem.IsLocalBuild, "Not running on build server")
    .WithCriteria(() => IssuesParameters.PullRequestSystem.ShouldSetPullRequestStatus, "Setting of pull request status is disabled")
    .WithCriteria<IssuesData>((context, data) => data.BuildServer != null ? data.BuildServer.DetermineIfPullRequest(context) : false, "Not a pull request build")
    .IsDependentOn("Read-Issues")
    .Does<IssuesData>((data) =>
{
    if (data.PullRequestSystem == null)
    {
        Information("Not supported pull request system.");
        return;
    }

    data.PullRequestSystem.SetPullRequestIssuesState(Context, data);
});

///////////////////////////////////////////////////////////////////////////////
// HELPER Functions
///////////////////////////////////////////////////////////////////////////////

private IReadIssuesSettings GetSettings(IReadIssuesSettings configuredSettings, IReadIssuesSettings defaultSettings)
{
    if (configuredSettings == null)
    {
        return defaultSettings;
    }

    if (configuredSettings.FileLinkSettings == null)
    {
        configuredSettings.FileLinkSettings = defaultSettings.FileLinkSettings;
    }

    return configuredSettings;
}

#load tasks/tasks.cake
