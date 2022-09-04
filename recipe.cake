#load nuget:https://pkgs.dev.azure.com/cake-contrib/Home/_packaging/addins/nuget/v3/index.json?package=Cake.Recipe&version=3.0.0-beta0001-0007&prerelease

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
    shouldGenerateDocumentation: false);

BuildParameters.PrintParameters(Context);

ToolSettings.SetToolSettings(context: Context);

//*************************************************************************************************
// Extensions
//*************************************************************************************************

Task("Generate-Version-File")
    .Does<BuildVersion>((context, buildVersion) => {
        var buildMetaDataCodeGen = TransformText(@"
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
