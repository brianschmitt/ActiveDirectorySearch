namespace ActiveDirectorySearch.Searches
{
    using ActiveDirectorySearch;

    public class FullUserDetail : BaseSearch, ISearch
    {
        public FullUserDetail()
            : base("Full User Detail", "Search Term")
        {
        }

        public void Search(IActiveDirectorySearcher searcher)
        {
            if (!IsValid())
            {
                return;
            }

            var searchTerm = Terms[0].Value;
            const bool recursive = false;
            var filter = string.Format("(&(objectCategory=user)(anr={0}))", searchTerm);
            var props = new string[]
            {
            };

            Results = Search(searcher, filter, props, recursive);
        }
    }
}
