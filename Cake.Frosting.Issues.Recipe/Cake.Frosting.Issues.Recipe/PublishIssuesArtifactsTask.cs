using Cake.Common.Build;
using Cake.Common.Diagnostics;

namespace Cake.Frosting.Issues.Recipe
{
    /// <summary>
    /// Publish issue artifacts to build server.
    /// </summary>
    [TaskName("Publish-IssuesArtifacts")]
    [IsDependentOn(typeof(CreateFullIssuesReportTask))]
    public sealed class PublishIssuesArtifactsTask : FrostingTask<IssuesContext>
    {
        /// <inheritdoc/>
        public override bool ShouldRun(IssuesContext context)
        {
            return !context.BuildSystem().IsLocalBuild;
        }

        /// <inheritdoc/>
        public override void Run(IssuesContext context)
        {
            if (context.State.BuildServer == null)
            {
                context.Information("Not supported build server.");
                return;
            }

            context.State.BuildServer.PublishIssuesArtifacts(context);
        }
    }
}
