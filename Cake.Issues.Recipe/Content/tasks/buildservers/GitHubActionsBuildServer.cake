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
        context.NotNull();

        return new System.Uri($"https://github.com/{context.GitHubActions().Environment.Workflow.Repository}.git");
    }

    /// <inheritdoc />
    public override string DetermineCommitId(
        ICakeContext context,
        DirectoryPath repositoryRootDirectory)
    {
        context.NotNull();

        return context.GitHubActions().Environment.Workflow.Sha;
    }

    /// <inheritdoc />
    public override bool DetermineIfPullRequest(ICakeContext context)
    {
        context.NotNull();

        return context.GitHubActions().Environment.PullRequest.IsPullRequest;
    }

    /// <inheritdoc />
    public override int? DeterminePullRequestId(ICakeContext context)
    {
        context.NotNull();

        // Not supported by GitHub Actions
        return null;
   }

    /// <inheritdoc />
    public override void ReportIssuesToBuildServer(
        ICakeContext context,
        IssuesData data)
    {
        context.NotNull();
        data.NotNull();

        context.ReportIssuesToPullRequest(
            data.Issues,
            context.GitHubActionsBuilds(),
            data.ProjectRootDirectory);
    }

    /// <inheritdoc />
    public override void CreateSummaryIssuesReport(
        ICakeContext context,
        IssuesData data,
        [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "")
    {
        context.NotNull();
        data.NotNull();

        // Summary issues report is not supported for GitHub Actions.
    }

    /// <inheritdoc />
    public override void PublishIssuesArtifacts(ICakeContext context, IssuesData data)
    {
        context.NotNull();
        data.NotNull();

        if (IssuesParameters.BuildServer.ShouldPublishFullIssuesReport &&
            data.FullIssuesReport != null &&
            context.FileExists(data.FullIssuesReport))
        {
            // Set GitHub Actions output for full issues report
            var outputFile = context.EnvironmentVariable("GITHUB_OUTPUT");
            if (!string.IsNullOrEmpty(outputFile))
            {
                var outputContent = $"full-issues-report-path={data.FullIssuesReport.FullPath}";
                System.IO.File.AppendAllText(outputFile, outputContent + System.Environment.NewLine);
                context.Information($"Set GitHub Actions output: {outputContent}");
            }
            else
            {
                // Fallback to workflow command for older GitHub Actions runners
                context.Information($"::set-output name=full-issues-report-path::{data.FullIssuesReport.FullPath}");
            }
        }

        if (IssuesParameters.BuildServer.ShouldPublishSarifReport &&
            data.SarifReport != null &&
            context.FileExists(data.SarifReport))
        {
            // Set GitHub Actions output for SARIF report
            var outputFile = context.EnvironmentVariable("GITHUB_OUTPUT");
            if (!string.IsNullOrEmpty(outputFile))
            {
                var outputContent = $"sarif-report-path={data.SarifReport.FullPath}";
                System.IO.File.AppendAllText(outputFile, outputContent + System.Environment.NewLine);
                context.Information($"Set GitHub Actions output: {outputContent}");
            }
            else
            {
                // Fallback to workflow command for older GitHub Actions runners
                context.Information($"::set-output name=sarif-report-path::{data.SarifReport.FullPath}");
            }
        }
    }
}