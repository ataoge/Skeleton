using System;
using Ataoge.GisCore.Utilities;

namespace Ataoge.GisCore.Wmts
{
    public class ArcGISTileSystem : ProjectedTileSystem
    {
        public ArcGISTileSystem()
        {

        }

        private int _tilesize = TileSystem.DEFALUT_TILESIZE;
        public override int TileSize 
        {
            get { return  _tilesize;}
        }

        public void SetTileSize(int tileSize)
        {
            _tilesize = tileSize;
        }


        public virtual int Srid {get; set;}

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

        double _originXShift = 20037508.34;
        double _originYShift = -20037508.34;
        public void SetXYOriginShift(double originXShift, double originYShift)
        {
            this._originXShift = originXShift;
            this._originYShift = originYShift;
        }

        public void SetXYOrigin(double originX, double originY)
        {
            this._originXShift = - originX;
            this._originYShift = - originY;
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

        double _minX = -20037507.0672;
        double _minY = -20036018.7354;
        double _maxX = 20037507.0672;
        double _maxY = 20102482.4102;
        public void SetExtent(double minX, double minY, double maxX, double maxY)
        {
            _minX = minX;
            _minY = minY;
            _maxX = maxX;
            _maxY = maxY;
        }

         public override TileSize GetTileMatrix(int zoom)
        {
            var res = this.GetResolution(zoom);
            var height = (_maxY - _minY) / (this.TileSize * res);
            var width = (_maxX - _minX) / (this.TileSize * res);
            return new TileSize(){ Width = (int)Math.Ceiling(width), Height = (int)Math.Ceiling(height)};
        }

        double initialResolution;
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

        public override PixelPoint XYToPixel(double x, double y, int zoom)
        {
            double res = this.GetResolution(zoom);
            int px = (int)Math.Ceiling((x + this.OriginXShift) / res);
            int py = (int)Math.Ceiling((y + this.OriginYShift) / res) * -1;
            return new PixelPoint() { X = px, Y = py };
        }

        public override MapPoint PixelToXY(int px, int py, int zoom)
        {
            double res = this.GetResolution(zoom);
            double mx = px * res - this.OriginXShift;
            double my = -py * res - this.OriginYShift;
            return new MapPoint(){ X = mx,  Y = my};
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
            return new PixelPoint(){X = tx, Y = ty};
        }

        public override MapPoint LatLonToXY(double lat, double lon)
        {
            if (Srid > 100000)
            {
                TransverseMercatorHelper.BLToLocal2000(lon, lat, Srid, out double x, out double y);
                return new MapPoint() { X = x, Y = y};
            }
            throw new NotSupportedException();
        }

        public override MapPoint XYToLatLon(double x, double y)
        {
            if (Srid > 100000)
            {
                TransverseMercatorHelper.Local2000ToBL(x, y, Srid, out double lon, out double lat);
                return new MapPoint() { X = lon, Y = lat};
            }
            throw new NotImplementedException();
        }
    }
}