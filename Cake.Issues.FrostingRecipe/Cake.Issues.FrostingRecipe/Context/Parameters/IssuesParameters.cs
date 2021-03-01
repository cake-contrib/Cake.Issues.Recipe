using Cake.Core.IO;

namespace Cake.Issues.FrostingRecipe
{
    /// <summary>
    /// Class for configuring the script.
    /// </summary>
    public class IssuesParameters
    {
        /// <summary>
        /// Gets or sets the path to the output directory.
        /// A relative path will be relative to the current working directory.
        /// Default value is <c>BuildArtifacts</c>.
        /// </summary>
        public DirectoryPath OutputDirectory { get; set; } = "BuildArtifacts";

        /// <summary>
        /// Gets or sets a identifier for the build run.
        /// If set this identifier will be used to identify to artifacts provided by the
        /// build if building on multiple configurations.
        /// Default value is <c>string.Empty</c>.
        /// </summary>
        public string BuildIdentifier { get; set; } = string.Empty;

        /// <summary>
        /// Gets the parameters for the input files.
        /// </summary>
        public IssuesParametersInputFiles InputFiles { get; } = new IssuesParametersInputFiles();

        /// <summary>
        /// Gets the parameters for reporting.
        /// </summary>
        public IssuesParametersReporting Reporting { get; } = new IssuesParametersReporting();

        /// <summary>
        /// Gets the parameters for build server integration.
        /// </summary>
        public IssuesParametersBuildServer BuildServer { get; } = new IssuesParametersBuildServer();

        /// <summary>
        /// Gets the parameters for pull request integration.
        /// </summary>
        public IssuesParametersPullRequest PullRequest { get; } = new IssuesParametersPullRequest();
    }
}