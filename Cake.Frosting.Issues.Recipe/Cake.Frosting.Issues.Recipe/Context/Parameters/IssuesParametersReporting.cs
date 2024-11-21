namespace Cake.Frosting.Issues.Recipe
{
    /// <summary>
    /// Parameters for reporting.
    /// </summary>
    public class IssuesParametersReporting : IIssuesParametersReporting
    {
        /// <inheritdoc />
        public bool ShouldCreateFullIssuesReport { get; set; } = true;

        /// <inheritdoc />
        public GenericIssueReportFormatSettings FullIssuesReportSettings { get; set; } =
            GenericIssueReportFormatSettings
                .FromEmbeddedTemplate(GenericIssueReportTemplate.HtmlDxDataGrid)
                .WithOption(HtmlDxDataGridOption.Theme, DevExtremeTheme.MaterialBlueLight);

        /// <inheritdoc />
        public bool ShouldCreateSarifReport { get; set; } = true;

        /// <inheritdoc />
        public bool ShouldReportIssuesToConsole { get; set; }

        /// <inheritdoc />
        public ConsoleIssueReportFormatSettings ReportToConsoleSettings { get; set; } =
            new ConsoleIssueReportFormatSettings();
    }
}