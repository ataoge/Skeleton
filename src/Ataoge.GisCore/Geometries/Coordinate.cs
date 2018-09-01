using System;
using System.Collections.Generic;

namespace Ataoge.GisCore.Geometries
{
    public class Coordinate : IEqualityComparer<Coordinate>, IEquatable<Coordinate>
    {
        ///<summary>
        /// The value used to indicate a null or missing ordinate value.
        /// In particular, used for the value of ordinates for dimensions
        /// greater than the defined dimension of a coordinate.
        ///</summary>
        public const double NullOrdinate = Double.NaN;

        private static readonly DoubleTenDecimalPlaceComparer DoubleComparer = new DoubleTenDecimalPlaceComparer();

        public Coordinate() : this(NullOrdinate, NullOrdinate)
        {

        }

        /// <summary>
        /// Constructs a <other>Coordinate</other> at (x,y,z).
        /// </summary>
        /// <param name="x">X value.</param>
        /// <param name="y">Y value.</param>
        public Coordinate(double x, double y)
        {
            X = x;
            Y = y;
        }

        public double X {get; set;}

        public double Y {get; set;}

        public double? Z {get; set;}

        public double? M {get; set;}
    

    #region IEqualityComparer, IEquatable

        /// <summary>
        /// Determines whether the specified object is equal to the current object
        /// </summary>
        public override bool Equals(object obj)
        {
            return (this == (obj as Coordinate));
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object
        /// </summary>
        public bool Equals(Coordinate other)
        {
            return (this == other);
        }

        /// <summary>
        /// Determines whether the specified object instances are considered equal
        /// </summary>
        public bool Equals(Coordinate left, Coordinate right)
        {
            return (left == right);
        }


        /// <summary>
        /// Determines whether the specified object instances are considered equal
        /// </summary>
        public static bool operator ==(Coordinate left, Coordinate right)
        {
            if (ReferenceEquals(left, right))
            {
                return true;
            }
            if (ReferenceEquals(null, right) || ReferenceEquals(null, left))
            {
                return false;
            }
            if (!DoubleComparer.Equals(left.X, right.X) ||
                !DoubleComparer.Equals(left.Y, right.Y))
            {
                return false;
            }
            return left.Z.HasValue == right.Z.HasValue &&
                   (!left.Z.HasValue || DoubleComparer.Equals(left.Z.Value, right.Z.Value)) &&
                    left.M.HasValue == right.M.HasValue &&
                   (!left.M.HasValue || DoubleComparer.Equals(left.M.Value, right.M.Value));
        }

        /// <summary>
        /// Determines whether the specified object instances are considered equal
        /// </summary>
        public static bool operator !=(Coordinate left, Coordinate right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Returns the hash code for this instance
        /// </summary>
        public override int GetHashCode()
        {
            var hash = 397 ^ X.GetHashCode();
            hash = (hash * 397) ^ Y.GetHashCode();
            hash = (hash * 397) ^ Z.GetValueOrDefault().GetHashCode();
            hash = (hash * 397) ^ M.GetValueOrDefault().GetHashCode();
            return hash;
        }

        /// <summary>
        /// Returns the hash code for the specified object
        /// </summary>
        public int GetHashCode(Coordinate other)
        {
            return other.GetHashCode();
        }

       
        #endregion
    }
}