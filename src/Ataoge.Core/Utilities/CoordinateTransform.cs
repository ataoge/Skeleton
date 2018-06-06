using System;

namespace Ataoge.Utilities
{
    public static class CoordinateTransform
    {
        const double a = 6378245.0;
        const double ee = 0.00669342162296594323;


        #region  WGS84 && GCJ02 Transformation
        /// <summary>
        /// OutofChina
        /// </summary>
        /// <param name="lat"></param>
        /// <param name="lon"></param>
        /// <returns></returns>
        static bool OutOfChina(double lat, double lon)
        {
            if (lon < 72.004 || lon > 137.8347)
                return true;
            if (lat < 0.8293 || lat > 55.8271)
                return true;
            return false;
        }

        /// <summary>
        /// TransformLat
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        static double TransformLat(double x, double y)
        {
            double ret = -100.0 + 2.0 * x + 3.0 * y + 0.2 * y * y + 0.1 * x * y + 0.2 * Math.Sqrt(Math.Abs(x));
            ret += (20.0 * Math.Sin(6.0 * x * Math.PI) + 20.0 * Math.Sin(2.0 * x * Math.PI)) * 2.0 / 3.0;
            ret += (20.0 * Math.Sin(y * Math.PI) + 40.0 * Math.Sin(y / 3.0 * Math.PI)) * 2.0 / 3.0;
            ret += (160.0 * Math.Sin(y / 12.0 * Math.PI) + 320 * Math.Sin(y * Math.PI / 30.0)) * 2.0 / 3.0;
            return ret;
        }

        /// <summary>
        /// TransformLon
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        static double TransformLon(double x, double y)
        {
            double ret = 300.0 + x + 2.0 * y + 0.1 * x * x + 0.1 * x * y + 0.1 * Math.Sqrt(Math.Abs(x));
            ret += (20.0 * Math.Sin(6.0 * x * Math.PI) + 20.0 * Math.Sin(2.0 * x * Math.PI)) * 2.0 / 3.0;
            ret += (20.0 * Math.Sin(x * Math.PI) + 40.0 * Math.Sin(x / 3.0 * Math.PI)) * 2.0 / 3.0;
            ret += (150.0 * Math.Sin(x / 12.0 * Math.PI) + 300.0 * Math.Sin(x / 30.0 * Math.PI)) * 2.0 / 3.0;
            return ret;
        }

        /// <summary>
        /// Delta
        /// </summary>
        /// <param name="lat"></param>
        /// <param name="lon"></param>
        /// <param name="dlat"></param>
        /// <param name="dlon"></param>
        /// <returns></returns>
        static void Delta(double lat, double lng, out double dLat, out double dLng)
        {
            dLat = TransformLat(lng - 105.0, lat - 35.0);
            dLng = TransformLon(lng - 105.0, lat - 35.0);
            double radLat = lat / 180.0 * Math.PI;
            double magic = Math.Sin(radLat);
            magic = 1 - ee * magic * magic;
            double sqrtMagic = Math.Sqrt(magic);
            dLat = (dLat * 180.0) / ((a * (1 - ee)) / (magic * sqrtMagic) * Math.PI);
            dLng = (dLng * 180.0) / (a / sqrtMagic * Math.Cos(radLat) * Math.PI);
        }

        /// <summary>
        /// World Geodetic System ==> Mars Geodetic System
        /// WGS84坐标转火星坐标GCJ02
        /// </summary>
        /// <param name="wgsLat"></param>
        /// <param name="wgsLon"></param>
        /// <param name="gcjLat"></param>
        /// <param name="gcjLon"></param>
        public static void WGS84ToGCJ02(double wgsLat, double wgsLon, out double gcjLat, out double gcjLon)
        {
            if (OutOfChina(wgsLat, wgsLon))
            {
                gcjLat = wgsLat;
                gcjLon = wgsLon;
                return;
            }
            double dLat = TransformLat(wgsLon - 105.0, wgsLat - 35.0);
            double dLon = TransformLon(wgsLon - 105.0, wgsLat - 35.0);
            double radLat = wgsLat / 180.0 * Math.PI;
            double magic = Math.Sin(radLat);
            magic = 1 - ee * magic * magic;
            double sqrtMagic = Math.Sqrt(magic);
            dLat = (dLat * 180.0) / ((a * (1 - ee)) / (magic * sqrtMagic) * Math.PI);
            dLon = (dLon * 180.0) / (a / sqrtMagic * Math.Cos(radLat) * Math.PI);
            gcjLat = wgsLat + dLat;
            gcjLon = wgsLon + dLon;
        }

