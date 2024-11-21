namespace Cake.Frosting.Issues.Recipe
{
    using Cake.Core.Diagnostics;

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
                context.Log.Verbose(
                    "Setting issue limit for provider '{0}' to MaxIssuesToPost '{1}' and MaxIssuesToPostAcrossRuns '{2}'...",
                    providerIssueLimit.Key,
                    providerIssueLimit.Value.MaxIssuesToPost,
                    providerIssueLimit.Value.MaxIssuesToPostAcrossRuns);
                settings.ProviderIssueLimits.Add(providerIssueLimit.Key, providerIssueLimit.Value);
            }

            foreach (var issueFilter in context.Parameters.PullRequestSystem.IssueFilters)
            {
                context.Log.Verbose("Adding issue filter...");
                settings.IssueFilters.Add(issueFilter);
            }

            return settings;
        }
    }
}