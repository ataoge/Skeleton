namespace Ataoge.GisCore.Geometry
{
    public class EsriExtent : IEsriGeometry
    {
        public double XMin {get; set;}

        public double YMin {get; set;}

        public double XMax {get; set;}

        public double YMax {get; set;}

        public SpatialReference SpatialReference {get; set;}

    }


    public class SpatialReference
    {
        public SpatialReference(int wkId)
        {
            this.Wkid = wkId;
            this.LatestWkid = wkId;
        }

        public int? Wkid {get; set;}

        public int? LatestWkid {get; set;}

        //public int? VcsWkId {get; set;}

        //public double XyTolerance {get; set;}

        //public double ZTolerance {get; set;}

        //public double MTolerance {get; set;}

        //public double FalseX {get; set;}

        //public double FalseY {get; set;}

        //public long  XyUnits {get; set;} = 10000;

        //public double FalseZ {get; set;}

        //public long ZUnits {get; set;} = 10000;

        //public double FalseM {get; set;}

        //public long MUnits {get; set;} = 10000;

        public static SpatialReference WGS84 = new SpatialReference(4326);

        public static SpatialReference WebMercator = new SpatialReference(3857);

    }
}