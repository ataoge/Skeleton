using Ataoge.GisCore.Geometry.Converters;
using Ataoge.GisCore.Geometry;
using Newtonsoft.Json;

namespace Ataoge.GisCore.Features
{
    public class Feature<TDto>
        where TDto : IHasGeometry
    {
        public Feature(TDto dto)
        {
            Properties = dto;
        }

        public int? Id 
        {
            get 
            {
                IHasGeometryWithId geo = Properties as IHasGeometryWithId;
                return geo?.Id;
            }
            set 
            {
                if (value.HasValue && Properties is IHasGeometryWithId)
                {
                    IHasGeometryWithId geo = Properties as IHasGeometryWithId;
                    geo.Id = value.Value;
                }
                
            }
        }

        [JsonProperty("type", Required = Required.Always)]
        public string Type {get; set;} = "Feature";

        [JsonProperty("geometry", Required = Required.Always)]
        [JsonConverter(typeof(GeometryConverter))]
        public IGeometry Geometry
        {
            get {return Properties.Geometry;}
            set {Properties.Geometry = value;}
        }

        [JsonProperty("properties", Required = Required.Always)]
        public TDto Properties {get; set;}

    }
}