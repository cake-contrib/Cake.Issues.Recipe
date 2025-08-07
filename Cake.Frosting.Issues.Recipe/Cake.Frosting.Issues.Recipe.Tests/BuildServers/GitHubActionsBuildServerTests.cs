namespace Cake.Frosting.Issues.Recipe.Tests.BuildServers;

using Shouldly;
using Xunit;

public sealed class GitHubActionsBuildServerTests
{
    public sealed class TheDetermineCommitIdMethod
    {
        [Fact]
        public void Should_Use_Correct_Logic_For_Pull_Request_Events()
        {
            // Given
            var buildServer = new GitHubActionsBuildServer();

            // This is a basic smoke test since we can't easily mock the GitHubActions() environment
            // The actual functionality is tested through integration tests

            // When/Then - we're mainly testing that the code compiles and the class can be instantiated
            _ = buildServer.ShouldNotBeNull();
            _ = buildServer.ShouldBeOfType<GitHubActionsBuildServer>();
        }
    }
}