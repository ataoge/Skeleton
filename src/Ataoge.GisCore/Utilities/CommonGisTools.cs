using System;

namespace Ataoge.GisCore.Utilities
{
    public static class CommonGisTools
    {
        public const double ARCGIS_ORIGN = 20037508.342787001;
        public const double ARCGIS_INITSCALE = 295829355.45453244;
        public const double ARCGIS_INITRESOLUTION = 78271.516964011724;
        public static double ScaleToResolution(double resolution, double dpi = 96.0)
        {
            return resolution * dpi / 0.0254;  // 1 英寸= 0.0254米 WMTS Default DPI is 90.7 , 100.0/ 90.7 * 0.00254 = 0.0028004410143329657
        }

        public static double ResolutionToScale(double scale, double dpi = 96.0)
        {
            return scale * 0.0254 / dpi;
        }


        ///<summary>
        /// 将切片的坐标转换为MapBox经纬度坐标
        ///</summary>
        public static void LocalXYToMapBox(double x, double y, out double lon, out double lat, double xOrigin, double yOrigin, double initResolution)
        {
            lon = (x - xOrigin) / (512 * initResolution) * 360 -180;
            var y2 = 180 -(yOrigin - y) /(512 * initResolution) * 360;
            lat = 360 / Math.PI * Math.Atan(Math.Exp(y2 * Math.PI / 180)) -90;
        }

        ///<summary>
        /// 将MapBox的经纬度坐标转换为切片的坐标
        ///</summary>
        public static void MapBoxToLocalXY(double lon, double lat, out double x, out double y, double xOrigin, double yOrigin, double initResolution)
        {
            x = (180 + lon) / 360 * (512 * initResolution) + xOrigin;
            y = (180 - (180 / Math.PI * Math.Log(Math.Tan(Math.PI / 4 + lat * Math.PI / 360)))) /  360;
            y = yOrigin - y * 512 * initResolution;
        }


        public static MapExtent GetTileExtent(int z, int x, int y, int tileSize = 256, double ox = -20037508.342787, double oy = 20037508.342787, double initResolution = 78271.51696401172)
        {
            double res = initResolution / Math.Pow(2, z);
            double left = x * tileSize * res + ox;
            double top = oy - y * tileSize * res;
            double right = (x+1)  * tileSize * res + ox;
            double bottom = oy - (y+1) * tileSize * res;
            return new MapExtent(){MinX = left, MinY = bottom, MaxX = right, MaxY = top};
        }
    }
}