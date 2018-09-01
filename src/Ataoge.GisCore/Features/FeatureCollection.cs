using System;
using System.Collections.Generic;
using Ataoge.GisCore.Geometry;
using Newtonsoft.Json;

namespace Ataoge.GisCore.Features
{
    public class FeatureCollection<TDto>
        where TDto: IHasGeometry
    {
        public FeatureCollection():
            this(new List<Feature<TDto>>())
        {

        }

       

        public FeatureCollection(IEnumerable<Feature<TDto>> features)
        {
            if (features == null)
            {
                throw new ArgumentNullException(nameof(features));
            }

            Features = features;
        }

        [JsonProperty(PropertyName = "type", Required = Required.Always)]
        public string Type {get; set;} = "FeatureCollection"; 
         
        [JsonProperty(PropertyName = "features", Required = Required.Always)]
        public IEnumerable<Feature<TDto>> Features { get; private set; }
   
        
    }
}