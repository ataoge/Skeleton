namespace Ataoge.GisCore.Geometry
{
    public abstract class Surface : Geometry
    {
        public override GeometryType GeometryType { get { return GeometryType.Surface; } }
    }    
}