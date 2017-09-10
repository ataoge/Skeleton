using System;

namespace Ataoge.Data
{
    [AttributeUsage(AttributeTargets.Property)]
    public class UiColumnAttribute : Attribute
    {
        public string DisplayName {get; set;}
        
        public bool Visible {get; set;} = true;

        public bool Orderable {get; set;} = false;

        public bool Searchable {get; set;} = false;

    }
}