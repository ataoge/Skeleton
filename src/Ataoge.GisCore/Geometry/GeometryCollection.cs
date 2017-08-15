using System;
using System.Collections.Generic;
using System.Linq;

namespace Ataoge.GisCore.Geometry
{
    public class GeometryCollection : Geometry, IEquatable<GeometryCollection>
    {
        public override GeometryType GeometryType { get { return GeometryType.GeometryCollection; } }
        public override bool IsEmpty { get { return !Geometries.Any(); } }

        public List<Geometry> Geometries { get; private set; }

        public GeometryCollection()
            : this(new List<Geometry>())
        {
        }

        public GeometryCollection(IEnumerable<Geometry> geometries)
        {
            Geometries = new List<Geometry>(geometries);

            if (Geometries.Any())
                Dimension = Geometries.First().Dimension;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (!(obj is GeometryCollection))
                return false;

            return Equals((GeometryCollection)obj);
        }

        public bool Equals(GeometryCollection other)
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
}