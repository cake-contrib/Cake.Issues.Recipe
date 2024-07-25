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
        public IDictionary<FilePath, IReadIssuesSettings> MsBuildBinaryLogFilePaths { get; } = new Dictionary<FilePath, IReadIssuesSettings>();

        /// <inheritdoc />
        public IDictionary<FilePath, IReadIssuesSettings> InspectCodeLogFilePaths { get; } = new Dictionary<FilePath, IReadIssuesSettings>();

        /// <inheritdoc />
        public IDictionary<FilePath, IReadIssuesSettings> MarkdownlintCliLogFilePaths { get; } = new Dictionary<FilePath, IReadIssuesSettings>();

        /// <inheritdoc />
        public IDictionary<FilePath, IReadIssuesSettings> MarkdownlintCliJsonLogFilePaths { get; } = new Dictionary<FilePath, IReadIssuesSettings>();

        /// <inheritdoc />
        public IDictionary<FilePath, IReadIssuesSettings> MarkdownlintV1LogFilePaths { get; } = new Dictionary<FilePath, IReadIssuesSettings>();

        /// <inheritdoc />
        public IDictionary<FilePath, IReadIssuesSettings> EsLintJsonLogFilePaths { get; } = new Dictionary<FilePath, IReadIssuesSettings>();

        /// <inheritdoc />
        public IDictionary<FilePath, IReadIssuesSettings> SarifLogFilePaths { get; } = new Dictionary<FilePath, IReadIssuesSettings>();

        /// <inheritdoc />
        public void AddMsBuildXmlFileLoggerLogFile(FilePath logfilePath)
        {
            logfilePath.NotNull(nameof(logfilePath));

            this.AddMsBuildXmlFileLoggerLogFile(logfilePath, null);
        }

        /// <inheritdoc />
        public void AddMsBuildXmlFileLoggerLogFile(FilePath logfilePath, IReadIssuesSettings settings)
        {
            logfilePath.NotNull(nameof(logfilePath));

            this.MsBuildXmlFileLoggerLogFilePaths.Add(logfilePath, settings);
        }

        /// <inheritdoc />
        public void AddMsBuildBinaryLogFile(FilePath logfilePath)
        {
            logfilePath.NotNull(nameof(logfilePath));

            this.AddMsBuildBinaryLogFile(logfilePath, null);
        }

        /// <inheritdoc />
        public void AddMsBuildBinaryLogFile(FilePath logfilePath, IReadIssuesSettings settings)
        {
            logfilePath.NotNull(nameof(logfilePath));

            this.MsBuildBinaryLogFilePaths.Add(logfilePath, settings);
        }

        /// <inheritdoc />
        public void AddInspectCodeLogFile(FilePath logfilePath)
        {
            logfilePath.NotNull(nameof(logfilePath));

            this.AddInspectCodeLogFile(logfilePath, null);
        }

        /// <inheritdoc />
        public void AddInspectCodeLogFile(FilePath logfilePath, IReadIssuesSettings settings)
        {
            logfilePath.NotNull(nameof(logfilePath));

            this.InspectCodeLogFilePaths.Add(logfilePath, settings);
        }

        /// <inheritdoc />
        public void AddMarkdownlintCliLogFile(FilePath logfilePath)
        {
            logfilePath.NotNull(nameof(logfilePath));

            this.AddMarkdownlintCliLogFile(logfilePath, null);
        }

        /// <inheritdoc />
        public void AddMarkdownlintCliLogFile(FilePath logfilePath, IReadIssuesSettings settings)
        {
            logfilePath.NotNull(nameof(logfilePath));

            this.MarkdownlintCliLogFilePaths.Add(logfilePath, settings);
        }

        /// <inheritdoc />
        public void AddMarkdownlintCliJsonLogFile(FilePath logfilePath)
        {
            logfilePath.NotNull(nameof(logfilePath));

            this.AddMarkdownlintCliJsonLogFile(logfilePath, null);
        }

        /// <inheritdoc />
        public void AddMarkdownlintCliJsonLogFile(FilePath logfilePath, IReadIssuesSettings settings)
        {
            logfilePath.NotNull(nameof(logfilePath));

            this.MarkdownlintCliJsonLogFilePaths.Add(logfilePath, settings);
        }


        /// <inheritdoc />
        public void AddMarkdownlintV1LogFile(FilePath logfilePath)
        {
            logfilePath.NotNull(nameof(logfilePath));

            this.AddMarkdownlintV1LogFile(logfilePath, null);
        }

        /// <inheritdoc />
        public void AddMarkdownlintV1LogFile(FilePath logfilePath, IReadIssuesSettings settings)
        {
            logfilePath.NotNull(nameof(logfilePath));

            this.MarkdownlintV1LogFilePaths.Add(logfilePath, settings);
        }

        /// <inheritdoc />
        public void AddEsLintJsonLogFile(FilePath logfilePath)
        {
            logfilePath.NotNull(nameof(logfilePath));

            this.AddEsLintJsonLogFile(logfilePath, null);
        }

        /// <inheritdoc />
        public void AddEsLintJsonLogFile(FilePath logfilePath, IReadIssuesSettings settings)
        {
            logfilePath.NotNull(nameof(logfilePath));

            this.EsLintJsonLogFilePaths.Add(logfilePath, settings);
        }

        /// <inheritdoc />
        public void AddSarifLogFile(FilePath logfilePath)
        {
            logfilePath.NotNull(nameof(logfilePath));

            this.AddSarifLogFile(logfilePath, null);
        }

        /// <inheritdoc />
        public void AddSarifLogFile(FilePath logfilePath, IReadIssuesSettings settings)
        {
            logfilePath.NotNull(nameof(logfilePath));

            this.SarifLogFilePaths.Add(logfilePath, settings);
        }
    }
}