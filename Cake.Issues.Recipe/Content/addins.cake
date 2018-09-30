///////////////////////////////////////////////////////////////////////////////
// ADDINS
///////////////////////////////////////////////////////////////////////////////

#addin nuget:?package=Cake.Issues&version=0.6.2

Action<string> RequireAddin = (addin) => {
    var script = MakeAbsolute(File(string.Format("./{0}.cake", Guid.NewGuid())));
    try
    {
        System.IO.File.WriteAllText(script.FullPath, addin);
        CakeExecuteScript(script);
    }
    finally
    {
        if (FileExists(script))
        {
            DeleteFile(script);
        }
    }
};
