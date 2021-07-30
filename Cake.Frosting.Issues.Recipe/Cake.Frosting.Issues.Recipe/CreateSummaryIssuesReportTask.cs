using Cake.Common.Build;
using Cake.Common.Diagnostics;

namespace Cake.Frosting.Issues.Recipe
{
    /// <summary>
    /// Creates a summary issue report.
    /// </summary>
    [TaskName("Create-SummaryIssuesReport")]
    [IsDependentOn(typeof(ReadIssuesTask))]
    public sealed class CreateSummaryIssuesReportTask : FrostingTask<IssuesContext>
    {
        /// <inheritdoc/>
        public override bool ShouldRun(IssuesContext context)
        {
            return
                !context.BuildSystem().IsLocalBuild &&
                context.Parameters.BuildServer.ShouldCreateSummaryIssuesReport;
        }

        /// <inheritdoc/>
        public override void Run(IssuesContext context)
        {
            if (context.State.BuildServer == null)
            {
                context.Information("Not supported build server.");
                return;
            }

            context.State.BuildServer.CreateSummaryIssuesReport(context);
        }
    }
}
