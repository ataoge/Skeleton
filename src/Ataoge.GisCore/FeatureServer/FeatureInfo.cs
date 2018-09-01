using System.Collections.Generic;
using Ataoge.GisCore.Geometry;
using Newtonsoft.Json;

namespace Ataoge.GisCore.FeatureServer
{

    public interface IFeatureInfo
    {
        
    }

    public class FeatureInfo : IFeatureInfo
    {
        public FeatureInfo()
        {
            Attributes = new Dictionary<string, object>();
        }

        public IDictionary<string, object> Attributes {get; set;}

        public IGeometry Geometry {get; set;}

        
    }

    public interface IEsriTable : IHasGeometry
    {
        int Id {get; set;}
        string GlobalId {get; set;}

    }

    public class FeatureInfo<TEsriTable> : IFeatureInfo
        where TEsriTable : IEsriTable
    {
        public FeatureInfo(TEsriTable esriTable)
        {

        }

        [JsonProperty("geometries", Required = Required.Always)]
        //[JsonConverter(typeof(GeometryConverter))]
        public IGeometry Geometry 
        {
            get 
            {
                return Attributes.Geometry;
            }
            set 
            {
                Attributes.Geometry = value;
            }
        }

        [JsonProperty("attributes", Required = Required.Always)]
        public TEsriTable Attributes {get; set;}
    }

   
}