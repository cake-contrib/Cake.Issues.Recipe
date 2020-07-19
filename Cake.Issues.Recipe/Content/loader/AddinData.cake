using System.Reflection;
using Cake.Core.Annotations;

public class AddinData
{
    private readonly ICakeContext _context;

    public AddinData(ICakeContext context, string packageName, string packageVersion, string assemblyName = null)
    {
        this._context = context;
        this.Initialize(context, packageName, packageVersion, assemblyName);
    }

    public Assembly AddinAssembly { get; private set; }
    public IList<Type> _declaredEnums = new List<Type>();
    public IList<TypeInfo> _definedClasses = new List<TypeInfo>();
    private IList<MethodInfo> _definedMethods = new List<MethodInfo>();

    public object CreateClass(string classTypeString, params object[] parameters)
    {
        var possibleClass = _definedClasses.FirstOrDefault(c => string.Compare(c.Name, classTypeString, StringComparison.OrdinalIgnoreCase) == 0);

        if (possibleClass is null)
        {
            throw new NullReferenceException($"No loaded class named {classTypeString} was found in this assembly.");
        }

        return CreateClass(possibleClass, parameters);
    }

    public object CreateClass(TypeInfo classType, params object[] parameters)
    {
        parameters = parameters ?? new object[0];
        var constructors = classType.DeclaredConstructors.Where(c => c.IsPublic && !c.IsStatic && c.GetParameters().Length == parameters.Length);
        ConstructorInfo constructor = null;

        foreach (var ctx in constructors)
        {
            var ctxParams = ctx.GetParameters();
            bool useCtx = true;
            for (int i = 0; i < ctxParams.Length && useCtx; i++)
            {
                useCtx = ctxParams[i].ParameterType == parameters[i].GetType();
            }

            if (useCtx)
            {
                constructor = ctx;
                break;
            }
        }

        if (constructor is null)
        {
            throw new NullReferenceException("No valid constructor was found!");
        }

        return constructor.Invoke(parameters ?? new object[0]);
    }

    public object CallStaticMethod(string methodName, params object[] parameters)
    {
        parameters = parameters ?? new object[0];

        for (int i = 0; i < parameters.Length; i++)
        {
            var parameterType = parameters[i].GetType();
            int index = parameterType == typeof(string) ? parameters[i].ToString().IndexOf('.') : -1;
            if (index >= 0)
            {
                var enumOrClass = parameters[i].ToString().Substring(0, index);
                var enumType = _declaredEnums.FirstOrDefault(e => string.Compare(e.Name, enumOrClass, StringComparison.OrdinalIgnoreCase) == 0);
                if (enumType is object)
                {
                    var value = parameters[i].ToString().Substring(index+1);
                    parameters[i] = Enum.Parse(enumType, value);
                }
            }
        }

        var methods = this._definedMethods.Where(m => m.IsPublic && m.IsStatic && string.Compare(m.Name, methodName, StringComparison.OrdinalIgnoreCase) == 0);
        MethodInfo method = null;

        foreach (var m in methods.Where(m => m.GetParameters().Length == parameters.Length))
        {
            var methodParams = m.GetParameters();
            bool useMethod = true;

            for (int i = 0; i < methodParams.Length && useMethod; i++)
            {
                var methodParamType = methodParams[i].ParameterType;
                var optionParamType = parameters[i].GetType();
                if (methodParamType.IsEnum && optionParamType == typeof(string))
                {
                    try
                    {
                        var parsedValue = Enum.Parse(methodParamType, parameters[i].ToString());
                        if (parsedValue is object)
                        {
                            parameters[i] = parsedValue;
                        }
                        else
                            useMethod = false;
                    }
                    catch
                    {
                        useMethod = false;
                    }
                }
                else if (methodParamType == typeof(Enum) && optionParamType.IsEnum)
                {
                    useMethod = true;
                }
                else
                {
                    useMethod = methodParamType == optionParamType || methodParamType.IsAssignableFrom(optionParamType);
                }
            }

            if (useMethod)
            {
                method = m;
                break;
            }
        }

        if (method is null)
        {
            throw new NullReferenceException("No method with the specified name was found!");
        }

        return method.Invoke(null, parameters);
    }

    protected void Initialize(ICakeContext context, string packageName, string packageVersion, string assemblyName = null)
    {
        if (string.IsNullOrEmpty(assemblyName))
        {
            assemblyName = packageName;
        }
        
        var assembly = LoadAddinAssembly(context, packageName, packageVersion, assemblyName);

        if (assembly is null)
        {
            return;
        }

        AddinAssembly = assembly;

        foreach (var ti in assembly.DefinedTypes.Where(ti => ti.IsPublic))
        {
            if (ti.IsEnum)
            {
                _declaredEnums.Add(ti.AsType());
            }
            else if(ti.IsClass && (!ti.IsAbstract || ti.IsStatic()) && !ti.IsGenericTypeDefinition)
            {
                _definedClasses.Add(ti);
                ParseClass(context, ti);
            }
        }
    }

    protected void ParseClass(ICakeContext context, TypeInfo classTypeInfo)
    {
        var aliases = new List<MethodInfo>();
        var methods = new List<MethodInfo>();

        foreach (var mi in classTypeInfo.DeclaredMethods.Where(i => i.IsPublic))
        {
            _definedMethods.Add(mi);
        }
    }

    private static Assembly LoadAddinAssembly(ICakeContext context, string packageName, string packageVersion, string assemblyName)
    {
        var dllPath = GetAssemblyPath(context, packageName, packageVersion, assemblyName);

        if (dllPath is null)
        {
            var script = $"#tool nuget:?package={packageName}&version={packageVersion}&prerelease";
            var tempFile = Guid.NewGuid() + ".cake";

            System.IO.File.WriteAllText(tempFile, script);

            try
            {
                context.CakeExecuteScript(tempFile);
            }
            finally
            {
                if (context.FileExists(tempFile))
                {
                    context.DeleteFile(tempFile);
                }
            }
        }

        dllPath = GetAssemblyPath(context, packageName, packageVersion, assemblyName);

        if (dllPath is null)
        {
            context.Warning("Unable to find path to the {0} package assembly!", packageName);
            return null;
        }

        var assembly = Assembly.LoadFrom(dllPath.FullPath);
        return assembly;
    }

    private static FilePath GetAssemblyPath(ICakeContext context, string packageName, string packageVersion, string assemblyName)
    {
        FilePath dllPath = null;
        const string pathFormat = "{0}.{1}/**/{2}*/{3}.dll";

        var possibleFrameworks = new List<String>();

        if (context.Environment.Runtime.IsCoreClr)
        {
            possibleFrameworks.Add("netcoreapp");
        }
        else
        {
            possibleFrameworks.Add("net4");
        }
        possibleFrameworks.Add("netstandard");

        foreach (var framework in possibleFrameworks)
        {
            dllPath = context.Tools.Resolve(string.Format(pathFormat, packageName, packageVersion, framework, assemblyName));
            if (dllPath is null)
                dllPath = context.Tools.Resolve(string.Format(pathFormat, packageName, "*", framework, assemblyName));
            if (dllPath is object)
                break;
        }

        return dllPath;
    }
}