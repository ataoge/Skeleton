namespace Ataoge.GisCore.Geometry
{
    public class EsriPolyline : EsriGeometry
    {
        private ObservableCollection<EsriPointCollection> ptc = new ObservableCollection<EsriPointCollection>();

        public EsriPolyline()
        {

        }

        public ObservableCollection<EsriPointCollection> Paths
        {
            get { return ptc; }
            set { ptc = value; }
        }

        public override string ToString()
        {
            return base.ToString();// GeoToWKt.Write(this);
        }
    }
}