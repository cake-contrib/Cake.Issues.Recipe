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
    .IsDependentOn("Set-PullRequestIssuesState")
    .IsDependentOn("Report-IssuesToConsole")
    .Does<IssuesData>((data) =>
{
    if (IssuesParameters.BuildBreaking.ShouldFailBuildOnIssues)
    {
        BreakBuildOnIssues(
            data.Issues,
            new BuildBreakingSettings
            {
                MinimumPriority = IssuesParameters.BuildBreaking.MinimumPriority,
                IssueProvidersToConsider = IssuesParameters.BuildBreaking.IssueProvidersToConsider,
                IssueProvidersToIgnore = IssuesParameters.BuildBreaking.IssueProvidersToIgnore
            },
            x =>
            {
                // Print issues to console before failing build.
                _ = CreateIssueReport(
                    x,
                    ConsoleIssueReportFormat(
                        new ConsoleIssueReportFormatSettings
                        {
                            Compact = true,
                            GroupByRule = true,
                        }),
                    data.ProjectRootDirectory,
                    string.Empty);
            });
    }
});

IssuesBuildTasks.ReadIssuesTask = Task("Read-Issues")
    .Description("Reads issues from the provided log files.")
    .Does<IssuesData>((data) =>
{
    // Read MSBuild log files created by XmlFileLogger.
    foreach (var logFile in IssuesParameters.InputFiles.MsBuildXmlFileLoggerLogFilePaths)
    {
        data.AddIssues(
            MsBuildIssuesFromFilePath(
                logFile.Key,
                MsBuildXmlFileLoggerFormat),
            logFile.Value);
    }

    // Read MSBuild log content created by XmlFileLogger.
    foreach (var logFileContent in IssuesParameters.InputFiles.MsBuildXmlFileLoggerLogFileContent)
    {
        data.AddIssues(
            MsBuildIssues(
                new MsBuildIssuesSettings(
                    logFileContent.Key, 
                    MsBuildXmlFileLoggerFormat)),
            logFileContent.Value);
    }

    // Read MSBuild binary log files.
    foreach (var logFile in IssuesParameters.InputFiles.MsBuildBinaryLogFilePaths)
    {
        data.AddIssues(
            MsBuildIssuesFromFilePath(
                logFile.Key,
                MsBuildBinaryLogFileFormat),
            logFile.Value);
    }

    // Read MSBuild binary log content.
    foreach (var logFileContent in IssuesParameters.InputFiles.MsBuildBinaryLogFileContent)
    {
        data.AddIssues(
            MsBuildIssues(
                new MsBuildIssuesSettings(
                    logFileContent.Key,
                    MsBuildBinaryLogFileFormat)),
            logFileContent.Value);
    }

    // Read InspectCode log files.
    foreach (var logFile in IssuesParameters.InputFiles.InspectCodeLogFilePaths)
    {
        data.AddIssues(
            InspectCodeIssuesFromFilePath(logFile.Key),
            logFile.Value);
    }

    // Read InspectCode log content.
    foreach (var logFileContent in IssuesParameters.InputFiles.InspectCodeLogFileContent)
    {
        data.AddIssues(
            InspectCodeIssues(
                new InspectCodeIssuesSettings(logFileContent.Key)),
            logFileContent.Value);
    }

    // Read markdownlint-cli log files.
    foreach (var logFile in IssuesParameters.InputFiles.MarkdownlintCliLogFilePaths)
    {
        data.AddIssues(
            MarkdownlintIssuesFromFilePath(
                logFile.Key,
                MarkdownlintCliLogFileFormat),
            logFile.Value);
    }

    // Read markdownlint-cli log content.
    foreach (var logFileContent in IssuesParameters.InputFiles.MarkdownlintCliLogFileContent)
    {
        data.AddIssues(
            MarkdownlintIssues(
                new MarkdownlintIssuesSettings(
                    logFileContent.Key,
                    MarkdownlintCliLogFileFormat)),
            logFileContent.Value);
    }

    // Read markdownlint-cli log files created with --json.
    foreach (var logFile in IssuesParameters.InputFiles.MarkdownlintCliJsonLogFilePaths)
    {
        data.AddIssues(
            MarkdownlintIssuesFromFilePath(
                logFile.Key,
                MarkdownlintCliJsonLogFileFormat),
            logFile.Value);
    }

    // Read markdownlint-cli log content created with --json.
    foreach (var logFileContent in IssuesParameters.InputFiles.MarkdownlintCliJsonLogFileContent)
    {
        data.AddIssues(
            MarkdownlintIssues(
                new MarkdownlintIssuesSettings(
                    logFileContent.Key,
                    MarkdownlintCliJsonLogFileFormat)),
            logFileContent.Value);
    }

    // Read markdownlint log files in version 1.
    foreach (var logFile in IssuesParameters.InputFiles.MarkdownlintV1LogFilePaths)
    {
        data.AddIssues(
            MarkdownlintIssuesFromFilePath(
                logFile.Key,
                MarkdownlintV1LogFileFormat),
            logFile.Value);
    }

    // Read markdownlint log content in version 1.
    foreach (var logFileContent in IssuesParameters.InputFiles.MarkdownlintV1LogFileContent)
    {
        data.AddIssues(
            MarkdownlintIssues(
                new MarkdownlintIssuesSettings(
                    logFileContent.Key,
                    MarkdownlintV1LogFileFormat)),
            logFileContent.Value);
    }

    // Read ESLint log files in JSON format.
    foreach (var logFile in IssuesParameters.InputFiles.EsLintJsonLogFilePaths)
    {
        data.AddIssues(
            EsLintIssuesFromFilePath(
                logFile.Key,
                EsLintJsonFormat),
            logFile.Value);
    }

    // Read ESLint log content in JSON format.
    foreach (var logFileContent in IssuesParameters.InputFiles.EsLintJsonLogFileContent)
    {
        data.AddIssues(
            EsLintIssues(
                new EsLintIssuesSettings(
                    logFileContent.Key,
                    EsLintJsonFormat)),
            logFileContent.Value);
    }

    // Read SARIF log files.
    foreach (var logFile in IssuesParameters.InputFiles.SarifLogFilePaths)
    {
        data.AddIssues(
            SarifIssues(
                new SarifIssuesSettings(logFile.Key)
                {
                    // Since there might be multiple SARIF log files we need to have a predictable
                    // issue provider name for reporting pull request states.
                    UseToolNameAsIssueProviderName = false
                }),
            logFile.Value);
    }

    // Read SARIF content.
    foreach (var logFileContent in IssuesParameters.InputFiles.SarifLogFileContent)
    {
        data.AddIssues(
            SarifIssues(
                new SarifIssuesSettings(logFileContent.Key)
                {
                    // Since there might be multiple SARIF log files we need to have a predictable
                    // issue provider name for reporting pull request states.
                    UseToolNameAsIssueProviderName = false
                }),
            logFileContent.Value);
    }

    // Read generic TAP log files.
    foreach (var logFile in IssuesParameters.InputFiles.GenericTapLogFilePaths)
    {
        data.AddIssues(
            TapIssues(
                new TapIssuesSettings(
                    logFile.Key,
                    GenericLogFileFormat)),
            logFile.Value);
    }

    // Read generic TAP content.
    foreach (var logFileContent in IssuesParameters.InputFiles.GenericTapLogFileContent)
    {
        data.AddIssues(
            TapIssues(
                new TapIssuesSettings(
                    logFileContent.Key,
                    GenericLogFileFormat)),
            logFileContent.Value);
    }

    // Read Stylelint TAP log files.
    foreach (var logFile in IssuesParameters.InputFiles.GenericTapLogFilePaths)
    {
        data.AddIssues(
            TapIssues(
                new TapIssuesSettings(
                    logFile.Key,
                    StylelintLogFileFormat)),
            logFile.Value);
    }

    // Read Stylelint TAP content.
    foreach (var logFileContent in IssuesParameters.InputFiles.GenericTapLogFileContent)
    {
        data.AddIssues(
            TapIssues(
                new TapIssuesSettings(
                    logFileContent.Key,
                    StylelintLogFileFormat)),
            logFileContent.Value);
    }

    // Read Textlint TAP log files.
    foreach (var logFile in IssuesParameters.InputFiles.GenericTapLogFilePaths)
    {
        data.AddIssues(
            TapIssues(
                new TapIssuesSettings(
                    logFile.Key,
                    TextlintLogFileFormat)),
            logFile.Value);
    }

    // Read Textlint TAP content.
    foreach (var logFileContent in IssuesParameters.InputFiles.GenericTapLogFileContent)
    {
        data.AddIssues(
            TapIssues(
                new TapIssuesSettings(
                    logFileContent.Key,
                    TextlintLogFileFormat)),
            logFileContent.Value);
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

    // Create HTML report.
    CreateIssueReport(
        data.Issues,
        GenericIssueReportFormat(IssuesParameters.Reporting.FullIssuesReportSettings),
        data.ProjectRootDirectory,
        data.FullIssuesReport);
});

IssuesBuildTasks.CreateSarifReportTask = Task("Create-SarifReport")
    .Description("Creates report in SARIF format.")
    .WithCriteria(() => IssuesParameters.Reporting.ShouldCreateSarifReport, "Creating of report in SARIF format is disabled")
    .IsDependentOn("Read-Issues")
    .Does<IssuesData>((data) =>
{
    var reportFileName = "report";
    if (!string.IsNullOrWhiteSpace(IssuesParameters.BuildIdentifier))
    {
        reportFileName += $"-{IssuesParameters.BuildIdentifier}";
    }
    reportFileName += ".sarif";

    data.SarifReport =
        IssuesParameters.OutputDirectory.CombineWithFilePath(reportFileName);
    EnsureDirectoryExists(IssuesParameters.OutputDirectory);

    // Create SARIF report.
    CreateIssueReport(
        data.Issues,
        SarifIssueReportFormat(),
        data.ProjectRootDirectory,
        data.SarifReport);
});

IssuesBuildTasks.PublishIssuesArtifactsTask = Task("Publish-IssuesArtifacts")
    .Description("Publish issue artifacts to build server.")
    .WithCriteria(() => !BuildSystem.IsLocalBuild, "Not running on build server")
    .IsDependentOn("Create-FullIssuesReport")
    .IsDependentOn("Create-SarifReport")
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
    .WithCriteria(() => 
        IssuesParameters.PullRequestSystem.ShouldSetPullRequestStatus || IssuesParameters.PullRequestSystem.ShouldSetSeparatePullRequestStatusForEachIssueProviderAndRun,
        "Setting of pull request status is disabled")
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

IssuesBuildTasks.ReportIssuesToConsoleTask = Task("Report-IssuesToConsole")
    .Description("Reports issues to the console.")
    .WithCriteria(() => IssuesParameters.Reporting.ShouldReportIssuesToConsole, "Reporting of issues to the console is disabled")
    .IsDependentOn("Read-Issues")
    .Does<IssuesData>((data) =>
{
    // Print issues to console.
    CreateIssueReport(
        data.Issues,
        ConsoleIssueReportFormat(IssuesParameters.Reporting.ReportToConsoleSettings),
        data.ProjectRootDirectory,
        string.Empty);
});

#load tasks/tasks.cake
