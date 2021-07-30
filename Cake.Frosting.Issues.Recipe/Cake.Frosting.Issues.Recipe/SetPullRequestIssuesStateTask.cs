using Cake.Common.Build;
using Cake.Common.Diagnostics;

namespace Cake.Frosting.Issues.Recipe
{
    /// <summary>
    /// Set pull request status.
    /// </summary>
    [TaskName("Set-PullRequestIssuesState")]
    [IsDependentOn(typeof(ReadIssuesTask))]
    public sealed class SetPullRequestIssuesStateTask : FrostingTask<IssuesContext>
    {
#pragma warning disable SA1123 // Do not place regions within elements
        #region DupFinder Exclusion
#pragma warning restore SA1123 // Do not place regions within elements
        /// <inheritdoc/>
        public override bool ShouldRun(IssuesContext context)
        {
            return
                !context.BuildSystem().IsLocalBuild &&
                context.Parameters.PullRequestSystem.ShouldReportIssuesToPullRequest &&
                context.State.BuildServer != null && context.State.BuildServer.DetermineIfPullRequest(context);
        }
        #endregion

        /// <inheritdoc/>
        public override void Run(IssuesContext context)
        {
            if (context.State.PullRequestSystem == null)
            {
                context.Information("Not supported pull request system.");
                return;
            }

            context.State.PullRequestSystem.SetPullRequestIssuesState(context);
        }
    }
}
