namespace Cake.Frosting.Issues.Recipe;

using Cake.Core;
using Cake.Core.IO;
using GitReader;
using GitReader.Structures;

/// <summary>
/// Provider to retrieve repository information using <see href="https://github.com/kekyo/GitReader">GitReader library</see>.
/// </summary>
internal sealed class GitReaderRepositoryInfoProvider : IRepositoryInfoProvider
{
    /// <inheritdoc />
    public DirectoryPath GetRepositoryRootDirectory(ICakeContext context, DirectoryPath buildRootDirectory)
    {
        context.NotNull();
        buildRootDirectory.NotNull();

        // Search for .git directory starting from build root and going up
        var currentDir = new DirectoryInfo(buildRootDirectory.FullPath);
        while (currentDir != null)
        {
            var gitDir = Path.Combine(currentDir.FullName, ".git");
            if (Directory.Exists(gitDir) || File.Exists(gitDir))
            {
                return new DirectoryPath(currentDir.FullName);
            }
            currentDir = currentDir.Parent;
        }

        throw new DirectoryNotFoundException($"Could not find Git repository root starting from {buildRootDirectory.FullPath}");
    }

    /// <inheritdoc />
    public Uri GetRepositoryRemoteUrl(ICakeContext context, DirectoryPath repositoryRootDirectory)
    {
        context.NotNull();
        repositoryRootDirectory.NotNull();

        try
        {
            // Read the origin remote URL from .git/config
            var configPath = Path.Combine(repositoryRootDirectory.FullPath, ".git", "config");
            if (File.Exists(configPath))
            {
                var configLines = File.ReadAllLines(configPath);
                bool inOriginRemote = false;
                
                foreach (var line in configLines)
                {
                    var trimmedLine = line.Trim();
                    
                    if (trimmedLine == "[remote \"origin\"]")
                    {
                        inOriginRemote = true;
                        continue;
                    }
                    
                    if (trimmedLine.StartsWith("[") && inOriginRemote)
                    {
                        // Exited the origin remote section
                        break;
                    }
                    
                    if (inOriginRemote && trimmedLine.StartsWith("url = "))
                    {
                        var url = trimmedLine.Substring(6); // Remove "url = "
                        return new Uri(url);
                    }
                }
            }
            
            throw new InvalidOperationException("Could not find origin remote URL in repository configuration");
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to read repository remote URL from {repositoryRootDirectory.FullPath}", ex);
        }
    }

    /// <inheritdoc />
    public string GetCommitId(ICakeContext context, DirectoryPath repositoryRootDirectory)
    {
        context.NotNull();
        repositoryRootDirectory.NotNull();

        try
        {
            // Use async method with GetAwaiter().GetResult() pattern as used in other parts of Cake
            using var repository = Repository.Factory.OpenStructureAsync(repositoryRootDirectory.FullPath).GetAwaiter().GetResult();
            
            if (repository.Head is Branch head)
            {
                var commit = head.GetHeadCommitAsync().GetAwaiter().GetResult();
                return commit.Hash.ToString();
            }
            
            throw new InvalidOperationException("Could not find HEAD commit in repository");
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to read commit ID from {repositoryRootDirectory.FullPath}", ex);
        }
    }
}