namespace Cake.Frosting.Issues.Recipe
{
    using Cake.Issues;

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
            IIssuesContext context);

        /// <summary>
        /// Set pull request status.
        /// </summary>
        /// <param name="context">The Cake context.</param>
        void SetPullRequestIssuesState(
            IIssuesContext context);

        /// <summary>
        /// Get settings for linking to files.
        /// </summary>
        /// <param name="context">The Cake context.</param>
        /// <returns>Settings for linking to files.</returns>
        FileLinkSettings GetFileLinkSettings(
            IIssuesContext context);
    }
}