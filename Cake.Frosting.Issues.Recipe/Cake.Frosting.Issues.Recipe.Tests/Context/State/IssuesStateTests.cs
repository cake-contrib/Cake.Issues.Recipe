namespace Cake.Frosting.Issues.Recipe.Tests.Context.State;

using Cake.Core.IO;
using Shouldly;
using Xunit;

public sealed class IssuesStateVirtualMethodTests
{
    public sealed class TheProjectRootDirectoryProviderFunctionality
    {
        [Fact]
        public void Should_Use_Custom_Function_When_Provided()
        {
            // Given
            var fixture = new CakeContextFixture();
            var buildRootDirectory = new DirectoryPath("/test/build");
            var customProjectRoot = new DirectoryPath("/custom/project");

            DirectoryPath customProvider(IIssuesState state) => customProjectRoot;

            // When
            var state = new IssuesState(
                fixture.CreateContext(),
                RepositoryInfoProviderType.CakeGit,
                customProvider);

            // Then
            state.ProjectRootDirectory.ShouldBe(customProjectRoot);
        }

        [Fact]
        public void Should_Use_Default_Logic_When_Provider_Is_Null()
        {
            // Given
            var fixture = new CakeContextFixture();
            var buildRootDirectory = new DirectoryPath(Directory.GetCurrentDirectory());
            var expectedProjectRoot = buildRootDirectory.Combine("..").Collapse();

            // When
            var state = new IssuesState(
                fixture.CreateContext(),
                RepositoryInfoProviderType.CakeGit,
                null);

            // Then
            state.ProjectRootDirectory.ShouldBe(expectedProjectRoot);
        }

        [Fact]
        public void Should_Pass_State_Instance_To_Provider_Function()
        {
            // Given
            var fixture = new CakeContextFixture();
            var buildRootDirectory = new DirectoryPath("/test/build");

            IIssuesState capturedState = null;
            DirectoryPath customProvider(IIssuesState state)
            {
                capturedState = state;
                return state.BuildRootDirectory;
            }

            // When
            var state = new IssuesState(
                fixture.CreateContext(),
                RepositoryInfoProviderType.CakeGit,
                customProvider);

            // Then
            capturedState.ShouldNotBeNull().ShouldBe(state);
        }
    }
}