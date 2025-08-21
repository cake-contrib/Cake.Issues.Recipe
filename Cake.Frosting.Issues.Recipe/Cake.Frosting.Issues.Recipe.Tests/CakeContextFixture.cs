namespace Cake.Frosting.Issues.Recipe.Tests;

using Cake.Core;
using Cake.Core.Configuration;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Core.Tooling;
using NSubstitute;

internal sealed class CakeContextFixture
{
    public IFileSystem FileSystem { get; set; }
    public ICakeEnvironment Environment { get; set; }
    public IGlobber Globber { get; set; }
    public ICakeLog Log { get; set; }
    public ICakeArguments Arguments { get; set; }
    public IProcessRunner ProcessRunner { get; set; }
    public IRegistry Registry { get; set; }
    public IToolLocator Tools { get; set; }
    public ICakeDataService Data { get; set; }
    public ICakeConfiguration Configuration { get; set; }

    public CakeContextFixture()
    {
        this.Environment = new FakeEnvironment(PlatformFamily.Linux)
        {
            WorkingDirectory = new DirectoryPath(Directory.GetCurrentDirectory())
        };
        this.FileSystem = new FileSystem();
        this.Globber = Substitute.For<IGlobber>();
        this.Log = new FakeLog();
        this.Arguments = Substitute.For<ICakeArguments>();
        this.ProcessRunner = Substitute.For<IProcessRunner>();
        this.Registry = Substitute.For<IRegistry>();
        this.Tools = Substitute.For<IToolLocator>();
        this.Data = Substitute.For<ICakeDataService>();
        this.Configuration = new FakeConfiguration();
    }

    public IssuesContext CreateContext() => new(
        new CakeContext(
            this.FileSystem,
            this.Environment,
            this.Globber,
            this.Log,
            this.Arguments,
            this.ProcessRunner,
            this.Registry,
            this.Tools,
            this.Data,
            this.Configuration),
        RepositoryInfoProviderType.Cli);
}
