/// <summary>
/// Support for Azure DevOps / Azure Pipelines builds.
/// </summary>
public class AzureDevOpsBuildServer : BaseBuildServer
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

        return new Uri(context.EnvironmentVariable("BUILD_REPOSITORY_URI"));
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

        return context.AzurePipelines().Environment.Repository.SourceVersion;
    }

    /// <inheritdoc />
    public override bool DetermineIfPullRequest(ICakeContext context)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }
        
        // Could be simplified once https://github.com/cake-build/cake/issues/2149 is fixed
        return !string.IsNullOrWhiteSpace(context.EnvironmentVariable("SYSTEM_PULLREQUEST_PULLREQUESTID"));
    }

    /// <inheritdoc />
    public override int? DeterminePullRequestId(ICakeContext context)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        if (!Int32.TryParse(context.EnvironmentVariable("SYSTEM_PULLREQUEST_PULLREQUESTID"), out var pullRequestId))
        {
            throw new Exception(string.Format("Invalid pull request ID: {0}", context.EnvironmentVariable("SYSTEM_PULLREQUEST_PULLREQUESTID")));
        }
        else
        {
            return pullRequestId;
        }
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
            context.AzurePipelines().Commands.WriteWarning(
                issue.MessageText,
                new AzurePipelinesMessageData
                {
                    SourcePath = issue.AffectedFileRelativePath?.FullPath,
                    LineNumber = issue.Line
                });
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

        var summaryFileName = "summary";
        if (!string.IsNullOrWhiteSpace(IssuesParameters.BuildIdentifier))
        {
            summaryFileName += $"-{IssuesParameters.BuildIdentifier}";
        }
        summaryFileName += ".md";
        var summaryFilePath = IssuesParameters.OutputDirectory.CombineWithFilePath(summaryFileName);

        // Create summary for Azure Pipelines using custom template.
        context.CreateIssueReport(
            data.Issues,
            context.GenericIssueReportFormatFromFilePath(
                new FilePath(sourceFilePath).GetDirectory().Combine("tasks").Combine("buildservers").CombineWithFilePath("AzurePipelineSummary.cshtml")),
            data.BuildRootDirectory,
            summaryFilePath);

        context.AzurePipelines().Commands.UploadTaskSummary(summaryFilePath);
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

        if (IssuesParameters.BuildServer.ShouldPublishFullIssuesReport &&
            data.FullIssuesReport != null &&
            context.FileExists(data.FullIssuesReport))
        {
            context.AzurePipelines().Commands.UploadArtifact("Issues", data.FullIssuesReport, "Issues");
        }
    }
}