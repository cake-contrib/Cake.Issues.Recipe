namespace Cake.Frosting.Issues.Recipe
{
    using Cake.Core.IO;

    /// <summary>
    /// Basic implementation for all build servers.
    /// </summary>
    internal abstract class BaseBuildServer : IIssuesBuildServer
    {
        /// <inheritdoc />
        public virtual Uri DetermineRepositoryRemoteUrl(
            IIssuesContext context,
            DirectoryPath repositoryRootDirectory)
        {
            context.NotNull();
            repositoryRootDirectory.NotNull();

            return context.State.RepositoryInfo.GetRepositoryRemoteUrl(context, repositoryRootDirectory);
        }

        /// <inheritdoc />
        public virtual string DetermineCommitId(
            IIssuesContext context,
            DirectoryPath repositoryRootDirectory)
        {
            context.NotNull();
            repositoryRootDirectory.NotNull();

            return context.State.RepositoryInfo.GetCommitId(context, repositoryRootDirectory);
        }

        /// <inheritdoc />
        public virtual bool DetermineIfPullRequest(IIssuesContext context)
        {
            context.NotNull();

            return false;
        }

        /// <inheritdoc />
        public virtual int? DeterminePullRequestId(IIssuesContext context)
        {
            context.NotNull();

            return null;
        }

        /// <inheritdoc />
        public abstract void ReportIssuesToBuildServer(
            IIssuesContext context);

        /// <inheritdoc />
        public abstract void CreateSummaryIssuesReport(
            IIssuesContext context,
            [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "");

        /// <inheritdoc />
        public abstract void PublishIssuesArtifacts(
            IIssuesContext context);
    }
}