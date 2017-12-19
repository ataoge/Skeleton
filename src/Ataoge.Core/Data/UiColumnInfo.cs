using Ataoge.Utilities;
using JetBrains.Annotations;

namespace Ataoge.Data
{
    public class UiColumnInfo
    {
        public UiColumnInfo([NotNull] string propertyName)
        {
            this.PropertyName = propertyName;
            this.JsonPropertyName = propertyName.ToCamelCase();
        }

        public UiColumnInfo([NotNull] string propertyName, [NotNull] string jsonPropertyName)
        {
            this.PropertyName = propertyName;
            this.JsonPropertyName = jsonPropertyName;
        }

        public string JsonPropertyName {get;}

        public string PropertyName {get;}

        public string PropertyValueType {get; set;}

        public object PropertyValue {get; set;} = null;

        public string Key => CreateKey();

        private string CreateKey()
        {
            if (string.IsNullOrEmpty(this.JsonPropertyName))
                return StringUtils.NormalizeForKey(this.PropertyName);
            return StringUtils.NormalizeForKey(this.JsonPropertyName);
        }

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

        private string _displayName;
        public string DisplayName
        {
            get { return _displayName ?? PropertyName; } 
            set { _displayName = value;}
        }

        public int Weight {get; set;} = 0;

        public int Width {get; set;} = 0;

        public string FormatMethod {get; set;}

        public string OperationEvent {get; set;}
    }


    public enum SearchMode
    {
        Normal = 0x10,
        NormalAnd = 0x10,

        NormalOr = 0x11,

        Flag = 0x20,
        FlagAnd = 0x20,
        FlagOr = 0x21,

        IntRange = 0x32,
        DateTimeRange = 0x42,
        DateTimeYear = 0x43,
        DateTimeMonth = 0x44,
        DataTimeDay = 0x45,
        
        NumberRange = 0x52,

        StringValue = 0x64,
        
        StringLike = 0x68,

        Contains = 0x70,
        ContainsAnd = 0x70,

        ContainsOr = 0x71
    }

    public enum FilterMode
    {
        And = 0,
        Or = 1
    }
}