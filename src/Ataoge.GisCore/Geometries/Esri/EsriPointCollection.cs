using System.Text;
using Ataoge.GisCore.Geometry.Converters;
using Newtonsoft.Json;

namespace Ataoge.GisCore.Geometry
{
    [JsonConverter(typeof(EsriPointCollectionConverter))]
    public class EsriPointCollection : ObservableCollection<EsriPoint>
    {
        public override bool IsReadOnly { get { return true; } }

        public override string ToString()
        {
            int num = this.Count;
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < num; i++)
            {
                sb.Append(this[i].X + " " + this[i].Y);
                sb.Append(",");
            }
            if (sb.Length > 0)
                sb.Remove(sb.Length - 1, 1);
            return sb.ToString();
        }

        public void Concat(EsriPointCollection pc)
        {
            if (pc != null)
            {
                int num = pc.Count;
                for (int i = 0; i < num; i++)
                    this.Add(pc[i]);
            }
        }


    }
}