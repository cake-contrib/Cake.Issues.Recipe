namespace Cake.Frosting.Issues.Recipe
{
    /// <summary>
    /// Represents a pull request status.
    /// </summary>
    /// <param name="Name">Name of the status entry.</param>
    /// <param name="Genre">Category of the status entry.</param>
    /// <param name="State">Sate of the status entry.</param>
    /// <param name="Description">Description of the status entry.</param>
    internal record PullRequestStatus(string Name, string Genre, PullRequestStatusState State, string Description);
}
