using System;

namespace ActiveDirectorySearch.ActiveDirectorySearch
{
    public class Input<T>
    {
        public Input()
        {
            Id = Guid.NewGuid().ToString();
        }
        public bool Visible { get; set; }
        public string Label { get; set; }
        public T Value { get; set; }
        public string Id { get; private set; }
    }
}
