namespace Ataoge.GisCore.Geometry
{
    public class EsriPoint : EsriGeometry
    {
        public EsriPoint()
        {

        }

        public EsriPoint(double x, double y)
        {
            this.X = x;
            this.Y = y;

            this.SpatialReference = new SpatialReference(4326);
        }

        public EsriPoint(double x, double y, SpatialReference sref)
        {
            X = x;
            Y = y;
            this.SpatialReference = sref;
        }

        public double? X { get; set; }
        public double? Y { get; set; }
        public double? Z { get; set; }
        public double? M { get; set; }

        //public SpatialReference SpatialReference {get; set;}

        /// <summary>
        /// 转化为ESRI格式点
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return X.ToString() + " " + Y.ToString();
        }
    }
}