using Cake.Common;
using Cake.Common.Build;
using Cake.Common.Build.AzurePipelines.Data;
using Cake.Common.IO;
using Cake.Core.IO;
using Cake.Issues;
using Cake.Issues.Reporting;
using Cake.Issues.Reporting.Generic;
using System;
using System.IO;

namespace Cake.Frosting.Issues.Recipe
{
    /// <summary>
    /// Support for Azure DevOps / Azure Pipelines builds.
    /// </summary>
    internal class AzureDevOpsBuildServer : BaseBuildServer
    {
        /// <inheritdoc />
        public override Uri DetermineRepositoryRemoteUrl(
            IssuesContext context,
            DirectoryPath repositoryRootDirectory)
        {
            context.NotNull(nameof(context));

            return new Uri(context.EnvironmentVariable("BUILD_REPOSITORY_URI"));
        }

        /// <inheritdoc />
        public override string DetermineCommitId(
            IssuesContext context,
            DirectoryPath repositoryRootDirectory)
        {
            context.NotNull(nameof(context));

            return context.AzurePipelines().Environment.Repository.SourceVersion;
        }

        /// <inheritdoc />
        public override bool DetermineIfPullRequest(IssuesContext context)
        {
            context.NotNull(nameof(context));

            // Could be simplified once https://github.com/cake-build/cake/issues/2149 is fixed
            return !string.IsNullOrWhiteSpace(context.EnvironmentVariable("SYSTEM_PULLREQUEST_PULLREQUESTID"));
        }

        /// <inheritdoc />
        public override int? DeterminePullRequestId(IssuesContext context)
        {
            context.NotNull(nameof(context));

            if (!Int32.TryParse(context.EnvironmentVariable("SYSTEM_PULLREQUEST_PULLREQUESTID"), out var pullRequestId))
            {
                throw new Exception($"Invalid pull request ID: {context.EnvironmentVariable("SYSTEM_PULLREQUEST_PULLREQUESTID")}");
            }
            else
            {
                return pullRequestId;
            }
        }

        /// <inheritdoc />
        public override void ReportIssuesToBuildServer(
            IssuesContext context)
        {
            context.NotNull(nameof(context));

            foreach (var issue in context.State.Issues)
            {
                context.AzurePipelines().Commands.WriteWarning(
                    issue.MessageText,
                    new AzurePipelinesMessageData
                    {
                        SourcePath = issue.AffectedFileRelativePath?.FullPath,
                        LineNumber = issue.Line
                    });
            }
        }

        /// <inheritdoc />
        public override void CreateSummaryIssuesReport(
            IssuesContext context,
            [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "")
        {
            context.NotNull(nameof(context));

            var summaryFileName = "summary";
            if (!string.IsNullOrWhiteSpace(context.Parameters.BuildIdentifier))
            {
                summaryFileName += $"-{context.Parameters.BuildIdentifier}";
            }
            summaryFileName += ".md";
            var summaryFilePath = context.Parameters.OutputDirectory.CombineWithFilePath(summaryFileName);

            var templateName = "Cake.Frosting.Issues.Recipe.BuildServers.AzurePipelineSummary.cshtml";
            using (var stream = this.GetType().Assembly.GetManifestResourceStream(templateName))
            {
                if (stream == null)
                {
                    throw new ApplicationException($"Could not load resource {templateName}");
                }

                using (var sr = new StreamReader(stream))
                {
                    // Create summary for Azure Pipelines using custom template.
                    context.CreateIssueReport(
                        context.State.Issues,
                        context.GenericIssueReportFormatFromContent(sr.ReadToEnd()),
                        context.State.ProjectRootDirectory,
                        summaryFilePath);
                }
            }

            context.AzurePipelines().Commands.UploadTaskSummary(summaryFilePath);
        }

        /// <inheritdoc />
        public override void PublishIssuesArtifacts(IssuesContext context)
        {
            context.NotNull(nameof(context));

            if (context.Parameters.BuildServer.ShouldPublishFullIssuesReport &&
                context.State.FullIssuesReport != null &&
                context.FileExists(context.State.FullIssuesReport))
            {
                context.AzurePipelines().Commands.UploadArtifact("Issues", context.State.FullIssuesReport, "Issues");
            }
        }
    }
}