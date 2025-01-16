namespace Cake.Frosting.Issues.Recipe;

using Cake.Core.IO;

/// <summary>
/// Extensions for <see cref="IIssuesParametersInputFiles"/>.
/// </summary>
public static class IIssuesParametersInputFilesExtensions
{
    #region MsBuildXmlFileLoggerLogFile

    /// <summary>
    /// Adds a path to a MSBuild log file created by XmlFileLogger.
    /// </summary>
    /// <param name="parameters">Parameter instance.</param>
    /// <param name="logfilePath">Path to the MSBuild log file.</param>
    public static void AddMsBuildXmlFileLoggerLogFilePath(
        this IIssuesParametersInputFiles parameters,
        FilePath logfilePath)
    {
        parameters.NotNull();
        logfilePath.NotNull();

        parameters.AddMsBuildXmlFileLoggerLogFilePath(logfilePath, null);
    }

    /// <summary>
    /// Adds content of a MSBuild log file created by XmlFileLogger.
    /// </summary>
    /// <param name="parameters">Parameter instance.</param>
    /// <param name="logfileContent">Content of the MSBuild log file.</param>
    public static void AddMsBuildXmlFileLoggerLogFileContent(
        this IIssuesParametersInputFiles parameters,
        string logfileContent)
    {
        parameters.NotNull();
        logfileContent.NotNullOrWhiteSpace();

        parameters.AddMsBuildXmlFileLoggerLogFileContent(logfileContent.ToByteArray(), null);
    }

    /// <summary>
    /// Adds content of a MSBuild log file created by XmlFileLogger.
    /// </summary>
    /// <param name="parameters">Parameter instance.</param>
    /// <param name="logfileContent">Content of the MSBuild log file.</param>
    public static void AddMsBuildXmlFileLoggerLogFileContent(
        this IIssuesParametersInputFiles parameters,
        byte[] logfileContent)
    {
        parameters.NotNull();
        logfileContent.NotNullOrEmpty();

        parameters.AddMsBuildXmlFileLoggerLogFileContent(logfileContent, null);
    }

    /// <summary>
    /// Adds content of a MSBuild log file created by XmlFileLogger.
    /// </summary>
    /// <param name="parameters">Parameter instance.</param>
    /// <param name="logfileContent">Content of the MSBuild log file.</param>
    /// <param name="settings">Settings for reading the log file. <c>Null</c> for default values.</param>
    public static void AddMsBuildXmlFileLoggerLogFileContent(
        this IIssuesParametersInputFiles parameters,
        string logfileContent,
        IReadIssuesSettings settings)
    {
        parameters.NotNull();
        logfileContent.NotNullOrEmpty();

        parameters.AddMsBuildXmlFileLoggerLogFileContent(logfileContent.ToByteArray(), settings);
    }

    #endregion

    #region MsBuildBinaryLogFile

    /// <summary>
    /// Adds a path to a MSBuild binary log file.
    /// </summary>
    /// <param name="parameters">Parameter instance.</param>
    /// <param name="logfilePath">Path to the MSBuild log file.</param>
    public static void AddMsBuildBinaryLogFilePath(
        this IIssuesParametersInputFiles parameters,
        FilePath logfilePath)
    {
        parameters.NotNull();
        logfilePath.NotNull();

        parameters.AddMsBuildBinaryLogFilePath(logfilePath, null);
    }

    /// <summary>
    /// Adds content of a MSBuild binary log file.
    /// </summary>
    /// <param name="parameters">Parameter instance.</param>
    /// <param name="logfileContent">Content of the MSBuild log file.</param>
    public static void AddMsBuildBinaryLogFileContent(
        this IIssuesParametersInputFiles parameters,
        string logfileContent)
    {
        parameters.NotNull();
        logfileContent.NotNullOrWhiteSpace();

        parameters.AddMsBuildBinaryLogFileContent(logfileContent.ToByteArray(), null);
    }

    /// <summary>
    /// Adds content of a MSBuild binary log file.
    /// </summary>
    /// <param name="parameters">Parameter instance.</param>
    /// <param name="logfileContent">Content of the MSBuild log file.</param>
    public static void AddMsBuildBinaryLogFileContent(
        this IIssuesParametersInputFiles parameters,
        byte[] logfileContent)
    {
        parameters.NotNull();
        logfileContent.NotNullOrEmpty();

        parameters.AddMsBuildBinaryLogFileContent(logfileContent, null);
    }

