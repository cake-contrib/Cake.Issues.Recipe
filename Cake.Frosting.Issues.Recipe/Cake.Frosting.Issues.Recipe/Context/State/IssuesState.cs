using Cake.Common;
using Cake.Common.Build;
using Cake.Common.Diagnostics;
using Cake.Common.IO;
using Cake.Core.IO;
using Cake.Issues;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Cake.Frosting.Issues.Recipe
{
    /// <summary>
    /// Mutable state of the build run.
    /// </summary>
    public class IssuesState : IIssuesState
    {
        private readonly IIssuesContext context;

        private readonly List<IIssue> issues = new List<IIssue>();

        private readonly List<(IIssueProvider, string)> issueProvidersAndRuns = new List<(IIssueProvider, string)>();

        /// <inheritdoc />
        public DirectoryPath RepositoryRootDirectory { get; }

        /// <inheritdoc />
        public DirectoryPath BuildRootDirectory { get; }

        /// <inheritdoc />
        public DirectoryPath ProjectRootDirectory { get; set; }

        /// <inheritdoc />
        public Uri RepositoryRemoteUrl { get; }

        /// <inheritdoc />
        public string CommitId { get; }

        /// <inheritdoc />
        public FilePath FullIssuesReport { get; set; }

        /// <inheritdoc />
        public FilePath SummaryIssuesReport { get; set; }

        /// <inheritdoc />
        public IRepositoryInfoProvider RepositoryInfo { get; }

        /// <inheritdoc />
        public IIssuesBuildServer BuildServer { get; }

        /// <inheritdoc />
        public IIssuesPullRequestSystem PullRequestSystem { get; }

        /// <inheritdoc />
        public IEnumerable<IIssue> Issues => this.issues.AsReadOnly();

        /// <inheritdoc />
        public IList<(IIssueProvider, string)> IssueProvidersAndRuns => this.issueProvidersAndRuns.AsReadOnly();

        /// <summary>
        /// Creates a new instance of the <see cref="IssuesState"/> class.
        /// </summary>
        /// <param name="context">The Cake context.</param>
        /// <param name="repositoryInfoProviderType">Defines how information about the Git repository should be determined.</param>
        public IssuesState(
            IIssuesContext context,
            RepositoryInfoProviderType repositoryInfoProviderType)
        {
            context.NotNull(nameof(context));

            this.context = context;

            this.BuildRootDirectory = context.MakeAbsolute(context.Directory("./"));
            context.Information("Build script root directory: {0}", this.BuildRootDirectory);

            this.ProjectRootDirectory = this.BuildRootDirectory.Combine("..").Collapse();
            context.Information("Project root directory: {0}", this.ProjectRootDirectory);

            this.RepositoryInfo = DetermineRepositoryInfoProvider(context, repositoryInfoProviderType);

            this.RepositoryRootDirectory = this.RepositoryInfo.GetRepositoryRootDirectory(context, this.BuildRootDirectory);
            context.Information("Repository root directory: {0}", this.RepositoryRootDirectory);

            this.BuildServer = DetermineBuildServer(context);
            if (this.BuildServer != null)
            {
                this.RepositoryRemoteUrl =
                    BuildServer.DetermineRepositoryRemoteUrl(context, this.RepositoryRootDirectory);
                context.Information("Repository remote URL: {0}", this.RepositoryRemoteUrl);

                this.CommitId =
                    BuildServer.DetermineCommitId(context, this.RepositoryRootDirectory);
                context.Information("CommitId: {0}", this.CommitId);

                this.PullRequestSystem =
                    DeterminePullRequestSystem(
                        context,
                        this.RepositoryRemoteUrl);
            }
        }

        /// <inheritdoc />
        public void AddIssue(IIssue issue)
        {
            issue.NotNull(nameof(issue));

            this.issues.Add(issue);
        }

        /// <inheritdoc />
        public void AddIssues(IEnumerable<IIssue> issues)
        {
            issues.NotNull(nameof(issues));

            this.issues.AddRange(issues);
        }

        /// <inheritdoc />
        public void AddIssues(IIssueProvider issueProvider, IReadIssuesSettings settings)
        {
            issueProvider.NotNull(nameof(issueProvider));

            this.issueProvidersAndRuns.Add((issueProvider, settings?.Run));

            // Define default settings.
            var defaultSettings = new ReadIssuesSettings(this.ProjectRootDirectory);

            if (this.PullRequestSystem != null)
            {
                defaultSettings.FileLinkSettings =
                    this.PullRequestSystem.GetFileLinkSettings(context);
            }

            AddIssues(
                context.ReadIssues(
                    issueProvider,
                    GetSettings(settings, defaultSettings)));
        }

        /// <summary>
        /// Determines the repository info provider to use.
        /// </summary>
        /// <param name="context">The Cake context.</param>
        /// <param name="repositoryInfoProviderType">Defines how information about the Git repository should be determined.</param>
        /// <returns>The repository info provider which should be used.</returns>
        private static IRepositoryInfoProvider DetermineRepositoryInfoProvider(
            IIssuesContext context,
            RepositoryInfoProviderType repositoryInfoProviderType)
        {
            context.NotNull(nameof(context));

            switch (repositoryInfoProviderType)
            {
                case RepositoryInfoProviderType.CakeGit:
                    context.Information("Using Cake.Git for providing repository information");
                    return new CakeGitRepositoryInfoProvider();
                case RepositoryInfoProviderType.Cli:
                    context.Information("Using Git CLI for providing repository information");
                    return new CliRepositoryInfoProvider();
                default:
                    throw new NotImplementedException("Unsupported repository info provider");
            }
        }

        /// <summary>
        /// Determines the build server on which the build is running.
        /// </summary>
        /// <param name="context">The Cake context.</param>
        /// <returns>The build server on which the build is running or <c>null</c> if unknown build server.</returns>
        private static IIssuesBuildServer DetermineBuildServer(IIssuesContext context)
        {
            context.NotNull(nameof(context));

            // Could be simplified once https://github.com/cake-build/cake/issues/1684 / https://github.com/cake-build/cake/issues/1580 are fixed.
            if (!string.IsNullOrWhiteSpace(context.EnvironmentVariable("TF_BUILD")) &&
                !string.IsNullOrWhiteSpace(context.EnvironmentVariable("SYSTEM_COLLECTIONURI")) &&
                (
                    new Uri(context.EnvironmentVariable("SYSTEM_COLLECTIONURI")).Host == "dev.azure.com" ||
                    new Uri(context.EnvironmentVariable("SYSTEM_COLLECTIONURI")).Host.EndsWith("visualstudio.com", StringComparison.InvariantCulture)
                ))
            {
                context.Information("Build server detected: {0}", "Azure Pipelines");
                return new AzureDevOpsBuildServer();
            }

            if (context.AppVeyor().IsRunningOnAppVeyor)
            {
                context.Information("Build server detected: {0}", "AppVeyor");
                return new AppVeyorBuildServer();
            }

            if (context.GitHubActions().IsRunningOnGitHubActions)
            {
                context.Information("Build server detected: {0}", "GitHub Actions");
                return new GitHubActionsBuildServer();
            }

            return null;
        }

        /// <summary>
        /// Determines the pull request system.
        /// </summary>
        /// <param name="context">The Cake context.</param>
        /// <param name="repositoryUrl">The URL of the remote repository.</param>
        /// <returns>The pull request system or <c>null</c> if unknown pull request system.</returns>
        private static IIssuesPullRequestSystem DeterminePullRequestSystem(IIssuesContext context, Uri repositoryUrl)
        {
            context.NotNull(nameof(context));
            repositoryUrl.NotNull(nameof(repositoryUrl));

            if (repositoryUrl.Host == "dev.azure.com" || repositoryUrl.Host.EndsWith("visualstudio.com", StringComparison.InvariantCulture))
            {
                context.Information("Pull request system detected: {0}", "Azure Repos");
                return new AzureDevOpsPullRequestSystem();
            }

            if (repositoryUrl.Host == "github.com")
            {
                return new GitHubPullRequestSystem();
            }

            return null;
        }

        private static IReadIssuesSettings GetSettings(IReadIssuesSettings configuredSettings, IReadIssuesSettings defaultSettings)
        {
            if (configuredSettings == null)
            {
                return defaultSettings;
            }

            if (configuredSettings.FileLinkSettings == null)
            {
                configuredSettings.FileLinkSettings = defaultSettings.FileLinkSettings;
            }

            return configuredSettings;
        }
    }
}
