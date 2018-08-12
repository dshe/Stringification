using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace Stringification
{
    public static class Instantiator
    {
        public static IEnumerable<PropertyInfo> GetNonDefaultProperties(object o)
        {
            var typeInfo = o.GetType().GetTypeInfo();

            // try to create an instance of the type to find it's default properties
            var instance = typeInfo.CreateInstance();

            return typeInfo
                .DeclaredProperties
                .Where(p => p.GetMethod.IsPublic)
                .Where(p => instance == null || !Equals(p.GetValue(o), p.GetValue(instance)))
                .OrderBy(p => p.MetadataToken);
        }

        public static object CreateInstance(this TypeInfo typeInfo)
        {
            try
            {
                return Instantiate(typeInfo);
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Instantiator could not instantiate type '{typeInfo.Name}'.", e);
                return null;
            }
        }

        private static object Instantiate(TypeInfo typeInfo)
        {
            if (typeInfo.AsType() == typeof(string))
                return "";

            if (typeInfo.IsValueType)
                return Activator.CreateInstance(typeInfo);

            // reference types
            var ctors = typeInfo.DeclaredConstructors.Where(c => !c.IsStatic).ToList();

            var ctor = ctors.SingleOrDefault(c => !c.GetParameters().Any());
            if (ctor != null) // ctor taking no arguments
                return ctor.Invoke(null);

            ctor = ctors.SingleOrDefault(c => c.GetParameters().All(p => p.HasDefaultValue));
            if (ctor != null) // ctor taking all defaults
                return ctor.Invoke(ctor.GetParameters().Select(p => p.DefaultValue).ToArray());

            Debug.WriteLine($"Instantiator could not instantiate type '{typeInfo.Name}'.");

            return null;
        }
    }
}


