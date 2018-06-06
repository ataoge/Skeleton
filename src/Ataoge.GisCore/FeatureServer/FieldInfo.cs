namespace Ataoge.GisCore.FeatureServer
{
    public class BaseFieldInfo
    {
        public string Name {get; set;}

        public string Type {get; set;} = "esriFieldTypeString";

        public string Alias {get; set;}

        public int Length {get; set;} = 0;
    }

     public class FieldInfo : BaseFieldInfo
    {
       
        

        public FieldDomain Domain {get; set;} = null;

        public bool Editable {get; set;} = true;

        public bool Nullable {get; set;} = true;

       

       // public object DefalutValue {get; set;} = null;

        //public string ModelName {get; set;}
    }
}