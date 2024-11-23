namespace Cake.Frosting.Issues.Recipe;

/// <summary>
/// Parameters of the build.
/// </summary>
public class IssuesParameters : IssuesParameters<IssuesParametersInputFiles, IssuesParametersReporting, IssuesParametersBuildServer, IssuesParametersPullRequestSystem>
{
    /// <inheritdoc />
    protected override IssuesParametersBuildServer CreateBuildServerParameters()
    {
        return new IssuesParametersBuildServer();
    }

    /// <inheritdoc />
    protected override IssuesParametersInputFiles CreateInputFilesParameters()
    {
        return new IssuesParametersInputFiles();
    }

    /// <inheritdoc />
    protected override IssuesParametersPullRequestSystem CreatePullRequestSystemParameters()
    {
        return new IssuesParametersPullRequestSystem();
    }

    /// <inheritdoc />
    protected override IssuesParametersReporting CreateReportingParameters()
    {
        return new IssuesParametersReporting();
    }
}