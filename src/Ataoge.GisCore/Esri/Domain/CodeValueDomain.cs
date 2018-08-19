using System.Collections.Generic;

namespace Ataoge.GisCore
{
    public class CodeValue
    {
        public string Name {get; set;}

        public string Code {get; set;}
    }

    public class CodeValueDomain : Domain
    {
        public CodeValueDomain()
        {
            Type = DomainType.codedValue;
        }

        public string Name {get; set;}

        public IEnumerable<CodeValue> CodeValues {get; set;}
    }
}