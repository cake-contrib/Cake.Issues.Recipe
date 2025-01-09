namespace Cake.Frosting.Issues.Recipe;

/// <summary>
/// Parameters for build breaking.
/// </summary>
public interface IIssuesParametersBuildBreaking
{
    /// <summary>
    /// Gets or sets a value indicating whether build should fail if any issues are found.
    /// Default value is <c>false</c>.
    /// </summary>
    bool ShouldFailBuildOnIssues { get; set; }

    /// <summary>
    /// Gets or sets the minimum priority of issues considered to fail the build.
    /// If set to <see cref="IssuePriority.Undefined"/>, all issues are considered.
    /// Default value is <see cref="IssuePriority.Undefined"/>.
    /// </summary>
    IssuePriority MinimumPriority { get; set; }

    /// <summary>
    /// Gets the list of issue provider types to consider.
    /// If empty, all providers are considered.
    /// Default value is empty.
    /// </summary>
    IList<string> IssueProvidersToConsider { get; }

    /// <summary>
    /// Gets the list of issue provider types to ignore.
    /// Default value is empty.
    /// </summary>
    IList<string> IssueProvidersToIgnore { get; }
}