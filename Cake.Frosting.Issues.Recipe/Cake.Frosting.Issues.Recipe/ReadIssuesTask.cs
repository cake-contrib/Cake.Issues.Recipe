using Cake.Common.Diagnostics;
using Cake.Issues;
using Cake.Issues.DupFinder;
using Cake.Issues.EsLint;
using Cake.Issues.InspectCode;
using Cake.Issues.Markdownlint;
using Cake.Issues.MsBuild;
using System.Linq;

namespace Cake.Frosting.Issues.Recipe
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
            context.NotNull(nameof(context));

            // Define default settings.
            var defaultSettings = new ReadIssuesSettings(context.State.ProjectRootDirectory);

            if (context.State.PullRequestSystem != null)
            {
                defaultSettings.FileLinkSettings =
                    context.State.PullRequestSystem.GetFileLinkSettings(context);
            }

            // Read MSBuild log files created by XmlFileLogger.
            foreach (var logFile in context.Parameters.InputFiles.MsBuildXmlFileLoggerLogFilePaths)
            {
                context.NotNull(nameof(context));

                context.State.AddIssues(
                    context.ReadIssues(
                        context.MsBuildIssuesFromFilePath(
                            logFile.Key,
                            context.MsBuildXmlFileLoggerFormat()),
                        GetSettings(logFile.Value, defaultSettings)));
            }

            // Read MSBuild binary log files.
            foreach (var logFile in context.Parameters.InputFiles.MsBuildBinaryLogFilePaths)
            {
                context.State.AddIssues(
                    context.ReadIssues(
                        context.MsBuildIssuesFromFilePath(
                            logFile.Key,
                            context.MsBuildBinaryLogFileFormat()),
                        GetSettings(logFile.Value, defaultSettings)));
            }

            // Read InspectCode log files.
            foreach (var logFile in context.Parameters.InputFiles.InspectCodeLogFilePaths)
            {
                context.State.AddIssues(
                    context.ReadIssues(
                        context.InspectCodeIssuesFromFilePath(logFile.Key),
                        GetSettings(logFile.Value, defaultSettings)));
            }

            // Read dupFinder log files.
            foreach (var logFile in context.Parameters.InputFiles.DupFinderLogFilePaths)
            {
                context.State.AddIssues(
                    context.ReadIssues(
                        context.DupFinderIssuesFromFilePath(logFile.Key),
                        GetSettings(logFile.Value, defaultSettings)));
            }

            // Read markdownlint-cli log files.
            foreach (var logFile in context.Parameters.InputFiles.MarkdownlintCliLogFilePaths)
            {
                context.State.AddIssues(
                    context.ReadIssues(
                        context.MarkdownlintIssuesFromFilePath(
                            logFile.Key,
                            context.MarkdownlintCliLogFileFormat()),
                        GetSettings(logFile.Value, defaultSettings)));
            }

            // Read markdownlint log files in version 1.
            foreach (var logFile in context.Parameters.InputFiles.MarkdownlintV1LogFilePaths)
            {
                context.State.AddIssues(
                    context.ReadIssues(
                        context.MarkdownlintIssuesFromFilePath(
                            logFile.Key,
                            context.MarkdownlintV1LogFileFormat()),
                        GetSettings(logFile.Value, defaultSettings)));
            }

            // Read ESLint log files in JSON format.
            foreach (var logFile in context.Parameters.InputFiles.EsLintJsonLogFilePaths)
            {
                context.State.AddIssues(
                    context.ReadIssues(
                        context.EsLintIssuesFromFilePath(
                            logFile.Key,
                            context.EsLintJsonFormat()),
                        GetSettings(logFile.Value, defaultSettings)));
            }

            context.Information("{0} issues are found.", context.State.Issues.Count());
        }

        private static IReadIssuesSettings GetSettings(IReadIssuesSettings configuredSettings, IReadIssuesSettings defaultSettings)
        {
            if (configuredSettings == null)
            {
                return defaultSettings;
            }

            if (configuredSettings.FileLinkSettings == null)
            {
                configuredSettings.FileLinkSettings = defaultSettings.FileLinkSettings;
            }

            return configuredSettings;
        }
    }
}
