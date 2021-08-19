using Cake.Issues;

namespace Cake.Frosting.Issues.Recipe
{
    /// <summary>
    /// Support for GitHub hosted code.
    /// </summary>
    internal class GitHubPullRequestSystem : BasePullRequestSystem
    {
        /// <inheritdoc />
        public override void ReportIssuesToPullRequest(IssuesContext context)
        {
            context.NotNull(nameof(context));

            // Not supported yet
        }

        /// <inheritdoc />
        public override void SetPullRequestIssuesState(IssuesContext context)
        {
            context.NotNull(nameof(context));

            // Not supported yet
        }

        /// <inheritdoc />
        public override FileLinkSettings GetFileLinkSettings(IssuesContext context)
        {
            context.NotNull(nameof(context));

            var rootPath = context.State.RepositoryRootDirectory.GetRelativePath(context.State.ProjectRootDirectory);

            return context.IssueFileLinkSettingsForGitHubCommit(
                context.State.RepositoryRemoteUrl,
                context.State.CommitId,
                rootPath.FullPath);
        }
    }
}