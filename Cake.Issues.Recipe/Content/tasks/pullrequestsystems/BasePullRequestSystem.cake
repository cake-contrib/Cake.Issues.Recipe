/// <summary>
/// Basic implementation for all pull request server.
/// </summary>
public abstract class BasePullRequestSystem
{
    /// <inheritdoc />
    public abstract void ReportIssuesToPullRequest(
        ICakeContext context,
        IssuesData data);

    /// <inheritdoc />
    public abstract void SetPullRequestIssuesState(
        ICakeContext context,
        IssuesData data);
}