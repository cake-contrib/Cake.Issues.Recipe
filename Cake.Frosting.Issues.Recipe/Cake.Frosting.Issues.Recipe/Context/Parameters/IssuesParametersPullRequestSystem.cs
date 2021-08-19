using Cake.Issues;
using Cake.Issues.PullRequests;
using System.Collections.Generic;

namespace Cake.Frosting.Issues.Recipe
{
    /// <summary>
    /// Parameters for pull request integration.
    /// </summary>
    public class IssuesParametersPullRequestSystem
    {
        /// <summary>
        /// Gets or sets a value indicating whether issues should be reported to the pull request system.
        /// Default value is <c>true</c>.
        /// </summary>
        public bool ShouldReportIssuesToPullRequest { get; set; } = true;

        /// <summary>
        /// Gets or sets the global number of issues which should be posted at maximum over all
        /// <see cref="IIssueProvider"/>.
        /// Issues are filtered by <see cref="IIssue.Priority"/> and issues with an <see cref="IIssue.AffectedFileRelativePath"/>
        /// are prioritized.
        /// Default is <c>null</c> which won't set a global limit.
        /// Use <see cref="MaxIssuesToPostAcrossRuns"/> to set a limit across multiple runs.
        /// </summary>
        public int? MaxIssuesToPost { get; set; }

        /// <summary>
        /// Gets or sets the global number of issues which should be posted at maximum over all
        /// <see cref="IIssueProvider"/> and across multiple runs.
        /// Issues are filtered by <see cref="IIssue.Priority"/> and issues with an <see cref="IIssue.AffectedFileRelativePath"/>
        /// are prioritized.
        /// Default is <c>null</c> which won't set a limit across multiple runs.
        /// Use <see cref="MaxIssuesToPost"/> to set a limit for a single run.
        /// </summary>
        public int? MaxIssuesToPostAcrossRuns { get; set; }

        /// <summary>
        /// Gets or sets the number of issues which should be posted at maximum for each
        /// <see cref="IIssueProvider"/>.
        /// Issues are filtered by <see cref="IIssue.Priority"/> and issues with an <see cref="IIssue.AffectedFileRelativePath"/>
        /// are prioritized.
        /// <c>null</c> won't limit issues per issue provider.
        /// Default is to filter to 100 issues for each issue provider.
        /// Use <see cref="ProviderIssueLimits"/> to set limits for individual issue providers.
        /// </summary>
        public int? MaxIssuesToPostForEachIssueProvider { get; set; } = 100;

        /// <summary>
        /// Gets the issue limits for individual <see cref="IIssueProvider"/>.
        /// The key must be the <see cref="IIssue.ProviderType"/> of a specific provider to which the limits should be applied to.
        /// Use <see cref="MaxIssuesToPostForEachIssueProvider"/> to set the same limit to all issue providers.
        /// </summary>
        public Dictionary<string, IProviderIssueLimits> ProviderIssueLimits { get; } = new Dictionary<string, IProviderIssueLimits>();

        /// <summary>
        /// Gets or sets a value indicating whether a status on the pull request should be set.
        /// Default value is <c>true</c>.
        /// </summary>
        public bool ShouldSetPullRequestStatus { get; set; } = true;
    }
}