        /// <summary>
        /// GCJ02火星坐标转WGS84, 快速转换
        /// </summary>
        /// <param name="gcjLat"></param>
        /// <param name="gcjLon"></param>
        /// <param name="wgsLat"></param>
        /// <param name="wgsLon"></param>
        public static void GCJ022ToWGS84(double gcjLat, double gcjLon, out double wgsLat, out double wgsLon)
        {
            if (OutOfChina(gcjLat, gcjLon))
            {
                wgsLat = gcjLat;
                wgsLon = gcjLon;
            }
            double dLat;
            double dLng;
            Delta(gcjLat, gcjLon, out dLat, out dLng);
            wgsLat = gcjLat - dLat;
            wgsLon = gcjLon - dLng;
        }

        /// <summary>
        /// GCJ02火星坐标转WGS84, 精确转换
        /// </summary>
        /// <param name="gcjLat"></param>
        /// <param name="gcjLon"></param>
        /// <param name="wgsLat"></param>
        /// <param name="wgsLon"></param>
        public static void GCJ02ToWGS84Exact(double gcjLat, double gcjLon, out double wgsLat, out double wgsLon)
        {
            double initDelta = 0.01;
            double threshold = 0.000001;
            double dLat = initDelta;
            double dLng = initDelta;
            double mLat = gcjLat - dLat;
            double mLng = gcjLon - dLng;
            double pLat = gcjLat + dLat;
            double pLng = gcjLon + dLng;
            wgsLat = 0;
            wgsLon = 0;

            for (int i = 0; i < 30; i++)
            {
                wgsLat = (mLat + pLat) / 2;
                wgsLon = (mLng + pLng) / 2;
                double tmpLat;
                double tmpLng;
                WGS84ToGCJ02(wgsLat, wgsLon, out tmpLat, out tmpLng);
                dLat = tmpLat - gcjLat;
                dLng = tmpLng - gcjLon;
                if ((Math.Abs(dLat) < threshold) && (Math.Abs(dLng) < threshold))
                {
                    return;
                }
                if (dLat > 0)
                {
                    pLat = wgsLat;
                }
                else
                {
                    mLat = wgsLat;
                }
                if (dLng > 0)
                {
                    pLng = wgsLon;
                }
                else
                {
                    mLng = wgsLon;
                }
            }
        }

        #endregion

        #region Baidu MC && LL Transformation
        const double X_PI = Math.PI * 3000.0 / 180.0;

        /// <summary>
        /// GCJ02火星坐标转百度经纬度坐标BD09
        /// </summary>
        /// <param name="gcjLat"></param>
        /// <param name="gcjLon"></param>
        /// <param name="bdLat"></param>
        /// <param name="bdLon"></param>
        public static void GCJ02ToBD09(double gcjLat, double gcjLon, out double bdLat, out double bdLon)
        {
            double x = gcjLon;
            double y = gcjLat;
            double z = Math.Sqrt(x * x + y * y) + 0.00002 * Math.Sin(y * X_PI);
            double theta = Math.Atan2(y, x) + 0.000003 * Math.Cos(x * X_PI);
            bdLon = z * Math.Cos(theta) + 0.0065;
            bdLat = z * Math.Sin(theta) + 0.006;
        }

        /// <summary>
        /// 百度经纬度坐标BD09转GCJ02火星坐标
        /// </summary>
        /// <param name="bdLat"></param>
        /// <param name="bdLon"></param>
        /// <param name="gcjLat"></param>
        /// <param name="gcjLon"></param>
        public static void BD09ToGCJ02(double bdLat, double bdLon, out double gcjLat, out double gcjLon)
        {
            double x = bdLon - 0.0065;
            double y = bdLat - 0.006;
            double z = Math.Sqrt(x * x + y * y) - 0.00002 * Math.Sin(y * X_PI);
            double theta = Math.Atan2(y, x) - 0.000003 * Math.Cos(x * X_PI);
            gcjLon = z * Math.Cos(theta);
            gcjLat = z * Math.Sin(theta);
        }

