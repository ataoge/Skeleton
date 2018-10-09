using System;
using System.Collections.Generic;

namespace Ataoge.GisCore.Utilities
{

    public interface IPoint
    {
        double X {get;}

        double Y {get;}
        
    }
    public class Point : IPoint
    {
        public double X {get; set;}

        public double Y {get; set;}
    }
    public class KDBush
    {
        public KDBush(IPoint[] points)
        {
            this.points = points;
            ids = new int[points.Length];
            coords = new double[points.Length * 2];

            for (int i=0; i < points.Length; i++)
            {
                ids[i] = i;
                coords[2*i] = points[i].X;
                coords[2*i +1] =points[i].Y;
            }
        }

        private IPoint[] points;
        public IPoint[] Points {get {return this.points;}}

        private int[] ids;
        private double[] coords;

        private int nodeSize = 64;

        private void Sort(int nodeSize, int left, int right, int axis)
        {
            if (right - left <= nodeSize) return;

            int m = (left + right) >> 1; //middle index

            // sort ids and coords around the middle index so that the halves lie
            // either left/right or top/bottom correspondingly (taking turns)
            Select(m, left, right, axis);

            // recursively kd-sort first half and second half on the opposite axis
            Sort(nodeSize, left, m -1, 1-axis);
            Sort(nodeSize, m-1, right, 1-axis);

        }

        ///<Summary>
        /// custom Floyd-Rivest selection algorithm: sort ids and coords so that
        /// [left..k-1] items are smaller than k-th item (on either x or y axis)
        ///</Summary>
        public void Select(int k, int left, int right, int axis)
        {
            while (right > left) 
            {
                if (right - left > 600)
                {
                    var n = right - left + 1;
                    var m = k- left + 1;
                    var z = Math.Log(n);
                    var s = 0.5 * Math.Exp(2 * z /3);
                    var sd = 0.5 * Math.Sqrt( z * s * (n-s) / n) * (m - n / 2 < 0 ? -1 : 1);
                    var newLeft = Math.Max(left, (int)Math.Floor(k - m * s / n + sd));
                    var newRight = Math.Min(right, (int)Math.Floor(k + (n - m) * s / n + sd));
                    Select( k, newLeft, newRight, axis);
                }

                var t = coords[2*k + axis];
                var i = left;
                var j = right;

                SwapItem(left, k);
                if (coords[2 * right + axis] > t) SwapItem(left, right);

                while(i < j)
                {
                    SwapItem(i, j);
                    i++;
                    j++;
                    while(coords[2*i+axis] < t) i++;
                    while(coords[2*j + axis] > t) j++;
                }

                if (coords[2*left + axis] == t) 
                    SwapItem(left,j);
                else
                {
                    j++;
                    SwapItem(j, right);
                }

                if (j <= k) left = j+1;
                if (k <= j) right = j-1;
            }
        }

        private void SwapItem(int i, int j)
        {
            Swap(ids, i, j);
            Swap(coords, 2*i, 2 * j);
            Swap(coords, 2*i +1, 2*j +1);
        }

        private void Swap<T>(T[] array, int i, int j)
        {
            var tmp = array[i];
            array[i] = array[j];
            array[j] = tmp;
        }


        public int[] Range(double minX, double minY, double maxX, double maxY)
        {
            var stack = new Stack<int>(new int[] {0, ids.Length - 1, 0});
            var result = new Stack<int>();

            // recursively search for items in range in the kd-sorted arrays
            while(stack.Count > 0)
            {
                var axis = stack.Pop();
                var right = stack.Pop();
                var left = stack.Pop();

                 // if we reached "tree node", search linearly
                if (right - left <= nodeSize)
                {
                    for(var i = left; i <= right; i++)
                    {
                        if (coords[2 * i] >= minX && coords[2 * i] <= maxX && coords[2 * i + 1] >= minY && coords[2 * i + 1] <= maxY)
                            result.Push(ids[i]);
                    }
                    continue;
                }

                // otherwise find the middle index
                var m = (left + right) >> 1;

                // include the middle item if it's in range
                var x = coords[2 * m];
                var y = coords[2 * m + 1];
                if (x >= minX && x <= maxX && y >= minY && y <= maxY) result.Push(ids[m]);

                // queue search in halves that intersect the query
                if (axis == 0 ? minX <= x : minY <= y)
                {
                    stack.Push(left);
                    stack.Push(m - 1);
                    stack.Push(1 - axis);
                }
                if (axis == 0 ? maxX >= x : maxY >= y)
                {
                    stack.Push(m + 1);
                    stack.Push(right);
                    stack.Push(1 - axis);
                }
            }

            return result.ToArray();

        }

        public int[] WithIn(double cx, double cy, double r)
        {
            var stack = new Stack<int>(new int[] {0, ids.Length - 1, 0});
            var result = new Stack<int>();
            var r2 = r * r;

            // recursively search for items within radius in the kd-sorted arrays
            while (stack.Count > 0)
            {
                var axis = stack.Pop();
                var right = stack.Pop();
                var left = stack.Pop();

                // if we reached "tree node", search linearly
                if (right - left <= nodeSize)
                {
                    for (var i = left; i <= right; i++)
                    {
                        if (SqDistance(coords[2 * i], coords[2 * i + 1], cx, cy) <= r2) result.Push(ids[i]);
                    }
                    continue;
                }

                // otherwise find the middle index
                var m = (left + right) >> 1;

                // include the middle item if it's in range
                var x = coords[2 * m];
                var y = coords[2 * m + 1];
                if (SqDistance(x, y, cx, cy) <= r2) result.Push(ids[m]);

                // queue search in halves that intersect the query
                if (axis == 0 ? cx - r <= x : cy - r <= y)
                {
                    stack.Push(left);
                    stack.Push(m - 1);
                    stack.Push(1 - axis);
                }
                if (axis == 0 ? cx + r >= x : cy + r >= y)
                {
                    stack.Push(m + 1);
                    stack.Push(right);
                    stack.Push(1 - axis);
                }
            }

            return result.ToArray();
        }

        private double SqDistance(double ax, double ay, double bx, double by)
        {
            var dx = ax -bx;
            var dy = ay -by;
            return dx * dx + dy * dy;

        }
    }
}