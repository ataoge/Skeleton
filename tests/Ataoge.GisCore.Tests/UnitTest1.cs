using System;
using Xunit;
using Newtonsoft.Json;
using System.Globalization;

namespace Ataoge.GisCore.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
           
            TestSymbol test = new TestSymbol();
            test.Type = "abc";
            test.Color = new EsriColor() {R=255,G=118,B=8};
            string colorstring = JsonConvert.SerializeObject(test);
            //EsriColor color2 = JsonConvert.DeserializeObject<EsriColor>(colorstring);
        }
    }

    public class TestSymbol
    {
        public string Type {get; set;}
        public EsriColor Color {get; set;}

        
    }

    [JsonConverter(typeof(ColorIntArrayConverter))]
    public class EsriColor
    {
        public int R {get; set;}

        public int G {get; set;}

        public int B {get; set;}

        public int Alpha {get; set;}
    }

    public class ColorIntArrayConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            if (objectType == typeof(EsriColor))
                return true;
            return false;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
            {
                return null;
            }

            if  (reader.TokenType == JsonToken.StartArray)
            {
                int[] aa = new int[4];
                int i=0;
                while(reader.Read())
                {
                    switch (reader.TokenType)
                    {
                        case JsonToken.Integer:
                            aa[i] = Convert.ToInt32(reader.Value, CultureInfo.InvariantCulture);
                            i++;
                            break;
                        case JsonToken.EndArray:
                            
                            return new EsriColor() {R = aa[0], G= aa[1], B= aa[2], Alpha = aa[3]};
                        case JsonToken.Comment:
                            // skip
                            break;
                        default:
                            throw new Exception("EsriColor Json Read Error");
                    }

                }
            }
            throw new Exception("EsriColor Json Read Error");
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }

            EsriColor color = (EsriColor)value;
            int[] aa = new int[4]{color.R, color.G, color.B, color.Alpha};

            writer.WriteStartArray();
            
            writer.WriteValue(color.R);
            writer.WriteValue(color.G);
            writer.WriteValue(color.B);
            writer.WriteValue(color.Alpha);
            writer.WriteEndArray();
            
        }
    }
}
