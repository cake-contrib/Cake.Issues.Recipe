namespace Cake.Frosting.Issues.Recipe;

using Cake.Common.Build;
using Cake.Common.Diagnostics;

/// <summary>
/// Report issues to build server.
/// </summary>
[TaskName("Report-IssuesToBuildServer")]
[IsDependentOn(typeof(ReadIssuesTask))]
public sealed class ReportIssuesToBuildServerTask : FrostingTask<IIssuesContext>
{
    /// <inheritdoc/>
    public override bool ShouldRun(IIssuesContext context)
    {
        context.NotNull();

        return
            !context.BuildSystem().IsLocalBuild &&
            context.Parameters.BuildServer.ShouldReportIssuesToBuildServer;
    }

    /// <inheritdoc/>
    public override void Run(IIssuesContext context)
    {
        context.NotNull();

        if (context.State.BuildServer == null)
        {
            context.Information("Not supported build server.");
            return;
        }

        context.State.BuildServer.ReportIssuesToBuildServer(context);
    }
}
