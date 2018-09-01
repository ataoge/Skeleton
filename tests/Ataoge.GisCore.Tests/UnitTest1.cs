 using System;
using Xunit;
using Newtonsoft.Json;
using System.Globalization;
using Ataoge.GisCore.Geometry;
using Ataoge.GisCore.Geometry.Converters;
using System.Collections.Generic;
using Ataoge.GisCore.Features;
using Newtonsoft.Json.Serialization;
using System.Reflection;
using System.Runtime.Serialization;

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

        [Fact]
        public void TestGeoJson()
        {
            Point pt1 = new Point(100,100);
            Point pt2 = new Point(200,200);
            Point pt3 = new Point(300,300);
            MultiPoint pts = new MultiPoint();
            pts.Geometries.Add(pt1);
            pts.Geometries.Add(pt2);
            pts.Geometries.Add(pt3);

            

            var testDto1 = new TestDto(){Id = 1, Geometry = pt1, Name = "MyName"};
            var feature1 = new Feature<TestDto>(testDto1);

            var testDto2 = new TestDto(){Id = 2, Geometry = pt2, Name = "MyName2"};
            var feature2 = new Feature<TestDto>(testDto2);
            JsonSerializerSettings geoJsonSetting = new JsonSerializerSettings() {
                ContractResolver = FeatuerGeometryResolver.Instance
                
            };
            //geoJsonSetting.Converters.Add(new GeometryConverter());

            //var jsonPt = JsonConvert.SerializeObject(pt1, geoJsonSetting);
        var features = new FeatureCollection<TestDto>(new List<Feature<TestDto>>() {feature1, feature2});
           var json = JsonConvert.SerializeObject(features, geoJsonSetting);
            

            LineString lineString = new LineString(pts.Geometries);
            var jsonLineString = JsonConvert.SerializeObject(lineString, new GeometryConverter());

            var externRing = new List<Point>() {
               new Point(100.0, 0.0), 
               new Point(101.0, 0.0), 
               new Point(101.0, 1.0), 
               new Point(100.0, 1.0),
               new Point(100.0, 0.0)
            };

             var holeRing = new List<Point>() {
               new Point(100.2, 0.2), 
               new Point(100.8, 0.2), 
               new Point(100.8, 0.8), 
               new Point(100.2, 0.8),
               new Point(100.2, 0.2)
            };
            
            Polygon polygon = new Polygon(externRing);
            polygon.InteriorRings.Add(new LinearRing(holeRing));
            var jsonPloygon = JsonConvert.SerializeObject(polygon, new GeometryConverter());
        }

        [Fact]
        public void TestJsonExpandData()
        {
            var jsonSettings = new JsonSerializerSettings()
            {
                ContractResolver = new UniversalEntitySerializeContractResolver()
            };
            var data = new TestExpandData() {Id = 1, Name ="My"};

            var json = JsonConvert.SerializeObject(data, jsonSettings);
            data["DateTime"] = DateTime.Now;
            data["Double"] = 22.223;
            json = JsonConvert.SerializeObject(data, jsonSettings);

            var data1 = JsonConvert.DeserializeObject<TestExpandData>(json);
            var aa = data1["DateTime"];
            var bb = data1["Double"];
        }
    }

    public class UniversalEntitySerializeContractResolver : CamelCasePropertyNamesContractResolver
    {
       //public static readonly UniversalEntitySerializeContractResolver Instance = new UniversalEntitySerializeContractResolver();
 
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
             JsonProperty property = base.CreateProperty(member, memberSerialization);
 
            if (member.Name == "DateTime") 
            {
                //property.PropertyName = Ataoge.Utilities.StringUtils.ToCamelCase(property.PropertyName);
            }
          return property;
        }
    }

        
    public class TestClass
    {
        public int Id {get; set;}

        public string Name {get; set;}

        public DateTime DateTime {get; set;}
    }


    public class TestExpandData
    {
        public TestExpandData()
        {
            ExtensionData = new Dictionary<string, object>();
        }

        public int Id {get; set;}

        public string Name {get; set;}

        public object this[string name]
        {
            get 
            {
                name = ToCamelCase(name);
                if (ExtensionData.ContainsKey(name))
                    return ExtensionData[name];
                return null;
            }
            set 
            {
                if (value != null)
                {
                    name = ToCamelCase(name);
                    ExtensionData[name] = value;
                }
            }
        }

        public static string ToCamelCase(string s)
        {
            if (string.IsNullOrEmpty(s) || !char.IsUpper(s[0]))
            {
                return s;
            }

            char[] chars = s.ToCharArray();
            chars[0] = char.ToLowerInvariant(chars[0]);
            return new string(chars);;
        }

        [JsonExtensionData]
        private IDictionary<string, object> ExtensionData {get; set;}

        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
        }
    }

    public class TestDto : IHasGeometryWithId
    {
        public int Id { get; set ; }
        public IGeometry Geometry { get; set; }

        public string Name {get; set;}
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
