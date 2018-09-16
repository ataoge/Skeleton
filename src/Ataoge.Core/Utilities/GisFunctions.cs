using System;


namespace Ataoge.Utilities
{
    public static class GisFunctions
    {
        #region 几何空间数据关系函数
        //[DbFunction("ST_Equals")]
        public static bool ST_Equals(byte[] geom1, byte[] geom2)
        {
            throw new NotImplementedException();
        }

        //[DbFunction("ST_Distance")]
        public static double ST_Distance(byte[] geom1, byte[] geom2)
        {
            throw new NotImplementedException();
        }

        //[DbFunction("ST_DWithin")]
        public static bool ST_DWithin(byte[] geom1, byte[] geom2, double distance)
        {
            throw new NotImplementedException();
        }

        //[DbFunction("ST_Disjoint")]
        public static bool ST_Disjoint(byte[] geom1, byte[] geom2)
        {
            throw new NotImplementedException();
        }

        //[DbFunction("ST_Intersects")]
        public static bool ST_Intersects(byte[] geom1, byte[] geom2)
        {
            throw new NotImplementedException();
        }

        //[DbFunction("ST_Touches")]
        public static bool ST_Touches(byte[] geom1, byte[] geom2)
        {
            throw new NotImplementedException();
        }

        //[DbFunction("ST_Crosses")]
        public static bool ST_Crosses(byte[] geom1, byte[] geom2)
        {
            throw new NotImplementedException();
        }

        //[DbFunction("ST_Within")]
        public static bool ST_Within(byte[] geom1, byte[] geom2)
        {
            throw new NotImplementedException();
        }

        //[DbFunction("ST_Overlaps")]
        public static bool ST_Overlaps(byte[] geom1, byte[] geom2)
        {
            throw new NotImplementedException();
        }

        //[DbFunction("ST_Contains")]
        public static bool ST_Contains(byte[] geom1, byte[] geom2)
        {
            throw new Exception();
        }

        //[DbFunction("ST_Covers")]
        public static bool ST_Covers(byte[] geom1, byte[] geom2)
        {
            throw new NotImplementedException();
        }

        //[DbFunction("ST_CoveredBy")]
        public static bool ST_CoveredBy(byte[] geom1, byte[] geom2)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Geometry Processing Functions

        //[DbFunction("ST_Centroid")]
        public static byte[] ST_Centroid(byte[] geom)
        {
            throw new NotImplementedException();
        }

        //[DbFunction("ST_Area")]
        public static double ST_Area(byte[] geom)
        {
            throw new NotImplementedException();
        }

        //[DbFunction("ST_Length")]
        public static double ST_Length(byte[] geom)
        {
            throw new NotImplementedException();
        }

        ///<summary>
        ///
        ///</summary>
        //[DbFunction("ST_PointOnSurface")]
        public static byte[] ST_PointOnSurface(byte[] geom)
        {
            throw new NotImplementedException();
        }

        ///<summary>
        ///第一个參数是要操作的空间几何数据。第二个參数长度（距离），第三个參数为一个整型
        ///最后一个參数表示在组成一个1/4圆的有几个点分隔
        ///1英里= 63360 米  1米=1/1852 海里  1海里= 1/60度
        ///  500米 =  500*1/1852*1/60（度）
        ///</summary>
        //[DbFunction("ST_Buffer")]
        public static byte[] ST_Buffer(byte[] geom, double degree, int pts = 8)
        {
            throw new NotImplementedException();
        }

        ///<summary>
        ///个函数能够返回mbr(空间最小外包矩形)。传入參数能够是point line polygon
        ///</summary>
        //[DbFunction("ST_Envelope")]
        public static byte[] ST_Envelope(byte[] geom)
        {
            throw new NotImplementedException();
        }

        ///<summary>
        ///
        ///</summary>
        //[DbFunction("ST_extent")]
        public static byte[] ST_extent(byte[] geomset)
        {
            throw new NotImplementedException();
        }

        ///<summary>
        /// 返回一个几何空间数据A不同于空间数据B的几何空间数据类型。不要使用GeometryCollection作为參数。
        ///</summary>
        //[DbFunction("ST_Difference")]
        public static byte[] ST_Difference(byte[] geom1, byte[] geom2)
        {
            throw new NotImplementedException();
        }

        ///<summary>
        /// 返回一个合并的几何空间数据，将两个几何空间数据合并为一个几何空间数据。或者GeometryCollection，不要使用GeometryCollection作为參数。
        ///</summary>
        //[DbFunction("ST_Union")]
        public static byte[] ST_Union(byte[] geom1, byte[] geom2)
        {
            throw new NotImplementedException();
        }

        ///<summary>
        /// 合并为线
        ///</summary>
        //[DbFunction("ST_LineMerge")]
        public static byte[] ST_LineMerge(byte[] geom)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Geometry Accessors

        ///<summary>
        ///
        ///</summary>
        //[DbFunction("ST_AsBinary")]
        public static byte[] ST_AsBinary(byte[] geomset)
        {
            throw new NotImplementedException();
        }

