using System.Collections.Generic;

namespace Ataoge.GisCore
{
    public class ClassBreakInfo
    {
        public double ClassMaxValue {get; set;}

        public Label Label {get; set;}

        public string Description {get; set;}

        public Symbol Symbol {get; set;}
    }

    public class ClassBreaksRenderer : Renderer
    {
        public ClassBreaksRenderer()
        {
            Type = RendererType.classBreaks;
        }

        public string Field {get; set;}

        public double MinValue {get; set;}

        public IEnumerable<ClassBreakInfo> ClassBreakInfos {get; set;}
    }
}