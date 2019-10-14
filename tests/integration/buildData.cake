public class BuildData
{
    public IssuesData IssuesData { get; }

    public FilePath MsBuildLogFilePath { get; }

    public FilePath InspectCodeLogFilePath { get; }

    public BuildData(ICakeContext context)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        this.IssuesData = context.Data.Get<IssuesData>();

        this.MsBuildLogFilePath = IssuesParameters.OutputDirectory.CombineWithFilePath("msbuild.log");
        this.InspectCodeLogFilePath = IssuesParameters.OutputDirectory.CombineWithFilePath("inspectCode.log");
    }
}