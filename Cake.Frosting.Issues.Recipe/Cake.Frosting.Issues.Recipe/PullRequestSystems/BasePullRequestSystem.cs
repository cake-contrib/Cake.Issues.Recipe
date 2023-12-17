namespace Cake.Frosting.Issues.Recipe
{
    using Cake.Issues;
    using Cake.Issues.PullRequests;

    /// <summary>
    /// Basic implementation for all pull request server.
    /// </summary>
    internal abstract class BasePullRequestSystem : IIssuesPullRequestSystem
    {
        /// <inheritdoc />
        public abstract void ReportIssuesToPullRequest(
            IIssuesContext context);

        /// <inheritdoc />
        public abstract void SetPullRequestIssuesState(
            IIssuesContext context);

        /// <inheritdoc />
        public abstract FileLinkSettings GetFileLinkSettings(
            IIssuesContext context);

        /// <summary>
        /// Returns settings for reporting issues to pull requests.
        /// </summary>
        /// <param name="context">The Cake context.</param>
        /// <returns>Settings for reporting issues to pull requests.</returns>
        protected static IReportIssuesToPullRequestSettings GetReportIssuesToPullRequestSettings(IIssuesContext context)
        {
            var settings =
                new ReportIssuesToPullRequestSettings(context.State.ProjectRootDirectory)
                {
                    MaxIssuesToPost = context.Parameters.PullRequestSystem.MaxIssuesToPost,
                    MaxIssuesToPostAcrossRuns = context.Parameters.PullRequestSystem.MaxIssuesToPostAcrossRuns,
                    MaxIssuesToPostForEachIssueProvider = context.Parameters.PullRequestSystem.MaxIssuesToPostForEachIssueProvider
                };

            foreach (var providerIssueLimit in context.Parameters.PullRequestSystem.ProviderIssueLimits)
            {
                settings.ProviderIssueLimits.Add(providerIssueLimit.Key, providerIssueLimit.Value);
            }

            return settings;
        }
    }
}