namespace ActiveDirectorySearch.ActiveDirectorySearch
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class Extensions
    {
        private static bool ContainsIgnore(string source, string toCheck)
        {
            return source.IndexOf(toCheck, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private static bool AnyPropertyContains(object o, string filter)
        {
            return o.GetType()
                    .GetProperties()
                    .Select(prop => prop.GetValue(o, null).ToString())
                    .Any(val => ContainsIgnore(val, filter));
        }

        public static IEnumerable<dynamic> Filter(this IEnumerable<dynamic> source, string filter)
        {
            IEnumerable<dynamic> source2 = !string.IsNullOrEmpty(filter) ? source.Where(c => AnyPropertyContains(c, filter)) : source;

            return
                from r in source2
                select r;
        }
    }
}
