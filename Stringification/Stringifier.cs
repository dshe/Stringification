using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using StringEnums;
using System;
using System.Collections;
using System.Linq;
using System.Reflection;

namespace Stringification
{
    public partial class Stringifier
    {
        private readonly ILogger Logger;
        public Stringifier(ILogger logger) => Logger = logger;

        public string Stringify(object source, bool nonDefaultProperties = true, bool includeTypeName = true)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            string result = Recurse(source, nonDefaultProperties);

            if (includeTypeName)
                result = $"{source.GetType().Name}: {(result == "" ? "{}" : result)}";

            Logger.LogTrace($"Stringify({source.GetType().Name}) => {result}");

            return result;
        }

        private string Recurse(object? o, bool nonDefaultProperties)
        {
            if (o is null)
                return "";

            string? str = o switch
            {
                string s => $"\"{s}\"",
                ValueType v => v.ToString() ?? "",
                Type t => $"Type:\"{t.Name}\"",
                Exception e => $"Exception:\"{e.Message}\"",
                IEnumerable enumerable => StringifyEnumerable(enumerable, nonDefaultProperties),
                _ => null
            };

            if (str is not null)
                return str;

            TypeInfo type = o.GetType().GetTypeInfo();

            if (type.IsStringEnum())
                return o.ToString() ?? "";

            if (type.IsClass)
                return StringifyClass(o, nonDefaultProperties);

            throw new Exception($"Unable to stringify type: {type.Name}.");
        }

        private string StringifyEnumerable(IEnumerable enumerable, bool nonDefaultProperties) =>
            enumerable
                .Cast<object>()
                .Select(x => Recurse(x, nonDefaultProperties))
                .Where(x => !string.IsNullOrEmpty(x))
                .Select(x => $"{string.Join(", ", x)}")
                .GroupBy(x => "")
                .Select(list => string.Join(", ", list))
                .Select(s => "[" + s + "]")
                .SingleOrDefault() ?? "";

        private string StringifyClass(object o, bool nonDefaultProperties)
       {
            return GetProperties(o, nonDefaultProperties)
                .Select(property => new { name = property.Name, value = Recurse(property.GetValue(o), nonDefaultProperties) })
                .Select(x => $"{x.name}:{x.value}")
                .GroupBy(x => "")
                .Select(list => string.Join(", ", list))
                .Select(s => "{" + s + "}")
                .SingleOrDefault() ?? "";
        }
    }
}

