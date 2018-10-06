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
    .IsDependentOn("Report-IssuesToPullRequest");

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

IssuesBuildTasks.ReportIssuesToPullRequestTask = Task("Report-IssuesToPullRequest")
    .IsDependentOn("Read-Issues")
    .IsDependentOn("Report-IssuesToAzureDevOpsPullRequest");

IssuesBuildTasks.ReportIssuesToAzureDevOpsPullRequestTask = Task("Report-IssuesToAzureDevOpsPullRequest")
    .IsDependentOn("Read-Issues")
    .WithCriteria<IssuesData>((context, data) => data.IsRunningOnAzureDevOps, "Not running on Azure DevOps")
    .WithCriteria<IssuesData>((context, data) => data.IsPullRequestBuild, "Not a pull request build")
    .WithCriteria((context) => !string.IsNullOrWhiteSpace(context.EnvironmentVariable("SYSTEM_ACCESSTOKEN")), "SYSTEM_ACCESSTOKEN environment variable not set. Make sure the 'Allow Scripts to access OAuth token' option is enabled on the build definition.")
    .Does<IssuesData>((data) =>
{
    ReportIssuesToPullRequest(
        data.Issues,
        TfsPullRequests(
            data.RepositoryUrl,
            data.PullRequestId,
            TfsAuthenticationOAuth(EnvironmentVariable("SYSTEM_ACCESSTOKEN"))),
        data.RepositoryRootDirectory);
});