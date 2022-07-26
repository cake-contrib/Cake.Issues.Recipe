using Cake.Core.IO;
using Cake.Issues;
using System;
using System.Collections.Generic;

namespace Cake.Frosting.Issues.Recipe
{
    /// <summary>
    /// Description of the mutable state of the build run.
    /// </summary>    
    public interface IIssuesState
    {
        /// <summary>
        /// Gets the root directory of the repository.
        /// </summary>
        DirectoryPath RepositoryRootDirectory { get; }

        /// <summary>
        /// Gets the root directory of the build script.
        /// </summary>
        DirectoryPath BuildRootDirectory { get; }

        /// <summary>
        /// Gets or sets the root directory of the project.
        /// Default value is the parent directory of the <see cref="BuildRootDirectory"/>.
        /// </summary>
        DirectoryPath ProjectRootDirectory { get; set; }

        /// <summary>
        /// Gets the remote URL of the repository.
        /// </summary>
        Uri RepositoryRemoteUrl { get; }

        /// <summary>
        /// Gets the SHA ID of the current commit.
        /// </summary>
        string CommitId { get; }

        /// <summary>
        /// Gets or sets the path to the full issues report.
        /// </summary>
        FilePath FullIssuesReport { get; set; }

        /// <summary>
        /// Gets or sets the path to the summary issues report.
        /// </summary>
        FilePath SummaryIssuesReport { get; set; }

        /// <summary>
        /// Gets the provider to read information about the Git repository.
        /// </summary>
        IRepositoryInfoProvider RepositoryInfo { get; }

        /// <summary>
        /// Gets the build server under which the build is running.
        /// Returns <c>null</c> if running locally or on an unsupported build server.
        /// </summary>
        IIssuesBuildServer BuildServer { get; }

        /// <summary>
        /// Gets the pull request system used for the code.
        /// Returns <c>null</c> if not running a pull request build or on an unsupported build server.
        /// </summary>
        IIssuesPullRequestSystem PullRequestSystem { get; }

        /// <summary>
        /// Gets the list of reported issues.
        /// </summary>
        IEnumerable<IIssue> Issues { get; }

        /// <summary>
        /// Adds an issue to the data class.
        /// </summary>
        /// <param name="issue">Issue which should be added.</param>
        void AddIssue(IIssue issue);

        /// <summary>
        /// Adds a list of issues to the data class.
        /// </summary>
        /// <param name="issues">Issues which should be added.</param>
        void AddIssues(IEnumerable<IIssue> issues);
    }
}
