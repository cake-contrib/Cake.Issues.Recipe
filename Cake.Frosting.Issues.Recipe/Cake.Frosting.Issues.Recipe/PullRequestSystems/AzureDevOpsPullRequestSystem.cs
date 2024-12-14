namespace Cake.Frosting.Issues.Recipe;

using Cake.AzureDevOps;
using Cake.AzureDevOps.Repos.PullRequest;
using Cake.Common;
using Cake.Common.Diagnostics;

/// <summary>
/// Support for Azure DevOps / Azure Repository hosted code.
/// </summary>
internal sealed class AzureDevOpsPullRequestSystem : BasePullRequestSystem
{
    /// <inheritdoc />
    public override void ReportIssuesToPullRequest(IIssuesContext context)
    {
        context.NotNull();

        if (string.IsNullOrWhiteSpace(context.EnvironmentVariable("SYSTEM_ACCESSTOKEN")))
        {
            context.Warning("SYSTEM_ACCESSTOKEN environment variable not set. Make sure the 'Allow Scripts to access OAuth token' option is enabled on the build definition.");
            return;
        }

        context.ReportIssuesToPullRequest(
            context.State.Issues,
            context.AzureDevOpsPullRequests(
                context.State.BuildServer.DetermineRepositoryRemoteUrl(context, context.State.RepositoryRootDirectory),
                context.State.BuildServer.DeterminePullRequestId(context).Value,
                context.AzureDevOpsAuthenticationOAuth(context.EnvironmentVariable("SYSTEM_ACCESSTOKEN"))),
            GetReportIssuesToPullRequestSettings(context));
    }

    /// <inheritdoc />
    public override void SetPullRequestIssuesState(IIssuesContext context)
    {
        context.NotNull();

        if (string.IsNullOrWhiteSpace(context.EnvironmentVariable("SYSTEM_ACCESSTOKEN")))
        {
            context.Warning("SYSTEM_ACCESSTOKEN environment variable not set. Make sure the 'Allow Scripts to access OAuth token' option is enabled on the build definition.");
            return;
        }

        var status = PullRequestStatusCalculator.GetPullRequestStates(context).ToList();
        if (status.Count > 0)
        {
            var pullRequestSettings =
                new AzureDevOpsPullRequestSettings(
                    context.State.BuildServer.DetermineRepositoryRemoteUrl(context, context.State.RepositoryRootDirectory),
                    context.State.BuildServer.DeterminePullRequestId(context).Value,
                    context.AzureDevOpsAuthenticationOAuth(context.EnvironmentVariable("SYSTEM_ACCESSTOKEN")));

            foreach (var item in status)
            {
                context.Information("Set status {0} to {1}", item.Name, item.State);

                context.AzureDevOpsSetPullRequestStatus(
                    pullRequestSettings,
                    item.ToAzureDevOpsPullRequestStatus());
            }
        }
    }

    /// <inheritdoc />
    public override FileLinkSettings GetFileLinkSettings(IIssuesContext context)
    {
        context.NotNull();

        var rootPath = context.State.RepositoryRootDirectory.GetRelativePath(context.State.ProjectRootDirectory);

        return context.IssueFileLinkSettingsForAzureDevOpsCommit(
            context.State.RepositoryRemoteUrl,
            context.State.CommitId,
            rootPath.FullPath);
    }
}