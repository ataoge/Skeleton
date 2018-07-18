namespace Ataoge.GisCore.Geometry
{
    public class EsriPolygon : EsriGeometry
    {
        private ObservableCollection<EsriPointCollection> ptc = new ObservableCollection<EsriPointCollection>();

        public ObservableCollection<EsriPointCollection> Rings
        {
            get { return ptc; }
            set { ptc = value; }
        }

        public override string ToString()
        {
            return base.ToString();//GeoToWKt.Write(this);
        }

    }
}