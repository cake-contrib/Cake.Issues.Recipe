#load nuget:?package=Cake.Recipe&version=2.1.0

Environment.SetVariableNames();

BuildParameters.SetParameters(
    context: Context,
    buildSystem: BuildSystem,
    sourceDirectoryPath: "./Cake.Issues.FrostingRecipe",
    title: "Cake.Issues.Recipe",
    repositoryOwner: "cake-contrib",
    repositoryName: "Cake.Issues.Recipe",
    appVeyorAccountName: "cakecontrib",
    solutionFilePath: "./Cake.Issues.FrostingRecipe/Cake.Issues.FrostingRecipe.sln",
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
            public static string CakeVersion { get; } = ""<%cakeversion%>"";
        }",
        "<%",
        "%>"
        )
   .WithToken("date", BuildMetaData.Date)
   .WithToken("version", buildVersion.SemVersion)
   .WithToken("cakeversion", BuildMetaData.CakeVersion)
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
