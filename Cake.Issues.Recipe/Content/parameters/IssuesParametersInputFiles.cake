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
    /// Gets or sets the path to the InspectCode log file.
    /// </summary>
    public FilePath InspectCodeLogFilePath { get; set; }
}