#load data/data.cake
#load parameters/parameters.cake

///////////////////////////////////////////////////////////////////////////////
// GLOBAL VARIABLES
///////////////////////////////////////////////////////////////////////////////

/// <summary>
/// Object for accessing the tasks provided by this script.
/// </summary>
var IssuesBuildTasks = new IssuesBuildTaskDefinitions();

///////////////////////////////////////////////////////////////////////////////
// SETUP / TEARDOWN
///////////////////////////////////////////////////////////////////////////////

Setup<IssuesData>(setupContext =>
{
    Information("Initializing Cake.Issues.Recipe (Version {0})...", BuildMetaData.Version);
    return new IssuesData(setupContext);
});

///////////////////////////////////////////////////////////////////////////////
// TASK DEFINITIONS
///////////////////////////////////////////////////////////////////////////////

IssuesBuildTasks.IssuesTask = Task("Issues")
    .IsDependentOn("Publish-IssuesArtifacts")
    .IsDependentOn("Report-IssuesToPullRequest")
    .IsDependentOn("Set-PullRequestIssuesState");

IssuesBuildTasks.ReadIssuesTask = Task("Read-Issues")
    .Does<IssuesData>((data) =>
{
    var settings =
        new ReadIssuesSettings(data.RepositoryRootDirectory)
        {
            Format = IssueCommentFormat.Markdown
        };

    // Determine which issue providers should be used.
    var issueProviders = new List<IIssueProvider>();

    if (IssuesParameters.MsBuildXmlFileLoggerLogFilePath != null)
    {
        issueProviders.Add(
            MsBuildIssuesFromFilePath(
                IssuesParameters.MsBuildXmlFileLoggerLogFilePath,
                MsBuildXmlFileLoggerFormat));
    }

    // Read issues from log files.
    data.AddIssues(
        ReadIssues(
            issueProviders,
            settings));

    Information("{0} issues are found.", data.Issues.Count());
});

IssuesBuildTasks.CreateFullIssuesReportTask = Task("Create-FullIssuesReport")
    .WithCriteria(() => IssuesParameters.ShouldCreateFullIssuesReport, "Creating of full issues report is disabled")
    .IsDependentOn("Read-Issues")
    .Does<IssuesData>((data) =>
{
    data.FullIssuesReport = IssuesParameters.OutputDirectory.CombineWithFilePath("report.html");
    EnsureDirectoryExists(IssuesParameters.OutputDirectory);

    // Create HTML report using DevExpress template.
    var settings = 
        GenericIssueReportFormatSettings
            .FromEmbeddedTemplate(GenericIssueReportTemplate.HtmlDxDataGrid)
            .WithOption(HtmlDxDataGridOption.Theme, DevExtremeTheme.MaterialBlueLight);
    CreateIssueReport(
        data.Issues,
        GenericIssueReportFormat(settings),
        data.RepositoryRootDirectory,
        data.FullIssuesReport);
});

IssuesBuildTasks.PublishIssuesArtifactsTask = Task("Publish-IssuesArtifacts")
    .IsDependentOn("Create-FullIssuesReport")
    .IsDependentOn("Publish-AzureDevOpsIssuesArtifacts");

IssuesBuildTasks.ReportIssuesToPullRequestTask = Task("Report-IssuesToPullRequest")
    .WithCriteria(() => IssuesParameters.ShouldReportIssuesToPullRequest, "Reporting of issues to pull requests is disabled")
    .IsDependentOn("Read-Issues")
    .IsDependentOn("Report-IssuesToAzureDevOpsPullRequest");

IssuesBuildTasks.SetPullRequestIssuesStateTask = Task("Set-PullRequestIssuesState")
    .WithCriteria(() => IssuesParameters.ShouldSetPullRequestStatus, "Setting of pull request status is disabled")
    .IsDependentOn("Read-Issues")
    .IsDependentOn("Set-AzureDevOpsPullRequestIssuesState");

#load tasks/tasks.cake