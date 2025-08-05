/// <summary>
/// Support for GitHub hosted code.
/// </summary>
public class GitHubPullRequestSystem : BasePullRequestSystem
{
    /// <inheritdoc />
    public override void ReportIssuesToPullRequest(ICakeContext context, IssuesData data)
    {
        context.NotNull();
        data.NotNull();

        // Not supported yet
    }

    /// <inheritdoc />
    public override void SetPullRequestIssuesState(ICakeContext context, IssuesData data)
    {
        context.NotNull();
        data.NotNull();

        // Check for GitHub token in environment variables
        var githubToken = context.EnvironmentVariable("GITHUB_TOKEN");
        if (string.IsNullOrWhiteSpace(githubToken))
        {
            context.Warning("GITHUB_TOKEN environment variable not set. Cannot set GitHub status checks.");
            return;
        }

        // Parse repository URL to get owner and repository name
        if (!TryParseGitHubUrl(data.RepositoryRemoteUrl.ToString(), out var owner, out var repository))
        {
            context.Warning($"Could not parse GitHub repository URL: {data.RepositoryRemoteUrl}");
            return;
        }

        var status = PullRequestStatusCalculator.GetPullRequestStates(data).ToList();
        if (status.Count > 0)
        {
            foreach (var item in status)
            {
                context.Information("Setting GitHub status {0} for commit {1} to {2}", item.Name, data.CommitId, item.State);

                context.GitHubStatus(
                    null, // Use null for username when using access token
                    githubToken,
                    owner,
                    repository,
                    data.CommitId,
                    PullRequestStatusExtensions.ToGitHubStatusSettings(item));
            }
        }
    }

    /// <inheritdoc />
    public override FileLinkSettings GetFileLinkSettings(ICakeContext context, IssuesData data)
    {
        context.NotNull();
        data.NotNull();

        var rootPath = data.RepositoryRootDirectory.GetRelativePath(data.ProjectRootDirectory);

        return context.IssueFileLinkSettingsForGitHubCommit(
            data.RepositoryRemoteUrl,
            data.CommitId,
            rootPath.FullPath);
    }

    /// <summary>
    /// Parses a GitHub repository URL to extract owner and repository name.
    /// </summary>
    /// <param name="repositoryUrl">The repository URL to parse.</param>
    /// <param name="owner">The extracted owner/organization name.</param>
    /// <param name="repository">The extracted repository name.</param>
    /// <returns>True if parsing was successful, false otherwise.</returns>
    private static bool TryParseGitHubUrl(string repositoryUrl, out string owner, out string repository)
    {
        owner = null;
        repository = null;

        if (string.IsNullOrWhiteSpace(repositoryUrl))
        {
            return false;
        }

        try
        {
            var uri = new System.Uri(repositoryUrl);
            if (!uri.Host.Equals("github.com", System.StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            var pathSegments = uri.AbsolutePath.Trim('/').Split('/');
            if (pathSegments.Length < 2)
            {
                return false;
            }

            owner = pathSegments[0];
            repository = pathSegments[1];

            // Remove .git suffix if present
            if (repository.EndsWith(".git", System.StringComparison.OrdinalIgnoreCase))
            {
                repository = repository.Substring(0, repository.Length - 4);
            }

            return !string.IsNullOrWhiteSpace(owner) && !string.IsNullOrWhiteSpace(repository);
        }
        catch
        {
            return false;
        }
    }
}