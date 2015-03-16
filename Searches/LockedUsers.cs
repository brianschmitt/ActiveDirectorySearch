namespace ActiveDirectorySearch.Searches
{
    using System.Collections.Generic;
    using ActiveDirectorySearch;

    public class LockedUsers : BaseSearch, ISearch
    {
        public LockedUsers()
            : base("Locked Users")
        {
        }

        public new List<Input<string>> Terms
        {
            get
            {
                return new List<Input<string>>();
            }
        }

        public new List<Input<bool>> Options
        {
            get
            {
                return new List<Input<bool>>();
            }
        }

        public void Search(IActiveDirectorySearcher searcher)
        {
            const string filter = "(&(objectCategory=person)(objectClass=user)(lockoutTime>=1))";
            var props = new[] { "Name" };
            Results = Search(searcher, filter, props, false);
        }

        public new bool IsValid()
        {
            return true;
        }
    }
}
