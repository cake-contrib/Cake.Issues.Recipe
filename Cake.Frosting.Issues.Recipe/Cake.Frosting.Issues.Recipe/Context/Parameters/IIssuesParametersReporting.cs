using Cake.Issues.Reporting.Generic;

namespace Cake.Frosting.Issues.Recipe
{
    /// <summary>
    /// Parameters for reporting.
    /// </summary>
    public interface IIssuesParametersReporting
    {
        /// <summary>
        /// Gets or sets a value indicating whether full issues report should be created.
        /// Default value is <c>true</c>.
        /// </summary>
        bool ShouldCreateFullIssuesReport { get; set; }

        /// <summary>
        /// Gets the settings for the full issues report.
        /// By default <see cref="GenericIssueReportTemplate.HtmlDxDataGrid"/> template is used
        /// with <see cref="DevExtremeTheme.MaterialBlueLight"/> theme.
        /// </summary>
        GenericIssueReportFormatSettings FullIssuesReportSettings { get; }
    }
}