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
    internal sealed class AzureDevOpsPullRequestSystem : BasePullRequestSystem
    {
        /// <inheritdoc />
        public override void ReportIssuesToPullRequest(IIssuesContext context)
        {
            context.NotNull(nameof(context));

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
            context.NotNull(nameof(context));

            if (string.IsNullOrWhiteSpace(context.EnvironmentVariable("SYSTEM_ACCESSTOKEN")))
            {
                context.Warning("SYSTEM_ACCESSTOKEN environment variable not set. Make sure the 'Allow Scripts to access OAuth token' option is enabled on the build definition.");
                return;
            }

            // Set status across all issues
            if (context.Parameters.PullRequestSystem.ShouldSetPullRequestStatus)
            {
                SetPullRequestStatus(
                    context,
                    context.State.Issues,
                    null);
            }

            // Set status for individual issue providers
            if (context.Parameters.PullRequestSystem.ShouldSetSeparatePullRequestStatusForEachIssueProviderAndRun)
            {
                // Determine issue providers and runs from the list of issues providers from which issues were read
                // and issue providers and runs of the reported issues.
                var issueProvidersAndRuns =
                    context.State.IssueProvidersAndRuns
                        .Select(x => 
                            new 
                            { 
                                x.Item1.ProviderName, 
                                Run = x.Item2 
                            })
                        .Union(
                            context.State.Issues
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
                        context.State.Issues.Where(x => x.ProviderName == item.ProviderName && x.Run == item.Run),
                        issueProvider);
                }
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

            var issuesList = issues.ToList();

            var pullRequestStatusName = "Issues";
            var pullRequestDescriptionIfIssues = $"Found {issuesList.Count} issues";
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

            var state =
                issuesList.Count != 0 ? 
                    AzureDevOpsPullRequestStatusState.Failed : 
                    AzureDevOpsPullRequestStatusState.Succeeded;

            context.Information("Set status {0} to {1}", pullRequestStatusName, state);

            var pullRequestStatus =
                new AzureDevOpsPullRequestStatus(pullRequestStatusName)
                {
                    Genre = "Cake.Issues.Recipe",
                    State = state,
                    Description = issuesList.Count != 0 ? pullRequestDescriptionIfIssues : pullRequestDescriptionIfNoIssues
                };

            context.AzureDevOpsSetPullRequestStatus(
                pullRequestSettings,
                pullRequestStatus);
        }
    }
}