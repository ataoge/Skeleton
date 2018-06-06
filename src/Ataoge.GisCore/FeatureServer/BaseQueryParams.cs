namespace Ataoge.GisCore.FeatureServer
{
    public class BaseQueryParams
    {
        public string Format {get; set;}

        public string Geometry {get; set;}

        public string GeometryType {get; set;}

        public string InSR {get; set;}

        //esriSpatialRelIntersects | esriSpatialRelContains | esriSpatialRelCrosses | esriSpatialRelEnvelopeIntersects | esriSpatialRelIndexIntersects | esriSpatialRelOverlaps | esriSpatialRelTouches | esriSpatialRelWithin
        public string SpatialRel {get; set;}

        //time=1199145600000
        //time=1199145600000, 1230768000000
        // time=null, 1230768000000
        public string Time {get; set;}

        public bool ReturnGeometry {get; set;}

        public double MaxAllowableOffset {get; set;}

        public int GeometryPrecision {get; set;}

        public string OutSR {get; set;}

        public string GdbVersion {get; set;}

        public bool ReturnIdOnly {get; set;}

        public bool ReturnCountOnly {get; set;}

        public bool ReturnZ {get; set;}

        public bool ReturnM {get; set;}

        public string HistoricMoment {get; set;}

        public bool ReturnTrueCurves {get; set;}

        //Values: none | standard | native
        public string SqlFormat {get; set;}
    }
}