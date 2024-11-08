﻿using Microsoft.Extensions.Logging;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace Stringification;

public partial class Stringifier
{
    public T CreateInstance<T>() => (T)CreateInstance(typeof(T).GetTypeInfo());

    public object CreateInstance(TypeInfo type)
    {
        ArgumentNullException.ThrowIfNull(type);
        if (type == typeof(string))
            return "";
        if (type.IsValueType)
            return Activator.CreateInstance(type, true) ?? throw new InvalidOperationException("Activator"); // ctor can be public or internal

        // reference types
        List<ConstructorInfo> ctors = type.DeclaredConstructors.Where(c => !c.IsStatic).ToList();

        // try ctor taking no arguments
        ConstructorInfo? ctor = ctors.SingleOrDefault(c => c.GetParameters().Length == 0);
        if (ctor != null)
        {
            Logger.LogTrace("Creating instance of {Name} with parameterless constructor.", type.Name);
            return ctor.Invoke(null);
        }

        // try ctor taking all defaults
        ctor = ctors.SingleOrDefault(c => c.GetParameters().All(p => p.HasDefaultValue));
        if (ctor != null)
        {
            Logger.LogTrace("Creating instance of {Name} with all-default parameters constructor.", type.Name);
            return ctor.Invoke(ctor.GetParameters().Select(p => p.DefaultValue).ToArray());
        }

        Logger.LogTrace("Creating instance of {Name} using FormatterServices.", type.Name);
        object instance = RuntimeHelpers.GetUninitializedObject(type);

        // try to initialize the object
        PropertyInfo[] properties = type.GetProperties(BindingFlags.Instance);
        foreach (PropertyInfo property in properties)
        {
            TypeInfo propertyType = property.PropertyType.GetTypeInfo();
            object propertyInstance = CreateInstance(propertyType);

            if (property.CanWrite)
            {
                property.SetValue(instance, propertyInstance);
                continue;
            }

            FieldInfo? fieldInfo = type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic)
                .FirstOrDefault(f => f.Name.StartsWith($"<{property.Name}>", StringComparison.Ordinal));

            if (fieldInfo != null)
            {
                fieldInfo.SetValue(instance, propertyInstance);
                continue;
            }

            Logger.LogWarning("Unable to create instance of readonly property '{Name}'.", property.Name);
        }

        return instance;
    }
}
