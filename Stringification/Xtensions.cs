namespace Stringification;

public static class ExtensionMethods
{
    public static string Stringify(this object obj, bool nonDefaultProperties = true, bool includeTypeName = true) =>
        Stringifier.Instance.Stringify(obj, nonDefaultProperties, includeTypeName);

    public static IEnumerable<string> StringifyItems(this IEnumerable<object> source, bool nonDefaultProperties = true, bool includeTypeName = true) =>
        source.Select(o => Stringifier.Instance.Stringify(o, nonDefaultProperties, includeTypeName));

    public static string JoinStrings(this IEnumerable<string> strings, string separator = "") =>
        string.Join(separator, strings);
}
