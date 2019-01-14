namespace Ataoge.GisCore.Utilities
{
    public class TMInfo
    {
        public double CentralMeridian {get; set;}
        public double FalseEasting {get; set;} = 500000.0;
        public double FalseNorthing {get; set;} = 0.0;
        public double ScaleFactor {get; set;} = 1.0;
        public double LatitudeOfOrigin {get; set;} = 0.0;
    }
}