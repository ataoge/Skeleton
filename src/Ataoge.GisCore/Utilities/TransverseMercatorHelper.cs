using System;
using System.Collections.Generic;

namespace Ataoge.GisCore.Utilities
{
    public static class TransverseMercatorHelper
    {
        static TransverseMercatorHelper()
        {
            //WuHan2000
            _dictArgs.Add(420100, new TMInfo(){ CentralMeridian = 114.3333333333, FalseEasting = 800000, FalseNorthing = -3000000});
            //Foshan2000
            _dictArgs.Add(440600, new TMInfo(){ CentralMeridian = 113.0, FalseEasting = 700000.0});
            //Jining2000
            _dictArgs.Add(370800, new TMInfo(){ CentralMeridian = 117.0, FalseEasting = 39500000.0});

            //GZ2000
            _dictArgs.Add(440100, new TMInfo(){ CentralMeridian = 113.0,});

        }

        private static IDictionary<int, TMInfo> _dictArgs = new Dictionary<int, TMInfo>();

        public const double CGCS2000_SEMIMAJORAXIS = 6378137.0; //CGSC2000 WGS84 椭球体 长半轴
        public const double CGCS2000_SEMIMINORAXIS = 6356752.3141403558; // CGSC2000 WGS84 椭球体 长半轴
        public const double CGCS2000_FLATTENING = 1 / 298.257222101; // CGCS2000 WGS84 扁率
        public const double CGCS2000_INVERSEFLATTENING = 298.257222101; // CGCS2000 WGS84 扁率的倒数

        public const double WGS84_SEMIMAJORAXIS = 6378137.0; //WGS84 椭球体 长半轴
        public const double WGS84_SEMIMINORAXIS = 6356752.3142451795; // WGS84 椭球体 长半轴
        public const double WGS84_FLATTENING = 1 / 298.257223563; // WGS84 扁率
        public const double WGS84_INVERSEFLATTENING = 298.257223563; // WGS84 扁率的倒数

        public const double KRASOVSKY54_SEMIMAJORAXIS = 6378245.0; //Krasovsky BEIJING54 椭球体 长半轴
        public const double KRASOVSKY54_SEMIMINORAXIS = 6356863.018773047; // Krasovsky BEIJING54 椭球体 长半轴
        public const double KRASOVSKY54_FLATTENING = 1 / 298.3; // Krasovsky BEIJING54 扁率
        public const double KRASOVSKY54_INVERSEFLATTENING = 298.3; // Krasovsky BEIJING54 扁率的倒数

        public const double IAG75_XIAN80_SEMIMAJORAXIS = 6378140.0; //1975国际椭球体 西安80 椭球体 长半轴
        public const double IAG75_XIAN80_SEMIMINORAXIS = 6356755.288157528; // Krasovsky BEIJING54 椭球体 长半轴
        public const double IAG75_XIAN80_FLATTENING = 1 / 298.2570; // Krasovsky BEIJING54 扁率
        public const double IAG75_XIAN80_INVERSEFLATTENING = 298.2570; // Krasovsky BEIJING54 扁率的倒数

        public const double ANGULARUNIT_DEGREE = 0.0174532925199433; // 角度单位 弧度  iPI 3.1415926535898/180.0;

         ///<summary>
        /// CGCS2000 经纬度转地方平面坐标
        ///<summary>
        /// <param name="lon">经度,小数度</param>
        /// <param name="lat">纬度，小数度</param>
        /// <param name="srid">地方两千坐标srid</param>
        /// <param name="x">返回X坐标，单位米</param>
        /// <param name="y">返回Y坐标，单位米</param>
        public static void BLToLocal2000(double lon, double lat, int srid, out double x, out double y)
        {
            if (!_dictArgs.ContainsKey(srid))
                throw new NotSupportedException();
            var tmInfo = _dictArgs[srid];
            BLToYXCore(lat, lon, out y, out x, CGCS2000_SEMIMAJORAXIS, CGCS2000_FLATTENING, tmInfo.CentralMeridian, tmInfo.FalseEasting, tmInfo.FalseNorthing, tmInfo.ScaleFactor, tmInfo.LatitudeOfOrigin);
        }

