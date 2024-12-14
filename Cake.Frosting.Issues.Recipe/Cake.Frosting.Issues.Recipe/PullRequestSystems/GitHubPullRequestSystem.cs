namespace Cake.Frosting.Issues.Recipe;

/// <summary>
/// Support for GitHub hosted code.
/// </summary>
internal sealed class GitHubPullRequestSystem : BasePullRequestSystem
{
    /// <inheritdoc />
    public override void ReportIssuesToPullRequest(IIssuesContext context) => context.NotNull(); // Not supported yet

    /// <inheritdoc />
    public override void SetPullRequestIssuesState(IIssuesContext context) => context.NotNull(); // Not supported yet

    /// <inheritdoc />
    public override FileLinkSettings GetFileLinkSettings(IIssuesContext context)
    {
        context.NotNull();

        var rootPath = context.State.RepositoryRootDirectory.GetRelativePath(context.State.ProjectRootDirectory);

        return context.IssueFileLinkSettingsForGitHubCommit(
            context.State.RepositoryRemoteUrl,
            context.State.CommitId,
            rootPath.FullPath);
    }
}