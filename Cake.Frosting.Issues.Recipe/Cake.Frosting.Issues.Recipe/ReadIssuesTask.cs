namespace Cake.Frosting.Issues.Recipe
{
    using Cake.Common.Diagnostics;

    /// <summary>
    /// Reads issues from the provided log files.
    /// </summary>
    [TaskName("Read-Issues")]
    public sealed class ReadIssuesTask : FrostingTask<IIssuesContext>
    {
        /// <inheritdoc/>
        public override void Run(IIssuesContext context)
        {
            context.NotNull(nameof(context));

            // Read MSBuild log files created by XmlFileLogger.
            foreach (var logFile in context.Parameters.InputFiles.MsBuildXmlFileLoggerLogFilePaths)
            {
                context.State.AddIssues(
                    context.MsBuildIssuesFromFilePath(
                        logFile.Key,
                        context.MsBuildXmlFileLoggerFormat()), 
                    logFile.Value);
            }

            // Read MSBuild binary log files.
            foreach (var logFile in context.Parameters.InputFiles.MsBuildBinaryLogFilePaths)
            {
                context.State.AddIssues(
                    context.MsBuildIssuesFromFilePath(
                        logFile.Key,
                        context.MsBuildBinaryLogFileFormat()),
                    logFile.Value);
            }

            // Read InspectCode log files.
            foreach (var logFile in context.Parameters.InputFiles.InspectCodeLogFilePaths)
            {
                context.State.AddIssues(
                    context.InspectCodeIssuesFromFilePath(logFile.Key),
                    logFile.Value);
            }

            // Read markdownlint-cli log files.
            foreach (var logFile in context.Parameters.InputFiles.MarkdownlintCliLogFilePaths)
            {
                context.State.AddIssues(
                    context.MarkdownlintIssuesFromFilePath(
                        logFile.Key,
                        context.MarkdownlintCliLogFileFormat()),
                    logFile.Value);
            }

            // Read markdownlint-cli log files created with --json.
            foreach (var logFile in context.Parameters.InputFiles.MarkdownlintCliJsonLogFilePaths)
            {
                context.State.AddIssues(
                    context.MarkdownlintIssuesFromFilePath(
                        logFile.Key,
                        context.MarkdownlintCliJsonLogFileFormat()),
                    logFile.Value);
            }

            // Read markdownlint log files in version 1.
            foreach (var logFile in context.Parameters.InputFiles.MarkdownlintV1LogFilePaths)
            {
                context.State.AddIssues(
                    context.MarkdownlintIssuesFromFilePath(
                        logFile.Key,
                        context.MarkdownlintV1LogFileFormat()),
                    logFile.Value);
            }

            // Read ESLint log files in JSON format.
            foreach (var logFile in context.Parameters.InputFiles.EsLintJsonLogFilePaths)
            {
                context.State.AddIssues(
                    context.EsLintIssuesFromFilePath(
                        logFile.Key,
                        context.EsLintJsonFormat()),
                    logFile.Value);
            }

            context.Information("{0} issues are found.", context.State.Issues.Count());
        }
    }
}
