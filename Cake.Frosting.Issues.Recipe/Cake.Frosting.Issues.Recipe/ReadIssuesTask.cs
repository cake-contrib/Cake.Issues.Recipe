namespace Cake.Frosting.Issues.Recipe;

using Cake.Common.Diagnostics;
using Cake.Issues.MsBuild;

/// <summary>
/// Reads issues from the provided log files.
/// </summary>
[TaskName("Read-Issues")]
public sealed class ReadIssuesTask : FrostingTask<IIssuesContext>
{
    /// <inheritdoc/>
    public override void Run(IIssuesContext context)
    {
        context.NotNull();

        // Read MSBuild log files created by XmlFileLogger.
        foreach (var logFile in context.Parameters.InputFiles.MsBuildXmlFileLoggerLogFilePaths)
        {
            context.State.AddIssues(
                context.MsBuildIssuesFromFilePath(
                    logFile.Key,
                    context.MsBuildXmlFileLoggerFormat()),
                logFile.Value);
        }

        // Read MSBuild log content created by XmlFileLogger.
        foreach (var logFileContent in context.Parameters.InputFiles.MsBuildXmlFileLoggerLogFileContent)
        {
            context.State.AddIssues(
                context.MsBuildIssues(
                    new MsBuildIssuesSettings(
                        logFileContent.Key,
                        context.MsBuildXmlFileLoggerFormat())),
                logFileContent.Value);
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

        // Read MSBuild binary log content.
        foreach (var logFileContent in context.Parameters.InputFiles.MsBuildBinaryLogFileContent)
        {
            context.State.AddIssues(
                context.MsBuildIssues(
                    new MsBuildIssuesSettings(
                        logFileContent.Key,
                        context.MsBuildBinaryLogFileFormat())),
                logFileContent.Value);
        }

        // Read InspectCode log files.
        foreach (var logFile in context.Parameters.InputFiles.InspectCodeLogFilePaths)
        {
            context.State.AddIssues(
                context.InspectCodeIssuesFromFilePath(logFile.Key),
                logFile.Value);
        }

        // Read InspectCode log content.
        foreach (var logFileContent in context.Parameters.InputFiles.InspectCodeLogFileContent)
        {
            context.State.AddIssues(
                context.InspectCodeIssues(
                    new InspectCodeIssuesSettings(logFileContent.Key)),
                logFileContent.Value);
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

        // Read markdownlint-cli log content.
        foreach (var logFileContent in context.Parameters.InputFiles.MarkdownlintCliLogFileContent)
        {
            context.State.AddIssues(
                context.MarkdownlintIssues(
                    new MarkdownlintIssuesSettings(
                        logFileContent.Key,
                        context.MarkdownlintCliLogFileFormat())),
                logFileContent.Value);
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

        // Read markdownlint-cli log content created with --json.
        foreach (var logFileContent in context.Parameters.InputFiles.MarkdownlintCliJsonLogFileContent)
        {
            context.State.AddIssues(
                context.MarkdownlintIssues(
                    new MarkdownlintIssuesSettings(
                        logFileContent.Key,
                        context.MarkdownlintCliJsonLogFileFormat())),
                logFileContent.Value);
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

        // Read markdownlint log content in version 1.
        foreach (var logFileContent in context.Parameters.InputFiles.MarkdownlintV1LogFileContent)
        {
            context.State.AddIssues(
                context.MarkdownlintIssues(
                    new MarkdownlintIssuesSettings(
                        logFileContent.Key,
                        context.MarkdownlintV1LogFileFormat())),
                logFileContent.Value);
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

        // Read ESLint log content in JSON format.
        foreach (var logFileContent in context.Parameters.InputFiles.EsLintJsonLogFileContent)
        {
            context.State.AddIssues(
                context.EsLintIssues(
                    new EsLintIssuesSettings(
                        logFileContent.Key,
                        context.EsLintJsonFormat())),
                logFileContent.Value);
        }

        // Read SARIF log files.
        foreach (var logFile in context.Parameters.InputFiles.SarifLogFilePaths)
        {
            context.State.AddIssues(
                context.SarifIssues(
                    new SarifIssuesSettings(logFile.Key)
                    {
                        // Since there might be multiple SARIF log files we need to have a predictable
                        // issue provider name for reporting pull request states.
                        UseToolNameAsIssueProviderName = false
                    }),
                logFile.Value);
        }

        // Read SARIF content.
        foreach (var logFileContent in context.Parameters.InputFiles.SarifLogFileContent)
        {
            context.State.AddIssues(
                context.SarifIssues(
                    new SarifIssuesSettings(logFileContent.Key)
                    {
                        // Since there might be multiple SARIF log files we need to have a predictable
                        // issue provider name for reporting pull request states.
                        UseToolNameAsIssueProviderName = false
                    }),
                logFileContent.Value);
        }

        // Read generic TAP log files.
        foreach (var logFile in context.Parameters.InputFiles.GenericTapLogFilePaths)
        {
            context.State.AddIssues(
                context.TapIssues(
                    new TapIssuesSettings(logFile.Key, context.GenericLogFileFormat())),
                logFile.Value);
        }

        // Read generic TAP content.
        foreach (var logFileContent in context.Parameters.InputFiles.GenericTapLogFileContent)
        {
            context.State.AddIssues(
                context.TapIssues(
                    new TapIssuesSettings(logFileContent.Key, context.GenericLogFileFormat())),
                logFileContent.Value);
        }

        // Read Stylelint TAP log files.
        foreach (var logFile in context.Parameters.InputFiles.StylelintTapLogFilePaths)
        {
            context.State.AddIssues(
                context.TapIssues(
                    new TapIssuesSettings(logFile.Key, context.StylelintLogFileFormat())),
                logFile.Value);
        }

        // Read Stylelint TAP content.
        foreach (var logFileContent in context.Parameters.InputFiles.StylelintTapLogFileContent)
        {
            context.State.AddIssues(
                context.TapIssues(
                    new TapIssuesSettings(logFileContent.Key, context.StylelintLogFileFormat())),
                logFileContent.Value);
        }

        // Read Textlint TAP log files.
        foreach (var logFile in context.Parameters.InputFiles.TextlintTapLogFilePaths)
        {
            context.State.AddIssues(
                context.TapIssues(
                    new TapIssuesSettings(logFile.Key, context.TextlintLogFileFormat())),
                logFile.Value);
        }

        // Read Textlint TAP content.
        foreach (var logFileContent in context.Parameters.InputFiles.TextlintTapLogFileContent)
        {
            context.State.AddIssues(
                context.TapIssues(
                    new TapIssuesSettings(logFileContent.Key, context.TextlintLogFileFormat())),
                logFileContent.Value);
        }

        context.Information("{0} issues are found.", context.State.Issues.Count());
    }
}
