﻿using Cake.Common.Build;
using Cake.Common.Diagnostics;
using Cake.Issues;

namespace Cake.Frosting.Issues.Recipe
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
            context.NotNull(nameof(context));

            return
                !context.BuildSystem().IsLocalBuild &&
                context.Parameters.BuildServer.ShouldReportIssuesToBuildServer;
        }

        /// <inheritdoc/>
        public override void Run(IssuesContext context)
        {
            context.NotNull(nameof(context));

            if (context.State.BuildServer == null)
            {
                context.Information("Not supported build server.");
                return;
            }

            context.State.BuildServer.ReportIssuesToBuildServer(context);
        }
    }
}
