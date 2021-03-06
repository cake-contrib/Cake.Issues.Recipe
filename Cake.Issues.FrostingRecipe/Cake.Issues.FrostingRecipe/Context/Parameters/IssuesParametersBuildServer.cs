namespace Cake.Issues.FrostingRecipe
{
    /// <summary>
    /// Parameters for build server integration.
    /// </summary>
    public class IssuesParametersBuildServer
    {
        /// <summary>
        /// Gets or sets a value indicating whether issues should be reported to the build server.
        /// Default value is <c>true</c>.
        /// </summary>
        public bool ShouldReportIssuesToBuildServer { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether full issues report should be published as artifact to the build system.
        /// Default value is <c>true</c>.
        /// </summary>
        public bool ShouldPublishFullIssuesReport { get; set; } = true;

        // TODO Requires Cake.Issues.Reporting.Generic working with Frosting https://github.com/cake-contrib/Cake.Issues.Reporting.Generic/issues/361

        /// <summary>
        /// Gets or sets a value indicating whether summary issues report should be created.
        /// Default value is <c>true</c>.
        /// </summary>
        public bool ShouldCreateSummaryIssuesReport { get; set; } = false;
    }
}