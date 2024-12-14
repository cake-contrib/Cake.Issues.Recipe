namespace Cake.Frosting.Issues.Recipe;

using Cake.Core;

/// <summary>
/// Parameters and state for the build run.
/// </summary>
/// <param name="context">The Cake context.</param>
/// <param name="repositoryInfoProviderType">Defines how information about the Git repository should be determined.</param>
public class IssuesContext(
    ICakeContext context,
    RepositoryInfoProviderType repositoryInfoProviderType) : IssuesContext<IssuesParameters, IssuesState>(context)
{
    private readonly RepositoryInfoProviderType repositoryInfoProviderType = repositoryInfoProviderType;

    /// <inheritdoc />
    protected override IssuesParameters CreateIssuesParameters() => new();

    /// <inheritdoc />
    protected override IssuesState CreateIssuesState() => new(this, this.repositoryInfoProviderType);
}