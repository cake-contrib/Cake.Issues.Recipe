namespace Cake.Frosting.Issues.Recipe
{
    using Cake.Common.Build;
    using Cake.Common.IO;
    using Cake.Core.IO;

    /// <summary>
    /// Support for AppVeyor builds.
    /// </summary>
    internal sealed class AppVeyorBuildServer : BaseBuildServer
    {
        /// <inheritdoc />
        public override Uri DetermineRepositoryRemoteUrl(
            IIssuesContext context,
            DirectoryPath repositoryRootDirectory)
        {
            context.NotNull(nameof(context));

            return context.AppVeyor().Environment.Repository.Provider switch
            {
                "bitBucket" => new Uri($"https://bitbucket.org/{context.AppVeyor().Environment.Repository.Name}/src"),
                "gitHub" => new Uri($"https://github.com/{context.AppVeyor().Environment.Repository.Name}.git"),
                "gitLab" => new Uri($"https://gitlab.com/{context.AppVeyor().Environment.Repository.Name}.git"),
                "vso" => new Uri($"https://dev.azure.com/{context.AppVeyor().Environment.Repository.Name}"),
                _ => new Uri(context.AppVeyor().Environment.Repository.Name),
            };
        }

        /// <inheritdoc />
        public override string DetermineCommitId(
            IIssuesContext context,
            DirectoryPath repositoryRootDirectory)
        {
            context.NotNull(nameof(context));

            return context.AppVeyor().Environment.Repository.Commit.Id;
        }

        /// <inheritdoc />
        public override bool DetermineIfPullRequest(IIssuesContext context)
        {
            context.NotNull(nameof(context));

            return context.AppVeyor().Environment.PullRequest.IsPullRequest;
        }

        /// <inheritdoc />
        public override int? DeterminePullRequestId(IIssuesContext context)
        {
            context.NotNull(nameof(context));

            return context.AppVeyor().Environment.PullRequest.Number;
        }

        /// <inheritdoc />
        public override void ReportIssuesToBuildServer(
            IIssuesContext context)
        {
            context.NotNull(nameof(context));

            context.ReportIssuesToPullRequest(
                context.State.Issues,
                context.AppVeyorBuilds(),
                context.State.ProjectRootDirectory);
        }

        /// <inheritdoc />
        public override void CreateSummaryIssuesReport(
            IIssuesContext context,
            [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "")
        {
            context.NotNull(nameof(context));

            // Summary issues report is not supported for AppVeyor.
        }

        /// <inheritdoc />
        public override void PublishIssuesArtifacts(IIssuesContext context)
        {
            context.NotNull(nameof(context));

            if (context.Parameters.BuildServer.ShouldPublishFullIssuesReport &&
                context.State.FullIssuesReport != null &&
                context.FileExists(context.State.FullIssuesReport))
            {
                context.AppVeyor().UploadArtifact(context.State.FullIssuesReport);
            }

            if (context.Parameters.BuildServer.ShouldPublishSarifReport &&
                context.State.SarifReport != null &&
                context.FileExists(context.State.SarifReport))
            {
                context.AppVeyor().UploadArtifact(context.State.SarifReport);
            }
        }
    }
}