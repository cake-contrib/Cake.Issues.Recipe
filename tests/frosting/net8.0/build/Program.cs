using System;
using System.Reflection;
using Cake.Common;
using Cake.Common.Diagnostics;
using Cake.Common.Tools.DotNet;
using Cake.Common.Tools.DotNet.Build;
using Cake.Common.Tools.DotNet.MSBuild;
using Cake.Common.Tools.InspectCode;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Frosting;
using Cake.Frosting.Issues.Recipe;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

public class BuildContext : IssuesContext<BuildParameters, BuildState>
{
    public BuildContext(ICakeContext context)
        : base(context)
    {
    }

    protected override BuildParameters CreateIssuesParameters()
    {
        return new BuildParameters();
    }

    protected override BuildState CreateIssuesState()
    {
        return new BuildState(this);
    }
}

public class BuildParameters : IssuesParameters
{
    public DirectoryPath LogDirectoryPath => this.OutputDirectory.Combine("logs");

    public BuildParameters()
    {
        this.OutputDirectory = this.OutputDirectory.Combine("output");
    }
}

public class BuildState : IssuesState
{
    public FilePath SolutionFilePath { get; }

    public BuildState(BuildContext context)
        : base(context, RepositoryInfoProviderType.Cli)
    {
        this.SolutionFilePath =
            this.ProjectRootDirectory
                .Combine("src")
                .CombineWithFilePath("ClassLibrary1.sln");
    }
}

public class Lifetime : FrostingLifetime<BuildContext>
{
    public override void Setup(BuildContext context, ISetupContext info)
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

[TaskName("Build")]
public sealed class BuildTask : FrostingTask<BuildContext>
{
    public override void Run(BuildContext context)
    {
        var solutionFile = context.State.SolutionFilePath;
        var msBuildLogFilePath = context.Parameters.LogDirectoryPath.CombineWithFilePath("msbuild.binlog");

        context.DotNetRestore(solutionFile.FullPath);

        var settings =
            new DotNetMSBuildSettings()
                .WithTarget("Rebuild")
                .EnableBinaryLogger(msBuildLogFilePath.FullPath);

        context.DotNetBuild(
            solutionFile.FullPath,
                new DotNetBuildSettings
                {
                    MSBuildSettings = settings
                });

        // Pass path to log file to Cake.Frosting.Issues.Recipe.
        context.Parameters.InputFiles.AddMsBuildBinaryLogFile(msBuildLogFilePath);
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
        var inspectCodeLogFilePath = context.Parameters.LogDirectoryPath.CombineWithFilePath("inspectCode.log");

        var settings = new InspectCodeSettings() {
            OutputFile = inspectCodeLogFilePath,
            ArgumentCustomization = x => x.Append("--no-build"),
            WorkingDirectory = context.State.ProjectRootDirectory
        };

        context.InspectCode(
            context.State.SolutionFilePath,
            settings);

        // Pass path to InspectCode log file to Cake.Frosting.Issues.Recipe.
        context.Parameters.InputFiles.AddInspectCodeLogFile(inspectCodeLogFilePath);
    }
}

[TaskName("Lint")]
[IsDependentOn(typeof(BuildTask))]
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
