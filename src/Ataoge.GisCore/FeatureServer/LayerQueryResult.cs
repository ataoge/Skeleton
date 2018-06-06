using Ataoge.GisCore.Geometry;

namespace Ataoge.GisCore.FeatureServer
{
    public class LayerQueryResult
    {
        public string ObjectIdFieldName {get; set;}

        public string GlobalIdFiledName {get; set;}

        public string GeometryType {get; set;}

        public SpatialReference SpatialReference {get; set;}
        public bool HasZ {get; set;}

        public bool HasM {get; set;}

        public BaseFieldInfo[] Fields {get; set;}


        public IFeatureInfo[] Features {get; set;}
        
    }
}