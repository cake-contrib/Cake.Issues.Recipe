using Cake.Issues;

namespace Cake.Frosting.Issues.Recipe
{
    /// <summary>
    /// Basic implementation for all pull request server.
    /// </summary>
    internal abstract class BasePullRequestSystem : IIssuesPullRequestSystem
    {
        /// <inheritdoc />
        public abstract void ReportIssuesToPullRequest(
            IssuesContext context);

        /// <inheritdoc />
        public abstract void SetPullRequestIssuesState(
            IssuesContext context);

        /// <inheritdoc />
        public abstract FileLinkSettings GetFileLinkSettings(
            IssuesContext context);
    }
}