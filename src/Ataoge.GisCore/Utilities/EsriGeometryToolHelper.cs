using System;
using System.Globalization;
using System.IO;
using Ataoge.GisCore.Geometry;

namespace Ataoge.GisCore.Utilities
{
    public static class EsriGeometryToolHelper
    {
        public static string Write(EsriGeometry geometry)
        {
            StringWriter sw = new StringWriter();
            Write(geometry, sw);
            return sw.ToString();
        }

        public static void Write(EsriGeometry geometry, StringWriter writer)
        {
            AppendGeometryTaggedText(geometry, writer);
        }

        private static void AppendGeometryTaggedText(EsriGeometry geometry, StringWriter writer)
        {
            if (geometry == null)
                throw new NullReferenceException("Cannot write Well-Known Text: geometry was null");
            if (geometry is EsriPoint)
            {
                var point = geometry as EsriPoint;
                AppendPointTaggedText(point, writer);
            }
            else if (geometry is EsriPolyline)
            {
                var pl = (EsriPolyline)geometry;
                if (pl.Paths.Count == 1)
                {
                    AppendLineStringTaggedText(geometry as EsriPolyline, writer);
                }
                else if (pl.Paths.Count > 1)
                {
                    AppendMultiLineStringTaggedText(geometry as EsriPolyline, writer);
                }
            }
            else if (geometry is EsriPolygon)
            {
                var plo = (EsriPolygon)geometry;
                if (plo.Rings.Count == 1)
                {
                    AppendPolygonTaggedText(geometry as EsriPolygon, writer);
                }
                else if (plo.Rings.Count > 1)
                {
                    AppendMultiPolygonTaggedText(geometry as EsriPolygon, writer);
                }
            }
            else if (geometry is EsriMultiPoint)
                AppendMultiPointTaggedText(geometry as EsriMultiPoint, writer);
            else if (geometry is EsriExtent)
            {
                var ply = new EsriPolygon();
                EsriPointCollection pc = new EsriPointCollection();
                EsriExtent ev = geometry as EsriExtent;
                pc.Add(new EsriPoint(ev.XMin, ev.YMin));
                pc.Add(new EsriPoint(ev.XMax, ev.YMin));
                pc.Add(new EsriPoint(ev.XMax, ev.YMax));
                pc.Add(new EsriPoint(ev.XMin, ev.YMax));
                pc.Add(new EsriPoint(ev.XMin, ev.YMin));
                ply.Rings.Add(pc);
                AppendPolygonTaggedText(ply, writer);
            }

            else
                throw new NotSupportedException("Unsupported Geometry implementation:" + geometry.GetType().Name);
        }


        private static void AppendPointTaggedText(EsriPoint coordinate, StringWriter writer)
        {
            writer.Write("POINT ");
            AppendPointText(coordinate, writer);
        }

        private static void AppendLineStringTaggedText(EsriPolyline lineString, StringWriter writer)
        {
            writer.Write("LINESTRING ");
            AppendLineStringText(lineString.Paths[0], writer);
        }

        private static void AppendPolygonTaggedText(EsriPolygon polygon, StringWriter writer)
        {
            writer.Write("Polygon");
            AppendPolygonText(polygon, writer);
        }

        private static void AppendMultiPointTaggedText(EsriMultiPoint multipoint, StringWriter writer)
        {
            writer.Write("MultiPoint ");
            AppendMultiPointText(multipoint, writer);
        }

        private static void AppendMultiLineStringTaggedText(EsriPolyline multiLineString, StringWriter writer)
        {
            writer.Write("MultiLineString");
            AppendMultiLineStringText(multiLineString, writer);
        }

        private static void AppendMultiPolygonTaggedText(EsriPolygon multiPolygon, StringWriter writer)
        {
            writer.Write("MultiPolygon");
            AppendMultiPolygonText(multiPolygon, writer);
        }

        private static void AppendPointText(EsriPoint coordinate, StringWriter writer)
        {
            if (coordinate == null)
                writer.Write("Empty");
            else
            {
                writer.Write("(");
                AppendCoordinate(coordinate, writer);
                writer.Write(")");
            }
        }

        private static void AppendCoordinate(EsriPoint coordinate, StringWriter writer)
        {
            writer.Write(coordinate.X + " " + coordinate.Y);
        }

        private static string WriteNumber(double d)
        {
            return d.ToString(NumberFormatInfo.CurrentInfo);
        }

        private static void AppendLineStringText(EsriPointCollection lineString, StringWriter writer)
        {
            if (lineString == null)
                writer.Write("Empty");
            else
            {
                writer.Write("(");
                for (int i = 0; i < lineString.Count; i++)
                {
                    if (i > 0)
                        writer.Write(", ");
                    AppendCoordinate(lineString[i], writer);
                }
                writer.Write(")");
            }
        }

        private static void AppendPolygonText(EsriPolygon polygon, StringWriter writer)
        {
            if (polygon == null)
                writer.Write("Empty");
            else
            {
                writer.Write("(");
                AppendLineStringText(polygon.Rings[0], writer);
                for (int i = 1; i < polygon.Rings.Count; i++)
                {
                    writer.Write(", ");
                    AppendLineStringText(polygon.Rings[i], writer);
                }
                writer.Write(")");
            }
        }

        private static void AppendMultiPointText(EsriMultiPoint multiPoint, StringWriter writer)
        {
            if (multiPoint == null)
                writer.Write("Empty");
            else
            {
                writer.Write("(");
                for (int i = 0; i < multiPoint.Points.Count; i++)
                {
                    if (i > 0)
                        writer.Write(", ");
                    AppendCoordinate(multiPoint.Points[i], writer);
                }
                writer.Write(")");
            }
        }

        private static void AppendMultiLineStringText(EsriPolyline multiLineString, StringWriter writer)
        {
            if (multiLineString == null)
                writer.Write("Empty");
            else
            {
                writer.Write("(");
                for (int i = 0; i < multiLineString.Paths.Count; i++)
                {
                    if (i > 0)
                        writer.Write(", ");
                    AppendLineStringText(multiLineString.Paths[i], writer);
                }
                writer.Write(")");
            }
        }

        private static void AppendMultiPolygonText(EsriPolygon multiPolygon, StringWriter writer)
        {
            if (multiPolygon == null)
                writer.Write("Empty");
            else
            {
                writer.Write("(");
                for (int i = 0; i < multiPolygon.Rings.Count; i++)
                {
                    if (i > 0)
                        writer.Write(", ");
                    EsriPolygon pol = new EsriPolygon();
                    pol.Rings.Add(multiPolygon.Rings[i]);
                    AppendPolygonText(pol, writer);
                }
                writer.Write(")");
            }
        }

        public static EsriGeometry ConvertToEsriGeometry(byte[] ewkb, int? outSrid = null)
        {
            if (ewkb == null)
                return null;
            
            using (MemoryStream stream = new MemoryStream(ewkb))
            {
                if (outSrid== null)
                {
                    var geometry = new EsriEwkbReader(stream).Read();
                    return geometry;
                }
                else
                {
                    var geometry = new EsriEwkbReader(stream, outSrid.Value).Read();
                    return geometry;
                }
            }
 
        }
    }
}