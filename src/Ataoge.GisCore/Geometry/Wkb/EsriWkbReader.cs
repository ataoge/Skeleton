using System;
using System.Collections.Generic;
using System.IO;

namespace Ataoge.GisCore.Geometry
{
    internal class EsriWkbReader
    {
        protected EndianBinaryReader wkbReader;

        internal EsriWkbReader(Stream stream)
        {
            wkbReader = new EndianBinaryReader(stream);
        }

        protected int? Srid {get; set;}

        internal EsriGeometry Read()
        {
            wkbReader.IsBigEndian = !wkbReader.ReadBoolean();
            uint type = wkbReader.ReadUInt32();
            GeometryType geometryType = ReadGeometryType(type);
            Dimension dimension = ReadDimension(type);
            //int? srid = ReadSrid(type);
            Srid = ReadSrid(type);

            EsriGeometry geometry = null;

            switch (geometryType)
            {
                case GeometryType.Point: geometry = ReadPoint(dimension); break;
                case GeometryType.LineString: geometry = ReadLineString(dimension); break;
                case GeometryType.Polygon: geometry = ReadPolygon(dimension); break;
                case GeometryType.MultiPoint: geometry = ReadMultiPoint(dimension); break;
                //case GeometryType.MultiLineString: geometry = ReadMultiLineString(dimension); break;
                //case GeometryType.MultiPolygon: geometry = ReadMultiPolygon(dimension); break;
                //case GeometryType.GeometryCollection: geometry = ReadGeometryCollection(dimension); break;
                default: throw new Exception();
            }

            //geometry.Dimension = dimension;
            geometry.SpatialReference = CreateSpatialReference();

            return geometry;
        }

        protected virtual GeometryType ReadGeometryType(uint type)
        {
            return (GeometryType)(type % 1000);
        }

        protected virtual Dimension ReadDimension(uint type)
        {
            if (type >= 1000 && type < 2000)
                return Dimension.Xyz;
            else if (type >= 2000 && type < 3000)
                return Dimension.Xym;
            else if (type >= 3000 && type < 4000)
                return Dimension.Xyzm;

            return Dimension.Xy;
        }

        protected virtual int? ReadSrid(uint type)
        {
            return null;
        }

        private T Read<T>() where T : EsriGeometry
        {
            return (T)Read();
        }

        private EsriPoint ReadPoint(Dimension dimension)
        {
            /*switch (dimension)
            {
                case Dimension.Xy: return new EsriPoint(wkbReader.ReadDouble(), wkbReader.ReadDouble());
                case Dimension.Xyz: return new EsriPoint(wkbReader.ReadDouble(), wkbReader.ReadDouble()) { Z =  wkbReader.ReadDouble()};
                case Dimension.Xym: return new EsriPoint(wkbReader.ReadDouble(), wkbReader.ReadDouble()) { M = wkbReader.ReadDouble()};
                case Dimension.Xyzm: return new EsriPoint(wkbReader.ReadDouble(), wkbReader.ReadDouble()) { Z = wkbReader.ReadDouble(), M = wkbReader.ReadDouble()};
                default: throw new Exception();
            }*/

            switch (dimension)
            {
                case Dimension.Xy: return CreatePoint(wkbReader.ReadDouble(), wkbReader.ReadDouble(), null, null);
                case Dimension.Xyz: return CreatePoint(wkbReader.ReadDouble(), wkbReader.ReadDouble(), wkbReader.ReadDouble(), null);
                case Dimension.Xym: return CreatePoint(wkbReader.ReadDouble(), wkbReader.ReadDouble(), null, wkbReader.ReadDouble());
                case Dimension.Xyzm: return CreatePoint(wkbReader.ReadDouble(), wkbReader.ReadDouble(), wkbReader.ReadDouble(), wkbReader.ReadDouble());
                default: throw new Exception();
            }
        }

        protected virtual EsriPoint  CreatePoint(double x, double y, double? z, double? m)
        {
            EsriPoint point = new EsriPoint(x, y);
            point.Z = z;
            point.M =m;
            return point;
            
        }

        protected virtual SpatialReference CreateSpatialReference()
        {
            if (Srid != null)
                return new SpatialReference(Srid.Value);
            return null;
        }

        private EsriPolyline ReadLineString(Dimension dimension)
        {
            EsriPolyline lineString = new EsriPolyline();

            uint pointCount = wkbReader.ReadUInt32();

            EsriPointCollection pts = new EsriPointCollection();
            for (int i = 0; i < pointCount; i++)
                pts.Add(ReadPoint(dimension));
            
            lineString.Paths.Add(pts);

            return lineString;
        }

        private EsriPolygon ReadPolygon(Dimension dimension)
        {
            EsriPolygon polygon = new EsriPolygon();

            uint ringCount = wkbReader.ReadUInt32();

            if (ringCount > 0)
            {
                uint exteriorRingCount = wkbReader.ReadUInt32();
                EsriPointCollection pts = new EsriPointCollection();
                for (int i = 0; i < exteriorRingCount; i++)
                    pts.Add(ReadPoint(dimension));
                polygon.Rings.Add(pts);

                for (int i = 1; i < ringCount; i++)
                {
                    pts = new EsriPointCollection();

                    uint interiorRingCount = wkbReader.ReadUInt32();
                    for (int j = 0; j < interiorRingCount; j++)
                        pts.Add(ReadPoint(dimension));
                    polygon.Rings.Add(pts);
                }
            }

            return polygon;
        }

        private EsriMultiPoint ReadMultiPoint(Dimension dimension)
        {
            var multiPoint = new EsriMultiPoint();

            uint pointCount = wkbReader.ReadUInt32();

            for (int i = 0; i < pointCount; i++)
                multiPoint.Points.Add(Read<EsriPoint>());

            return multiPoint;
        }

        /* 
        private MultiLineString ReadMultiLineString(Dimension dimension)
        {
            MultiLineString multiLineString = new MultiLineString();

            uint lineStringCount = wkbReader.ReadUInt32();

            for (int i = 0; i < lineStringCount; i++)
                multiLineString.LineStrings.Add(Read<LineString>());

            return multiLineString;
        }

        private MultiPolygon ReadMultiPolygon(Dimension dimension)
        {
            MultiPolygon multiPolygon = new MultiPolygon();

            uint polygonCount = wkbReader.ReadUInt32();

            for (int i = 0; i < polygonCount; i++)
                multiPolygon.Polygons.Add(Read<Polygon>());

            return multiPolygon;
        }

        private GeometryCollection ReadGeometryCollection(Dimension dimension)
        {
            GeometryCollection geometryCollection = new GeometryCollection();

            uint geometryCount = wkbReader.ReadUInt32();

            for (int i = 0; i < geometryCount; i++)
                geometryCollection.Geometries.Add(Read());

            return geometryCollection;
        }
        */
    }
}