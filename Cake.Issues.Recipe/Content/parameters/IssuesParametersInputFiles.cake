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
    /// Gets list of registered content of MSBuild log files created by XmlFileLogger.
    /// </summary>
    public IDictionary<byte[], IReadIssuesSettings> MsBuildXmlFileLoggerLogFileContent { get; } = new Dictionary<byte[], IReadIssuesSettings>();

    /// <summary>
    /// Gets list of registered paths to MSBuild binary log files.
    /// </summary>
    public IDictionary<FilePath, IReadIssuesSettings> MsBuildBinaryLogFilePaths { get; } = new Dictionary<FilePath, IReadIssuesSettings>();

    /// <summary>
    /// Gets list of registered content of MSBuild binary log files.
    /// </summary>
    public IDictionary<byte[], IReadIssuesSettings> MsBuildBinaryLogFileContent { get; } = new Dictionary<byte[], IReadIssuesSettings>();

    /// <summary>
    /// Gets list of registered paths to InspectCode log files.
    /// </summary>
    public IDictionary<FilePath, IReadIssuesSettings> InspectCodeLogFilePaths { get; } = new Dictionary<FilePath, IReadIssuesSettings>();

    /// <summary>
    /// Gets list of registered content of InspectCode log files.
    /// </summary>
    public IDictionary<byte[], IReadIssuesSettings> InspectCodeLogFileContent { get; } = new Dictionary<byte[], IReadIssuesSettings>();

    /// <summary>
    /// Gets list of registered paths to markdownlint-cli log files.
    /// </summary>
    public IDictionary<FilePath, IReadIssuesSettings> MarkdownlintCliLogFilePaths { get; } = new Dictionary<FilePath, IReadIssuesSettings>();

    /// <summary>
    /// Gets list of registered content of markdownlint-cli log files.
    /// </summary>
    public IDictionary<byte[], IReadIssuesSettings> MarkdownlintCliLogFileContent { get; } = new Dictionary<byte[], IReadIssuesSettings>();

    /// <summary>
    /// Gets list of registered paths to markdownlint-cli log files created with <c>--json</c>.
    /// </summary>
    public IDictionary<FilePath, IReadIssuesSettings> MarkdownlintCliJsonLogFilePaths { get; } = new Dictionary<FilePath, IReadIssuesSettings>();

    /// <summary>
    /// Gets list of registered content of markdownlint-cli log files created with <c>--json</c>.
    /// </summary>
    public IDictionary<byte[], IReadIssuesSettings> MarkdownlintCliJsonLogFileContent { get; } = new Dictionary<byte[], IReadIssuesSettings>();

    /// <summary>
    /// Gets list of registered paths to markdownlint log files in version 1.
    /// </summary>
    public IDictionary<FilePath, IReadIssuesSettings> MarkdownlintV1LogFilePaths { get; } = new Dictionary<FilePath, IReadIssuesSettings>();

    /// <summary>
    /// Gets list of registered paths to markdownlint log files in version 1.
    /// </summary>
    public IDictionary<byte[], IReadIssuesSettings> MarkdownlintV1LogFileContent { get; } = new Dictionary<byte[], IReadIssuesSettings>();

    /// <summary>
    /// Gets list of registered paths to ESLint log files in JSON format.
    /// </summary>
    public IDictionary<FilePath, IReadIssuesSettings> EsLintJsonLogFilePaths { get; } = new Dictionary<FilePath, IReadIssuesSettings>();

    /// <summary>
    /// Gets list of registered content of ESLint log files in JSON format.
    /// </summary>
    public IDictionary<byte[], IReadIssuesSettings> EsLintJsonLogFileContent { get; } = new Dictionary<byte[], IReadIssuesSettings>();

    /// <summary>
    /// Gets list of registered paths to SARIF log files.
    /// </summary>
    public IDictionary<FilePath, IReadIssuesSettings> SarifLogFilePaths { get; } = new Dictionary<FilePath, IReadIssuesSettings>();

    /// <summary>
    /// Gets list of registered content of SARIF log files.
    /// </summary>
    public IDictionary<byte[], IReadIssuesSettings> SarifLogFileContent { get; } = new Dictionary<byte[], IReadIssuesSettings>();

    /// <summary>
    /// Gets list of registered paths to TAP log files.
    /// </summary>
    public IDictionary<FilePath, IReadIssuesSettings> GenericTapLogFilePaths { get; } = new Dictionary<FilePath, IReadIssuesSettings>();

    /// <summary>
    /// Gets list of registered content of TAP log files.
    /// </summary>
    public IDictionary<byte[], IReadIssuesSettings> GenericTapLogFileContent { get; } = new Dictionary<byte[], IReadIssuesSettings>();

    /// <summary>
    /// Gets list of registered paths to Stylelint TAP log files.
    /// </summary>
    public IDictionary<FilePath, IReadIssuesSettings> StylelintTapLogFilePaths { get; } = new Dictionary<FilePath, IReadIssuesSettings>();

    /// <summary>
    /// Gets list of registered content of Stylelint TAP log files.
    /// </summary>
    public IDictionary<byte[], IReadIssuesSettings> StylelintTapLogFileContent { get; } = new Dictionary<byte[], IReadIssuesSettings>();

    /// <summary>
    /// Gets list of registered paths to Textlint TAP log files.
    /// </summary>
    public IDictionary<FilePath, IReadIssuesSettings> TextlintTapLogFilePaths { get; } = new Dictionary<FilePath, IReadIssuesSettings>();

    /// <summary>
    /// Gets list of registered content of Textlint TAP log files.
    /// </summary>
    public IDictionary<byte[], IReadIssuesSettings> TextlintTapLogFileContent { get; } = new Dictionary<byte[], IReadIssuesSettings>();

    #region MsBuildXmlFileLoggerLogFile

    /// <summary>
    /// Adds a path to a MSBuild log file created by XmlFileLogger.
    /// </summary>
    /// <param name="logfilePath">Path to the MSBuild log file.</param>
    public void AddMsBuildXmlFileLoggerLogFilePath(FilePath logfilePath)
    {
        logfilePath.NotNull();

        this.AddMsBuildXmlFileLoggerLogFilePath(logfilePath, null);
    }

    /// <summary>
    /// Adds a path to a MSBuild log file created by XmlFileLogger.
    /// </summary>
    /// <param name="logfilePath">Path to the MSBuild log file.</param>
    /// <param name="settings">Settings for reading the log file. <c>Null</c> for default values.</param>
    public void AddMsBuildXmlFileLoggerLogFilePath(FilePath logfilePath, IReadIssuesSettings settings)
    {
        logfilePath.NotNull();

        if (this.MsBuildXmlFileLoggerLogFilePaths.ContainsKey(logfilePath))
        {
            throw new ArgumentException(
                $"The path '{logfilePath.FullPath}' is already registered for the MsBuildXmlFileLogger log file format.",
                nameof(logfilePath));
        }

        this.MsBuildXmlFileLoggerLogFilePaths.Add(logfilePath, settings);
    }

    /// <summary>
    /// Adds content of a MSBuild log file created by XmlFileLogger.
    /// </summary>
    /// <param name="logfileContent">Content of the MSBuild log file.</param>
    public void AddMsBuildXmlFileLoggerLogFileContent(string logfileContent)
    {
        logfileContent.NotNullOrWhiteSpace();

        this.AddMsBuildXmlFileLoggerLogFileContent(logfileContent.ToByteArray(), null);
    }

    /// <summary>
    /// Adds content of a MSBuild log file created by XmlFileLogger.
    /// </summary>
    /// <param name="logfileContent">Content of the MSBuild log file.</param>
    public void AddMsBuildXmlFileLoggerLogFileContent(byte[] logfileContent)
    {
        logfileContent.NotNullOrEmpty();

        this.AddMsBuildXmlFileLoggerLogFileContent(logfileContent, null);
    }

    /// <summary>
    /// Adds content of a MSBuild log file created by XmlFileLogger.
    /// </summary>
    /// <param name="logfileContent">Content of the MSBuild log file.</param>
    /// <param name="settings">Settings for reading the log file. <c>Null</c> for default values.</param>
    public void AddMsBuildXmlFileLoggerLogFileContent(string logfileContent, IReadIssuesSettings settings)
    {
        logfileContent.NotNullOrEmpty();

        this.AddMsBuildXmlFileLoggerLogFileContent(logfileContent.ToByteArray(), settings);
    }

    /// <summary>
    /// Adds content of a MSBuild log file created by XmlFileLogger.
    /// </summary>
    /// <param name="logfileContent">Content of the MSBuild log file.</param>
    /// <param name="settings">Settings for reading the log file. <c>Null</c> for default values.</param>
    public void AddMsBuildXmlFileLoggerLogFileContent(byte[] logfileContent, IReadIssuesSettings settings)
    {
        logfileContent.NotNullOrEmpty();

        this.MsBuildXmlFileLoggerLogFileContent.Add(logfileContent, settings);
    }

    #endregion

    #region MsBuildBinaryLogFile

    /// <summary>
    /// Adds a path to a MSBuild binary log file.
    /// </summary>
    /// <param name="logfilePath">Path to the MSBuild log file.</param>
    public void AddMsBuildBinaryLogFilePath(FilePath logfilePath)
    {
        logfilePath.NotNull();

        this.AddMsBuildBinaryLogFilePath(logfilePath, null);
    }

    /// <summary>
    /// Adds a path to a MSBuild binary log file.
    /// </summary>
    /// <param name="logfilePath">Path to the MSBuild log file.</param>
    /// <param name="settings">Settings for reading the log file. <c>Null</c> for default values.</param>
    public void AddMsBuildBinaryLogFilePath(FilePath logfilePath, IReadIssuesSettings settings)
    {
        logfilePath.NotNull();

        if (this.MsBuildBinaryLogFilePaths.ContainsKey(logfilePath))
        {
            throw new ArgumentException(
                $"The path '{logfilePath.FullPath}' is already registered for the MsBuildBinary log file format.",
                nameof(logfilePath));
        }

        this.MsBuildBinaryLogFilePaths.Add(logfilePath, settings);
    }

    /// <summary>
    /// Adds content of a MSBuild binary log file.
    /// </summary>
    /// <param name="logfileContent">Content of the MSBuild log file.</param>
    public void AddMsBuildBinaryLogFileContent(string logfileContent)
    {
        logfileContent.NotNullOrWhiteSpace();

        this.AddMsBuildBinaryLogFileContent(logfileContent.ToByteArray(), null);
    }

    /// <summary>
    /// Adds content of a MSBuild binary log file.
    /// </summary>
    /// <param name="logfileContent">Content of the MSBuild log file.</param>
    public void AddMsBuildBinaryLogFileContent(byte[] logfileContent)
    {
        logfileContent.NotNullOrEmpty();

        this.AddMsBuildBinaryLogFileContent(logfileContent, null);
    }

    /// <summary>
    /// Adds content of a MSBuild binary log file.
    /// </summary>
    /// <param name="logfileContent">Content of the MSBuild log file.</param>
    /// <param name="settings">Settings for reading the log file. <c>Null</c> for default values.</param>
    public void AddMsBuildBinaryLogFileContent(string logfileContent, IReadIssuesSettings settings)
    {
        logfileContent.NotNullOrEmpty();

        this.AddMsBuildBinaryLogFileContent(logfileContent.ToByteArray(), settings);
    }

    /// <summary>
    /// Adds content of a MSBuild binary log file.
    /// </summary>
    /// <param name="logfileContent">Content of the MSBuild log file.</param>
    /// <param name="settings">Settings for reading the log file. <c>Null</c> for default values.</param>
    public void AddMsBuildBinaryLogFileContent(byte[] logfileContent, IReadIssuesSettings settings)
    {
        logfileContent.NotNullOrEmpty();

        this.MsBuildBinaryLogFileContent.Add(logfileContent, settings);
    }

    #endregion

    #region InspectCodeLogFile

    /// <summary>
    /// Adds a path to an InspectCode log file.
    /// </summary>
    /// <param name="logfilePath">Path to the InspectCode log file.</param>
    public void AddInspectCodeLogFilePath(FilePath logfilePath)
    {
        logfilePath.NotNull();

        this.AddInspectCodeLogFilePath(logfilePath, null);
    }

    /// <summary>
    /// Adds a path to a InspectCode log file.
    /// </summary>
    /// <param name="logfilePath">Path to the InspectCode log file.</param>
    /// <param name="settings">Settings for reading the log file. <c>Null</c> for default values.</param>
    public void AddInspectCodeLogFilePath(FilePath logfilePath, IReadIssuesSettings settings)
    {
        logfilePath.NotNull();

        if (this.InspectCodeLogFilePaths.ContainsKey(logfilePath))
        {
            throw new ArgumentException(
                $"The path '{logfilePath.FullPath}' is already registered for InspectCode issue provider.",
                nameof(logfilePath));
        }

        this.InspectCodeLogFilePaths.Add(logfilePath, settings);
    }

    /// <summary>
    /// Adds content of an InspectCode log file.
    /// </summary>
    /// <param name="logfileContent">Content of the InspectCode log file.</param>
    public void AddInspectCodeLogFileContent(string logfileContent)
    {
        logfileContent.NotNullOrWhiteSpace();

        this.AddInspectCodeLogFileContent(logfileContent.ToByteArray(), null);
    }

    /// <summary>
    /// Adds content of an InspectCode log file.
    /// </summary>
    /// <param name="logfileContent">Content of the InspectCode log file.</param>
    public void AddInspectCodeLogFileContent(byte[] logfileContent)
    {
        logfileContent.NotNullOrEmpty();

        this.AddInspectCodeLogFileContent(logfileContent, null);
    }

    /// <summary>
    /// Adds content of an InspectCode log file.
    /// </summary>
    /// <param name="logfileContent">Content of the InspectCode log file.</param>
    /// <param name="settings">Settings for reading the log file. <c>Null</c> for default values.</param>
    public void AddInspectCodeLogFileContent(string logfileContent, IReadIssuesSettings settings)
    {
        logfileContent.NotNullOrEmpty();

        this.AddInspectCodeLogFileContent(logfileContent.ToByteArray(), settings);
    }

    /// <summary>
    /// Adds content of a InspectCode log file.
    /// </summary>
    /// <param name="logfileContent">Content of the InspectCode log file.</param>
    /// <param name="settings">Settings for reading the log file. <c>Null</c> for default values.</param>
    public void AddInspectCodeLogFileContent(byte[] logfileContent, IReadIssuesSettings settings)
    {
        logfileContent.NotNullOrEmpty();

        this.InspectCodeLogFileContent.Add(logfileContent, settings);
    }

    #endregion

    #region MarkdownlintCliLogFile

    /// <summary>
    /// Adds a path to a markdownlint-cli log file.
    /// </summary>
    /// <param name="logfilePath">Path to the markdownlint-cli log file.</param>
    public void AddMarkdownlintCliLogFilePath(FilePath logfilePath)
    {
        logfilePath.NotNull();

        this.AddMarkdownlintCliLogFilePath(logfilePath, null);
    }

    /// <summary>
    /// Adds a path to a markdownlint-cli log file.
    /// </summary>
    /// <param name="logfilePath">Path to the markdownlint-cli log file.</param>
    /// <param name="settings">Settings for reading the log file. <c>Null</c> for default values.</param>
    public void AddMarkdownlintCliLogFilePath(FilePath logfilePath, IReadIssuesSettings settings)
    {
        logfilePath.NotNull();

        if (this.MarkdownlintCliLogFilePaths.ContainsKey(logfilePath))
        {
            throw new ArgumentException(
                $"The path '{logfilePath.FullPath}' is already registered for the MarkdownlintCli log file format.",
                nameof(logfilePath));
        }

        this.MarkdownlintCliLogFilePaths.Add(logfilePath, settings);
    }

    /// <summary>
    /// Adds content of a markdownlint-cli log file.
    /// </summary>
    /// <param name="logfileContent">Content of the markdownlint-cli log file.</param>
    public void AddMarkdownlintCliLogFileContent(string logfileContent)
    {
        logfileContent.NotNullOrWhiteSpace();

        this.AddMarkdownlintCliLogFileContent(logfileContent.ToByteArray(), null);
    }

    /// <summary>
    /// Adds content of a markdownlint-cli log file.
    /// </summary>
    /// <param name="parameters">Parameter instance.</param>
    /// <param name="logfileContent">Content of the markdownlint-cli log file.</param>
    public void AddMarkdownlintCliLogFileContent(byte[] logfileContent)
    {
        logfileContent.NotNullOrEmpty();

        this.AddMarkdownlintCliLogFileContent(logfileContent, null);
    }

    /// <summary>
    /// Adds content of a markdownlint-cli log file.
    /// </summary>
    /// <param name="logfileContent">Content of the markdownlint-cli log file.</param>
    /// <param name="settings">Settings for reading the log file. <c>Null</c> for default values.</param>
    public void AddMarkdownlintCliLogFileContent(string logfileContent, IReadIssuesSettings settings)
    {
        logfileContent.NotNullOrEmpty();

        this.AddMarkdownlintCliLogFileContent(logfileContent.ToByteArray(), settings);
    }

    /// <summary>
    /// Adds content of a markdownlint-cli log file.
    /// </summary>
    /// <param name="logfileContent">Content of the markdownlint-cli log file.</param>
    /// <param name="settings">Settings for reading the log file. <c>Null</c> for default values.</param>
    public void AddMarkdownlintCliLogFileContent(byte[] logfileContent, IReadIssuesSettings settings)
    {
        logfileContent.NotNullOrEmpty();

        this.MarkdownlintCliLogFileContent.Add(logfileContent, settings);
    }

    #endregion

    #region MarkdownlintCliJsonLogFile

    /// <summary>
    /// Adds a path to a markdownlint-cli log file created with <c>--json</c>.
    /// </summary>
    /// <param name="logfilePath">Path to the markdownlint-cli log file.</param>
    public void AddMarkdownlintCliJsonLogFilePath(FilePath logfilePath)
    {
        logfilePath.NotNull();

        this.AddMarkdownlintCliJsonLogFilePath(logfilePath, null);
    }

    /// <summary>
    /// Adds a path to a markdownlint-cli log file created with <c>--json</c>.
    /// </summary>
    /// <param name="logfilePath">Path to the markdownlint-cli log file.</param>
    /// <param name="settings">Settings for reading the log file. <c>Null</c> for default values.</param>
    public void AddMarkdownlintCliJsonLogFilePath(FilePath logfilePath, IReadIssuesSettings settings)
    {
        logfilePath.NotNull();

        if (this.MarkdownlintCliJsonLogFilePaths.ContainsKey(logfilePath))
        {
            throw new ArgumentException(
                $"The path '{logfilePath.FullPath}' is already registered for the MarkdownlintCliJson log file format.",
                nameof(logfilePath));
        }

        this.MarkdownlintCliJsonLogFilePaths.Add(logfilePath, settings);
    }

    /// <summary>
    /// Adds content of a markdownlint-cli log file created with <c>--json</c>.
    /// </summary>
    /// <param name="logfileContent">Content of the markdownlint-cli log file.</param>
    public void AddMarkdownlintCliJsonLogFileContent(string logfileContent)
    {
        logfileContent.NotNullOrWhiteSpace();

        this.AddMarkdownlintCliJsonLogFileContent(logfileContent.ToByteArray(), null);
    }

    /// <summary>
    /// Adds content of a markdownlint-cli log file created with <c>--json</c>.
    /// </summary>
    /// <param name="logfileContent">Content of the markdownlint-cli log file.</param>
    public void AddMarkdownlintCliJsonLogFileContent(byte[] logfileContent)
    {
        logfileContent.NotNullOrEmpty();

        this.AddMarkdownlintCliJsonLogFileContent(logfileContent, null);
    }

    /// <summary>
    /// Adds content of a markdownlint-cli log file created with <c>--json</c>.
    /// </summary>
    /// <param name="logfileContent">Content of the markdownlint-cli log file.</param>
    /// <param name="settings">Settings for reading the log file. <c>Null</c> for default values.</param>
    public void AddMarkdownlintCliJsonLogFileContent(string logfileContent, IReadIssuesSettings settings)
    {
        logfileContent.NotNullOrEmpty();

        this.AddMarkdownlintCliJsonLogFileContent(logfileContent.ToByteArray(), settings);
    }

    /// <summary>
    /// Adds content of a markdownlint-cli log file created with <c>--json</c>.
    /// </summary>
    /// <param name="logfileContent">Content of the markdownlint-cli log file.</param>
    /// <param name="settings">Settings for reading the log file. <c>Null</c> for default values.</param>
    public void AddMarkdownlintCliJsonLogFileContent(byte[] logfileContent, IReadIssuesSettings settings)
    {
        logfileContent.NotNullOrEmpty();

        this.MarkdownlintCliJsonLogFileContent.Add(logfileContent, settings);
    }

    #endregion

    #region MarkdownlintV1LogFile

    /// <summary>
    /// Adds a path to a markdownlint log file in version 1.
    /// </summary>
    /// <param name="logfilePath">Path to the markdownlint log file.</param>
    public void AddMarkdownlintV1LogFilePath(FilePath logfilePath)
    {
        logfilePath.NotNull();

        this.AddMarkdownlintV1LogFilePath(logfilePath, null);
    }

    /// <summary>
    /// Adds a path to a markdownlint log file in version 1.
    /// </summary>
    /// <param name="logfilePath">Path to the markdownlint log file.</param>
    /// <param name="settings">Settings for reading the log file. <c>Null</c> for default values.</param>
    public void AddMarkdownlintV1LogFilePath(FilePath logfilePath, IReadIssuesSettings settings)
    {
        logfilePath.NotNull();

        if (this.MarkdownlintV1LogFilePaths.ContainsKey(logfilePath))
        {
            throw new ArgumentException(
                $"The path '{logfilePath.FullPath}' is already registered for the MarkdownlintV1 log file format.",
                nameof(logfilePath));
        }

        this.MarkdownlintV1LogFilePaths.Add(logfilePath, settings);
    }

    /// <summary>
    /// Adds content of a markdownlint log file in version 1.
    /// </summary>
    /// <param name="logfileContent">Content of the markdownlint log file.</param>
    public void AddMarkdownlintV1LogFileContent(string logfileContent)
    {
        logfileContent.NotNullOrWhiteSpace();

        this.AddMarkdownlintV1LogFileContent(logfileContent.ToByteArray(), null);
    }

    /// <summary>
    /// Adds content of a markdownlint log file in version 1.
    /// </summary>
    /// <param name="logfileContent">Content of the markdownlint log file.</param>
    public void AddMarkdownlintV1LogFileContent(byte[] logfileContent)
    {
        logfileContent.NotNullOrEmpty();

        this.AddMarkdownlintV1LogFileContent(logfileContent, null);
    }

    /// <summary>
    /// Adds content of a markdownlint log file in version 1.
    /// </summary>
    /// <param name="logfileContent">Content of the markdownlint log file.</param>
    /// <param name="settings">Settings for reading the log file. <c>Null</c> for default values.</param>
    public void AddMarkdownlintV1LogFileContent(string logfileContent, IReadIssuesSettings settings)
    {
        logfileContent.NotNullOrEmpty();

        this.AddMarkdownlintV1LogFileContent(logfileContent.ToByteArray(), settings);
    }

    /// <summary>
    /// Adds content of a markdownlint log file in version 1.
    /// </summary>
    /// <param name="logfileContent">Content of the markdownlint log file.</param>
    /// <param name="settings">Settings for reading the log file. <c>Null</c> for default values.</param>
    public void AddMarkdownlintV1LogFileContent(byte[] logfileContent, IReadIssuesSettings settings)
    {
        logfileContent.NotNullOrEmpty();

        this.MarkdownlintV1LogFileContent.Add(logfileContent, settings);
    }

    #endregion

    #region EsLintJsonLogFile

    /// <summary>
    /// Adds a path to a ESLint log file in JSON format.
    /// </summary>
    /// <param name="logfilePath">Path to the ESLint log file.</param>
    public void AddEsLintJsonLogFilePath(FilePath logfilePath)
    {
        logfilePath.NotNull();

        this.AddEsLintJsonLogFilePath(logfilePath, null);
    }

    /// <summary>
    /// Adds a path to a ESLint log file in JSON format.
    /// </summary>
    /// <param name="logfilePath">Path to the ESLint log file.</param>
    /// <param name="settings">Settings for reading the log file. <c>Null</c> for default values.</param>
    public void AddEsLintJsonLogFilePath(FilePath logfilePath, IReadIssuesSettings settings)
    {
        logfilePath.NotNull();

        if (this.EsLintJsonLogFilePaths.ContainsKey(logfilePath))
        {
            throw new ArgumentException(
                $"The path '{logfilePath.FullPath}' is already registered for the EsLintJson log file format.",
                nameof(logfilePath));
        }

        this.EsLintJsonLogFilePaths.Add(logfilePath, settings);
    }

    /// <summary>
    /// Adds content of a ESLint log file in JSON format.
    /// </summary>
    /// <param name="logfileContent">Content of the ESLint log file.</param>
    public void AddEsLintJsonLogFileContent(string logfileContent)
    {
        logfileContent.NotNullOrWhiteSpace();

        this.AddEsLintJsonLogFileContent(logfileContent.ToByteArray(), null);
    }

    /// <summary>
    /// Adds content of a ESLint log file in JSON format.
    /// </summary>
    /// <param name="logfileContent">Content of the ESLint log file.</param>
    public void AddEsLintJsonLogFileContent(byte[] logfileContent)
    {
        logfileContent.NotNullOrEmpty();

        this.AddEsLintJsonLogFileContent(logfileContent, null);
    }

    /// <summary>
    /// Adds content of a ESLint log file in JSON format.
    /// </summary>
    /// <param name="logfileContent">Content of the ESLint log file.</param>
    /// <param name="settings">Settings for reading the log file. <c>Null</c> for default values.</param>
    public void AddEsLintJsonLogFileContent(string logfileContent, IReadIssuesSettings settings)
    {
        logfileContent.NotNullOrEmpty();

        this.AddEsLintJsonLogFileContent(logfileContent.ToByteArray(), settings);
    }

    /// <summary>
    /// Adds content of a ESLint log file in JSON format.
    /// </summary>
    /// <param name="logfileContent">Content of the ESLint log file.</param>
    /// <param name="settings">Settings for reading the log file. <c>Null</c> for default values.</param>
    public void AddEsLintJsonLogFileContent(byte[] logfileContent, IReadIssuesSettings settings)
    {
        logfileContent.NotNullOrEmpty();

        this.EsLintJsonLogFileContent.Add(logfileContent, settings);
    }

    #endregion

    #region SarifLogFile

    /// <summary>
    /// Adds a path to a log file in SARIF format.
    /// </summary>
    /// <param name="logfilePath">Path to the SARIF log file.</param>
    public void AddSarifLogFilePath(FilePath logfilePath)
    {
        logfilePath.NotNull();

        this.AddSarifLogFilePath(logfilePath, null);
    }

    /// <summary>
    /// Adds a path to a log file in SARIF format.
    /// </summary>
    /// <param name="logfilePath">Path to the SARIF log file.</param>
    /// <param name="settings">Settings for reading the log file. <c>Null</c> for default values.</param>
    public void AddSarifLogFilePath(FilePath logfilePath, IReadIssuesSettings settings)
    {
        logfilePath.NotNull();

        if (this.SarifLogFilePaths.ContainsKey(logfilePath))
        {
            throw new ArgumentException(
                $"The path '{logfilePath.FullPath}' is already registered for the SARIF issue provider.",
                nameof(logfilePath));
        }

        this.SarifLogFilePaths.Add(logfilePath, settings);
    }

    /// <summary>
    /// Adds content of a log file in SARIF format.
    /// </summary>
    /// <param name="logfileContent">Content of the SARIF log file.</param>
    public void AddSarifLogFileContent(string logfileContent)
    {
        logfileContent.NotNullOrWhiteSpace();

        this.AddSarifLogFileContent(logfileContent.ToByteArray(), null);
    }

    /// <summary>
    /// Adds content of a log file in SARIF format.
    /// </summary>
    /// <param name="logfileContent">Content of the SARIF log file.</param>
    public void AddSarifLogFileContent(byte[] logfileContent)
    {
        logfileContent.NotNullOrEmpty();

        this.AddSarifLogFileContent(logfileContent, null);
    }

    /// <summary>
    /// Adds content of a log file in SARIF format.
    /// </summary>
    /// <param name="logfileContent">Content of the SARIF log file.</param>
    /// <param name="settings">Settings for reading the log file. <c>Null</c> for default values.</param>
    public void AddSarifLogFileContent(
        string logfileContent,
        IReadIssuesSettings settings)
    {
        logfileContent.NotNullOrEmpty();

        this.AddSarifLogFileContent(logfileContent.ToByteArray(), settings);
    }

    /// <summary>
    /// Adds content of a log file in SARIF format.
    /// </summary>
    /// <param name="logfileContent">Content of the SARIF log file.</param>
    /// <param name="settings">Settings for reading the log file. <c>Null</c> for default values.</param>
    public void AddSarifLogFileContent(byte[] logfileContent, IReadIssuesSettings settings)
    {
        logfileContent.NotNullOrEmpty();

        this.SarifLogFileContent.Add(logfileContent, settings);
    }

   #endregion

    #region GenericTapLogFile

    /// <summary>
    /// Adds a path to a log file in Test Anything Protocol format.
    /// </summary>
    /// <param name="logfilePath">Path to the log file in Test Anything Protocol format.</param>
    public void AddGenericTapLogFilePath(FilePath logfilePath)
    {
        logfilePath.NotNull();

        this.AddGenericTapLogFilePath(logfilePath, null);
    }

    /// <summary>
    /// Adds a path to a log file in Test Anything Protocol format.
    /// </summary>
    /// <param name="logfilePath">Path to the log file in Test Anything Protocol format.</param>
    /// <param name="settings">Settings for reading the log file. <c>Null</c> for default values.</param>
    public void AddGenericTapLogFilePath(FilePath logfilePath, IReadIssuesSettings settings)
    {
        logfilePath.NotNull();

        if (this.GenericTapLogFilePaths.ContainsKey(logfilePath))
        {
            throw new ArgumentException(
                $"The path '{logfilePath.FullPath}' is already registered for the Generic TAP issue provider.",
                nameof(logfilePath));
        }

        this.GenericTapLogFilePaths.Add(logfilePath, settings);
    }

    /// <summary>
    /// Adds content of a log file in Test Anything Protocol format.
    /// </summary>
    /// <param name="logfileContent">Content of the log file in Test Anything Protocol format.</param>
    public void AddGenericTapLogFileContent(string logfileContent)
    {
        logfileContent.NotNullOrWhiteSpace();

        this.AddGenericTapLogFileContent(logfileContent.ToByteArray(), null);
    }

    /// <summary>
    /// Adds content of a log file in Test Anything Protocol format.
    /// </summary>
    /// <param name="logfileContent">Content of the log file in Test Anything Protocol format.</param>
    public void AddGenericTapLogFileContent(byte[] logfileContent)
    {
        logfileContent.NotNullOrEmpty();

        this.AddGenericTapLogFileContent(logfileContent, null);
    }

    /// <summary>
    /// Adds content of a log file in Test Anything Protocol format.
    /// </summary>
    /// <param name="logfileContent">Content of the log file in Test Anything Protocol format.</param>
    /// <param name="settings">Settings for reading the log file. <c>Null</c> for default values.</param>
    public void AddGenericTapLogFileContent(
        string logfileContent,
        IReadIssuesSettings settings)
    {
        logfileContent.NotNullOrEmpty();

        this.AddGenericTapLogFileContent(logfileContent.ToByteArray(), settings);
    }

    /// <summary>
    /// Adds content of a log file in Test Anything Protocol format.
    /// </summary>
    /// <param name="logfileContent">Content of the log file in Test Anything Protocol format.</param>
    /// <param name="settings">Settings for reading the log file. <c>Null</c> for default values.</param>
    public void AddGenericTapLogFileContent(byte[] logfileContent, IReadIssuesSettings settings)
    {
        logfileContent.NotNullOrEmpty();

        this.GenericTapLogFileContent.Add(logfileContent, settings);
    }

   #endregion

    #region StylelintTapLogFile

    /// <summary>
    /// Adds a path to a Stylelint log file in Test Anything Protocol format.
    /// </summary>
    /// <param name="logfilePath">Path to the Stylelint log file in Test Anything Protocol format.</param>
    public void AddStylelintTapLogFilePath(FilePath logfilePath)
    {
        logfilePath.NotNull();

        this.AddStylelintTapLogFilePath(logfilePath, null);
    }

    /// <summary>
    /// Adds a path to a Stylelint log file in Test Anything Protocol format.
    /// </summary>
    /// <param name="logfilePath">Path to the Stylelint log file in Test Anything Protocol format.</param>
    /// <param name="settings">Settings for reading the log file. <c>Null</c> for default values.</param>
    public void AddStylelintTapLogFilePath(FilePath logfilePath, IReadIssuesSettings settings)
    {
        logfilePath.NotNull();

        if (this.StylelintTapLogFilePaths.ContainsKey(logfilePath))
        {
            throw new ArgumentException(
                $"The path '{logfilePath.FullPath}' is already registered for the Stylelint TAP issue provider.",
                nameof(logfilePath));
        }

        this.StylelintTapLogFilePaths.Add(logfilePath, settings);
    }

    /// <summary>
    /// Adds content of a Stylelint log file in Test Anything Protocol format.
    /// </summary>
    /// <param name="logfileContent">Content of the Stylelint log file in Test Anything Protocol format.</param>
    public void AddStylelintTapLogFileContent(string logfileContent)
    {
        logfileContent.NotNullOrWhiteSpace();

        this.AddStylelintTapLogFileContent(logfileContent.ToByteArray(), null);
    }

    /// <summary>
    /// Adds content of a Stylelint log file in Test Anything Protocol format.
    /// </summary>
    /// <param name="logfileContent">Content of the Stylelint log file in Test Anything Protocol format.</param>
    public void AddStylelintTapLogFileContent(byte[] logfileContent)
    {
        logfileContent.NotNullOrEmpty();

        this.AddStylelintTapLogFileContent(logfileContent, null);
    }

    /// <summary>
    /// Adds content of a Stylelint log file in Test Anything Protocol format.
    /// </summary>
    /// <param name="logfileContent">Content of the Stylelint log file in Test Anything Protocol format.</param>
    /// <param name="settings">Settings for reading the log file. <c>Null</c> for default values.</param>
    public void AddStylelintTapLogFileContent(
        string logfileContent,
        IReadIssuesSettings settings)
    {
        logfileContent.NotNullOrEmpty();

        this.AddStylelintTapLogFileContent(logfileContent.ToByteArray(), settings);
    }

    /// <summary>
    /// Adds content of a Stylelint log file in Test Anything Protocol format.
    /// </summary>
    /// <param name="logfileContent">Content of the Stylelint log file in Test Anything Protocol format.</param>
    /// <param name="settings">Settings for reading the log file. <c>Null</c> for default values.</param>
    public void AddStylelintTapLogFileContent(byte[] logfileContent, IReadIssuesSettings settings)
    {
        logfileContent.NotNullOrEmpty();

        this.StylelintTapLogFileContent.Add(logfileContent, settings);
    }

   #endregion

    #region TextlintTapLogFile

    /// <summary>
    /// Adds a path to a Textlint log file in Test Anything Protocol format.
    /// </summary>
    /// <param name="logfilePath">Path to the Textlint log file in Test Anything Protocol format.</param>
    public void AddTextlintTapLogFilePath(FilePath logfilePath)
    {
        logfilePath.NotNull();

        this.AddTextlintTapLogFilePath(logfilePath, null);
    }

    /// <summary>
    /// Adds a path to a Textlint log file in Test Anything Protocol format.
    /// </summary>
    /// <param name="logfilePath">Path to the Textlint log file in Test Anything Protocol format.</param>
    /// <param name="settings">Settings for reading the log file. <c>Null</c> for default values.</param>
    public void AddTextlintTapLogFilePath(FilePath logfilePath, IReadIssuesSettings settings)
    {
        logfilePath.NotNull();

        if (this.TextlintTapLogFilePaths.ContainsKey(logfilePath))
        {
            throw new ArgumentException(
                $"The path '{logfilePath.FullPath}' is already registered for the Textlint TAP issue provider.",
                nameof(logfilePath));
        }

        this.TextlintTapLogFilePaths.Add(logfilePath, settings);
    }

    /// <summary>
    /// Adds content of a Textlint log file in Test Anything Protocol format.
    /// </summary>
    /// <param name="logfileContent">Content of the Textlint log file in Test Anything Protocol format.</param>
    public void AddTextlintTapLogFileContent(string logfileContent)
    {
        logfileContent.NotNullOrWhiteSpace();

        this.AddTextlintTapLogFileContent(logfileContent.ToByteArray(), null);
    }

    /// <summary>
    /// Adds content of a Textlint log file in Test Anything Protocol format.
    /// </summary>
    /// <param name="logfileContent">Content of the Textlint log file in Test Anything Protocol format.</param>
    public void AddTextlintTapLogFileContent(byte[] logfileContent)
    {
        logfileContent.NotNullOrEmpty();

        this.AddTextlintTapLogFileContent(logfileContent, null);
    }

    /// <summary>
    /// Adds content of a Textlint log file in Test Anything Protocol format.
    /// </summary>
    /// <param name="logfileContent">Content of the Textlint log file in Test Anything Protocol format.</param>
    /// <param name="settings">Settings for reading the log file. <c>Null</c> for default values.</param>
    public void AddTextlintTapLogFileContent(
        string logfileContent,
        IReadIssuesSettings settings)
    {
        logfileContent.NotNullOrEmpty();

        this.AddTextlintTapLogFileContent(logfileContent.ToByteArray(), settings);
    }

    /// <summary>
    /// Adds content of a Textlint log file in Test Anything Protocol format.
    /// </summary>
    /// <param name="logfileContent">Content of the Textlint log file in Test Anything Protocol format.</param>
    /// <param name="settings">Settings for reading the log file. <c>Null</c> for default values.</param>
    public void AddTextlintTapLogFileContent(byte[] logfileContent, IReadIssuesSettings settings)
    {
        logfileContent.NotNullOrEmpty();

        this.TextlintTapLogFileContent.Add(logfileContent, settings);
    }

   #endregion
}