using System;
using System.Collections.Generic;
using System.IO;

namespace Ataoge.GisCore.Geometry
{
    internal class WkbWriter
    {
        protected BinaryWriter wkbWriter;

        internal byte[] Write(Geometry geometry)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                wkbWriter = new BinaryWriter(memoryStream);
                WriteInternal(geometry);

                return memoryStream.ToArray();
            }
        }

        protected virtual void WriteWkbType(GeometryType geometryType, Dimension dimension, int? srid)
        {
            uint dimensionType = 0;

            switch (dimension)
            {
                case Dimension.Xyz: dimensionType = 1000; break;
                case Dimension.Xym: dimensionType = 2000; break;
                case Dimension.Xyzm: dimensionType = 3000; break;
            }

            wkbWriter.Write(dimensionType + (uint)geometryType);
        }

        private void WriteInternal(Geometry geometry, Geometry parentGeometry = null)
        {
            wkbWriter.Write(true);

            Dimension dimension = parentGeometry != null ? parentGeometry.Dimension : geometry.Dimension;

            WriteWkbType(geometry.GeometryType, dimension, geometry.Srid);

            switch (geometry.GeometryType)
            {
                case GeometryType.Point: WritePoint(geometry as Point, dimension); break;
                case GeometryType.LineString: WriteLineString(geometry as LineString); break;
                case GeometryType.Polygon: WritePolygon(geometry as Polygon); break;
                case GeometryType.MultiPoint: WriteMultiPoint(geometry as MultiPoint); break;
                case GeometryType.MultiLineString: WriteMultiLineString(geometry as MultiLineString); break;
                case GeometryType.MultiPolygon: WriteMultiPolygon(geometry as MultiPolygon); break;
                case GeometryType.GeometryCollection: WriteGeometryCollection(geometry as GeometryCollection); break;
                default: throw new Exception();
            }
        }

        private void WritePoint(Point point, Dimension dimension)
        {
            wkbWriter.Write(point.X ?? -double.NaN);
            wkbWriter.Write(point.Y ?? -double.NaN);

            if (dimension == Dimension.Xyz || dimension == Dimension.Xyzm)
                wkbWriter.Write(point.Z ?? -double.NaN);

            if (dimension == Dimension.Xym || dimension == Dimension.Xyzm)
                wkbWriter.Write(point.M ?? -double.NaN);
        }

        private void WriteLineString(LineString lineString)
        {
            wkbWriter.Write(lineString.Points.Count);

            foreach (Point point in lineString.Points)
                WritePoint(point, lineString.Dimension);
        }

        private void WritePolygon(Polygon polygon)
        {
            if (polygon.IsEmpty)
            {
                wkbWriter.Write(0);
                return;
            }

            wkbWriter.Write(1 + polygon.InteriorRings.Count);

            wkbWriter.Write(polygon.ExteriorRing.Count);
            foreach (Point point in polygon.ExteriorRing)
                WritePoint(point, polygon.Dimension);

            foreach (List<Point> interiorRing in polygon.InteriorRings)
            {
                wkbWriter.Write(interiorRing.Count);
                foreach (Point point in interiorRing)
                    WritePoint(point, polygon.Dimension);
            }
        }

        private void WriteMultiPoint(MultiPoint multiPoint)
        {
            wkbWriter.Write(multiPoint.Points.Count);

            foreach (Point point in multiPoint.Points)
                WriteInternal(point, multiPoint);
        }

        private void WriteMultiLineString(MultiLineString multiLineString)
        {
            wkbWriter.Write(multiLineString.LineStrings.Count);

            foreach (LineString lineString in multiLineString.LineStrings)
                WriteInternal(lineString, multiLineString);
        }

        private void WriteMultiPolygon(MultiPolygon multiPolygon)
        {
            wkbWriter.Write(multiPolygon.Polygons.Count);

            foreach (Polygon polygon in multiPolygon.Polygons)
                WriteInternal(polygon, multiPolygon);
        }

        private void WriteGeometryCollection(GeometryCollection geometryCollection)
        {
            wkbWriter.Write(geometryCollection.Geometries.Count);

            foreach (Geometry geometry in geometryCollection.Geometries)
                WriteInternal(geometry);
        }
    }
}