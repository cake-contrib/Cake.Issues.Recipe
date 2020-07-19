public class GenericReporterData : AddinData
{
    private static GenericReporterData _reporter = null;

    private GenericReporterData(ICakeContext context)
    : base(context, "Cake.Issues.Reporting.Generic", CakeIssuesReportingGenericVersion)
    {
        _reporter = this;
    }

    public static GenericReporterData GetReporter(ICakeContext context)
    {
        return _reporter ?? new GenericReporterData(context);
    }

    public static IIssueReportFormat CreateIssueFormatFromEmbeddedTemplate(ICakeContext context, IssuesParametersReporting parameters)
    {
        var reporter = GetReporter(context);

        var settings = reporter.CallStaticMethod("FromEmbeddedTemplate", "HtmlDxDataGrid");

        var theme = "DevExtremeTheme.MaterialBlueLight"; // Should be changed to be set on IssuesParametersReporting settings class.
        reporter.CallStaticMethod("WithOption", settings, "HtmlDxDataGridOption.Theme", theme);

        var issueFormat = (IIssueReportFormat)reporter.CallStaticMethod("GenericIssueReportFormat", context, settings);

        return issueFormat;
    }

    public static IIssueReportFormat CreateIssueFormatFromFilePath(ICakeContext context, IssuesParametersReporting parameters, FilePath reportPath)
    {
        var reporter = GetReporter(context);

        var issueFormat = (IIssueReportFormat)reporter.CallStaticMethod("GenericIssueReportFormatFromFilePath", context, reportPath);

        return issueFormat;
    }
}