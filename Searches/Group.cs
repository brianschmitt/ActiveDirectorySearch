namespace ActiveDirectorySearch.Searches
{
    using ActiveDirectorySearch;

    public class Group : BaseSearch, ISearch
    {
        public Group() : base("Group", "Group Name")
        {
        }

        public void Search(IActiveDirectorySearcher searcher)
        {
            var searchTerm = Terms[0].Value;
            var recursive = Options[0].Value;
            var filter = string.Format("(&(objectCategory=group)(sAMAccountName={0}))", searchTerm);
            var props = new[] { "member" };
            Results = Search(searcher, filter, props, recursive);
        }
    }
}
