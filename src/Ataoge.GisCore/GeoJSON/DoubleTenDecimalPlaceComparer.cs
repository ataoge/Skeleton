using System;
using System.Collections.Generic;

namespace Ataoge.GisCore
{
    /// <summary>
    ///     Compares doubles for equality.
    /// </summary>
    /// <remarks>
    ///     10 decimal places equates to accuracy to 11.1 μm.
    /// </remarks>
    public class DoubleTenDecimalPlaceComparer : IEqualityComparer<double>
    {
        public bool Equals(double x, double y)
        {
            return Math.Abs(x - y) < 0.0000000001;
        }

        public int GetHashCode(double obj)
        {
            return obj.GetHashCode();
        }
    }
}