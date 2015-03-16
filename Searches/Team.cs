namespace ActiveDirectorySearch.Searches
{
    using ActiveDirectorySearch;
    using System.Collections.Generic;
    using System.Linq;

    public class Team : BaseSearch, ISearch
    {
        public Team()
            : base("Team", "Search Term")
        {
        }

        public void Search(IActiveDirectorySearcher searcher)
        {
            if (!IsValid())
            {
                return;
            }

            var results = new List<dynamic>();

            var filter1 = new GroupMember();
            filter1.Terms[0].Value = Terms[0].Value;
            filter1.Search(searcher);

            foreach (var mem in filter1.Results.Where(f => f.PropertyName == "member"))
            {
                var filter = string.Format("(&(objectCategory=user)(anr={0}))", mem.CommonName);
                var props = new[]
                    {
                        "displayName",
                        "title",
                        "departmentnumber"
                    };
                const bool recursive = false;
                var subResults = this.Search(searcher, filter, props, recursive);

                results.AddRange(subResults);
            }

            Results = results
                        .GroupBy(c => c.Parent)
                        .Select(g => new 
                            {
                                User = g.Key,
                                Title = g.Where(c => c.PropertyName == "title").Select(d => d.Value).First().ToString(),
                                Department = g.Where(c => c.PropertyName == "departmentnumber").Select(d => d.Value).First().ToString()
                            });
        }
    }
}
