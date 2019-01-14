using System;

namespace Ataoge.GisCore.Wmts
{
    public class GlobalGeodeticTileSystem : TileSystem
    {
        public override double EarthRadius
        {
            get
            {
                return WGS84_EARTH_RADIUS;
            }
        }

        public override double InitialResolution
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override double OriginXShift
        {
            get
            {
                return 180;
            }
        }

        public override double OriginYShift
        {
            get
            {
                return 90;
            }
        }

        public override int TileSize
        {
            get
            {
                return DEFALUT_TILESIZE;
            }
        }

        public override PixelPoint LatLonToPixel(double lat, double lon, int zoom)
        {
            var res = this.GetResolution(zoom);
            int px = (int) ((this.OriginXShift + lon) / res);
            int py = (int)((this.OriginYShift + lat) / res);
            return new PixelPoint() { X = px, Y = py};
        }

        public override MapPoint PixelToLatLon(int px, int py, int zoom)
        {
            var res = this.GetResolution(zoom);
            double lon = px * res - this.OriginXShift;
            double lat = py * res - this.OriginYShift;
            return new MapPoint(){ X = lat, Y = lon};
        }

        public override MapExtent TileToLatLonBounds(int tx, int ty, int zoom)
        {
            var res = this.GetResolution(zoom);
            double minLat = ty * this.TileSize * res - this.OriginYShift;
            double minLon = tx * this.TileSize * res - this.OriginXShift;
            double maxLat = (ty +1) * this.TileSize * res - this.OriginYShift;
            double maxLon = (tx +1) * this.TileSize * res - this.OriginXShift;
            return new MapExtent() { MinY = minLat, MinX = minLon, MaxY = maxLat, MaxX = maxLon};
        }

        public override TileSize GetTileMatrix(int zoom)
        {
            var height = 1 << zoom;
            var width = height * 2;
            return new TileSize(){ Width = width,  Height = height};
        }

   
    }
}