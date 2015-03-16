namespace ActiveDirectorySearch.ActiveDirectorySearch
{
    using System;
    using System.Collections.Generic;
    using System.DirectoryServices;

    public interface IActiveDirectorySearcher
    {
        event EventHandler BadLogOn;

        bool Invalid { get; }

        //SearchResult FindOne(string query, params string[] props);

        SearchResultCollection FindAll(string query, params string[] props);

        Dictionary<string, string> AttributeValuesMultiString(string attributeName, string objectDistinguishedName, Dictionary<string, string> valuesCollection, bool recursive);
    }
}
