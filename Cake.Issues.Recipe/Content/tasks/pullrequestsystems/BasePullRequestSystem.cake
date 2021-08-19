/// <summary>
/// Basic implementation for all pull request server.
/// </summary>
public abstract class BasePullRequestSystem : IIssuesPullRequestSystem
{
    /// <inheritdoc />
    public abstract void ReportIssuesToPullRequest(
        ICakeContext context,
        IssuesData data);

    /// <inheritdoc />
    public abstract void SetPullRequestIssuesState(
        ICakeContext context,
        IssuesData data);

    /// <inheritdoc />
    public abstract FileLinkSettings GetFileLinkSettings(
        ICakeContext context,
        IssuesData data);

    /// <summary>
    /// Returns settings for reporting issues to pull requests.
    /// </summary>
    /// <param name="context">The Cake context.</param>
    /// <returns>Settings for reporting issues to pull requests.</returns>
    protected static IReportIssuesToPullRequestSettings GetReportIssuesToPullRequestSettings(IssuesData data)
    {
        var settings =
            new ReportIssuesToPullRequestSettings(data.ProjectRootDirectory)
            {
                MaxIssuesToPost = IssuesParameters.PullRequestSystem.MaxIssuesToPost,
                MaxIssuesToPostAcrossRuns = IssuesParameters.PullRequestSystem.MaxIssuesToPostAcrossRuns,
                MaxIssuesToPostForEachIssueProvider = IssuesParameters.PullRequestSystem.MaxIssuesToPostForEachIssueProvider
            };
        foreach (var providerIssueLimit in IssuesParameters.PullRequestSystem.ProviderIssueLimits)
        {
            settings.ProviderIssueLimits.Add(providerIssueLimit.Key, providerIssueLimit.Value);
        }
        return settings;
    }
}