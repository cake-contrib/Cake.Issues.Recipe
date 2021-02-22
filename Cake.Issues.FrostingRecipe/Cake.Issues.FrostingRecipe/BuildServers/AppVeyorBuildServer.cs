using Cake.Common.Build;
using Cake.Common.IO;
using Cake.Core.IO;
using Cake.Issues.PullRequests;
using Cake.Issues.PullRequests.AppVeyor;
using System;

namespace Cake.Issues.FrostingRecipe
{
    /// <summary>
    /// Support for AppVeyor builds.
    /// </summary>
    internal class AppVeyorBuildServer : BaseBuildServer
    {
        /// <inheritdoc />
        public override Uri DetermineRepositoryRemoteUrl(
            IssuesContext context,
            DirectoryPath repositoryRootDirectory)
        {
            context.NotNull(nameof(context));

            switch (context.AppVeyor().Environment.Repository.Provider)
            {
                case "bitBucket": return new Uri($"https://bitbucket.org/{context.AppVeyor().Environment.Repository.Name}/src");
                case "gitHub": return new Uri($"https://github.com/{context.AppVeyor().Environment.Repository.Name}.git");
                case "gitLab": return new Uri($"https://gitlab.com/{context.AppVeyor().Environment.Repository.Name}.git");
                case "vso": return new Uri($"https://dev.azure.com/{context.AppVeyor().Environment.Repository.Name}");
                default: return new Uri(context.AppVeyor().Environment.Repository.Name);
            }
        }

        /// <inheritdoc />
        public override string DetermineCommitId(
            IssuesContext context,
            DirectoryPath repositoryRootDirectory)
        {
            context.NotNull(nameof(context));

            return context.AppVeyor().Environment.Repository.Commit.Id;
        }

        /// <inheritdoc />
        public override bool DetermineIfPullRequest(IssuesContext context)
        {
            context.NotNull(nameof(context));

            return context.AppVeyor().Environment.PullRequest.IsPullRequest;
        }

        /// <inheritdoc />
        public override int? DeterminePullRequestId(IssuesContext context)
        {
            context.NotNull(nameof(context));

            return context.AppVeyor().Environment.PullRequest.Number;
        }

        /// <inheritdoc />
        public override void ReportIssuesToBuildServer(
            IssuesContext context)
        {
            context.NotNull(nameof(context));

            context.ReportIssuesToPullRequest(
                context.State.Issues,
                context.AppVeyorBuilds(),
                context.State.BuildRootDirectory);
        }

        /// <inheritdoc />
        public override void CreateSummaryIssuesReport(
            IssuesContext context,
            [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "")
        {
            context.NotNull(nameof(context));

            // Summary issues report is not supported for AppVeyor.
        }

        /// <inheritdoc />
        public override void PublishIssuesArtifacts(IssuesContext context)
        {
            context.NotNull(nameof(context));

            if (context.Parameters.BuildServer.ShouldPublishFullIssuesReport &&
                context.State.FullIssuesReport != null &&
                context.FileExists(context.State.FullIssuesReport))
            {
                context.AppVeyor().UploadArtifact(context.State.FullIssuesReport);
            }
        }
    }
}