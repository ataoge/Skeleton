namespace Ataoge.GisCore.Geometry
{
    public class EsriPoint : IEsriGeometry
    {
        public EsriPoint()
        {

        }

        public EsriPoint(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }

        public double? X { get; set; }
        public double? Y { get; set; }
        public double? Z { get; set; }
        public double? M { get; set; }

        public SpatialReference SpatialReference {get; set;}
    }
}