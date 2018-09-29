using System;
using Xunit;
using Ataoge.GisCore.Utilities;
using System.Diagnostics;

namespace Ataoge.GisCore.Tests
{
    public class UnitTest2
    {
        [Fact]
        public void TestKDBush()
        {
            int max = 1000000;
            var pts = new Point[max];
            var random = new Random();
            for (var i = 0; i < max; i++)
            {
                var pt = new Point();
                pt.X = random.Next(-180000, 180000) * 0.001;
                pt.Y = random.Next(-90000, 90000) * 0.001;
                pts[i] = pt;
            }

            Stopwatch sp = new Stopwatch();
            sp.Start();
            var index = new KDBush(pts);
            sp.Stop();
            Console.WriteLine($"索引耗时：{sp.ElapsedMilliseconds}");

            sp.Start();
            for (int i = 0; i < 10000; i++)
            {
                var pt = new Point();
                pt.X = random.Next(-180000, 180000) * 0.001;
                pt.Y = random.Next(-90000, 90000) * 0.001;
                index.Range(pt.X - 1, pt.Y - 1, pt.X + 1, pt.Y + 1);
            }
            sp.Stop();
            Console.WriteLine($"小范围查询耗时：{sp.ElapsedMilliseconds}");

            sp.Start();
            for (int i = 0; i < 10000; i++)
            {
                var pt = new Point();
                pt.X = random.Next(-180000, 180000) * 0.001;
                pt.Y = random.Next(-90000, 90000) * 0.001;
                index.WithIn(pt.X, pt.Y, 1);
            }
            sp.Stop();
            Console.WriteLine($"半径查询耗时：{sp.ElapsedMilliseconds}");

            var pt1 = new Point();
            pt1.X = random.Next(-180000, 180000) * 0.001;
            pt1.Y = random.Next(-90000, 90000) * 0.001;
            var rr = index.Range(pt1.X - 10, pt1.Y - 10, pt1.X + 10, pt1.Y + 10);

            var pt2 = new Point();
            pt2.X = random.Next(-180000, 180000) * 0.001;
            pt2.Y = random.Next(-90000, 90000) * 0.001;
            var bb = index.WithIn(pt2.X, pt2.Y, 1);
        }

        [Fact]
        public void TestWHCoord()
        {
            double x;
            double y;
            BLToWH54(114.30, 30.60, out x, out y);
            MapLatLonToXY(30.06, 114.30, out x, out y);

            double lat;
            double lon;
            MapXYToLatLon(x, y, out lon, out lat);
            //WH2000ToBL(407889.6354,3245942.418, out lon, out lat);
            WH54ToBL(461574.705, 3271333.382, out lon, out lat);
            MapXYToLatLon(461574.705, 3271333.382, out lat, out lon);
        }

        public static void WH54ToBL(double X, double Y, out double longitude, out double latitude, int zoneWide = 6, int projNo = 0)
        {
            

            double longitude1, latitude1, longitude0, X0, Y0, xval, yval;//latitude0,
            double e1, e2, f, a, ee, NN, T, C, M, D, R, u, fai, iPI;
            iPI = 0.0174532925199433; //3.1415926535898/180.0;
            //a = 6378245.0; f = 1.0/298.3; //54年北京坐标系参数
            a = 6378245; f = 1 / 298.3; //80年西安坐标系参数
                                                  //ZoneWide = 6; ////6度带宽 我国13-23  3度带 我国24～45带

            longitude0 = 114 * iPI; //中央经线

            //latitude0 = 0;
            X0 = 500000;//ProjNo * 1000000L + 500000L;
            Y0 = 0;//-3000000;
            xval = X - X0; yval = Y - Y0; //带内大地坐标
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
            longitude = longitude1 / iPI;
            latitude = latitude1 / iPI;
            return;
        }

