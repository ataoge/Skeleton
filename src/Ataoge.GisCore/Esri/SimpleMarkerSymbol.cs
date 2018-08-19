namespace Ataoge.GisCore
{
    public enum SMSStyle
    {
        esriSMSCircle,
        esriSMSCross,
        esriSMSDiamond,
        esriSMSSquare,
        esriSMSX
    }

    public class SimpleMarkerSymbol : Symbol
    {
        public SimpleMarkerSymbol()
        {
            Type = SymbolType.esriSMS;
        }

        public SMSStyle Style {get;set;} = SMSStyle.esriSMSCircle;

        public EsriColor Color {get; set;}

        public int Size {get; set;}

        public int Angle {get; set;}

        public int Xoffset {get; set;}

        public int Yoffset {get; set;}

        public OutLine Outline {get; set;}
    }
}