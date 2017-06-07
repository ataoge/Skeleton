namespace Ataoge.Data
{
    public interface IHasUrl
    {
        string Url {get; set;}
    }

    public interface IHasUrlForPlat : IHasUrl
    {
        int PlatSupport {get; set;}
    }
}