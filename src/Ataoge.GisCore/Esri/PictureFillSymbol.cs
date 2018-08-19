namespace Ataoge.GisCore
{
    public class PictureFillSymbol : Symbol
    {
        public PictureFillSymbol()
        {
            Type = SymbolType.esriPFS;
        }

         public string Url {get; set;}
        //base64EncodedImageData
        public string ImageData {get; set;}
        public string ContentType {get; set;}

        public EsriColor Color {get; set;}

        public SimpleLineSymbol Outline {get; set;}

        public double Width {get; set;}

        public double Height {get;set;}

        public int Angle {get; set;} = 0;

        public int Xoffset {get; set;} = 0;

        public int Yoffset {get; set;} = 0;

        public int Xscale {get; set;} = 1;

        public int Yscale {get; set;} = 1;

    }
}