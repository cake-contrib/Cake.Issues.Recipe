using Cake.Issues.Reporting.Generic;

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
        public GenericIssueReportFormatSettings FullIssuesReportSettings { get; } =
            GenericIssueReportFormatSettings
                .FromEmbeddedTemplate(GenericIssueReportTemplate.HtmlDxDataGrid)
                .WithOption(HtmlDxDataGridOption.Theme, DevExtremeTheme.MaterialBlueLight);
    }
}