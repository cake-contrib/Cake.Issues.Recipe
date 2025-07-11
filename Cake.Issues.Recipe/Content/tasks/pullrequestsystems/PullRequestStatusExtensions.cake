/// <summary>
/// Extension methods for <see cref="PullRequestStatus"/>.
/// </summary>
internal static class PullRequestStatusExtensions
{
    /// <summary>
    /// Converts the <see cref="PullRequestStatus"/> to <see cref="AzureDevOpsPullRequestStatus"/>.
    /// </summary>
    /// <param name="status">Status to convert.</param>
    /// <returns>Converted status.</returns>
    public static AzureDevOpsPullRequestStatus ToAzureDevOpsPullRequestStatus(PullRequestStatus status) =>
        new (status.Name)
        {
            Genre = status.Genre,
            State = PullRequestStatusStateExtensions.ToAzureDevOpsPullRequestStatusState(status.State),
            Description = status.Description
        };

    /// <summary>
    /// Converts the <see cref="PullRequestStatus"/> to <see cref="GitHubStatusSettings"/>.
    /// </summary>
    /// <param name="status">Status to convert.</param>
    /// <returns>Converted status.</returns>
    public static GitHubStatusSettings ToGitHubStatusSettings(PullRequestStatus status) =>
        new GitHubStatusSettings
        {
            State = PullRequestStatusStateExtensions.ToGitHubStatusState(status.State),
            Context = status.Name,
            Description = status.Description
        };
}