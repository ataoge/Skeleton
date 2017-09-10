namespace Ataoge.Data
{
    public class UiColumnInfo
    {
        public UiColumnInfo(string propertyName)
        {

        }

        public string PropertyName {get;}

        public string Key => Normalize(this.PropertyName);

        protected virtual string Normalize(string name)
        {
            if (string.IsNullOrEmpty(name))
                return null;
            return name.ToLower();
        }

        public bool Visible {get; set;} = true;

        public bool Orderable {get; set;} = false;

        public bool Searchable {get; set;} = false;

        private string _displayName;
        public string DisplayName
        {
            get { return _displayName ?? PropertyName; } 
            set { _displayName = value;}
        }
    }
}