namespace Ataoge.GisCore
{
    public enum SLSStyle
    {
        esriSLSDash,
        esriSLSDashDotDot,
        esriSLSDot,
        esriSLSNull,
        esriSLSSolid
    }

    public class OutLine
    {
        public EsriColor Color {get; set;}

        public int Width {get; set;}
    }

    public class SimpleLineSymbol : Symbol
    {
        public SimpleLineSymbol()
        {
            Type = SymbolType.esriSLS;
        }

        public SLSStyle Style {get; set;} = SLSStyle.esriSLSSolid;

        public EsriColor Color {get; set;}

        public int Width {get; set;}
    }
}