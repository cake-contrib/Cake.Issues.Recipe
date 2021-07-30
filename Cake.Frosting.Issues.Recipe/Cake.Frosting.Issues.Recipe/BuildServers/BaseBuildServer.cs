using Cake.Core.IO;
using Cake.Git;
using Cake.Issues;
using System;
using System.Linq;

namespace Cake.Frosting.Issues.Recipe
{
    /// <summary>
    /// Basic implementation for all build servers.
    /// </summary>
    internal abstract class BaseBuildServer : IIssuesBuildServer
    {
        /// <inheritdoc />
        public virtual Uri DetermineRepositoryRemoteUrl(
            IssuesContext context,
            DirectoryPath repositoryRootDirectory)
        {
            context.NotNull(nameof(context));
            repositoryRootDirectory.NotNull(nameof(repositoryRootDirectory));

            var currentBranch = context.GitBranchCurrent(repositoryRootDirectory);
            return new Uri(currentBranch.Remotes.Single(x => x.Name == "origin").Url);
        }

        /// <inheritdoc />
        public virtual string DetermineCommitId(
            IssuesContext context,
            DirectoryPath repositoryRootDirectory)
        {
            context.NotNull(nameof(context));
            repositoryRootDirectory.NotNull(nameof(repositoryRootDirectory));

            return context.GitLogTip(repositoryRootDirectory).Sha;
        }

        /// <inheritdoc />
        public virtual bool DetermineIfPullRequest(IssuesContext context)
        {
            context.NotNull(nameof(context));

            return false;
        }

        /// <inheritdoc />
        public virtual int? DeterminePullRequestId(IssuesContext context)
        {
            context.NotNull(nameof(context));

            return null;
        }

        /// <inheritdoc />
        public abstract void ReportIssuesToBuildServer(
            IssuesContext context);

        /// <inheritdoc />
        public abstract void CreateSummaryIssuesReport(
            IssuesContext context,
            [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "");

        /// <inheritdoc />
        public abstract void PublishIssuesArtifacts(
            IssuesContext context);
    }
}