        ///<summary>
        /// 地方2000 平面坐标转经纬度
        ///<summary>
        /// <param name="y">返回Y坐标，单位米</param>
        /// <param name="x">返回X坐标，单位米</param>
        /// <param name="srid">地方两千坐标srid</param>
        /// <param name="lat">纬度，小数度</param>
        /// <param name="lon">经度,小数度</param>
        public static void Local2000ToBL(double x, double y, int srid, out double lon, out double lat)
        {
             if (!_dictArgs.ContainsKey(srid))
                throw new NotSupportedException();
            var tmInfo = _dictArgs[srid];
            YXToBLCore(x, y, out lat, out lon, CGCS2000_SEMIMAJORAXIS, CGCS2000_FLATTENING, tmInfo.CentralMeridian, tmInfo.FalseEasting, tmInfo.FalseNorthing, tmInfo.ScaleFactor, tmInfo.LatitudeOfOrigin);
        }

        ///<summary>
        /// CGCS2000 经纬度转平面坐标
        ///<summary>
        /// <param name="lon">经度,小数度</param>
        /// <param name="lat">纬度，小数度</param>
        /// <param name="x">返回X坐标，单位米</param>
        /// <param name="y">返回Y坐标，单位米</param>
        /// <param name="centralMeridian">中央经线</param>
        /// <param name="flattening">东偏移量, 单位为米</param>
         /// <param name="falseNorthing">北偏移量， 单位为米，默认为0</param>
        public static void BLToXY_CGCS2000(double lon, double lat, out double x, out double y, double centralMeridian, double falseEasting, double falseNorthing = 0.0)
        {
            BLToYXCore(lat, lon, out y, out x, CGCS2000_SEMIMAJORAXIS, CGCS2000_FLATTENING, centralMeridian, falseEasting, falseNorthing);
        }

        ///<summary>
        /// CGCS2000 平面坐标转经纬度
        ///<summary>
        /// <param name="y">返回Y坐标，单位米</param>
        /// <param name="x">返回X坐标，单位米</param>
        /// <param name="lat">纬度，小数度</param>
        /// <param name="lon">经度,小数度</param>
        /// <param name="centralMeridian">中央经线</param>
        /// <param name="flattening">东偏移量, 单位为米</param>
         /// <param name="falseNorthing">北偏移量， 单位为米，默认为0</param>
        public static void XYToBL_CGCS2000(double x, double y, out double lon, out double lat, double centralMeridian, double falseEasting, double falseNorthing = 0.0)
        {
            YXToBLCore(y, x, out lat, out lon, CGCS2000_SEMIMAJORAXIS, CGCS2000_FLATTENING, centralMeridian, falseEasting, falseNorthing);
        }

        ///<summary>
        /// xian80 经纬度转平面坐标
        ///<summary>
        /// <param name="lon">经度,小数度</param>
        /// <param name="lat">纬度，小数度</param>
        /// <param name="x">返回X坐标，单位米</param>
        /// <param name="y">返回Y坐标，单位米</param>
        /// <param name="centralMeridian">中央经线</param>
        /// <param name="flattening">东偏移量, 单位为米</param>
         /// <param name="falseNorthing">北偏移量， 单位为米，默认为0</param>
        public static void BLToXY_XIAN80(double lon, double lat, out double x, out double y, double centralMeridian, double falseEasting, double falseNorthing = 0.0)
        {
            BLToYXCore(lat, lon, out y, out x, IAG75_XIAN80_SEMIMAJORAXIS, IAG75_XIAN80_FLATTENING, centralMeridian, falseEasting, falseNorthing);
        }

        ///<summary>
        /// xian80 平面坐标转经纬度
        ///<summary>
        /// <param name="y">返回Y坐标，单位米</param>
        /// <param name="x">返回X坐标，单位米</param>
        /// <param name="lat">纬度，小数度</param>
        /// <param name="lon">经度,小数度</param>
        /// <param name="centralMeridian">中央经线</param>
        /// <param name="flattening">东偏移量, 单位为米</param>
         /// <param name="falseNorthing">北偏移量， 单位为米，默认为0</param>
        public static void XYToBL_XIAN80(double x, double y, out double lon, out double lat, double centralMeridian, double falseEasting, double falseNorthing = 0.0)
        {
            YXToBLCore(y, x, out lat, out lon, IAG75_XIAN80_SEMIMAJORAXIS, IAG75_XIAN80_FLATTENING, centralMeridian, falseEasting, falseNorthing);
        }

        ///<summary>
        /// Beijing54 经纬度转平面坐标
        ///<summary>
        /// <param name="lon">经度,小数度</param>
        /// <param name="lat">纬度，小数度</param>
        /// <param name="x">返回X坐标，单位米</param>
        /// <param name="y">返回Y坐标，单位米</param>
        /// <param name="centralMeridian">中央经线</param>
        /// <param name="flattening">东偏移量, 单位为米</param>
         /// <param name="falseNorthing">北偏移量， 单位为米，默认为0</param>
        public static void BLToXY_BJ54(double lon, double lat, out double x, out double y, double centralMeridian, double falseEasting, double falseNorthing = 0.0)
        {
            BLToYXCore(lat, lon, out y, out x, KRASOVSKY54_SEMIMAJORAXIS, KRASOVSKY54_FLATTENING, centralMeridian, falseEasting, falseNorthing);
        }

