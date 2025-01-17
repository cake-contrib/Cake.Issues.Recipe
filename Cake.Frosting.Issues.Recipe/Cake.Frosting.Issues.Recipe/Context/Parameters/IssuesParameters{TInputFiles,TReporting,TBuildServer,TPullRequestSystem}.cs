namespace Cake.Frosting.Issues.Recipe;

using Cake.Core.IO;

/// <summary>
/// Parameters of the build.
/// </summary>
/// <typeparam name="TInputFiles">Type of input file parameters.</typeparam>
/// <typeparam name="TReporting">Type of reporting parameters.</typeparam>
/// <typeparam name="TBuildServer">Type of parameters for build server integration.</typeparam>
/// <typeparam name="TPullRequestSystem">Type of parameters for pull request system integration</typeparam>
/// <typeparam name="TBuildBreaking">Type of parameters for build breaking.</typeparam>
public abstract class IssuesParameters<TInputFiles, TReporting, TBuildServer, TPullRequestSystem, TBuildBreaking> : IIssuesParameters
    where TInputFiles : IIssuesParametersInputFiles
    where TReporting : IIssuesParametersReporting
    where TBuildServer : IIssuesParametersBuildServer
    where TPullRequestSystem : IIssuesParametersPullRequestSystem
    where TBuildBreaking : IIssuesParametersBuildBreaking
{
    private readonly Lazy<TInputFiles> inputFiles;
    private readonly Lazy<TReporting> reporting;
    private readonly Lazy<TBuildServer> buildServer;
    private readonly Lazy<TPullRequestSystem> pullRequestSystem;
    private readonly Lazy<TBuildBreaking> buildBreaking;

    /// <inheritdoc />
    public DirectoryPath OutputDirectory { get; set; } = "BuildArtifacts";

    /// <inheritdoc />
    public string BuildIdentifier { get; set; } = string.Empty;

    /// <inheritdoc />
    public TInputFiles InputFiles => this.inputFiles.Value;

    /// <inheritdoc />
    public TReporting Reporting => this.reporting.Value;

    /// <inheritdoc />
    public TBuildServer BuildServer => this.buildServer.Value;

    /// <inheritdoc />
    public TPullRequestSystem PullRequestSystem => this.pullRequestSystem.Value;

    /// <inheritdoc />
    public TBuildBreaking BuildBreaking => this.buildBreaking.Value;

    IIssuesParametersInputFiles IIssuesParameters.InputFiles => this.InputFiles;

    IIssuesParametersReporting IIssuesParameters.Reporting => this.Reporting;

    IIssuesParametersBuildServer IIssuesParameters.BuildServer => this.BuildServer;

    IIssuesParametersPullRequestSystem IIssuesParameters.PullRequestSystem => this.PullRequestSystem;

    IIssuesParametersBuildBreaking IIssuesParameters.BuildBreaking => this.BuildBreaking;

    /// <summary>
    /// Creates a new instance of the <see cref="IssuesParameters"/> class.
    /// </summary>
    protected IssuesParameters()
    {
        this.inputFiles = new Lazy<TInputFiles>(this.CreateInputFilesParameters);
        this.reporting = new Lazy<TReporting>(this.CreateReportingParameters);
        this.buildServer = new Lazy<TBuildServer>(this.CreateBuildServerParameters);
        this.pullRequestSystem = new Lazy<TPullRequestSystem>(this.CreatePullRequestSystemParameters);
        this.buildBreaking = new Lazy<TBuildBreaking>(this.CreateBuildBreakingParameters);
    }

    /// <summary>
    /// Factory method to instantiate <see cref="InputFiles"/>.
    /// </summary>
    /// <returns>Parameters object for the input files.</returns>
    protected abstract TInputFiles CreateInputFilesParameters();

    /// <summary>
    /// Factory method to instantiate <see cref="Reporting"/>.
    /// </summary>
    /// <returns>Parameters object for reporting.</returns>
    protected abstract TReporting CreateReportingParameters();

    /// <summary>
    /// Factory method to instantiate <see cref="BuildServer"/>.
    /// </summary>
    /// <returns>Parameters object for build server integration.</returns>
    protected abstract TBuildServer CreateBuildServerParameters();

    /// <summary>
    /// Factory method to instantiate <see cref="PullRequestSystem"/>.
    /// </summary>
    /// <returns>Parameters object pull request system integration.</returns>
    protected abstract TPullRequestSystem CreatePullRequestSystemParameters();

    /// <summary>
    /// Factory method to instantiate <see cref="buildBreaking"/>.
    /// </summary>
    /// <returns>Parameters object for build breaking settings.</returns>
    protected abstract TBuildBreaking CreateBuildBreakingParameters();
}