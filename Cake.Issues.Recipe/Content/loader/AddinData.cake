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

    public ClassWrapper CreateClass(string classTypeString, params object[] parameters)
    {
        var possibleClass = _definedClasses.FirstOrDefault(c => string.Compare(c.Name, classTypeString, StringComparison.OrdinalIgnoreCase) == 0);

        if (possibleClass is null)
        {
            throw new NullReferenceException($"No loaded class named {classTypeString} was found in this assembly.");
        }

        return CreateClass(possibleClass, parameters);
    }

    public ClassWrapper CreateClass(TypeInfo classType, params object[] parameters)
    {
        parameters = parameters ?? new object[0];
        var constructors = classType.DeclaredConstructors.Where(c => c.IsPublic && !c.IsStatic && c.GetParameters().Length == parameters.Length);
        ConstructorInfo constructor = null;

        foreach (var ctx in constructors)
        {
            var ctxParams = ctx.GetParameters();
            bool useCtx = ParametersMatch(ctxParams, parameters);

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

        var result = constructor.Invoke(parameters ?? new object[0]);

        return new ClassWrapper(result, this);
    }

    public TType CallStaticMethod<TType>(string methodName, params object[] parameters)
    {
        var result = CallStaticMethod(methodName, parameters);

        if (result.GetType().IsClass)
        {
            return (TType)result.ToActual();
        }

        return (TType)result;
    }

    public dynamic CallStaticMethod(string methodName, params object[] parameters)
    {
        parameters = TransformParameters(parameters);

        var methods = this._definedMethods.Where(m => m.IsPublic && m.IsStatic && string.Compare(m.Name, methodName, StringComparison.OrdinalIgnoreCase) == 0);
        MethodInfo method = null;

        foreach (var m in methods.Where(m => m.GetParameters().Length == parameters.Length))
        {
            var methodParams = m.GetParameters();
            bool useMethod = ParametersMatch(methodParams, parameters);

            if (useMethod)
            {
                method = m;
                break;
            }
        }

        if (method is null)
        {
            throw new NullReferenceException($"No method with the name '{methodName}' was found!");
        }

        var result = method.Invoke(null, parameters);

        if (result.GetType().IsClass)
        {
            return new ClassWrapper(result, this);
        }

        return result;
    }

    public object[] TransformParameters(params object[] parameters)
    {
        var newParameters = new List<object>();
        if (parameters is null)
        {
            return newParameters.ToArray();
        }

        foreach (var parameter in parameters)
        {
            object value = parameter;
            if (parameter is string sParam)
            {
                int index = sParam.IndexOf('.');
                if (index >= 0)
                {
                    var enumOrClass = sParam.Substring(0, index);
                    var subValue = sParam.Substring(index+1);
                    var enumType = _declaredEnums.FirstOrDefault(e => string.Compare(e.Name, enumOrClass, StringComparison.OrdinalIgnoreCase) == 0);
                    var classType = _definedClasses.FirstOrDefault(c => string.Compare(c.Name, enumOrClass, StringComparison.OrdinalIgnoreCase) == 0);
                    if (enumType is object)
                    {
                        value = Enum.Parse(enumType, subValue);
                    }
                    else if (classType is object)
                    {
                        var property = classType.GetProperty(subValue, BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.Static);
                        value = property.GetValue(null);
                    }
                }
            }
            else if (parameter is ClassWrapper wrapper)
            {
                value = wrapper.ToActual();
            }

            newParameters.Add(value);
        }

        return newParameters.ToArray();
    }

    public static bool ParametersMatch(ParameterInfo[] methodParameters, object[] parameters)
    {
        bool useMethod = true;
        for (int i = 0; i < methodParameters.Length && useMethod; i++)
        {
            var methodParamType = methodParameters[i].ParameterType;
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

        return useMethod;
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