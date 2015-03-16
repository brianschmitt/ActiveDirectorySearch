namespace ActiveDirectorySearch.ActiveDirectorySearch
{
    using System;
    using System.Collections.Generic;
    using System.DirectoryServices;

    public class AdSearcher : IActiveDirectorySearcher
    {
        private readonly string _domain;
        private readonly string _userName;
        private readonly string _password;

        public AdSearcher(string activeDirectoryDomain, string activeDirectoryUserName, string activeDirectoryPassword)
        {
            _domain = activeDirectoryDomain;
            _userName = activeDirectoryUserName;
            _password = activeDirectoryPassword;
        }

        public event EventHandler BadLogOn;

        public bool Invalid
        {
            get;
            private set;
        }

        //public SearchResult FindOne(string query, params string[] props)
        //{
        //    SearchResult result;
        //    if (Invalid)
        //    {
        //        return null;
        //    }

        //    var directoryEntry = GetDirectoryEntry();
        //    try
        //    {
        //        using (var directorySearcher = new DirectorySearcher())
        //        {
        //            directorySearcher.SearchRoot = directoryEntry;
        //            foreach (var value in props)
        //            {
        //                directorySearcher.PropertiesToLoad.Add(value);
        //            }

        //            directorySearcher.Filter = query;
        //            directorySearcher.CacheResults = true;
        //            directorySearcher.ReferralChasing = ReferralChasingOption.None;
        //            result = directorySearcher.FindOne();
        //        }
        //    }
        //    catch
        //    {
        //        if (BadLogOn != null)
        //        {
        //            BadLogOn(this, null);
        //        }

        //        Invalid = true;
        //        result = null;
        //    }

        //    return result;
        //}

        public SearchResultCollection FindAll(string query, params string[] props)
        {
            SearchResultCollection result;

            if (Invalid)
            {
                return null;
            }

            var directoryEntry = GetDirectoryEntry();
            try
            {
                using (var directorySearcher = new DirectorySearcher())
                {
                    directorySearcher.SearchRoot = directoryEntry;
                    foreach (var value in props)
                    {
                        directorySearcher.PropertiesToLoad.Add(value);
                    }

                    directorySearcher.Filter = query;
                    directorySearcher.CacheResults = true;
                    directorySearcher.ReferralChasing = ReferralChasingOption.None;
                    result = directorySearcher.FindAll();
                }
            }
            catch
            {
                if (BadLogOn != null)
                {
                    BadLogOn(this, null);
                }

                Invalid = true;
                result = null;
            }

            return result;
        }

        public Dictionary<string, string> AttributeValuesMultiString(string attributeName, string objectDistinguishedName, Dictionary<string, string> valuesCollection, bool recursive)
        {
            using (var directoryEntry = new DirectoryEntry(objectDistinguishedName))
            {
                var propertyValueCollection = directoryEntry.Properties[attributeName];
                var enumerator = propertyValueCollection.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    if (enumerator.Current == null || valuesCollection.ContainsKey(enumerator.Current.ToString()))
                    {
                        continue;
                    }

                    valuesCollection.Add(enumerator.Current.ToString(), objectDistinguishedName);
                    if (recursive)
                    {
                        AttributeValuesMultiString(attributeName, "LDAP://" + enumerator.Current, valuesCollection, true);
                    }
                }
            }

            return valuesCollection;
        }

        private DirectoryEntry GetDirectoryEntry()
        {
            var defaultDomain = Environment.UserDomainName;
            return (_domain == defaultDomain)
                    ? new DirectoryEntry("LDAP://" + defaultDomain)
                    : new DirectoryEntry("LDAP://" + _domain, _domain + "\\" + _userName, _password);
        }
    }
}