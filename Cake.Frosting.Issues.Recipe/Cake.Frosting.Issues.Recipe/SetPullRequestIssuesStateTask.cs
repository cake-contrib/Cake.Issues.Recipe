using Cake.Common.Build;
using Cake.Common.Diagnostics;
using Cake.Issues;

namespace Cake.Frosting.Issues.Recipe
{
    /// <summary>
    /// Set pull request status.
    /// </summary>
    [TaskName("Set-PullRequestIssuesState")]
    [IsDependentOn(typeof(ReadIssuesTask))]
    public sealed class SetPullRequestIssuesStateTask : FrostingTask<IIssuesContext>
    {
#pragma warning disable SA1123 // Do not place regions within elements
        #region DupFinder Exclusion
#pragma warning restore SA1123 // Do not place regions within elements
        /// <inheritdoc/>
        public override bool ShouldRun(IIssuesContext context)
        {
            context.NotNull(nameof(context));

            return
                !context.BuildSystem().IsLocalBuild &&
                context.Parameters.PullRequestSystem.ShouldSetPullRequestStatus &&
                context.State.BuildServer != null && context.State.BuildServer.DetermineIfPullRequest(context);
        }
        #endregion

        /// <inheritdoc/>
        public override void Run(IIssuesContext context)
        {
            context.NotNull(nameof(context));

            if (context.State.PullRequestSystem == null)
            {
                context.Information("Not supported pull request system.");
                return;
            }

            context.State.PullRequestSystem.SetPullRequestIssuesState(context);
        }
    }
}
