using System;
using Ataoge.GisCore.Utilities;

namespace Ataoge.GisCore.Wmts
{
    public class GoogleTileSystem : ProjectedTileSystem
    {
        public override double OriginYShift
        {
            get
            {
                return -base.OriginYShift;
            }
        }

        public override MapPoint LatLonToXY(double lat, double lon)
        {
          
            CoordinateTransform.WGS84ToWebMercator(lat, lon, out double x, out double y);
            return new MapPoint(){ X = x, Y = y};
        }

        

        public override MapPoint XYToLatLon(double x, double y)
        {
           
            CoordinateTransform.WebMercatorToWGS84(y, x, out double lat, out double lon);
            return new MapPoint() { X= lon, Y = lat};
        }

        public override PixelPoint XYToPixel(double x, double y, int zoom)
        {
            double res = this.GetResolution(zoom);
            int px = (int)Math.Ceiling((x + this.OriginXShift) / res);
            int py = (int)Math.Ceiling((y + this.OriginYShift) / res) * -1;
            return new PixelPoint(){X = px, Y = py}; 
            
        }

        public override MapPoint PixelToXY(int px, int py, int zoom)
        {
             double res = this.GetResolution(zoom);
            double mx = px * res - this.OriginXShift;
            double my = -py * res - this.OriginYShift;
            return new MapPoint(){ X = mx, Y = my};
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
    }
}