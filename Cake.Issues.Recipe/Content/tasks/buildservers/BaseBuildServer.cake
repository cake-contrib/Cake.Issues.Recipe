/// <summary>
/// Basic implementation for all build servers.
/// </summary>
public abstract class BaseBuildServer : IIssuesBuildServer
{
    /// <inheritdoc />
    public virtual Uri DetermineRepositoryRemoteUrl(
        ICakeContext context,
        DirectoryPath repositoryRootDirectory)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        if (repositoryRootDirectory == null)
        {
            throw new ArgumentNullException(nameof(repositoryRootDirectory));
        }

        var currentBranch = context.GitBranchCurrent(repositoryRootDirectory);
        return new Uri(currentBranch.Remotes.Single(x => x.Name == "origin").Url);
    }

    /// <inheritdoc />
    public virtual string DetermineCommitId(
        ICakeContext context,
        DirectoryPath repositoryRootDirectory)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        if (repositoryRootDirectory == null)
        {
            throw new ArgumentNullException(nameof(repositoryRootDirectory));
        }

        return context.GitLogTip(repositoryRootDirectory).Sha;
    }

    /// <inheritdoc />
    public virtual bool DetermineIfPullRequest(ICakeContext context)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        return false;
    }

    /// <inheritdoc />
    public virtual int? DeterminePullRequestId(ICakeContext context)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        return null;
   }

    /// <inheritdoc />
    public abstract void ReportIssuesToBuildServer(
        ICakeContext context,
        IssuesData data);

    /// <inheritdoc />
    public abstract void CreateSummaryIssuesReport(
        ICakeContext context,
        IssuesData data,
        [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "");

    /// <inheritdoc />
    public abstract void PublishIssuesArtifacts(
        ICakeContext context,
        IssuesData data);
}