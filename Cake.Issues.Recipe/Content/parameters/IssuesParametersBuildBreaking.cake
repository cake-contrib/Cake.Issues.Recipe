/// <summary>
/// Parameters for build breaking.
/// </summary>
public class IssuesParametersBuildBreaking
{
    /// <summary>
    /// Gets or sets a value indicating whether build should fail if any issues are found.
    /// Default value is <c>false</c>.
    /// </summary>
    public bool ShouldFailBuildOnIssues { get; set; }

    /// <summary>
    /// Gets or sets the minimum priority of issues considered to fail the build.
    /// If set to <see cref="IssuePriority.Undefined"/>, all issues are considered.
    /// Default value is <see cref="IssuePriority.Undefined"/>.
    /// </summary>
    public IssuePriority MinimumPriority { get; set; } = IssuePriority.Undefined;

    /// <summary>
    /// Gets the list of issue provider types to consider.
    /// If empty, all providers are considered.
    /// Default value is empty.
    /// </summary>
    public IList<string> IssueProvidersToConsider { get; } = [];

    /// <summary>
    /// Gets the list of issue provider types to ignore.
    /// Default value is empty.
    /// </summary>
    public IList<string> IssueProvidersToIgnore { get; } = [];
}