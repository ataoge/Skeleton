namespace Ataoge.GisCore.Geometry
{
    public class EsriMultiPoint : EsriGeometry
    {
        private EsriPointCollection ptc = new EsriPointCollection();

        public EsriMultiPoint()
        {

        }

        public EsriMultiPoint(EsriPointCollection points)
        {
            Points = points;
        }

        public EsriMultiPoint(EsriPointCollection points, SpatialReference sref)
        {
            Points = points;
            this.SpatialReference = sref;
        }

        public EsriPointCollection Points
        {
            get
            {
                return ptc;
            }
            set
            {
                ptc = value;
            }
        }

        public EsriMultiPoint Clone()
        {
            EsriMultiPoint mp = new EsriMultiPoint();
            if (this.SpatialReference != null)
                mp.SpatialReference = this.SpatialReference;
            foreach (EsriPoint mp1 in Points)
                mp.Points.Add(new EsriPoint(mp1.X.Value, mp1.Y.Value));
            return mp;
        }
    }
}