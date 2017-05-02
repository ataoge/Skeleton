using System;

namespace Ataoge.Data
{
    [AttributeUsage(AttributeTargets.Property)]
    public class DbAutoCreatedAttribute : Attribute
    {
        public string PatternerName
        {
            get;
            set;
        }


        public string Format
        {
            get;
            set;
        }

        public bool LazyCreate
        {
            get;
            set;
        }
    }
}