namespace Ataoge.GisCore.Geometry
{
    public interface IHasGeometry
    {
        IGeometry Geometry {get; set;}
    }

    public interface IHasGeometryWithId : IHasGeometry
    {
        int Id {get; set;}
    }
}