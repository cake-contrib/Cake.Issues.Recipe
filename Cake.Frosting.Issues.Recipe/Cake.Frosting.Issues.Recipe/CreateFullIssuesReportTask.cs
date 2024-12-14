namespace Cake.Frosting.Issues.Recipe;

using Cake.Common.IO;

/// <summary>
/// Creates issue report.
/// </summary>
[TaskName("Create-FullIssuesReport")]
[IsDependentOn(typeof(ReadIssuesTask))]
public sealed class CreateFullIssuesReportTask : FrostingTask<IIssuesContext>
{
    /// <inheritdoc/>
    public override bool ShouldRun(IIssuesContext context)
    {
        context.NotNull();

        return context.Parameters.Reporting.ShouldCreateFullIssuesReport;
    }

    /// <inheritdoc/>
    public override void Run(IIssuesContext context)
    {
        context.NotNull();

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