    /// <summary>
    /// Adds content of a MSBuild binary log file.
    /// </summary>
    /// <param name="parameters">Parameter instance.</param>
    /// <param name="logfileContent">Content of the MSBuild log file.</param>
    /// <param name="settings">Settings for reading the log file. <c>Null</c> for default values.</param>
    public static void AddMsBuildBinaryLogFileContent(
        this IIssuesParametersInputFiles parameters,
        string logfileContent,
        IReadIssuesSettings settings)
    {
        parameters.NotNull();
        logfileContent.NotNullOrEmpty();

        parameters.AddMsBuildBinaryLogFileContent(logfileContent.ToByteArray(), settings);
    }

    #endregion

    #region InspectCodeLogFile

    /// <summary>
    /// Adds a path to an InspectCode log file.
    /// </summary>
    /// <param name="parameters">Parameter instance.</param>
    /// <param name="logfilePath">Path to the InspectCode log file.</param>
    public static void AddInspectCodeLogFilePath(
        this IIssuesParametersInputFiles parameters,
        FilePath logfilePath)
    {
        parameters.NotNull();
        logfilePath.NotNull();

        parameters.AddInspectCodeLogFilePath(logfilePath, null);
    }

    /// <summary>
    /// Adds content of an InspectCode log file.
    /// </summary>
    /// <param name="parameters">Parameter instance.</param>
    /// <param name="logfileContent">Content of the InspectCode log file.</param>
    public static void AddInspectCodeLogFileContent(
        this IIssuesParametersInputFiles parameters,
        string logfileContent)
    {
        parameters.NotNull();
        logfileContent.NotNullOrWhiteSpace();

        parameters.AddInspectCodeLogFileContent(logfileContent.ToByteArray(), null);
    }

    /// <summary>
    /// Adds content of an InspectCode log file.
    /// </summary>
    /// <param name="parameters">Parameter instance.</param>
    /// <param name="logfileContent">Content of the InspectCode log file.</param>
    public static void AddInspectCodeLogFileContent(
        this IIssuesParametersInputFiles parameters,
        byte[] logfileContent)
    {
        parameters.NotNull();
        logfileContent.NotNullOrEmpty();

        parameters.AddInspectCodeLogFileContent(logfileContent, null);
    }

    /// <summary>
    /// Adds content of an InspectCode log file.
    /// </summary>
    /// <param name="parameters">Parameter instance.</param>
    /// <param name="logfileContent">Content of the InspectCode log file.</param>
    /// <param name="settings">Settings for reading the log file. <c>Null</c> for default values.</param>
    public static void AddInspectCodeLogFileContent(
        this IIssuesParametersInputFiles parameters,
        string logfileContent,
        IReadIssuesSettings settings)
    {
        parameters.NotNull();
        logfileContent.NotNullOrEmpty();

        parameters.AddInspectCodeLogFileContent(logfileContent.ToByteArray(), settings);
    }

    #endregion

    #region MarkdownlintCliLogFile

    /// <summary>
    /// Adds a path to a markdownlint-cli log file.
    /// </summary>
    /// <param name="parameters">Parameter instance.</param>
    /// <param name="logfilePath">Path to the markdownlint-cli log file.</param>
    public static void AddMarkdownlintCliLogFilePath(
        this IIssuesParametersInputFiles parameters,
        FilePath logfilePath)
    {
        parameters.NotNull();
        logfilePath.NotNull();

        parameters.AddMarkdownlintCliLogFilePath(logfilePath, null);
    }

    /// <summary>
    /// Adds content of a markdownlint-cli log file.
    /// </summary>
    /// <param name="parameters">Parameter instance.</param>
    /// <param name="logfileContent">Content of the markdownlint-cli log file.</param>
    public static void AddMarkdownlintCliLogFileContent(
        this IIssuesParametersInputFiles parameters,
        string logfileContent)
    {
        parameters.NotNull();
        logfileContent.NotNullOrWhiteSpace();

        parameters.AddMarkdownlintCliLogFileContent(logfileContent.ToByteArray(), null);
    }

    /// <summary>
    /// Adds content of a markdownlint-cli log file.
    /// </summary>
    /// <param name="parameters">Parameter instance.</param>
    /// <param name="logfileContent">Content of the markdownlint-cli log file.</param>
    public static void AddMarkdownlintCliLogFileContent(
        this IIssuesParametersInputFiles parameters,
        byte[] logfileContent)
    {
        parameters.NotNull();
        logfileContent.NotNullOrEmpty();

        parameters.AddMarkdownlintCliLogFileContent(logfileContent, null);
    }