        //private static double EARTHRADIUS = 6370996.81; //百度
        private static double[] MCBAND = { 12890594.86, 8362377.87, 5591021d, 3481989.83, 1678043.12, 0d };
        private static double[] LLBAND = { 75d, 60d, 45d, 30d, 15d, 0d };
        private static double[][] MC2LL = new double[6][] { new double[] { 1.410526172116255e-8, 0.00000898305509648872, -1.9939833816331, 200.9824383106796, -187.2403703815547, 91.6087516669843, -23.38765649603339, 2.57121317296198, -0.03801003308653, 17337981.2 }, new double[] { -7.435856389565537e-9, 0.000008983055097726239, -0.78625201886289, 96.32687599759846, -1.85204757529826, -59.36935905485877, 47.40033549296737, -16.50741931063887, 2.28786674699375, 10260144.86 }, new double[] { -3.030883460898826e-8, 0.00000898305509983578, 0.30071316287616, 59.74293618442277, 7.357984074871, -25.38371002664745, 13.45380521110908, -3.29883767235584, 0.32710905363475, 6856817.37 }, new double[] { -1.981981304930552e-8, 0.000008983055099779535, 0.03278182852591, 40.31678527705744, 0.65659298677277, -4.44255534477492, 0.85341911805263, 0.12923347998204, -0.04625736007561, 4482777.06 }, new double[] { 3.09191371068437e-9, 0.000008983055096812155, 0.00006995724062, 23.10934304144901, -0.00023663490511, -0.6321817810242, -0.00663494467273, 0.03430082397953, -0.00466043876332, 2555164.4 }, new double[] { 2.890871144776878e-9, 0.000008983055095805407, -3.068298e-8, 7.47137025468032, -0.00000353937994, -0.02145144861037, -0.00001234426596, 0.00010322952773, -0.00000323890364, 826088.5 } };
        private static double[][] LL2MC = new double[6][] { new double[] { -0.0015702102444, 111320.7020616939, 1704480524535203d, -10338987376042340d, 26112667856603880d, -35149669176653700d, 26595700718403920d, -10725012454188240d, 1800819912950474d, 82.5 }, new double[] { 0.0008277824516172526, 111320.7020463578, 647795574.6671607, -4082003173.641316, 10774905663.51142, -15171875531.51559, 12053065338.62167, -5124939663.577472, 913311935.9512032, 67.5 }, new double[] { 0.00337398766765, 111320.7020202162, 4481351.045890365, -23393751.19931662, 79682215.47186455, -115964993.2797253, 97236711.15602145, -43661946.33752821, 8477230.501135234, 52.5 }, new double[] { 0.00220636496208, 111320.7020209128, 51751.86112841131, 3796837.749470245, 992013.7397791013, -1221952.21711287, 1340652.697009075, -620943.6990984312, 144416.9293806241, 37.5 }, new double[] { -0.0003441963504368392, 111320.7020576856, 278.2353980772752, 2485758.690035394, 6070.750963243378, 54821.18345352118, 9540.606633304236, -2710.55326746645, 1405.483844121726, 22.5 }, new double[] { -0.0003218135878613132, 111320.7020701615, 0.00369383431289, 823725.6402795718, 0.46104986909093, 2351.343141331292, 1.58060784298199, 8.77738589078284, 0.37238884252424, 7.45 } };

        
        /// <summary>
        /// 百度墨卡托坐标转百度经纬度坐标
         /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="lat"></param>
        /// <param name="lon"></param>
        public static void BD09_MC2LL(double x, double y, out double lat, out double lon)
        {
            double[] cF = null;
            x = Math.Abs(x);
            y = Math.Abs(y);

            for (int cE = 0; cE < MCBAND.Length; cE++)
            {
                if (y >= MCBAND[cE])
                {
                    cF = MC2LL[cE];
                    break;
                }
            }
            converter(x, y, cF, out lon, out lat);
        }


