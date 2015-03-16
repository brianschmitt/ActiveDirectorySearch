namespace ActiveDirectorySearch.Searches
{
    using ActiveDirectorySearch;

    public class UserDetail : BaseSearch, ISearch
    {
        public UserDetail()
            : base("User Detail", "Search Term")
        {
        }

        public void Search(IActiveDirectorySearcher searcher)
        {
            if (!IsValid())
            {
                return;
            }

            var searchTerm = Terms[0].Value;
            var filter = string.Format("(&(objectCategory=user)(anr={0}))", searchTerm);
            var props = new[]
            {
                "whenCreated",
                "whenChanged",
                "displayName",
                "title",
                "employeeNumber",
                "departmentnumber",
                "badPwdCount",
                "homeDirectory",
                "badPasswordTime",
                "lastLogon",
                "pwdLastSet",
                "accountExpires",
                "logonCount",
                "lockoutTime",
                "lastLogonTimestamp",
                "thumbnailPhoto"
            };
            const bool recursive = false;
            Results = Search(searcher, filter, props, recursive);
        }
    }
}
