namespace Ataoge.Services
{
    public interface IQueryParamsAccessor
    {
        int GetIntParam(string name, int defaultValue = 0);

        string GetStringParam(string name, string defaultValue = null);

        string GetClientIpAddress();
        //int GetPageIndex(int defaultValue = 0);

        //int GetPageSize(int defaultValue = 10);

    }
}