/// <summary>
/// Class for holding shared data between tasks.
/// </summary>
public class IssuesBuildTaskDefinitions
{
    /// <summary>
    /// Gets or sets the task for analyzing and processing issues from linters and code analyzers.
    /// </summary>
    public CakeTaskBuilder IssuesTask { get; set; }

    /// <summary>
    /// Gets or sets the task for reading issues.
    /// </summary>
    public CakeTaskBuilder ReadIssuesTask { get; set; }

    /// <summary>
    /// Gets or sets the task for creating a full issue report.
    /// </summary>
    public CakeTaskBuilder CreateFullIssuesReportTask { get; set; }

    /// <summary>
    /// Gets or sets the task for publishing issues artifacts.
    /// </summary>
    public CakeTaskBuilder PublishIssuesArtifactsTask { get; set; }

    /// <summary>
    /// Gets or sets the task for reporting issues to build server.
    /// </summary>
    public CakeTaskBuilder ReportIssuesToBuildServerTask { get; set; }

    /// <summary>
    /// Gets or sets the task for creating a summary issue report.
    /// </summary>
    public CakeTaskBuilder CreateSummaryIssuesReportTask { get; set; }

    /// <summary>
    /// Gets or sets the task for reporting issues to pull request.
    /// </summary>
    public CakeTaskBuilder ReportIssuesToPullRequestTask { get; set; }

    /// <summary>
    /// Gets or sets the task for settings the state on the pull request.
    /// </summary>
    public CakeTaskBuilder SetPullRequestIssuesStateTask { get; set; }
}
