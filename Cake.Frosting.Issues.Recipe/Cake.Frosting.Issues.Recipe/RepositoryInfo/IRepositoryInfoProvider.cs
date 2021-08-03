using Cake.Core;
using Cake.Core.IO;
using System;

namespace Cake.Frosting.Issues.Recipe
{
    /// <summary>
    /// Description of a provider to retrieve repository information.
    /// </summary>
    public interface IRepositoryInfoProvider
    {
        /// <summary>
        /// Returns the root directory of the current repository.
        /// </summary>
        /// <param name="context">The Cake context.</param>
        /// <param name="buildRootDirectory">Root directory of the build script.</param>
        /// <returns>The root directory of the current repository.</returns>
        DirectoryPath GetRepositoryRootDirectory(ICakeContext context, DirectoryPath buildRootDirectory);

        /// <summary>
        /// Returns the URL of the remote repository.
        /// </summary>
        /// <param name="context">The Cake context.</param>
        /// <param name="repositoryRootDirectory">Root directory of the repository.</param>
        /// <returns>The URL of the remote repository.</returns>
        Uri GetRepositoryRemoteUrl(ICakeContext context, DirectoryPath repositoryRootDirectory);

        /// <summary>
        /// Returns the SHA hash of the current commit.
        /// </summary>
        /// <param name="context">The Cake context.</param>
        /// <param name="repositoryRootDirectory">Root directory of the repository.</param>
        /// <returns>The SHA hash of the current commit</returns>
        string GetCommitId(ICakeContext context, DirectoryPath repositoryRootDirectory);
    }
}