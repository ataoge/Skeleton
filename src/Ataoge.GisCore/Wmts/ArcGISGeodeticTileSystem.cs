using System;

namespace Ataoge.GisCore.Wmts
{
    public class ArcGISGeodeticTileSystem : TileSystem
    {
        public ArcGISGeodeticTileSystem()
        {
            this.initialResolution = 0.70391441567318047;
            this.initialScale = 295829355.44999999;
        }
        public override double EarthRadius
        {
            get
            {
                return WGS84_EARTH_RADIUS;
            }
        }
        double initialResolution;
        public override double InitialResolution
        {
            get
            {
                return this.initialResolution;
            }
            
        }

        public override double OriginXShift
        {
            get
            {
                return this._originXShift;
            }
        }

        public override double OriginYShift
        {
            get
            {
                return this._originYShift;
            }
        }

        double _originXShift = 180;
        double _originYShift = -90;
        public void SetXYOriginShift(double originXShift, double originYShift)
        {
            this._originXShift = originXShift;
            this._originYShift = originYShift;
        }

        public override int TileSize
        {
            get
            {
                return DEFALUT_TILESIZE;
            }
        }

        public void SetInitialResolution(double value)
        {
            this.initialResolution = value;
        }

        private double initialScale = 0.0;
        public void SetInitailScale(double scale)
        {
            this.initialScale = scale;
        }

        MapExtent _extent = new MapExtent() {MinY = -90, MinX = -180, MaxY = 90, MaxX = 180};

        public void SetExtent(double minLat, double minLon, double maxLat, double maxLon)
        {
            _extent.MinY = minLat;
            _extent.MinX = minLon;
            _extent.MaxY = maxLat;
            _extent.MaxX = maxLon;
        }

        public override TileSize GetTileMatrix(int zoom)
        {
            var res = this.GetResolution(zoom);
            var width = (_extent.MaxX - _extent.MinX) / (this.TileSize * res);
            var height = (_extent.MaxY - _extent.MinY) / (this.TileSize * res);
            return new TileSize(){ Width = (int)Math.Ceiling(width), Height = (int)Math.Ceiling(height)};
        }

        public override double GetResolution(int zoom)
        {
            return this.initialResolution / (1 << zoom);
        }

        
        public override double GetMapScale(int zoom, int screenDpi = 96)
        {
            if (initialScale > 0.0)
            {
                return this.initialScale / (1 << zoom);
            }
            return base.GetMapScale(zoom, screenDpi);
        }

        public override PixelPoint PixelToTile(int px, int py, int bTopRight = 0)
        {
            int tx = px / this.TileSize;
            int ty = py / this.TileSize;
            if ((bTopRight & 0x1) == 0x1)
            {
                if (px % this.TileSize == 0)
                    tx = tx == 0 ? 0 : tx - 1;
            }
            if ((bTopRight & 0x2) != 0x2)
            {
                if (py % this.TileSize == 0)
                    ty = ty == 0 ? 0 : ty - 1;
            }
            return new PixelPoint(){ X = tx, Y = ty};
        }

        public override PixelPoint PixelToRaster(int px, int py, int zoom)
        {
            return new PixelPoint() {X = px, Y = py};
        }

        public override PixelPoint LatLonToPixel(double lat, double lon, int zoom)
        {
            var res = this.GetResolution(zoom);
            int px = (int)((this.OriginXShift + lon) / res);
            int py = (int)((this.OriginYShift + lat) / res) * -1;
            return new PixelPoint() {X = px, Y = py};
        }

        public override MapPoint PixelToLatLon(int px, int py, int zoom)
        {
            var res = this.GetResolution(zoom);
            double lon = px * res - this.OriginXShift;
            double lat = -py * res - this.OriginYShift;
            return new MapPoint() { Y =lat, X = lon};
        }

        public override MapExtent TileToLatLonBounds(int tx, int ty, int zoom)
        {
            var res = this.GetResolution(zoom);
            double minLat = ty * this.TileSize * res - this.OriginYShift;
            double minLon = tx * this.TileSize * res - this.OriginXShift;
            double maxLat = (ty + 1) * this.TileSize * res - this.OriginYShift;
            double maxLon = (tx + 1) * this.TileSize * res - this.OriginXShift;
            return new MapExtent(){ MinY = minLat, MinX = minLon, MaxY = maxLat, MaxX = maxLon};
        }

    }
}