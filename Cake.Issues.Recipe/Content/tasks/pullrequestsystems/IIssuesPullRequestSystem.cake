/// <summary>
/// Description of a pull request system implementation.
/// </summary>
public interface IIssuesPullRequestSystem
{
    /// <summary>
    /// Report issues as comments to pull request.
    /// </summary>
    /// <param name="context">The Cake context.</param>
    /// <param name="data">Object containing the issues.</param>
    void ReportIssuesToPullRequest(
        ICakeContext context,
        IssuesData data);

    /// <summary>
    /// Set pull request status.
    /// </summary>
    /// <param name="context">The Cake context.</param>
    /// <param name="data">Object containing the issues.</param>
    void SetPullRequestIssuesState(
        ICakeContext context,
        IssuesData data);

    /// <summary>
    /// Get settings for linking to files.
    /// </summary>
    /// <param name="context">The Cake context.</param>
    /// <param name="data">Object containing information about the build.</param>
    /// <returns>Settings for linking to files.</returns>
    FileLinkSettings GetFileLinkSettings(
        ICakeContext context,
        IssuesData data);
}