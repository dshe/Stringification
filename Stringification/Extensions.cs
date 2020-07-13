using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;

namespace Stringification
{
    public static class Extensions
    {
        public static string Stringify(this object o, bool includeTypeName = true) =>
            Stringifier.Stringify(o, includeTypeName);

        public static IEnumerable<string> StringifyItems(this IEnumerable<object> source, bool includeTypeName = true) =>
            source.Select(o => o.Stringify(includeTypeName));

        public static IObservable<string> StringifyItems(this IObservable<object> source, bool includeTypeName = true) =>
            source.Select(o => o.Stringify(includeTypeName));

        public static string JoinStrings(this IEnumerable<string> strings, string separator = "") =>
            string.Join(separator, strings);
    }
}
