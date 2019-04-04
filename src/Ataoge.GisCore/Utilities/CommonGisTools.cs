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


    }
}