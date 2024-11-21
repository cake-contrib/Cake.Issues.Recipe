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
        context.NotNull();
        repositoryRootDirectory.NotNull();

        var currentBranch = context.GitBranchCurrent(repositoryRootDirectory);
        return new Uri(currentBranch.Remotes.Single(x => x.Name == "origin").Url);
    }

    /// <inheritdoc />
    public virtual string DetermineCommitId(
        ICakeContext context,
        DirectoryPath repositoryRootDirectory)
    {
        context.NotNull();
        repositoryRootDirectory.NotNull();

        return context.GitLogTip(repositoryRootDirectory).Sha;
    }

    /// <inheritdoc />
    public virtual bool DetermineIfPullRequest(ICakeContext context)
    {
        context.NotNull();

        return false;
    }

    /// <inheritdoc />
    public virtual int? DeterminePullRequestId(ICakeContext context)
    {
        context.NotNull();

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