    /// <summary>
    /// Adds content of a markdownlint-cli log file.
    /// </summary>
    /// <param name="parameters">Parameter instance.</param>
    /// <param name="logfileContent">Content of the markdownlint-cli log file.</param>
    /// <param name="settings">Settings for reading the log file. <c>Null</c> for default values.</param>
    public static void AddMarkdownlintCliLogFileContent(
        this IIssuesParametersInputFiles parameters,
        string logfileContent,
        IReadIssuesSettings settings)
    {
        parameters.NotNull();
        logfileContent.NotNullOrEmpty();

        parameters.AddMarkdownlintCliLogFileContent(logfileContent.ToByteArray(), settings);
    }

    #endregion

    #region MarkdownlintCliJsonLogFile

    /// <summary>
    /// Adds a path to a markdownlint-cli log file created with <c>--json</c>.
    /// </summary>
    /// <param name="parameters">Parameter instance.</param>
    /// <param name="logfilePath">Path to the markdownlint-cli log file.</param>
    public static void AddMarkdownlintCliJsonLogFilePath(
        this IIssuesParametersInputFiles parameters,
        FilePath logfilePath)
    {
        parameters.NotNull();
        logfilePath.NotNull();

        parameters.AddMarkdownlintCliJsonLogFilePath(logfilePath, null);
    }

    /// <summary>
    /// Adds content of a markdownlint-cli log file created with <c>--json</c>.
    /// </summary>
    /// <param name="parameters">Parameter instance.</param>
    /// <param name="logfileContent">Content of the markdownlint-cli log file.</param>
    public static void AddMarkdownlintCliJsonLogFileContent(
        this IIssuesParametersInputFiles parameters,
        string logfileContent)
    {
        parameters.NotNull();
        logfileContent.NotNullOrWhiteSpace();

        parameters.AddMarkdownlintCliJsonLogFileContent(logfileContent.ToByteArray(), null);
    }

    /// <summary>
    /// Adds content of a markdownlint-cli log file created with <c>--json</c>.
    /// </summary>
    /// <param name="parameters">Parameter instance.</param>
    /// <param name="logfileContent">Content of the markdownlint-cli log file.</param>
    public static void AddMarkdownlintCliJsonLogFileContent(
        this IIssuesParametersInputFiles parameters,
        byte[] logfileContent)
    {
        parameters.NotNull();
        logfileContent.NotNullOrEmpty();

        parameters.AddMarkdownlintCliJsonLogFileContent(logfileContent, null);
    }

    /// <summary>
    /// Adds content of a markdownlint-cli log file created with <c>--json</c>.
    /// </summary>
    /// <param name="parameters">Parameter instance.</param>
    /// <param name="logfileContent">Content of the markdownlint-cli log file.</param>
    /// <param name="settings">Settings for reading the log file. <c>Null</c> for default values.</param>
    public static void AddMarkdownlintCliJsonLogFileContent(
        this IIssuesParametersInputFiles parameters,
        string logfileContent,
        IReadIssuesSettings settings)
    {
        parameters.NotNull();
        logfileContent.NotNullOrEmpty();

        parameters.AddMarkdownlintCliJsonLogFileContent(logfileContent.ToByteArray(), settings);
    }

    #endregion

    #region MarkdownlintV1LogFile

    /// <summary>
    /// Adds a path to a markdownlint log file in version 1.
    /// </summary>
    /// <param name="parameters">Parameter instance.</param>
    /// <param name="logfilePath">Path to the markdownlint log file.</param>
    public static void AddMarkdownlintV1LogFilePath(
        this IIssuesParametersInputFiles parameters,
        FilePath logfilePath)
    {
        parameters.NotNull();
        logfilePath.NotNull();

        parameters.AddMarkdownlintV1LogFilePath(logfilePath, null);
    }

    /// <summary>
    /// Adds content of a markdownlint log file in version 1.
    /// </summary>
    /// <param name="parameters">Parameter instance.</param>
    /// <param name="logfileContent">Content of the markdownlint log file.</param>
    public static void AddMarkdownlintV1LogFileContent(
        this IIssuesParametersInputFiles parameters,
        string logfileContent)
    {
        parameters.NotNull();
        logfileContent.NotNullOrWhiteSpace();

        parameters.AddMarkdownlintV1LogFileContent(logfileContent.ToByteArray(), null);
    }

