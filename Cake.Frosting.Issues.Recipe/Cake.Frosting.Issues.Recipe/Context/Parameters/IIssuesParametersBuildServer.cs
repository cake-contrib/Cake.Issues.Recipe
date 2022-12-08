namespace Cake.Frosting.Issues.Recipe
{
    /// <summary>
    /// Parameters for build server integration.
    /// </summary>
    public interface IIssuesParametersBuildServer
    {
        /// <summary>
        /// Gets or sets a value indicating whether issues should be reported to the build server.
        /// Default value is <c>true</c>.
        /// </summary>
        bool ShouldReportIssuesToBuildServer { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether full issues report should be published as artifact to the build system.
        /// Default value is <c>true</c>.
        /// </summary>
        bool ShouldPublishFullIssuesReport { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether summary issues report should be created.
        /// Default value is <c>true</c>.
        /// </summary>
        bool ShouldCreateSummaryIssuesReport { get; set; }
    }
}