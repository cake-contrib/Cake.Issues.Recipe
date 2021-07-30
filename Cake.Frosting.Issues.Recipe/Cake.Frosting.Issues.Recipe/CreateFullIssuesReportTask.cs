using Cake.Common.IO;
using Cake.Issues.Reporting;
using Cake.Issues.Reporting.Generic;

namespace Cake.Frosting.Issues.Recipe
{
    /// <summary>
    /// Creates issue report.
    /// </summary>
    [TaskName("Create-FullIssuesReport")]
    [IsDependentOn(typeof(ReadIssuesTask))]
    public sealed class CreateFullIssuesReportTask : FrostingTask<IssuesContext>
    {
        /// <inheritdoc/>
        public override bool ShouldRun(IssuesContext context)
        {
            return context.Parameters.Reporting.ShouldCreateFullIssuesReport;
        }

        /// <inheritdoc/>
        public override void Run(IssuesContext context)
        {
            var reportFileName = "report";
            if (!string.IsNullOrWhiteSpace(context.Parameters.BuildIdentifier))
            {
                reportFileName += $"-{context.Parameters.BuildIdentifier}";
            }
            reportFileName += ".html";

            context.State.FullIssuesReport =
                context.Parameters.OutputDirectory.CombineWithFilePath(reportFileName);
            context.EnsureDirectoryExists(context.Parameters.OutputDirectory);

            // Create HTML report using DevExpress template.
            var settings =
                GenericIssueReportFormatSettings
                    .FromEmbeddedTemplate(GenericIssueReportTemplate.HtmlDxDataGrid)
                    .WithOption(HtmlDxDataGridOption.Theme, DevExtremeTheme.MaterialBlueLight);
            context.CreateIssueReport(
                context.State.Issues,
                context.GenericIssueReportFormat(settings),
                context.State.BuildRootDirectory,
                context.State.FullIssuesReport);
        }
    }
}
