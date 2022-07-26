﻿using Cake.Common.Build;
using Cake.Common.Diagnostics;
using Cake.Issues;

namespace Cake.Frosting.Issues.Recipe
{
    /// <summary>
    /// Creates a summary issue report.
    /// </summary>
    [TaskName("Create-SummaryIssuesReport")]
    [IsDependentOn(typeof(ReadIssuesTask))]
    public sealed class CreateSummaryIssuesReportTask : FrostingTask<IIssuesContext>
    {
        /// <inheritdoc/>
        public override bool ShouldRun(IIssuesContext context)
        {
            context.NotNull(nameof(context));

            return
                !context.BuildSystem().IsLocalBuild &&
                context.Parameters.BuildServer.ShouldCreateSummaryIssuesReport;
        }

        /// <inheritdoc/>
        public override void Run(IIssuesContext context)
        {
            context.NotNull(nameof(context));

            if (context.State.BuildServer == null)
            {
                context.Information("Not supported build server.");
                return;
            }

            context.State.BuildServer.CreateSummaryIssuesReport(context);
        }
    }
}
