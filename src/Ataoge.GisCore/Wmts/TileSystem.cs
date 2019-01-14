using System;

namespace Ataoge.GisCore.Wmts
{
    public class LodInfo
    {
        public int Level {get; set;}
        public double Scale {get; set;}

        public double Resolution {get; set;}
    }
    
    public abstract class TileSystem : ITileSystem
    {
        public const int DEFALUT_TILESIZE = 256;
        public const double WGS84_EARTH_RADIUS = 6378137.0;
        public const double DEFAULT_MINLATITUDE = -85.05112878;
        public const double DEFAULT_MAXLATITUDE = 85.05112878;
        public const double DEFAULT_MINLONGITUDE = -180;
        public const double DEFAULT_MAXLONGTITUE = 180;

        public abstract double EarthRadius { get;  }
        public abstract double InitialResolution { get; }
        public abstract double OriginXShift{ get;  }
        public abstract double OriginYShift { get;  }
        public abstract int TileSize { get; }

        public virtual double GetMapScale(double latitude, int zoom, int screenDpi = 96)
        {
            return this.EarthRadius * Math.PI / 180 * GetResolution(latitude, zoom) * screenDpi / 0.0254;  //WMTS Default DPI is 90.7 , 100.0/ 90.7 * 0.00254 = 0.0028004410143329657
        }

        
        public virtual double GetMapScale(int zoom, int screenDpi = 96)
        {
            return  this.EarthRadius * Math.PI / 180 * GetResolution(zoom) *  screenDpi / 0.0254;
        }


        public virtual double GetResolution(double latitude, int zoom)
        {
            return GetResolution(zoom);
        }

        /// <summary>
        /// 获取某一级别的分辨率
        /// </summary>
        /// <param name="zoom"></param>
        /// <returns></returns>
        public virtual double GetResolution(int zoom)
        {
            return 180.0 / this.TileSize / (1 << zoom);
        }

        public uint MapSize(int zoom)
        {
            return (uint)this.TileSize << zoom;
        }

        
        public abstract TileSize GetTileMatrix(int zoom);

        public abstract MapPoint PixelToLatLon(int px, int py, int zoom);
        
        public abstract MapExtent TileToLatLonBounds(int tx, int ty, int zoom);
       
        /// <summary>
        /// 基于切片的坐标转为基于原点的坐标
        /// </summary>
        /// <param name="px"></param>
        /// <param name="py"></param>
        /// <param name="zoom"></param>
        /// <returns></returns>
        public virtual PixelPoint PixelToRaster(int px, int py, int zoom)
        {
            int mapSize = this.TileSize << zoom;
            return new PixelPoint() { X = px, Y = mapSize - py};
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="px"></param>
        /// <param name="py"></param>
        /// <param name="bTopRight">是否右上，1表示右 2表示上</param>
        /// <returns></returns>
        public virtual PixelPoint PixelToTile(int px, int py, int bTopRight = 0)
        {
            int tx = px / this.TileSize;
            int ty = py / this.TileSize;
            if ((bTopRight & 0x1) == 0x1)
            {
                if (px % this.TileSize == 0)
                    tx = tx == 0 ? 0 : tx - 1;
            }
            if ((bTopRight & 0x2) == 0x2)
            {
                if (py % this.TileSize == 0)
                    ty = ty == 0 ? 0 : ty - 1;
            }
            return new PixelPoint(){ X= tx, Y = ty};
        }

        public abstract PixelPoint LatLonToPixel(double lat, double lon, int zoom);


        public virtual PixelPoint LatLonToTile(double lat, double lon, int zoom, int bTopRight = 0)
        {
            var pxPoint = this.LatLonToPixel(lat, lon, zoom);
            return this.PixelToTile(pxPoint.X, pxPoint.Y, bTopRight);
        }

        public virtual TileExtent LatLonBoundsToTileMatrix(double minLat, double minLon, double maxLat, double maxLon, int zoom)
        {
            var t1 = this.LatLonToTile(minLat, minLon, zoom);
            var t2 = this.LatLonToTile(maxLat, maxLat, zoom, 3);

            return new TileExtent(){ MinRow = t1.X, MinCol = t1.Y, MaxRow = t2.X, MaxCol = t2.Y };
        }

        public PixelPoint TileToPixel(int tx, int ty)
        {
            int px = tx * this.TileSize;
            int py = ty * this.TileSize;
            return new PixelPoint(){ X = px, Y = py};
        }

        public virtual int ZoomForPixelSize(double pixelSize)
        {
            for (int i = 0; i < 30; i++)
            {
                if (pixelSize > this.GetResolution(i))
                    if (i != 0)
                        return i - 1;
                    else
                        return 0;

            }
            return 0;
        }
    }

    public abstract class ProjectedTileSystem : TileSystem, IProjectedTileSystem
    {
        public ProjectedTileSystem()
        {
            this.originShift = 2 * Math.PI * this.EarthRadius / 2.0;
            this.initialResolution = 2 * Math.PI * this.EarthRadius / this.TileSize;
        }

        public override int TileSize
        {
            get { return DEFALUT_TILESIZE; }
        }

        public override double EarthRadius
        {
            get { return WGS84_EARTH_RADIUS; }
        }
        private double initialResolution;
        public override double InitialResolution
        {
            get { return this.initialResolution; }
        }
        private double originShift;
        public override double OriginXShift
        {
            get { return this.originShift; }
        }

