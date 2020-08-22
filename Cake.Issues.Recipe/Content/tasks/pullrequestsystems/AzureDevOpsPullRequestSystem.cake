/// <summary>
/// Support for Azure DevOps / Azure Repository hosted code.
/// </summary>
public class AzureDevOpsPullRequestSystem : BasePullRequestSystem
{
    /// <inheritdoc />
    public override void ReportIssuesToPullRequest(ICakeContext context, IssuesData data)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        if (data == null)
        {
            throw new ArgumentNullException(nameof(data));
        }

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
            data.BuildRootDirectory);
    }

    /// <inheritdoc />
    public override void SetPullRequestIssuesState(ICakeContext context, IssuesData data)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        if (data == null)
        {
            throw new ArgumentNullException(nameof(data));
        }

        if (string.IsNullOrWhiteSpace(context.EnvironmentVariable("SYSTEM_ACCESSTOKEN")))
        {
            context.Warning("SYSTEM_ACCESSTOKEN environment variable not set. Make sure the 'Allow Scripts to access OAuth token' option is enabled on the build definition.");
            return;
        }

        var pullRequestSettings =
            new AzureDevOpsPullRequestSettings(
                data.BuildServer.DetermineRepositoryRemoteUrl(context, data.RepositoryRootDirectory),
                data.BuildServer.DeterminePullRequestId(context).Value,
                context.AzureDevOpsAuthenticationOAuth(context.EnvironmentVariable("SYSTEM_ACCESSTOKEN")));

        var pullRequestStatusName = "Issues";
        var pullRequestDescriptionIfIssues = $"Found {data.Issues.Count()} issues";
        var pullRequestDescriptionIfNoIssues = "No issues found";
        if (!string.IsNullOrWhiteSpace(IssuesParameters.BuildIdentifier))
        {
            pullRequestStatusName += $"-{IssuesParameters.BuildIdentifier}";
            pullRequestDescriptionIfIssues += $" for build {IssuesParameters.BuildIdentifier}";
            pullRequestDescriptionIfNoIssues += $" for build {IssuesParameters.BuildIdentifier}";
        }

        var pullRequestStatus =
            new AzureDevOpsPullRequestStatus(pullRequestStatusName)
            {
                Genre = "Cake.Issues.Recipe",
                State = data.Issues.Any() ? AzureDevOpsPullRequestStatusState.Failed : AzureDevOpsPullRequestStatusState.Succeeded,
                Description = data.Issues.Any() ? pullRequestDescriptionIfIssues : pullRequestDescriptionIfNoIssues
            };

        context.AzureDevOpsSetPullRequestStatus(
            pullRequestSettings,
            pullRequestStatus);
    }

    /// <inheritdoc />
    public override FileLinkSettings GetFileLinkSettings(ICakeContext context, IssuesData data)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        if (data == null)
        {
            throw new ArgumentNullException(nameof(data));
        }

        var rootPath = data.RepositoryRootDirectory.GetRelativePath(data.BuildRootDirectory);

        return context.IssueFileLinkSettingsForAzureDevOpsCommit(
            data.RepositoryRemoteUrl,
            data.CommitId,
            rootPath.FullPath);
    }
}