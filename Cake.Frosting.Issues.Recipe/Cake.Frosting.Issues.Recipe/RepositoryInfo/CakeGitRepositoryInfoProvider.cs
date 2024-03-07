namespace Cake.Frosting.Issues.Recipe
{
    using Cake.Core;
    using Cake.Core.IO;
    using Cake.Git;
    using Cake.Issues;
    using System;
    using System.Linq;

    /// <summary>
    /// Provider to retrieve repository information using <see href="https://cakebuild.net/extensions/cake-git/">Cake.Git addin</see>.
    /// </summary>
    internal sealed class CakeGitRepositoryInfoProvider : IRepositoryInfoProvider
    {
        /// <inheritdoc />
        public DirectoryPath GetRepositoryRootDirectory(ICakeContext context, DirectoryPath buildRootDirectory)
        {
            context.NotNull(nameof(context));
            buildRootDirectory.NotNull(nameof(buildRootDirectory));

            return context.GitFindRootFromPath(buildRootDirectory);
        }

        /// <inheritdoc />
        public Uri GetRepositoryRemoteUrl(ICakeContext context, DirectoryPath repositoryRootDirectory)
        {
            context.NotNull(nameof(context));
            repositoryRootDirectory.NotNull(nameof(repositoryRootDirectory));

            var currentBranch = context.GitBranchCurrent(repositoryRootDirectory);
            return new Uri(currentBranch.Remotes.Single(x => x.Name == "origin").Url);
        }

        /// <inheritdoc />
        public string GetCommitId(ICakeContext context, DirectoryPath repositoryRootDirectory)
        {
            context.NotNull(nameof(context));
            repositoryRootDirectory.NotNull(nameof(repositoryRootDirectory));

            return context.GitLogTip(repositoryRootDirectory).Sha;
        }
    }
}