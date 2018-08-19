namespace Ataoge.GisCore
{
    public enum SFSStyle
    {
        esriSFSBackwordDiagonal,
        esriSFSCross,
        esriSFSDiagonalCross,
        esriSFSForwardDiagonal,
        esriSFSHorizontal,
        esriSFSNull,
        esriSFSSolid,
        esriSFSVertical
    }

    public class SimpleFillSymbol :Symbol
    {
        public SimpleFillSymbol()
        {
            Type = SymbolType.esriSFS;
        }

        public SFSStyle Style {get; set;} = SFSStyle.esriSFSSolid;

        public EsriColor Color {get; set;}

        public SimpleLineSymbol Outline {get; set;}
    }
}