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

        if (IssuesParameters.BuildServer.ShouldPublishSarifReport &&
            data.SarifReport != null &&
            context.FileExists(data.SarifReport))
        {
            UploadSarifToCodeScanning(context, data);
        }
    }

    private static void UploadSarifToCodeScanning(ICakeContext context, IssuesData data)
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
        var sarifContent = System.IO.File.ReadAllText(data.SarifReport.FullPath);
        var sarifBase64 = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(sarifContent));

        // Prepare the request
        var apiUrl = new System.Uri($"https://api.github.com/repos/{repository}/code-scanning/sarifs");
        var requestBody = new
        {
            commit_sha = commitSha,
            ref_ = ref_,
            sarif = sarifBase64,
            tool_name = "Cake.Issues.Recipe"
        };

        var json = Newtonsoft.Json.JsonConvert.SerializeObject(requestBody);

        // Make the API request
        using var httpClient = new System.Net.Http.HttpClient();
        httpClient.DefaultRequestHeaders.Add("Authorization", $"token {token}");
        httpClient.DefaultRequestHeaders.Add("Accept", "application/vnd.github.v3+json");
        httpClient.DefaultRequestHeaders.Add("User-Agent", "Cake.Issues.Recipe");

        using var content = new System.Net.Http.StringContent(json, System.Text.Encoding.UTF8, "application/json");
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

    private static bool IsCodeScanningEnabled(ICakeContext context, string repository, string token)
    {
        // Check if code scanning is enabled by attempting to fetch code scanning alerts
        var apiUrl = new System.Uri($"https://api.github.com/repos/{repository}/code-scanning/alerts?per_page=1");
        
        using var httpClient = new System.Net.Http.HttpClient();
        httpClient.DefaultRequestHeaders.Add("Authorization", $"token {token}");
        httpClient.DefaultRequestHeaders.Add("Accept", "application/vnd.github.v3+json");
        httpClient.DefaultRequestHeaders.Add("User-Agent", "Cake.Issues.Recipe");

        try
        {
            var response = httpClient.GetAsync(apiUrl).Result;
            
            // If we get a successful response (200) or even a 404 for no alerts, code scanning is enabled
            if (response.IsSuccessStatusCode || response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return true;
            }
            
            // If we get a 403 (Forbidden), check if it's because code scanning is not enabled
            if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
            {
                var errorContent = response.Content.ReadAsStringAsync().Result;
                if (errorContent.Contains("Code Security must be enabled"))
                {
                    return false;
                }
            }
            
            // For any other error, assume code scanning might be enabled but there's another issue
            // Log the issue but don't block the upload attempt
            context.Warning($"Unable to determine code scanning status. Response: {response.StatusCode}");
            return true;
        }
        catch (System.Exception ex)
        {
            // If there's an exception checking the status, assume code scanning might be enabled
            context.Warning($"Error checking code scanning status: {ex.Message}");
            return true;
        }
    }
}