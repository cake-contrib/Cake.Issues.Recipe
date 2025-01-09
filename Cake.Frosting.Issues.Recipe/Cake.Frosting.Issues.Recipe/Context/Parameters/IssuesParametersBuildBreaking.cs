namespace Cake.Frosting.Issues.Recipe;

/// <summary>
/// Parameters for passing input files.
/// </summary>
public class IssuesParametersBuildBreaking: IIssuesParametersBuildBreaking
{
    /// <inheritdoc />
    public bool ShouldFailBuildOnIssues { get; set; }

    /// <inheritdoc />
    public IssuePriority MinimumPriority { get; set; } = IssuePriority.Undefined;

    /// <inheritdoc />
    public IList<string> IssueProvidersToConsider { get; } = [];

    /// <inheritdoc />
    public IList<string> IssueProvidersToIgnore { get; } = [];
}