        ///<summary>
        /// Beijing54 平面坐标转经纬度
        ///<summary>
        /// <param name="y">返回Y坐标，单位米</param>
        /// <param name="x">返回X坐标，单位米</param>
        /// <param name="lat">纬度，小数度</param>
        /// <param name="lon">经度,小数度</param>
        /// <param name="centralMeridian">中央经线</param>
        /// <param name="flattening">东偏移量, 单位为米</param>
         /// <param name="falseNorthing">北偏移量， 单位为米，默认为0</param>
        public static void XYToBL_BJ54(double x, double y, out double lon, out double lat, double centralMeridian, double falseEasting, double falseNorthing = 0.0)
        {
            YXToBLCore(y, x, out lat, out lon, KRASOVSKY54_SEMIMAJORAXIS, KRASOVSKY54_FLATTENING, centralMeridian, falseEasting, falseNorthing);
        }

        //<summary>
        /// 西安80高斯分带投影 经纬度转平面坐标
        ///<summary>
        /// <param name="lon">经度,小数度</param>
        /// <param name="lat">纬度，小数度</param>
        /// <param name="x">返回X坐标，单位米</param>
        /// <param name="y">返回Y坐标，单位米</param>
        /// <param name="zoneWidth">3度带还是6度带， 默认3度带</param>
        /// <param name="withZoneNumber">大于0则返回的X坐标是加上带号，等于0不加，默认=0</param>
        public static void GKBLToXY_XIAN80(double lon, double lat, out double x, out double y,int zoneWidth = 3, int withZoneNumber = 0)
        {
            BLToYX_GK(lat, lon, out y, out x, IAG75_XIAN80_SEMIMAJORAXIS, IAG75_XIAN80_FLATTENING, zoneWidth, withZoneNumber);
        }

        ///<summary>
        /// 西安80高斯分带投影 平面坐标转经纬度
        ///<summary>
        /// <param name="y">返回Y坐标，单位米</param>
        /// <param name="x">返回X坐标，单位米</param>
        /// <param name="lat">纬度，小数度</param>
        /// <param name="lon">经度,小数度</param>
        /// <param name="zoneWidth">3度带还是6度带， 默认3度带</param>
        /// <param name="withZoneNumber">大于0则是带号，等于0表示x坐标已包含带号，默认=0</param>
        public static void GKXYToBL_XIAN80(double x, double y, out double lon, out double lat,int zoneWidth = 3, int withZoneNumber = 0)
        {
            YXToBL_GK(y, x, out lat, out lon, IAG75_XIAN80_SEMIMAJORAXIS, IAG75_XIAN80_FLATTENING, zoneWidth, withZoneNumber);
        }

        ///<summary>
        ///Gauss_Kruger 高斯克吕格分带经纬度转平面
        ///</summary>
        /// <param name="lat">纬度，小数度</param>
        /// <param name="lon">经度,小数度</param>
        /// <param name="y">返回Y坐标，单位米</param>
        /// <param name="x">返回X坐标，单位米</param>
        /// <param name="smeimajor">椭球体长半轴</param>
        /// <param name="flattening">椭球体扁率</param>
        /// <param name="zoneWidth">3度带还是6度带， 默认3度带</param>
        /// <param name="withZoneNumber">大于0则返回的X坐标是加上带号，等于0不加，默认=0</param>
        private static void BLToYX_GK(double lat, double lon, out double y, out double x, double smeimajor, double flattening, int zoneWidth = 3, int withZoneNumber = 0)
        {
            int projNo = GetZoneNumber(lon, zoneWidth);
            var centralMeridian = GetCentralMeridian(projNo, zoneWidth);
            //500,000 米的东偏移量。赤道以南区域的北偏移量为 10,000,000 米，
            BLToYXCore(lat, lon, out y, out x, smeimajor, flattening, centralMeridian, 500000, lat < 0 ? 10000000 : 0);
            if (withZoneNumber > 0)
            {
                x += 1000000L * projNo;
            }
            return;
        }

