using System.IO;
using Ataoge.GisCore.Utilities;

namespace Ataoge.GisCore.Geometry
{
    internal class EsriEwkbReader : EsriWkbReader
    {
        internal EsriEwkbReader(Stream stream, int outSrid)
            : base(stream)
        {
            this._outSrid = outSrid;
        }

        internal EsriEwkbReader(Stream stream)
            : base(stream)
        {
        }

        private int? _outSrid;

        protected override GeometryType ReadGeometryType(uint type)
        {
            return (GeometryType)(type & 0XF);
        }

        protected override Dimension ReadDimension(uint type)
        {
            if ((type & EwkbFlags.HasZ) == EwkbFlags.HasZ && (type & EwkbFlags.HasM) == EwkbFlags.HasM)
                return Dimension.Xyzm;
            else if ((type & EwkbFlags.HasZ) == EwkbFlags.HasZ)
                return Dimension.Xyz;
            else if ((type & EwkbFlags.HasM) == EwkbFlags.HasM)
                return Dimension.Xym;

            return Dimension.Xy;
        }

        protected override int? ReadSrid(uint type)
        {
            if ((type & EwkbFlags.HasSrid) == EwkbFlags.HasSrid)
                return wkbReader.ReadInt32();

            return null;
        }

       protected override EsriPoint CreatePoint(double x, double y, double? z, double? m)
       {
           
           if (_outSrid == Srid)
           {
               return base.CreatePoint(x, y, z, m);
           }
           else
           {
               EsriPoint point = null;
               double ox;
               double oy;
               if (Srid == 4326 || Srid == 4490)
               {
                   if (_outSrid == 3857 || _outSrid == 102100)
                   {
                       CoordinateTransform.WGS84ToWebMercator(y, x, out ox, out oy);
                       point = new EsriPoint(ox, oy);
                   }
                   else
                   {
                       point = new EsriPoint(x, y);
                   }
               }
               else
               {
                   if (_outSrid == 4326 || _outSrid == 4490)
                   {
                       CoordinateTransform.WebMercatorToWGS84(y, x, out oy, out ox);
                       point = new EsriPoint(ox, oy);
                   }
                   else
                   {
                       point = new EsriPoint(x, y);
                   }
               }

               point.Z = z;
               point.M = m;
               
               return point;
           }
           
       }

       protected override SpatialReference CreateSpatialReference()
       {
            if (_outSrid!= null)
                return null;
            return base.CreateSpatialReference();
       }
    }
}