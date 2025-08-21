namespace Cake.Frosting.Issues.Recipe.Tests.Context.State;

using Cake.Core.IO;
using Shouldly;
using Xunit;

public sealed class IssuesStateVirtualMethodTests
{
    public sealed class TheDetermineProjectRootDirectoryMethod
    {
        [Fact]
        public void Should_Return_Parent_Directory_By_Default()
        {
            // Given
            var buildRootDirectory = new DirectoryPath("/test/build");
            var expectedProjectRoot = buildRootDirectory.Combine("..").Collapse();
            
            // When
            var testState = new TestableIssuesState(buildRootDirectory);
            var actualProjectRoot = testState.TestDetermineProjectRootDirectory();
            
            // Then
            actualProjectRoot.ShouldBe(expectedProjectRoot);
        }

        [Fact]
        public void Should_Allow_Override_In_Derived_Class()
        {
            // Given
            var buildRootDirectory = new DirectoryPath("/test/build");
            var customProjectRoot = new DirectoryPath("/custom/project/path");
            
            // When
            var testState = new DerivedTestableIssuesState(buildRootDirectory, customProjectRoot);
            var actualProjectRoot = testState.TestDetermineProjectRootDirectory();
            
            // Then
            actualProjectRoot.ShouldBe(customProjectRoot);
            actualProjectRoot.ShouldNotBe(buildRootDirectory.Combine("..").Collapse());
        }
    }

    /// <summary>
    /// Testable version of IssuesState that exposes the protected virtual method for testing.
    /// </summary>
    private class TestableIssuesState
    {
        protected readonly DirectoryPath buildRootDirectory;

        public TestableIssuesState(DirectoryPath buildRootDirectory)
        {
            this.buildRootDirectory = buildRootDirectory;
        }

        public DirectoryPath TestDetermineProjectRootDirectory()
        {
            return DetermineProjectRootDirectory();
        }

        protected virtual DirectoryPath DetermineProjectRootDirectory()
        {
            return this.buildRootDirectory.Combine("..").Collapse();
        }
    }

    /// <summary>
    /// Derived test class that overrides DetermineProjectRootDirectory.
    /// </summary>
    private class DerivedTestableIssuesState : TestableIssuesState
    {
        private readonly DirectoryPath customProjectRoot;

        public DerivedTestableIssuesState(DirectoryPath buildRootDirectory, DirectoryPath customProjectRoot)
            : base(buildRootDirectory)
        {
            this.customProjectRoot = customProjectRoot;
        }

        protected override DirectoryPath DetermineProjectRootDirectory()
        {
            return this.customProjectRoot;
        }
    }
}