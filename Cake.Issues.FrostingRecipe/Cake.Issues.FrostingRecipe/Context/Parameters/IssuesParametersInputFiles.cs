using Cake.Core.IO;
using System.Collections.Generic;

namespace Cake.Issues.FrostingRecipe
{
    /// <summary>
    /// Parameters for passing input files.
    /// </summary>
    public class IssuesParametersInputFiles
    {
        /// <summary>
        /// Gets list of registered paths to MSBuild log files created by XmlFileLogger.
        /// </summary>
        public IDictionary<FilePath, IReadIssuesSettings> MsBuildXmlFileLoggerLogFilePaths { get; } = new Dictionary<FilePath, IReadIssuesSettings>();

        /// <summary>
        /// Gets list of registered paths to MSBuild binary log files.
        /// </summary>
        public IDictionary<FilePath, IReadIssuesSettings> MsBuildBinaryLogFilePaths { get; } = new Dictionary<FilePath, IReadIssuesSettings>();

        /// <summary>
        /// Gets list of registered paths to InspectCode log files.
        /// </summary>
        public IDictionary<FilePath, IReadIssuesSettings> InspectCodeLogFilePaths { get; } = new Dictionary<FilePath, IReadIssuesSettings>();

        /// <summary>
        /// Gets list of registered paths to dupFinder log files.
        /// </summary>
        public IDictionary<FilePath, IReadIssuesSettings> DupFinderLogFilePaths { get; } = new Dictionary<FilePath, IReadIssuesSettings>();

        /// <summary>
        /// Gets list of registered paths to markdownlint-cli log files.
        /// </summary>
        public IDictionary<FilePath, IReadIssuesSettings> MarkdownlintCliLogFilePaths { get; } = new Dictionary<FilePath, IReadIssuesSettings>();

        /// <summary>
        /// Gets list of registered paths to markdownlint log files in version 1.
        /// </summary>
        public IDictionary<FilePath, IReadIssuesSettings> MarkdownlintV1LogFilePaths { get; } = new Dictionary<FilePath, IReadIssuesSettings>();

        /// <summary>
        /// Gets list of registered paths to ESLint log files in JSON format.
        /// </summary>
        public IDictionary<FilePath, IReadIssuesSettings> EsLintJsonLogFilePaths { get; } = new Dictionary<FilePath, IReadIssuesSettings>();

        /// <summary>
        /// Adds a path to a MSBuild log file created by XmlFileLogger.
        /// </summary>
        /// <param name="logfilePath">Path to the MSBuild log file.</param>
        public void AddMsBuildXmlFileLoggerLogFile(FilePath logfilePath)
        {
            logfilePath.NotNull(nameof(logfilePath));

            this.AddMsBuildXmlFileLoggerLogFile(logfilePath, null);
        }

        /// <summary>
        /// Adds a path to a MSBuild log file created by XmlFileLogger.
        /// </summary>
        /// <param name="logfilePath">Path to the MSBuild log file.</param>
        /// <param name="settings">Settings for reading the log file. <c>Null</c> for default values.</param>
        public void AddMsBuildXmlFileLoggerLogFile(FilePath logfilePath, IReadIssuesSettings settings)
        {
            logfilePath.NotNull(nameof(logfilePath));

            this.MsBuildXmlFileLoggerLogFilePaths.Add(logfilePath, settings);
        }

        /// <summary>
        /// Adds a path to a MSBuild binary log file.
        /// </summary>
        /// <param name="logfilePath">Path to the MSBuild log file.</param>
        public void AddMsBuildBinaryLogFile(FilePath logfilePath)
        {
            logfilePath.NotNull(nameof(logfilePath));

            this.AddMsBuildBinaryLogFile(logfilePath, null);
        }

        /// <summary>
        /// Adds a path to a MSBuild binary log file.
        /// </summary>
        /// <param name="logfilePath">Path to the MSBuild log file.</param>
        /// <param name="settings">Settings for reading the log file. <c>Null</c> for default values.</param>
        public void AddMsBuildBinaryLogFile(FilePath logfilePath, IReadIssuesSettings settings)
        {
            logfilePath.NotNull(nameof(logfilePath));

            this.MsBuildBinaryLogFilePaths.Add(logfilePath, settings);
        }

