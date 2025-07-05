namespace Cake.Frosting.Issues.Recipe;

using System;
using Cake.Common.Build;
using Cake.Core.IO;

/// <summary>
/// Support for builds running on GitHub Actions.
/// </summary>
internal sealed class GitHubActionsBuildServer : BaseBuildServer
{
    /// <inheritdoc />
    public override Uri DetermineRepositoryRemoteUrl(
        IIssuesContext context,
        DirectoryPath repositoryRootDirectory)
    {
        context.NotNull();

        return new Uri($"https://github.com/{context.GitHubActions().Environment.Workflow.Repository}.git");
    }

    /// <inheritdoc />
    public override string DetermineCommitId(
        IIssuesContext context,
        DirectoryPath repositoryRootDirectory)
    {
        context.NotNull();

        return context.GitHubActions().Environment.Workflow.Sha;
    }

    /// <inheritdoc />
    public override bool DetermineIfPullRequest(IIssuesContext context)
    {
        context.NotNull();

        return context.GitHubActions().Environment.PullRequest.IsPullRequest;
    }

    /// <inheritdoc />
    public override int? DeterminePullRequestId(IIssuesContext context)
    {
        context.NotNull();

        // Not supported by GitHub Actions
        return null;
    }

    /// <inheritdoc />
    public override void ReportIssuesToBuildServer(
        IIssuesContext context)
    {
        context.NotNull();

        context.ReportIssuesToPullRequest(
            context.State.Issues,
            context.GitHubActionsBuilds(),
            context.State.ProjectRootDirectory);
    }

    /// <inheritdoc />
    public override void CreateSummaryIssuesReport(
        IIssuesContext context,
        [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "") =>
            context.NotNull(); // Summary issues report is not supported for GitHub Actions.

    /// <inheritdoc />
    public override void PublishIssuesArtifacts(IIssuesContext context)
    {
        context.NotNull();

        if (context.Parameters.BuildServer.ShouldPublishFullIssuesReport &&
            context.State.FullIssuesReport != null &&
            context.FileExists(context.State.FullIssuesReport))
        {
            // Set GitHub Actions output for full issues report
            var outputFile = context.EnvironmentVariable("GITHUB_OUTPUT");
            if (!string.IsNullOrEmpty(outputFile))
            {
                var outputContent = $"full-issues-report-path={context.State.FullIssuesReport.FullPath}";
                System.IO.File.AppendAllText(outputFile, outputContent + Environment.NewLine);
                context.Information($"Set GitHub Actions output: {outputContent}");
            }
            else
            {
                // Fallback to workflow command for older GitHub Actions runners
                context.Information($"::set-output name=full-issues-report-path::{context.State.FullIssuesReport.FullPath}");
            }
        }

        if (context.Parameters.BuildServer.ShouldPublishSarifReport &&
            context.State.SarifReport != null &&
            context.FileExists(context.State.SarifReport))
        {
            // Set GitHub Actions output for SARIF report
            var outputFile = context.EnvironmentVariable("GITHUB_OUTPUT");
            if (!string.IsNullOrEmpty(outputFile))
            {
                var outputContent = $"sarif-report-path={context.State.SarifReport.FullPath}";
                System.IO.File.AppendAllText(outputFile, outputContent + Environment.NewLine);
                context.Information($"Set GitHub Actions output: {outputContent}");
            }
            else
            {
                // Fallback to workflow command for older GitHub Actions runners
                context.Information($"::set-output name=sarif-report-path::{context.State.SarifReport.FullPath}");
            }
        }
    }
}