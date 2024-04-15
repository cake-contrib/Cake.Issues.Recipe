#load nuget:?package=Cake.Recipe&version=3.1.1

//*************************************************************************************************
// Settings
//*************************************************************************************************

Environment.SetVariableNames();

BuildParameters.SetParameters(
    context: Context,
    buildSystem: BuildSystem,
    sourceDirectoryPath: "./Cake.Frosting.Issues.Recipe",
    title: "Cake.Issues.Recipe",
    repositoryOwner: "cake-contrib",
    repositoryName: "Cake.Issues.Recipe",
    appVeyorAccountName: "cakecontrib",
    solutionFilePath: "./Cake.Frosting.Issues.Recipe/Cake.Frosting.Issues.Recipe.sln",
    shouldRunDotNetCorePack: true,
    shouldPostToGitter: false, // Disabled because it's currently failing
    shouldGenerateDocumentation: false);

BuildParameters.PrintParameters(Context);

ToolSettings.SetToolPreprocessorDirectives(
    gitReleaseManagerGlobalTool: "#tool dotnet:?package=GitReleaseManager.Tool&version=0.17.0"
);

ToolSettings.SetToolSettings(context: Context);

//*************************************************************************************************
// Extensions
//*************************************************************************************************

Task("Generate-Version-File")
    .Does<BuildVersion>((context, buildVersion) => {
        // Write metadata to configuration file
        System.IO.File.WriteAllText(
            "./Cake.Issues.Recipe/cake-version.yml",
            @"TargetCakeVersion: 4.0.0
TargetFrameworks:
- net6.0
- net7.0
- net8.0"
        );

        // Write metadata to class for use when running a build
        var buildMetaDataCodeGen =
            TransformText(@"
                public class BuildMetaDataCakeIssuesRecipe
                {
                    public static string Date { get; } = ""<%date%>"";
                    public static string Version { get; } = ""<%version%>"";
                }",
                "<%",
                "%>"
            )
            .WithToken("date", BuildMetaData.Date)
            .WithToken("version", buildVersion.SemVersion)
            .ToString();

        System.IO.File.WriteAllText(
            "./Cake.Issues.Recipe/Content/version.cake",
            buildMetaDataCodeGen
        );
    });

BuildParameters.Tasks.CleanTask
    .IsDependentOn("Generate-Version-File");

//*************************************************************************************************
// Execution
//*************************************************************************************************

Build.RunDotNetCore();
