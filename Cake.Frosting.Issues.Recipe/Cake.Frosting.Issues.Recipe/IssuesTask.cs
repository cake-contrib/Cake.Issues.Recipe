namespace Cake.Frosting.Issues.Recipe;
/// <summary>
/// Main task for issue management integration.
/// </summary>
[TaskName("Issues")]
[IsDependentOn(typeof(PublishIssuesArtifactsTask))]
[IsDependentOn(typeof(ReportIssuesToBuildServerTask))]
[IsDependentOn(typeof(CreateSummaryIssuesReportTask))]
[IsDependentOn(typeof(ReportIssuesToPullRequestTask))]
[IsDependentOn(typeof(SetPullRequestIssuesStateTask))]
[IsDependentOn(typeof(ReportIssuesToConsoleTask))]
public sealed class IssuesTask : FrostingTask<IIssuesContext>
{
    /// <inheritdoc/>
    public override void Run(IIssuesContext context)
    {
        context.NotNull();

        if (context.Parameters.BuildBreaking.ShouldFailBuildOnIssues)
        {
            context.BreakBuildOnIssues(
                context.State.Issues,
                new BuildBreakingSettings
                {
                    MinimumPriority = context.Parameters.BuildBreaking.MinimumPriority,
                    IssueProvidersToConsider = context.Parameters.BuildBreaking.IssueProvidersToConsider,
                    IssueProvidersToIgnore = context.Parameters.BuildBreaking.IssueProvidersToIgnore
                },
                x =>
                {
                    // Print issues to console before failing build.
                    _ = context.CreateIssueReport(
                        x,
                        context.ConsoleIssueReportFormat(
                            new ConsoleIssueReportFormatSettings
                            {
                                Compact = true,
                                GroupByRule = true,
                            }),
                        context.State.ProjectRootDirectory,
                        string.Empty);
                });
        }
    }
}
