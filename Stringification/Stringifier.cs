using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System.Collections;
using System.Globalization;
using System.Reflection;
namespace Stringification;

public partial class Stringifier
{
    public static Stringifier Instance { get; } = new(NullLoggerFactory.Instance);
    private ILogger Logger { get; }
    public Stringifier(ILogger logger) => Logger = logger;
    public Stringifier(ILoggerFactory loggerFactory) => Logger = loggerFactory.CreateLogger<Stringifier>();

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

        if (o is DateTime dt)
            return $"\"{dt:O}\"";

        if (o is DateTimeOffset dto)
            return $"\"{dto:O}\"";

        if (o is ValueType)
            return Convert.ToString(o, CultureInfo.InvariantCulture) ?? "";

        string? str = o switch
        {
            string s => $"\"{s}\"",
            Type t => $"Type:\"{t.Name}\"",
            Exception e => $"Exception:\"{e.Message}\"",
            IEnumerable enumerable => StringifyEnumerable(enumerable, nonDefaultProperties),
            _ => null
        };

        if (str is not null)
            return str;

        Logger.LogTrace("class: {Name}", o.GetType().Name);

        TypeInfo type = o.GetType().GetTypeInfo();
        if (type.IsClass)
            return StringifyClass(o, nonDefaultProperties);

        throw new InvalidOperationException($"Unable to stringify type: {type.Name}.");
    }

    private string StringifyEnumerable(IEnumerable enumerable, bool nonDefaultProperties)
    {
        var items = enumerable
            .Cast<object>()
            .Select(x => Recurse(x, nonDefaultProperties))
            .Where(x => !string.IsNullOrEmpty(x));
        return "[" + string.Join(", ", items) + "]";
    }

    private string StringifyClass(object o, bool nonDefaultProperties)
    {
        var items = GetProperties(o, nonDefaultProperties)
            .Select(property => $"{property.Name}:{Recurse(property.GetValue(o), nonDefaultProperties)}")
            .ToList();

        if (items.Count == 0)
            return "";

        return "{" + string.Join(", ", items) + "}";
    }

}

