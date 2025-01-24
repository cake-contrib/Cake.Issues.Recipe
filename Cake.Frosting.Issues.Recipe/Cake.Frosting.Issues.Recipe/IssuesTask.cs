namespace Cake.Frosting.Issues.Recipe;

using Cake.Common.Diagnostics;

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
                    var issueCount = x.Count();
                    context.Error(
                        issueCount == 1
                        ? "{0} issue was found in the build."
                        : "{0} issues were found in the build",
                        issueCount);
                    context.Error("Issues containing file location information are printed below. Note that there might be more issues which cannot be listed.");
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
