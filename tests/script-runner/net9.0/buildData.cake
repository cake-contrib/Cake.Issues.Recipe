public class BuildData
{
    public IssuesData IssuesData { get; }

    public FilePath MsBuildLogFilePath { get; }

    public FilePath InspectCodeLogFilePath { get; }

    public FilePath MarkdownlintCliLogFilePath { get; }

    public BuildData(ICakeContext context)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        this.IssuesData = context.Data.Get<IssuesData>();

        var buildArtifactsDirectory = new DirectoryPath("BuildArtifacts");
        IssuesParameters.OutputDirectory = buildArtifactsDirectory.Combine("output");

        var logDirectory = buildArtifactsDirectory.Combine("logs");
        this.MsBuildLogFilePath = logDirectory.CombineWithFilePath("msbuild.binlog");
        this.InspectCodeLogFilePath = logDirectory.CombineWithFilePath("inspectCode.log");
        this.MarkdownlintCliLogFilePath = logDirectory.CombineWithFilePath("markdownlintCli.log");
    }
}