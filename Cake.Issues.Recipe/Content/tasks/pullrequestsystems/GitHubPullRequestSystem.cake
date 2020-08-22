/// <summary>
/// Support for GitHub hosted code.
/// </summary>
public class GitHubPullRequestSystem : BasePullRequestSystem
{
    /// <inheritdoc />
    public override void ReportIssuesToPullRequest(ICakeContext context, IssuesData data)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        if (data == null)
        {
            throw new ArgumentNullException(nameof(data));
        }

        // Not supported yet
    }

    /// <inheritdoc />
    public override void SetPullRequestIssuesState(ICakeContext context, IssuesData data)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        if (data == null)
        {
            throw new ArgumentNullException(nameof(data));
        }

        // Not supported yet
    }

    /// <inheritdoc />
    public override FileLinkSettings GetFileLinkSettings(ICakeContext context, IssuesData data)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        if (data == null)
        {
            throw new ArgumentNullException(nameof(data));
        }

        var rootPath = data.RepositoryRootDirectory.GetRelativePath(data.BuildRootDirectory);

        return context.IssueFileLinkSettingsForGitHubCommit(
            data.RepositoryRemoteUrl,
            data.CommitId,
            rootPath.FullPath);
    }
}