        /// <summary>
        /// Adds a path to a InspectCode log file.
        /// </summary>
        /// <param name="logfilePath">Path to the InspectCode log file.</param>
        public void AddInspectCodeLogFile(FilePath logfilePath)
        {
            logfilePath.NotNull(nameof(logfilePath));

            this.AddInspectCodeLogFile(logfilePath, null);
        }

        /// <summary>
        /// Adds a path to a InspectCode log file.
        /// </summary>
        /// <param name="logfilePath">Path to the InspectCode log file.</param>
        /// <param name="settings">Settings for reading the log file. <c>Null</c> for default values.</param>
        public void AddInspectCodeLogFile(FilePath logfilePath, IReadIssuesSettings settings)
        {
            logfilePath.NotNull(nameof(logfilePath));

            this.InspectCodeLogFilePaths.Add(logfilePath, settings);
        }

        /// <summary>
        /// Adds a path to a dupFinder log file.
        /// </summary>
        /// <param name="logfilePath">Path to the dupFinder log file.</param>
        public void AddDupFinderLogFile(FilePath logfilePath)
        {
            logfilePath.NotNull(nameof(logfilePath));

            this.AddDupFinderLogFile(logfilePath, null);
        }

        /// <summary>
        /// Adds a path to a dupFinder log file.
        /// </summary>
        /// <param name="logfilePath">Path to the dupFinder log file.</param>
        /// <param name="settings">Settings for reading the log file. <c>Null</c> for default values.</param>
        public void AddDupFinderLogFile(FilePath logfilePath, IReadIssuesSettings settings)
        {
            logfilePath.NotNull(nameof(logfilePath));

            this.DupFinderLogFilePaths.Add(logfilePath, settings);
        }

        /// <summary>
        /// Adds a path to a markdownlint-cli log file.
        /// </summary>
        /// <param name="logfilePath">Path to the markdownlint-cli log file.</param>
        public void AddMarkdownlintCliLogFile(FilePath logfilePath)
        {
            logfilePath.NotNull(nameof(logfilePath));

            this.AddMarkdownlintCliLogFile(logfilePath, null);
        }

        /// <summary>
        /// Adds a path to a markdownlint-cli log file.
        /// </summary>
        /// <param name="logfilePath">Path to the markdownlint-cli log file.</param>
        /// <param name="settings">Settings for reading the log file. <c>Null</c> for default values.</param>
        public void AddMarkdownlintCliLogFile(FilePath logfilePath, IReadIssuesSettings settings)
        {
            logfilePath.NotNull(nameof(logfilePath));

            this.MarkdownlintCliLogFilePaths.Add(logfilePath, settings);
        }

        /// <summary>
        /// Adds a path to a markdownlint log file in version 1.
        /// </summary>
        /// <param name="logfilePath">Path to the markdownlint log file.</param>
        public void AddMarkdownlintV1LogFile(FilePath logfilePath)
        {
            logfilePath.NotNull(nameof(logfilePath));

            this.AddMarkdownlintV1LogFile(logfilePath, null);
        }

        /// <summary>
        /// Adds a path to a markdownlint log file in version 1.
        /// </summary>
        /// <param name="logfilePath">Path to the markdownlint log file.</param>
        /// <param name="settings">Settings for reading the log file. <c>Null</c> for default values.</param>
        public void AddMarkdownlintV1LogFile(FilePath logfilePath, IReadIssuesSettings settings)
        {
            logfilePath.NotNull(nameof(logfilePath));

            this.MarkdownlintV1LogFilePaths.Add(logfilePath, settings);
        }

        /// <summary>
        /// Adds a path to a ESLint log file in JSON format.
        /// </summary>
        /// <param name="logfilePath">Path to the ESLint log file.</param>
        public void AddEsLintJsonLogFile(FilePath logfilePath)
        {
            logfilePath.NotNull(nameof(logfilePath));

            this.AddEsLintJsonLogFile(logfilePath, null);
        }

        /// <summary>
        /// Adds a path to a ESLint log file in JSON format.
        /// </summary>
        /// <param name="logfilePath">Path to the ESLint log file.</param>
        /// <param name="settings">Settings for reading the log file. <c>Null</c> for default values.</param>
        public void AddEsLintJsonLogFile(FilePath logfilePath, IReadIssuesSettings settings)
        {
            logfilePath.NotNull(nameof(logfilePath));

            this.EsLintJsonLogFilePaths.Add(logfilePath, settings);
        }
    }
}