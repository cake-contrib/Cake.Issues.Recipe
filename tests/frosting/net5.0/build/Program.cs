using System;
using System.Reflection;
using Cake.Common;
using Cake.Common.Diagnostics;
using Cake.Common.Tools.DotNetCore;
using Cake.Common.Tools.DotNetCore.Build;
using Cake.Common.Tools.DotNetCore.MSBuild;
using Cake.Common.Tools.InspectCode;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Frosting;
using Cake.Issues.FrostingRecipe;

public static class Program
{
    public static int Main(string[] args)
    {
        return new CakeHost()
            .UseContext<BuildContext>()
            .UseLifetime<Lifetime>()
            .InstallTool(new Uri("nuget:?package=JetBrains.ReSharper.CommandLineTools"))
            .AddAssembly(Assembly.GetAssembly(typeof(IssuesTask)))
            .Run(args);
    }
}

public class BuildContext : IssuesContext
{
    public DirectoryPath BuildArtifactsDirectory { get; }

    public FilePath SolutionFilePath { get; }

    public FilePath MsBuildLogFilePath { get; }

    public FilePath InspectCodeLogFilePath { get; }

    public FilePath DupFinderLogFilePath { get; }

    public FilePath MarkdownlintCliLogFilePath { get; }

    public BuildContext(ICakeContext context)
        : base(context)
    {
        this.BuildArtifactsDirectory = new DirectoryPath("BuildArtifacts");

        this.SolutionFilePath =
            this.State.BuildRootDirectory
                .Combine("..")
                .Combine("src")
                .Combine("ClassLibrary1")
                .CombineWithFilePath("ClassLibrary1.csproj");

        var logDirectory = this.BuildArtifactsDirectory.Combine("logs");
        this.MsBuildLogFilePath = logDirectory.CombineWithFilePath("msbuild.binlog");
        this.InspectCodeLogFilePath = logDirectory.CombineWithFilePath("inspectCode.log");
        this.DupFinderLogFilePath = logDirectory.CombineWithFilePath("dupFinder.log");
        this.MarkdownlintCliLogFilePath = logDirectory.CombineWithFilePath("markdownlintCli.log");
    }
}

public class Lifetime : FrostingLifetime<BuildContext>
{
    public override void Setup(BuildContext context)
    {
        context.Parameters.Reporting.ShouldCreateFullIssuesReport = false;

        var platform = context.Environment.Platform.Family.ToString();
        var runtime = context.Environment.Runtime.IsCoreClr ? ".NET Core" : ".NET Framework";

        context.Parameters.BuildIdentifier = $"Cake Frosting {platform} ({runtime})";

        context.Information("Build identifier: {0}", context.Parameters.BuildIdentifier);
    }

    public override void Teardown(BuildContext context, ITeardownContext info)
    {
    }
}

[TaskName("Build")]
[IsDependeeOf(typeof(ReadIssuesTask))]
public sealed class BuildTask : FrostingTask<BuildContext>
{
    public override void Run(BuildContext context)
    {
        var solutionFile =
            context.State.BuildRootDirectory
                .Combine("..")
                .Combine("src")
                .Combine("ClassLibrary1")
                .CombineWithFilePath("ClassLibrary1.csproj");

        context.DotNetCoreRestore(solutionFile.FullPath);

        // TODO Use StructuredLogger
        var settings =
            new DotNetCoreMSBuildSettings()
                .WithTarget("Rebuild")
                .EnableBinaryLogger(context.MsBuildLogFilePath.FullPath);
        //var settings =
        //    new DotNetCoreMSBuildSettings()
        //        .WithTarget("Rebuild")
        //        .WithLogger(
        //            "BinaryLogger," + context.Tools.Resolve("Cake.Issues.MsBuild*/**/StructuredLogger.dll"),
        //            "",
        //            msBuildLogFilePath.FullPath
        //        );

        context.DotNetCoreBuild(
            context.SolutionFilePath.FullPath,
            new DotNetCoreBuildSettings
            {
                MSBuildSettings = settings
            });

        // Pass path to MsBuild log file to Cake.Issues.Recipe
        //context.Parameters.InputFiles.MsBuildBinaryLogFilePath = context.MsBuildLogFilePath;
    }
}

[TaskName("Run-InspectCode")]
public sealed class RunInspectCodeTask : FrostingTask<BuildContext>
{
    public override bool ShouldRun(BuildContext context)
    {
        return context.IsRunningOnWindows();
    }

    public override void Run(BuildContext context)
    {
        var settings = new InspectCodeSettings() {
            OutputFile = context.InspectCodeLogFilePath
        };

        context.InspectCode(
            context.State.BuildRootDirectory.Combine("..").Combine("src").CombineWithFilePath("ClassLibrary1.sln"),
            settings);

        // Pass path to InspectCode log file to Cake.Issues.Recipe
        context.Parameters.InputFiles.AddInspectCodeLogFile(context.InspectCodeLogFilePath);
    }
}

[TaskName("Lint")]
[IsDependentOn(typeof(RunInspectCodeTask))]
[IsDependeeOf(typeof(ReadIssuesTask))]
public class LintTask : FrostingTask
{
}

[TaskName("Default")]
[IsDependentOn(typeof(IssuesTask))]
public class DefaultTask : FrostingTask
{
}