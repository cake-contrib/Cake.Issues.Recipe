using Cake.Common.Diagnostics;
using Cake.Frosting;
using Cake.Issues.DupFinder;
using Cake.Issues.EsLint;
using Cake.Issues.InspectCode;
using Cake.Issues.Markdownlint;
using Cake.Issues.MsBuild;
using System.Collections.Generic;
using System.Linq;

namespace Cake.Issues.FrostingRecipe
{
    /// <summary>
    /// Reads issues from the provided log files.
    /// </summary>
    [TaskName("Read-Issues")]
    public sealed class ReadIssuesTask : FrostingTask<IssuesContext>
    {
        /// <inheritdoc/>
        public override void Run(IssuesContext context)
        {
            // Determine which issue providers should be used.
            var issueProviders = new List<IIssueProvider>();

            if (context.Parameters.InputFiles.MsBuildXmlFileLoggerLogFilePath != null)
            {
                issueProviders.Add(
                    context.MsBuildIssuesFromFilePath(
                        context.Parameters.InputFiles.MsBuildXmlFileLoggerLogFilePath,
                        context.MsBuildXmlFileLoggerFormat()));
            }

            if (context.Parameters.InputFiles.MsBuildBinaryLogFilePath != null)
            {
                issueProviders.Add(
                    context.MsBuildIssuesFromFilePath(
                        context.Parameters.InputFiles.MsBuildBinaryLogFilePath,
                        context.MsBuildBinaryLogFileFormat()));
            }

            if (context.Parameters.InputFiles.InspectCodeLogFilePath != null)
            {
                issueProviders.Add(
                    context.InspectCodeIssuesFromFilePath(
                        context.Parameters.InputFiles.InspectCodeLogFilePath));
            }

            if (context.Parameters.InputFiles.DupFinderLogFilePath != null)
            {
                issueProviders.Add(
                    context.DupFinderIssuesFromFilePath(
                        context.Parameters.InputFiles.DupFinderLogFilePath));
            }

            if (context.Parameters.InputFiles.MarkdownlintCliLogFilePath != null)
            {
                issueProviders.Add(
                    context.MarkdownlintIssuesFromFilePath(
                        context.Parameters.InputFiles.MarkdownlintCliLogFilePath,
                        context.MarkdownlintCliLogFileFormat()));
            }

            if (context.Parameters.InputFiles.MarkdownlintV1LogFilePath != null)
            {
                issueProviders.Add(
                    context.MarkdownlintIssuesFromFilePath(
                        context.Parameters.InputFiles.MarkdownlintV1LogFilePath,
                        context.MarkdownlintV1LogFileFormat()));
            }

            if (context.Parameters.InputFiles.EsLintJsonLogFilePath != null)
            {
                issueProviders.Add(
                    context.EsLintIssuesFromFilePath(
                        context.Parameters.InputFiles.EsLintJsonLogFilePath,
                        context.EsLintJsonFormat()));
            }

            if (!issueProviders.Any())
            {
                context.Information("No files to process...");
                return;
            }

            // Read issues from log files.
            var settings = new ReadIssuesSettings(context.State.BuildRootDirectory);

            if (context.State.PullRequestSystem != null)
            {
                settings.FileLinkSettings =
                    context.State.PullRequestSystem.GetFileLinkSettings(context);
            }

            context.State.AddIssues(
                context.ReadIssues(
                    issueProviders,
                    settings));

            context.Information("{0} issues are found.", context.State.Issues.Count());
        }
    }
}
