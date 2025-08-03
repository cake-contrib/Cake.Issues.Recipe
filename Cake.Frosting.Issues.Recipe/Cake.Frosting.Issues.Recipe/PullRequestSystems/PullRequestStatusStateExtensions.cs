namespace Cake.Frosting.Issues.Recipe;

using Cake.AzureDevOps.Repos.PullRequest;
using Cake.GitHub;

/// <summary>
/// Extension methods for <see cref="PullRequestStatusState"/>.
/// </summary>
internal static class PullRequestStatusStateExtensions
{
    /// <summary>
    /// Converts the <see cref="PullRequestStatusState"/> to <see cref="AzureDevOpsPullRequestStatusState"/>.
    /// </summary>
    /// <param name="status">Status to convert.</param>
    /// <returns>Converted status.</returns>
    public static AzureDevOpsPullRequestStatusState ToAzureDevOpsPullRequestStatusState(this PullRequestStatusState status) =>
        status switch
        {
            PullRequestStatusState.Succeeded => AzureDevOpsPullRequestStatusState.Succeeded,
            PullRequestStatusState.Failed => AzureDevOpsPullRequestStatusState.Failed,
            _ => AzureDevOpsPullRequestStatusState.Succeeded,
        };

    /// <summary>
    /// Converts the <see cref="PullRequestStatusState"/> to GitHub status state.
    /// </summary>
    /// <param name="status">Status to convert.</param>
    /// <returns>Converted status.</returns>
    public static GitHubStatusState ToGitHubStatusState(this PullRequestStatusState status) =>
        status switch
        {
            PullRequestStatusState.Succeeded => GitHubStatusState.Success,
            PullRequestStatusState.Failed => GitHubStatusState.Failure,
            _ => GitHubStatusState.Success,
        };
}
