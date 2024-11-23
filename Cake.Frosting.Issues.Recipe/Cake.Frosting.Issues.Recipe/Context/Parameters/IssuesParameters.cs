namespace Cake.Frosting.Issues.Recipe;

/// <summary>
/// Parameters of the build.
/// </summary>
public class IssuesParameters : IssuesParameters<IssuesParametersInputFiles, IssuesParametersReporting, IssuesParametersBuildServer, IssuesParametersPullRequestSystem>
{
    /// <inheritdoc />
    protected override IssuesParametersBuildServer CreateBuildServerParameters() => new IssuesParametersBuildServer();

    /// <inheritdoc />
    protected override IssuesParametersInputFiles CreateInputFilesParameters() => new IssuesParametersInputFiles();

    /// <inheritdoc />
    protected override IssuesParametersPullRequestSystem CreatePullRequestSystemParameters() => new IssuesParametersPullRequestSystem();

    /// <inheritdoc />
    protected override IssuesParametersReporting CreateReportingParameters() => new IssuesParametersReporting();
}