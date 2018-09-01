namespace Ataoge.GisCore.Geometry
{
    public abstract class EsriGeometry : IEsriGeometry
    {
        private const int WebMercator = 0x18EE1; //100210
        private const int WGS1984 = 0x10E6; //4326
        private const int CGCS2000 = 0x118A; //4490

        /// <summary>
        /// 坐标系
        /// </summary>
        public virtual SpatialReference SpatialReference {get; set;}
    }
}