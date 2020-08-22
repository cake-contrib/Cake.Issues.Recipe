/// <summary>
/// Description of a build server implementation.
/// </summary>
public interface IIssuesBuildServer
{
    /// <summary>
    /// Determines the repository remote url.
    /// </summary>
    /// <param name="context">The Cake context.</param>
    /// <param name="repositoryRootDirectory">The root directory of the repository.</param>
    /// <returns>The remote URL of the repository.</returns>
    Uri DetermineRepositoryRemoteUrl(
        ICakeContext context,
        DirectoryPath repositoryRootDirectory);

    /// <summary>
    /// Determines the SHA ID of the current commit.
    /// </summary>
    /// <param name="context">The Cake context.</param>
    /// <param name="repositoryRootDirectory">The root directory of the repository.</param>
    /// <returns>The SHA ID of the current commit.</returns>
    string DetermineCommitId(
        ICakeContext context,
        DirectoryPath repositoryRootDirectory);

    /// <summary>
    /// Determines whether the build is for a pull request.
    /// </summary>
    /// <param name="context">The Cake context.</param>
    /// <returns>A value indicating whether the build is for a pull request.</returns>
    bool DetermineIfPullRequest(ICakeContext context);

    /// <summary>
    /// Determines ID of the pull request.
    /// </summary>
    /// <param name="context">The Cake context.</param>
    /// <returns>ID of the pull request.</returns>
    int? DeterminePullRequestId(ICakeContext context);

    /// <summary>
    /// Reports issues to the build server.
    /// </summary>
    /// <param name="context">The Cake context.</param>
    /// <param name="data">Object containing the issues.</param>
    void ReportIssuesToBuildServer(
        ICakeContext context,
        IssuesData data);

    /// <summary>
    /// Creates a summary report of the issues.
    /// </summary>
    /// <param name="context">The Cake context.</param>
    /// <param name="data">Object containing the issues.</param>
    void CreateSummaryIssuesReport(
        ICakeContext context,
        IssuesData data,
        [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "");

    /// <summary>
    /// Publishes issues report as artifact to the build system.
    /// </summary>
    /// <param name="context">The Cake context.</param>
    /// <param name="data">Object containing the issues.</param>
    void PublishIssuesArtifacts(
        ICakeContext context,
        IssuesData data);
}