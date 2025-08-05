namespace Cake.Frosting.Issues.Recipe;

using System;
using System.Linq;
using Cake.Common;
using Cake.Common.Diagnostics;
using Cake.GitHub;

/// <summary>
/// Support for GitHub hosted code.
/// </summary>
internal sealed class GitHubPullRequestSystem : BasePullRequestSystem
{
    /// <inheritdoc />
    public override void ReportIssuesToPullRequest(IIssuesContext context) => context.NotNull(); // Not supported yet

    /// <inheritdoc />
    public override void SetPullRequestIssuesState(IIssuesContext context)
    {
        context.NotNull();

        // Check for GitHub token in environment variables
        var githubToken = context.EnvironmentVariable("GITHUB_TOKEN");
        if (string.IsNullOrWhiteSpace(githubToken))
        {
            context.Warning("GITHUB_TOKEN environment variable not set. Cannot set GitHub status checks.");
            return;
        }

        var repositoryUrl = context.State.BuildServer.DetermineRepositoryRemoteUrl(context, context.State.RepositoryRootDirectory);
        var commitSha = context.State.BuildServer.DetermineCommitId(context, context.State.RepositoryRootDirectory);

        // Parse repository URL to get owner and repository name
        if (!TryParseGitHubUrl(repositoryUrl.ToString(), out var owner, out var repository))
        {
            context.Warning($"Could not parse GitHub repository URL: {repositoryUrl}");
            return;
        }

        var status = PullRequestStatusCalculator.GetPullRequestStates(context).ToList();
        if (status.Count > 0)
        {
            foreach (var item in status)
            {
                context.Information("Setting GitHub status {0} for commit {1} to {2}", item.Name, context.State.CommitId, item.State);

                _ = context.GitHubStatus(
                    null, // Use null for username when using access token
                    githubToken,
                    owner,
                    repository,
                    commitSha,
                    item.ToGitHubStatusSettings());
            }
        }
    }

    /// <inheritdoc />
    public override FileLinkSettings GetFileLinkSettings(IIssuesContext context)
    {
        context.NotNull();

        var rootPath = context.State.RepositoryRootDirectory.GetRelativePath(context.State.ProjectRootDirectory);

        return context.IssueFileLinkSettingsForGitHubCommit(
            context.State.RepositoryRemoteUrl,
            context.State.CommitId,
            rootPath.FullPath);
    }

    /// <summary>
    /// Parses a GitHub repository URL to extract owner and repository name.
    /// </summary>
    /// <param name="repositoryUrl">The repository URL to parse.</param>
    /// <param name="owner">The extracted owner/organization name.</param>
    /// <param name="repository">The extracted repository name.</param>
    /// <returns>True if parsing was successful, false otherwise.</returns>
    internal static bool TryParseGitHubUrl(string repositoryUrl, out string owner, out string repository)
    {
        owner = null;
        repository = null;

        if (string.IsNullOrWhiteSpace(repositoryUrl))
        {
            return false;
        }

        Uri uri;
        try
        {
            uri = new Uri(repositoryUrl);
        }
        catch (UriFormatException)
        {
            return false;
        }

        if (!uri.Host.Equals("github.com", StringComparison.OrdinalIgnoreCase))
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
        if (repository.EndsWith(".git", StringComparison.OrdinalIgnoreCase))
        {
            repository = repository[..^4];
        }

        return !string.IsNullOrWhiteSpace(owner) && !string.IsNullOrWhiteSpace(repository);
    }
}