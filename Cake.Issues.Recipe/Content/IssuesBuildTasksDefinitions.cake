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
    /// Gets or sets the task for reporting issues to pull request.
    /// </summary>
    public CakeTaskBuilder ReportIssuesToPullRequestTask { get; set; }

    /// <summary>
    /// Gets or sets the task for reporting issues to Azure DevOps pull request.
    /// </summary>
    public CakeTaskBuilder ReportIssuesToAzureDevOpsPullRequestTask { get; set; }

    /// <summary>
    /// Gets or sets the task for settings the state on the pull request.
    /// </summary>
    public CakeTaskBuilder SetPullRequestIssuesStateTask { get; set; }

    /// <summary>
    /// Gets or sets the task for settings the state on an Azure DevOps pull request.
    /// </summary>
    public CakeTaskBuilder SetAzureDevOpsPullRequestIssuesStateTask { get; set; }
}
