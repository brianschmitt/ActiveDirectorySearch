using System;

namespace ActiveDirectorySearch.Searches
{
    using System.Collections.Generic;
    using System.DirectoryServices;
    using System.Linq;

    using ActiveDirectorySearch;

    public class BaseSearch
    {
        private readonly string _searchName;
        private readonly string _searchTermDisplay;

        private List<Input<string>> _searchTerms;
        private List<Input<bool>> _searchOptions;

        protected BaseSearch(string name)
        {
            Errors = new List<string>();
            _searchName = name;
        }

        protected BaseSearch(string name, string searchTerm)
        {
            Errors = new List<string>();
            _searchName = name;
            _searchTermDisplay = searchTerm;
        }

        public string Name
        {
            get
            {
                return _searchName;
            }
        }

        public List<string> Errors { get; private set; }

        public List<Input<bool>> Options
        {
            get
            {
                if (_searchOptions != null)
                {
                    return _searchOptions;
                }
                var input = new Input<bool> { Label = "Recurse", Visible = true, Value = false };
                _searchOptions = new List<Input<bool>> { input };

                return _searchOptions;
            }
        }

        public List<Input<string>> Terms
        {
            get
            {
                if (_searchTerms != null)
                {
                    return _searchTerms;
                }
                var input1 = new Input<string> { Label = _searchTermDisplay, Visible = true, Value = string.Empty};
                _searchTerms = new List<Input<string>> { input1 };

                return _searchTerms;
            }
        }

        public IEnumerable<dynamic> Results
        {
            get; protected set;
        }

        protected bool IsRequired(string searchValue)
        {
            if (!string.IsNullOrWhiteSpace(searchValue))
            {
                return true;
            }
            Errors.Add(Resources.ErrorRequired);
            return false;
        }

        protected bool MinLength(string searchValue)
        {
            if (searchValue.Length == 4 && searchValue.Contains("*"))
            {
                Errors.Add(Resources.ErrorGeneric);
                return false;
            }

            if (searchValue.Length >= 4)
            {
                return true;
            }
            Errors.Add(Resources.ErrorGeneric);
            return false;
        }

        protected bool NotEqual(string searchValue, string searchValue2)
        {
            if (searchValue == searchValue2)
            {
                return true;
            }
            Errors.Add(Resources.ErrorGeneric);
            return false;
        }

        protected bool NotBoth(bool searchValue, bool searchValue2)
        {
            if (!searchValue || !searchValue2)
            {
                return true;
            }
            Errors.Add(Resources.ErrorTooManyOptions);
            return false;
        }

        protected IEnumerable<dynamic> Search(IActiveDirectorySearcher searcher, string filter, string[] props, bool deepSearch)
        {
            var list = new List<dynamic>();
            var searchResultCollection = searcher.FindAll(filter, props);
            if (searchResultCollection == null)
            {
                return list;
            }
            list.AddRange(
                from SearchResult srchRslt in searchResultCollection
                from prop in srchRslt.Properties.PropertyNames.Cast<string>().Where(p => p != "adspath")
                let elementName = Converter.CleanCommonName(srchRslt.Path)
                from val in srchRslt.Properties[prop].Cast<object>()
                select new
                {
                    PropertyName = prop,
                    CommonName = Converter.CleanCommonName(val),
                    OrganizationalUnit = Converter.CleanOrganizationalUnit(val),
                    // prop == "memberof" ? Converter.CleanCommonName(val) : Converter.CleanOrganizationalUnit(val),
                    //ContainerName = prop == "memberof" ? Converter.CleanOrganizationalUnit(val) : prop,
                    Value = val,
                    Parent = elementName
                });

            if (!props.Contains("memberof") || !deepSearch)
            {
                return list;
            }
            var valuesCollection = new Dictionary<string, string>();
            var dictionary = searcher.AttributeValuesMultiString("memberof", searchResultCollection[0].Path, valuesCollection, true);
            foreach (var ele in from current in dictionary
                                let containerName = Converter.CleanOrganizationalUnit(current.Key)
                                let commonName = Converter.CleanCommonName(current.Value)
                                select new
                                {
                                    PropertyName = "memberof",
                                    CommonName = commonName,
                                    ContainerName = containerName,
                                    Value = current.Value,
                                    Parent = containerName,
                                }
                                    into ele
                                    where !list.Exists(e => e.ContainerName == ele.ContainerName && e.CommonName == ele.CommonName && e.PropertyName == ele.PropertyName)
                                    select ele)
            {
                list.Add(ele);
            }

            return list;
        }

        public bool IsValid()
        {
            var searchTerm = Terms[0].Value;
            Errors.Clear();

            var result = IsRequired(searchTerm) & MinLength(searchTerm);

            return result;
        }
    }
}
