using Cake.Core; 

namespace Cake.Frosting.Issues.Recipe
{
    /// <summary>
    /// Parameters and state for the build run.
    /// </summary>
    public class IssuesContext : IssuesContext<IssuesParameters, IssuesState>
    {
        private readonly RepositoryInfoProviderType repositoryInfoProviderType;

        /// <summary>
        /// Creates a new instance of the <see cref="IssuesContext"/> class.
        /// </summary>
        /// <param name="context">The Cake context.</param>
        /// <param name="repositoryInfoProviderType">Defines how information about the Git repository should be determined.</param>
        public IssuesContext(
            ICakeContext context,
            RepositoryInfoProviderType repositoryInfoProviderType)
            : base(context)
        {
            this.repositoryInfoProviderType = repositoryInfoProviderType;
        }

        /// <inheritdoc />
        protected override IssuesParameters CreateIssuesParameters()
        {
            return new IssuesParameters();
        }

        /// <inheritdoc />
        protected override IssuesState CreateIssuesState()
        {
            return new IssuesState(this, this.repositoryInfoProviderType);
        }
    }
}