        ///<summary>
        ///Gauss_Kruger 高斯克吕格分带平面转经纬度
        ///</summary>
        /// <param name="y">返回Y坐标，单位米</param>
        /// <param name="x">返回X坐标，单位米</param>
        /// <param name="lat">纬度，小数度</param>
        /// <param name="lon">经度,小数度</param>
        /// <param name="smeimajor">椭球体长半轴</param>
        /// <param name="flattening">椭球体扁率</param>
        /// <param name="zoneWidth">3度带还是6度带， 默认3度带</param>
        /// <param name="withZoneNumber">大于0则是带号，等于0表示x坐标已包含带号，默认=0</param>
        /// <param name="isSouth">是否南半球，默认false</param>
        private static void YXToBL_GK(double y, double x, out double lat, out double lon, double smeimajor, double flattening, int zoneWidth = 3, int withZoneNumber = 0, bool isSouth = false)
        {
            int projNo = withZoneNumber;
            if (withZoneNumber == 0)
            {
                projNo =  (int)(x / 1000000L); //查找带号
                x -= projNo * 1000000L;
            }
            var centralMeridian = GetCentralMeridian(projNo, zoneWidth);
            YXToBLCore(y, x, out lat, out lon, smeimajor, flattening, centralMeridian, 500000, isSouth ? 100000000 : 0);
            if (isSouth)
                lat *= -1;
            return;
        }

        ///<summary>
        ///根据带号计算中央经线
        ///</summary>
        private static double GetCentralMeridian(int projNo, int zoneWidth = 3)
        {
            if (zoneWidth == 6)
            {
                return projNo * zoneWidth - zoneWidth / 2;
            }
            return projNo * zoneWidth;
        }

        ///<summary>
        ///根据经线计算带号
        ///</summary>
        private static int GetZoneNumber(double longitude, int zoneWidth = 3)
        {
            if (zoneWidth == 6)
            {
                return (int)Math.Round((longitude + 3) / zoneWidth);
            }
            return (int)Math.Round(longitude / 3);
        }

        

        ///<summary>
        /// 经纬度转平面坐标
        ///<summary>
        /// <param name="lat">纬度，小数度</param>
        /// <param name="lon">经度,小数度</param>
        /// <param name="y">返回Y坐标，单位米</param>
        /// <param name="x">返回X坐标，单位米</param>
        /// <param name="smeimajor">椭球体长半轴</param>
        /// <param name="flattening">椭球体扁率</param>
        /// <param name="centralMeridian">中央经线</param>
        /// <param name="flattening">东偏移量, 单位为米</param>
         /// <param name="falseNorthing">北偏移量， 单位为米，默认为0</param>
        /// <param name="scaleFactor">比例因子 UTM为0.9996，我国为1.0 </param>
        /// <param name="latitudeOfOrigin">起始纬度，默认为0</param>
        private static void BLToYXCore(double lat, double lon, out double y, out double x, double smeimajor, double flattening, double centralMeridian, double falseEasting, double falseNorthing = 0.0, double scaleFactor = 1.0, double latitudeOfOrigin = 0.0)
        {
            double longitude1, latitude1, longitude0, xval, yval; //X0, Y0,  latitude0,
            double a, f, e2, ee, NN, T, C, A, M;
            double iPI = ANGULARUNIT_DEGREE; ////3.1415926535898/180.0;
            
            a = smeimajor; f = flattening; // 长半轴和扁率

            longitude0 = centralMeridian * iPI; //中央经线 转为弧度
            longitude1 = lon * iPI; //经度转换为弧度
            latitude1 = lat * iPI; //纬度转换为弧度

            e2 = 2 * f - f * f;
            ee = e2 * (1.0 - e2);
            NN = a / Math.Sqrt(1.0 - e2 * Math.Sin(latitude1) * Math.Sin(latitude1));
            T = Math.Tan(latitude1) * Math.Tan(latitude1);
            C = ee * Math.Cos(latitude1) * Math.Cos(latitude1);
            A = (longitude1 - longitude0) * Math.Cos(latitude1);
            M = a * ((1 - e2 / 4 - 3 * e2 * e2 / 64 - 5 * e2 * e2 * e2 / 256) * latitude1 - (3 * e2 / 8 + 3 * e2 * e2 / 32 + 45 * e2 * e2
            * e2 / 1024) * Math.Sin(2 * latitude1)
            + (15 * e2 * e2 / 256 + 45 * e2 * e2 * e2 / 1024) * Math.Sin(4 * latitude1) - (35 * e2 * e2 * e2 / 3072) * Math.Sin(6 * latitude1));
            xval = NN * (A + (1 - T + C) * A * A * A / 6 + (5 - 18 * T + T * T + 72 * C - 58 * ee) * A * A * A * A * A / 120);
            yval = M + NN * Math.Tan(latitude1) * (A * A / 2 + (5 - T + 9 * C + 4 * C * C) * A * A * A * A / 24
            + (61 - 58 * T + T * T + 600 * C - 330 * ee) * A * A * A * A * A * A / 720);


            x = xval * scaleFactor + falseEasting; 
            y = yval * scaleFactor + falseNorthing;
          
            return;

        }

