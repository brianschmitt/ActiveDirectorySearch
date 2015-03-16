namespace ActiveDirectorySearch.Searches
{
    using ActiveDirectorySearch;

    public class GroupMember : BaseSearch, ISearch
    {
        public GroupMember()
            : base("Group Member Search", "Search Term")
        {
        }

        public void Search(IActiveDirectorySearcher searcher)
        {
            var searchTerm = Terms[0].Value;
            var recursive = Options[0].Value;
            var filter = string.Format("(&(anr={0}))", searchTerm);
            var props = new[] { "member", "memberof" };
            Results = Search(searcher, filter, props, recursive);
        }
    }
}
