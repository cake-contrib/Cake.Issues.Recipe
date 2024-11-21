/// <summary>
/// Support for builds running on GitHub Actions.
/// </summary>
public class GitHubActionsBuildServer : BaseBuildServer
{
    /// <inheritdoc />
    public override Uri DetermineRepositoryRemoteUrl(
        ICakeContext context,
        DirectoryPath repositoryRootDirectory)
    {
        context.NotNull();

        return new System.Uri($"https://github.com/{context.GitHubActions().Environment.Workflow.Repository}.git");
    }

    /// <inheritdoc />
    public override string DetermineCommitId(
        ICakeContext context,
        DirectoryPath repositoryRootDirectory)
    {
        context.NotNull();

        return context.GitHubActions().Environment.Workflow.Sha;
    }

    /// <inheritdoc />
    public override bool DetermineIfPullRequest(ICakeContext context)
    {
        context.NotNull();

        return context.GitHubActions().Environment.PullRequest.IsPullRequest;
    }

    /// <inheritdoc />
    public override int? DeterminePullRequestId(ICakeContext context)
    {
        context.NotNull();

        // Not supported by GitHub Actions
        return null;
   }

    /// <inheritdoc />
    public override void ReportIssuesToBuildServer(
        ICakeContext context,
        IssuesData data)
    {
        context.NotNull();
        data.NotNull();

        context.ReportIssuesToPullRequest(
            data.Issues,
            context.GitHubActionsBuilds(),
            data.ProjectRootDirectory);
    }

    /// <inheritdoc />
    public override void CreateSummaryIssuesReport(
        ICakeContext context,
        IssuesData data,
        [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "")
    {
        context.NotNull();
        data.NotNull();

        // Summary issues report is not supported for GitHub Actions.
    }

    /// <inheritdoc />
    public override void PublishIssuesArtifacts(ICakeContext context, IssuesData data)
    {
        context.NotNull();
        data.NotNull();

        // Publishing artifacts is currently not supported for GitHub Actions.
    }
}