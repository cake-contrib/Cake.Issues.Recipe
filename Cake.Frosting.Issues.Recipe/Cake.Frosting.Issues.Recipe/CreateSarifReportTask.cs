namespace Cake.Frosting.Issues.Recipe
{
    using Cake.Common.IO;
    using Cake.Issues;
    using Cake.Issues.Reporting;
    using Cake.Issues.Reporting.Sarif;

    /// <summary>
    /// Creates issue report in SARIF format.
    /// </summary>
    [TaskName("Create-SarifReport")]
    [IsDependentOn(typeof(ReadIssuesTask))]
    public sealed class CreateSarifReportTask : FrostingTask<IIssuesContext>
    {
        /// <inheritdoc/>
        public override bool ShouldRun(IIssuesContext context)
        {
            context.NotNull(nameof(context));

            return context.Parameters.Reporting.ShouldCreateSarifReport;
        }

        /// <inheritdoc/>
        public override void Run(IIssuesContext context)
        {
            context.NotNull(nameof(context));

            var reportFileName = "report";
            if (!string.IsNullOrWhiteSpace(context.Parameters.BuildIdentifier))
            {
                reportFileName += $"-{context.Parameters.BuildIdentifier}";
            }
            reportFileName += ".sarif";

            context.State.SarifReport =
                context.Parameters.OutputDirectory.CombineWithFilePath(reportFileName);
            context.EnsureDirectoryExists(context.Parameters.OutputDirectory);

            // Create SARIF report.
            context.CreateIssueReport(
                context.State.Issues,
                context.SarifIssueReportFormat(),
                context.State.ProjectRootDirectory,
                context.State.SarifReport);
        }
    }
}
