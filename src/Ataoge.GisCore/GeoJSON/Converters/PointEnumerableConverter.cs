using System;
using System.Collections.Generic;
using System.Linq;
using Ataoge.GisCore.GeoJSON.Geometry;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Ataoge.GisCore.GeoJSON.Converters
{
    /// <summary>
    /// Converter to read and write the <see cref="IEnumerable{Point}" /> type.
    /// </summary>
    public class PointEnumerableConverter : JsonConverter
    {
        private static readonly PositionConverter PositionConverter = new PositionConverter();
        /// <inheritdoc />
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value is IEnumerable<Point> points)
            {
                writer.WriteStartArray();
                foreach (var point in points)
                {
                    PositionConverter.WriteJson(writer, point.Coordinates, serializer);
                }
                writer.WriteEndArray();
            }
            else
            {
                throw new ArgumentException($"{nameof(PointEnumerableConverter)}: unsupported value {value}");
            }
        }

        /// <inheritdoc />
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var coordinates = existingValue as JArray ?? serializer.Deserialize<JArray>(reader);
            return coordinates.Select(position => new Point(position.ToObject<IEnumerable<double>>().ToPosition()));
        }

        /// <inheritdoc />
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(IEnumerable<Point>);
        }
    }
}