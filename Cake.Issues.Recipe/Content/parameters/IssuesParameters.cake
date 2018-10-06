/// <summary>
/// Class for configuring the script.
/// </summary>
public static class IssuesParameters
{
    /// <summary>
    /// Gets or sets the path to the MSBuild log file created by XmlFileLogger.
    /// </summary>
    public static FilePath MsBuildXmlFileLoggerLogFilePath { get; set; }

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