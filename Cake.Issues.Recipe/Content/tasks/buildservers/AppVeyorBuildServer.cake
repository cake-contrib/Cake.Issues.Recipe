/// <summary>
/// Support for AppVeyor builds.
/// </summary>
public class AppVeyorBuildServer : BaseBuildServer
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

        switch(context.AppVeyor().Environment.Repository.Provider)
        {
            case "bitBucket": return new System.Uri($"https://bitbucket.org/{context.AppVeyor().Environment.Repository.Name}/src");
            case "gitHub": return new System.Uri($"https://github.com/{context.AppVeyor().Environment.Repository.Name}.git");
            case "gitLab": return new System.Uri($"https://gitlab.com/{context.AppVeyor().Environment.Repository.Name}.git");
            case "vso": return new System.Uri($"https://dev.azure.com/{context.AppVeyor().Environment.Repository.Name}");
            default: return new System.Uri(context.AppVeyor().Environment.Repository.Name);
        }
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

        return context.AppVeyor().Environment.Repository.Commit.Id;
    }

    /// <inheritdoc />
    public override bool DetermineIfPullRequest(ICakeContext context)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }
        
        return context.AppVeyor().Environment.PullRequest.IsPullRequest;
    }

    /// <inheritdoc />
    public override int? DeterminePullRequestId(ICakeContext context)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        return context.AppVeyor().Environment.PullRequest.Number;
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

        context.ReportIssuesToPullRequest(
            data.Issues,
            context.AppVeyorBuilds(),
            data.BuildRootDirectory);
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

        // Summary issues report is not supported for AppVeyor.
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
            context.AppVeyor().UploadArtifact(data.FullIssuesReport);
        }
    }
}