using System;
using Ataoge.GisCore.Utilities;

namespace Ataoge.GisCore.Wmts
{
    public class TmsTileSystem : ProjectedTileSystem
    {
        public TmsTileSystem()
        {
            
        }

        public override MapPoint LatLonToXY(double lat, double lon)
        {
             double mx = lon * this.OriginXShift / 180.0;
            double my = Math.Log(Math.Tan((90 + lat) * Math.PI / 360.0)) / (Math.PI / 180.0);
            my = my * this.OriginYShift / 180.0;
            return new MapPoint() {X = mx, Y = my};
            //CoordinateTransform.WGS84ToWebMercator(lat, lon, out double x, out double y);
            //return new MapPoint(){ X = x, Y = y};
        }

        

        public override MapPoint XYToLatLon(double x, double y)
        {
             double lon = (x / this.OriginXShift) * 180.0;
            double lat = (y / this.OriginYShift) * 180.0;
            lat = 180 / Math.PI * (2 * Math.Atan(Math.Exp(lat * Math.PI / 180.0)) - Math.PI / 2.0);

            //CoordinateTransform.WebMercatorToWGS84(y, x, out double lat, out double lon);
            return new MapPoint() { X= lon, Y = lat};
        }


    }
}