        public override double OriginYShift
        {
            get { return this.originShift; }
        }

        public override double GetMapScale(double latitude, int zoom, int screenDpi = 96)
        {
            return GetResolution(latitude, zoom) * screenDpi / 0.0254;  //WMTS Default DPI is 90.7 , 100.0/ 90.7 * 0.00254 = 0.0028004410143329657
        }

        public override double GetMapScale(int zoom, int screenDpi = 96)
        {
            return GetResolution(zoom) * screenDpi / 0.0254;
        } 

        public override double GetResolution(int zoom)
        {
            return this.InitialResolution / (1 << zoom);  //2的zoom次方  zoom开始值为0
        }

        public override  double GetResolution(double latitude, int zoom)
        {
            latitude = ProjectedTileSystem.Clip(latitude, DEFAULT_MINLATITUDE, DEFAULT_MAXLATITUDE);
            return Math.Cos(latitude * Math.PI / 180) * 2 * Math.PI * EarthRadius / MapSize(zoom);
        }

        private static double Clip(double n, double minValue, double maxValue)
        {
            return Math.Min(Math.Max(n, minValue), maxValue);
        }

        public override PixelPoint LatLonToPixel(double lat, double lon, int zoom)
        {
            var meterXY = this.LatLonToXY(lat, lon);
            return this.XYToPixel(meterXY.X, meterXY.Y, zoom);
        }

        public override MapPoint PixelToLatLon(int px, int py, int zoom)
        {
            var meterXY = this.PixelToXY(px, py, zoom);
            return this.XYToLatLon(meterXY.X, meterXY.Y);
        }

        

        public abstract MapPoint LatLonToXY(double lat, double lon);

        public abstract MapPoint XYToLatLon(double x, double y);
 
        public virtual PixelPoint XYToPixel(double x, double y, int zoom)
        {
            double res = this.GetResolution(zoom);
            int px = (int)Math.Ceiling((x + this.OriginXShift) / res);
            int py = (int)Math.Ceiling((y + this.OriginYShift) / res);
            return new PixelPoint(){ X = px, Y = py};
        }

        public virtual MapPoint PixelToXY(int px, int py, int zoom)
        {
            double res = this.GetResolution(zoom);
            double mx = px * res - this.OriginXShift;
            double my = py * res - this.OriginYShift;
            return new MapPoint(){ X = mx, Y = my};
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="zoom"></param>
        /// <param name="bTopRight">是否右上，1表示右 2表示上</param>
        /// <returns></returns>
        public PixelPoint MetersToTile(double x, double y, int zoom, int bTopRight = 0)
        {
            var pxPoint = this.XYToPixel(x, y, zoom);
            return this.PixelToTile(pxPoint.X, pxPoint.Y,  bTopRight);
        }

        public override TileSize GetTileMatrix(int zoom)
        {
            var num = 1 << zoom;
            return new TileSize() { Width = num, Height = num};
        }

        public virtual MapExtent TileToXYBounds(int tx, int ty, int zoom)
        {
            var minXY = this.PixelToXY(tx * this.TileSize, ty * this.TileSize, zoom);
            var maxXY = this.PixelToXY((tx + 1) * this.TileSize, (ty + 1) * this.TileSize, zoom);
            return new MapExtent(){ MinX =minXY.X, MinY = minXY.Y, MaxX = maxXY.X, MaxY = maxXY.Y};
        }

        public override MapExtent TileToLatLonBounds(int tx, int ty, int zoom)
        {
            var meterBounds = this.TileToXYBounds(tx, ty, zoom);
            var minLatLon = this.XYToLatLon(meterBounds.MinX, meterBounds.MinY);
            var maxLatLon = this.XYToLatLon(meterBounds.MaxX, meterBounds.MaxY);
            return new MapExtent(){ MinX =minLatLon.X, MinY = minLatLon.Y, MaxX = maxLatLon.X, MaxY = maxLatLon.Y};
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="zoom"></param>
        /// <param name="bTopRight">是否右上，1表示右 2表示上</param>
        /// <returns></returns>
        public PixelPoint XYToTile(double x, double y, int zoom, int bTopRight = 0)
        {
            var pxPoint = this.XYToPixel(x, y, zoom);
            return this.PixelToTile(pxPoint.X, pxPoint.Y, bTopRight);
        }

        public TileExtent XYBoundsToTileMatrix(double minX, double minY, double maxX, double maxY, int zoom)
        {
            var t1 = this.XYToTile(minX, minY, zoom);
            var t2 = this.XYToTile(maxX, maxY, zoom, 3);

            return new TileExtent() { MinRow = Math.Min(t1.X, t2.X), MinCol = Math.Min(t1.Y, t2.Y), MaxRow = Math.Max(t1.X, t2.X), MaxCol = Math.Max(t1.Y, t2.Y)};

        }

        public override TileExtent LatLonBoundsToTileMatrix(double minLat, double minLon, double maxLat, double maxLon, int zoom)
        {
            var minXY = this.LatLonToXY(minLat, minLon);
            var maxXY = this.LatLonToXY(maxLat, maxLon);
            return XYBoundsToTileMatrix(minXY.X, minXY.Y, maxXY.X, maxXY.Y, zoom);
        }

        public override PixelPoint LatLonToTile(double lat, double lon, int zoom, int bTopRight = 0)
        {
            var xy = this.LatLonToXY(lat, lon);
            return this.XYToTile(xy.X, xy.Y, zoom, bTopRight);
        }

    }
}