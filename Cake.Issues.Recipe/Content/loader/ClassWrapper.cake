using System.Reflection;

public sealed class ClassWrapper
{
    private readonly Type _classType;
    private readonly object _concreteClass;
    private readonly AddinData _addinData;

    public ClassWrapper(object concreteClass, AddinData addinData)
    {
        var classType = concreteClass.GetType();

        if (!classType.IsClass)
        {
            throw new Exception($"The class {classType.Name} is not a class type!");
        }

        _concreteClass = concreteClass;
        _addinData = addinData;
        _classType = classType;
    }

    public object GetPropertyValue(string propertyName)
    {
        var property = _classType.GetProperty(propertyName, BindingFlags.GetProperty | BindingFlags.Public);

        return property.GetValue(_concreteClass);
    }

    public void SetPropertyValue(string propertyName, object value)
    {
        var property = _classType.GetProperty(propertyName, BindingFlags.SetProperty | BindingFlags.Public);
        value = _addinData.TransformParameters(value).First();
        if (property.PropertyType.IsEnum && !value.GetType().IsEnum)
        {
            value = Enum.Parse(property.PropertyType, value.ToString());
        }

        property.SetValue(_concreteClass, value);
    }

    public dynamic CallExtensionMethod(string methodName, params object[] parameters)
    {
        var newParameters = new List<object>();
        newParameters.Add(_concreteClass);
        newParameters.AddRange(_addinData.TransformParameters(parameters));

        return _addinData.CallStaticMethod(methodName, newParameters.ToArray());
    }

    public dynamic CallMethod(string methodName, params object[] parameters)
    {
        parameters = _addinData.TransformParameters(parameters);

        var methods = _classType.GetMethods(BindingFlags.Instance | BindingFlags.Public).Where(m => string.Compare(m.Name, methodName, StringComparison.OrdinalIgnoreCase) == 0);
        MethodInfo method = null;

        foreach (var m in methods.Where(m => m.GetParameters().Length == parameters.Length))
        {
            var methodParams = m.GetParameters();
            bool useMethod = AddinData.ParametersMatch(methodParams, parameters);

            if (useMethod)
            {
                method = m;
                break;
            }
        }

        if (method is null)
        {
            throw new NullReferenceException($"No method with the name '{methodName}' was found in the class '{_classType.Name}'");
        }

        var result = method.Invoke(_concreteClass, parameters);

        if (result.GetType().IsClass)
        {
            return new ClassWrapper(result, _addinData);
        }

        return result;
    }

    public object ToActual()
    {
        return _concreteClass;
    }
}