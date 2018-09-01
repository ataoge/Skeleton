using System;
using System.Collections.Generic;
using System.Linq;

namespace Ataoge.GisCore.Geometry
{
    public class PolyhedralSurface<T> : Surface, IEquatable<PolyhedralSurface<T>> where T : Polygon
    {
        public override GeometryType GeometryType { get { return GeometryType.PolyhedralSurface; } }
        public override bool IsEmpty { get { return !Geometries.Any(); } }

        public List<T> Geometries { get; private set; }

        public PolyhedralSurface()
            : this(new List<T>())
        {
        }

        public PolyhedralSurface(IEnumerable<T> geometries)
        {
            Geometries = new List<T>(geometries);

            if (Geometries.Any())
                Dimension = Geometries.First().Dimension;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (!(obj is PolyhedralSurface<T>))
                return false;

            return Equals((PolyhedralSurface<T>)obj);
        }

        public bool Equals(PolyhedralSurface<T> other)
        {
            return Geometries.SequenceEqual(other.Geometries);
        }

        public override int GetHashCode()
        {
            return new { Geometries }.GetHashCode();
        }

        public override Point GetCenter()
        {
            return Geometries.Select(g => g.GetCenter()).GetCenter();
        }

        public override BoundingBox GetBoundingBox()
        {
            return Geometries.Select(g => g.GetBoundingBox()).GetBoundingBox();
        }
    }

    public class PolyhedralSurface : PolyhedralSurface<Polygon>
    {
        public PolyhedralSurface()
            : base()
        {
        }

        public PolyhedralSurface(IEnumerable<Polygon> geometries)
            : base(geometries)
        {
        }
    }
}