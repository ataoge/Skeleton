using Ataoge.Data;
using Microsoft.Extensions.Logging;

namespace Ataoge.Services
{
    public interface IServiceContext
    {
        
        IPageInfo PageInfo { get;}

        ILogger Logger {get;}

        string BrowserInfo { get;}

        string ClientIpAddress { get;}
        
        string GetStringParam(string name, string defaultValue = null);

        string[] GetStringArrayParam(string name);

        int GetIntParam(string name, int defaultValue = 0);

        int[] GetIntArrayParam(string name);

        string GetStringForm(string name, string defaultValue = null);

        int GetIntForm(string name, int defaultValue = 0);

        string[] GetStringArrayForm(string name);

        int[] GetIntArrayForm(string name);

    }
}