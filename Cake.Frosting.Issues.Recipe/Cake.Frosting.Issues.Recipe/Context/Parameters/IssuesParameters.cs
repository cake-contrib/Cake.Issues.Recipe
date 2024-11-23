namespace Cake.Frosting.Issues.Recipe;

/// <summary>
/// Parameters of the build.
/// </summary>
public class IssuesParameters : IssuesParameters<IssuesParametersInputFiles, IssuesParametersReporting, IssuesParametersBuildServer, IssuesParametersPullRequestSystem>
{
    /// <inheritdoc />
    protected override IssuesParametersBuildServer CreateBuildServerParameters() => new();

    /// <inheritdoc />
    protected override IssuesParametersInputFiles CreateInputFilesParameters() => new();

    /// <inheritdoc />
    protected override IssuesParametersPullRequestSystem CreatePullRequestSystemParameters() => new();

    /// <inheritdoc />
    protected override IssuesParametersReporting CreateReportingParameters() => new();
}