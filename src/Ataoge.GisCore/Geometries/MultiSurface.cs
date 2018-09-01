using System;
using System.Collections.Generic;
using System.Linq;

namespace Ataoge.GisCore.Geometry
{
    public class MultiSurface<T> : GeometryCollection<T>, IEquatable<MultiSurface<T>> where T : Surface
    {
        public override GeometryType GeometryType { get { return GeometryType.MultiSurface; } }

        public MultiSurface()
            : base()
        {
        }

        public MultiSurface(IEnumerable<T> geometries)
            : base(geometries)
        {
        }

        public bool Equals(MultiSurface<T> other)
        {
            return Geometries.SequenceEqual(other.Geometries);
        }
    }

    public class MultiSurface : MultiSurface<Surface>
    {
        public MultiSurface()
            : base()
        {
        }

        public MultiSurface(IEnumerable<Surface> geometries)
            : base(geometries)
        {
        }
    }
}