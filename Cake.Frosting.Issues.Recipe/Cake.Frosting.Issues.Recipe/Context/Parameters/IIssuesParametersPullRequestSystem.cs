namespace Cake.Frosting.Issues.Recipe
{
    /// <summary>
    /// Parameters for pull request integration.
    /// </summary>
    public interface IIssuesParametersPullRequestSystem
    {
        /// <summary>
        /// Gets or sets a value indicating whether issues should be reported to the pull request system.
        /// Default value is <c>true</c>.
        /// </summary>
        bool ShouldReportIssuesToPullRequest { get; set; }

        /// <summary>
        /// Gets or sets the global number of issues which should be posted at maximum over all
        /// <see cref="IIssueProvider"/>.
        /// Issues are filtered by <see cref="IIssue.Priority"/> and issues with an <see cref="IIssue.AffectedFileRelativePath"/>
        /// are prioritized.
        /// Default is <c>null</c> which won't set a global limit.
        /// Use <see cref="MaxIssuesToPostAcrossRuns"/> to set a limit across multiple runs.
        /// </summary>
        int? MaxIssuesToPost { get; set; }

        /// <summary>
        /// Gets or sets the global number of issues which should be posted at maximum over all
        /// <see cref="IIssueProvider"/> and across multiple runs.
        /// Issues are filtered by <see cref="IIssue.Priority"/> and issues with an <see cref="IIssue.AffectedFileRelativePath"/>
        /// are prioritized.
        /// Default is <c>null</c> which won't set a limit across multiple runs.
        /// Use <see cref="MaxIssuesToPost"/> to set a limit for a single run.
        /// </summary>
        int? MaxIssuesToPostAcrossRuns { get; set; }

        /// <summary>
        /// Gets or sets the number of issues which should be posted at maximum for each
        /// <see cref="IIssueProvider"/>.
        /// Issues are filtered by <see cref="IIssue.Priority"/> and issues with an <see cref="IIssue.AffectedFileRelativePath"/>
        /// are prioritized.
        /// <c>null</c> won't limit issues per issue provider.
        /// Default is to filter to 100 issues for each issue provider.
        /// Use <see cref="ProviderIssueLimits"/> to set limits for individual issue providers.
        /// </summary>
        int? MaxIssuesToPostForEachIssueProvider { get; set; }

        /// <summary>
        /// Gets the issue limits for individual <see cref="IIssueProvider"/>.
        /// The key must be the <see cref="IIssue.ProviderType"/> of a specific provider to which the limits should be applied to.
        /// Use <see cref="MaxIssuesToPostForEachIssueProvider"/> to set the same limit to all issue providers.
        /// </summary>
        Dictionary<string, IProviderIssueLimits> ProviderIssueLimits { get; }

        /// <summary>
        /// Gets list of filter functions which should be applied before posting issues to pull requests.
        /// </summary>
        IList<Func<IEnumerable<IIssue>, IEnumerable<IIssue>>> IssueFilters { get; }

        /// <summary>
        /// Gets or sets a value indicating whether a status on the pull request should be set if there are any issues found.
        /// The status is succeeded if there are no issues and fails as soon as issues from any issue provider or run have been found.
        /// Use <see cref="ShouldSetSeparatePullRequestStatusForEachIssueProviderAndRun"/> to report additional status for issues of every issue provider and run.
        /// Default value is <c>true</c>.
        /// </summary>
        bool ShouldSetPullRequestStatus { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether a separate status should be set for issues of every issue provider and run.
        /// Use <see cref="ShouldSetPullRequestStatus"/> to report status across all issue providers and runs.
        /// Default value is <c>true</c>.
        /// </summary>
        bool ShouldSetSeparatePullRequestStatusForEachIssueProviderAndRun { get; set; }
    }
}