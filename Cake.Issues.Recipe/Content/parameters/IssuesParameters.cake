/// <summary>
/// Class for configuring the script.
/// </summary>
public static class IssuesParameters
{
    /// <summary>
    /// Gets or sets the path to the output directory.
    /// A relative path will be relative to the current working directory.
    /// Default value is <c>BuildArtifacts</c>.
    /// </summary>
    public static DirectoryPath OutputDirectory { get; set; } = "BuildArtifacts";

    /// <summary>
    /// Gets or sets the path to the MSBuild log file created by XmlFileLogger.
    /// </summary>
    public static FilePath MsBuildXmlFileLoggerLogFilePath { get; set; }

    /// <summary>
    /// Gets or sets the path to the InspectCode log file.
    /// </summary>
    public static FilePath InspectCodeLogFilePath { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether full issues report should be created.
    /// Default value is <c>true</c>.
    /// </summary>
    public static bool ShouldCreateFullIssuesReport { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether full issues report should be published as artifact to the build system.
    /// Default value is <c>true</c>.
    /// </summary>
    public static bool ShouldPublishFullIssuesReport { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether summary issues report should be created.
    /// Default value is <c>true</c>.
    /// </summary>
    public static bool ShouldCreateSummaryIssuesReport { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether issues should be reported to the pull request system.
    /// Default value is <c>true</c>.
    /// </summary>
    public static bool ShouldReportIssuesToPullRequest { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether a status on the pull request should be set.
    /// Default value is <c>true</c>.
    /// </summary>
    public static bool ShouldSetPullRequestStatus { get; set; } = true;
}