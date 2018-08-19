namespace Ataoge.GisCore
{
    public class SimpleRenderer : Renderer
    {
        public SimpleRenderer()
        {
            Type = RendererType.simple;
        }

        public Symbol Symbole {get; set;}

        public Label Label {get; set;}

        public string Description {get; set;}
    }
}