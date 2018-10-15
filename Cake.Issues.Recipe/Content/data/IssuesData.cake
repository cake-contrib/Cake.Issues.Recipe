/// <summary>
/// Class for holding shared data between tasks.
/// </summary>
public class IssuesData
{
    private readonly List<IIssue> issues = new List<IIssue>();

    /// <summary>
    /// Gets the root directory of the repository.
    /// </summary>
    public DirectoryPath RepositoryRootDirectory { get; }

    /// <summary>
    /// Gets or sets the path to the full issues report.
    /// </summary>
    public FilePath FullIssuesReport { get; set; }

    /// <summary>
    /// Gets or sets the path to the summary issues report.
    /// </summary>
    public FilePath SummaryIssuesReport { get; set; }

    /// <summary>
    /// Gets the build server under which the build is running.
    /// </summary>
    public IssuesBuildServer BuildServer { get; }

    /// <summary>
    /// Gets URL of the remote repository.
    /// </summary>
    public Uri RepositoryUrl { get; }

    /// <summary>
    /// Gets the pull request system used for the code.
    /// </summary>
    public IssuesPullRequestSystem PullRequestSystem { get; }

    /// <summary>
    /// Gets a value indicating whether the build is for a pull request.
    /// </summary>
    public bool IsPullRequestBuild { get; }

    /// <summary>
    /// Gets ID of the pull request in case <see cref="IsPullRequestBuild"/> returns <c>true</c>.
    /// </summary>
    public int? PullRequestId { get; }

    /// <summary>
    /// Gets the list of reported issues.
    /// </summary>
    public IEnumerable<IIssue> Issues 
    { 
        get
        {
            return issues.AsReadOnly();
        } 
    }

    /// <summary>
    /// Creates a new instance of the <see cref="IssuesData"/> class.
    /// </summary>
    /// <param name="context">The Cake context.</param>
    public IssuesData(ICakeContext context)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        this.RepositoryRootDirectory = context.MakeAbsolute(context.Directory("./"));

        this.BuildServer = DetermineBuildServer(context);
        this.RepositoryUrl = DetermineRepositoryRemoteUrl(context, this.BuildServer, this.RepositoryRootDirectory);
        this.PullRequestSystem = DeterminePullRequestSystem(context, this.RepositoryUrl);
        this.IsPullRequestBuild = DetermineIfPullRequest(context, this.BuildServer);

        if (this.IsPullRequestBuild)
        {
            this.PullRequestId = DeterminePullRequestId(context, this.BuildServer);
        }
    }

    /// <summary>
    /// Adds an issue to the data class.
    /// </summary>
    /// <param name="issue">Issue which should be added.</param>
    public void AddIssue(IIssue issue)
    {
        if (issue == null)
        {
            throw new ArgumentNullException(nameof(issue));
        }

        this.issues.Add(issue);
    }

    /// <summary>
    /// Adds a list of issues to the data class.
    /// </summary>
    /// <param name="issues">Issues which should be added.</param>
    public void AddIssues(IEnumerable<IIssue> issues)
    {
        if (issues == null)
        {
            throw new ArgumentNullException(nameof(issues));
        }

        this.issues.AddRange(issues);
    }

    /// <summary>
    /// Determines the build server on which the build is running.
    /// </summary>
    /// <param name="context">The Cake context.</param>
    /// <returns>The build server on which the build is running.</returns>
    private static IssuesBuildServer DetermineBuildServer(ICakeContext context)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        // Could be simplified once https://github.com/cake-build/cake/issues/1684 / https://github.com/cake-build/cake/issues/1580 are fixed.
        if (!string.IsNullOrWhiteSpace(context.EnvironmentVariable("TF_BUILD")) &&
            !string.IsNullOrWhiteSpace(context.EnvironmentVariable("SYSTEM_COLLECTIONURI")) &&
            (
                new Uri(context.EnvironmentVariable("SYSTEM_COLLECTIONURI")).Host == "dev.azure.com" ||
                new Uri(context.EnvironmentVariable("SYSTEM_COLLECTIONURI")).Host.EndsWith("visualstudio.com")
            )) 
        {
            return IssuesBuildServer.AzureDevOps;
        }

        return IssuesBuildServer.Unknown;
    }

    /// <summary>
    /// Determines the repository remote url.
    /// </summary>
    /// <param name="context">The Cake context.</param>
    /// <param name="buildServer">The build server under which the build is running.</param>
    /// <param name="repositoryRootDirectory">The root directory of the repository.</param>
    /// <returns>The remote URL of the repository.</returns>
    private static Uri DetermineRepositoryRemoteUrl(ICakeContext context, IssuesBuildServer buildServer, DirectoryPath repositoryRootDirectory)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        if (repositoryRootDirectory == null)
        {
            throw new ArgumentNullException(nameof(repositoryRootDirectory));
        }

        switch (buildServer)
        {
            case IssuesBuildServer.AzureDevOps:
                return new Uri(context.EnvironmentVariable("BUILD_REPOSITORY_URI"));

            default:
                var currentBranch = context.GitBranchCurrent(repositoryRootDirectory);
                return new Uri(currentBranch.Remotes.Single(x => x.Name == "origin").Url);
        }
    }

    /// <summary>
    /// Determines the pull request system.
    /// </summary>
    /// <param name="context">The Cake context.</param>
    /// <param name="repositoryUrl">The URL of the remote repository.</param>
    /// <returns>The pull request system.</returns>
    private static IssuesPullRequestSystem DeterminePullRequestSystem(ICakeContext context, Uri repositoryUrl)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        if (repositoryUrl == null)
        {
            throw new ArgumentNullException(nameof(repositoryUrl));
        }

        if (repositoryUrl.Host == "dev.azure.com" || repositoryUrl.Host.EndsWith("visualstudio.com"))
        {
            return IssuesPullRequestSystem.AzureDevOps;
        }

        return IssuesPullRequestSystem.Unknown;
    }

    /// <summary>
    /// Determines whether the build is for a pull request.
    /// </summary>
    /// <param name="context">The Cake context.</param>
    /// <param name="buildServer">The build server under which the build is running.</param>
    /// <returns>A value indicating whether the build is for a pull request.</returns>
    private static bool DetermineIfPullRequest(ICakeContext context, IssuesBuildServer buildServer)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        switch (buildServer)
        {
            case IssuesBuildServer.AzureDevOps:
                // Could be simplified once https://github.com/cake-build/cake/issues/2149 is fixed
               return !string.IsNullOrWhiteSpace(context.EnvironmentVariable("SYSTEM_PULLREQUEST_PULLREQUESTID"));

            default:
                return false;
        }
    }

    /// <summary>
    /// Determines ID of the pull request.
    /// </summary>
    /// <param name="context">The Cake context.</param>
    /// <param name="buildServer">The build server under which the build is running.</param>
    /// <returns>ID of the pull request.</returns>
    private static int? DeterminePullRequestId(ICakeContext context, IssuesBuildServer buildServer)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        switch (buildServer)
        {
            case IssuesBuildServer.AzureDevOps:
                if (!Int32.TryParse(context.EnvironmentVariable("SYSTEM_PULLREQUEST_PULLREQUESTID"), out var pullRequestId))
                {
                    throw new Exception(string.Format("Invalid pull request ID: {0}", context.EnvironmentVariable("SYSTEM_PULLREQUEST_PULLREQUESTID")));
                }
                else
                {
                    return pullRequestId;
                }

            default:
                return null;
        }
   }
}