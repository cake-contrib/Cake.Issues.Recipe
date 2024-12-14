namespace Cake.Frosting.Issues.Recipe;

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
    /// Gets or sets the settings for the full issues report.
    /// By default <see cref="GenericIssueReportTemplate.HtmlDxDataGrid"/> template is used
    /// with <see cref="DevExtremeTheme.MaterialBlueLight"/> theme.
    /// </summary>
    GenericIssueReportFormatSettings FullIssuesReportSettings { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether report in SARIF format should be created.
    /// Default value is <c>true</c>.
    /// </summary>
    bool ShouldCreateSarifReport { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether issues should be written to the console.
    /// Default value is <c>false</c>.
    /// </summary>
    bool ShouldReportIssuesToConsole { get; set; }

    /// <summary>
    /// Gets or sets the settings for reporting issues to the console.
    /// </summary>
    ConsoleIssueReportFormatSettings ReportToConsoleSettings { get; set; }
}