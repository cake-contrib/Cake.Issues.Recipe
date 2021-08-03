namespace Cake.Frosting.Issues.Recipe
{
    /// <summary>
    /// Supported ways to read repository information.
    /// </summary>
    public enum RepositoryInfoProviderType
    {
        /// <summary>
        /// Read repository information using Cake.Git addin.
        /// Requires system to be compatible with Cake.Git addin.
        /// </summary>
        CakeGit,

        /// <summary>
        /// Read repository information using Git CLI.
        /// Requires Git CLI to be available in path.
        /// </summary>
        Cli
    }
}