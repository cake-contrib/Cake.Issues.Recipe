/// <summary>
/// Support for Azure DevOps / Azure Repository hosted code.
/// </summary>
public class AzureDevOpsPullRequestSystem : BasePullRequestSystem
{
    /// <inheritdoc />
    public override void ReportIssuesToPullRequest(ICakeContext context, IssuesData data)
    {
        context.NotNull();
        data.NotNull();

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
        context.NotNull();
        data.NotNull();

        if (string.IsNullOrWhiteSpace(context.EnvironmentVariable("SYSTEM_ACCESSTOKEN")))
        {
            context.Warning("SYSTEM_ACCESSTOKEN environment variable not set. Make sure the 'Allow Scripts to access OAuth token' option is enabled on the build definition.");
            return;
        }

        var status = PullRequestStatusCalculator.GetPullRequestStates(data).ToList();
        if (status.Count > 0)
        {
            var pullRequestSettings =
                new AzureDevOpsPullRequestSettings(
                    data.BuildServer.DetermineRepositoryRemoteUrl(context, data.RepositoryRootDirectory),
                    data.BuildServer.DeterminePullRequestId(context).Value,
                    context.AzureDevOpsAuthenticationOAuth(context.EnvironmentVariable("SYSTEM_ACCESSTOKEN")));

            foreach (var item in status)
            {
                context.Information("Set status {0} to {1}", item.Name, item.State);

                context.AzureDevOpsSetPullRequestStatus(
                    pullRequestSettings,
                    PullRequestStatusExtensions.ToAzureDevOpsPullRequestStatus(item));
            }
        }
    }

    /// <inheritdoc />
    public override FileLinkSettings GetFileLinkSettings(ICakeContext context, IssuesData data)
    {
        context.NotNull();
        data.NotNull();

        var rootPath = data.RepositoryRootDirectory.GetRelativePath(data.ProjectRootDirectory);

        return context.IssueFileLinkSettingsForAzureDevOpsCommit(
            data.RepositoryRemoteUrl,
            data.CommitId,
            rootPath.FullPath);
    }
 }