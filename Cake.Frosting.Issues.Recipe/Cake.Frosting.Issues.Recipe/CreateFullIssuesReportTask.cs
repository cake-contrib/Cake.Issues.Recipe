using Cake.Common.IO;
using Cake.Issues;
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
            context.NotNull(nameof(context));

            return context.Parameters.Reporting.ShouldCreateFullIssuesReport;
        }

        /// <inheritdoc/>
        public override void Run(IssuesContext context)
        {
            context.NotNull(nameof(context));

            var reportFileName = "report";
            if (!string.IsNullOrWhiteSpace(context.Parameters.BuildIdentifier))
            {
                reportFileName += $"-{context.Parameters.BuildIdentifier}";
            }
            reportFileName += ".html";

            context.State.FullIssuesReport =
                context.Parameters.OutputDirectory.CombineWithFilePath(reportFileName);
            context.EnsureDirectoryExists(context.Parameters.OutputDirectory);

            // Create HTML report.
            context.CreateIssueReport(
                context.State.Issues,
                context.GenericIssueReportFormat(context.Parameters.Reporting.FullIssuesReportSettings),
                context.State.ProjectRootDirectory,
                context.State.FullIssuesReport);
        }
    }
}
