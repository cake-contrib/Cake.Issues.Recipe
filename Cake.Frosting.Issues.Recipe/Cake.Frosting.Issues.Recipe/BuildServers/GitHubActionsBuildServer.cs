using Cake.Common.Build;
using Cake.Core.IO;
using Cake.Issues;
using Cake.Issues.PullRequests;
using Cake.Issues.PullRequests.GitHubActions;
using System;

namespace Cake.Frosting.Issues.Recipe
{
    /// <summary>
    /// Support for builds running on GitHub Actions.
    /// </summary>
    internal class GitHubActionsBuildServer : BaseBuildServer
    {
        /// <inheritdoc />
        public override Uri DetermineRepositoryRemoteUrl(
            IIssuesContext context,
            DirectoryPath repositoryRootDirectory)
        {
            context.NotNull(nameof(context));

            return new Uri($"https://github.com/{context.GitHubActions().Environment.Workflow.Repository}.git");
        }

        /// <inheritdoc />
        public override string DetermineCommitId(
            IIssuesContext context,
            DirectoryPath repositoryRootDirectory)
        {
            context.NotNull(nameof(context));

            return context.GitHubActions().Environment.Workflow.Sha;
        }

        /// <inheritdoc />
        public override bool DetermineIfPullRequest(IIssuesContext context)
        {
            context.NotNull(nameof(context));

            return context.GitHubActions().Environment.PullRequest.IsPullRequest;
        }

        /// <inheritdoc />
        public override int? DeterminePullRequestId(IIssuesContext context)
        {
            context.NotNull(nameof(context));

            // Not supported by GitHub Actions
            return null;
        }

        /// <inheritdoc />
        public override void ReportIssuesToBuildServer(
            IIssuesContext context)
        {
            context.NotNull(nameof(context));

            context.ReportIssuesToPullRequest(
                context.State.Issues,
                context.GitHubActionsBuilds(),
                context.State.ProjectRootDirectory);
        }

        /// <inheritdoc />
        public override void CreateSummaryIssuesReport(
            IIssuesContext context,
            [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "")
        {
            context.NotNull(nameof(context));

            // Summary issues report is not supported for GitHub Actions.
        }

        /// <inheritdoc />
        public override void PublishIssuesArtifacts(IIssuesContext context)
        {
            context.NotNull(nameof(context));

            // Publishing artifacts is currently not supported for GitHub Actions.
        }
    }
}