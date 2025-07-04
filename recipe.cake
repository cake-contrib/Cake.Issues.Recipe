#load nuget:https://pkgs.dev.azure.com/cake-contrib/Home/_packaging/addins/nuget/v3/index.json?package=Cake.Recipe&version=4.0.0-alpha0130

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
    shouldRunInspectCode: false, // Currently failing on AppVeyor since .NET 9 update
    shouldRunDotNetCorePack: true,
    shouldGenerateDocumentation: false);

BuildParameters.PrintParameters(Context);

ToolSettings.SetToolSettings(context: Context);

//*************************************************************************************************
// Extensions
//*************************************************************************************************

Task("Generate-Version-File")
    .Does<BuildVersion>((context, buildVersion) => {
        // Write metadata to configuration file
        System.IO.File.WriteAllText(
            "./Cake.Issues.Recipe/cake-version.yml",
            @"TargetCakeVersion: 5.0.0
TargetFrameworks:
- net8.0
- net9.0"
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
