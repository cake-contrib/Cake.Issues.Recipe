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
    /// Gets the parameters for the input files.
    /// </summary>
    public static IssuesParametersInputFiles InputFiles { get; } = new IssuesParametersInputFiles();

    /// <summary>
    /// Gets the parameters for reporting.
    /// </summary>
    public static IssuesParametersReporting Reporting { get; } = new IssuesParametersReporting();

    /// <summary>
    /// Gets the parameters for build server integration.
    /// </summary>
    public static IssuesParametersBuildServer BuildServer { get; } = new IssuesParametersBuildServer();

    /// <summary>
    /// Gets the parameters for pull request system integration.
    /// </summary>
    public static IssuesParametersPullRequestSystem PullRequestSystem { get; } = new IssuesParametersPullRequestSystem();
}