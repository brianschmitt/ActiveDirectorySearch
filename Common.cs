using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Reflection;
using ActiveDirectorySearch.ActiveDirectorySearch;
using ActiveDirectorySearch.Searches;

namespace ActiveDirectorySearch
{
    static class Common
    {
        private static IEnumerable<Type> GetObjectsByInterface<T>(Assembly assembly)
        {
            if (!typeof(T).IsInterface)
            {
                throw new ArgumentException("T must be an interface");
            }

            return assembly.GetTypes()
                .Where(x => x.GetInterface(typeof(T).Name) != null)
                .Select(x => x)
                .ToArray();
        }

        public static IEnumerable<ISearch> GetAvailableSearches()
        {
            var searchPlugins = GetObjectsByInterface<ISearch>(Assembly.GetExecutingAssembly())
                                        .Where(HasDefaultContructor)
                                        .Select(x => (ISearch)Activator.CreateInstance(x))
                                        .ToList();
            return searchPlugins;
        }

        private static bool HasDefaultContructor(Type item)
        {
            return item.GetConstructors().FirstOrDefault(x => x.GetParameters().Length == 0) != null;
        }

        public static List<string> GetDomains()
        {
            var list = new List<string>();
            var resultCollection = SearchActiveDirectoryForDomains();
            if (resultCollection == null) return list;
            var srDe = from SearchResult sr in resultCollection select sr.GetDirectoryEntry();
            var domains = from DirectoryEntry newEntry in srDe select newEntry.Properties["flatName"].Value.ToString();
            list.AddRange(domains.OrderBy(s => s));
            return list;
        }

        private static SearchResultCollection SearchActiveDirectoryForDomains()
        {
            var searcher = new AdSearcher(Environment.UserDomainName, Environment.UserName, string.Empty);
            var domains = searcher.FindAll("(&(objectCategory=trustedDomain)(name=*))", "flatName");
            return domains;
        }
    }
}
