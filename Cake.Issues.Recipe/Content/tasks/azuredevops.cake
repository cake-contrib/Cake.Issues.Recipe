IssuesBuildTasks.PublishAzureDevOpsIssuesArtifactsTask = Task("Publish-AzureDevOpsIssuesArtifacts")
    .IsDependentOn("Create-FullIssuesReport")
    .WithCriteria<IssuesData>((context, data) => data.IsRunningOnAzureDevOps, "Not running on Azure DevOps")
    .Does<IssuesData>((data) =>
{
    if (IssuesParameters.ShouldPublishFullIssuesReport &&
        data.FullIssuesReport != null &&
        FileExists(data.FullIssuesReport))
    {
        TFBuild.Commands.UploadArtifact("Issues", data.FullIssuesReport, "Issues");
    }
});

IssuesBuildTasks.ReportIssuesToAzureDevOpsPullRequestTask = Task("Report-IssuesToAzureDevOpsPullRequest")
    .IsDependentOn("Read-Issues")
    .WithCriteria(() => IssuesParameters.ShouldReportIssuesToPullRequest, "Reporting of issues to pull requests is disabled")
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

IssuesBuildTasks.SetAzureDevOpsPullRequestIssuesStateTask = Task("Set-AzureDevOpsPullRequestIssuesState")
    .IsDependentOn("Read-Issues")
    .WithCriteria(() => IssuesParameters.ShouldSetPullRequestStatus, "Setting of pull request status is disabled")
    .WithCriteria<IssuesData>((context, data) => data.IsRunningOnAzureDevOps, "Not running on Azure DevOps")
    .WithCriteria<IssuesData>((context, data) => data.IsPullRequestBuild, "Not a pull request build")
    .WithCriteria((context) => !string.IsNullOrWhiteSpace(context.EnvironmentVariable("SYSTEM_ACCESSTOKEN")), "SYSTEM_ACCESSTOKEN environment variable not set. Make sure the 'Allow Scripts to access OAuth token' option is enabled on the build definition.")
    .Does<IssuesData>((data) =>
{
    var pullRequestSettings =
        new TfsPullRequestSettings(
            data.RepositoryUrl,
            data.PullRequestId,
            TfsAuthenticationOAuth(EnvironmentVariable("SYSTEM_ACCESSTOKEN")));

    var pullRequstStatus =
        new TfsPullRequestStatus("Issues")
        {
            Genre = "Cake.Issues.Recipe",
            State = data.Issues.Any() ? TfsPullRequestStatusState.Failed : TfsPullRequestStatusState.Succeeded,
            Description = data.Issues.Any() ? $"Found {data.Issues.Count()} issues" : "No issues found"
        };

    TfsSetPullRequestStatus(
        pullRequestSettings,
        pullRequstStatus);
});