using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace Stringification
{
    public partial class Stringifier
    {
        internal T CreateInstance<T>() => (T)CreateInstance(typeof(T).GetTypeInfo());

        internal object CreateInstance(TypeInfo type)
        {
            if (type == typeof(string))
                return "";

            if (type.IsValueType)
                return Activator.CreateInstance(type, true) ?? throw new Exception("Activator"); // ctor can be public or internal

            // reference types
            List<ConstructorInfo> ctors = type.DeclaredConstructors.Where(c => !c.IsStatic).ToList();

            // try ctor taking no arguments
            ConstructorInfo? ctor = ctors.SingleOrDefault(c => !c.GetParameters().Any());
            if (ctor != null)
            {
                Logger.LogInformation($"Creating instance of {type.Name} with parameterless constructor.");
                return ctor.Invoke(null);
            }

            // try ctor taking all defaults
            ctor = ctors.SingleOrDefault(c => c.GetParameters().All(p => p.HasDefaultValue));
            if (ctor != null) 
            {
                Logger.LogInformation($"Creating instance of {type.Name} with all-default parameters constructor.");
                return ctor.Invoke(ctor.GetParameters().Select(p => p.DefaultValue).ToArray());
            }

            Logger.LogInformation($"Creating instance of {type.Name} using FormatterServices.");
            object instance = FormatterServices.GetUninitializedObject(type);

            // try to initialize the object
            PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var property in properties)
            {
                TypeInfo propertyType = property.PropertyType.GetTypeInfo();
                object propertyInstance = CreateInstance(propertyType);

                if (property.CanWrite)
                {
                    property.SetValue(instance, propertyInstance);
                    continue;
                }

                FieldInfo? fieldInfo = type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic)
                    .FirstOrDefault(f => f.Name.StartsWith($"<{property.Name}>"));

                if (fieldInfo != null)
                {
                    fieldInfo.SetValue(instance, propertyInstance);
                    continue;
                }
                Logger.LogInformation($"Unable to create instance of readonly property '{property.Name}'.");
            }

            return instance;
        }
    }
}


