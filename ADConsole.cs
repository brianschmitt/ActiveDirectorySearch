using System;
using System.Linq;

namespace ActiveDirectorySearch
{
    using ActiveDirectorySearch;

    class AdConsole
    {
        public void Search(string searchType, params string[] searchTerm)
        {
            var searchPlugins = Common.GetAvailableSearches();
            var activeDirectorySearcher = new AdSearcher(Environment.UserDomainName, Environment.UserName, string.Empty);
            var searcher = searchPlugins.FirstOrDefault(s => s.Name == searchType);
            if (searcher == null)
            {
                return;
            }

            var index = 0;
            foreach (var item in searcher.Terms.Where(t => t.Visible = true))
            {
                
                item.Value = index > searchTerm.Length ? searchTerm.LastOrDefault() : searchTerm[index];
                index += 1;
            }

            searcher.Search(activeDirectorySearcher);
            var results = searcher.Results;

            if (results == null)
            {
                return;
            }

            var userDetails = results.GroupBy(s => s.Parent).Select(s => s.Key);

            foreach (var user in userDetails)
            {
                if (userDetails.Count() > 1)
                {
                    Console.WriteLine(user);
                }
                var user1 = user;
                var results2 = results
                    .Where(s => s.Parent == user1)
                    .Select(r => string.Format("{0}: {1} : {2}", r.PropertyName, r.CommonName, r.OrganizationalUnit));

                foreach (var res in results2)
                {
                    Console.WriteLine(res);
                }
            }
        }
    }
}
