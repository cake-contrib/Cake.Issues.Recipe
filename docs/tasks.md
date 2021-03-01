---
Order: 60
Title: Tasks
Description: Tasks provided by Cake.Issues recipes.
---

Cake.Issues recipes provide the following tasks to your build script:

| Task                         | Description                                  | Cake.Issues.Recipe task instance                 | Cake.Issues.FrostingRecipe task type                       |
|------------------------------|----------------------------------------------|--------------------------------------------------|------------------------------------------------------------|
| `Issues`                     | Main tasks for issue management integration. | `IssuesBuildTasks.IssuesTask`                    | `Cake.Issues.FrostingRecipe.IssuesTask`                    |
| `Read-Issues`                | Reads issues from the provided log files.    | `IssuesBuildTasks.ReadIssuesTask`                | `Cake.Issues.FrostingRecipe.ReadIssuesTask`                |
| `Create-FullIssuesReport`    | Creates issue report.                        | `IssuesBuildTasks.CreateFullIssuesReportTask`    | `Cake.Issues.FrostingRecipe.CreateFullIssuesReportTask`    |
| `Publish-IssuesArtifacts`    | Publish artifacts to build server.           | `IssuesBuildTasks.PublishIssuesArtifactsTask`    | `Cake.Issues.FrostingRecipe.PublishIssuesArtifactsTask`    |
| `Report-IssuesToBuildServer` | Report issues to build server.               | `IssuesBuildTasks.ReportIssuesToBuildServerTask` | `Cake.Issues.FrostingRecipe.ReportIssuesToBuildServerTask` |
| `Create-SummaryIssuesReport` | Creates a summary issue report.              | `IssuesBuildTasks.CreateSummaryIssuesReportTask` | `Cake.Issues.FrostingRecipe.CreateSummaryIssuesReportTask` |
| `Report-IssuesToPullRequest` | Report issues to pull request.               | `IssuesBuildTasks.ReportIssuesToPullRequestTask` | `Cake.Issues.FrostingRecipe.ReportIssuesToPullRequestTask` |
| `Set-PullRequestIssuesState` | Set pull request status.                     | `IssuesBuildTasks.SetPullRequestIssuesStateTask` | `Cake.Issues.FrostingRecipe.SetPullRequestIssuesStateTask` |