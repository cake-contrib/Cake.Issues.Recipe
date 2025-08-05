namespace Cake.Frosting.Issues.Recipe;

using Cake.AzureDevOps.Repos.PullRequest;
using Cake.GitHub;

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
    public static AzureDevOpsPullRequestStatus ToAzureDevOpsPullRequestStatus(this PullRequestStatus status) =>
        new(status.Name)
        {
            Genre = status.Genre,
            State = status.State.ToAzureDevOpsPullRequestStatusState(),
            Description = status.Description
        };

    /// <summary>
    /// Converts the <see cref="PullRequestStatus"/> to <see cref="GitHubStatusSettings"/>.
    /// </summary>
    /// <param name="status">Status to convert.</param>
    /// <returns>Converted status.</returns>
    public static GitHubStatusSettings ToGitHubStatusSettings(this PullRequestStatus status) =>
        new GitHubStatusSettings
        {
            State = status.State.ToGitHubStatusState(),
            Context = status.Name,
            Description = status.Description
        };
}
