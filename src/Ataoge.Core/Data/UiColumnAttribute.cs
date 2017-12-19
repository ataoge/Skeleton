using System;

namespace Ataoge.Data
{
    [AttributeUsage(AttributeTargets.Property)]
    public class UiColumnAttribute : Attribute
    {
        ///<summary>
        /// 数据库实体属性的名称
        ///</summary>
        public string EntityPropertyName {get; set;} = null;

        ///<summary>
        /// 数据库实体属性的值类型
        ///</summary>
        public string EntityPropertyValueType {get; set;} = null;

        public object EntityPropertyValue {get; set;} = null;
 

        public string DisplayName {get; set;}

        public string DefaultContent {get; set;}
        
        public bool Visible {get; set;} = true;

        public bool Orderable {get; set;} = false;

        public bool Searchable {get; set;} = false;

        public SearchMode SearchMode {get; set;}= SearchMode.NormalAnd;

        public bool Filterable {get; set;} = false;
        public FilterMode FilterMode {get; set;}= FilterMode.And;

        public string ReferField {get; set;} = null;

        public string ReferCondition {get; set;} = null;

        public string ReferValueFormatMethod {get; set;} = null;

        public int Weight {get; set;} = 0;

        public int Width {get; set;} = 0;

        

        public string FormatMethod {get; set;}

        public string OperationEvent {get; set;}

    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class UiClassColumnAttribute : UiColumnAttribute
    {
        public UiClassColumnAttribute()
        {
            base.Visible = false;
            base.Searchable = true;
            base.Filterable = true;
        }

        public string QueryKey {get; set;} = null;

    }
}