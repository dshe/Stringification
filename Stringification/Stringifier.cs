using StringEnums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

#nullable enable

namespace Stringification
{
    public static class Stringifier
    {
        public static string Stringify(this object o, bool includeTypeName = true)
        {
            if (o == null)
                return "";
            if (o is string || o is Exception)
                return o.ToString();
            var result = Recurse(o);
            if (!includeTypeName)
                return result;
            return o.GetType().Name + ": " + (result ?? "{}");
        }

        private static string Recurse(this object o)
        {
            switch (o)
            {
                case null:
                    return "";
                case string s:
                    return $"\"{s}\"";
                case ValueType v:
                    return v.ToString();
                case Type t:
                    return $"Type:\"{t.Name}\"";
                case Exception e:
                    return $"\"{e.ToString()}\"";
                case IEnumerable enumerable:
                    return enumerable.StringifyEnumerable();
            }

            var type = o.GetType().GetTypeInfo();

            if (type.IsStringEnum())
                return o.ToString();

            if (type.IsClass)
                return o.StringifyClass();

            throw new Exception($"Unable to stringify type: {type.Name}.");
        }

        private static string StringifyEnumerable(this IEnumerable enumerable) =>
            enumerable
                .Cast<object>()
                .Select(Recurse)
                .Where(x => x != null)
                .Select(x => $"{string.Join(", ", x)}")
                .GroupBy(x => "")
                .Select(list => string.Join(", ", list))
                .Select(s => "[" + s + "]")
                .SingleOrDefault();

        private static string StringifyClass(this object o) =>
            Utilities.GetNonDefaultProperties(o)
                .Select(property => new { name = property.Name, value = property.GetValue(o).Recurse() })
                .Where(x => x.value != null)
                .Select(x => $"{x.name}:{x.value}")
                .GroupBy(x => "")
                .Select(list => string.Join(", ", list))
                .Select(s => "{" + s + "}")
                .SingleOrDefault();
    }
}

