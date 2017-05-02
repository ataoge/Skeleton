using System;

namespace Ataoge.Data
{
    [AttributeUsage(AttributeTargets.Property)]
    public class DbNavigationAttribute : Attribute
    {
        public DbNavigationAttribute(string name, string tableName)
            : this(name)
        {
            this.TableName = tableName.ToUpper();
        }

        public DbNavigationAttribute(string name)
        {
            this.Name = name.ToUpper();
        }

        public string Name {get;set;}

        public string TableName {get; set;}
    }
}