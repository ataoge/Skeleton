namespace Ataoge.GisCore.Geometry
{
    public interface IEsriGeometry : IGeometry
    {
        SpatialReference SpatialReference {get; set;}
    }
}