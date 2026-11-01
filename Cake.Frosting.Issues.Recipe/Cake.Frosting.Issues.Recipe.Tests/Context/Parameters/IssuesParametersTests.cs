namespace Cake.Frosting.Issues.Recipe.Tests.Context.Parameters;

using Cake.Core.IO;
using Cake.Frosting.Issues.Recipe;
using Shouldly;
using Xunit;

public sealed class IssuesParametersTests
{
    [Fact]
    public void Should_Use_Parent_BuildArtifacts_Directory_By_Default()
    {
        // Given
        var parameters = new IssuesParameters();

        // When
        var outputDirectory = parameters.OutputDirectory;

        // Then
        outputDirectory.ShouldBe(new DirectoryPath("../BuildArtifacts"));
    }
}
