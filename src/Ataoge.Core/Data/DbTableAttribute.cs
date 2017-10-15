using System;

namespace Ataoge.Data
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public class DbTableAttribute : Attribute 
    {
        public DbTableAttribute(string tableName)
        {
            this.tableName = tableName.ToUpper();
        } 

        string tableName;

        /// <summary>
        /// 表名
        /// </summary>
        public string TableName 
        {
            get {return this.tableName;}
            set {this.tableName = value;}
        }

        string aliasName;
        /// <summary>
        /// 表别名
        /// </summary>
        public string AliasName
        {
            get { return this.aliasName; }
            set { this.aliasName = value; }
        }

        string schemeName;

        public string SchemeName
        {
            get { return this.schemeName;}
            set { this.schemeName = value;}
        }

        public Type RepostitoryInterface
        {
            get;
            set;
        }
    }
}