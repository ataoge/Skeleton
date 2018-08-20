using System.Collections.Generic;

namespace Ataoge.GisCore
{
    public class UniqueValueInfo
    {
        public string Value {get; set;}

        public Label Label {get; set;}

        public string Description {get; set;}

        public Symbol Symbol {get;set;}
    }

    public class UniqueValueRenderer : Renderer
    {
        public UniqueValueRenderer()
        {
            Type = RendererType.uniqueValue;
        }

        public string Field1 {get; set;}
        public string Field2 {get; set;}
        public string Field3 {get; set;}

        public string FieldDelimiter {get; set;}

        public Symbol DefaultSymbol {get; set;}

        public Label DefaultLabel {get; set;}

        public IEnumerable<UniqueValueInfo> UniqueValueInfos {get; set;}
    }
}