        ///<summary>
        /// 平面坐标转经纬度
        ///<summary>
        /// <param name="y">返回Y坐标，单位米</param>
        /// <param name="x">返回X坐标，单位米</param>
        /// <param name="lat">纬度，小数度</param>
        /// <param name="lon">经度,小数度</param>
        /// <param name="smeimajor">椭球体长半轴</param>
        /// <param name="flattening">椭球体扁率</param>
        /// <param name="centralMeridian">中央经线</param>
        /// <param name="flattening">东偏移量, 单位为米</param>
         /// <param name="falseNorthing">北偏移量， 单位为米，默认为0</param>
        /// <param name="scaleFactor">比例因子 UTM为0.9996，我国为1.0 </param>
        /// <param name="latitudeOfOrigin">起始纬度，默认为0</param>
        private static void YXToBLCore(double y, double x, out double lat, out double lon, double smeimajor, double flattening, double centralMeridian, double falseEasting, double falseNorthing = 0.0, double scaleFactor = 1.0, double latitudeOfOrigin = 0.0)
        {
            double longitude1, latitude1, longitude0,  xval, yval;//X0, Y0, latitude0,
            double e1, e2, f, a, ee, NN, T, C, M, D, R, u, fai, iPI;
            iPI = ANGULARUNIT_DEGREE;
        
            a = smeimajor; f = flattening; //长半轴，扁率

            longitude0 = centralMeridian* iPI; //中央经线转弧度

            xval = (x - falseEasting) / scaleFactor; yval = (y - falseNorthing) / scaleFactor; //带内大地坐标
            e2 = 2 * f - f * f;
            e1 = (1.0 - Math.Sqrt(1 - e2)) / (1.0 + Math.Sqrt(1 - e2));
            ee = e2 / (1 - e2);
            M = yval;
            u = M / (a * (1 - e2 / 4 - 3 * e2 * e2 / 64 - 5 * e2 * e2 * e2 / 256));
            fai = u + (3 * e1 / 2 - 27 * e1 * e1 * e1 / 32) * Math.Sin(2 * u) + (21 * e1 * e1 / 16 - 55 * e1 * e1 * e1 * e1 / 32) * Math.Sin(
            4 * u)
            + (151 * e1 * e1 * e1 / 96) * Math.Sin(6 * u) + (1097 * e1 * e1 * e1 * e1 / 512) * Math.Sin(8 * u);
            C = ee * Math.Cos(fai) * Math.Cos(fai);
            T = Math.Tan(fai) * Math.Tan(fai);
            NN = a / Math.Sqrt(1.0 - e2 * Math.Sin(fai) * Math.Sin(fai));
            R = a * (1 - e2) / Math.Sqrt((1 - e2 * Math.Sin(fai) * Math.Sin(fai)) * (1 - e2 * Math.Sin(fai) * Math.Sin(fai)) * (1 - e2 * Math.Sin
            (fai) * Math.Sin(fai)));
            D = xval / NN;
            //计算经度(Longitude) 纬度(Latitude)
            longitude1 = longitude0 + (D - (1 + 2 * T + C) * D * D * D / 6 + (5 - 2 * C + 28 * T - 3 * C * C + 8 * ee + 24 * T * T) * D
            * D * D * D * D / 120) / Math.Cos(fai);
            latitude1 = fai - (NN * Math.Tan(fai) / R) * (D * D / 2 - (5 + 3 * T + 10 * C - 4 * C * C - 9 * ee) * D * D * D * D / 24
            + (61 + 90 * T + 298 * C + 45 * T * T - 256 * ee - 3 * C * C) * D * D * D * D * D * D / 720);
            //转换为度 DD
            lon = longitude1 / iPI;
            lat = latitude1 / iPI;
            return;
        }

