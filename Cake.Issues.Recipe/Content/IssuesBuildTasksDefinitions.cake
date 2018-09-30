/// <summary>
/// Class for holding shared data between tasks.
/// </summary>
public class IssuesBuildTaskDefinitions
{
    /// <summary>
    /// Gets or sets the task for analyzing and processing issues from linters and code analyzers.
    /// </summary>
    public CakeTaskBuilder IssuesTask { get; set; }
}
