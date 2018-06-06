using System.Collections.Generic;
using Ataoge.GisCore.Geometry;

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

   
}