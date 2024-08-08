using System.Reflection;

namespace Stringification;

public partial class Stringifier
{
    private static readonly Dictionary<TypeInfo, (object instance, PropertyInfo[] properties)> Cache = [];
    private (object instance, PropertyInfo[] properties) GetInstanceAndProperties(TypeInfo type)
    {
        lock (Cache)
        {
            if (!Cache.TryGetValue(type, out var item))
            {
                // create an instance of the type to find it's default properties
                item.instance = CreateInstance(type);
                item.properties = [.. type.GetProperties(BindingFlags.Public | BindingFlags.Instance).OrderBy(p => p.MetadataToken)];
                Cache.Add(type, item);
            }
            return item;
        }
    }

    private IEnumerable<PropertyInfo> GetProperties(object o, bool nonDefault)
    {
        ArgumentNullException.ThrowIfNull(o);

        TypeInfo type = o.GetType().GetTypeInfo();
        (object instance, PropertyInfo[] properties) = GetInstanceAndProperties(type);

        if (!nonDefault)
            return properties;

        return properties
            .Select(p => (p, p.GetValue(o), p.GetValue(instance)))
            .Where(pvv => !DeepEquals(pvv.p, pvv.Item2, pvv.Item3))
            .Select(pvv => pvv.p);
    }

    private bool DeepEquals(PropertyInfo pi, object? instance1, object? instance2)
    {
        if (pi.PropertyType == typeof(string))
        {
            var x1 = (string?)instance1;
            var x2 = (string?)instance2;
            return (x1 == x2 || (string.IsNullOrEmpty(x1) && string.IsNullOrEmpty(x2)));
        }   

        string s1 = Recurse(instance1, false);
        string s2 = Recurse(instance2, false);

        return s1 == s2;
    }
}


