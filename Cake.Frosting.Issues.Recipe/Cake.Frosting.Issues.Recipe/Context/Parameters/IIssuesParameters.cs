using Cake.Core.IO;

namespace Cake.Frosting.Issues.Recipe
{
    /// <summary>
    /// Description of parameters of the build.
    /// </summary>    
    public interface IIssuesParameters
    {
        /// <summary>
        /// Gets or sets the path to the output directory.
        /// A relative path will be relative to the current working directory.
        /// Default value is <c>BuildArtifacts</c>.
        /// </summary>
        DirectoryPath OutputDirectory { get; set; }

        /// <summary>
        /// Gets or sets a identifier for the build run.
        /// If set this identifier will be used to identify to artifacts provided by the
        /// build if building on multiple configurations.
        /// Default value is <c>string.Empty</c>.
        /// </summary>
        string BuildIdentifier { get; set; }

        /// <summary>
        /// Gets the parameters for the input files.
        /// </summary>
        IIssuesParametersInputFiles InputFiles { get; }

        /// <summary>
        /// Gets the parameters for reporting.
        /// </summary>
        IIssuesParametersReporting Reporting { get; }

        /// <summary>
        /// Gets the parameters for build server integration.
        /// </summary>
        IIssuesParametersBuildServer BuildServer { get; }

        /// <summary>
        /// Gets the parameters for pull request system integration.
        /// </summary>
        IIssuesParametersPullRequestSystem PullRequestSystem { get; }
    }
}
