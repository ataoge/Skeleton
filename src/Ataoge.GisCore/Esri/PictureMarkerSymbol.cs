namespace Ataoge.GisCore
{
    public class PictureMarkerSymbol :Symbol
    {
        public PictureMarkerSymbol()
        {
            Type = SymbolType.esriPMS;
        }

        //relative URL
        public string Url {get; set;}
        //base64EncodedImageData
        public string ImageData {get; set;}
        public string ContentType {get; set;}

        public EsriColor Color {get; set;}

        public double Width {get; set;}

        public double Height {get;set;}

        public int Angle {get; set;} = 0;

        public int Xoffset {get; set;} = 0;

        public int Yoffset {get; set;} = 0;

    }
}