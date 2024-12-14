namespace Cake.Frosting.Issues.Recipe;

/// <summary>
/// Description of parameters and state for the build run.
/// </summary>
public interface IIssuesContext : IFrostingContext
{
    /// <summary>
    /// Gets input parameters.
    /// </summary>
    IIssuesParameters Parameters { get; }

    /// <summary>
    /// Gets the mutable state of the build run.
    /// </summary>
    IIssuesState State { get; }
}