         public static void BLToWH54(double longitude, double latitude, out double x, out double y, int zoneWide = 6, int projNo = 0)
        {

            double longitude1, latitude1, longitude0, X0, Y0, xval, yval; //latitude0,
            double a, f, e2, ee, NN, T, C, A, M, iPI;
            iPI = 0.0174532925199433; ////3.1415926535898/180.0;
            //ZoneWide = 6; ////6度带宽
            //a = 6378245.0; f = 1.0 / 298.3; //54年北京坐标系参数
            //a = 6378140.0; f = 1 / 298.257; //80年西安坐标系参数
            a = 6378245; f = 1 / 298.3; // CGCS2000坐标系参数

            longitude0 = 114 * iPI;
            //latitude0 = 0;

            longitude1 = longitude * iPI; //经度转换为弧度
            latitude1 = latitude * iPI; //纬度转换为弧度
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

            X0 = 500000;
            Y0 = 0;//-3000000;

            xval = xval + X0; yval = yval + Y0;
            x = xval;
            y = yval;
            return;
        }

        /// <summary>
        /// CGCS2000高斯投影坐标换算经纬度
        /// </summary>
        /// <param name="X">X坐标（可包含带号）</param>
        /// <param name="Y">Y坐标</param>
        /// <param name="longitude">经度</param>
        /// <param name="latitude">纬度</param>
        /// <param name="zoneWidth">带宽（6度带|3度带）</param>
        /// <param name="projNo">如果X坐标包含带号，则本参数可选</param>
        public static void WH2000ToBL(double X, double Y, out double longitude, out double latitude, int zoneWide = 6, int projNo = 0)
        {
            int ProjNo; //int ZoneWide; ////带宽

            double longitude1, latitude1, longitude0, X0, Y0, xval, yval;//latitude0,
            double e1, e2, f, a, ee, NN, T, C, M, D, R, u, fai, iPI;
            iPI = 0.0174532925199433; //3.1415926535898/180.0;
            //a = 6378245.0; f = 1.0/298.3; //54年北京坐标系参数
            a = 6378137.0; f = 1 / 298.257222101; //80年西安坐标系参数
                                                  //ZoneWide = 6; ////6度带宽 我国13-23  3度带 我国24～45带

            longitude0 = 114.3333333333 * iPI; //中央经线

            //latitude0 = 0;
            X0 = 800000;//ProjNo * 1000000L + 500000L;
            Y0 = -3000000;
            xval = X - X0; yval = Y - Y0; //带内大地坐标
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
            longitude = longitude1 / iPI;
            latitude = latitude1 / iPI;
            return;
        }

        /// <summary>
        /// 由经纬度反算成高斯投影坐标CGCS2000
        /// </summary>
        /// <param name="longitude">经度</param>
        /// <param name="latitude">纬度</param>
        /// <param name="x">X坐标（可能包含带号）</param>
        /// <param name="y">Y坐标</param>
        /// <param name="zoneWidth">带宽（6度带|3度带）</param>
        /// <param name="projNo">设置本参数，则X包含带号</param>
        public static void BLToWH2000(double longitude, double latitude, out double x, out double y, int zoneWide = 6, int projNo = 0)
        {

            double longitude1, latitude1, longitude0, X0, Y0, xval, yval; //latitude0,
            double a, f, e2, ee, NN, T, C, A, M, iPI;
            iPI = 0.0174532925199433; ////3.1415926535898/180.0;
            //ZoneWide = 6; ////6度带宽
            //a = 6378245.0; f = 1.0 / 298.3; //54年北京坐标系参数
            //a = 6378140.0; f = 1 / 298.257; //80年西安坐标系参数
            a = 6378137.0; f = 1 / 298.257222101; // CGCS2000坐标系参数

            longitude0 = 114.3333333333333 * iPI;
            //latitude0 = 0;

            longitude1 = longitude * iPI; //经度转换为弧度
            latitude1 = latitude * iPI; //纬度转换为弧度
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

            X0 = 800000;
            Y0 = -3000000;

            xval = xval + X0; yval = yval + Y0;
            x = xval;
            y = yval;
            return;
        }

