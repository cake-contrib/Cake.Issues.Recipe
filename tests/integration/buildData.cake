public class BuildData
{
    public IssuesData IssuesData { get; }

    public FilePath MsBuildLogFilePath { get; }

    public FilePath InspectCodeLogFilePath { get; }

    public FilePath DupFinderLogFilePath { get; }

    public FilePath MarkdownlintCliLogFilePath { get; }

    public BuildData(ICakeContext context)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        this.IssuesData = context.Data.Get<IssuesData>();

        this.MsBuildLogFilePath = IssuesParameters.OutputDirectory.CombineWithFilePath("msbuild.binlog");
        this.InspectCodeLogFilePath = IssuesParameters.OutputDirectory.CombineWithFilePath("inspectCode.log");
        this.DupFinderLogFilePath = IssuesParameters.OutputDirectory.CombineWithFilePath("dupFinder.log");
        this.MarkdownlintCliLogFilePath = IssuesParameters.OutputDirectory.CombineWithFilePath("markdownlintCli.log");
    }
}