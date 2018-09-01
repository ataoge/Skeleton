using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Ataoge.GisCore.Geometry.Converters
{
    public class GeometryConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(Geometry).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Geometry geom = value as Geometry;
            if (geom == null)
            {
                writer.WriteNull();
                return;
            }

            writer.WriteStartObject(); 
            GeometryType geomType = ToGeoJsonObject(geom);
            writer.WritePropertyName("type");
            writer.WriteValue(Enum.GetName(typeof(GeometryType), geomType));

            switch (geomType)
            {
                case GeometryType.Point:
                    var point = geom as Point;
                    if (serializer.NullValueHandling == NullValueHandling.Include || !point.IsEmpty)
                    {
                        writer.WritePropertyName("coordinates");
                        //serializer.Serialize(writer, point);
                        WritePointCoordinates(writer, point);
                    }
                    break;
                case GeometryType.LineString:
                    var lineString = geom as LineString;
                    if (serializer.NullValueHandling == NullValueHandling.Include || !lineString.IsEmpty)
                    {
                        writer.WritePropertyName("coordinates");
                        //serializer.Serialize(writer, lineString.Points);
                        WritePointEnumCoordinates(writer, lineString.Points);
                    }
                    break;
                case GeometryType.MultiPoint:
                    var multiPoint = geom as MultiPoint;
                    if (serializer.NullValueHandling == NullValueHandling.Include || !multiPoint.IsEmpty)
                    {
                        writer.WritePropertyName("coordinates");
                        writer.WriteStartArray();
                        foreach(var pt in multiPoint.Geometries)
                        {
                            //serializer.Serialize(writer, pt);
                            WritePointCoordinates(writer, pt);
                        }
                        writer.WriteEndArray();
                    }
                    break;
                case GeometryType.MultiLineString:
                    var multiLineString = geom as MultiLineString;
                    if (serializer.NullValueHandling == NullValueHandling.Include || !multiLineString.IsEmpty)
                    {
                        writer.WritePropertyName("coordinates");
                        writer.WriteStartArray();
                        foreach(var ls in multiLineString.Geometries)
                        {
                            //serializer.Serialize(writer, ls.Points);
                            WritePointEnumCoordinates(writer, ls.Points);
                        }
                        writer.WriteEndArray();
                    }
                    break;
                case GeometryType.Polygon:
                    Polygon polygon = geom as Polygon;
                    if (serializer.NullValueHandling == NullValueHandling.Include || !polygon.IsEmpty)
                    {
                        writer.WritePropertyName("coordinates");
                        writer.WriteStartArray();
                        //serializer.Serialize(writer, polygon.ExteriorRing.Points);
                         WritePointEnumCoordinates(writer, polygon.ExteriorRing.Points);

                        foreach(var lineRing in polygon.InteriorRings)
                        {
                            //serializer.Serialize(writer, lineRing.Points);
                             WritePointEnumCoordinates(writer, lineRing.Points);
                        }

                        writer.WriteEndArray();
                    }
                    break;
                case GeometryType.MultiPolygon:
                    MultiPolygon multiPolygon = geom as MultiPolygon;
                    if (serializer.NullValueHandling == NullValueHandling.Include || !multiPolygon.IsEmpty)
                    {
                        writer.WritePropertyName("coordinates");
                        writer.WriteStartArray();

                        foreach(var thePolygon in multiPolygon.Geometries)
                        {
                            writer.WriteStartArray();
                            //serializer.Serialize(writer, thePolygon.ExteriorRing.Points);
                             WritePointEnumCoordinates(writer, thePolygon.ExteriorRing.Points);

                            foreach(var lineRing in thePolygon.InteriorRings)
                            {
                                //serializer.Serialize(writer, lineRing.Points);
                                 WritePointEnumCoordinates(writer, lineRing.Points);
                            }

                            writer.WriteEndArray();
                        }
                        writer.WriteEndArray();
                    }
                    break;
                case GeometryType.GeometryCollection:
                    GeometryCollection geometryCollection= geom as GeometryCollection;
                    if (serializer.NullValueHandling == NullValueHandling.Include || !geometryCollection.IsEmpty)
                    {
                        writer.WritePropertyName("geometries");
                        writer.WriteStartArray();
                        foreach(var geometry in geometryCollection.Geometries)
                        {
                            serializer.Serialize(writer, geometry);
                        }
                        writer.WriteEndArray();
                    }
                    break;
            }

            writer.WriteEndObject();
        }

        private void WritePointCoordinates(JsonWriter writer, Point point)
        {
            writer.WriteStartArray();
            if (!point.IsEmpty)
            {
                writer.WriteValue(point.X.Value);
                writer.WriteValue(point.Y.Value);
            }

            if (point.Z.HasValue)
            {
                writer.WriteValue(point.Z.Value);
            }

            writer.WriteEndArray();
        }

        private Point ReadPointCoordinates(JsonReader reader, Type objectType, object point, JsonSerializer serializer)
        {
            double[] coordinates;

            try
            {
                coordinates = serializer.Deserialize<double[]>(reader);
            }
            catch (Exception e)
            {
                throw new JsonReaderException("error parsing coordinates", e);
            }
            //给点赋值

            return null;
        }

        private void WritePointEnumCoordinates(JsonWriter writer, IEnumerable<Point> points)
        {
            writer.WriteStartArray();
            foreach (var point in points)
            {
                WritePointCoordinates(writer, point);
            }
            writer.WriteEndArray();
        }

        private IEnumerable<Point> PointEnumCoordinates(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var points = existingValue as JArray ?? serializer.Deserialize<JArray>(reader);
            return points.Select(pos => ReadPointCoordinates(pos.CreateReader(),
                typeof(Point),
                pos,
                serializer
            )).Cast<Point>();
        }

        private GeometryType ToGeoJsonObject(Geometry geom)
        {
            return geom.GeometryType;
        }

        private static GeometryType GetGeometryType(JsonReader reader)
        {
            return (GeometryType)Enum.Parse(typeof(GeometryType), (string)reader.Value, true);
        }
    
    }
}