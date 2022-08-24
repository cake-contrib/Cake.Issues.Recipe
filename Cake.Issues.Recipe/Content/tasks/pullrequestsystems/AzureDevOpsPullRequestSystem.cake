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

        if (IssuesParameters.PullRequestSystem.ShouldSetSeparatePullRequestStatusForEachIssueProviderAndRun)
        {
            foreach (var providerGroup in data.Issues.GroupBy(x => x.ProviderType))
            {
                var issueProvider = providerGroup.Key;
                foreach (var runGroup in providerGroup.GroupBy(x => x.Run))
                {
                    if (!string.IsNullOrEmpty(runGroup.Key))
                    {
                        issueProvider += $"-{runGroup.Key}";
                    }

                    SetPullRequestStatus(
                        context,
                        data,
                        runGroup,
                        issueProvider);
                }
            }
        }
        else
        {
            SetPullRequestStatus(
                context,
                data,
                data.Issues,
                null);
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

        var pullRequestStatus =
            new AzureDevOpsPullRequestStatus(pullRequestStatusName)
            {
                Genre = "Cake.Issues.Recipe",
                State = issues.Any() ? AzureDevOpsPullRequestStatusState.Failed : AzureDevOpsPullRequestStatusState.Succeeded,
                Description = issues.Any() ? pullRequestDescriptionIfIssues : pullRequestDescriptionIfNoIssues
            };

        context.AzureDevOpsSetPullRequestStatus(
            pullRequestSettings,
            pullRequestStatus);
    }
}