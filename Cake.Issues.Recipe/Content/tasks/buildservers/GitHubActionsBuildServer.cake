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
}