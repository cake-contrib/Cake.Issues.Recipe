namespace Cake.Frosting.Issues.Recipe;

/// <summary>
/// Parameters for pull request integration.
/// </summary>
public class IssuesParametersPullRequestSystem : IIssuesParametersPullRequestSystem
{
    /// <inheritdoc />
    public bool ShouldReportIssuesToPullRequest { get; set; } = true;

    /// <inheritdoc />
    public bool ShouldSetPullRequestStatus { get; set; } = true;

    /// <inheritdoc />
    public bool ShouldSetSeparatePullRequestStatusForEachIssueProviderAndRun { get; set; } = true;

    /// <inheritdoc />
    public int? MaxIssuesToPost { get; set; }

    /// <inheritdoc />
    public int? MaxIssuesToPostAcrossRuns { get; set; }

    /// <inheritdoc />
    public int? MaxIssuesToPostForEachIssueProvider { get; set; } = 100;

    /// <inheritdoc />
    public Dictionary<string, IProviderIssueLimits> ProviderIssueLimits { get; } = [];

    /// <inheritdoc />
    public IList<Func<IEnumerable<IIssue>, IEnumerable<IIssue>>> IssueFilters { get; } = [];
}