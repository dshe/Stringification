using System.Reflection;

namespace Stringification;

public partial class Stringifier
{
    private static readonly Dictionary<TypeInfo, (object instance, PropertyInfo[] properties)> Cache = new();
    private (object instance, PropertyInfo[] properties) GetInstanceAndProperties(TypeInfo type)
    {
        lock (Cache)
        {
            if (!Cache.TryGetValue(type, out var item))
            {
                // create an instance of the type to find it's default properties
                item.instance = CreateInstance(type);
                item.properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .OrderBy(p => p.MetadataToken).ToArray();
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
            .Where(pvv => !DeepEquals(pvv.Item2, pvv.Item3))
            .Select(pvv => pvv.p);
    }

    private bool DeepEquals<T>(T instance1, T instance2)
    {
        string s1 = Recurse(instance1, false);
        string s2 = Recurse(instance2, false);
        return s1 == s2;
    }
}


