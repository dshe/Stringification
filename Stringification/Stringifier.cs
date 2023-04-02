using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System.Collections;
using System.Reflection;

namespace Stringification;

public partial class Stringifier
{
    private ILogger Logger { get; }
    public Stringifier(ILogger logger) => Logger = logger;
    private Stringifier() => Logger = NullLogger.Instance;
    public static Stringifier Instance { get; } = new();

    public string Stringify(object source, bool nonDefaultProperties = true, bool includeTypeName = true)
    {
        ArgumentNullException.ThrowIfNull(source);

        Logger.LogTrace("Stringify: {Name}", source.GetType().Name);

        string result = Recurse(source, nonDefaultProperties);

        if (includeTypeName)
            result = $"{source.GetType().Name}: {(result.Length == 0 ? "{}" : result)}";

        Logger.LogTrace("Stringify({Name}) => {Result}", source.GetType().Name, result);

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
        if (type.IsGenericType)
            return "";

        Logger.LogTrace("class: {Name}", o.GetType().Name);

        if (type.IsClass)
            return StringifyClass(o, nonDefaultProperties);

        throw new InvalidOperationException($"Unable to stringify type: {type.Name}.");
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
            .SingleOrDefault("");

    private string StringifyClass(object o, bool nonDefaultProperties)
    {
        return GetProperties(o, nonDefaultProperties)
            .Select(property => new { name = property.Name, value = Recurse(property.GetValue(o), nonDefaultProperties) })
            .Select(x => $"{x.name}:{x.value}")
            .GroupBy(x => "")
            .Select(list => string.Join(", ", list))
            .Select(s => "{" + s + "}")
            .SingleOrDefault("");
    }
}

