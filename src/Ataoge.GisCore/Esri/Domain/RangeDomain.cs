namespace Ataoge.GisCore
{
    public class RangeDomain :Domain
    {
        public RangeDomain()
        {
            Type = DomainType.range;
        }

        public string Name  {get; set;}

        public int[] Range {get; set;}
    }
}