        ///<summary>
        ///ArcLengthOfMeridian
        ///Computes the ellipsoidal distance from the equator to a point at a
        ///given latitude.
        ///Reference: Hoffmann-Wellenhof, B., Lichtenegger, H., and Collins, J.,
        /// GPS: Theory and Practice, 3rd ed.  New York: Springer-Verlag Wien, 1994.
        // Inputs:
        ///     phi - Latitude of the point, in radians.
        /// Globals:
        ///     sm_a - Ellipsoid model major axis.
        ///     sm_b - Ellipsoid model minor axis.
        ///Returns:
        ///    The ellipsoidal distance of the point from the equator, in meters.
        ///</summary>
        private static double ArcLengthOfMeridian(double phi, double smeimajor, double smeiminor)
        {
            double alpha, beta, gamma, delta, epsilon, n;
            double result;
            double sm_a = smeimajor;
            double sm_b = smeiminor;

            /* Precalculate n */
            n = (sm_a - sm_b) / (sm_a + sm_b);

            /* Precalculate alpha */
            alpha = ((sm_a + sm_b) / 2.0) * (1.0 + (Math.Pow(n, 2.0) / 4.0) + (Math.Pow(n, 4.0) / 64.0));

            /* Precalculate beta */
            beta = (-3.0 * n / 2.0) + (9.0 * Math.Pow(n, 3.0) / 16.0) + (-3.0 * Math.Pow(n, 5.0) / 32.0);

            /* Precalculate gamma */
            gamma = (15.0 * Math.Pow(n, 2.0) / 16.0) + (-15.0 * Math.Pow(n, 4.0) / 32.0);

            /* Precalculate delta */
            delta = (-35.0 * Math.Pow(n, 3.0) / 48.0) + (105.0 * Math.Pow(n, 5.0) / 256.0);

            /* Precalculate epsilon */
            epsilon = (315.0 * Math.Pow(n, 4.0) / 512.0);

            /* Now calculate the sum of the series and return */
            result = alpha * (phi + (beta * Math.Sin(2.0 * phi)) + (gamma * Math.Sin(4.0 * phi)) + (delta * Math.Sin(6.0 * phi)) + (epsilon * Math.Sin(8.0 * phi)));

            return result;
        }

        ///<summary>
        ///FootpointLatitude
        ///Computes the footpoint latitude for use in converting transverse
        ///Mercator coordinates to ellipsoidal coordinates.
        ///Reference: Hoffmann-Wellenhof, B., Lichtenegger, H., and Collins, J.,
        /// GPS: Theory and Practice, 3rd ed.  New York: Springer-Verlag Wien, 1994.
        // Inputs:
        ///     y - The UTM northing coordinate, in meters.
        /// Globals:
        ///     sm_a - Ellipsoid model major axis.
        ///     sm_b - Ellipsoid model minor axis.
        ///Returns:
        ///    The footpoint latitude, in radians.
        ///</summary>
        private static double FootpointLatitude(double y, double smeimajor, double smeiminor)
        {
            double y_, alpha_, beta_, gamma_, delta_, epsilon_, n;
            double result;
            double sm_a = smeimajor;
            double sm_b = smeiminor;

            /* Precalculate n (Eq. 10.18) */
            n = (sm_a - sm_b) / (sm_a + sm_b);

            /* Precalculate alpha_ (Eq. 10.22) */
            /* (Same as alpha in Eq. 10.17) */
            alpha_ = ((sm_a + sm_b) / 2.0) * (1 + (Math.Pow(n, 2.0) / 4) + (Math.Pow(n, 4.0) / 64));

            /* Precalculate y_ (Eq. 10.23) */
            y_ = y / alpha_;

            /* Precalculate beta_ (Eq. 10.22) */
            beta_ = (3.0 * n / 2.0) + (-27.0 * Math.Pow(n, 3.0) / 32.0) + (269.0 * Math.Pow(n, 5.0) / 512.0);

            /* Precalculate gamma_ (Eq. 10.22) */
            gamma_ = (21.0 * Math.Pow(n, 2.0) / 16.0) + (-55.0 * Math.Pow(n, 4.0) / 32.0);

            /* Precalculate delta_ (Eq. 10.22) */
            delta_ = (151.0 * Math.Pow(n, 3.0) / 96.0) + (-417.0 * Math.Pow(n, 5.0) / 128.0);

            /* Precalculate epsilon_ (Eq. 10.22) */
            epsilon_ = (1097.0 * Math.Pow(n, 4.0) / 512.0);

            /* Now calculate the sum of the series (Eq. 10.21) */
            result = y_ + (beta_ * Math.Sin(2.0 * y_)) + (gamma_ * Math.Sin(4.0 * y_)) + (delta_ * Math.Sin(6.0 * y_)) + (epsilon_ * Math.Sin(8.0 * y_));

            return result;
        }

