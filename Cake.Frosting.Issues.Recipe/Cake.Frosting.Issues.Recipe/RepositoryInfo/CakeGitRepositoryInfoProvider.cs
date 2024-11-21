namespace Cake.Frosting.Issues.Recipe
{
    using Cake.Core;
    using Cake.Core.IO;
    using Cake.Git;

    /// <summary>
    /// Provider to retrieve repository information using <see href="https://cakebuild.net/extensions/cake-git/">Cake.Git addin</see>.
    /// </summary>
    internal sealed class CakeGitRepositoryInfoProvider : IRepositoryInfoProvider
    {
        /// <inheritdoc />
        public DirectoryPath GetRepositoryRootDirectory(ICakeContext context, DirectoryPath buildRootDirectory)
        {
            context.NotNull();
            buildRootDirectory.NotNull();

            return context.GitFindRootFromPath(buildRootDirectory);
        }

        /// <inheritdoc />
        public Uri GetRepositoryRemoteUrl(ICakeContext context, DirectoryPath repositoryRootDirectory)
        {
            context.NotNull();
            repositoryRootDirectory.NotNull();

            var currentBranch = context.GitBranchCurrent(repositoryRootDirectory);
            return new Uri(currentBranch.Remotes.Single(x => x.Name == "origin").Url);
        }

        /// <inheritdoc />
        public string GetCommitId(ICakeContext context, DirectoryPath repositoryRootDirectory)
        {
            context.NotNull();
            repositoryRootDirectory.NotNull();

            return context.GitLogTip(repositoryRootDirectory).Sha;
        }
    }
}