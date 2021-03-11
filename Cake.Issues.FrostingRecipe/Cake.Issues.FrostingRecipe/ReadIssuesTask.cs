using Cake.Common.Diagnostics;
using Cake.Frosting;
using Cake.Issues.DupFinder;
using Cake.Issues.EsLint;
using Cake.Issues.InspectCode;
using Cake.Issues.Markdownlint;
using Cake.Issues.MsBuild;
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
            // Define default settings.
            var defaultSettings = new ReadIssuesSettings(context.State.BuildRootDirectory);

            if (context.State.PullRequestSystem != null)
            {
                defaultSettings.FileLinkSettings =
                    context.State.PullRequestSystem.GetFileLinkSettings(context);
            }

            // Read MSBuild log files created by XmlFileLogger.
            foreach (var logFile in context.Parameters.InputFiles.MsBuildXmlFileLoggerLogFilePaths)
            {
                context.State.AddIssues(
                    context.ReadIssues(
                        context.MsBuildIssuesFromFilePath(
                            logFile.Key,
                            context.MsBuildXmlFileLoggerFormat()),
                        this.GetSettings(logFile.Value, defaultSettings)));
            }

            // Read MSBuild binary log files.
            foreach (var logFile in context.Parameters.InputFiles.MsBuildBinaryLogFilePaths)
            {
                context.State.AddIssues(
                    context.ReadIssues(
                        context.MsBuildIssuesFromFilePath(
                            logFile.Key,
                            context.MsBuildBinaryLogFileFormat()),
                        this.GetSettings(logFile.Value, defaultSettings)));
            }

            // Read InspectCode log files.
            foreach (var logFile in context.Parameters.InputFiles.InspectCodeLogFilePaths)
            {
                context.State.AddIssues(
                    context.ReadIssues(
                        context.InspectCodeIssuesFromFilePath(logFile.Key),
                        this.GetSettings(logFile.Value, defaultSettings)));
            }

            // Read dupFinder log files.
            foreach (var logFile in context.Parameters.InputFiles.DupFinderLogFilePaths)
            {
                context.State.AddIssues(
                    context.ReadIssues(
                        context.DupFinderIssuesFromFilePath(logFile.Key),
                        this.GetSettings(logFile.Value, defaultSettings)));
            }

            // Read markdownlint-cli log files.
            foreach (var logFile in context.Parameters.InputFiles.MarkdownlintCliLogFilePaths)
            {
                context.State.AddIssues(
                    context.ReadIssues(
                        context.MarkdownlintIssuesFromFilePath(
                            logFile.Key,
                            context.MarkdownlintCliLogFileFormat()),
                        this.GetSettings(logFile.Value, defaultSettings)));
            }

            // Read markdownlint log files in version 1.
            foreach (var logFile in context.Parameters.InputFiles.MarkdownlintV1LogFilePaths)
            {
                context.State.AddIssues(
                    context.ReadIssues(
                        context.MarkdownlintIssuesFromFilePath(
                            logFile.Key,
                            context.MarkdownlintV1LogFileFormat()),
                        this.GetSettings(logFile.Value, defaultSettings)));
            }

            // Read ESLint log files in JSON format.
            foreach (var logFile in context.Parameters.InputFiles.EsLintJsonLogFilePaths)
            {
                context.State.AddIssues(
                    context.ReadIssues(
                        context.EsLintIssuesFromFilePath(
                            logFile.Key,
                            context.EsLintJsonFormat()),
                        this.GetSettings(logFile.Value, defaultSettings)));
            }

            context.Information("{0} issues are found.", context.State.Issues.Count());
        }

        private IReadIssuesSettings GetSettings(IReadIssuesSettings configuredSettings, IReadIssuesSettings defaultSettings)
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
