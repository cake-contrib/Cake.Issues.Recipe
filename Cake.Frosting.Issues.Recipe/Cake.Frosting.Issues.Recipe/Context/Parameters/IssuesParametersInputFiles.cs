namespace Cake.Frosting.Issues.Recipe;

using Cake.Core.IO;

/// <summary>
/// Parameters for passing input files.
/// </summary>
public class IssuesParametersInputFiles : IIssuesParametersInputFiles
{
    /// <inheritdoc />
    public IDictionary<FilePath, IReadIssuesSettings> MsBuildXmlFileLoggerLogFilePaths { get; } = new Dictionary<FilePath, IReadIssuesSettings>();

    /// <inheritdoc />
    public IDictionary<byte[], IReadIssuesSettings> MsBuildXmlFileLoggerLogFileContent { get; } = new Dictionary<byte[], IReadIssuesSettings>();

    /// <inheritdoc />
    public IDictionary<FilePath, IReadIssuesSettings> MsBuildBinaryLogFilePaths { get; } = new Dictionary<FilePath, IReadIssuesSettings>();

    /// <inheritdoc />
    public IDictionary<byte[], IReadIssuesSettings> MsBuildBinaryLogFileContent { get; } = new Dictionary<byte[], IReadIssuesSettings>();

    /// <inheritdoc />
    public IDictionary<FilePath, IReadIssuesSettings> InspectCodeLogFilePaths { get; } = new Dictionary<FilePath, IReadIssuesSettings>();

    /// <inheritdoc />
    public IDictionary<byte[], IReadIssuesSettings> InspectCodeLogFileContent { get; } = new Dictionary<byte[], IReadIssuesSettings>();

    /// <inheritdoc />
    public IDictionary<FilePath, IReadIssuesSettings> MarkdownlintCliLogFilePaths { get; } = new Dictionary<FilePath, IReadIssuesSettings>();

    /// <inheritdoc />
    public IDictionary<byte[], IReadIssuesSettings> MarkdownlintCliLogFileContent { get; } = new Dictionary<byte[], IReadIssuesSettings>();

    /// <inheritdoc />
    public IDictionary<FilePath, IReadIssuesSettings> MarkdownlintCliJsonLogFilePaths { get; } = new Dictionary<FilePath, IReadIssuesSettings>();

    /// <inheritdoc />
    public IDictionary<byte[], IReadIssuesSettings> MarkdownlintCliJsonLogFileContent { get; } = new Dictionary<byte[], IReadIssuesSettings>();

    /// <inheritdoc />
    public IDictionary<FilePath, IReadIssuesSettings> MarkdownlintV1LogFilePaths { get; } = new Dictionary<FilePath, IReadIssuesSettings>();

    /// <inheritdoc />
    public IDictionary<byte[], IReadIssuesSettings> MarkdownlintV1LogFileContent { get; } = new Dictionary<byte[], IReadIssuesSettings>();

    /// <inheritdoc />
    public IDictionary<FilePath, IReadIssuesSettings> EsLintJsonLogFilePaths { get; } = new Dictionary<FilePath, IReadIssuesSettings>();

    /// <inheritdoc />
    public IDictionary<byte[], IReadIssuesSettings> EsLintJsonLogFileContent { get; } = new Dictionary<byte[], IReadIssuesSettings>();

    /// <inheritdoc />
    public IDictionary<FilePath, IReadIssuesSettings> SarifLogFilePaths { get; } = new Dictionary<FilePath, IReadIssuesSettings>();

    /// <inheritdoc />
    public IDictionary<byte[], IReadIssuesSettings> SarifLogFileContent { get; } = new Dictionary<byte[], IReadIssuesSettings>();

    /// <inheritdoc />
    public IDictionary<FilePath, IReadIssuesSettings> GenericTapLogFilePaths { get; } = new Dictionary<FilePath, IReadIssuesSettings>();

    /// <inheritdoc />
    public IDictionary<byte[], IReadIssuesSettings> GenericTapLogFileContent { get; } = new Dictionary<byte[], IReadIssuesSettings>();

    /// <inheritdoc />
    public IDictionary<FilePath, IReadIssuesSettings> StylelintTapLogFilePaths { get; } = new Dictionary<FilePath, IReadIssuesSettings>();

    /// <inheritdoc />
    public IDictionary<byte[], IReadIssuesSettings> StylelintTapLogFileContent { get; } = new Dictionary<byte[], IReadIssuesSettings>();