        /// <summary>
        /// BLH转空间直角坐标，指定椭球体参数
        /// </summary>
        /// <param name="lat">纬度（度）</param>
        /// <param name="lon">经度（度）</param>
        /// <param name="h">高程（米）</param>
        /// <param name="x">X坐标（米）</param>
        /// <param name="y">Y坐标（米）</param>
        /// <param name="z">Z坐标（米）</param>
        /// <param name="semimajor">长半轴，默认值为WGS84椭球体 6378137</param>
        /// <param name="inverseflattening">扁率的倒数，默认值为WGS84椭球体298.257223563</param>
        internal static void BLHToXYZ(double lat, double lon, double h, out double x, out double y, out double z, double semimajor = 6378137, double inverseflattening = 298.257223563)
        {
            double a = semimajor;// 6378140;
            double f = 1 / inverseflattening;
            double b = a * (1 - f);
            double latArc = lat * Math.PI / 180;
            double lonArc = lon * Math.PI / 180;
            //double e = Math.Sqrt(a * a - b * b) / a;
            //double N = a / Math.Sqrt(1 - e * Math.Pow(Math.Sin(latArc), 2));
            double e = Math.Sqrt(a * a - b * b) / a;
            double W = Math.Sqrt(1 - e * e * Math.Sin(latArc) * Math.Sin(latArc));
            double N = a / W;

            x = (N + h) * Math.Cos(latArc) * Math.Cos(lonArc);
            y = (N + h) * Math.Cos(latArc) * Math.Sin(lonArc);
            z = (b * b * N / (a * a) + h) * Math.Sin(latArc);
        }

        /// <summary>
        /// 指定椭球体参数，空间直角转BLH
        /// </summary>
        /// <param name="x">X坐标（米）</param>
        /// <param name="y">Y坐标（米）</param>
        /// <param name="z">Z坐标（米）</param>
        /// <param name="lat">纬度（度）</param>
        /// <param name="lon">经度（度）</param>
        /// <param name="h">高程（米）</param>
        /// <param name="semimajor">长半轴，默认值为WGS84椭球体 6378137</param>
        /// <param name="inverseflattening">扁率的导数，默认值为WGS84椭球体298.257223563</param>
        internal static void XYZToBLH(double x, double y, double z, out double lat, out double lon, out double h, double semimajor = 6378137, double inverseflattening = 298.257223563)
        {
            double a = semimajor;// 6378140;
            double f = 1 / inverseflattening;
            double b = a * (1 - f);
            double e = Math.Sqrt(a * a - b * b) / a;
            double R = Math.Sqrt(x * x + y * y);
            double B0 = Math.Atan2(z, R);
            double B;
            double N;
            double delta = 1E-10; // Math.PI / (180 * 60 * 60 * 1000);
            while (true)
            {
                N = a / Math.Sqrt(1 - e * e * Math.Pow(Math.Sin(B0), 2));
                B = Math.Atan2(z + N * e * e * Math.Sin(B0), R);
                if (Math.Abs(B - B0) < delta)
                    break;
                B0 = B;
            }
            double L = Math.Atan2(y, x);
            h = R / Math.Cos(B) - N;
            h = z / Math.Sin(B) - N * (1 - e * e);
            lat = B * 180 / Math.PI;
            lon = L * 180 / Math.PI;
        }

         /// <summary>
        /// 利用7参数，从一个空间直角坐标转到另一个空间直角坐标
        /// </summary>
        /// <param name="x1">源坐标系X坐标</param>
        /// <param name="y1">源坐标系Y坐标</param>
        /// <param name="z1">源坐标系Z坐标</param>
        /// <param name="x2">目标坐标系X坐标</param>
        /// <param name="y2">目标坐标系Y坐标</param>
        /// <param name="z2">目标坐标系Z坐标</param>
        /// <param name="para7">转换7参数</param>
        /// <param name="flag">是否逆向使用转换参数，默认值为false</param>
        public static void XYZToXYZ(double x1, double y1, double z1, out double x2, out double y2, out double z2,  BursaSevenparam para7, bool flag = false)
        {
            BursaSevenparam para = para7;
            if (flag)
                para = para.Reversal();

            double x0 = para.X0; //米
            double y0 = para.Y0;
            double z0 = para.Z0;
            double dx = para.DX; //秒
            double dy = para.DY;
            double dz = para.DZ;
            double dm = para.DM / 1000000; //ppm指百万分之一
            double wx = dx / 648000 * Math.PI; //转弧度
            double wy = dy / 648000 * Math.PI;
            double wz = dz / 648000 * Math.PI;

            x2 = x0 + (1 + dm) * x1 + wz * y1 - wy * z1;
            y2 = y0 + (1 + dm) * y1 - wz * x1 + wx * z1;
            z2 = z0 + (1 + dm) * z1 + wy * x1 - wx * y1;
        }
        
    }

    
    ///<summary>
    /// 布尔莎七参数
    ///</summary>
    public class  BursaSevenparam
    {
        

