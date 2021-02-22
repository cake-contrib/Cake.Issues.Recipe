using Cake.Common.Build;
using Cake.Common.Diagnostics;
using Cake.Frosting;

namespace Cake.Issues.FrostingRecipe
{
    /// <summary>
    /// Report issues to build server.
    /// </summary>
    [TaskName("Report-IssuesToBuildServer")]
    [IsDependentOn(typeof(ReadIssuesTask))]
    public sealed class ReportIssuesToBuildServerTask : FrostingTask<IssuesContext>
    {
        /// <inheritdoc/>
        public override bool ShouldRun(IssuesContext context)
        {
            return 
                !context.BuildSystem().IsLocalBuild &&
                context.Parameters.BuildServer.ShouldReportIssuesToBuildServer;
        }

        /// <inheritdoc/>
        public override void Run(IssuesContext context)
        {
            if (context.State.BuildServer == null)
            {
                context.Information("Not supported build server.");
                return;
            }

            context.State.BuildServer.ReportIssuesToBuildServer(context);
        }
    }
}