    /// <summary>
    /// Adds content of a markdownlint log file in version 1.
    /// </summary>
    /// <param name="parameters">Parameter instance.</param>
    /// <param name="logfileContent">Content of the markdownlint log file.</param>
    public static void AddMarkdownlintV1LogFileContent(
        this IIssuesParametersInputFiles parameters,
        byte[] logfileContent)
    {
        parameters.NotNull();
        logfileContent.NotNullOrEmpty();

        parameters.AddMarkdownlintV1LogFileContent(logfileContent, null);
    }

    /// <summary>
    /// Adds content of a markdownlint log file in version 1.
    /// </summary>
    /// <param name="parameters">Parameter instance.</param>
    /// <param name="logfileContent">Content of the markdownlint log file.</param>
    /// <param name="settings">Settings for reading the log file. <c>Null</c> for default values.</param>
    public static void AddMarkdownlintV1LogFileContent(
        this IIssuesParametersInputFiles parameters,
        string logfileContent,
        IReadIssuesSettings settings)
    {
        parameters.NotNull();
        logfileContent.NotNullOrEmpty();

        parameters.AddMarkdownlintV1LogFileContent(logfileContent.ToByteArray(), settings);
    }

    #endregion

    #region EsLintJsonLogFile

    /// <summary>
    /// Adds a path to a ESLint log file in JSON format.
    /// </summary>
    /// <param name="parameters">Parameter instance.</param>
    /// <param name="logfilePath">Path to the ESLint log file.</param>
    public static void AddEsLintJsonLogFilePath(
        this IIssuesParametersInputFiles parameters,
        FilePath logfilePath)
    {
        parameters.NotNull();
        logfilePath.NotNull();

        parameters.AddEsLintJsonLogFilePath(logfilePath, null);
    }

    /// <summary>
    /// Adds content of a ESLint log file in JSON format.
    /// </summary>
    /// <param name="parameters">Parameter instance.</param>
    /// <param name="logfileContent">Content of the ESLint log file.</param>
    public static void AddEsLintJsonLogFileContent(
        this IIssuesParametersInputFiles parameters,
        string logfileContent)
    {
        parameters.NotNull();
        logfileContent.NotNullOrWhiteSpace();

        parameters.AddEsLintJsonLogFileContent(logfileContent.ToByteArray(), null);
    }

    /// <summary>
    /// Adds content of a ESLint log file in JSON format.
    /// </summary>
    /// <param name="parameters">Parameter instance.</param>
    /// <param name="logfileContent">Content of the ESLint log file.</param>
    public static void AddEsLintJsonLogFileContent(
        this IIssuesParametersInputFiles parameters,
        byte[] logfileContent)
    {
        parameters.NotNull();
        logfileContent.NotNullOrEmpty();

        parameters.AddEsLintJsonLogFileContent(logfileContent, null);
    }

    /// <summary>
    /// Adds content of a ESLint log file in JSON format.
    /// </summary>
    /// <param name="parameters">Parameter instance.</param>
    /// <param name="logfileContent">Content of the ESLint log file.</param>
    /// <param name="settings">Settings for reading the log file. <c>Null</c> for default values.</param>
    public static void AddEsLintJsonLogFileContent(
        this IIssuesParametersInputFiles parameters,
        string logfileContent,
        IReadIssuesSettings settings)
    {
        parameters.NotNull();
        logfileContent.NotNullOrEmpty();

        parameters.AddEsLintJsonLogFileContent(logfileContent.ToByteArray(), settings);
    }

    #endregion

    #region SarifLogFile

    /// <summary>
    /// Adds a path to a log file in SARIF format.
    /// </summary>
    /// <param name="parameters">Parameter instance.</param>
    /// <param name="logfilePath">Path to the SARIF log file.</param>
    public static void AddSarifLogFilePath(
        this IIssuesParametersInputFiles parameters,
        FilePath logfilePath)
    {
        parameters.NotNull();
        logfilePath.NotNull();

        parameters.AddSarifLogFilePath(logfilePath, null);
    }

