using Microsoft.Extensions.Logging.Abstractions;
using System.Collections.Generic;
using System.Linq;
namespace Stringification;

public static partial class Xtensions
{
    private static readonly Stringifier Stringifier = new(NullLogger.Instance);

    public static string Stringify(this object obj, bool nonDefaultProperties = true, bool includeTypeName = true) =>
        Stringifier.Stringify(obj, nonDefaultProperties, includeTypeName);

    public static IEnumerable<string> StringifyItems(this IEnumerable<object> source, bool nonDefaultProperties = true, bool includeTypeName = true) =>
        source.Select(o => Stringifier.Stringify(o, nonDefaultProperties, includeTypeName));

    public static string JoinStrings(this IEnumerable<string> strings, string separator = "") =>
        string.Join(separator, strings);
}
