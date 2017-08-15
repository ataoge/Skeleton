using System.IO;

namespace Ataoge.GisCore.Geometry
{
    public interface IGeometrySerializer
    {
        Geometry Deserialize(Stream stream);
        void Serialize(Geometry geometry, Stream stream);
    }
}