        public static double DegToRad(double deg)
        {
            return (deg / 180.0 * Math.PI);
        }

        public static double RadToDeg(double rad)
        {
            return (rad / Math.PI * 180.0);
        }

        public static double ArcLengthOfMeridian(double phi)
        {
            double alpha, beta, gamma, delta, epsilon, n;
            double result;

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

        private static double sm_a = 6378137.0;


        private static double sm_b = 6356752.3141403558;
        public static void MapLatLonToXY(double lat, double lon, out double x, out double y)

        {
            double phi = DegToRad(lat);
            double lambda = DegToRad(lon);
            double lambda0 = DegToRad(114.3333333333);
            double N, nu2, ep2, t, t2, l;
            double l3coef, l4coef, l5coef, l6coef, l7coef, l8coef;
            double tmp;

            /* Precalculate ep2 */
            ep2 = (Math.Pow(sm_a, 2.0) - Math.Pow(sm_b, 2.0)) / Math.Pow(sm_b, 2.0);

            /* Precalculate nu2 */
            nu2 = ep2 * Math.Pow(Math.Cos(phi), 2.0);

            /* Precalculate N */
            N = Math.Pow(sm_a, 2.0) / (sm_b * Math.Sqrt(1 + nu2));

            /* Precalculate t */
            t = Math.Tan(phi);
            t2 = t * t;
            tmp = (t2 * t2 * t2) - Math.Pow(t, 6.0);

            /* Precalculate l */
            l = lambda - lambda0;

            /* Precalculate coefficients for l**n in the equations below
            so a normal human being can read the expressions for easting
            and northing
            -- l**1 and l**2 have coefficients of 1.0 */
            l3coef = 1.0 - t2 + nu2;

            l4coef = 5.0 - t2 + 9 * nu2 + 4.0 * (nu2 * nu2);

            l5coef = 5.0 - 18.0 * t2 + (t2 * t2) + 14.0 * nu2 - 58.0 * t2 * nu2;

            l6coef = 61.0 - 58.0 * t2 + (t2 * t2) + 270.0 * nu2 - 330.0 * t2 * nu2;

            l7coef = 61.0 - 479.0 * t2 + 179.0 * (t2 * t2) - (t2 * t2 * t2);

            l8coef = 1385.0 - 3111.0 * t2 + 543.0 * (t2 * t2) - (t2 * t2 * t2);

            /* Calculate easting (x) */
            var xVal = N * Math.Cos(phi) * l + (N / 6.0 * Math.Pow(Math.Cos(phi), 3.0) * l3coef * Math.Pow(l, 3.0))
                + (N / 120.0 * Math.Pow(Math.Cos(phi), 5.0) * l5coef * Math.Pow(l, 5.0))
                + (N / 5040.0 * Math.Pow(Math.Cos(phi), 7.0) * l7coef * Math.Pow(l, 7.0));

            /* Calculate northing (y) */
            var yVal = ArcLengthOfMeridian(phi)
                + (t / 2.0 * N * Math.Pow(Math.Cos(phi), 2.0) * Math.Pow(l, 2.0))
                + (t / 24.0 * N * Math.Pow(Math.Cos(phi), 4.0) * l4coef * Math.Pow(l, 4.0))
                + (t / 720.0 * N * Math.Pow(Math.Cos(phi), 6.0) * l6coef * Math.Pow(l, 6.0))
                + (t / 40320.0 * N * Math.Pow(Math.Cos(phi), 8.0) * l8coef * Math.Pow(l, 8.0));

            var x00 = 800000;
            var y00 = -3000000;
            x = xVal + x00;
            y = yVal + y00;
        }

        public static double FootpointLatitude(double y)
        {
            double y_, alpha_, beta_, gamma_, delta_, epsilon_, n;
            double result;

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

        public static void MapXYToLatLon (double x, double y, out double lat, out double lon)
        {
            double lambda0 = DegToRad(114.3333333333);
            y = y + 3000000;
            x = x - 800000;
            double phif, Nf, Nfpow, nuf2, ep2, tf, tf2, tf4, cf;
            double x1frac, x2frac, x3frac, x4frac, x5frac, x6frac, x7frac, x8frac;
            double x2poly, x3poly, x4poly, x5poly, x6poly, x7poly, x8poly;

            /* Get the value of phif, the footpoint latitude. */
            phif = FootpointLatitude(y);

            /* Precalculate ep2 */
            ep2 = (Math.Pow(sm_a, 2.0) - Math.Pow(sm_b, 2.0)) / Math.Pow(sm_b, 2.0);

            /* Precalculate cos (phif) */
            cf = Math.Cos(phif);

            /* Precalculate nuf2 */
            nuf2 = ep2 * Math.Pow(cf, 2.0);

            /* Precalculate Nf and initialize NfMath.Pow */
            Nf = Math.Pow(sm_a, 2.0) / (sm_b * Math.Sqrt(1 + nuf2));
            Nfpow = Nf;

            /* Precalculate tf */
            tf = Math.Tan(phif);
            tf2 = tf * tf;
            tf4 = tf2 * tf2;

            /* Precalculate fractional coefficients for x**n in the equations
            below to simplify the expressions for latitude and longitude. */
            x1frac = 1.0 / (Nfpow * cf);

            Nfpow *= Nf;   /* now equals Nf**2) */
            x2frac = tf / (2.0 * Nfpow);

            Nfpow *= Nf;   /* now equals Nf**3) */
            x3frac = 1.0 / (6.0 * Nfpow * cf);

            Nfpow *= Nf;   /* now equals Nf**4) */
            x4frac = tf / (24.0 * Nfpow);

            Nfpow *= Nf;   /* now equals Nf**5) */
            x5frac = 1.0 / (120.0 * Nfpow * cf);

            Nfpow *= Nf;   /* now equals Nf**6) */
            x6frac = tf / (720.0 * Nfpow);

            Nfpow *= Nf;   /* now equals Nf**7) */
            x7frac = 1.0 / (5040.0 * Nfpow * cf);

            Nfpow *= Nf;   /* now equals Nf**8) */
            x8frac = tf / (40320.0 * Nfpow);

            /* Precalculate polynomial coefficients for x**n.
            -- x**1 does not have a polynomial coefficient. */
            x2poly = -1.0 - nuf2;

            x3poly = -1.0 - 2 * tf2 - nuf2;

            x4poly = 5.0 + 3.0 * tf2 + 6.0 * nuf2 - 6.0 * tf2 * nuf2 - 3.0 * (nuf2 * nuf2) - 9.0 * tf2 * (nuf2 * nuf2);

            x5poly = 5.0 + 28.0 * tf2 + 24.0 * tf4 + 6.0 * nuf2 + 8.0 * tf2 * nuf2;

            x6poly = -61.0 - 90.0 * tf2 - 45.0 * tf4 - 107.0 * nuf2 + 162.0 * tf2 * nuf2;

            x7poly = -61.0 - 662.0 * tf2 - 1320.0 * tf4 - 720.0 * (tf4 * tf2);

            x8poly = 1385.0 + 3633.0 * tf2 + 4095.0 * tf4 + 1575 * (tf4 * tf2);

            /* Calculate latitude */
            var lat0 = phif + x2frac * x2poly * (x * x) + x4frac * x4poly * Math.Pow(x, 4.0) + x6frac * x6poly * Math.Pow(x, 6.0) + x8frac * x8poly * Math.Pow(x, 8.0);

            /* Calculate longitude */
            var lon0 = lambda0 + x1frac * x + x3frac * x3poly * Math.Pow(x, 3.0) + x5frac * x5poly * Math.Pow(x, 5.0) + x7frac * x7poly * Math.Pow(x, 7.0);

            lat = RadToDeg(lat0);
            lon = RadToDeg(lon0);
        }


    }
}