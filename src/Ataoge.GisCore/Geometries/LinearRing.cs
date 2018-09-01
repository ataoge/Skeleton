using System;
using System.Collections.Generic;
using System.Linq;

namespace Ataoge.GisCore.Geometry
{
    public class LinearRing : LineString, IEquatable<LinearRing>
    {
        public LinearRing()
            : base()
        {
        }

        public LinearRing(IEnumerable<Point> points)
            : base(points)
        {
        }
        
        public bool Equals(LinearRing other)
        {
            return Points.SequenceEqual(other.Points);
        }

        public override Point GetCenter()
        {
            return Points.Take(Points.Count - 1).GetCenter();
        }
    }
}