using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

#nullable enable

namespace Stringification
{
    public static class Utilities
    {
        public static IEnumerable<PropertyInfo> GetNonDefaultProperties(object o)
        {
            if (o == null)
                throw new ArgumentNullException(nameof(o));

            var type = o.GetType().GetTypeInfo();

            // try to create an instance of the type to find it's default properties
            var instance = CreateInstance(type);

            // find all public properties
            return type
                .DeclaredProperties
                .Where(p => p.GetMethod.IsPublic)
                .Where(p => instance == null || !Equals(p.GetValue(o), p.GetValue(instance)))
                .OrderBy(p => p.MetadataToken);
        }

        public static object CreateInstance(TypeInfo type)
        {
            try
            {
                return TryCreateInstance(type);
            }
            catch (Exception e)
            {
                throw new Exception($"Could not create an instance of type '{type.Name}'.", e);
            }
        }

        private static object TryCreateInstance(TypeInfo type)
        {
            if (type == typeof(string))
                return "";

            if (type.IsValueType)
                return Activator.CreateInstance(type, true); // ctor can be public or internal

            // reference types
            var ctors = type.DeclaredConstructors.Where(c => !c.IsStatic).ToList();

            var ctor = ctors.SingleOrDefault(c => !c.GetParameters().Any());
            if (ctor != null) // ctor taking no arguments
            {
                Debug.WriteLine($"Creating instance of {type.Name} with parameterless constructor.");
                return ctor.Invoke(null);
            }

            ctor = ctors.SingleOrDefault(c => c.GetParameters().All(p => p.HasDefaultValue));
            if (ctor != null) // ctor taking all defaults
            {
                Debug.WriteLine($"Creating instance of {type.Name} with all-default parameters constructor.");
                return ctor.Invoke(ctor.GetParameters().Select(p => p.DefaultValue).ToArray());
            }

            Debug.WriteLine($"Creating instance of {type.Name} using FormatterServices.");
            return FormatterServices.GetUninitializedObject(type);
        }
    }
}


