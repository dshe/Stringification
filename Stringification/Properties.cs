using System.Reflection;
namespace Stringification;

// Stringification does not support cyclic object graphs.
// Attempting to stringify objects containing reference cycles will result in unbounded recursion.

public partial class Stringifier
{
    private static readonly Dictionary<TypeInfo, (object instance, PropertyInfo[] properties)> _cache = [];
    private (object instance, PropertyInfo[] properties) GetInstanceAndProperties(TypeInfo type)
    {
        lock (_cache)
        {
            if (!_cache.TryGetValue(type, out var item))
            {
                // create an instance of the type to find it's default properties
                item.instance = CreateInstance(type);
                item.properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .OrderBy(p => p.MetadataToken)
                    .ToArray();
                _cache.Add(type, item);
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
            .Select(p => (p, GetPropertyValueSafe(p, o), GetPropertyValueSafe(p, instance)))
            .Where(pvv => !DeepEquals(pvv.p, pvv.Item2, pvv.Item3))
            .Select(pvv => pvv.p);
    }

    private bool DeepEquals(PropertyInfo pi, object? instance1, object? instance2)
    {
        if (pi.PropertyType == typeof(string))
        {
            string? x1 = (string?)instance1;
            string? x2 = (string?)instance2;
            return (x1 == x2 || (string.IsNullOrEmpty(x1) && string.IsNullOrEmpty(x2)));
        }   

        string s1 = Recurse(instance1, false);
        string s2 = Recurse(instance2, false);

        return s1 == s2;
    }

    private static object? GetPropertyValueSafe(PropertyInfo p, object? instance)
    {
        try
        {
            return p.GetValue(instance);
        }
#pragma warning disable CA1031
        catch
        {
            // If getter throws for the default instance, treat as null so comparison can continue safely
            return null;
        }
    }
}


