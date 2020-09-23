/// <summary>
/// Support for builds running on GitHub Actions.
/// </summary>
public class GitHubActionsBuildServer : BaseBuildServer
{
    /// <inheritdoc />
    public override Uri DetermineRepositoryRemoteUrl(
        ICakeContext context,
        DirectoryPath repositoryRootDirectory)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        return new System.Uri($"https://github.com/{context.GitHubActions().Environment.Workflow.Repository}.git");
    }

    /// <inheritdoc />
    public override string DetermineCommitId(
        ICakeContext context,
        DirectoryPath repositoryRootDirectory)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        return context.GitHubActions().Environment.Workflow.Sha;
    }

    /// <inheritdoc />
    public override bool DetermineIfPullRequest(ICakeContext context)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        return context.GitHubActions().Environment.PullRequest.IsPullRequest;
    }

    /// <inheritdoc />
    public override int? DeterminePullRequestId(ICakeContext context)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        // Not supported by GitHub Actions
        return null;
   }

    /// <inheritdoc />
    public override void ReportIssuesToBuildServer(
        ICakeContext context,
        IssuesData data)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        if (data == null)
        {
            throw new ArgumentNullException(nameof(data));
        }

        foreach (var issue in data.Issues)
        {
            // Commands don't support line breaks, therefore we only use the first line of the message.
            var message =
                issue.MessageText
                    .Split(new[] {'\n'}, StringSplitOptions.RemoveEmptyEntries)
                    .FirstOrDefault()
                    ?.Trim();

            var rootDirectoryPath =
                data.RepositoryRootDirectory.GetRelativePath(data.BuildRootDirectory);

            context.Information($"::warning {FormatWarningOptions(rootDirectoryPath, issue.AffectedFileRelativePath, issue.Line, issue.Column)}::{message}");
        }
    }

    /// <inheritdoc />
    public override void CreateSummaryIssuesReport(
        ICakeContext context,
        IssuesData data,
        [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "")
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        if (data == null)
        {
            throw new ArgumentNullException(nameof(data));
        }

        // Summary issues report is not supported for GitHub Actions.
    }

    /// <inheritdoc />
    public override void PublishIssuesArtifacts(ICakeContext context, IssuesData data)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        if (data == null)
        {
            throw new ArgumentNullException(nameof(data));
        }

        // Publishing artifacts is currently not supported for GitHub Actions.
    }

    /// <summary>
    /// Formats the options for the warning service message.
    /// </summary>
    /// <param name="rootDirectoryPath">The root path of the file, relative to the repository root.</param>
    /// <param name="filePath">The file path relative to the project root.</param>
    /// <param name="line">The line where the issue ocurred.</param>
    /// <param name="column">The column where the issue ocurred.</param>
    /// <returns>Formatted options string for the warning service message.</returns>
    private static string FormatWarningOptions(DirectoryPath rootDirectoryPath, FilePath filePath, int? line, int? column)
    {
        var result = new List<string>();

        if (filePath != null)
        {
            result.Add($"file={rootDirectoryPath.CombineWithFilePath(filePath)}");
        }

        if (line.HasValue)
        {
            result.Add($"line={line.Value}");
        }

        if (column.HasValue)
        {
            result.Add($"col={column}");
        }

        return string.Join(",", result);
    }
}