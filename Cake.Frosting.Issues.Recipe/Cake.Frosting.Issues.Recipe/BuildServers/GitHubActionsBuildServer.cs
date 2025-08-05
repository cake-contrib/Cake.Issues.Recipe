namespace Cake.Frosting.Issues.Recipe;

using System;
using System.IO;
using System.Net.Http;
using Cake.Common;
using Cake.Common.Build;
using Cake.Common.Diagnostics;
using Cake.Common.IO;
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
            
            UploadSarifToCodeScanning(context);
        }
    }

    private static void UploadSarifToCodeScanning(IIssuesContext context)
    {
        var token = context.EnvironmentVariable("GITHUB_TOKEN");
        if (string.IsNullOrWhiteSpace(token))
        {
            context.Warning("GITHUB_TOKEN environment variable is not set. Skipping SARIF upload to GitHub code scanning.");
            return;
        }

        var repository = context.GitHubActions().Environment.Workflow.Repository;
        var commitSha = context.GitHubActions().Environment.Workflow.Sha;
        var ref_ = context.GitHubActions().Environment.Workflow.Ref;

        // Read and encode SARIF file
        var sarifContent = File.ReadAllText(context.State.SarifReport.FullPath);
        var sarifBase64 = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(sarifContent));

        // Prepare the request
        var apiUrl = new Uri($"https://api.github.com/repos/{repository}/code-scanning/sarifs");
        var requestBody = new
        {
            commit_sha = commitSha,
            ref_ = ref_,
            sarif = sarifBase64,
            tool_name = "Cake.Issues.Recipe"
        };

        var json = Newtonsoft.Json.JsonConvert.SerializeObject(requestBody);

        // Make the API request
        using var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Add("Authorization", $"token {token}");
        httpClient.DefaultRequestHeaders.Add("Accept", "application/vnd.github.v3+json");
        httpClient.DefaultRequestHeaders.Add("User-Agent", "Cake.Issues.Recipe");

        using var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
        var response = httpClient.PostAsync(apiUrl, content).Result;

        if (response.IsSuccessStatusCode)
        {
            context.Information("Successfully uploaded SARIF report to GitHub code scanning.");
        }
        else
        {
            var errorContent = response.Content.ReadAsStringAsync().Result;
            context.Warning($"Failed to upload SARIF report to GitHub code scanning. Status: {response.StatusCode}, Error: {errorContent}");
        }
    }
}