    /// <summary>
    /// Adds content of a log file in SARIF format.
    /// </summary>
    /// <param name="parameters">Parameter instance.</param>
    /// <param name="logfileContent">Content of the SARIF log file.</param>
    public static void AddSarifLogFileContent(
        this IIssuesParametersInputFiles parameters,
        string logfileContent)
    {
        parameters.NotNull();
        logfileContent.NotNullOrWhiteSpace();

        parameters.AddSarifLogFileContent(logfileContent.ToByteArray(), null);
    }

    /// <summary>
    /// Adds content of a log file in SARIF format.
    /// </summary>
    /// <param name="parameters">Parameter instance.</param>
    /// <param name="logfileContent">Content of the SARIF log file.</param>
    public static void AddSarifLogFileContent(
        this IIssuesParametersInputFiles parameters,
        byte[] logfileContent)
    {
        parameters.NotNull();
        logfileContent.NotNullOrEmpty();

        parameters.AddSarifLogFileContent(logfileContent, null);
    }

    /// <summary>
    /// Adds content of a log file in SARIF format.
    /// </summary>
    /// <param name="parameters">Parameter instance.</param>
    /// <param name="logfileContent">Content of the SARIF log file.</param>
    /// <param name="settings">Settings for reading the log file. <c>Null</c> for default values.</param>
    public static void AddSarifLogFileContent(
        this IIssuesParametersInputFiles parameters,
        string logfileContent,
        IReadIssuesSettings settings)
    {
        parameters.NotNull();
        logfileContent.NotNullOrEmpty();

        parameters.AddSarifLogFileContent(logfileContent.ToByteArray(), settings);
    }

    #endregion

    #region GenericTapLogFile

    /// <summary>
    /// Adds a path to a log file in Test Anything Protocol format.
    /// </summary>
    /// <param name="parameters">Parameter instance.</param>
    /// <param name="logfilePath">Path to the log file in Test Anything Protocol format.</param>
    public static void AddGenericTapLogFilePath(
        this IIssuesParametersInputFiles parameters,
        FilePath logfilePath)
    {
        parameters.NotNull();
        logfilePath.NotNull();

        parameters.AddGenericTapLogFilePath(logfilePath, null);
    }

    /// <summary>
    /// Adds content of a log file in Test Anything Protocol format.
    /// </summary>
    /// <param name="parameters">Parameter instance.</param>
    /// <param name="logfileContent">Content of the log file in Test Anything Protocol format.</param>
    public static void AddGenericTapLogFileContent(
        this IIssuesParametersInputFiles parameters,
        string logfileContent)
    {
        parameters.NotNull();
        logfileContent.NotNullOrWhiteSpace();

        parameters.AddGenericTapLogFileContent(logfileContent.ToByteArray(), null);
    }

    /// <summary>
    /// Adds content of a log file in Test Anything Protocol format.
    /// </summary>
    /// <param name="parameters">Parameter instance.</param>
    /// <param name="logfileContent">Content of the log file in Test Anything Protocol format.</param>
    public static void AddGenericTapLogFileContent(
        this IIssuesParametersInputFiles parameters,
        byte[] logfileContent)
    {
        parameters.NotNull();
        logfileContent.NotNullOrEmpty();

        parameters.AddGenericTapLogFileContent(logfileContent, null);
    }

    /// <summary>
    /// Adds content of a log file in Test Anything Protocol format.
    /// </summary>
    /// <param name="parameters">Parameter instance.</param>
    /// <param name="logfileContent">Content of the log file in Test Anything Protocol format.</param>
    /// <param name="settings">Settings for reading the log file. <c>Null</c> for default values.</param>
    public static void AddGenericTapLogFileContent(
        this IIssuesParametersInputFiles parameters,
        string logfileContent,
        IReadIssuesSettings settings)
    {
        parameters.NotNull();
        logfileContent.NotNullOrEmpty();

        parameters.AddGenericTapLogFileContent(logfileContent.ToByteArray(), settings);
    }

    #endregion

    #region StylelintTapLogFile

    /// <summary>
    /// Adds a path to a Stylelint log file in Test Anything Protocol format.
    /// </summary>
    /// <param name="parameters">Parameter instance.</param>
    /// <param name="logfilePath">Path to the Stylelint log file in Test Anything Protocol format.</param>
    public static void AddStylelintTapLogFilePath(
        this IIssuesParametersInputFiles parameters,
        FilePath logfilePath)
    {
        parameters.NotNull();
        logfilePath.NotNull();

        parameters.AddStylelintTapLogFilePath(logfilePath, null);
    }

