/// <summary>
/// Support for GitHub hosted code.
/// </summary>
public class GitHubPullRequestSystem : BasePullRequestSystem
{
    /// <inheritdoc />
    public override void ReportIssuesToPullRequest(ICakeContext context, IssuesData data)
    {
        context.NotNull(nameof(context));
        data.NotNull(nameof(data));

        // Not supported yet
    }

    /// <inheritdoc />
    public override void SetPullRequestIssuesState(ICakeContext context, IssuesData data)
    {
        context.NotNull(nameof(context));
        data.NotNull(nameof(data));

        // Not supported yet
    }

    /// <inheritdoc />
    public override FileLinkSettings GetFileLinkSettings(ICakeContext context, IssuesData data)
    {
        context.NotNull(nameof(context));
        data.NotNull(nameof(data));

        var rootPath = data.RepositoryRootDirectory.GetRelativePath(data.BuildRootDirectory);

        return context.IssueFileLinkSettingsForGitHubCommit(
            data.RepositoryRemoteUrl,
            data.CommitId,
            rootPath.FullPath);
    }
}