namespace Cake.Frosting.Issues.Recipe
{
    using Cake.Common.Build;
    using Cake.Common.Diagnostics;

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
            context.NotNull();

            return !context.BuildSystem().IsLocalBuild;
        }

        /// <inheritdoc/>
        public override void Run(IIssuesContext context)
        {
            context.NotNull();

            if (context.State.BuildServer == null)
            {
                context.Information("Not supported build server.");
                return;
            }

            context.State.BuildServer.PublishIssuesArtifacts(context);
        }
    }
}
