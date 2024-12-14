namespace Cake.Frosting.Issues.Recipe;

using Cake.Core.IO;

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
    /// Gets list of registered content of MSBuild log files created by XmlFileLogger.
    /// </summary>
    IDictionary<byte[], IReadIssuesSettings> MsBuildXmlFileLoggerLogFileContent { get; }

    /// <summary>
    /// Gets list of registered paths to MSBuild binary log files.
    /// </summary>
    IDictionary<FilePath, IReadIssuesSettings> MsBuildBinaryLogFilePaths { get; }

    /// <summary>
    /// Gets list of registered content of MSBuild binary log files.
    /// </summary>
    IDictionary<byte[], IReadIssuesSettings> MsBuildBinaryLogFileContent { get; }

    /// <summary>
    /// Gets list of registered paths to InspectCode log files.
    /// </summary>
    IDictionary<FilePath, IReadIssuesSettings> InspectCodeLogFilePaths { get; }

    /// <summary>
    /// Gets list of registered content of InspectCode log files.
    /// </summary>
    IDictionary<byte[], IReadIssuesSettings> InspectCodeLogFileContent { get; }

    /// <summary>
    /// Gets list of registered paths to markdownlint-cli log files.
    /// </summary>
    IDictionary<FilePath, IReadIssuesSettings> MarkdownlintCliLogFilePaths { get; }

    /// <summary>
    /// Gets list of registered content of markdownlint-cli log files.
    /// </summary>
    IDictionary<byte[], IReadIssuesSettings> MarkdownlintCliLogFileContent { get; }

    /// <summary>
    /// Gets list of registered paths to markdownlint-cli log files created with <c>--json</c>.
    /// </summary>
    IDictionary<FilePath, IReadIssuesSettings> MarkdownlintCliJsonLogFilePaths { get; }

    /// <summary>
    /// Gets list of registered content of markdownlint-cli log files created with <c>--json</c>.
    /// </summary>
    IDictionary<byte[], IReadIssuesSettings> MarkdownlintCliJsonLogFileContent { get; }

    /// <summary>
    /// Gets list of registered paths to markdownlint log files in version 1.
    /// </summary>
    IDictionary<FilePath, IReadIssuesSettings> MarkdownlintV1LogFilePaths { get; }

    /// <summary>
    /// Gets list of registered content of markdownlint log files in version 1.
    /// </summary>
    IDictionary<byte[], IReadIssuesSettings> MarkdownlintV1LogFileContent { get; }

    /// <summary>
    /// Gets list of registered paths to ESLint log files in JSON format.
    /// </summary>
    IDictionary<FilePath, IReadIssuesSettings> EsLintJsonLogFilePaths { get; }

    /// <summary>
    /// Gets list of registered content of ESLint log files in JSON format.
    /// </summary>
    IDictionary<byte[], IReadIssuesSettings> EsLintJsonLogFileContent { get; }

    /// <summary>
    /// Gets list of registered paths to SARIF log files.
    /// </summary>
    IDictionary<FilePath, IReadIssuesSettings> SarifLogFilePaths { get; }

    /// <summary>
    /// Gets list of registered content of SARIF log files.
    /// </summary>
    IDictionary<byte[], IReadIssuesSettings> SarifLogFileContent { get; }

    /// <summary>
    /// Adds a path to a MSBuild log file created by XmlFileLogger.
    /// </summary>
    /// <param name="logfilePath">Path to the MSBuild log file.</param>
    /// <param name="settings">Settings for reading the log file. <c>Null</c> for default values.</param>
    void AddMsBuildXmlFileLoggerLogFilePath(FilePath logfilePath, IReadIssuesSettings settings);

    /// <summary>
    /// Adds content of a MSBuild log file created by XmlFileLogger.
    /// </summary>
    /// <param name="logfileContent">Content of the MSBuild log file.</param>
    /// <param name="settings">Settings for reading the log file. <c>Null</c> for default values.</param>
    void AddMsBuildXmlFileLoggerLogFileContent(byte[] logfileContent, IReadIssuesSettings settings);

    /// <summary>
    /// Adds a path to a MSBuild binary log file.
    /// </summary>
    /// <param name="logfilePath">Path to the MSBuild log file.</param>
    /// <param name="settings">Settings for reading the log file. <c>Null</c> for default values.</param>
    void AddMsBuildBinaryLogFilePath(FilePath logfilePath, IReadIssuesSettings settings);

    /// <summary>
    /// Adds content of a MSBuild binary log file.
    /// </summary>
    /// <param name="logfileContent">Content of the MSBuild log file.</param>
    /// <param name="settings">Settings for reading the log file. <c>Null</c> for default values.</param>
    void AddMsBuildBinaryLogFileContent(byte[] logfileContent, IReadIssuesSettings settings);

    /// <summary>
    /// Adds a path to a InspectCode log file.
    /// </summary>
    /// <param name="logfilePath">Path to the InspectCode log file.</param>
    /// <param name="settings">Settings for reading the log file. <c>Null</c> for default values.</param>
    void AddInspectCodeLogFilePath(FilePath logfilePath, IReadIssuesSettings settings);

    /// <summary>
    /// Adds content of a InspectCode log file.
    /// </summary>
    /// <param name="logfileContent">Content of the InspectCode log file.</param>
    /// <param name="settings">Settings for reading the log file. <c>Null</c> for default values.</param>
    void AddInspectCodeLogFileContent(byte[] logfileContent, IReadIssuesSettings settings);

    /// <summary>
    /// Adds a path to a markdownlint-cli log file.
    /// </summary>
    /// <param name="logfilePath">Path to the markdownlint-cli log file.</param>
    /// <param name="settings">Settings for reading the log file. <c>Null</c> for default values.</param>
    void AddMarkdownlintCliLogFilePath(FilePath logfilePath, IReadIssuesSettings settings);

    /// <summary>
    /// Adds content of a markdownlint-cli log file.
    /// </summary>
    /// <param name="logfileContent">Content of the markdownlint-cli log file.</param>
    /// <param name="settings">Settings for reading the log file. <c>Null</c> for default values.</param>
    void AddMarkdownlintCliLogFileContent(byte[] logfileContent, IReadIssuesSettings settings);

    /// <summary>
    /// Adds a path to a markdownlint-cli log file created with <c>--json</c>.
    /// </summary>
    /// <param name="logfilePath">Path to the markdownlint-cli log file.</param>
    /// <param name="settings">Settings for reading the log file. <c>Null</c> for default values.</param>
    void AddMarkdownlintCliJsonLogFilePath(FilePath logfilePath, IReadIssuesSettings settings);

    /// <summary>
    /// Adds content of a markdownlint-cli log file created with <c>--json</c>.
    /// </summary>
    /// <param name="logfileContent">Content of the markdownlint-cli log file.</param>
    /// <param name="settings">Settings for reading the log file. <c>Null</c> for default values.</param>
    void AddMarkdownlintCliJsonLogFileContent(byte[] logfileContent, IReadIssuesSettings settings);

    /// <summary>
    /// Adds a path to a markdownlint log file in version 1.
    /// </summary>
    /// <param name="logfilePath">Path to the markdownlint log file.</param>
    /// <param name="settings">Settings for reading the log file. <c>Null</c> for default values.</param>
    void AddMarkdownlintV1LogFilePath(FilePath logfilePath, IReadIssuesSettings settings);

    /// <summary>
    /// Adds content of a markdownlint log file in version 1.
    /// </summary>
    /// <param name="logfileContent">Content of the markdownlint log file.</param>
    /// <param name="settings">Settings for reading the log file. <c>Null</c> for default values.</param>
    void AddMarkdownlintV1LogFileContent(byte[] logfileContent, IReadIssuesSettings settings);

    /// <summary>
    /// Adds a path to a ESLint log file in JSON format.
    /// </summary>
    /// <param name="logfilePath">Path to the ESLint log file.</param>
    /// <param name="settings">Settings for reading the log file. <c>Null</c> for default values.</param>
    void AddEsLintJsonLogFilePath(FilePath logfilePath, IReadIssuesSettings settings);

    /// <summary>
    /// Adds content of a ESLint log file in JSON format.
    /// </summary>
    /// <param name="logfileContent">Content of the ESLint log file.</param>
    /// <param name="settings">Settings for reading the log file. <c>Null</c> for default values.</param>
    void AddEsLintJsonLogFileContent(byte[] logfileContent, IReadIssuesSettings settings);

    /// <summary>
    /// Adds a path to a log file in SARIF format.
    /// </summary>
    /// <param name="logfilePath">Path to the SARIF log file.</param>
    /// <param name="settings">Settings for reading the log file. <c>Null</c> for default values.</param>
    void AddSarifLogFilePath(FilePath logfilePath, IReadIssuesSettings settings);

    /// <summary>
    /// Adds content of a log file in SARIF format.
    /// </summary>
    /// <param name="logfileContent">Content of the SARIF log file.</param>
    /// <param name="settings">Settings for reading the log file. <c>Null</c> for default values.</param>
    void AddSarifLogFileContent(byte[] logfileContent, IReadIssuesSettings settings);
}