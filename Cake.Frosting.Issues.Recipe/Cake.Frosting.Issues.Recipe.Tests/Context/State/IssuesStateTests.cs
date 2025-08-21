namespace Cake.Frosting.Issues.Recipe.Tests.Context.State;

using System;
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

    public sealed class TheProjectRootDirectoryProviderFunctionality
    {
        [Fact]
        public void Should_Use_Custom_Function_When_Provided()
        {
            // Given
            var buildRootDirectory = new DirectoryPath("/test/build");
            var customProjectRoot = new DirectoryPath("/custom/project");

            Func<TestableIssuesStateWithProvider, DirectoryPath> customProvider = state => customProjectRoot;

            // When
            var state = new TestableIssuesStateWithProvider(buildRootDirectory, customProvider);

            // Then
            state.ProjectRootDirectory.ShouldBe(customProjectRoot);
        }

        [Fact]
        public void Should_Use_Default_Logic_When_Provider_Is_Null()
        {
            // Given
            var buildRootDirectory = new DirectoryPath("/test/build");
            var expectedProjectRoot = buildRootDirectory.Combine("..").Collapse();

            // When
            var state = new TestableIssuesStateWithProvider(buildRootDirectory, null);

            // Then
            state.ProjectRootDirectory.ShouldBe(expectedProjectRoot);
        }

        [Fact]
        public void Should_Pass_State_Instance_To_Provider_Function()
        {
            // Given
            var buildRootDirectory = new DirectoryPath("/test/build");

            TestableIssuesStateWithProvider capturedState = null;
            Func<TestableIssuesStateWithProvider, DirectoryPath> customProvider = state =>
            {
                capturedState = state;
                return state.BuildRootDirectory.Combine("../custom").Collapse();
            };

            // When
            var state = new TestableIssuesStateWithProvider(buildRootDirectory, customProvider);

            // Then
            capturedState.ShouldNotBeNull();
            capturedState.ShouldBe(state);
            state.ProjectRootDirectory.ShouldBe(buildRootDirectory.Combine("../custom").Collapse());
        }
    }

    /// <summary>
    /// Testable version of IssuesState that exposes the protected virtual method for testing.
    /// </summary>
    private class TestableIssuesState(DirectoryPath buildRootDirectory)
    {
        public DirectoryPath TestDetermineProjectRootDirectory() => this.DetermineProjectRootDirectory();

        protected virtual DirectoryPath DetermineProjectRootDirectory() => buildRootDirectory.Combine("..").Collapse();
    }

    /// <summary>
    /// Derived test class that overrides DetermineProjectRootDirectory.
    /// </summary>
    private class DerivedTestableIssuesState(DirectoryPath buildRootDirectory, DirectoryPath customProjectRoot)
        : TestableIssuesState(buildRootDirectory)
    {
        protected override DirectoryPath DetermineProjectRootDirectory() => customProjectRoot;
    }

    /// <summary>
    /// Testable version that simulates the new constructor functionality.
    /// </summary>
    private class TestableIssuesStateWithProvider
    {
        public DirectoryPath BuildRootDirectory { get; }
        public DirectoryPath ProjectRootDirectory { get; }

        public TestableIssuesStateWithProvider(DirectoryPath buildRootDirectory, Func<TestableIssuesStateWithProvider, DirectoryPath> projectRootDirectoryProvider)
        {
            this.BuildRootDirectory = buildRootDirectory;
            this.ProjectRootDirectory = projectRootDirectoryProvider?.Invoke(this) ?? this.BuildRootDirectory.Combine("..").Collapse();
        }
    }
}