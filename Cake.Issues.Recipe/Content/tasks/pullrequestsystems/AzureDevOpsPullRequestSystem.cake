/// <summary>
/// Support for Azure DevOps / Azure Repository hosted code.
/// </summary>
public class AzureDevOpsPullRequestSystem : BasePullRequestSystem
{
    /// <inheritdoc />
    public override void ReportIssuesToPullRequest(ICakeContext context, IssuesData data)
    {
        context.NotNull(nameof(context));
        data.NotNull(nameof(data));

        if (string.IsNullOrWhiteSpace(context.EnvironmentVariable("SYSTEM_ACCESSTOKEN")))
        {
            context.Warning("SYSTEM_ACCESSTOKEN environment variable not set. Make sure the 'Allow Scripts to access OAuth token' option is enabled on the build definition.");
            return;
        }

        context.ReportIssuesToPullRequest(
            data.Issues,
            context.AzureDevOpsPullRequests(
                data.BuildServer.DetermineRepositoryRemoteUrl(context, data.RepositoryRootDirectory),
                data.BuildServer.DeterminePullRequestId(context).Value,
                context.AzureDevOpsAuthenticationOAuth(context.EnvironmentVariable("SYSTEM_ACCESSTOKEN"))),
            GetReportIssuesToPullRequestSettings(data));
    }

    /// <inheritdoc />
    public override void SetPullRequestIssuesState(ICakeContext context, IssuesData data)
    {
        context.NotNull(nameof(context));
        data.NotNull(nameof(data));

        if (string.IsNullOrWhiteSpace(context.EnvironmentVariable("SYSTEM_ACCESSTOKEN")))
        {
            context.Warning("SYSTEM_ACCESSTOKEN environment variable not set. Make sure the 'Allow Scripts to access OAuth token' option is enabled on the build definition.");
            return;
        }

        // Set status across all issues
        if (IssuesParameters.PullRequestSystem.ShouldSetPullRequestStatus)
        {
            SetPullRequestStatus(
                context,
                data,
                data.Issues,
                null);
        }

        // Set status for individual issue providers
        if (IssuesParameters.PullRequestSystem.ShouldSetSeparatePullRequestStatusForEachIssueProviderAndRun)
        {
            // Determine issue providers and runs from the list of issues providers from which issues were read
            // and issue providers and runs of the reported issues.
            var issueProvidersAndRuns =
                data.IssueProvidersAndRuns
                    .Select(x => 
                        new 
                        { 
                            x.Item1.ProviderName, 
                            Run = x.Item2 
                        })
                    .Union(
                        data.Issues
                            .GroupBy(x => 
                                new 
                                { 
                                    x.ProviderName, 
                                    x.Run 
                                })
                            .Select(x => 
                                new 
                                { 
                                    x.Key.ProviderName, 
                                    x.Key.Run 
                                }));

            foreach (var item in issueProvidersAndRuns)
            {
                var issueProvider = item.ProviderName;
                if (!string.IsNullOrEmpty(item.Run))
                {
                    issueProvider += $" ({item.Run})";
                }

                SetPullRequestStatus(
                    context,
                    data,
                    data.Issues.Where(x => x.ProviderName == item.ProviderName && x.Run == item.Run),
                    issueProvider);
            }
        }
    }

    /// <inheritdoc />
    public override FileLinkSettings GetFileLinkSettings(ICakeContext context, IssuesData data)
    {
        context.NotNull(nameof(context));
        data.NotNull(nameof(data));

        var rootPath = data.RepositoryRootDirectory.GetRelativePath(data.ProjectRootDirectory);

        return context.IssueFileLinkSettingsForAzureDevOpsCommit(
            data.RepositoryRemoteUrl,
            data.CommitId,
            rootPath.FullPath);
    }

    /// <summary>
    /// Reports a status to the pull request of the current build.
    /// </summary>
    /// <param name="context">The Cake context.</param>
    /// <param name="data">Object containing information about the build.</param>
    /// <param name="issues">Issues for which the status should be reported.</param>
    /// <param name="issueIdentifier">Identifier for the issues passed in <paramref name="issues"/>.</param>
    private static void SetPullRequestStatus(
        ICakeContext context,
        IssuesData data,
        IEnumerable<IIssue> issues,
        string issueIdentifier)
    {
        var pullRequestSettings =
            new AzureDevOpsPullRequestSettings(
                data.BuildServer.DetermineRepositoryRemoteUrl(context, data.RepositoryRootDirectory),
                data.BuildServer.DeterminePullRequestId(context).Value,
                context.AzureDevOpsAuthenticationOAuth(context.EnvironmentVariable("SYSTEM_ACCESSTOKEN")));

        var pullRequestStatusName = "Issues";
        var pullRequestDescriptionIfIssues = $"Found {issues.Count()} issues";
        var pullRequestDescriptionIfNoIssues = "No issues found";

        if (!string.IsNullOrWhiteSpace(issueIdentifier))
        {
            pullRequestStatusName += $"-{issueIdentifier}";
            pullRequestDescriptionIfIssues += $" for {issueIdentifier}";
            pullRequestDescriptionIfNoIssues += $" for {issueIdentifier}";
        }

        if (!string.IsNullOrWhiteSpace(IssuesParameters.BuildIdentifier))
        {
            pullRequestStatusName += $"-{IssuesParameters.BuildIdentifier}";
            pullRequestDescriptionIfIssues += $" in build {IssuesParameters.BuildIdentifier}";
            pullRequestDescriptionIfNoIssues += $" in build {IssuesParameters.BuildIdentifier}";
        }

        var state =
            issues.Any() ? 
                AzureDevOpsPullRequestStatusState.Failed : 
                AzureDevOpsPullRequestStatusState.Succeeded;

        context.Information("Set status {0} to {1}", pullRequestStatusName, state);

        var pullRequestStatus =
            new AzureDevOpsPullRequestStatus(pullRequestStatusName)
            {
                Genre = "Cake.Issues.Recipe",
                State = state,
                Description = issues.Any() ? pullRequestDescriptionIfIssues : pullRequestDescriptionIfNoIssues
            };

        context.AzureDevOpsSetPullRequestStatus(
            pullRequestSettings,
            pullRequestStatus);
    }
}