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
    /// Gets a value indicating whether the build is running on Azure DevOps.
    /// </summary>
    public bool IsRunningOnAzureDevOps { get; }

    /// <summary>
    /// Gets URL of the remote repository.
    /// </summary>
    public Uri RepositoryUrl { get; }

    /// <summary>
    /// Gets a value indicating whether the build is for a pull request.
    /// </summary>
    public bool IsPullRequestBuild { get; }

    /// <summary>
    /// Gets ID of the pull request in case <see cref="IsPullRequestBuild"/> returns <c>true</c>.
    /// </summary>
    public int PullRequestId { get; }

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

        // Could be simplified once https://github.com/cake-build/cake/issues/1684 / https://github.com/cake-build/cake/issues/1580 are fixed.
        this.IsRunningOnAzureDevOps =
            !string.IsNullOrWhiteSpace(context.EnvironmentVariable("TF_BUILD")) &&
            !string.IsNullOrWhiteSpace(context.EnvironmentVariable("SYSTEM_COLLECTIONURI")) &&
            (
                new Uri(context.EnvironmentVariable("SYSTEM_COLLECTIONURI")).Host == "dev.azure.com" ||
                new Uri(context.EnvironmentVariable("SYSTEM_COLLECTIONURI")).Host.EndsWith("visualstudio.com")
            );

        // Could be simplified once https://github.com/cake-build/cake/issues/2149 is fixed
        this.IsPullRequestBuild =
            !string.IsNullOrWhiteSpace(context.EnvironmentVariable("SYSTEM_PULLREQUEST_PULLREQUESTID"));

        if (this.IsRunningOnAzureDevOps)
        {
            this.RepositoryUrl = new Uri(context.EnvironmentVariable("BUILD_REPOSITORY_URI"));

            if (this.IsPullRequestBuild)
            {
                if (!Int32.TryParse(context.EnvironmentVariable("SYSTEM_PULLREQUEST_PULLREQUESTID"), out var pullRequestId))
                {
                    throw new Exception(string.Format("Invalid pull request ID: {0}", context.EnvironmentVariable("SYSTEM_PULLREQUEST_PULLREQUESTID")));
                }
                else
                {
                    this.PullRequestId = pullRequestId;
                }
            }
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
}