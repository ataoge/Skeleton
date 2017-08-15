namespace Ataoge.GisCore.Geometry
{
    internal class EwktWriter : WktWriter
    {
        internal override string Write(Geometry geometry)
        {
            return string.Concat("SRID=", geometry.Srid, ";", base.Write(geometry));
        }

        protected override void WriteWktType(GeometryType geometryType, Dimension dimension, bool isEmpty)
        {
            wktBuilder.Append(geometryType.ToString().ToUpperInvariant());

            if (dimension == Dimension.Xym)
                wktBuilder.Append("M");

            if (isEmpty)
                wktBuilder.Append(" EMPTY");
        }
    }
}