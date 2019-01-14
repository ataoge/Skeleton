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
        public double MinX {get; set;}

        public double MaxX {get; set;}

        public double MinY {get; set;}

        public double MaxY {get; set;}
    }
}