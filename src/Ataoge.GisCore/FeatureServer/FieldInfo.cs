using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Ataoge.GisCore.FeatureServer
{
    public class BaseFieldInfo
    {
        public string Name {get; set;}

        [JsonConverter(typeof(StringEnumConverter))]
        public EsriFieldType Type {get; set;} = EsriFieldType.esriFieldTypeString;

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