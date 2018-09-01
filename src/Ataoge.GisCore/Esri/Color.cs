using Ataoge.GisCore.Converters;
using Newtonsoft.Json;

namespace Ataoge.GisCore
{
    [JsonConverter(typeof(EsriColorConverter))]
    public class EsriColor
    {
        public int R {get; set;}

        public int G {get; set;}

        public int B {get; set;}

        public int Alpha {get; set;}
    }
}