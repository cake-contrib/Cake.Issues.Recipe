using Cake.Common.Build;
using Cake.Common.Diagnostics;
using Cake.Issues;

namespace Cake.Frosting.Issues.Recipe
{
    /// <summary>
    /// Report issues to build server.
    /// </summary>
    [TaskName("Report-IssuesToPullRequest")]
    [IsDependentOn(typeof(ReadIssuesTask))]
    public sealed class ReportIssuesToPullRequestTask : FrostingTask<IssuesContext>
    {
#pragma warning disable SA1123 // Do not place regions within elements
        #region DupFinder Exclusion
#pragma warning restore SA1123 // Do not place regions within elements
        /// <inheritdoc/>
        public override bool ShouldRun(IssuesContext context)
        {
            context.NotNull(nameof(context));

            return
                !context.BuildSystem().IsLocalBuild &&
                context.Parameters.PullRequestSystem.ShouldReportIssuesToPullRequest &&
                context.State.BuildServer != null && context.State.BuildServer.DetermineIfPullRequest(context);
        }
        #endregion

        /// <inheritdoc/>
        public override void Run(IssuesContext context)
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
