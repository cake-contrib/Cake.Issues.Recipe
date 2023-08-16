using Cake.Common.Build;
using Cake.Common.Diagnostics;
using Cake.Issues;

namespace Cake.Frosting.Issues.Recipe
{
    /// <summary>
    /// Publish issue artifacts to build server.
    /// </summary>
    [TaskName("Publish-IssuesArtifacts")]
    [IsDependentOn(typeof(CreateFullIssuesReportTask))]
    [IsDependentOn(typeof(CreateSarifReportTask))]
    public sealed class PublishIssuesArtifactsTask : FrostingTask<IIssuesContext>
    {
        /// <inheritdoc/>
        public override bool ShouldRun(IIssuesContext context)
        {
            context.NotNull(nameof(context));

            return !context.BuildSystem().IsLocalBuild;
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

            context.State.BuildServer.PublishIssuesArtifacts(context);
        }
    }
}
