using System;
using System.Reflection;
using Cake.Common;
using Cake.Common.Diagnostics;
using Cake.Common.Tools.InspectCode;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Frosting;
using Cake.Frosting.Issues.Recipe;

public static class Program
{
    public static int Main(string[] args)
    {
        return new CakeHost()
            .UseContext<BuildContext>()
            .UseLifetime<Lifetime>()
            .InstallTool(new Uri("nuget:?package=JetBrains.ReSharper.CommandLineTools"))
            // Register Cake.Frosting.Issues.Recipe tasks.
            .AddAssembly(Assembly.GetAssembly(typeof(IssuesTask)))
            .Run(args);
    }
}

public class BuildContext : IssuesContext
{
    public DirectoryPath LogDirectoryPath { get; }

    public FilePath SolutionFilePath { get; }

    public BuildContext(ICakeContext context)
        : base(context, RepositoryInfoProviderType.Cli)
    {
        this.LogDirectoryPath = this.Parameters.OutputDirectory.Combine("logs");
        this.SolutionFilePath =
            this.State.BuildRootDirectory
                .Combine("..")
                .Combine("src")
                .CombineWithFilePath("ClassLibrary1.sln");
    }
}

public class Lifetime : FrostingLifetime<BuildContext>
{
    public override void Setup(BuildContext context)
    {
        // Determine Build Identifier
        var platform = context.Environment.Platform.Family.ToString();
        var runtime = context.Environment.Runtime.IsCoreClr ? ".NET Core" : ".NET Framework";

        context.Parameters.BuildIdentifier = $"Cake Frosting {platform} ({runtime})";

        context.Information("Build identifier: {0}", context.Parameters.BuildIdentifier);
    }

    public override void Teardown(BuildContext context, ITeardownContext info)
    {
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
        // Run InspectCode.
        var inspectCodeLogFilePath = context.LogDirectoryPath.CombineWithFilePath("inspectCode.log");

        var settings = new InspectCodeSettings() {
            OutputFile = inspectCodeLogFilePath,
            ArgumentCustomization = x => x.Append("--no-build")
        };

        context.InspectCode(
            context.SolutionFilePath,
            settings);

        // Pass path to InspectCode log file to Cake.Frosting.Issues.Recipe.
        context.Parameters.InputFiles.AddInspectCodeLogFile(inspectCodeLogFilePath);
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