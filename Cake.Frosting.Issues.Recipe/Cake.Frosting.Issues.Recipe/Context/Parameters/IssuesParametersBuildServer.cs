namespace Cake.Frosting.Issues.Recipe
{
    /// <summary>
    /// Parameters for build server integration.
    /// </summary>
    public class IssuesParametersBuildServer : IIssuesParametersBuildServer
    {
        /// <inheritdoc />
        public bool ShouldReportIssuesToBuildServer { get; set; } = true;

        /// <inheritdoc />
        public bool ShouldPublishFullIssuesReport { get; set; } = true;

        /// <inheritdoc />
        public bool ShouldPublishSarifReport { get; set; } = true;

        /// <inheritdoc />
        public bool ShouldCreateSummaryIssuesReport { get; set; } = true;
    }
}