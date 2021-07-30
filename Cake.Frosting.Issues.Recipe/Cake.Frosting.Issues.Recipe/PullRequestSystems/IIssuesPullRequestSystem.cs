using Cake.Issues;

namespace Cake.Frosting.Issues.Recipe
{
    /// <summary>
    /// Description of a pull request system implementation.
    /// </summary>
    public interface IIssuesPullRequestSystem
    {
        /// <summary>
        /// Report issues as comments to pull request.
        /// </summary>
        /// <param name="context">The Cake context.</param>
        void ReportIssuesToPullRequest(
            IssuesContext context);

        /// <summary>
        /// Set pull request status.
        /// </summary>
        /// <param name="context">The Cake context.</param>
        void SetPullRequestIssuesState(
            IssuesContext context);

        /// <summary>
        /// Get settings for linking to files.
        /// </summary>
        /// <param name="context">The Cake context.</param>
        /// <returns>Settings for linking to files.</returns>
        FileLinkSettings GetFileLinkSettings(
            IssuesContext context);
    }
}