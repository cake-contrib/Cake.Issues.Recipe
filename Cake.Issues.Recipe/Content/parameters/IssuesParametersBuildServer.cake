/// <summary>
/// Parameters for build server integration.
/// </summary>
public class IssuesParametersBuildServer
{
    /// <summary>
    /// Gets or sets a value indicating whether full issues report should be published as artifact to the build system.
    /// Default value is <c>true</c>.
    /// </summary>
    public bool ShouldPublishFullIssuesReport { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether summary issues report should be created.
    /// Default value is <c>true</c>.
    /// </summary>
    public bool ShouldCreateSummaryIssuesReport { get; set; } = true;
}