namespace Cake.Frosting.Issues.Recipe.Tests.PullRequestSystems;

public sealed class GitHubPullRequestSystemTests
{
    public sealed class TheTryParseGitHubUrlMethod
    {
        [Fact]
        public void Should_Parse_GitHub_Https_Url_Successfully()
        {
            // Given
            var repositoryUrl = "https://github.com/cake-contrib/Cake.Issues.Recipe.git";

            // When
            var result = GitHubPullRequestSystem.TryParseGitHubUrl(repositoryUrl, out var owner, out var repository);

            // Then
            result.ShouldBeTrue();
            owner.ShouldBe("cake-contrib");
            repository.ShouldBe("Cake.Issues.Recipe");
        }

        [Fact]
        public void Should_Parse_GitHub_Https_Url_Without_Git_Suffix_Successfully()
        {
            // Given
            var repositoryUrl = "https://github.com/cake-contrib/Cake.Issues.Recipe";

            // When
            var result = GitHubPullRequestSystem.TryParseGitHubUrl(repositoryUrl, out var owner, out var repository);

            // Then
            result.ShouldBeTrue();
            owner.ShouldBe("cake-contrib");
            repository.ShouldBe("Cake.Issues.Recipe");
        }

        [Fact]
        public void Should_Return_False_For_Non_GitHub_Url()
        {
            // Given
            var repositoryUrl = "https://gitlab.com/someowner/somerepo.git";

            // When
            var result = GitHubPullRequestSystem.TryParseGitHubUrl(repositoryUrl, out var owner, out var repository);

            // Then
            result.ShouldBeFalse();
            owner.ShouldBeNull();
            repository.ShouldBeNull();
        }

        [Fact]
        public void Should_Return_False_For_Invalid_Url()
        {
            // Given
            var repositoryUrl = "not-a-valid-url";

            // When
            var result = GitHubPullRequestSystem.TryParseGitHubUrl(repositoryUrl, out var owner, out var repository);

            // Then
            result.ShouldBeFalse();
            owner.ShouldBeNull();
            repository.ShouldBeNull();
        }

        [Fact]
        public void Should_Return_False_For_Null_Or_Empty_Url()
        {
            // Given & When & Then
            GitHubPullRequestSystem.TryParseGitHubUrl(null, out var owner1, out var repository1).ShouldBeFalse();
            owner1.ShouldBeNull();
            repository1.ShouldBeNull();

            GitHubPullRequestSystem.TryParseGitHubUrl("", out var owner2, out var repository2).ShouldBeFalse();
            owner2.ShouldBeNull();
            repository2.ShouldBeNull();

            GitHubPullRequestSystem.TryParseGitHubUrl("   ", out var owner3, out var repository3).ShouldBeFalse();
            owner3.ShouldBeNull();
            repository3.ShouldBeNull();
        }

        [Fact]
        public void Should_Return_False_For_Incomplete_GitHub_Url()
        {
            // Given
            var repositoryUrl = "https://github.com/cake-contrib";

            // When
            var result = GitHubPullRequestSystem.TryParseGitHubUrl(repositoryUrl, out var owner, out var repository);

            // Then
            result.ShouldBeFalse();
            owner.ShouldBeNull();
            repository.ShouldBeNull();
        }
    }
}