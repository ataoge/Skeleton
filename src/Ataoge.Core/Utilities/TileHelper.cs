using System;
using System.Text;

namespace Ataoge.Utilities
{
    public static class TileHelper 
    {
        public const double DEFAULT_MINLATITUDE = -85.05112878;
        public const double DEFAULT_MAXLATITUDE = 85.05112878;
        public const double DEFAULT_MINLONGITUDE = -180.0;
        public const double DEFAULT_MAXLONGTITUE = 180.0;

        /// <summary>  
        /// Converts tile XY coordinates into aQuadKey at a specified level of detail.  
        /// </summary>  
        /// <paramname="tileX">Tile X coordinate.</param>  
        /// <paramname="tileY">Tile Y coordinate.</param>  
        /// <paramname="level">Level of detail, from 1 (lowest detail)  
        /// to 23 (highestdetail).</param>  
        /// <returns>A string containingthe QuadKey.</returns>
        public static string TileXYToQuadKey(int tileX, int tileY, int level)
        {
            if (level == 0)
                return "A";
            StringBuilder quadKey = new StringBuilder();  
            for (int i = level; i >0; i--)  
            {  
                char digit = '0';  
                int mask = 1 << (i - 1);  
                if ((tileX & mask) != 0)  
                {  
                    digit++;  
                }  
                if ((tileY & mask) != 0)  
                {  
                    digit++;  
                    digit++;  
                }  
                quadKey.Append(digit);  
            }  
            return quadKey.ToString();  
        }

        /// <summary>  
        /// Converts a QuadKey into tile XYcoordinates.  
        /// </summary>  
        /// <paramname="quadKey">QuadKey of the tile.</param>  
       /// <param name="tileX">Output parameter receiving thetile X coordinate.</param>  
        /// <paramname="tileY">Output parameter receiving the tile Ycoordinate.</param>  
        /// <paramname="level">Output parameter receiving the level ofdetail.</param>
        public static void QuadKeyToTileXY(string quadKey, out int tileX, out int tileY, out int level)
        {
            tileX = tileY = 0; 
            if (quadKey == "A")
            {
                level = 0;
                return; 
            }
            level = quadKey.Length;  
            for (int i = level; i > 0; i--)  
            {  
                int mask = 1 << (i - 1);  
                switch (quadKey[level - i])  
                {  
                    case '0':  
                        break;  
   
                    case '1':  
                        tileX |= mask;  
                        break;  
   
                    case '2':  
                        tileY |= mask;  
                        break;  
   
                    case '3':  
                        tileX |= mask;  
                        tileY |= mask;  
                        break;  
   
                    default:  
                        throw new ArgumentException("Invalid QuadKey digit sequence.");  
                }  
            }  
        }

        public static string GetParentQuadKey(string quadKey)
        {
            if (quadKey.Length > 1)
                return quadKey.Substring(0,quadKey.Length -1);
            
            if (quadKey != "A")
                throw new ArgumentException("Invalid QuadKey digit sequence.");
            
            return "A";
        }

        public static string ToID(int z, int x, int y)
        {
            return TileXYToQuadKey(x, y, z);
        }

        public static void GetOneTileIncludeExtent(double minX, double maxX, double minY, double maxY, out int z, out int tx, out int ty)
        {
            int z0 = 0;
            double ll = DEFAULT_MAXLONGTITUE - DEFAULT_MINLONGITUDE;
            double tt = DEFAULT_MAXLATITUDE - DEFAULT_MINLATITUDE;
            while (true)
            {
                int zoom = 1 << z0;
                if (Math.Floor((DEFAULT_MAXLONGTITUE + minX) * zoom / ll) != Math.Floor((DEFAULT_MAXLONGTITUE + maxX) * zoom / ll) || 
                    Math.Floor((DEFAULT_MAXLATITUDE - maxY) * zoom / tt) != Math.Floor((DEFAULT_MAXLATITUDE - minY) * zoom / tt))
                    break;
                z0++;
            }
            z = z0--;
            tx = (int) Math.Floor((DEFAULT_MAXLONGTITUE + minX) * (1 << z) / ll);
            ty = (int) Math.Floor((DEFAULT_MAXLATITUDE - maxY) * (1 << z) / tt);
        }

        

        private static double[] _WebMercatorScales = new double[] {5.91657527591555E8,2.95828763795777E8,1.47914381897889E8, 7.3957190948944E7, 3.6978595474472E7,
          1.8489297737236E7,9244648.868618,4622324.434309, 2311162.217155, 1155581.108577, 577790.554289,
          288895.277144,144447.638572 ,72223.819286,36111.909643,18055.954822, 9027.977411, 4513.988705,2256.994353,
          1128.497176};
        private static double[] _WebMercatorResolutions = new double[] {156543.033928,78271.5169639999,39135.7584820001, 19567.8792409999, 9783.93962049996,
          4891.96981024998,2445.98490512499,1222.99245256249, 611.49622628138, 305.748113140558, 152.874056570411,
          76.4370282850732,38.2185141425366,19.1092570712683, 9.55462853563415, 4.77731426794937, 2.38865713397468, 1.19432856685505,
          0.597164283559817, 0.298582141647617};

        public static int GetLevelByResolution(double resolution, bool fit = true)
        {
            int level = 0;
            double initResolution = _WebMercatorResolutions[0];
            while (true) 
            {
                if (resolution > initResolution)
                {
                    if (fit)
                        return level;
                    else
                        return level == 0 ? 0 : level -1 ;
                }

                level ++;
                initResolution = initResolution / 2;
            }
        }

        public static int GetLevelByScale(double scale, bool fit = true)
        {
            int level = 0;
            double initScale = _WebMercatorScales[0];
            while (true) 
            {
                if (scale > initScale)
                {
                    if (fit)
                        return level;
                    else
                        return level == 0 ? 0 : level -1 ;
                }

                level ++;
                initScale = initScale / 2;
            }
        }

        public static double GetResolution(int level)
        {
            double initResolution = _WebMercatorResolutions[0];
            return initResolution / (1 << level);
        }

        public static double GetScale(int level)
        {
            double initScale = _WebMercatorScales[0];
            return initScale / (1 << level);
        }

    }  
}