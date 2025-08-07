namespace Cake.Frosting.Issues.Recipe;

using Cake.Common;
using Cake.Common.Build;
using Cake.Common.Diagnostics;
using Cake.Common.IO;
using Cake.Core.IO;
using System.Net;
using System.Net.Http;

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

        // For pull request events, use the actual HEAD commit instead of the merge commit SHA
        if (this.DetermineIfPullRequest(context))
        {
            var eventPath = context.EnvironmentVariable("GITHUB_EVENT_PATH");

            if (!string.IsNullOrWhiteSpace(eventPath) && System.IO.File.Exists(eventPath))
            {
                try
                {
                    var eventJson = System.IO.File.ReadAllText(eventPath);
                    var eventData = Newtonsoft.Json.JsonConvert.DeserializeObject(eventJson) as Newtonsoft.Json.Linq.JObject;
                    var prHeadSha = eventData?["pull_request"]?["head"]?["sha"];

                    if (prHeadSha != null)
                    {
                        return prHeadSha.ToString();
                    }
                }
                catch (System.IO.IOException)
                {
                    // Fall through to default behavior if file I/O fails
                }
                catch (System.UnauthorizedAccessException)
                {
                    // Fall through to default behavior if access is denied
                }
                catch (Newtonsoft.Json.JsonException)
                {
                    // Fall through to default behavior if JSON parsing fails
                }
            }
        }

        // Default behavior for non-PR events or when event data is not available
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

        if (context.Parameters.BuildServer.ShouldPublishSarifReport &&
            context.State.SarifReport != null &&
            context.FileExists(context.State.SarifReport))
        {
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

        // Check if code scanning is enabled before attempting upload
        if (!IsCodeScanningEnabled(context, repository, token))
        {
            context.Information("GitHub code scanning is not enabled for this repository. Skipping SARIF upload.");
            return;
        }

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

    private static bool IsCodeScanningEnabled(IIssuesContext context, string repository, string token)
    {
        // Check if code scanning is enabled by attempting to fetch code scanning alerts
        var apiUrl = new Uri($"https://api.github.com/repos/{repository}/code-scanning/alerts?per_page=1");

        using var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Add("Authorization", $"token {token}");
        httpClient.DefaultRequestHeaders.Add("Accept", "application/vnd.github.v3+json");
        httpClient.DefaultRequestHeaders.Add("User-Agent", "Cake.Issues.Recipe");

        try
        {
            var response = httpClient.GetAsync(apiUrl).Result;

            // If we get a successful response (200) or even a 404 for no alerts, code scanning is enabled
            if (response.IsSuccessStatusCode || response.StatusCode == HttpStatusCode.NotFound)
            {
                return true;
            }

            // If we get a 403 (Forbidden), check if it's because code scanning is not enabled
            if (response.StatusCode == HttpStatusCode.Forbidden)
            {
                var errorContent = response.Content.ReadAsStringAsync().Result;
                if (errorContent.Contains("Code Security must be enabled", StringComparison.Ordinal))
                {
                    return false;
                }
            }

            // For any other error, assume code scanning might be enabled but there's another issue
            // Log the issue but don't block the upload attempt
            context.Warning($"Unable to determine code scanning status. Response: {response.StatusCode}");
            return true;
        }
        catch (HttpRequestException ex)
        {
            // If there's an HTTP exception checking the status, assume code scanning might be enabled
            context.Warning($"HTTP error checking code scanning status: {ex.Message}");
            return true;
        }
        catch (TaskCanceledException ex)
        {
            // If there's a timeout checking the status, assume code scanning might be enabled
            context.Warning($"Timeout checking code scanning status: {ex.Message}");
            return true;
        }
#pragma warning disable CA1031 // Do not catch general exception types - intentional fail-safe behavior
        catch (Exception ex)
        {
            // If there's any other exception checking the status, assume code scanning might be enabled
            context.Warning($"Error checking code scanning status: {ex.Message}");
            return true;
        }
#pragma warning restore CA1031 // Do not catch general exception types
    }
}