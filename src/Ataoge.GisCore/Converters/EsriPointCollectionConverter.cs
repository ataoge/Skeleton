using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Ataoge.GisCore.Geometry.Converters
{
    public class EsriPointCollectionConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(EsriPointCollection).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            EsriPointCollection pts = value as EsriPointCollection;
            writer.WriteStartArray();
            foreach(var point in pts)
            {
                WritePoint(writer,point);
            }
            writer.WriteEndArray();
        }

        private void WritePoint(JsonWriter writer, EsriPoint point)
        {
            
            if (point == null)
            {
                writer.WriteNull();
                return;
            }

            
            writer.WriteStartArray();
           
          
                writer.WriteValue(point.X.Value);
                writer.WriteValue(point.Y.Value);
            

            if (point.Z.HasValue)
            {
                writer.WriteValue(point.Z.Value);
            }

            
            if (point.M.HasValue)
            {
                writer.WriteValue(point.Z.Value);
            }

            writer.WriteEndArray();
        
        }
    }
}