        /// <summary>
        ///  百度经纬度坐标转百度墨卡托坐标
        /// </summary>
        /// <param name="lat"></param>
        /// <param name="lon"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public static void BDO9_LL2MC(double lat, double lon, out double x, out double y)
        {
            Double[] cE = null;
            lon = getLoop(lon, -180, 180);
            lat = getRange(lat, -74, 74);
            for (int i = 0; i < LLBAND.Length; i++)
            {
                if (lat >= LLBAND[i])
                {
                    cE = LL2MC[i];
                    break;
                }
            }
            if (cE == null)
            {
                for (int i = LLBAND.Length - 1; i >= 0; i--)
                {
                    if (lat <= -LLBAND[i])
                    {
                        cE = LL2MC[i];
                        break;
                    }
                }
            }

            converter(lon, lat, cE, out x, out y);
        }


        private static void converter(double x, double y, Double[] cE, out double ox, out double oy)
        {
            double xTemp = cE[0] + cE[1] * Math.Abs(x);
            double cC = Math.Abs(y) / cE[9];
            double yTemp = cE[2] + cE[3] * cC + cE[4] * cC * cC + cE[5] * cC * cC * cC + cE[6] * cC * cC * cC * cC + cE[7] * cC * cC * cC * cC * cC + cE[8] * cC * cC * cC * cC * cC * cC;
            xTemp *= (x < 0 ? -1 : 1);
            yTemp *= (y < 0 ? -1 : 1);

            ox = xTemp;
            oy = yTemp;

        }

        private static double getLoop(double lng, int min, int max)
        {
            while (lng > max)
            {
                lng -= max - min;
            }
            while (lng < min)
            {
                lng += max - min;
            }
            return lng;
        }
       
        private static double getRange(double lat, int min, int max)
        {
            lat = Math.Max(lat, min);
            lat = Math.Min(lat, max);
            return lat;
        }
        #endregion


        #region WGS && WebMercator Transformation

        private const double RADIANS_PR_DEGREES = 0.017453292519943295;
        private const double DEGREES_PR_RADIANS = 57.295779513082323;
        public const double WGS84_EARTHRADIUS = 6378137.0;

        /// <summary>
        /// WGS84坐标转Web墨卡托坐标
        /// </summary>
        /// <param name="lat"></param>
        /// <param name="lon"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public static void WGS84ToWebMercator(double lat, double lon, out double x, out double y)
        {

            if (lat <= -90.0)
            {
                y = double.NegativeInfinity;
            }
            else
            {
                if (lat > 90.0)
                {
                    y = double.PositiveInfinity;
                }
                else
                {
                    double num = lat * RADIANS_PR_DEGREES;
                    y = 3189068.5 * Math.Log((1.0 + Math.Sin(num)) / (1.0 - Math.Sin(num)));
                }
            }
            x = WGS84_EARTHRADIUS * (lon * RADIANS_PR_DEGREES);

        }

        /// <summary>
        /// Web墨卡托坐标转WGS84坐标
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="lat"></param>
        /// <param name="lon"></param>
        public static void WebMercatorToWGS84(double y, double x, out double lat, out double lon)
        {
            double num = x / WGS84_EARTHRADIUS;
            double num2 = num * DEGREES_PR_RADIANS;
            double num3 = Math.Floor((num2 + 180.0) / 360.0);
            double num4 = num2 - num3 * 360.0;
            double num5 = 1.5707963267948966 - 2.0 * Math.Atan(Math.Exp(-1.0 * y / WGS84_EARTHRADIUS));
            num5 *= DEGREES_PR_RADIANS;
            lat = num5;
            lon = num4 + num3 * 360.0;
        }

        #endregion

        /// <summary>
        /// 计算两经纬度之间的球面距离，默认为WGS84
        /// </summary>
        /// <param name="latA"></param>
        /// <param name="lonA"></param>
        /// <param name="latB"></param>
        /// <param name="lonB"></param>
        /// <param name="earthR"></param>
        /// <returns></returns>
        public static double Distance(double latA, double lonA, double latB, double lonB, double earthR = WGS84_EARTHRADIUS)
        {
            double x = Math.Cos(latA * Math.PI / 180) * Math.Cos(latB * Math.PI / 180) * Math.Cos((lonA - lonB) * Math.PI / 180);
            double y = Math.Sin(latA * Math.PI / 180) * Math.Sin(latB * Math.PI / 180);
            double s = x + y;
            if (s > 1)
                s = 1;
            if (s < -1)
                s = -1;
            double alpha = Math.Acos(s);
            var distance = alpha * earthR;
            return distance;
        }
        

    }
}