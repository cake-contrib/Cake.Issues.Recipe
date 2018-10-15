/// <summary>
/// Support for Azure DevOps / Azure Pipelines builds.
/// </summary>
public static class AzureDevOpsBuildServerHelper
{
    /// <inheritdoc />
    public static void CreateSummaryIssuesReport(ICakeContext context, IssuesData data)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        if (data == null)
        {
            throw new ArgumentNullException(nameof(data));
        }

        var summaryFile = IssuesParameters.OutputDirectory.CombineWithFilePath("summary.md");

        // Create summary for Azure Pipelines using custom template.
        context.CreateIssueReport(
            data.Issues,
            context.GenericIssueReportFormatFromFilePath("AzurePipelineSummary.cshtml"),
            data.RepositoryRootDirectory,
            summaryFile);

        context.TFBuild().Commands.UploadTaskSummary(summaryFile);
    }

    /// <inheritdoc />
    public static void PublishIssuesArtifacts(ICakeContext context, IssuesData data)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        if (data == null)
        {
            throw new ArgumentNullException(nameof(data));
        }

        if (IssuesParameters.ShouldPublishFullIssuesReport &&
            data.FullIssuesReport != null &&
            context.FileExists(data.FullIssuesReport))
        {
            context.TFBuild().Commands.UploadArtifact("Issues", data.FullIssuesReport, "Issues");
        }
    }
}