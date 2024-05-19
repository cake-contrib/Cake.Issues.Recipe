namespace Cake.Frosting.Issues.Recipe
{
    using Cake.Common.Build;
    using Cake.Common.Diagnostics;

    /// <summary>
    /// Report issues to build server.
    /// </summary>
    [TaskName("Report-IssuesToPullRequest")]
    [IsDependentOn(typeof(ReadIssuesTask))]
    public sealed class ReportIssuesToPullRequestTask : FrostingTask<IIssuesContext>
    {
        /// <inheritdoc/>
        public override bool ShouldRun(IIssuesContext context)
        {
            context.NotNull(nameof(context));

            return
                !context.BuildSystem().IsLocalBuild &&
                context.Parameters.PullRequestSystem.ShouldReportIssuesToPullRequest &&
                context.State.BuildServer != null && context.State.BuildServer.DetermineIfPullRequest(context);
        }

        /// <inheritdoc/>
        public override void Run(IIssuesContext context)
        {
            context.NotNull(nameof(context));

            if (context.State.PullRequestSystem == null)
            {
                context.Information("Not supported pull request system.");
                return;
            }

            context.State.PullRequestSystem.ReportIssuesToPullRequest(context);
        }
    }
}
