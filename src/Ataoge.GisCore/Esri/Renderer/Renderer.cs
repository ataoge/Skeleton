namespace Ataoge.GisCore
{
    public enum RendererType
    {
        simple,
        uniqueValue,
        classBreaks
    }

    public abstract class Renderer
    {
        public RendererType Type {get; set;}
    }


}