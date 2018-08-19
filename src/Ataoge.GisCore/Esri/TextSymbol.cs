namespace Ataoge.GisCore
{
    public enum FontStyle
    {
        italic,
        normal,
        oblique
    }

    public enum FontWeight
    {
        bold,
        bolder,
        lighter,
        normal
    }

    public enum FontDecoration
    {
        linethrough,
        underline,
        none
    }
    public class EsriFont
    {
        public string FontFamily {get; set;}

        public int Size {get; set;}

        public FontStyle Style {get; set;} = FontStyle.normal;

        public FontWeight Weight {get; set;} = FontWeight.normal;

        public FontDecoration Decoration {get; set;} = FontDecoration.none;
    }

    public enum VerticalAlignments
    {
        baseline,
        top,
        middle,
        bottom
    }

    public enum HorizontalAlignments
    {
        left,
        right,
        center
    }
    public class TextSymbol :Symbol
    {
        public TextSymbol()
        {
            Type = SymbolType.esriTS;
        }

        public EsriColor Color {get; set;}

        public EsriColor BackgroundColor {get; set;}

        public EsriColor BorderLineColor {get; set;}

        public VerticalAlignments VerticalAlignment {get; set;}

        public HorizontalAlignments HorizontalAlignment {get; set;}

        public bool RightToLeft {get; set;} = false;
        public int Angle {get; set;}

        public int Xoffset {get; set;}

        public int Yoffset {get; set;}

        public bool Kerning {get; set;}

        public EsriFont Font {get; set;}
    }
}