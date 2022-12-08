/// <summary>
/// Parameters for reporting.
/// </summary>
public class IssuesParametersReporting
{
    /// <summary>
    /// Gets or sets a value indicating whether full issues report should be created.
    /// Default value is <c>true</c>.
    /// </summary>
    public bool ShouldCreateFullIssuesReport { get; set; } = true;

    /// <summary>
    /// Gets or sets the settings for the full issues report.
    /// By default <see cref="GenericIssueReportTemplate.HtmlDxDataGrid"/> template is used
    /// with <see cref="DevExtremeTheme.MaterialBlueLight"/> theme.
    /// </summary>
    public GenericIssueReportFormatSettings FullIssuesReportSettings { get; set; } =
        GenericIssueReportFormatSettings
            .FromEmbeddedTemplate(GenericIssueReportTemplate.HtmlDxDataGrid)
            .WithOption(HtmlDxDataGridOption.Theme, DevExtremeTheme.MaterialBlueLight);
}