namespace Cake.Frosting.Issues.Recipe
{
    using Cake.Common.Build;
    using Cake.Common.Diagnostics;
    using Cake.Issues;

    /// <summary>
    /// Set pull request status.
    /// </summary>
    [TaskName("Set-PullRequestIssuesState")]
    [IsDependentOn(typeof(ReadIssuesTask))]
    public sealed class SetPullRequestIssuesStateTask : FrostingTask<IIssuesContext>
    {
        /// <inheritdoc/>
        public override bool ShouldRun(IIssuesContext context)
        {
            context.NotNull(nameof(context));

            return
                !context.BuildSystem().IsLocalBuild &&
                (context.Parameters.PullRequestSystem.ShouldSetPullRequestStatus ||
                context.Parameters.PullRequestSystem.ShouldSetSeparatePullRequestStatusForEachIssueProviderAndRun) &&
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

            context.State.PullRequestSystem.SetPullRequestIssuesState(context);
        }
    }
}
