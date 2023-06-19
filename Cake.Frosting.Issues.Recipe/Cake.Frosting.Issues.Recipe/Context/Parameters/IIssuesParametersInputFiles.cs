using Cake.Core.IO;
using Cake.Issues;
using System.Collections.Generic;

namespace Cake.Frosting.Issues.Recipe
{
    /// <summary>
    /// Parameters for passing input files.
    /// </summary>
    public interface IIssuesParametersInputFiles
    {
        /// <summary>
        /// Gets list of registered paths to MSBuild log files created by XmlFileLogger.
        /// </summary>
        IDictionary<FilePath, IReadIssuesSettings> MsBuildXmlFileLoggerLogFilePaths { get; } 

        /// <summary>
        /// Gets list of registered paths to MSBuild binary log files.
        /// </summary>
        IDictionary<FilePath, IReadIssuesSettings> MsBuildBinaryLogFilePaths { get; } 

        /// <summary>
        /// Gets list of registered paths to InspectCode log files.
        /// </summary>
        IDictionary<FilePath, IReadIssuesSettings> InspectCodeLogFilePaths { get; } 

        /// <summary>
        /// Gets list of registered paths to markdownlint-cli log files.
        /// </summary>
        IDictionary<FilePath, IReadIssuesSettings> MarkdownlintCliLogFilePaths { get; }

        /// <summary>
        /// Gets list of registered paths to markdownlint-cli log files created with <c>--json</c>.
        /// </summary>
        IDictionary<FilePath, IReadIssuesSettings> MarkdownlintCliJsonLogFilePaths { get; }

        /// <summary>
        /// Gets list of registered paths to markdownlint log files in version 1.
        /// </summary>
        IDictionary<FilePath, IReadIssuesSettings> MarkdownlintV1LogFilePaths { get; } 

        /// <summary>
        /// Gets list of registered paths to ESLint log files in JSON format.
        /// </summary>
        IDictionary<FilePath, IReadIssuesSettings> EsLintJsonLogFilePaths { get; }

        /// <summary>
        /// Adds a path to a MSBuild log file created by XmlFileLogger.
        /// </summary>
        /// <param name="logfilePath">Path to the MSBuild log file.</param>
        void AddMsBuildXmlFileLoggerLogFile(FilePath logfilePath);

        /// <summary>
        /// Adds a path to a MSBuild log file created by XmlFileLogger.
        /// </summary>
        /// <param name="logfilePath">Path to the MSBuild log file.</param>
        /// <param name="settings">Settings for reading the log file. <c>Null</c> for default values.</param>
        void AddMsBuildXmlFileLoggerLogFile(FilePath logfilePath, IReadIssuesSettings settings);

        /// <summary>
        /// Adds a path to a MSBuild binary log file.
        /// </summary>
        /// <param name="logfilePath">Path to the MSBuild log file.</param>
        void AddMsBuildBinaryLogFile(FilePath logfilePath);

        /// <summary>
        /// Adds a path to a MSBuild binary log file.
        /// </summary>
        /// <param name="logfilePath">Path to the MSBuild log file.</param>
        /// <param name="settings">Settings for reading the log file. <c>Null</c> for default values.</param>
        void AddMsBuildBinaryLogFile(FilePath logfilePath, IReadIssuesSettings settings);

        /// <summary>
        /// Adds a path to a InspectCode log file.
        /// </summary>
        /// <param name="logfilePath">Path to the InspectCode log file.</param>
        void AddInspectCodeLogFile(FilePath logfilePath);

        /// <summary>
        /// Adds a path to a InspectCode log file.
        /// </summary>
        /// <param name="logfilePath">Path to the InspectCode log file.</param>
        /// <param name="settings">Settings for reading the log file. <c>Null</c> for default values.</param>
        void AddInspectCodeLogFile(FilePath logfilePath, IReadIssuesSettings settings);

        /// <summary>
        /// Adds a path to a markdownlint-cli log file.
        /// </summary>
        /// <param name="logfilePath">Path to the markdownlint-cli log file.</param>
        void AddMarkdownlintCliLogFile(FilePath logfilePath);

        /// <summary>
        /// Adds a path to a markdownlint-cli log file.
        /// </summary>
        /// <param name="logfilePath">Path to the markdownlint-cli log file.</param>
        /// <param name="settings">Settings for reading the log file. <c>Null</c> for default values.</param>
        void AddMarkdownlintCliLogFile(FilePath logfilePath, IReadIssuesSettings settings);

        /// <summary>
        /// Adds a path to a markdownlint-cli log file created with <c>--json</c>.
        /// </summary>
        /// <param name="logfilePath">Path to the markdownlint-cli log file.</param>
        void AddMarkdownlintCliJsonLogFile(FilePath logfilePath);

        /// <summary>
        /// Adds a path to a markdownlint-cli log file created with <c>--json</c>.
        /// </summary>
        /// <param name="logfilePath">Path to the markdownlint-cli log file.</param>
        /// <param name="settings">Settings for reading the log file. <c>Null</c> for default values.</param>
        void AddMarkdownlintCliJsonLogFile(FilePath logfilePath, IReadIssuesSettings settings);
        
        /// <summary>
        /// Adds a path to a markdownlint log file in version 1.
        /// </summary>
        /// <param name="logfilePath">Path to the markdownlint log file.</param>
        void AddMarkdownlintV1LogFile(FilePath logfilePath);

        /// <summary>
        /// Adds a path to a markdownlint log file in version 1.
        /// </summary>
        /// <param name="logfilePath">Path to the markdownlint log file.</param>
        /// <param name="settings">Settings for reading the log file. <c>Null</c> for default values.</param>
        void AddMarkdownlintV1LogFile(FilePath logfilePath, IReadIssuesSettings settings);

        /// <summary>
        /// Adds a path to a ESLint log file in JSON format.
        /// </summary>
        /// <param name="logfilePath">Path to the ESLint log file.</param>
        void AddEsLintJsonLogFile(FilePath logfilePath);

        /// <summary>
        /// Adds a path to a ESLint log file in JSON format.
        /// </summary>
        /// <param name="logfilePath">Path to the ESLint log file.</param>
        /// <param name="settings">Settings for reading the log file. <c>Null</c> for default values.</param>
        void AddEsLintJsonLogFile(FilePath logfilePath, IReadIssuesSettings settings);
    }
}