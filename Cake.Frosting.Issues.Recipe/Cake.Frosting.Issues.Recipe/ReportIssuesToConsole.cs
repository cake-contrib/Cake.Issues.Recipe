namespace Cake.Frosting.Issues.Recipe
{
    /// <summary>
    /// Reports issues to the console.
    /// </summary>
    [TaskName("Report-IssuesToConsole")]
    [IsDependentOn(typeof(ReadIssuesTask))]
    public sealed class ReportIssuesToConsoleTask : FrostingTask<IIssuesContext>
    {
        /// <inheritdoc/>
        public override bool ShouldRun(IIssuesContext context)
        {
            context.NotNull(nameof(context));

            return context.Parameters.Reporting.ShouldReportIssuesToConsole;
        }

        /// <inheritdoc/>
        public override void Run(IIssuesContext context)
        {
            context.NotNull(nameof(context));

            // Print issues to console.
            context.CreateIssueReport(
                context.State.Issues,
                context.ConsoleIssueReportFormat(context.Parameters.Reporting.ReportToConsoleSettings),
                context.State.ProjectRootDirectory,
                string.Empty);
        }
    }
}
