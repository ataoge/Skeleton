namespace Ataoge.GisCore
{
    public class EsriError
    {
        public int Code {get; set;}

        public string Description {get; set;}
    }
    public class FeatureEditResponse
    {
        public int ObjectId {get; set;}

        public string GlobalId {get; set;}

        public bool Success {get; set;} = true;

        public EsriError Error {get; set;}
    }
}