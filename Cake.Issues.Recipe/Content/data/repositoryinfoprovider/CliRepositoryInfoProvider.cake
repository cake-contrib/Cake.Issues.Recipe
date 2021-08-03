/// <summary>
/// Provider to retrieve repository information using Git CLI.
/// </summary>
public class CliRepositoryInfoProvider : IRepositoryInfoProvider
{
    /// <inheritdoc />
    public DirectoryPath GetRepositoryRootDirectory(ICakeContext context, DirectoryPath buildRootDirectory)
    {
        var result =
            this.GitCommand(context, buildRootDirectory, "rev-parse", "--show-toplevel");
        return new DirectoryPath(result.Single());
    }

    /// <inheritdoc />
    public Uri GetRepositoryRemoteUrl(ICakeContext context, DirectoryPath repositoryRootDirectory)
    {
        var result =
            this.GitCommand(context, repositoryRootDirectory, "config", "--get", "remote.origin.url");
        return new Uri(result.Single());
    }

    /// <inheritdoc />
    public string GetCommitId(ICakeContext context, DirectoryPath repositoryRootDirectory)
    {
        return
            this.GitCommand(context, repositoryRootDirectory, "rev-parse", "HEAD")
            .Single();
    }

    private  IEnumerable<string> GitCommand(
        ICakeContext context,
        DirectoryPath repositoryRootFolder,
        params string[] arguments)
    {
        if (!arguments.Any())
        {
            throw new ArgumentOutOfRangeException(nameof(arguments));
        }
        var gitArguments = string.Join(" ", arguments);
        var exitCode = context.StartProcess(
            "git",
            new ProcessSettings
            {
                Arguments = gitArguments,
                WorkingDirectory = repositoryRootFolder.FullPath,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            },
            out var redirectedStandardOutput,
            out var redirectedErrorOutput
        );
        if (exitCode != 0)
        {
            throw new Exception(
                $"Git command failed with arguments {gitArguments}. Exit code: {exitCode}. Error output: {string.Join(Environment.NewLine, redirectedErrorOutput)}"
            );
        }
        return redirectedStandardOutput;
    }
}