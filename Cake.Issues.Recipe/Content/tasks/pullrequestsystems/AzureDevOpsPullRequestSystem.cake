/// <summary>
/// Support for Azure DevOps / Azure Repository hosted code.
/// </summary>
public class AzureDevOpsPullRequestSystem : BasePullRequestSystem
{
    /// <inheritdoc />
    public override void ReportIssuesToPullRequest(ICakeContext context, IssuesData data)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        if (data == null)
        {
            throw new ArgumentNullException(nameof(data));
        }

        if (string.IsNullOrWhiteSpace(context.EnvironmentVariable("SYSTEM_ACCESSTOKEN")))
        {
            context.Warning("SYSTEM_ACCESSTOKEN environment variable not set. Make sure the 'Allow Scripts to access OAuth token' option is enabled on the build definition.");
            return;
        }

        context.ReportIssuesToPullRequest(
            data.Issues,
            context.TfsPullRequests(
                data.BuildServer.DetermineRepositoryRemoteUrl(context, data.RepositoryRootDirectory),
                data.BuildServer.DeterminePullRequestId(context).Value,
                context.TfsAuthenticationOAuth(context.EnvironmentVariable("SYSTEM_ACCESSTOKEN"))),
            data.RepositoryRootDirectory);
    }

    /// <inheritdoc />
    public override void SetPullRequestIssuesState(ICakeContext context, IssuesData data)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        if (data == null)
        {
            throw new ArgumentNullException(nameof(data));
        }

        if (string.IsNullOrWhiteSpace(context.EnvironmentVariable("SYSTEM_ACCESSTOKEN")))
        {
            context.Warning("SYSTEM_ACCESSTOKEN environment variable not set. Make sure the 'Allow Scripts to access OAuth token' option is enabled on the build definition.");
            return;
        }

        var pullRequestSettings =
            new TfsPullRequestSettings(
                data.BuildServer.DetermineRepositoryRemoteUrl(context, data.RepositoryRootDirectory),
                data.BuildServer.DeterminePullRequestId(context).Value,
                context.TfsAuthenticationOAuth(context.EnvironmentVariable("SYSTEM_ACCESSTOKEN")));

        var pullRequstStatus =
            new TfsPullRequestStatus("Issues")
            {
                Genre = "Cake.Issues.Recipe",
                State = data.Issues.Any() ? TfsPullRequestStatusState.Failed : TfsPullRequestStatusState.Succeeded,
                Description = data.Issues.Any() ? $"Found {data.Issues.Count()} issues" : "No issues found"
            };

        context.TfsSetPullRequestStatus(
            pullRequestSettings,
            pullRequstStatus);
        }
}