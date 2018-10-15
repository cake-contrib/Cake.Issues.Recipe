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
    /// Gets or sets a value indicating whether a status on the pull request should be set.
    /// Default value is <c>true</c>.
    /// </summary>
    public bool ShouldSetPullRequestStatus { get; set; } = true;
}