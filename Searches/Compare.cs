namespace ActiveDirectorySearch.Searches
{
    using System.Collections.Generic;
    using System.Linq;

    using ActiveDirectorySearch;

    public class Compare : BaseSearch, ISearch
    {
        private ISearch subsearch1;
        private ISearch subsearch2;

        private List<Input<string>> _searchTerms;
        private List<Input<bool>> _searchOptions;

        public Compare()
            : base("Comparison Search")
        {
            subsearch1 = new GroupMember();
            subsearch2 = new GroupMember();
        }

        public new List<Input<string>> Terms
        {
            get
            {
                if (_searchTerms != null)
                {
                    return _searchTerms;
                }

                _searchTerms = new List<Input<string>>();

                _searchTerms.AddRange(subsearch1.Terms);
                _searchTerms.AddRange(subsearch2.Terms);

                return _searchTerms;
            }
        }

        public new List<Input<bool>> Options
        {
            get
            {
                if (_searchOptions != null)
                {
                    return _searchOptions;
                }

                var input1 = new Input<bool> { Label = "Only Where Same", Visible = true, Value = false };
                var input2 = new Input<bool> { Label = "Only Where Different", Visible = true, Value = false };
                _searchOptions = new List<Input<bool>> { input1, input2 };

                return _searchOptions;
            }
        }

        public void Search(IActiveDirectorySearcher searcher)
        {
            subsearch1.Terms[0].Value = Terms[0].Value;
            subsearch1.Search(searcher);

            subsearch2.Terms[0].Value = Terms[1].Value;
            subsearch2.Search(searcher);

            var results1 = subsearch1.Results;
            var results2 = subsearch2.Results;

            var leftOuter = from r1 in results1
                            join r2 in results2 on r1.CommonName equals r2.CommonName into outer
                            from o in outer.DefaultIfEmpty()
                            select new
                            {
                                CommonName = r1.Parent,
                                CommonName2 = (o == null) ? string.Empty : o.Parent,
                                ContainerName = r1.CommonName
                            };

            var rightOuter = from r2 in results2
                             join r1 in results1 on r2.CommonName equals r1.CommonName into outer
                             from o in outer.DefaultIfEmpty()
                             select new
                             {
                                 CommonName = (o == null) ? string.Empty : o.Parent,
                                 CommonName2 = r2.Parent,
                                 ContainerName = r2.CommonName
                             };

            var subResults = rightOuter.Union(leftOuter);

            if (Options[0].Value)
            {
                // Where Same
                Results = subResults.Where(r =>
                    !string.IsNullOrWhiteSpace(r.CommonName) && !string.IsNullOrWhiteSpace(r.CommonName2));
            }
            else if (Options[1].Value)
            {
                // Where Different
                Results = subResults.Where(r =>
                    (!string.IsNullOrWhiteSpace(r.CommonName) && string.IsNullOrWhiteSpace(r.CommonName2))
                    ||
                    (string.IsNullOrWhiteSpace(r.CommonName) && !string.IsNullOrWhiteSpace(r.CommonName2)));
            }
            else
            {
                Results = subResults;
            }
        }

        public new bool IsValid()
        {
            Errors.Clear();
            var searchTerm1 = Terms[0].Value;
            var searchTerm2 = Terms[0].Value;

            var result = IsRequired(searchTerm1);
            result &= MinLength(searchTerm1);

            result &= IsRequired(searchTerm2);
            result &= MinLength(searchTerm2);

            result &= NotEqual(searchTerm1, searchTerm2);

            result &= NotBoth(Options[0].Value, Options[1].Value);

            return result;
        }
    }
}