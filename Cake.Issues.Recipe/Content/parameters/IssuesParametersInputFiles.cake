/// <summary>
/// Parameters for passing input files.
/// </summary>
public class IssuesParametersInputFiles
{
    /// <summary>
    /// Gets or sets the path to the MSBuild log file created by XmlFileLogger.
    /// </summary>
    public FilePath MsBuildXmlFileLoggerLogFilePath { get; set; }

    /// <summary>
    /// Gets or sets the path to the MSBuild binary log file.
    /// </summary>
    public FilePath MsBuildBinaryLogFilePath { get; set; }

    /// <summary>
    /// Gets or sets the path to the InspectCode log file.
    /// </summary>
    public FilePath InspectCodeLogFilePath { get; set; }

    /// <summary>
    /// Gets or sets the path to the dupFinder log file.
    /// </summary>
    public FilePath DupFinderLogFilePath { get; set; }

    /// <summary>
    /// Gets or sets the path to the markdownlint-cli log file.
    /// </summary>
    public FilePath MarkdownlintCliLogFilePath { get; set; }

    /// <summary>
    /// Gets or sets the path to the markdownlint log file in version 1.
    /// </summary>
    public FilePath MarkdownlintV1LogFilePath { get; set; }
}