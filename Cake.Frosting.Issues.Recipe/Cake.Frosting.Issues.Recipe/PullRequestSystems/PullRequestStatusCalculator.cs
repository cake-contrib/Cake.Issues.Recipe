namespace Cake.Frosting.Issues.Recipe;

using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Class to calculate the status which should be reported to the pull request.
/// </summary>
internal class PullRequestStatusCalculator
{
    /// <summary>
    /// Returns the status which should be reported to the pull request.
    /// </summary>
    /// <param name="context">Context of the build.</param>
    /// <returns>Status which should be reported to the pull request.</returns>
    public static IEnumerable<PullRequestStatus> GetPullRequestStates(IIssuesContext context)
    {
        return GetPullRequestStates(
            context.State.IssueProvidersAndRuns,
            context.State.Issues,
            context.Parameters.PullRequestSystem.ShouldSetPullRequestStatus,
            context.Parameters.PullRequestSystem.ShouldSetSeparatePullRequestStatusForEachIssueProviderAndRun,
            context.Parameters.BuildIdentifier);
    }

    /// <summary>
    /// Returns the status which should be reported to the pull request.
    /// </summary>
    /// <param name="issueProvidersAndRuns">List of issue providers and runs.</param>
    /// <param name="issues">List of issues.</param>
    /// <param name="shouldSetPullRequestStatus">Value which indicates whether overall status for all issues should
    /// be reported.</param>
    /// <param name="shouldSetSeparatePullRequestStatusForEachIssueProviderAndRun">Value which indicates whether
    /// individual status for each issue provder and run should be reported.</param>
    /// <param name="buildIdentifier">Identifier of the build run.</param>
    /// <returns>Status which should be reported to the pull request.</returns>
    public static IEnumerable<PullRequestStatus> GetPullRequestStates(
        IList<(IIssueProvider, string)> issueProvidersAndRuns,
        IEnumerable<IIssue> issues,
        bool shouldSetPullRequestStatus,
        bool shouldSetSeparatePullRequestStatusForEachIssueProviderAndRun,
        string buildIdentifier)
    {
        var result = new List<PullRequestStatus>();

        // Set status across all issues
        if (shouldSetPullRequestStatus)
        {
            result.Add(PullRequestStatusCalculator.CreatePullRequestState(issues, null, buildIdentifier));
        }

        // Set status for individual issue providers
        if (shouldSetSeparatePullRequestStatusForEachIssueProviderAndRun)
        {
            // Determine issue providers and runs from the list of issues providers from which issues were read
            // and issue providers and runs of the reported issues.
            var comparer = new IssueProviderAndRunComparer();
            var pullrequestStateIssues =
                issues
                    .GroupBy(x =>
                        new
                        {
                            x.ProviderType,
                            x.ProviderName,
                            x.Run
                        })
                    .Select(x =>
                        new PullRequestStateIssue(
                            x.Key.ProviderType,
                            x.Key.ProviderName,
                            x.Key.Run))
                .Union(
                    issueProvidersAndRuns
                    .Select(x =>
                        new PullRequestStateIssue(
                            x.Item1.ProviderType,
                            x.Item1.ProviderName,
                            x.Item2)), comparer);

            foreach (var item in pullrequestStateIssues)
            {
                var issueProvider = item.ProviderName;
                if (!string.IsNullOrEmpty(item.Run))
                {
                    issueProvider += $" ({item.Run})";
                }

                result.Add(
                    PullRequestStatusCalculator.CreatePullRequestState(
                        issues.Where(x => x.ProviderName == item.ProviderName && x.Run == item.Run),
                        issueProvider,
                        buildIdentifier));
            }
        }

        return result;
    }

    private record PullRequestStateIssue(string ProviderType, string ProviderName, string Run);

    private class IssueProviderAndRunComparer : IEqualityComparer<PullRequestStateIssue>
    {
        public bool Equals(PullRequestStateIssue x, PullRequestStateIssue y)
        {
            return x.ProviderType == y.ProviderType && x.Run == y.Run;
        }

        public int GetHashCode(PullRequestStateIssue obj)
        {
            var result = obj.ProviderType.GetHashCode(StringComparison.OrdinalIgnoreCase);

            if (obj.Run != null)
            {
                result ^= obj.Run.GetHashCode(StringComparison.OrdinalIgnoreCase);
            }

            return result;
        }
    }

    private static PullRequestStatus CreatePullRequestState(
        IEnumerable<IIssue> issues,
        string issueIdentifier,
        string buildIdentifier)
    {
        var issuesList = issues.ToList();

        var pullRequestStatusName = "Issues";
        var pullRequestDescriptionIfIssues = $"Found {issuesList.Count} issues";
        var pullRequestDescriptionIfNoIssues = "No issues found";

        if (!string.IsNullOrWhiteSpace(issueIdentifier))
        {
            pullRequestStatusName += $"-{issueIdentifier}";
            pullRequestDescriptionIfIssues += $" for {issueIdentifier}";
            pullRequestDescriptionIfNoIssues += $" for {issueIdentifier}";
        }

        if (!string.IsNullOrWhiteSpace(buildIdentifier))
        {
            pullRequestStatusName += $"-{buildIdentifier}";
            pullRequestDescriptionIfIssues += $" in build {buildIdentifier}";
            pullRequestDescriptionIfNoIssues += $" in build {buildIdentifier}";
        }

        var state =
            issuesList.Count != 0 ?
                PullRequestStatusState.Failed :
                PullRequestStatusState.Succeeded;

        return new PullRequestStatus(
            pullRequestStatusName,
            "Cake.Issues.Recipe",
            state,
            issuesList.Count != 0 ? pullRequestDescriptionIfIssues : pullRequestDescriptionIfNoIssues);
    }
}