        /// <summary>
        /// 原点X方向平移量（米）
        /// </summary>
        public double X0 { get; set; }

        /// <summary>
        /// 原点Y方向平移量（米）
        /// </summary>
        public double Y0 { get; set; }

        /// <summary>
        /// 原点Z方向平移量（米）
        /// </summary>
        public double Z0 { get; set; }

        /// <summary>
        /// X坐标旋转角度（秒）
        /// </summary>
        public double DX { get; set; }

        /// <summary>
        /// Y坐标旋转角度（秒）
        /// </summary>
        public double DY { get; set; }

        /// <summary>
        /// Z坐标旋转角度（秒）
        /// </summary>
        public double DZ { get; set; }

        /// <summary>
        /// 尺度变换因子（PPM,百万分之一）
        /// </summary>
        public double DM { get; set; }

        public  BursaSevenparam Reversal()
        {
            return new  BursaSevenparam() { X0 = -this.X0, Y0 = -this.Y0, Z0 = -this.Z0, DX = -this.DX, DY = -this.DY, DZ = -this.DZ, DM = -this.DM };
        }

        public readonly static  BursaSevenparam WGS84TOXIAN80_DAYAWAN = new  BursaSevenparam() { X0 = 203.580, Y0 = 88.586, Z0 = 67.525, DX = 1.233155, DY = 2.542766, DZ = -2.984955, DM = -2.676026 };
    }

    /// <summary>
    /// 
    /// </summary>
    public enum DegreeType
    {
        Unknown = 0,
        East = 1,
        North = 2,
        West = 3,
        South = 4
    }

    public class DuFenMiao
    {
        public double Du { get; set; }

        public double Fen { get; set; }

        public double Miao { get; set; }

        private DegreeType type = DegreeType.Unknown;

        public static DuFenMiao FromDoubleDegree(double degree)
        {
            int du = (int)degree;
            int fen = (int)((degree - du) * 60);
            double miao = (((degree - du) * 60) - fen) * 60;
            return new DuFenMiao() { Du = du, Fen = fen, Miao = miao };
        }

        public static DuFenMiao FromDoubleDegree(double degree, bool isLon)
        {
            int du = (int)degree;
            int fen = (int)((degree - du) * 60);
            double miao = (((degree - du) * 60) - fen) * 60;
            int type = 0;
            if (isLon)
            {
                if (degree >= 0)
                    type = 1;
                else
                    type = 3;
            }
            else
            {
                if (degree >= 0)
                    type = 2;
                else
                    type = 4;
            }
            return new DuFenMiao() { Du = du, Fen = fen, Miao = miao, type = (DegreeType)type };
        }

        public static DuFenMiao FromString(string degree)
        {
            DegreeType type = DegreeType.Unknown;
            if (degree.EndsWith("E", StringComparison.InvariantCultureIgnoreCase))
            {
                type = DegreeType.East;
            }
            else if (degree.EndsWith("N", StringComparison.InvariantCultureIgnoreCase))
            {
                type = DegreeType.North;
            }
            else if (degree.EndsWith("W", StringComparison.InvariantCultureIgnoreCase))
            {
                type = DegreeType.West;
            }
            else if (degree.EndsWith("S", StringComparison.InvariantCultureIgnoreCase))
            {
                type = DegreeType.South;
            }
            int dot = degree.IndexOf(".");
            int du = int.Parse(degree.Substring(0, dot));
            int fen = int.Parse(degree.Substring(dot + 1, 2));
            int len = degree.Length - dot - 3;
            if (type > 0)
            {
                len -= 1;
            }
            double miao = double.Parse(degree.Substring(dot + 3, len)) / Math.Pow(10, len - 2);
            return new DuFenMiao() { Du = du, Fen = fen, Miao = miao, type = type };
        }

        public double ToDoubleDegree()
        {
            return this.Du + this.Fen / 60 + this.Miao / 3600;
        }


        public override string ToString()
        {
            string affix = String.Empty;
            switch (type)
            {
                case DegreeType.East:
                    affix = "E";
                    break;
                case DegreeType.North:
                    affix = "N";
                    break;
                case DegreeType.West:
                    affix = "W";
                    break;
                case DegreeType.South:
                    affix = "S";
                    break;
                default:
                    affix = "";
                    break;
            }
            return string.Format("{0:0}.{1:0}{2:r0}{3}", Math.Abs(Du), Fen, Math.Round(Miao * 100000), affix);
        }
    }
}
