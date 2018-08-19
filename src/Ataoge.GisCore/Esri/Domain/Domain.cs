namespace Ataoge.GisCore
{
    public enum DomainType
    {
        range,
        codedValue,
        inherited
    }

    public abstract class Domain
    {
        public DomainType Type {get; set;}
    }

    
}