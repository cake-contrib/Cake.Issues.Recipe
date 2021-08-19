using Cake.AzureDevOps;
using Cake.AzureDevOps.Repos.PullRequest;
using Cake.Common;
using Cake.Common.Diagnostics;
using Cake.Issues;
using Cake.Issues.PullRequests;
using Cake.Issues.PullRequests.AzureDevOps;
using System.Linq;

namespace Cake.Frosting.Issues.Recipe
{
    /// <summary>
    /// Support for Azure DevOps / Azure Repository hosted code.
    /// </summary>
    internal class AzureDevOpsPullRequestSystem : BasePullRequestSystem
    {
        /// <inheritdoc />
        public override void ReportIssuesToPullRequest(IssuesContext context)
        {
            context.NotNull(nameof(context));

#pragma warning disable SA1123 // Do not place regions within elements
            #region DupFinder Exclusion
#pragma warning restore SA1123 // Do not place regions within elements
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
                context.State.BuildRootDirectory);
            #endregion
        }

        /// <inheritdoc />
        public override void SetPullRequestIssuesState(IssuesContext context)
        {
            context.NotNull(nameof(context));

#pragma warning disable SA1123 // Do not place regions within elements
            #region DupFinder Exclusion
#pragma warning restore SA1123 // Do not place regions within elements
            if (string.IsNullOrWhiteSpace(context.EnvironmentVariable("SYSTEM_ACCESSTOKEN")))
            {
                context.Warning("SYSTEM_ACCESSTOKEN environment variable not set. Make sure the 'Allow Scripts to access OAuth token' option is enabled on the build definition.");
                return;
            }

            var pullRequestSettings =
                new AzureDevOpsPullRequestSettings(
                    context.State.BuildServer.DetermineRepositoryRemoteUrl(context, context.State.RepositoryRootDirectory),
                    context.State.BuildServer.DeterminePullRequestId(context).Value,
                    context.AzureDevOpsAuthenticationOAuth(context.EnvironmentVariable("SYSTEM_ACCESSTOKEN")));
            #endregion

            var pullRequestStatusName = "Issues";
            var pullRequestDescriptionIfIssues = $"Found {context.State.Issues.Count()} issues";
            var pullRequestDescriptionIfNoIssues = "No issues found";
            if (!string.IsNullOrWhiteSpace(context.Parameters.BuildIdentifier))
            {
                pullRequestStatusName += $"-{context.Parameters.BuildIdentifier}";
                pullRequestDescriptionIfIssues += $" for build {context.Parameters.BuildIdentifier}";
                pullRequestDescriptionIfNoIssues += $" for build {context.Parameters.BuildIdentifier}";
            }

            var pullRequestStatus =
                new AzureDevOpsPullRequestStatus(pullRequestStatusName)
                {
                    Genre = "Cake.Issues.Recipe",
                    State = context.State.Issues.Any() ? AzureDevOpsPullRequestStatusState.Failed : AzureDevOpsPullRequestStatusState.Succeeded,
                    Description = context.State.Issues.Any() ? pullRequestDescriptionIfIssues : pullRequestDescriptionIfNoIssues
                };

            context.AzureDevOpsSetPullRequestStatus(
                pullRequestSettings,
                pullRequestStatus);
        }

        /// <inheritdoc />
        public override FileLinkSettings GetFileLinkSettings(IssuesContext context)
        {
            context.NotNull(nameof(context));

            var rootPath = context.State.RepositoryRootDirectory.GetRelativePath(context.State.ProjectRootDirectory);

            return context.IssueFileLinkSettingsForAzureDevOpsCommit(
                context.State.RepositoryRemoteUrl,
                context.State.CommitId,
                rootPath.FullPath);
        }
    }
}