        ///<summary>
        ///
        ///</summary>
        //[DbFunction("ST_AsText")]
        public static string ST_AsText(byte[] geomset)
        {
            throw new NotImplementedException();
        }

        ///<summary>
        /// 返回当前几何空间数据的SRID值
        ///</summary>
        //[DbFunction("ST_SRID")]
        public static int ST_SRID(byte[] geomset)
        {
            throw new NotImplementedException();
        }

        ///<summary>
        /// 给一个几何对象(geometry)设置一个整型的SRID，对于在一个范围内的查询很实用。
        ///</summary>
        //[DbFunction("ST_SetSRID")]
        public static void ST_SetSRID(byte[] geomset, int srid)
        {
            throw new NotImplementedException();
        }

        ///<summary>
        /// 推断几何空间数据是否是闭合，就是推断起始点和终点坐标是同样的，假设是同样的返回true,否则返回false.
        ///</summary>
        //[DbFunction("ST_IsClosed")]
        public static bool ST_IsClosed(byte[] geomset)
        {
            throw new NotImplementedException();
        }

        ///<summary>
        /// 这个函数參数的对象是line。推断起始点和终点坐标是否同样
        ///</summary>
        //[DbFunction("ST_IsRing")]
        public static bool ST_IsRing(byte[] geomset)
        {
            throw new NotImplementedException();
        }

        ///<summary>
        /// 返回几何空间数据lineString上的第一条线上点的个数。
        ///</summary>
        //[DbFunction("ST_NumPoints")]
        public static bool ST_NumPoints(byte[] geomset)
        {
            throw new NotImplementedException();
        }

        ///<summary>
        /// 推断几何空间数据的类型
        ///</summary>
        //[DbFunction("GeometryType")]
        public static string GeometryType(byte[] geomset)
        {
            throw new NotImplementedException();
        }

        /*
        获取几何对象的维数 ST_Dimension(geometry) 
        获取几何对象的边界范围 ST_Envelope(geometry) 
        判断几何对象是否为空 ST_IsEmpty(geometry) 
        判断几何对象是否不包含特殊点（比如自相交） ST_IsSimple(geometry) 
        判断几何对象是否闭合 ST_IsClosed(geometry) 
        判断曲线是否闭合并且不包含特殊点 ST_IsRing(geometry) 
        获取多几何对象中的对象个数 ST_NumGeometries(geometry) 
        获取多几何对象中第N个对象 ST_GeometryN(geometry,int) 
        获取几何对象中的点个数 ST_NumPoints(geometry) 
        获取几何对象的第N个点 ST_PointN(geometry,integer) 
        获取多边形的外边缘 ST_ExteriorRing(geometry) 
        获取多边形内边界个数 ST_NumInteriorRings(geometry) 
        同上 ST_NumInteriorRing(geometry) 
        获取多边形的第N个内边界 ST_InteriorRingN(geometry,integer) 
        获取线的终点 ST_EndPoint(geometry) 
        获取线的起始点 ST_StartPoint(geometry) 
        获取几何对象的类型 GeometryType(geometry) 
        类似上，但是不检查M值，即POINTM对象会被判断为point ST_GeometryType(geometry) 
        获取点的X坐标 ST_X(geometry) 
        获取点的Y坐标 ST_Y(geometry) 
        获取点的Z坐标 ST_Z(geometry) 
        获取点的M值 ST_M(geometry)
        */


        #endregion

        /*
        几何对象构造函数 ： 
        
        ST_PointFromText(text,[]) 
        ST_LineFromText(text,[]) 
        ST_LinestringFromText(text,[]) 
        ST_PolyFromText(text,[]) 
        ST_PolygonFromText(text,[]) 
        ST_MPointFromText(text,[]) 
        ST_MLineFromText(text,[]) 
        ST_MPolyFromText(text,[]) 
        ST_GeomCollFromText(text,[]) 
        ST_GeomFromWKB(bytea,[]) 
        ST_GeometryFromWKB(bytea,[]) 
        ST_PointFromWKB(bytea,[]) 
        ST_LineFromWKB(bytea,[]) 
        ST_LinestringFromWKB(bytea,[]) 
        ST_PolyFromWKB(bytea,[]) 
        ST_PolygonFromWKB(bytea,[]) 
        ST_MPointFromWKB(bytea,[]) 
        ST_MLineFromWKB(bytea,[]) 
        ST_MPolyFromWKB(bytea,[]) 
        ST_GeomCollFromWKB(bytea,[]) 
        ST_BdPolyFromText(text WKT, integer SRID)

        ST_BdMPolyFromText(text WKT, integer SRID)
         */
        //[DbFunction("ST_GeomFromText")]
        public static byte[] ST_GeomFromText(string value)
        {
            throw new NotImplementedException();
        }

        //[DbFunction("ST_GeomFromText")]
        public static byte[] ST_GeomFromText(string value, int srid)
        {
            throw new Exception();
        }
    }
}