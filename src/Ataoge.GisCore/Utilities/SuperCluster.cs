using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Ataoge.GisCore.Utilities
{
    public class SuperCluster
    {
        public SuperCluster()
        {
            this.trees = new KDBush[maxZoom + 2];
        }

        private int minZoom = 0;  // min zoom to generate clusters on
        private int maxZoom = 16; // max zoom level to cluster the points on
        private int radius = 40; // cluster radius in pixels
        private int extent = 512; // tile extent (radius is calculated relative to it)
        private int nodeSize = 64; // size of the KD-tree leaf node, affects performance
        private bool log = false; // whether to log timing info


        
        private KDBush[] trees;
        private Point[] points;

        public void Load(Point[] points)
        {
            this.points = points;

            // generate a cluster object for each point and index input points into a KD-tree
            var clusters = new Stack<PointCluster>();
            for (var i = 0; i < points.Length; i++)
            {
                clusters.Push(new PointCluster(points[i], i));
            }
            this.trees[maxZoom + 1] = new KDBush(clusters.ToArray());


            // cluster points on max zoom, then cluster the results on previous zoom, etc.;
            // results in a cluster hierarchy across zoom levels
            for (var z = maxZoom; z >= minZoom; z--) 
            {
                // create a new set of clusters for the zoom and index them with a KD-tree
                clusters = this._cluster(clusters.ToArray(), z);
                this.trees[z] = new KDBush(clusters.ToArray());
            }

        }

        public IPoint[] GetClusters(double minX, double minY, double maxX, double maxY, int zoom)
        {
            var minLng = ((minX + 180) % 360 + 360) % 360 - 180;
            var minLat = Math.Max(-90, Math.Min(90, minY));
            var maxLng = maxX == 180? 180 : ((maxX + 180) % 360) % 360 - 180;
            var maxLat =  Math.Max(-90, Math.Min(90, minY));

            if (maxX - minX >= 360)
            {
                minLng = -180;
                maxLng = 180;
            }
            else 
            {
                if (minLng > maxLng) {
                    var easternHem = this.GetClusters(minLng, minLat, 180, maxLat, zoom);
                    var westernHem = this.GetClusters(-180, minLat, maxLng, maxLat, zoom);
                    return easternHem.Concat(westernHem).ToArray();
                }
            }

            var tree = this.trees[this._limitZoom(zoom)];
            var ids = tree.Range(SuperCluster.LngX(minLng), SuperCluster.LatY(maxLat), SuperCluster.LngX(maxLng), SuperCluster.LatY(minLat)); //注意 LatY之后 颠倒了原先的y值
            var cluster = new Stack<IPoint>();
            foreach (var id in ids)
            {
                var c = tree.Points[id] as PointCluster;
                cluster.Push(c is Cluster ? SuperCluster.CreatePointFromCluster( c as Cluster) : this.points[c.Index]);
            }

            return cluster.ToArray();
        }

        public IPoint[] GetChildren(int clusterId)
        {
            var originId = clusterId >> 5;
            var originZoom = clusterId % 32;
            var errorMsg = "No cluster with the specified id.";

            var index = this.trees[originZoom];
            if (index == null) throw new Exception(errorMsg);

            var origin = index.Points[originId];
            if (origin == null) throw new Exception(errorMsg);

            var r = this.radius / this.extent * Math.Pow(2, originZoom - 1);
            var ids = index.WithIn(origin.X, origin.Y, r);
            var children = new Stack<IPoint>();
            foreach(var id in ids)
            {
                var c = index.Points[id] as PointCluster;
                if (c.ParentId == clusterId)
                {
                    children.Push(c is Cluster ? SuperCluster.CreatePointFromCluster( c as Cluster) : this.points[c.Index]);
                }
            }

            if (children.Count == 0) throw new Exception(errorMsg);

            return children.ToArray();
        }

        public IPoint[] GetLeaves(int clusterId, int limit = 10, int offset = 0)
        {
            var leaves = new Stack<IPoint>();
            this._appendLeaves(leaves, clusterId, limit, offset, 0);
            return leaves.ToArray();
        }

        public void GetTile(int z, int x, int y)
        {
            var tree = this.trees[this._limitZoom(z)];
            var z2 = Math.Pow(2, z);
            var p = radius * 1.0 / extent;
            var top = (y - p) / z2;
            var bottom = (y + 1 + p) / z2;

            /*  const tile = {
            features: []
        };

        this._addTileFeatures(
            tree.range((x - p) / z2, top, (x + 1 + p) / z2, bottom),
            tree.points, x, y, z2, tile);

        if (x === 0) {
            this._addTileFeatures(
                tree.range(1 - p / z2, top, 1, bottom),
                tree.points, z2, y, z2, tile);
        }
        if (x === z2 - 1) {
            this._addTileFeatures(
                tree.range(0, top, p / z2, bottom),
                tree.points, -1, y, z2, tile);
        }

        return tile.features.length ? tile : null; */
        }

        public int GetClusterExpansionZoom(int clusterId)
        {
            var clusterZoom = (clusterId % 32) - 1;
            while(clusterZoom < this.maxZoom)
            {
                var children = this.GetChildren(clusterId);
                clusterZoom--;
                if (children.Length != 1) break;
                clusterId = (children[0] as PointEx).ClusterId;
            }
            return clusterZoom;
        }

        private int _appendLeaves(Stack<IPoint> result, int clusterId, int limit, int offset, int skipped)
        {
            var children = this.GetChildren(clusterId);

            foreach(var child in children)
            {
                var props = child as PointEx;
                if (props != null && props.Cluster)
                {
                    if (skipped + props.Count <= offset)
                    {
                        // skip the whole cluster
                        skipped += props.Count;
                    }
                    else
                    {
                        // enter the cluster
                        skipped = this._appendLeaves(result, props.ClusterId, limit, offset, skipped);
                        // enter the cluster
                    }
                }
                else if (skipped < offset)
                {
                    // skip a single point
                    skipped++;
                }
                else
                {
                    result.Push(child);
                }

                if (result.Count == limit)
                    break;
            }

            return skipped;
        }

        private void _addTileFeature(int[] ids, IPoint[] points, int x, int y, int z2)
        {
            /* for (const i of ids) {
            const c = points[i];
            const f = {
                type: 1,
                geometry: [[
                    Math.round(this.options.extent * (c.x * z2 - x)),
                    Math.round(this.options.extent * (c.y * z2 - y))
                ]],
                tags: c.numPoints ? getClusterProperties(c) : this.points[c.index].properties
            };
            const id = c.numPoints ? c.id : this.points[c.index].id;
            if (id !== undefined) {
                f.id = id;
            }
            tile.features.push(f);
            } */
        }

        private int _limitZoom(int z)
        {
            return Math.Max(this.minZoom, Math.Min(z, this.maxZoom + 1));
        }
        private Stack<PointCluster> _cluster(PointCluster[] points, int zoom) 
        {
            var r = this.radius / (extent * Math.Pow(2, zoom));
            var clusters = new Stack<PointCluster>();

            for (var i = 0; i < points.Length; i++)
            {
                var p = points[i];
                // if we've already visited the point at this zoom level, skip it
                if (p.Zoom <= zoom) continue;
                p.Zoom = zoom;

                // find all nearby points
                var tree = this.trees[zoom + 1];
                var neighborIds = tree.WithIn(p.X, p.Y, r);

                var numPoints = (p is Cluster) ? (p as Cluster).NumPoints : 1;
                var wx = p.X * numPoints;
                var wy = p.Y * numPoints;

                double propsValue = 0.0;
                if (true) //reduce
                {
                    propsValue = this._accumulate(propsValue, p);
                }

                var id = (i << 5) + (zoom + 1);

                foreach(var neighborId in neighborIds)
                {
                    var bp = tree.Points[neighborId];
                    var b = bp as PointCluster;
                    
                    // filter out neighbors that are already processed
                    if (b.Zoom <= zoom) continue;
                    b.Zoom = zoom;  // save the zoom (so it doesn't get processed twice)

                    var numPoints2 = (p is Cluster) ? (p as Cluster).NumPoints : 1;
                    wx += b.X * numPoints2; // accumulate coordinates for calculating weighted center
                    wy += b.Y * numPoints2;

                    numPoints += numPoints2;
                    b.ParentId = id;

                    if (true)
                    {
                        propsValue = this._accumulate(propsValue, b);
                    }
                }

                if (numPoints == 1)
                {
                    clusters.Push(p);
                }
                else
                {
                    p.ParentId = id;
                    clusters.Push(new Cluster(wx / numPoints, wy / numPoints, id, numPoints, propsValue));
                }
            }

            return clusters;
        }


        public double _accumulate(double propValue, IPoint point)
        {
            if (point is IProperty)
            {
                return propValue + (point as IProperty).Value;
            }
            return propValue;
        }


        public static IPoint CreatePointFromCluster(Cluster cluster)
        {
            var point = new PointEx();
            point.X = XLng(cluster.X);
            point.Y = YLat(cluster.Y);

            var count = cluster.NumPoints;
            string abbrev = count >= 10000 ? $"{Math.Round( count / 1000.0)}k" : count >= 1000 ? $"{Math.Round(count / 100.0) / 10}k" :  $"{count}";
            point.Id = cluster.Id;
            point.Cluster = true;
            point.ClusterId = cluster.Index;
            point.Count = count;
            point.CountAbbreviated = abbrev;

            return point;
        }

        // longitude/latitude to spherical mercator in [0..1] range
        public static double LngX(double lng)
        {
            return lng / 360 + 0.5;
        }

        public static double LatY(double lat)
        {
            var sin = Math.Sin(lat * Math.PI / 180);
            var y = (0.5 - 0.25 * Math.Log((1+sin) /(1-sin)) /Math.PI);
            return y < 0 ? 0 : y > 1 ? 1 : y;
        }

        public static double XLng(double x)
        {
            return (x-0.5) * 360;
        }

        public static double YLat(double y)
        {
            var y2 = (180 - y *360) * Math.PI /180;
            return 360 * Math.Atan(Math.Exp(y2)) / Math.PI -90;
        }
    }

    public interface IProperty
    {
        double Value {get; set;}
    }

    public class PointCluster : IPoint, IProperty
    {
        public PointCluster()
        {

        }
        public PointCluster(IPoint point, int id)
        {
            X = SuperCluster.LngX( point.X);
            Y = SuperCluster.LatY(point.Y);
            Index = id;
        }

        public double X {get; set;} // projected point coordinates

        public double Y {get; set;}

        public int Zoom {get; set;} = int.MaxValue; // the last zoom the point was processed at

        public int Index {get; set;} // index of the source feature in the original input array,

        public int ParentId {get; set;} = -1; // parent cluster id
        public double Value { get ; set; } = 1.0;
    }

    public class Cluster : PointCluster
    {
        public Cluster(double x, double y, int id, int numPoints, double propValue) 
        {
            this.X = x;
            this.Y = y;
            //this.Index = id;
            this.Id = id;
            this.ParentId = -1;
            this.NumPoints = numPoints;
            
            this.Value = propValue;
        }

        public int Id {get; set;}

        public int NumPoints {get; set;}


    }

    public class PointEx : Point
    {
        public int Id {get; set;}

        public bool Cluster {get; set;}

        public int ClusterId {get; set;}

        public int Count {get; set;}

        public string CountAbbreviated {get; set;}
    }

   
}