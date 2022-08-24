using Cake.AzureDevOps;
using Cake.AzureDevOps.Repos.PullRequest;
using Cake.Common;
using Cake.Common.Diagnostics;
using Cake.Issues;
using Cake.Issues.PullRequests;
using Cake.Issues.PullRequests.AzureDevOps;
using System.Collections.Generic;
using System.Linq;

namespace Cake.Frosting.Issues.Recipe
{
    /// <summary>
    /// Support for Azure DevOps / Azure Repository hosted code.
    /// </summary>
    internal class AzureDevOpsPullRequestSystem : BasePullRequestSystem
    {
        /// <inheritdoc />
        public override void ReportIssuesToPullRequest(IIssuesContext context)
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
                GetReportIssuesToPullRequestSettings(context));
            #endregion
        }

        /// <inheritdoc />
        public override void SetPullRequestIssuesState(IIssuesContext context)
        {
            context.NotNull(nameof(context));

            if (string.IsNullOrWhiteSpace(context.EnvironmentVariable("SYSTEM_ACCESSTOKEN")))
            {
                context.Warning("SYSTEM_ACCESSTOKEN environment variable not set. Make sure the 'Allow Scripts to access OAuth token' option is enabled on the build definition.");
                return;
            }

            if (context.Parameters.PullRequestSystem.ShouldSetSeparatePullRequestStatusForEachIssueProviderAndRun)
            {
                foreach (var providerGroup in context.State.Issues.GroupBy(x => x.ProviderType))
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
                            runGroup,
                            issueProvider);
                    }
                }
            }
            else
            {
                SetPullRequestStatus(
                    context,
                    context.State.Issues,
                    null);
            }
        }

        /// <inheritdoc />
        public override FileLinkSettings GetFileLinkSettings(IIssuesContext context)
        {
            context.NotNull(nameof(context));

            var rootPath = context.State.RepositoryRootDirectory.GetRelativePath(context.State.ProjectRootDirectory);

            return context.IssueFileLinkSettingsForAzureDevOpsCommit(
                context.State.RepositoryRemoteUrl,
                context.State.CommitId,
                rootPath.FullPath);
        }

        /// <summary>
        /// Reports a status to the pull request of the current build.
        /// </summary>
        /// <param name="context">The Cake context.</param>
        /// <param name="issues">Issues for which the status should be reported.</param>
        /// <param name="issueIdentifier">Identifier for the issues passed in <paramref name="issues"/>.</param>
        private static void SetPullRequestStatus(
            IIssuesContext context,
            IEnumerable<IIssue> issues,
            string issueIdentifier)
        {
            var pullRequestSettings =
                new AzureDevOpsPullRequestSettings(
                    context.State.BuildServer.DetermineRepositoryRemoteUrl(context, context.State.RepositoryRootDirectory),
                    context.State.BuildServer.DeterminePullRequestId(context).Value,
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

            if (!string.IsNullOrWhiteSpace(context.Parameters.BuildIdentifier))
            {
                pullRequestStatusName += $"-{context.Parameters.BuildIdentifier}";
                pullRequestDescriptionIfIssues += $" in build {context.Parameters.BuildIdentifier}";
                pullRequestDescriptionIfNoIssues += $" in build {context.Parameters.BuildIdentifier}";
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
}