    /// <inheritdoc />
    public IDictionary<FilePath, IReadIssuesSettings> TextlintTapLogFilePaths { get; } = new Dictionary<FilePath, IReadIssuesSettings>();

    /// <inheritdoc />
    public IDictionary<byte[], IReadIssuesSettings> TextlintTapLogFileContent { get; } = new Dictionary<byte[], IReadIssuesSettings>();

    /// <inheritdoc />
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

    /// <inheritdoc />
    public void AddMsBuildXmlFileLoggerLogFileContent(byte[] logfileContent, IReadIssuesSettings settings)
    {
        logfileContent.NotNullOrEmpty();

        this.MsBuildXmlFileLoggerLogFileContent.Add(logfileContent, settings);
    }

    /// <inheritdoc />
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

    /// <inheritdoc />
    public void AddMsBuildBinaryLogFileContent(byte[] logfileContent, IReadIssuesSettings settings)
    {
        logfileContent.NotNullOrEmpty();

        this.MsBuildBinaryLogFileContent.Add(logfileContent, settings);
    }

    /// <inheritdoc />
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

    /// <inheritdoc />
    public void AddInspectCodeLogFileContent(byte[] logfileContent, IReadIssuesSettings settings)
    {
        logfileContent.NotNullOrEmpty();

        this.InspectCodeLogFileContent.Add(logfileContent, settings);
    }

    /// <inheritdoc />
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

    /// <inheritdoc />
    public void AddMarkdownlintCliLogFileContent(byte[] logfileContent, IReadIssuesSettings settings)
    {
        logfileContent.NotNullOrEmpty();

        this.MarkdownlintCliLogFileContent.Add(logfileContent, settings);
    }

    /// <inheritdoc />
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

    /// <inheritdoc />
    public void AddMarkdownlintCliJsonLogFileContent(byte[] logfileContent, IReadIssuesSettings settings)
    {
        logfileContent.NotNullOrEmpty();

        this.MarkdownlintCliJsonLogFileContent.Add(logfileContent, settings);
    }

    /// <inheritdoc />
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

    /// <inheritdoc />
    public void AddMarkdownlintV1LogFileContent(byte[] logfileContent, IReadIssuesSettings settings)
    {
        logfileContent.NotNullOrEmpty();

        this.MarkdownlintV1LogFileContent.Add(logfileContent, settings);
    }

    /// <inheritdoc />
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

    /// <inheritdoc />
    public void AddEsLintJsonLogFileContent(byte[] logfileContent, IReadIssuesSettings settings)
    {
        logfileContent.NotNullOrEmpty();

        this.EsLintJsonLogFileContent.Add(logfileContent, settings);
    }

    /// <inheritdoc />
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

    /// <inheritdoc />
    public void AddSarifLogFileContent(byte[] logfileContent, IReadIssuesSettings settings)
    {
        logfileContent.NotNullOrEmpty();

        this.SarifLogFileContent.Add(logfileContent, settings);
    }

    /// <inheritdoc />
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

    /// <inheritdoc />
    public void AddGenericTapLogFileContent(byte[] logfileContent, IReadIssuesSettings settings)
    {
        logfileContent.NotNullOrEmpty();

        this.GenericTapLogFileContent.Add(logfileContent, settings);
    }

    /// <inheritdoc />
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

    /// <inheritdoc />
    public void AddStylelintTapLogFileContent(byte[] logfileContent, IReadIssuesSettings settings)
    {
        logfileContent.NotNullOrEmpty();

        this.StylelintTapLogFileContent.Add(logfileContent, settings);
    }

    /// <inheritdoc />
    public void AddTextlintTapLogFilePath(FilePath logfilePath, IReadIssuesSettings settings)
    {
        logfilePath.NotNull();

        if (this.StylelintTapLogFilePaths.ContainsKey(logfilePath))
        {
            throw new ArgumentException(
                $"The path '{logfilePath.FullPath}' is already registered for the Textlint TAP issue provider.",
                nameof(logfilePath));
        }

        this.StylelintTapLogFilePaths.Add(logfilePath, settings);
    }

    /// <inheritdoc />
    public void AddTextlintTapLogFileContent(byte[] logfileContent, IReadIssuesSettings settings)
    {
        logfileContent.NotNullOrEmpty();

        this.StylelintTapLogFileContent.Add(logfileContent, settings);
    }
}