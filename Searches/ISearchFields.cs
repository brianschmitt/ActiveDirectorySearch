namespace ActiveDirectorySearch.Searches
{
    using System.Collections.Generic;

    using ActiveDirectorySearch;

    public interface ISearch
    {
        string Name { get; }

        List<Input<string>> Terms { get; }

        List<Input<bool>> Options { get; }

        IEnumerable<dynamic> Results { get; }

        List<string> Errors { get; }

        void Search(IActiveDirectorySearcher searcher);

        bool IsValid();
    }
}
