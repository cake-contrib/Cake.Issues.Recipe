namespace Cake.Frosting.Issues.Recipe
{
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
        public void AddMsBuildXmlFileLoggerLogFile(FilePath logfilePath)
        {
            this.AddMsBuildXmlFileLoggerLogFilePath(logfilePath);
        }

        /// <inheritdoc />
        public void AddMsBuildXmlFileLoggerLogFile(FilePath logfilePath, IReadIssuesSettings settings)
        {
            this.AddMsBuildXmlFileLoggerLogFilePath(logfilePath, settings);
        }

        /// <inheritdoc />
        public void AddMsBuildXmlFileLoggerLogFilePath(FilePath logfilePath, IReadIssuesSettings settings)
        {
            logfilePath.NotNull();

            this.MsBuildXmlFileLoggerLogFilePaths.Add(logfilePath, settings);
        }

        /// <inheritdoc />
        public void AddMsBuildXmlFileLoggerLogFileContent(byte[] logfileContent, IReadIssuesSettings settings)
        {
            logfileContent.NotNullOrEmpty();

            this.MsBuildXmlFileLoggerLogFileContent.Add(logfileContent, settings);
        }

        /// <inheritdoc />
        public void AddMsBuildBinaryLogFile(FilePath logfilePath)
        {
            this.AddMsBuildBinaryLogFilePath(logfilePath);
        }

        /// <inheritdoc />
        public void AddMsBuildBinaryLogFile(FilePath logfilePath, IReadIssuesSettings settings)
        {
            this.AddMsBuildBinaryLogFilePath(logfilePath, settings);
        }

        /// <inheritdoc />
        public void AddMsBuildBinaryLogFilePath(FilePath logfilePath, IReadIssuesSettings settings)
        {
            logfilePath.NotNull();

            this.MsBuildBinaryLogFilePaths.Add(logfilePath, settings);
        }

        /// <inheritdoc />
        public void AddMsBuildBinaryLogFileContent(byte[] logfileContent, IReadIssuesSettings settings)
        {
            logfileContent.NotNullOrEmpty();

            this.MsBuildBinaryLogFileContent.Add(logfileContent, settings);
        }

        /// <inheritdoc />
        public void AddInspectCodeLogFile(FilePath logfilePath)
        {
            this.AddInspectCodeLogFilePath(logfilePath);
        }

        /// <inheritdoc />
        public void AddInspectCodeLogFile(FilePath logfilePath, IReadIssuesSettings settings)
        {
            this.AddInspectCodeLogFilePath(logfilePath, settings);
        }

        /// <inheritdoc />
        public void AddInspectCodeLogFilePath(FilePath logfilePath, IReadIssuesSettings settings)
        {
            logfilePath.NotNull();

            this.InspectCodeLogFilePaths.Add(logfilePath, settings);
        }

        /// <inheritdoc />
        public void AddInspectCodeLogFileContent(byte[] logfileContent, IReadIssuesSettings settings)
        {
            logfileContent.NotNullOrEmpty();

            this.InspectCodeLogFileContent.Add(logfileContent, settings);
        }

        /// <inheritdoc />
        public void AddMarkdownlintCliLogFile(FilePath logfilePath)
        {
            this.AddMarkdownlintCliLogFilePath(logfilePath);
        }

        /// <inheritdoc />
        public void AddMarkdownlintCliLogFile(FilePath logfilePath, IReadIssuesSettings settings)
        {
            this.AddMarkdownlintCliLogFilePath(logfilePath, settings);
        }

        /// <inheritdoc />
        public void AddMarkdownlintCliLogFilePath(FilePath logfilePath, IReadIssuesSettings settings)
        {
            logfilePath.NotNull();

            this.MarkdownlintCliLogFilePaths.Add(logfilePath, settings);
        }

        /// <inheritdoc />
        public void AddMarkdownlintCliLogFileContent(byte[] logfileContent, IReadIssuesSettings settings)
        {
            logfileContent.NotNullOrEmpty();

            this.MarkdownlintCliLogFileContent.Add(logfileContent, settings);
        }

        /// <inheritdoc />
        public void AddMarkdownlintCliJsonLogFile(FilePath logfilePath)
        {
            this.AddMarkdownlintCliJsonLogFilePath(logfilePath, null);
        }

        /// <inheritdoc />
        public void AddMarkdownlintCliJsonLogFile(FilePath logfilePath, IReadIssuesSettings settings)
        {
            this.AddMarkdownlintCliJsonLogFilePath(logfilePath, settings);
        }

        /// <inheritdoc />
        public void AddMarkdownlintCliJsonLogFilePath(FilePath logfilePath, IReadIssuesSettings settings)
        {
            logfilePath.NotNull();

            this.MarkdownlintCliJsonLogFilePaths.Add(logfilePath, settings);
        }

        /// <inheritdoc />
        public void AddMarkdownlintCliJsonLogFileContent(byte[] logfileContent, IReadIssuesSettings settings)
        {
            logfileContent.NotNullOrEmpty();

            this.MarkdownlintCliJsonLogFileContent.Add(logfileContent, settings);
        }

        /// <inheritdoc />
        public void AddMarkdownlintV1LogFile(FilePath logfilePath)
        {
            this.AddMarkdownlintV1LogFilePath(logfilePath);
        }

        /// <inheritdoc />
        public void AddMarkdownlintV1LogFile(FilePath logfilePath, IReadIssuesSettings settings)
        {
            this.AddMarkdownlintV1LogFilePath(logfilePath, settings);
        }

        /// <inheritdoc />
        public void AddMarkdownlintV1LogFilePath(FilePath logfilePath, IReadIssuesSettings settings)
        {
            logfilePath.NotNull();

            this.MarkdownlintV1LogFilePaths.Add(logfilePath, settings);
        }

        /// <inheritdoc />
        public void AddMarkdownlintV1LogFileContent(byte[] logfileContent, IReadIssuesSettings settings)
        {
            logfileContent.NotNullOrEmpty();

            this.MarkdownlintV1LogFileContent.Add(logfileContent, settings);
        }

        /// <inheritdoc />
        public void AddEsLintJsonLogFile(FilePath logfilePath)
        {
            this.AddEsLintJsonLogFilePath(logfilePath);
        }

        /// <inheritdoc />
        public void AddEsLintJsonLogFile(FilePath logfilePath, IReadIssuesSettings settings)
        {
            this.AddEsLintJsonLogFilePath(logfilePath, settings);
        }

        /// <inheritdoc />
        public void AddEsLintJsonLogFilePath(FilePath logfilePath, IReadIssuesSettings settings)
        {
            logfilePath.NotNull();

            this.EsLintJsonLogFilePaths.Add(logfilePath, settings);
        }

        /// <inheritdoc />
        public void AddEsLintJsonLogFileContent(byte[] logfileContent, IReadIssuesSettings settings)
        {
            logfileContent.NotNullOrEmpty();

            this.EsLintJsonLogFileContent.Add(logfileContent, settings);
        }

        /// <inheritdoc />
        public void AddSarifLogFile(FilePath logfilePath)
        {
            this.AddSarifLogFilePath(logfilePath);
        }

        /// <inheritdoc />
        public void AddSarifLogFile(FilePath logfilePath, IReadIssuesSettings settings)
        {
            this.AddSarifLogFilePath(logfilePath, settings);
        }

        /// <inheritdoc />
        public void AddSarifLogFilePath(FilePath logfilePath, IReadIssuesSettings settings)
        {
            logfilePath.NotNull();

            this.SarifLogFilePaths.Add(logfilePath, settings);
        }

        /// <inheritdoc />
        public void AddSarifLogFileContent(byte[] logfileContent, IReadIssuesSettings settings)
        {
            logfileContent.NotNullOrEmpty();

            this.SarifLogFileContent.Add(logfileContent, settings);
        }
    }
}