namespace Cake.Frosting.Issues.Recipe
{
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
    public sealed class IssuesTask : FrostingTask
    {
    }
}
