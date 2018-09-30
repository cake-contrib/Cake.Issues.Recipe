/// <summary>
/// Class for holding shared data between tasks.
/// </summary>
public class IssuesData
{
    private readonly List<IIssue> issues = new List<IIssue>();

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