    /// <summary>
    /// Adds content of a Stylelint log file in Test Anything Protocol format.
    /// </summary>
    /// <param name="parameters">Parameter instance.</param>
    /// <param name="logfileContent">Content of the Stylelint log file in Test Anything Protocol format.</param>
    public static void AddStylelintTapLogFileContent(
        this IIssuesParametersInputFiles parameters,
        string logfileContent)
    {
        parameters.NotNull();
        logfileContent.NotNullOrWhiteSpace();

        parameters.AddStylelintTapLogFileContent(logfileContent.ToByteArray(), null);
    }

    /// <summary>
    /// Adds content of a Stylelint log file in Test Anything Protocol format.
    /// </summary>
    /// <param name="parameters">Parameter instance.</param>
    /// <param name="logfileContent">Content of the Stylelint log file in Test Anything Protocol format.</param>
    public static void AddStylelintTapLogFileContent(
        this IIssuesParametersInputFiles parameters,
        byte[] logfileContent)
    {
        parameters.NotNull();
        logfileContent.NotNullOrEmpty();

        parameters.AddStylelintTapLogFileContent(logfileContent, null);
    }

    /// <summary>
    /// Adds content of a Stylelint log file in Test Anything Protocol format.
    /// </summary>
    /// <param name="parameters">Parameter instance.</param>
    /// <param name="logfileContent">Content of the Stylelint log file in Test Anything Protocol format.</param>
    /// <param name="settings">Settings for reading the log file. <c>Null</c> for default values.</param>
    public static void AddStylelintTapLogFileContent(
        this IIssuesParametersInputFiles parameters,
        string logfileContent,
        IReadIssuesSettings settings)
    {
        parameters.NotNull();
        logfileContent.NotNullOrEmpty();

        parameters.AddStylelintTapLogFileContent(logfileContent.ToByteArray(), settings);
    }

    #endregion

    #region TextlintTapLogFile

    /// <summary>
    /// Adds a path to a Textlint log file in Test Anything Protocol format.
    /// </summary>
    /// <param name="parameters">Parameter instance.</param>
    /// <param name="logfilePath">Path to the Textlint log file in Test Anything Protocol format.</param>
    public static void AddTextlintTapLogFilePath(
        this IIssuesParametersInputFiles parameters,
        FilePath logfilePath)
    {
        parameters.NotNull();
        logfilePath.NotNull();

        parameters.AddTextlintTapLogFilePath(logfilePath, null);
    }

    /// <summary>
    /// Adds content of a Textlint log file in Test Anything Protocol format.
    /// </summary>
    /// <param name="parameters">Parameter instance.</param>
    /// <param name="logfileContent">Content of the Textlint log file in Test Anything Protocol format.</param>
    public static void AddTextlintTapLogFileContent(
        this IIssuesParametersInputFiles parameters,
        string logfileContent)
    {
        parameters.NotNull();
        logfileContent.NotNullOrWhiteSpace();

        parameters.AddTextlintTapLogFileContent(logfileContent.ToByteArray(), null);
    }

    /// <summary>
    /// Adds content of a Textlint log file in Test Anything Protocol format.
    /// </summary>
    /// <param name="parameters">Parameter instance.</param>
    /// <param name="logfileContent">Content of the Textlint log file in Test Anything Protocol format.</param>
    public static void AddTextlintTapLogFileContent(
        this IIssuesParametersInputFiles parameters,
        byte[] logfileContent)
    {
        parameters.NotNull();
        logfileContent.NotNullOrEmpty();

        parameters.AddTextlintTapLogFileContent(logfileContent, null);
    }

    /// <summary>
    /// Adds content of a Textlint log file in Test Anything Protocol format.
    /// </summary>
    /// <param name="parameters">Parameter instance.</param>
    /// <param name="logfileContent">Content of the Textlint log file in Test Anything Protocol format.</param>
    /// <param name="settings">Settings for reading the log file. <c>Null</c> for default values.</param>
    public static void AddTextlintTapLogFileContent(
        this IIssuesParametersInputFiles parameters,
        string logfileContent,
        IReadIssuesSettings settings)
    {
        parameters.NotNull();
        logfileContent.NotNullOrEmpty();

        parameters.AddTextlintTapLogFileContent(logfileContent.ToByteArray(), settings);
    }

    #endregion
}
