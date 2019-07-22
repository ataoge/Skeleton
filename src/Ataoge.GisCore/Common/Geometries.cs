using System.Linq;

namespace Ataoge.GisCore
{
    public class PixelPoint
    {
        public int X {get; set;}
        public int Y {get; set;}
    }

    public class TileSize
    {
        public int Width {get; set;}

        public int Height {get; set;}
    }

   

    public class TileExtent
    {
        public int MinRow {get; set;}

        public int MaxRow {get; set;}

        public int MinCol {get; set;}

        public int MaxCol {get; set;}
    }

    public class MapPoint
    {
        public double X {get; set;}
        public double Y {get; set;}
    }

    public class MapExtent
    {
        public MapExtent()
        {

        }
        public double MinX {get; set;}

        public double MaxX {get; set;}

        public double MinY {get; set;}

        public double MaxY {get; set;}

        public static MapExtent FromString(string bbox, string format = null)
        {
            var bb = bbox.Split(',').Select(s => double.Parse(s)).ToArray();
            return new MapExtent() { MinX = bb[0], MinY = bb[1], MaxX = bb[2], MaxY = bb[3] };
        }

        public override string ToString()
        {
            return $"MapExtext: {this.MinX},{this.MinY},{this.MaxX},{this.MaxY}";
        }

        public string ToString(string format)
        {
            switch (format)
            {
                case "b":
                case "bbox":
                   return $"{this.MinX},{this.MinY},{this.MaxX},{this.MaxY}";
                default:
                    return ToString();
            }
           
        }
    }
}